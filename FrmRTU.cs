using Modbus.Data;
using Modbus.Device;
using Modbus.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modbus_Slave
{
    public partial class FrmRTU : Form
    {
        private ModbusSlave _slave;
        private CancellationTokenSource _cts;
        private System.Windows.Forms.Timer _timer;
        private List<ModbusSerialSlave> _slaves = new List<ModbusSerialSlave>();

        // lưu DataStore theo UnitID để dễ truy cập / hiển thị
        private Dictionary<byte, DataStore> _stores = new Dictionary<byte, DataStore>();

        // Lưu DataStore cho từng slave theo Unit ID
        public FrmRTU()
        {
            InitializeComponent();
        }
        private void FrmRTU_Load(object sender, EventArgs e)
        {
            RTU_LoadPorts();
            cboDeviceAddress.SelectedIndex = 0;
            _RTU_Port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(RTU_Port_DataReceived);
        }
        #region [ RTU Setting]
        private readonly SerialPort _RTU_Port = new SerialPort();

        private void RTU_LoadPorts()
        {
            cboPort.Items.Clear();
            foreach (string s in SerialPort.GetPortNames())
            {
                cboPort.Items.Add(s);
            }
            if (cboPort.Items.Count > 0)
            {
                cboPort.SelectedIndex = 0;
            }
            cboBaudRate.SelectedIndex = 8;
            cboDataBits.SelectedIndex = 3;
            cboParity.SelectedIndex = 0;
            cboStopBit.SelectedIndex = 1;
        }


        #endregion

        private void btnStart_Click(object sender, EventArgs e)
        {

            _RTU_Port.PortName = cboPort.Text;
            _RTU_Port.BaudRate = int.Parse(cboBaudRate.Text);
            _RTU_Port.DataBits = int.Parse(cboDataBits.Text);
            _RTU_Port.Parity = (Parity)Enum.Parse(typeof(Parity), cboParity.Text);
            _RTU_Port.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cboStopBit.Text);

            // Bluetooth SPP thường không cần flow control:
            _RTU_Port.Handshake = Handshake.None;
            _RTU_Port.ReadTimeout = 2000;  // ms
            _RTU_Port.WriteTimeout = 2000;  // ms
                                            // Một số adapter BT cần bật DTR/RTS:
            _RTU_Port.DtrEnable = true;
            _RTU_Port.RtsEnable = true;
            _RTU_Port.Open();
            rtxLog.AppendText($"\r\nRTU listen on {_RTU_Port.PortName} @ {_RTU_Port.BaudRate}bps");

            // 2) Tạo nhiều DataStore + Slave cho nhiều UnitID
            byte[] unitIds = { 1, 2, 3, 4 };   // 4 thiết bị giả lập

            _slaves.Clear();
            _stores.Clear();

            foreach (var id in unitIds)
            {
                var store = DataStoreFactory.CreateDefaultDataStore();

                // Gán giá trị thử khác nhau cho từng thiết bị
                store.HoldingRegisters[0] = (ushort)58982; // ví dụ: điện áp
                store.HoldingRegisters[1] = (ushort)17117;  // ví dụ: dòng
                store.HoldingRegisters[2] = (ushort)14418;  // ví dụ: công suất
                store.HoldingRegisters[3] = (ushort)17117;  // ví dụ: PF/THD...

                _stores[id] = store;

                var slave = ModbusSerialSlave.CreateRtu(id, _RTU_Port);
                slave.DataStore = store;
                _slaves.Add(slave);

                rtxLog.AppendText($"\r\n   -> Slave ID={id} ready");
            }

            // 3) Listen nền cho từng slave
            foreach (var s in _slaves)
            {
                Task.Run(() =>
                {
                    try { s.Listen(); }   // khi đóng COM sẽ thoát
                    catch (Exception ex)
                    {
                        // log nhẹ, tránh crash UI
                        BeginInvoke(new Action(() =>
                            rtxLog.AppendText($"\r\nListen error (ID?): {ex.Message}")));
                    }
                });
            }

            // 4) Timer update UI
            _timer = new System.Windows.Forms.Timer { Interval = 1000 };
            _timer.Tick += (s, e) =>
            {
                foreach (var kv in _stores)
                {
                    if (kv.Key == _DeviceViewInfomation)
                    {
                        var st = kv.Value;
                        HR_0.Text = kv.Value.HoldingRegisters[0].ToString();
                        HR_1.Text = kv.Value.HoldingRegisters[1].ToString();
                        HR_2.Text = kv.Value.HoldingRegisters[2].ToString();
                        HR_3.Text = kv.Value.HoldingRegisters[3].ToString();
                        HR_10.Text = kv.Value.HoldingRegisters[11].ToString();
                        /* string zArrayValue = "";
                         for (int i = 0; i < 12; i++)
                         {
                             zArrayValue += kv.Value.HoldingRegisters[i].ToString() + "-";
                         }
                         HR.Text = zArrayValue;*/
                        break;
                    }
                }
            };
            _timer.Start();

            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            try { _timer?.Stop(); } catch { }
            try { _cts?.Cancel(); } catch { }
            try
            {
                if (_RTU_Port != null && _RTU_Port.IsOpen)
                    _RTU_Port.Close();
            }
            catch { }

            _timer = null;
            _cts = null;
            _slave = null;
            _RTU_Port.Close();

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            rtxLog.AppendText("\r\n[RTU] Device(s) stopped listen");
        }

        private byte _DeviceViewInfomation = 1;
        private void cboDeviceAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            _DeviceViewInfomation = (byte)(cboDeviceAddress.SelectedIndex + 1);
        }
        private void RTU_Port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

        }

        private void btnData_Click(object sender, EventArgs e)
        {
           
        }
    }
}
