using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Modbus_Slave
{
    public partial class FrmRTU_Client : Form
    {
        private readonly FrmRTU_Multi _FrmMain;   // giữ tham chiếu về Main
        private DeviceModbus _DeviceInfo;
        public int AddressDefault = 1;
        public FrmRTU_Client(FrmRTU_Multi main)
        {
            _FrmMain = main;
            InitializeComponent();
        }
        private void FrmRTU_Client_Load(object sender, EventArgs e)
        {
            InitGridView(GV_Register);
            txtAddress.Text = AddressDefault.ToString();
            _DeviceInfo = new DeviceModbus();
        }
        public void InitGridView(DataGridView GV)
        {
            // Setup Column 
            GV.Columns.Add("Coil", "Coil");
            GV.Columns.Add("CoilValue", "Value");
            GV.Columns.Add("Discrete", "Discrete");
            GV.Columns.Add("DiscreteValue", "Value");
            GV.Columns.Add("Holding", "Holding");
            GV.Columns.Add("HoldingValue", "Value");
            GV.Columns.Add("Input", "Input");
            GV.Columns.Add("InputValue", "Value");

            GV.Columns[0].Width = 64;
            GV.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            GV.Columns[1].Width = 64;
            GV.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            GV.Columns[2].Width = 64;
            GV.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            GV.Columns[3].Width = 64;
            GV.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            GV.Columns[4].Width = 64;
            GV.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            GV.Columns[5].Width = 64;
            GV.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            GV.Columns[6].Width = 64;
            GV.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            GV.Columns[7].Width = 64;
            GV.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // setup style view

            GV.BackgroundColor = Color.White;
            GV.GridColor = Color.FromArgb(227, 239, 255);
            GV.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            GV.DefaultCellStyle.ForeColor = Color.Navy;
            GV.DefaultCellStyle.Font = new Font("Arial", 9);

            GV.AllowUserToResizeRows = false;
            GV.AllowUserToResizeColumns = true;
            GV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //GV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            GV.RowHeadersVisible = false;
            GV.AllowUserToAddRows = false;
            GV.AllowUserToDeleteRows = false;

            //// setup Height Header
            GV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            GV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            for (int i = 0; i < 10; i++)
            {
                GV.Rows.Add();
            }
            for (int i = 0; i < 10; i++)
            {
                GV.Rows[i].Cells[0].Value = i;
                GV.Rows[i].Cells[1].Value = 0;
                GV.Rows[i].Cells[2].Value = i;
                GV.Rows[i].Cells[3].Value = 0;
                GV.Rows[i].Cells[4].Value = i;
                GV.Rows[i].Cells[5].Value = 0;
                GV.Rows[i].Cells[6].Value = i;
                GV.Rows[i].Cells[7].Value = 0;
            }
        }
        private void Message_Log(string Message)
        {
            Invoke(new MethodInvoker(delegate
            {
                //LB_Log.Items.Add(Message);

            }));
        }
        private void btn_Initial_Click(object sender, EventArgs e)
        {
            int zID = 0, coilCount = 0, diCount = 0, hrCount = 0, irCount = 0;
            try
            {
                int.TryParse(txtAddress.Text, out zID);
                int.TryParse(txtCoils.Text, out coilCount);
                int.TryParse(txtDiscreteInputs.Text, out diCount);
                int.TryParse(txtHoldingRegisters.Text, out hrCount);
                int.TryParse(txtInputRegisters.Text, out irCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }

            _DeviceInfo = new DeviceModbus((byte)zID, coilCount, diCount, hrCount, irCount);
            _DeviceInfo.HoldingRegisters[0] = 1234;

            _FrmMain.DeviceAdd(_DeviceInfo);
            TimerData.Start();
            btn_Initial.Enabled = false;
        }

        private void UpdateDataCurrent()
        {
            int zAddress;
            int zValue = 0;
            for (int i = 0; i < GV_Register.Rows.Count; i++)
            {
                //Coils
                if (int.TryParse(GV_Register.Rows[i].Cells[0].Value.ToString(), out zAddress) &&
                     int.TryParse(GV_Register.Rows[i].Cells[1].Value.ToString(), out zValue))
                {
                    if (zValue == 0)
                        _DeviceInfo.Coils[zAddress] = false;
                    else
                        _DeviceInfo.Coils[zAddress] = true;
                }
                //DiscreteInputs
                if (int.TryParse(GV_Register.Rows[i].Cells[2].Value.ToString(), out zAddress) &&
                     int.TryParse(GV_Register.Rows[i].Cells[3].Value.ToString(), out zValue))
                {
                    if (zValue == 0)
                        _DeviceInfo.DiscreteInputs[zAddress] = false;
                    else
                        _DeviceInfo.DiscreteInputs[zAddress] = true;
                }
                if (int.TryParse(GV_Register.Rows[i].Cells[4].Value.ToString(), out zAddress) &&
                    int.TryParse(GV_Register.Rows[i].Cells[5].Value.ToString(), out zValue))
                {
                    _DeviceInfo.HoldingRegisters[zAddress] = (ushort)zValue;
                }
                if (int.TryParse(GV_Register.Rows[i].Cells[6].Value.ToString(), out zAddress) &&
                   int.TryParse(GV_Register.Rows[i].Cells[7].Value.ToString(), out zValue))
                {
                    _DeviceInfo.InputRegisters[zAddress] = (ushort)zValue;
                }
            }
        }
        private void ShowDataCurrent()
        {
            int zAddress;
            int zValue = 0;
            bool zIsCoilEdit = false;
            bool zIsDiscreteEdit = false;
            bool zIsHoldingEdit = false;
            bool zIsInputEdit = false;
            for (int i = 0; i < GV_Register.Rows.Count - 1; i++)
            {

                //Coils
                zIsCoilEdit = false;
                if (_CellSelected.Y == i && (_CellSelected.X == 0 || _CellSelected.X == 1))
                {
                    zIsCoilEdit = true;
                }
                if (!zIsCoilEdit)
                {
                    if (int.TryParse(GV_Register.Rows[i].Cells[0].Value.ToString(), out zAddress))
                    {
                        if (_DeviceInfo.Coils[zAddress])
                            GV_Register.Rows[i].Cells[1].Value = 1;
                        else
                            GV_Register.Rows[i].Cells[1].Value = 0;
                    }
                }
                //DiscreteInputs
                zIsDiscreteEdit = false;
                if (_CellSelected.Y == i && (_CellSelected.X == 2 || _CellSelected.X == 3))
                {
                    zIsDiscreteEdit = true;
                }
                if (!zIsDiscreteEdit)
                {
                    if (int.TryParse(GV_Register.Rows[i].Cells[2].Value.ToString(), out zAddress))
                    {
                        if (_DeviceInfo.DiscreteInputs[zAddress])
                            GV_Register.Rows[i].Cells[3].Value = 1;
                        else
                            GV_Register.Rows[i].Cells[3].Value = 0;
                    }
                }
                zIsHoldingEdit = false;
                if (_CellSelected.Y == i && (_CellSelected.X == 3 || _CellSelected.X == 4))
                {
                    zIsHoldingEdit = true;
                }

                if (!zIsHoldingEdit)
                {
                    if (int.TryParse(GV_Register.Rows[i].Cells[4].Value.ToString(), out zAddress))
                    {
                        GV_Register.Rows[i].Cells[5].Value = _DeviceInfo.HoldingRegisters[zAddress];
                        //Message_Log(zAddress + " : " +i.ToString() + ":" + GV_Register.Rows[i].Cells[5].Value.ToString());
                    }
                }
                zIsInputEdit = false;
                if (_CellSelected.Y == i && (_CellSelected.X == 6 || _CellSelected.X == 7))
                {
                    zIsInputEdit = true;
                }
                if (!zIsInputEdit)
                {
                    if (int.TryParse(GV_Register.Rows[i].Cells[6].Value.ToString(), out zAddress))
                    {
                        GV_Register.Rows[i].Cells[7].Value = _DeviceInfo.InputRegisters[zAddress];
                    }
                }
            }
        }
        private void TimerData_Tick(object sender, EventArgs e)
        {
            TimerData.Stop();
            ShowDataCurrent();
            TimerData.Start();
        }
        private Point _CellSelected = new Point();
        private void GV_Register_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _CellSelected.X = e.ColumnIndex;
            _CellSelected.Y = e.RowIndex;
        }

        private void GV_Register_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int zAddress;
            int zValue;
            if (int.TryParse(GV_Register.CurrentCell.Value.ToString(), out zValue) &&
            int.TryParse(GV_Register.Rows[_CellSelected.Y].Cells[_CellSelected.X - 1].Value.ToString(), out zAddress))
            {
                if (_CellSelected.X == 1)
                {
                    if (zValue == 0)
                        _DeviceInfo.Coils[zAddress] = false;
                    else
                        _DeviceInfo.Coils[zAddress] = true;
                }
                if (_CellSelected.X == 3)
                {
                    if (zValue == 0)
                        _DeviceInfo.DiscreteInputs[zAddress] = false;
                    else
                        _DeviceInfo.DiscreteInputs[zAddress] = true;
                }
                if (_CellSelected.X == 5)
                {
                    _DeviceInfo.HoldingRegisters[zAddress] = (ushort)zValue;
                }
                if (_CellSelected.X == 7)
                {
                    _DeviceInfo.InputRegisters[zAddress] = (ushort)zValue;
                }
            }

            _CellSelected.X = -1;
            _CellSelected.Y = -1;
        }

        private void FrmRTU_Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            TimerData.Stop();
            _FrmMain.DeviceRemove(_DeviceInfo.SlaveId);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GV_Register.Rows[0].Cells[5].Value = 1234;
        }

        private void btn_Switch_Click(object sender, EventArgs e)
        {
            if (btn_Switch.Text == "OFF")
            {
                btn_Switch.Text = "ON";
                btn_Switch.BackColor = Color.Green;
                _DeviceInfo.HoldingRegisters[2] = 1;
            }
            else
            {
                btn_Switch.Text = "OFF";
                btn_Switch.BackColor = Color.Red;
                _DeviceInfo.HoldingRegisters[2] = 0;
            }
        }

        private Random _Rand = new Random();
        private void btn_AutoData_Click(object sender, EventArgs e)
        {
            if (btn_AutoData.Text == "AutoData")
            {
                btn_AutoData.Text = "Stop";
                TimerAutoData.Start();
            }
            else
            {
                btn_AutoData.Text = "AutoData";
                TimerAutoData.Stop();
            }
        }
      
        private void TimerAutoData_Tick(object sender, EventArgs e)
        {
            _DeviceInfo.HoldingRegisters[5] = (ushort)_Rand.Next(1, 300);
            _DeviceInfo.HoldingRegisters[6] = (ushort)_Rand.Next(200, 600);
            _DeviceInfo.HoldingRegisters[7] = (ushort)_Rand.Next(300, 800);
            _DeviceInfo.HoldingRegisters[8] = (ushort)_Rand.Next(100, 400);
            for (int i = 0; i < 20; i = i + 2)
            {
                float zValue = _Rand.Next(100, 200);
                ushort[] zUshortValue = FromFloat32_WORD_SWAP(zValue);
                _DeviceInfo.InputRegisters[i] = zUshortValue[0];
                _DeviceInfo.InputRegisters[i+1] = zUshortValue[1];
            }
        }
        public static ushort[] FromFloat32_WORD_SWAP(float value)
        {
            // Start with the same logic as your ToFloat32_WORD_SWAP in reverse

            // Get bytes from float
            byte[] bytes = BitConverter.GetBytes(value);

            // Apply the Array.Reverse(b) from your function (in reverse)
            Array.Reverse(bytes); // This undoes the Array.Reverse in your function

            // Now reconstruct the ushorts according to your WORD SWAP logic
            ushort[] result = new ushort[2];

            // Based on your code:
            // b[0] = (byte)(regs[1] >> 8);  -> regs[1] high byte
            // b[1] = (byte)(regs[1] & 0xFF); -> regs[1] low byte
            // b[2] = (byte)(regs[0] >> 8);  -> regs[0] high byte  
            // b[3] = (byte)(regs[0] & 0xFF); -> regs[0] low byte

            // So after Array.Reverse:
            // bytes[3] = regs[1] high byte
            // bytes[2] = regs[1] low byte  
            // bytes[1] = regs[0] high byte
            // bytes[0] = regs[0] low byte

            // Reconstruct ushorts
            result[0] = (ushort)((bytes[1] << 8) | bytes[0]); // regs[0]
            result[1] = (ushort)((bytes[3] << 8) | bytes[2]); // regs[1]

            return result;
        }
    }
}