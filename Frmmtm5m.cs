using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Modbus_Slave
{
    /// <summary>
    /// MTM5M Smart Metering MCCB Implementation
    /// ⭐ FIXED: N3uron mapping + CDAB float encoding
    /// 
    /// N3uron Address Mapping:
    /// 40001 = Holding Register 0
    /// 41001 = Holding Register 1000 ✅ START HERE
    /// </summary>
    public partial class FrmMTM5M : Form
    {
        private readonly FrmRTU_Multi? _FrmMain;
        private DeviceModbus? _DeviceInfo;
        public int AddressDefault = 1;
        private Random _Rand = new Random();

        // ========== MODBUS ADDRESS CONSTANTS ==========
        // Base Address: 1000 (MTM5M Protocol)
        // N3uron: 41001 = Register 1000

        // Voltage Measurements (1000-1002) → N3uron: 41001-41003
        private const ushort REG_VOLTAGE_A = 1000;      // N3uron: 41001 (float32, cdab)
        private const ushort REG_VOLTAGE_B = 1002;      // N3uron: 41003
        private const ushort REG_VOLTAGE_C = 1004;      // N3uron: 41005

        // Residual Current (1006) → N3uron: 41007
        private const ushort REG_RESIDUAL_CURRENT = 1006;  // N3uron: 41007 (float32, cdab)

        // Phase Currents (1010-1015) → N3uron: 41011-41016
        private const ushort REG_CURRENT_A = 1010;      // N3uron: 41011 (float32, cdab)
        private const ushort REG_CURRENT_B = 1012;      // N3uron: 41013
        private const ushort REG_CURRENT_C = 1014;      // N3uron: 41015

        // Switch Status (1020) → N3uron: 41021
        private const ushort REG_SWITCH_STATUS = 1020;  // N3uron: 41021 (uint16)

        // Power Measurements (1030-1051) → N3uron: 41031-41052
        private const ushort REG_POWER_TOTAL_ACTIVE = 1030;     // N3uron: 41031 (float32, cdab)
        private const ushort REG_POWER_A_ACTIVE = 1032;         // N3uron: 41033
        private const ushort REG_POWER_B_ACTIVE = 1034;         // N3uron: 41035
        private const ushort REG_POWER_C_ACTIVE = 1036;         // N3uron: 41037
        private const ushort REG_POWER_TOTAL_REACTIVE = 1038;   // N3uron: 41039
        private const ushort REG_POWER_TOTAL_APPARENT = 1040;   // N3uron: 41041

        // Power Factor (1050-1053) → N3uron: 41051-41054
        private const ushort REG_PF_TOTAL = 1050;       // N3uron: 41051 (float32, cdab)
        private const ushort REG_PF_A = 1052;           // N3uron: 41053

        // Grid Frequency (1060) → N3uron: 41061
        private const ushort REG_FREQUENCY = 1060;      // N3uron: 41061 (float32, cdab)

        // Temperature (1070-1077) → N3uron: 41071-41078
        private const ushort REG_TEMP_FRONT_A = 1070;   // N3uron: 41071 (float32, cdab)
        private const ushort REG_TEMP_FRONT_B = 1072;   // N3uron: 41073
        private const ushort REG_TEMP_FRONT_C = 1074;   // N3uron: 41075
        private const ushort REG_TEMP_FRONT_N = 1076;   // N3uron: 41077

        // Energy (1100-1115) → N3uron: 41101-41116
        private const ushort REG_ENERGY_TOTAL_FORWARD = 1100;   // N3uron: 41101 (float32, cdab)
        private const ushort REG_ENERGY_A_FORWARD = 1102;       // N3uron: 41103
        private const ushort REG_ENERGY_B_FORWARD = 1104;       // N3uron: 41105
        private const ushort REG_ENERGY_C_FORWARD = 1106;       // N3uron: 41107
        private const ushort REG_ENERGY_TOTAL_REVERSE = 1108;   // N3uron: 41109

        public FrmMTM5M()
        {
            InitializeComponent();
        }

        public FrmMTM5M(FrmRTU_Multi main)
        {
            _FrmMain = main;
            InitializeComponent();
        }

        private void FrmMTM5M_Load(object sender, EventArgs e)
        {
            InitGridView(GV_Register);
            txtAddress.Text = AddressDefault.ToString();
        }

        private void InitGridView(DataGridView GV)
        {
            GV.Columns.Add("No", "No");
            GV.Columns.Add("Address", "Modbus Addr");
            GV.Columns.Add("N3uron", "N3uron Addr");
            GV.Columns.Add("Description", "Description");
            GV.Columns.Add("Value", "Value");
            GV.Columns.Add("Unit", "Unit");

            GV.Columns[0].Width = 40;
            GV.Columns[1].Width = 80;
            GV.Columns[2].Width = 80;
            GV.Columns[3].Width = 200;
            GV.Columns[4].Width = 100;
            GV.Columns[5].Width = 60;

            foreach (DataGridViewColumn col in GV.Columns)
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            GV.BackgroundColor = Color.White;
            GV.GridColor = Color.FromArgb(220, 235, 245);
            GV.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            GV.RowHeadersVisible = false;
            GV.AllowUserToAddRows = false;
        }

        private void btn_Initial_Click(object sender, EventArgs e)
        {
            try
            {
                int slaveId = int.Parse(txtAddress.Text);

                _DeviceInfo = new DeviceModbus((byte)slaveId,
                    coilCount: 100,
                    diCount: 100,
                    hrCount: 10000,
                    irCount: 100);

                if (_FrmMain != null)
                    _FrmMain.DeviceAdd(_DeviceInfo);

                InitializeDevice();
                PopulateGrid();

                TimerData.Start();
                btn_Initial.Enabled = false;
                btn_AutoData.Enabled = true;
                btn_Switch.Enabled = true;

                MessageBox.Show(
                    "✅ MTM5M Initialized!\n\n" +
                    "N3uron Address Mapping:\n" +
                    "• Phase A Voltage: 41001 (float32, cdab)\n" +
                    "• Phase A Current: 41011 (float32, cdab)\n" +
                    "• Total Power: 41031 (float32, cdab)\n" +
                    "• Frequency: 41061 (float32, cdab)\n" +
                    "• Switch Status: 41021 (uint16)",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization error: {ex.Message}");
            }
        }

        private void InitializeDevice()
        {
            var regs = _DeviceInfo!.HoldingRegisters;

            // Voltage (400V 3-phase)
            WriteFloat(regs, REG_VOLTAGE_A, 230.0f);
            WriteFloat(regs, REG_VOLTAGE_B, 230.0f);
            WriteFloat(regs, REG_VOLTAGE_C, 230.0f);

            // Current (balanced 25A)
            WriteFloat(regs, REG_CURRENT_A, 25.0f);
            WriteFloat(regs, REG_CURRENT_B, 25.0f);
            WriteFloat(regs, REG_CURRENT_C, 25.0f);

            WriteFloat(regs, REG_RESIDUAL_CURRENT, 0.015f);  // 15mA

            // Switch Status
            regs[REG_SWITCH_STATUS] = 1;  // ON

            // Power (P = V * I * PF)
            float pf = 0.95f;
            float pPhase = 230.0f * 25.0f * pf / 1000.0f;  // 5.46 kW
            WriteFloat(regs, REG_POWER_A_ACTIVE, pPhase);
            WriteFloat(regs, REG_POWER_B_ACTIVE, pPhase);
            WriteFloat(regs, REG_POWER_C_ACTIVE, pPhase);
            WriteFloat(regs, REG_POWER_TOTAL_ACTIVE, pPhase * 3);

            // Reactive Power
            float qPhase = pPhase * 0.33f;  // Q ≈ P * tan(acos(0.95))
            WriteFloat(regs, REG_POWER_TOTAL_REACTIVE, qPhase * 3);

            // Apparent Power
            float sPhase = 230.0f * 25.0f / 1000.0f;  // 5.75 kVA
            WriteFloat(regs, REG_POWER_TOTAL_APPARENT, sPhase * 3);

            // Power Factor & Frequency
            WriteFloat(regs, REG_PF_TOTAL, pf);
            WriteFloat(regs, REG_PF_A, pf);
            WriteFloat(regs, REG_FREQUENCY, 50.0f);

            // Temperature
            WriteFloat(regs, REG_TEMP_FRONT_A, 45.0f);
            WriteFloat(regs, REG_TEMP_FRONT_B, 45.0f);
            WriteFloat(regs, REG_TEMP_FRONT_C, 45.0f);
            WriteFloat(regs, REG_TEMP_FRONT_N, 42.0f);

            // Energy
            WriteFloat(regs, REG_ENERGY_TOTAL_FORWARD, 2345.67f);
            WriteFloat(regs, REG_ENERGY_A_FORWARD, 781.89f);
            WriteFloat(regs, REG_ENERGY_B_FORWARD, 781.89f);
            WriteFloat(regs, REG_ENERGY_C_FORWARD, 781.89f);
            WriteFloat(regs, REG_ENERGY_TOTAL_REVERSE, 0.0f);
        }

        private void PopulateGrid()
        {
            var displayRegs = new[]
            {
                (REG_VOLTAGE_A, "Phase A Voltage", "V"),
                (REG_VOLTAGE_B, "Phase B Voltage", "V"),
                (REG_VOLTAGE_C, "Phase C Voltage", "V"),
                (REG_CURRENT_A, "Phase A Current", "A"),
                (REG_CURRENT_B, "Phase B Current", "A"),
                (REG_CURRENT_C, "Phase C Current", "A"),
                (REG_POWER_TOTAL_ACTIVE, "Total Active Power", "kW"),
                (REG_POWER_TOTAL_REACTIVE, "Total Reactive Power", "kVar"),
                (REG_POWER_TOTAL_APPARENT, "Total Apparent Power", "kVA"),
                (REG_PF_TOTAL, "Total Power Factor", ""),
                (REG_FREQUENCY, "Frequency", "Hz"),
                (REG_SWITCH_STATUS, "Switch Status", ""),
                (REG_TEMP_FRONT_A, "Front Temp Phase A", "°C"),
                (REG_ENERGY_TOTAL_FORWARD, "Total Forward Energy", "kWh"),
            };

            int rowNum = 1;
            foreach (var (addr, desc, unit) in displayRegs)
            {
                int n3uronAddr = 40001 + addr;
                GV_Register.Rows.Add(rowNum++, addr, n3uronAddr, desc, "", unit);
            }
        }

        private void ShowDataCurrent()
        {
            if (_DeviceInfo == null) return;
            var regs = _DeviceInfo.HoldingRegisters;

            foreach (DataGridViewRow row in GV_Register.Rows)
            {
                if (row.Cells["Address"].Value == null) continue;

                int addr = Convert.ToInt32(row.Cells["Address"].Value);

                if (addr == REG_SWITCH_STATUS)
                {
                    row.Cells["Value"].Value = regs[addr] == 1 ? "ON" : "OFF";
                }
                else
                {
                    float value = ReadFloat(regs, (ushort)addr);
                    row.Cells["Value"].Value = value.ToString("F2");
                }
            }

            // Update button status
            if (regs[REG_SWITCH_STATUS] == 1)
            {
                btn_Switch.Text = "ON";
                btn_Switch.BackColor = Color.Green;
            }
            else
            {
                btn_Switch.Text = "OFF";
                btn_Switch.BackColor = Color.Red;
            }
        }

        private void TimerData_Tick(object sender, EventArgs e)
        {
            TimerData.Stop();
            ShowDataCurrent();
            TimerData.Start();
        }

        private void btn_Switch_Click(object sender, EventArgs e)
        {
            var regs = _DeviceInfo.HoldingRegisters;

            if (regs[REG_SWITCH_STATUS] == 1)
            {
                regs[REG_SWITCH_STATUS] = 0;
                btn_Switch.Text = "OFF";
                btn_Switch.BackColor = Color.Red;
            }
            else
            {
                regs[REG_SWITCH_STATUS] = 1;
                btn_Switch.Text = "ON";
                btn_Switch.BackColor = Color.Green;
            }
        }

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

        private void btn_RandomData_Click(object sender, EventArgs e)
        {
            SimulateMeasurements();
        }

        private void TimerAutoData_Tick(object sender, EventArgs e)
        {
            SimulateMeasurements();
        }

        private void SimulateMeasurements()
        {
            if (_DeviceInfo == null) return;
            var regs = _DeviceInfo.HoldingRegisters;

            // Only simulate if switch is ON
            if (regs[REG_SWITCH_STATUS] == 0)
            {
                WriteFloat(regs, REG_CURRENT_A, 0.0f);
                WriteFloat(regs, REG_CURRENT_B, 0.0f);
                WriteFloat(regs, REG_CURRENT_C, 0.0f);
                WriteFloat(regs, REG_POWER_TOTAL_ACTIVE, 0.0f);
                return;
            }

            // Voltage variations (±3%)
            float vA = 225.0f + (float)_Rand.NextDouble() * 10.0f;
            float vB = 225.0f + (float)_Rand.NextDouble() * 10.0f;
            float vC = 225.0f + (float)_Rand.NextDouble() * 10.0f;
            WriteFloat(regs, REG_VOLTAGE_A, vA);
            WriteFloat(regs, REG_VOLTAGE_B, vB);
            WriteFloat(regs, REG_VOLTAGE_C, vC);

            // Current variations (±20%)
            float iA = 20.0f + (float)_Rand.NextDouble() * 10.0f;
            float iB = 20.0f + (float)_Rand.NextDouble() * 10.0f;
            float iC = 20.0f + (float)_Rand.NextDouble() * 10.0f;
            WriteFloat(regs, REG_CURRENT_A, iA);
            WriteFloat(regs, REG_CURRENT_B, iB);
            WriteFloat(regs, REG_CURRENT_C, iC);

            // Power calculation
            float pf = 0.92f + (float)_Rand.NextDouble() * 0.06f;
            float pA = vA * iA * pf / 1000.0f;
            float pB = vB * iB * pf / 1000.0f;
            float pC = vC * iC * pf / 1000.0f;
            WriteFloat(regs, REG_POWER_A_ACTIVE, pA);
            WriteFloat(regs, REG_POWER_B_ACTIVE, pB);
            WriteFloat(regs, REG_POWER_C_ACTIVE, pC);
            WriteFloat(regs, REG_POWER_TOTAL_ACTIVE, pA + pB + pC);

            // Apparent power
            float sTotal = (vA * iA + vB * iB + vC * iC) / 1000.0f;
            WriteFloat(regs, REG_POWER_TOTAL_APPARENT, sTotal);

            // Reactive power
            float pTotal = pA + pB + pC;
            float qTotal = (float)Math.Sqrt(Math.Max(0, sTotal * sTotal - pTotal * pTotal));
            WriteFloat(regs, REG_POWER_TOTAL_REACTIVE, qTotal);

            // Update PF
            WriteFloat(regs, REG_PF_TOTAL, pf);

            // Frequency drift
            float freq = 49.95f + (float)_Rand.NextDouble() * 0.10f;
            WriteFloat(regs, REG_FREQUENCY, freq);

            // Temperature variation
            float temp = ReadFloat(regs, REG_TEMP_FRONT_A);
            temp += (float)(_Rand.NextDouble() - 0.5) * 0.5f;
            temp = Math.Max(30.0f, Math.Min(80.0f, temp));
            WriteFloat(regs, REG_TEMP_FRONT_A, temp);

            // Energy accumulation
            float currentEnergy = ReadFloat(regs, REG_ENERGY_TOTAL_FORWARD);
            WriteFloat(regs, REG_ENERGY_TOTAL_FORWARD, currentEnergy + 0.01f);
        }

        // ⭐ CDAB Float Encoding (Word Swap) - SAME AS 7KT0310
        private void WriteFloat(ushort[] regs, ushort address, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            regs[address] = (ushort)((bytes[1] << 8) | bytes[0]);      // CD
            regs[address + 1] = (ushort)((bytes[3] << 8) | bytes[2]);  // AB
        }

        private float ReadFloat(ushort[] regs, ushort address)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(regs[address] & 0xFF);        // D
            bytes[1] = (byte)(regs[address] >> 8);          // C
            bytes[2] = (byte)(regs[address + 1] & 0xFF);    // B
            bytes[3] = (byte)(regs[address + 1] >> 8);      // A
            return BitConverter.ToSingle(bytes, 0);
        }

        private void FrmMTM5M_FormClosed(object sender, FormClosedEventArgs e)
        {
            TimerData.Stop();
            TimerAutoData.Stop();
            if (_DeviceInfo != null && _FrmMain != null)
                _FrmMain.DeviceRemove(_DeviceInfo.SlaveId);
        }
    }
}