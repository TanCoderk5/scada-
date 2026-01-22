using Modbus_Slave;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Timers;

namespace Modbus_Slave
{
    public class ModbusRtuServer : IDisposable
    {
        private readonly SerialPort _RTU_Port = new SerialPort();
        private readonly List<byte> _rxBuffer = new List<byte>();
        private readonly System.Timers.Timer _frameTimer;
        private const int FrameTimeoutMs = 5;

        private readonly List<DeviceModbus> _devices = new List<DeviceModbus>();

        public ModbusRtuServer(string portName, int baudRate = 9600)
        {
            _RTU_Port.PortName = portName;
            _RTU_Port.BaudRate = baudRate;
            _RTU_Port.Parity = Parity.Even;
            _RTU_Port.DataBits = 8;
            _RTU_Port.StopBits = StopBits.One;
            _RTU_Port.Handshake = Handshake.None;

            _RTU_Port.DataReceived += RTU_Port_DataReceived;

            _frameTimer = new System.Timers.Timer(FrameTimeoutMs);
            _frameTimer.AutoReset = false;
            _frameTimer.Elapsed += FrameTimer_Elapsed;
        }

        public void AddDevice(DeviceModbus device)
        {
            _devices.Add(device);
        }

        public void Open()
        {
            if (!_RTU_Port.IsOpen)
                _RTU_Port.Open();
        }

        public void Close()
        {
            if (_RTU_Port.IsOpen)
                _RTU_Port.Close();
        }

        public void Dispose()
        {
            _frameTimer?.Dispose();
            if (_RTU_Port.IsOpen) _RTU_Port.Close();
            _RTU_Port.Dispose();
        }

        private void RTU_Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int bytes = _RTU_Port.BytesToRead;
                if (bytes <= 0) return;

                byte[] buf = new byte[bytes];
                _RTU_Port.Read(buf, 0, bytes);

                lock (_rxBuffer)
                {
                    _rxBuffer.AddRange(buf);
                    _frameTimer.Stop();
                    _frameTimer.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataReceived error: " + ex.Message);
            }
        }

        private void FrameTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            byte[] frame;

            lock (_rxBuffer)
            {
                if (_rxBuffer.Count == 0) return;

                frame = _rxBuffer.ToArray();
                _rxBuffer.Clear();
            }

            ProcessFrame(frame);
        }
        private void ProcessFrame(byte[] frame)
        {
            // Format: [0]=SlaveId, [1..N-3]=PDU, [N-2..N-1]=CRC
            if (frame.Length < 4)
            {
                Console.WriteLine("Frame quá ngắn.");
                return;
            }

            ushort crcReceived = (ushort)(frame[^2] | (frame[^1] << 8));
            ushort crcCalc = ModbusCrc(frame, frame.Length - 2);

            if (crcReceived != crcCalc)
            {
                Console.WriteLine("CRC sai, bỏ qua frame.");
                return;
            }

            byte slaveId = frame[0];

            // Tìm DeviceModbus tương ứng
            DeviceModbus? dev = _devices.Find(d => d.SlaveId == slaveId);
            if (dev == null)
            {
                // Không có thiết bị nào với ID này -> im lặng
                Console.WriteLine($"Không có device SlaveId={slaveId}, bỏ qua.");
                return;
            }

            // Tách PDU (không gồm SlaveId và CRC)
            int pduLength = frame.Length - 3; // trừ 1 byte Slave, 2 byte CRC
            byte[] requestPdu = new byte[pduLength];
            Array.Copy(frame, 1, requestPdu, 0, pduLength);

            byte[] responsePdu = dev.HandleRequest(requestPdu);
            if (responsePdu == null || responsePdu.Length == 0)
                return;

            // Đóng gói lại ADU: SlaveId + PDU + CRC
            byte[] respFrame = BuildResponseFrame(slaveId, responsePdu);

            if (_RTU_Port.IsOpen)
                _RTU_Port.Write(respFrame, 0, respFrame.Length);

            Console.WriteLine($"Đã trả lời Slave={slaveId}, Func=0x{responsePdu[0]:X2}, Len={respFrame.Length}");
        }
        private byte[] BuildResponseFrame(byte slaveId, byte[] pdu)
        {
            byte[] frame = new byte[1 + pdu.Length + 2];
            frame[0] = slaveId;
            Array.Copy(pdu, 0, frame, 1, pdu.Length);

            ushort crc = ModbusCrc(frame, frame.Length - 2);
            frame[^2] = (byte)(crc & 0xFF);
            frame[^1] = (byte)(crc >> 8);

            return frame;
        }
        private static ushort ModbusCrc(byte[] data, int length)
        {
            ushort crc = 0xFFFF;

            for (int pos = 0; pos < length; pos++)
            {
                crc ^= data[pos];

                for (int i = 0; i < 8; i++)
                {
                    bool lsb = (crc & 0x0001) != 0;
                    crc >>= 1;
                    if (lsb)
                        crc ^= 0xA001;
                }
            }

            return crc;
        }
    }
}