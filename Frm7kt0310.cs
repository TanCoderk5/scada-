using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Modbus_Slave
{
    /// <summary>
    /// Siemens SMART 7KT0310 Multifunction Meter Implementation
    /// ⭐ FIXED: Address mapping for N3uron PLC notation (40001-49999)
    /// 
    /// N3uron Address Mapping:
    /// 40001 = Holding Register 0
    /// 41000 = Holding Register 999
    /// 41001 = Holding Register 1000 ✅ START HERE
    /// </summary>
    public partial class Frm7KT0310 : Form
    {
        private readonly FrmRTU_Multi? _FrmMain;
        private DeviceModbus? _DeviceInfo;
        public int AddressDefault = 1;
        private Random _Rand = new Random();

        // ========== MODBUS ADDRESS CONSTANTS ==========
        // ⭐ Sử dụng base address = 0 để dễ mapping với N3uron
        // N3uron 40001 = Register 0
        // N3uron 40011 = Register 10 (Voltage V1N)

        // Voltage Measurements (10-25) → N3uron: 40011-40026
        private const ushort REG_VOLTAGE_V1N = 10;         // N3uron: 40011
        private const ushort REG_VOLTAGE_V2N = 12;         // N3uron: 40013
        private const ushort REG_VOLTAGE_V3N = 14;         // N3uron: 40015
        private const ushort REG_VOLTAGE_VAVG_LN = 16;     // N3uron: 40017
        private const ushort REG_VOLTAGE_V12 = 18;         // N3uron: 40019
        private const ushort REG_VOLTAGE_V23 = 20;         // N3uron: 40021
        private const ushort REG_VOLTAGE_V31 = 22;         // N3uron: 40023
        private const ushort REG_VOLTAGE_VAVG_LL = 24;     // N3uron: 40025

        // Current Measurements (30-37) → N3uron: 40031-40038
        private const ushort REG_CURRENT_I1 = 30;          // N3uron: 40031
        private const ushort REG_CURRENT_I2 = 32;          // N3uron: 40033
        private const ushort REG_CURRENT_I3 = 34;          // N3uron: 40035
        private const ushort REG_CURRENT_IAVG = 36;        // N3uron: 40037

        // Power Measurements (50-73) → N3uron: 40051-40074
        private const ushort REG_POWER_KW1 = 50;           // N3uron: 40051
        private const ushort REG_POWER_KW2 = 52;           // N3uron: 40053
        private const ushort REG_POWER_KW3 = 54;           // N3uron: 40055
        private const ushort REG_POWER_KVA1 = 56;          // N3uron: 40057
        private const ushort REG_POWER_KVA2 = 58;          // N3uron: 40059
        private const ushort REG_POWER_KVA3 = 60;          // N3uron: 40061
        private const ushort REG_POWER_KVAR1 = 62;         // N3uron: 40063
        private const ushort REG_POWER_KVAR2 = 64;         // N3uron: 40065
        private const ushort REG_POWER_KVAR3 = 66;         // N3uron: 40067
        private const ushort REG_POWER_KW_TOTAL = 68;      // N3uron: 40069
        private const ushort REG_POWER_KVA_TOTAL = 70;     // N3uron: 40071
        private const ushort REG_POWER_KVAR_TOTAL = 72;    // N3uron: 40073

        // Power Factor (80-87) → N3uron: 40081-40088
        private const ushort REG_PF1 = 80;                 // N3uron: 40081
        private const ushort REG_PF2 = 82;                 // N3uron: 40083
        private const ushort REG_PF3 = 84;                 // N3uron: 40085
        private const ushort REG_PF_AVG = 86;              // N3uron: 40087

        // Frequency (90-91) → N3uron: 40091-40092
        private const ushort REG_FREQUENCY = 90;           // N3uron: 40091

        // Energy (100-105) → N3uron: 40101-40106
        private const ushort REG_ENERGY_KWH_NET = 100;     // N3uron: 40101
        private const ushort REG_ENERGY_KVAH_NET = 102;    // N3uron: 40103
        private const ushort REG_ENERGY_KVARH_NET = 104;   // N3uron: 40105

        // Demand Power (110-119) → N3uron: 40111-40120
        private const ushort REG_DEMAND_KW_MAX = 110;      // N3uron: 40111
        private const ushort REG_DEMAND_KW_MIN = 112;      // N3uron: 40113
        private const ushort REG_DEMAND_KVAR_MAX = 114;    // N3uron: 40115
        private const ushort REG_DEMAND_KVAR_MIN = 116;    // N3uron: 40117
        private const ushort REG_DEMAND_KVA_MAX = 118;     // N3uron: 40119

        // Phase Energy (150-165) → N3uron: 40151-40166
        private const ushort REG_KWH1_IMP = 150;           // N3uron: 40151
        private const ushort REG_KWH2_IMP = 152;           // N3uron: 40153
        private const ushort REG_KWH3_IMP = 154;           // N3uron: 40155
        private const ushort REG_KWH1_EXP = 156;           // N3uron: 40157
        private const ushort REG_KWH2_EXP = 158;           // N3uron: 40159
        private const ushort REG_KWH3_EXP = 160;           // N3uron: 40161
        private const ushort REG_KWH_TOTAL_IMP = 162;      // N3uron: 40163
        private const ushort REG_KWH_TOTAL_EXP = 164;      // N3uron: 40165

        // Neutral Current (170-171) → N3uron: 40171-40172
        private const ushort REG_CURRENT_NEUTRAL = 170;    // N3uron: 40171

        // THD (200-217) → N3uron: 40201-40218
        private const ushort REG_THD_V1 = 200;             // N3uron: 40201
        private const ushort REG_THD_V2 = 202;             // N3uron: 40203
        private const ushort REG_THD_V3 = 204;             // N3uron: 40205
        private const ushort REG_THD_V12 = 206;            // N3uron: 40207
        private const ushort REG_THD_V23 = 208;            // N3uron: 40209
        private const ushort REG_THD_V13 = 210;            // N3uron: 40211
        private const ushort REG_THD_I1 = 212;             // N3uron: 40213
        private const ushort REG_THD_I2 = 214;             // N3uron: 40215
        private const ushort REG_THD_I3 = 216;             // N3uron: 40217

        // Configuration (1000-1020) → N3uron: 41001-41021
        private const ushort REG_CFG_SLAVE_ID = 1000;      // N3uron: 41001
        private const ushort REG_CFG_BAUD_RATE = 1001;     // N3uron: 41002
        private const ushort REG_CFG_PARITY = 1002;        // N3uron: 41003

        public Frm7KT0310()
        {
            InitializeComponent();
        }

        public Frm7KT0310(FrmRTU_Multi main)
        {
            _FrmMain = main;
            InitializeComponent();
        }

        private void Frm7KT0310_Load(object sender, EventArgs e)
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
            GV.GridColor = Color.FromArgb(200, 220, 240);
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
                    hrCount: 10000,  // Enough for address 0-9999
                    irCount: 100);

                if (_FrmMain != null)
                    _FrmMain.DeviceAdd(_DeviceInfo);

                InitializeDevice();
                PopulateGrid();

                TimerData.Start();
                btn_Initial.Enabled = false;
                btn_AutoData.Enabled = true;

                MessageBox.Show(
                    "✅ 7KT0310 Initialized!\n\n" +
                    "N3uron Address Mapping:\n" +
                    "• Phase 1 Voltage: 40011 (float32, cdab)\n" +
                    "• Phase 1 Current: 40031 (float32, cdab)\n" +
                    "• Total Power: 40069 (float32, cdab)\n" +
                    "• Frequency: 40091 (float32, cdab)",
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

            // Voltage
            WriteFloat(regs, REG_VOLTAGE_V1N, 230.0f);      // 230V
            WriteFloat(regs, REG_VOLTAGE_V2N, 230.0f);
            WriteFloat(regs, REG_VOLTAGE_V3N, 230.0f);
            WriteFloat(regs, REG_VOLTAGE_VAVG_LN, 230.0f);
            WriteFloat(regs, REG_VOLTAGE_V12, 400.0f);
            WriteFloat(regs, REG_VOLTAGE_V23, 400.0f);
            WriteFloat(regs, REG_VOLTAGE_V31, 400.0f);
            WriteFloat(regs, REG_VOLTAGE_VAVG_LL, 400.0f);

            // Current
            WriteFloat(regs, REG_CURRENT_I1, 20.5f);
            WriteFloat(regs, REG_CURRENT_I2, 20.3f);
            WriteFloat(regs, REG_CURRENT_I3, 20.7f);
            WriteFloat(regs, REG_CURRENT_IAVG, 20.5f);
            WriteFloat(regs, REG_CURRENT_NEUTRAL, 0.5f);

            // Power
            WriteFloat(regs, REG_POWER_KW1, 4.37f);
            WriteFloat(regs, REG_POWER_KW2, 4.35f);
            WriteFloat(regs, REG_POWER_KW3, 4.39f);
            WriteFloat(regs, REG_POWER_KW_TOTAL, 13.11f);
            WriteFloat(regs, REG_POWER_KVA_TOTAL, 13.8f);
            WriteFloat(regs, REG_POWER_KVAR_TOTAL, 4.32f);

            // Power Factor & Frequency
            WriteFloat(regs, REG_PF_AVG, 0.95f);
            WriteFloat(regs, REG_FREQUENCY, 50.0f);

            // Energy
            WriteFloat(regs, REG_ENERGY_KWH_NET, 1234.56f);
            WriteFloat(regs, REG_KWH_TOTAL_IMP, 1234.56f);

            // THD
            WriteFloat(regs, REG_THD_I1, 4.5f);
            WriteFloat(regs, REG_THD_I2, 4.6f);
            WriteFloat(regs, REG_THD_I3, 4.4f);
        }

        private void PopulateGrid()
        {
            var displayRegs = new[]
            {
                (REG_VOLTAGE_V1N, "Phase 1 Voltage L-N", "V"),
                (REG_VOLTAGE_V2N, "Phase 2 Voltage L-N", "V"),
                (REG_VOLTAGE_V3N, "Phase 3 Voltage L-N", "V"),
                (REG_CURRENT_I1, "Phase 1 Current", "A"),
                (REG_CURRENT_I2, "Phase 2 Current", "A"),
                (REG_CURRENT_I3, "Phase 3 Current", "A"),
                (REG_POWER_KW_TOTAL, "Total Active Power", "kW"),
                (REG_POWER_KVA_TOTAL, "Total Apparent Power", "kVA"),
                (REG_PF_AVG, "Average Power Factor", ""),
                (REG_FREQUENCY, "Frequency", "Hz"),
                (REG_ENERGY_KWH_NET, "Total Active Energy", "kWh"),
                (REG_THD_I1, "THD Current Phase 1", "%"),
            };

            int rowNum = 1;
            foreach (var (addr, desc, unit) in displayRegs)
            {
                int n3uronAddr = 40001 + addr;  // Calculate N3uron address
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
                float value = ReadFloat(regs, (ushort)addr);
                row.Cells["Value"].Value = value.ToString("F2");
            }
        }

        private void TimerData_Tick(object sender, EventArgs e)
        {
            TimerData.Stop();
            ShowDataCurrent();
            TimerData.Start();
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

            // Simulate voltage variations
            float v1 = 225.0f + (float)_Rand.NextDouble() * 10.0f;
            WriteFloat(regs, REG_VOLTAGE_V1N, v1);

            // Simulate current
            float i1 = 18.0f + (float)_Rand.NextDouble() * 5.0f;
            WriteFloat(regs, REG_CURRENT_I1, i1);

            // Calculate power
            float pf = 0.92f + (float)_Rand.NextDouble() * 0.06f;
            float kw = v1 * i1 * pf / 1000.0f;
            WriteFloat(regs, REG_POWER_KW_TOTAL, kw * 3);

            // Frequency drift
            float freq = 49.95f + (float)_Rand.NextDouble() * 0.10f;
            WriteFloat(regs, REG_FREQUENCY, freq);
        }

       
        // ⭐ CDAB Float Encoding (Word Swap) - FIXED FOR N3URON
        private void WriteFloat(ushort[] regs, ushort address, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            // BitConverter trả về little-endian: [0][1][2][3] = D C B A

            // CDAB Word Swap for N3uron:
            // reg[addr]   = CD = bytes[1] bytes[0]
            // reg[addr+1] = AB = bytes[3] bytes[2]

            regs[address] = (ushort)((bytes[1] << 8) | bytes[0]);  // CD
            regs[address + 1] = (ushort)((bytes[3] << 8) | bytes[2]);  // AB
        }

        private float ReadFloat(ushort[] regs, ushort address)
        {
            byte[] bytes = new byte[4];

            // Reconstruct from CDAB
            bytes[0] = (byte)(regs[address] & 0xFF);        // D
            bytes[1] = (byte)(regs[address] >> 8);          // C
            bytes[2] = (byte)(regs[address + 1] & 0xFF);    // B
            bytes[3] = (byte)(regs[address + 1] >> 8);      // A

            return BitConverter.ToSingle(bytes, 0);
        }

        private void Frm7KT0310_FormClosed(object sender, FormClosedEventArgs e)
        {
            TimerData.Stop();
            TimerAutoData.Stop();
            if (_DeviceInfo != null && _FrmMain != null)
                _FrmMain.DeviceRemove(_DeviceInfo.SlaveId);
        }
    }
}