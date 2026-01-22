using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Modbus_Slave
{
    /// <summary>
    /// Terasaki TemBreak PRO (KRB-5432) MCCB Implementation
    /// ⭐ FIXED: N3uron mapping + CDAB float encoding
    /// 
    /// N3uron Address Mapping:
    /// 40001 = Holding Register 0
    /// 44353 = Holding Register 4352 ✅ START HERE (Measurements)
    /// </summary>
    public partial class FrmTemBreakPro : Form
    {
        private readonly FrmRTU_Multi? _FrmMain;
        private DeviceModbus? _DeviceInfo;
        public int AddressDefault = 1;
        private Random _Rand = new Random();

        // ========== MODBUS ADDRESS CONSTANTS ==========
        // Measurement Information (4352-4942) - Main monitoring data
        // N3uron: 44353 = Register 4352

        // Voltage measurements (4352-4374) → N3uron: 44353-44375
        private const ushort REG_VOLTAGE_U12 = 4352;    // N3uron: 44353 (float32, cdab)
        private const ushort REG_VOLTAGE_U23 = 4354;    // N3uron: 44355
        private const ushort REG_VOLTAGE_U31 = 4356;    // N3uron: 44357
        private const ushort REG_VOLTAGE_V1N = 4358;    // N3uron: 44359
        private const ushort REG_VOLTAGE_V2N = 4360;    // N3uron: 44361
        private const ushort REG_VOLTAGE_V3N = 4362;    // N3uron: 44363
        private const ushort REG_VOLTAGE_UAVG = 4372;   // N3uron: 44373
        private const ushort REG_VOLTAGE_VAVG = 4374;   // N3uron: 44375

        // Current measurements (4392-4406) → N3uron: 44393-44407
        private const ushort REG_CURRENT_I1 = 4392;     // N3uron: 44393 (float32, cdab)
        private const ushort REG_CURRENT_I2 = 4394;     // N3uron: 44395
        private const ushort REG_CURRENT_I3 = 4396;     // N3uron: 44397
        private const ushort REG_CURRENT_IN = 4398;     // N3uron: 44399
        private const ushort REG_CURRENT_IG = 4400;     // N3uron: 44401 (Ground)
        private const ushort REG_CURRENT_IAVG = 4406;   // N3uron: 44407

        // Power measurements (4418-4440) → N3uron: 44419-44441
        private const ushort REG_POWER_P1 = 4418;       // N3uron: 44419 (float32, cdab)
        private const ushort REG_POWER_P2 = 4420;       // N3uron: 44421
        private const ushort REG_POWER_P3 = 4422;       // N3uron: 44423
        private const ushort REG_POWER_PSUM = 4424;     // N3uron: 44425
        private const ushort REG_POWER_Q1 = 4426;       // N3uron: 44427
        private const ushort REG_POWER_QSUM = 4432;     // N3uron: 44433
        private const ushort REG_POWER_S1 = 4434;       // N3uron: 44435
        private const ushort REG_POWER_SSUM = 4440;     // N3uron: 44441

        // Power Factor (4442-4448) → N3uron: 44443-44449
        private const ushort REG_PF_1 = 4442;           // N3uron: 44443 (float32, cdab)
        private const ushort REG_PF_2 = 4444;           // N3uron: 44445
        private const ushort REG_PF_3 = 4446;           // N3uron: 44447
        private const ushort REG_PF_TOTAL = 4448;       // N3uron: 44449

        // Frequency (4458) → N3uron: 44459
        private const ushort REG_FREQUENCY = 4458;      // N3uron: 44459 (float32, cdab)

        // THD (4460-4476) → N3uron: 44461-44477
        private const ushort REG_THD_V1N = 4466;        // N3uron: 44467 (float32, cdab)
        private const ushort REG_THD_V2N = 4468;        // N3uron: 44469
        private const ushort REG_THD_V3N = 4470;        // N3uron: 44471
        private const ushort REG_THD_I1 = 4472;         // N3uron: 44473
        private const ushort REG_THD_I2 = 4474;         // N3uron: 44475
        private const ushort REG_THD_I3 = 4476;         // N3uron: 44477

        // Energy (4808-4812) → N3uron: 44809-44813
        private const ushort REG_ENERGY_EA_TOTAL_IMP = 4808;    // N3uron: 44809 (float32, cdab)
        private const ushort REG_ENERGY_EA_TOTAL_EXP = 4812;    // N3uron: 44813

        // Display Information (5376-5396) → N3uron: 45377-45397
        private const ushort REG_BREAKER_STATUS = 5376; // N3uron: 45377 (uint16)
        private const ushort REG_OCR_TEMPERATURE = 5396;        // N3uron: 45397 (float32, cdab)

        // Device Information (4096-4255) → N3uron: 44097-44256
        private const ushort REG_MANUFACTURER_NAME = 4096;      // String[16]
        private const ushort REG_PRODUCT_CODE = 4112;           // String[16]
        private const ushort REG_MCCB_RATED_CURRENT = 4254;     // N3uron: 44255 (uint16)
        private const ushort REG_MCCB_NUM_POLES = 4255;         // N3uron: 44256 (uint16)

        public FrmTemBreakPro()
        {
            InitializeComponent();
        }

        public FrmTemBreakPro(FrmRTU_Multi main)
        {
            _FrmMain = main;
            InitializeComponent();
        }

        private void FrmTemBreakPro_Load(object sender, EventArgs e)
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
            GV.GridColor = Color.FromArgb(227, 239, 255);
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
                    "✅ TemBreak PRO Initialized!\n\n" +
                    "N3uron Address Mapping:\n" +
                    "• Phase 1 Voltage L-L: 44353 (float32, cdab)\n" +
                    "• Phase 1 Current: 44393 (float32, cdab)\n" +
                    "• Total Power: 44425 (float32, cdab)\n" +
                    "• Frequency: 44459 (float32, cdab)\n" +
                    "• Breaker Status: 45377 (uint16)",
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

            // Device Information
            EncodeAsciiToRegisters("Terasaki Electric", regs, REG_MANUFACTURER_NAME, 16);
            EncodeAsciiToRegisters("TPCM00D02WA", regs, REG_PRODUCT_CODE, 16);
            regs[REG_MCCB_RATED_CURRENT] = 40;  // 40A
            regs[REG_MCCB_NUM_POLES] = 3;       // 3 poles

            // Voltages (400V 3-phase)
            WriteFloat(regs, REG_VOLTAGE_U12, 400.0f);
            WriteFloat(regs, REG_VOLTAGE_U23, 400.0f);
            WriteFloat(regs, REG_VOLTAGE_U31, 400.0f);
            WriteFloat(regs, REG_VOLTAGE_V1N, 230.0f);
            WriteFloat(regs, REG_VOLTAGE_V2N, 230.0f);
            WriteFloat(regs, REG_VOLTAGE_V3N, 230.0f);
            WriteFloat(regs, REG_VOLTAGE_UAVG, 400.0f);
            WriteFloat(regs, REG_VOLTAGE_VAVG, 230.0f);

            // Currents (light load: 15A)
            WriteFloat(regs, REG_CURRENT_I1, 15.0f);
            WriteFloat(regs, REG_CURRENT_I2, 15.0f);
            WriteFloat(regs, REG_CURRENT_I3, 15.0f);
            WriteFloat(regs, REG_CURRENT_IN, 0.0f);
            WriteFloat(regs, REG_CURRENT_IG, 0.0f);
            WriteFloat(regs, REG_CURRENT_IAVG, 15.0f);

            // Power (P = V * I * PF)
            float pf = 0.95f;
            float pPhase = 230.0f * 15.0f * pf / 1000.0f;  // 3.28 kW
            WriteFloat(regs, REG_POWER_P1, pPhase);
            WriteFloat(regs, REG_POWER_P2, pPhase);
            WriteFloat(regs, REG_POWER_P3, pPhase);
            WriteFloat(regs, REG_POWER_PSUM, pPhase * 3);

            // Reactive Power
            float qPhase = pPhase * 0.33f;
            WriteFloat(regs, REG_POWER_Q1, qPhase);
            WriteFloat(regs, REG_POWER_QSUM, qPhase * 3);

            // Apparent Power
            float sPhase = 230.0f * 15.0f / 1000.0f;  // 3.45 kVA
            WriteFloat(regs, REG_POWER_S1, sPhase);
            WriteFloat(regs, REG_POWER_SSUM, sPhase * 3);

            // Power Factor
            WriteFloat(regs, REG_PF_1, pf);
            WriteFloat(regs, REG_PF_2, pf);
            WriteFloat(regs, REG_PF_3, pf);
            WriteFloat(regs, REG_PF_TOTAL, pf);

            // Frequency
            WriteFloat(regs, REG_FREQUENCY, 50.0f);

            // THD (low values for clean power)
            WriteFloat(regs, REG_THD_V1N, 2.0f);  // 2.0%
            WriteFloat(regs, REG_THD_V2N, 2.0f);
            WriteFloat(regs, REG_THD_V3N, 2.0f);
            WriteFloat(regs, REG_THD_I1, 5.0f);   // 5.0%
            WriteFloat(regs, REG_THD_I2, 5.0f);
            WriteFloat(regs, REG_THD_I3, 5.0f);

            // Energy
            WriteFloat(regs, REG_ENERGY_EA_TOTAL_IMP, 1000.0f);  // 1000 kWh
            WriteFloat(regs, REG_ENERGY_EA_TOTAL_EXP, 0.0f);

            // Status
            regs[REG_BREAKER_STATUS] = 1;  // ON
            WriteFloat(regs, REG_OCR_TEMPERATURE, 35.0f);  // 35°C
        }

        private void PopulateGrid()
        {
            var displayRegs = new[]
            {
                (REG_VOLTAGE_U12, "Phase 1-2 Voltage L-L", "V"),
                (REG_VOLTAGE_V1N, "Phase 1 Voltage L-N", "V"),
                (REG_VOLTAGE_V2N, "Phase 2 Voltage L-N", "V"),
                (REG_VOLTAGE_V3N, "Phase 3 Voltage L-N", "V"),
                (REG_CURRENT_I1, "Phase 1 Current", "A"),
                (REG_CURRENT_I2, "Phase 2 Current", "A"),
                (REG_CURRENT_I3, "Phase 3 Current", "A"),
                (REG_POWER_PSUM, "Total Active Power", "kW"),
                (REG_POWER_QSUM, "Total Reactive Power", "kVar"),
                (REG_POWER_SSUM, "Total Apparent Power", "kVA"),
                (REG_PF_TOTAL, "Total Power Factor", ""),
                (REG_FREQUENCY, "Frequency", "Hz"),
                (REG_BREAKER_STATUS, "Breaker Status", ""),
                (REG_OCR_TEMPERATURE, "OCR Temperature", "°C"),
                (REG_ENERGY_EA_TOTAL_IMP, "Total Import Energy", "kWh"),
                (REG_THD_I1, "THD Current Phase 1", "%"),
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

                if (addr == REG_BREAKER_STATUS)
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
            if (regs[REG_BREAKER_STATUS] == 1)
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

            if (regs[REG_BREAKER_STATUS] == 1)
            {
                regs[REG_BREAKER_STATUS] = 0;
                btn_Switch.Text = "OFF";
                btn_Switch.BackColor = Color.Red;
            }
            else
            {
                regs[REG_BREAKER_STATUS] = 1;
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

        private void TimerAutoData_Tick(object sender, EventArgs e)
        {
            SimulateMeasurements();
        }

        private void SimulateMeasurements()
        {
            if (_DeviceInfo == null) return;
            var regs = _DeviceInfo.HoldingRegisters;

            // Only simulate if breaker is ON
            if (regs[REG_BREAKER_STATUS] == 0)
            {
                WriteFloat(regs, REG_CURRENT_I1, 0.0f);
                WriteFloat(regs, REG_CURRENT_I2, 0.0f);
                WriteFloat(regs, REG_CURRENT_I3, 0.0f);
                WriteFloat(regs, REG_POWER_PSUM, 0.0f);
                return;
            }

            // Voltage variations (±5%)
            float v1 = 225.0f + (float)_Rand.NextDouble() * 10.0f;
            float v2 = 225.0f + (float)_Rand.NextDouble() * 10.0f;
            float v3 = 225.0f + (float)_Rand.NextDouble() * 10.0f;
            WriteFloat(regs, REG_VOLTAGE_V1N, v1);
            WriteFloat(regs, REG_VOLTAGE_V2N, v2);
            WriteFloat(regs, REG_VOLTAGE_V3N, v3);

            // Current variations (60% of rated ± 20%)
            uint ratedCurrent = regs[REG_MCCB_RATED_CURRENT];  // e.g., 40A
            float nominalCurrent = ratedCurrent * 0.6f;        // 24A
            float i1 = nominalCurrent * (0.8f + (float)_Rand.NextDouble() * 0.4f);
            float i2 = nominalCurrent * (0.8f + (float)_Rand.NextDouble() * 0.4f);
            float i3 = nominalCurrent * (0.8f + (float)_Rand.NextDouble() * 0.4f);
            WriteFloat(regs, REG_CURRENT_I1, i1);
            WriteFloat(regs, REG_CURRENT_I2, i2);
            WriteFloat(regs, REG_CURRENT_I3, i3);

            // Power calculation
            float pf = 0.92f + (float)_Rand.NextDouble() * 0.06f;
            float p1 = v1 * i1 * pf / 1000.0f;
            float p2 = v2 * i2 * pf / 1000.0f;
            float p3 = v3 * i3 * pf / 1000.0f;
            WriteFloat(regs, REG_POWER_P1, p1);
            WriteFloat(regs, REG_POWER_P2, p2);
            WriteFloat(regs, REG_POWER_P3, p3);
            WriteFloat(regs, REG_POWER_PSUM, p1 + p2 + p3);

            // Apparent power
            float s1 = v1 * i1 / 1000.0f;
            float sTotal = (v1 * i1 + v2 * i2 + v3 * i3) / 1000.0f;
            WriteFloat(regs, REG_POWER_S1, s1);
            WriteFloat(regs, REG_POWER_SSUM, sTotal);

            // Reactive power
            float pTotal = p1 + p2 + p3;
            float qTotal = (float)Math.Sqrt(Math.Max(0, sTotal * sTotal - pTotal * pTotal));
            WriteFloat(regs, REG_POWER_QSUM, qTotal);

            // Update PF
            WriteFloat(regs, REG_PF_TOTAL, pf);

            // Frequency drift
            float freq = 49.95f + (float)_Rand.NextDouble() * 0.10f;
            WriteFloat(regs, REG_FREQUENCY, freq);

            // Temperature variation
            float temp = ReadFloat(regs, REG_OCR_TEMPERATURE);
            temp += (float)(_Rand.NextDouble() - 0.5);
            temp = Math.Max(30.0f, Math.Min(80.0f, temp));
            WriteFloat(regs, REG_OCR_TEMPERATURE, temp);

            // Energy accumulation
            float currentEnergy = ReadFloat(regs, REG_ENERGY_EA_TOTAL_IMP);
            WriteFloat(regs, REG_ENERGY_EA_TOTAL_IMP, currentEnergy + 0.01f);
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

        private void EncodeAsciiToRegisters(string text, ushort[] regs, int address, int length)
        {
            int maxBytes = length * 2;
            byte[] asciiBytes = Encoding.ASCII.GetBytes(text);

            if (asciiBytes.Length > maxBytes)
                asciiBytes = asciiBytes.Take(maxBytes).ToArray();

            byte[] buf = new byte[maxBytes];
            Array.Clear(buf, 0, buf.Length);
            Array.Copy(asciiBytes, buf, asciiBytes.Length);

            for (int i = 0; i < length; i++)
            {
                byte hi = buf[i * 2];
                byte lo = buf[i * 2 + 1];
                regs[address + i] = (ushort)((hi << 8) | lo);
            }
        }

        private void FrmTemBreakPro_FormClosed(object sender, FormClosedEventArgs e)
        {
            TimerData.Stop();
            TimerAutoData.Stop();
            if (_DeviceInfo != null && _FrmMain != null)
                _FrmMain.DeviceRemove(_DeviceInfo.SlaveId);
        }
    }
}