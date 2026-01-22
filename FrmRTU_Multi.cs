using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modbus_Slave
{
    public partial class FrmRTU_Multi : Form
    {
        private ModbusTcpServer? _tcpServer;
        private CancellationTokenSource? _tcpCancellationSource;
        private readonly List<DeviceModbus> _Devices = new List<DeviceModbus>();

        public FrmRTU_Multi()
        {
            InitializeComponent();
        }

        private void FrmRTU_Multi_Load(object sender, EventArgs e)
        {
            TCP_LoadNetworkInfo();
            Message_Log("=== MODBUS TCP SIMULATOR ===", Color.Cyan);
            Message_Log("📡 Ready to simulate Modbus TCP devices", Color.Cyan);
        }

        private void Message_Log(string Message, Color? ColorView = null)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { AppendLog(Message, ColorView); }));
            }
            else
            {
                AppendLog(Message, ColorView);
            }
        }

        private void AppendLog(string Message, Color? ColorView)
        {
            Color colorToUse = ColorView ?? rtxLog.ForeColor;
            rtxLog.SelectionStart = rtxLog.TextLength;
            rtxLog.SelectionColor = colorToUse;
            rtxLog.AppendText("\r\n" + DateTime.Now.ToString("HH:mm:ss.fff") + " : " + Message);
            rtxLog.SelectionStart = rtxLog.Text.Length;
            rtxLog.ScrollToCaret();
        }

        public void DeviceAdd(DeviceModbus NewDevice)
        {
            lock (_Devices)
            {
                _Devices.Add(NewDevice);
            }
            Message_Log($"✅ Device added: Slave ID={NewDevice.SlaveId}", Color.Green);
            UpdateDeviceCount();
        }

        public void DeviceRemove(int SlaveId)
        {
            lock (_Devices)
            {
                for (int i = 0; i < _Devices.Count; i++)
                {
                    if (_Devices[i].SlaveId == SlaveId)
                    {
                        _Devices.RemoveAt(i);
                        Message_Log($"❌ Device removed: Slave ID={SlaveId}", Color.Orange);
                        UpdateDeviceCount();
                        break;
                    }
                }
            }
        }

        private void UpdateDeviceCount()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateDeviceCount));
                return;
            }

            lblDeviceCount.Text = $"Devices: {_Devices.Count}";
            lblTcpClients.Text = _tcpServer?.IsRunning == true
                ? $"TCP: {_tcpServer.ClientCount} client(s)"
                : "TCP: Stopped";
        }

        private void TCP_LoadNetworkInfo()
        {
            cboTcpIp.Items.Clear();
            cboTcpIp.Items.Add("0.0.0.0");
            cboTcpIp.Items.Add("127.0.0.1");

            try
            {
                var hostName = Dns.GetHostName();
                var addresses = Dns.GetHostAddresses(hostName);

                foreach (var addr in addresses)
                {
                    if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        cboTcpIp.Items.Add(addr.ToString());
                    }
                }
            }
            catch { }

            if (cboTcpIp.Items.Count > 0)
                cboTcpIp.SelectedIndex = 0;

            txtTcpPort.Text = "502";
        }

        private async void btnStartTCP_Click(object sender, EventArgs e)
        {
            if (btnStartTCP.Text == "Start TCP")
            {
                try
                {
                    string ipStr = cboTcpIp.Text;
                    if (!int.TryParse(txtTcpPort.Text, out int port))
                    {
                        MessageBox.Show("Invalid port number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (port < 1 || port > 65535)
                    {
                        MessageBox.Show("Port must be between 1 and 65535", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    IPAddress ipAddress = IPAddress.Parse(ipStr);

                    _tcpCancellationSource = new CancellationTokenSource();
                    _tcpServer = new ModbusTcpServer(_Devices);

                    _tcpServer.OnLog += (msg) => Message_Log(msg, Color.DarkCyan);

                    await _tcpServer.StartAsync(ipAddress, port, _tcpCancellationSource.Token);

                    btnStartTCP.Text = "Stop TCP";
                    btnStartTCP.BackColor = Color.LightGreen;

                    cboTcpIp.Enabled = false;
                    txtTcpPort.Enabled = false;

                    UpdateDeviceCount();
                    timerTcpStatus.Start();

                    Message_Log("✅ TCP Server started successfully", Color.Green);
                }
                catch (InvalidOperationException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
                {
                    MessageBox.Show(
                        $"Port {txtTcpPort.Text} is already in use.\nWait a few seconds and try again.",
                        "Port In Use",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"TCP Start Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                StopTCP();
            }
        }

        private void StopTCP()
        {
            try
            {
                timerTcpStatus.Stop();
                _tcpCancellationSource?.Cancel();
                Task.Delay(200).Wait();
                _tcpServer?.Stop();
                _tcpServer?.Dispose();
                _tcpServer = null;
                _tcpCancellationSource?.Dispose();
                _tcpCancellationSource = null;

                btnStartTCP.Text = "Start TCP";
                btnStartTCP.BackColor = SystemColors.Control;

                cboTcpIp.Enabled = true;
                txtTcpPort.Enabled = true;

                UpdateDeviceCount();
                Message_Log("🛑 TCP Server stopped", Color.Gray);
            }
            catch (Exception ex)
            {
                Message_Log($"TCP Stop Error: {ex.Message}", Color.Red);
            }
        }

        private void timerTcpStatus_Tick(object sender, EventArgs e)
        {
            UpdateDeviceCount();
        }

        private void btn_AddDevice_Click(object sender, EventArgs e)
        {
            int addr = GetNextAvailableAddress();
            FrmRTU_Client frm = new FrmRTU_Client(this) { AddressDefault = addr, MdiParent = this };
            frm.Show();
            Message_Log($"📱 Generic device - Slave ID: {addr}", Color.Blue);
        }

        private void btn_AddTemBreakPro_Click(object sender, EventArgs e)
        {
            int addr = GetNextAvailableAddress();
            FrmTemBreakPro frm = new FrmTemBreakPro(this) { AddressDefault = addr, MdiParent = this };
            frm.Show();
            Message_Log($"📱 TemBreakPro - Slave ID: {addr}", Color.Blue);
        }

        private void btn_Add7KT0310_Click(object sender, EventArgs e)
        {
            int addr = GetNextAvailableAddress();
            Frm7KT0310 frm = new Frm7KT0310(this) { AddressDefault = addr, MdiParent = this };
            frm.Show();
            Message_Log($"📱 7KT0310 - Slave ID: {addr}", Color.Blue);
        }

        private void btn_AddMTM5M_Click(object sender, EventArgs e)
        {
            int addr = GetNextAvailableAddress();
            FrmMTM5M frm = new FrmMTM5M(this) { AddressDefault = addr, MdiParent = this };
            frm.Show();
            Message_Log($"📱 MTM5M - Slave ID: {addr}", Color.Blue);
        }

        private int GetNextAvailableAddress()
        {
            lock (_Devices)
            {
                if (_Devices.Count == 0) return 1;

                bool[] used = new bool[248];
                foreach (var dev in _Devices)
                {
                    if (dev.SlaveId < 248)
                        used[dev.SlaveId] = true;
                }

                for (int i = 1; i < 248; i++)
                {
                    if (!used[i]) return i;
                }

                return _Devices[_Devices.Count - 1].SlaveId + 1;
            }
        }

        private void FrmRTU_Multi_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_tcpServer?.IsRunning == true)
            {
                StopTCP();
                Task.Delay(300).Wait();
            }
        }
    }
}