using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Modbus_Slave
{
    public class ModbusTcpServer : IDisposable
    {
        private TcpListener? _listener;
        private readonly List<DeviceModbus> _devices;
        private readonly List<TcpClient> _clients = new List<TcpClient>();
        private CancellationTokenSource? _cts;
        private bool _isRunning;

        public event Action<string>? OnLog;
        public bool IsRunning => _isRunning;
        public int ClientCount => _clients.Count;

        public ModbusTcpServer(List<DeviceModbus> devices)
        {
            _devices = devices;
        }

        public async Task StartAsync(IPAddress ipAddress, int port, CancellationToken cancellationToken = default)
        {
            if (_isRunning)
            {
                Log("TCP Server already running");
                return;
            }

            try
            {
                _listener = new TcpListener(ipAddress, port);

                // ⭐ FIX: Enable ReuseAddress
                _listener.Server.SetSocketOption(
                    SocketOptionLevel.Socket,
                    SocketOptionName.ReuseAddress,
                    true
                );

                try
                {
                    _listener.Server.SetSocketOption(
                        SocketOptionLevel.Socket,
                        SocketOptionName.DontLinger,
                        true
                    );
                }
                catch { }

                _listener.Start();
                _isRunning = true;

                _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

                Log($"✅ TCP Server started on {ipAddress}:{port}");

                _ = Task.Run(() => AcceptClientsAsync(_cts.Token), _cts.Token);
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
            {
                Log($"❌ Port {port} is already in use");
                throw new InvalidOperationException($"Port {port} is already in use", ex);
            }
            catch (Exception ex)
            {
                Log($"❌ TCP Server start error: {ex.Message}");
                throw;
            }
        }

        public void Stop()
        {
            if (!_isRunning) return;

            try
            {
                _isRunning = false;
                _cts?.Cancel();

                lock (_clients)
                {
                    foreach (var client in _clients.ToList())
                    {
                        try
                        {
                            if (client.Connected)
                            {
                                client.Client.Shutdown(SocketShutdown.Both);
                            }
                            client?.Close();
                        }
                        catch { }
                    }
                    _clients.Clear();
                }

                _listener?.Stop();
                Log("🛑 TCP Server stopped");
            }
            catch (Exception ex)
            {
                Log($"Error stopping TCP server: {ex.Message}");
            }
        }

        private async Task AcceptClientsAsync(CancellationToken ct)
        {
            if (_listener == null) return;

            while (!ct.IsCancellationRequested && _isRunning)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();

                    client.NoDelay = true;
                    client.ReceiveTimeout = 30000;
                    client.SendTimeout = 30000;

                    var endpoint = client.Client.RemoteEndPoint?.ToString() ?? "Unknown";

                    lock (_clients)
                    {
                        _clients.Add(client);
                    }

                    Log($"📥 Client connected: {endpoint} (Total: {_clients.Count})");

                    _ = Task.Run(() => HandleClientAsync(client, ct), ct);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex) when (!ct.IsCancellationRequested)
                {
                    Log($"Accept client error: {ex.Message}");
                    await Task.Delay(1000, ct);
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken ct)
        {
            var endpoint = client.Client.RemoteEndPoint?.ToString() ?? "Unknown";
            NetworkStream? stream = null;

            try
            {
                stream = client.GetStream();
                byte[] buffer = new byte[260];

                while (!ct.IsCancellationRequested && client.Connected)
                {
                    if (!stream.DataAvailable)
                    {
                        await Task.Delay(10, ct);
                        continue;
                    }

                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, ct);

                    if (bytesRead == 0)
                    {
                        Log($"📤 Client disconnected: {endpoint}");
                        break;
                    }

                    byte[] frame = new byte[bytesRead];
                    Array.Copy(buffer, frame, bytesRead);

                    byte[]? response = ProcessTcpFrame(frame);

                    if (response != null && response.Length > 0)
                    {
                        await stream.WriteAsync(response, 0, response.Length, ct);
                        await stream.FlushAsync(ct);
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) when (!ct.IsCancellationRequested)
            {
                Log($"Client {endpoint} error: {ex.Message}");
            }
            finally
            {
                lock (_clients)
                {
                    _clients.Remove(client);
                }

                try
                {
                    if (client.Connected)
                    {
                        client.Client.Shutdown(SocketShutdown.Both);
                    }
                }
                catch { }

                try { stream?.Dispose(); } catch { }
                try { client?.Close(); } catch { }

                Log($"📤 Client disconnected: {endpoint} (Remaining: {_clients.Count})");
            }
        }

        private byte[]? ProcessTcpFrame(byte[] frame)
        {
            try
            {
                if (frame.Length < 8)
                {
                    Log("Invalid frame: too short");
                    return BuildErrorResponse(frame, 0x00, 0x04);
                }

                ushort transactionId = (ushort)((frame[0] << 8) | frame[1]);
                ushort protocolId = (ushort)((frame[2] << 8) | frame[3]);
                byte unitId = frame[6];

                if (protocolId != 0)
                {
                    Log($"Invalid protocol ID: {protocolId}");
                    return null;
                }

                int pduLength = frame.Length - 7;
                byte[] pdu = new byte[pduLength];
                Array.Copy(frame, 7, pdu, 0, pduLength);

                DeviceModbus? device;
                lock (_devices)
                {
                    device = _devices.FirstOrDefault(d => d.SlaveId == unitId);
                }

                if (device == null)
                {
                    Log($"Device not found: Unit ID={unitId}");
                    return BuildErrorResponse(frame, pdu[0], 0x0B);
                }

                byte[]? responsePdu = device.HandleRequest(pdu);

                if (responsePdu == null || responsePdu.Length == 0)
                {
                    return null;
                }

                return BuildTcpResponse(transactionId, unitId, responsePdu);
            }
            catch (Exception ex)
            {
                Log($"Frame processing error: {ex.Message}");
                return null;
            }
        }

        private byte[] BuildTcpResponse(ushort transactionId, byte unitId, byte[] pdu)
        {
            byte[] response = new byte[7 + pdu.Length];

            response[0] = (byte)(transactionId >> 8);
            response[1] = (byte)(transactionId & 0xFF);
            response[2] = 0x00;
            response[3] = 0x00;

            ushort length = (ushort)(1 + pdu.Length);
            response[4] = (byte)(length >> 8);
            response[5] = (byte)(length & 0xFF);
            response[6] = unitId;

            Array.Copy(pdu, 0, response, 7, pdu.Length);

            return response;
        }

        private byte[]? BuildErrorResponse(byte[] frame, byte functionCode, byte exceptionCode)
        {
            if (frame.Length < 7) return null;

            ushort transactionId = (ushort)((frame[0] << 8) | frame[1]);
            byte unitId = frame[6];

            byte[] pdu = new byte[] { (byte)(functionCode | 0x80), exceptionCode };

            return BuildTcpResponse(transactionId, unitId, pdu);
        }

        private void Log(string message)
        {
            OnLog?.Invoke($"[TCP] {message}");
        }

        public void Dispose()
        {
            Stop();
            Thread.Sleep(100);
            _cts?.Dispose();
            _cts = null;
            _listener = null;
        }
    }
}
