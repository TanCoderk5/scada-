
using Modbus.Data;
using Modbus.Device;
using System.Net;
using System.Net.Sockets;

namespace Modbus_Slave
{
    public partial class FrmTCP : Form
    {
        private TcpListener _listener;
        private ModbusSlave _slave;                // dùng class cụ thể
        private CancellationTokenSource _cts;
        private System.Windows.Forms.Timer _timer;
        private DataStore _Store = DataStoreFactory.CreateDefaultDataStore();
        public FrmTCP()
        {
            InitializeComponent();
            btnStop.Click += (s, e) => StopServer();
        }

        private void FrmTCP_Load(object sender, EventArgs e)
        {
            txtIP.Text = GetIPAddress();

        }
        private void StartServer(int port, byte unitId)
        {
            try
            {
                IPAddress IP = IPAddress.Parse(txtIP.Text);
                // 1) TCP listener
                _listener = new TcpListener(IP, port);
                _listener.Start();
                rtxLog.AppendText("\r\nDevice listen on " + IP.ToString() + ":" + port);

                // 2) DataStore mặc định + seed vài giá trị test

                // store.HoldingRegisters[1] = 1234;
                _Store.CoilDiscretes[1] = true;

                // 3) Tạo Modbus TCP slave trực tiếp từ listener
                _slave = ModbusTcpSlave.CreateTcp(unitId, _listener);
                _slave.DataStore = _Store;

                // 4) Lắng nghe nền (async) để không khóa UI
                _cts = new CancellationTokenSource();
                _ = Task.Run(() => _slave.Listen());

                _timer = new System.Windows.Forms.Timer { Interval = 1000 };
                _timer.Tick += (s, e) =>
                {
                    // cập nhật “nguồn dữ liệu thật” vào DataStore
                    // ví dụ: đảo coil 1, tăng Input/ Holding để client đọc thấy thay đổi
                    ushort[] zData = new ushort[2];
                    zData[0] = _Store.HoldingRegisters[3000];
                    zData[1] = _Store.HoldingRegisters[3001];
                    HR_0.Text = ConvertData.ToFloat32_BE(zData).ToString();

                    zData[0] = _Store.HoldingRegisters[3002];
                    zData[1] = _Store.HoldingRegisters[3003];
                    HR_1.Text = ConvertData.ToFloat32_BE(zData).ToString();

                    zData[0] = _Store.HoldingRegisters[3004];
                    zData[1] = _Store.HoldingRegisters[3005];
                    HR_2.Text = ConvertData.ToFloat32_BE(zData).ToString();

                    zData[0] = _Store.HoldingRegisters[3020];
                    zData[1] = _Store.HoldingRegisters[3021];
                    HR_3.Text = ConvertData.ToFloat32_BE(zData).ToString();

                    HR_10.Text = _Store.HoldingRegisters[11].ToString();
                    HR.Text = "";
                    for (int i = 3000; i < 3010; i++)
                    {
                        HR.Text += _Store.HoldingRegisters[i].ToString() + ":";
                    }
                    //HR.Text = _Store.HoldingRegisters[2].ToString() + ":" + _Store.HoldingRegisters[4].ToString() + ":";//
                    //HR.Text += _Store.HoldingRegisters[5].ToString() + ":" + _Store.HoldingRegisters[6].ToString();
                    /* string zArrayValue = "";
                     for (int i = 0; i < 12; i++)
                     {
                         zArrayValue += store.HoldingRegisters[i].ToString() + "-";
                     }
                     HR.Text = zArrayValue;*/
                };
                _timer.Start();

                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            catch (Exception ex)
            {
                rtxLog.AppendText("\r\nStart error: " + ex.Message);
            }
        }
        private string GetIPAddress()
        {
            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }
        private void StopServer()
        {
            try { _timer?.Stop(); } catch { }
            try { _cts?.Cancel(); } catch { }
            try
            {
                _listener?.Stop();
            }
            catch
            {
                MessageBox.Show("");
            }

            _timer = null;
            _cts = null;
            _slave = null;
            _listener = null;

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            rtxLog.AppendText("\r\nDevice stoped listen");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopServer();
            base.OnFormClosing(e);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            int zPort = int.Parse(txtPort.Text);
            StartServer(zPort, 1);
        }

        private Random _Rand = new Random();
        private void btnData_Click(object sender, EventArgs e)
        {
            for (int i = 2; i < 9000; i = i + 2)
            {
                _Store.HoldingRegisters[i] =  (ushort)_Rand.Next(1,1);
                _Store.HoldingRegisters[i + 1] = (ushort)_Rand.Next(10, 20);
            }
            ushort zIndex = 1;
            //for (int i = 3000; i < 3100; i++)
            //{
            //    _Store.HoldingRegisters[i] = zIndex;
            //    zIndex++;
            //}
            ushort[] zValue;
            for(int i = 3000;i<3010;i=i+2)
            {
                zValue = ConvertData.FromFloat32_BE(_Rand.Next(1,10));
                _Store.HoldingRegisters[i] = zValue[0];
                _Store.HoldingRegisters[i+1] = zValue[1];
            }
            for (int i = 3020; i < 3030; i = i + 2)
            {
                zValue = ConvertData.FromFloat32_BE(_Rand.Next(180, 240));
                _Store.HoldingRegisters[i] = zValue[0];
                _Store.HoldingRegisters[i + 1] = zValue[1];
            }


        }

        private void TimerAutoData_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i < 9000; i = i + 2)
            {
                _Store.HoldingRegisters[i] = 0;// (ushort)_Rand.Next(1, 5);
                _Store.HoldingRegisters[i + 1] = (ushort)_Rand.Next(100, 1000);
            }
            ushort[] zValue;
            for (int i = 3000; i < 3010; i = i + 2)
            {
                zValue = ConvertData.FromFloat32_BE(_Rand.Next(1, 10));
                _Store.HoldingRegisters[i] = zValue[0];
                _Store.HoldingRegisters[i + 1] = zValue[1];
            }
            for (int i = 3020; i < 3030; i = i + 2)
            {
                zValue = ConvertData.FromFloat32_BE(_Rand.Next(180, 240));
                _Store.HoldingRegisters[i] = zValue[0];
                _Store.HoldingRegisters[i + 1] = zValue[1];
            }

        }

        private void btn_AutoData_Click(object sender, EventArgs e)
        {
            if (btn_AutoData.Text == "Stop")
            {
                TimerAutoData.Stop();
                btn_AutoData.Text = "Auto Data";
            }
            else
            {
                TimerAutoData.Start();
                btn_AutoData.Text = "Stop";
            }
        }
    }
}
