namespace Modbus_Slave
{
    partial class FrmRTU_Multi
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            groupBoxTCP = new GroupBox();
            lblTcpClients = new Label();
            btnStartTCP = new Button();
            txtTcpPort = new TextBox();
            label7 = new Label();
            cboTcpIp = new ComboBox();
            label6 = new Label();
            groupBoxRTU = new GroupBox();
            lblRtuClients = new Label();
            btnStartRTU = new Button();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            cboStopBit = new ComboBox();
            cboParity = new ComboBox();
            cboDataBits = new ComboBox();
            cboBaudRate = new ComboBox();
            cboPort = new ComboBox();
            groupBoxDevices = new GroupBox();
            lblDeviceCount = new Label();
            btn_AddMTM5M = new Button();
            btn_Add7KT0310 = new Button();
            btn_AddTemBreakPro = new Button();
            btn_AddDevice = new Button();
            rtxLog = new RichTextBox();
            timerTcpStatus = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            groupBoxTCP.SuspendLayout();
            groupBoxRTU.SuspendLayout();
            groupBoxDevices.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBoxTCP);
            panel1.Controls.Add(groupBoxRTU);
            panel1.Controls.Add(groupBoxDevices);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(605, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(270, 661);
            panel1.TabIndex = 1;
            // 
            // groupBoxTCP
            // 
            groupBoxTCP.BackColor = Color.Honeydew;
            groupBoxTCP.Controls.Add(lblTcpClients);
            groupBoxTCP.Controls.Add(btnStartTCP);
            groupBoxTCP.Controls.Add(txtTcpPort);
            groupBoxTCP.Controls.Add(label7);
            groupBoxTCP.Controls.Add(cboTcpIp);
            groupBoxTCP.Controls.Add(label6);
            groupBoxTCP.Dock = DockStyle.Top;
            groupBoxTCP.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxTCP.Location = new Point(0, 240);
            groupBoxTCP.Name = "groupBoxTCP";
            groupBoxTCP.Size = new Size(270, 140);
            groupBoxTCP.TabIndex = 2;
            groupBoxTCP.TabStop = false;
            groupBoxTCP.Text = "🌐 Modbus TCP Server";
            // 
            // lblTcpClients
            // 
            lblTcpClients.AutoSize = true;
            lblTcpClients.Font = new Font("Segoe UI", 8F);
            lblTcpClients.ForeColor = Color.DarkGreen;
            lblTcpClients.Location = new Point(15, 115);
            lblTcpClients.Name = "lblTcpClients";
            lblTcpClients.Size = new Size(79, 13);
            lblTcpClients.TabIndex = 13;
            lblTcpClients.Text = "TCP: Stopped";
            // 
            // btnStartTCP
            // 
            btnStartTCP.Font = new Font("Segoe UI", 9F);
            btnStartTCP.Location = new Point(155, 80);
            btnStartTCP.Name = "btnStartTCP";
            btnStartTCP.Size = new Size(100, 28);
            btnStartTCP.TabIndex = 13;
            btnStartTCP.Text = "Start TCP";
            btnStartTCP.UseVisualStyleBackColor = true;
            btnStartTCP.Click += btnStartTCP_Click;
            // 
            // txtTcpPort
            // 
            txtTcpPort.Font = new Font("Segoe UI", 9F);
            txtTcpPort.Location = new Point(85, 53);
            txtTcpPort.Name = "txtTcpPort";
            txtTcpPort.Size = new Size(170, 23);
            txtTcpPort.TabIndex = 8;
            txtTcpPort.Text = "502";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F);
            label7.Location = new Point(15, 56);
            label7.Name = "label7";
            label7.Size = new Size(29, 15);
            label7.TabIndex = 9;
            label7.Text = "Port";
            // 
            // cboTcpIp
            // 
            cboTcpIp.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTcpIp.Font = new Font("Segoe UI", 9F);
            cboTcpIp.FormattingEnabled = true;
            cboTcpIp.Location = new Point(85, 24);
            cboTcpIp.Name = "cboTcpIp";
            cboTcpIp.Size = new Size(170, 23);
            cboTcpIp.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F);
            label6.Location = new Point(15, 27);
            label6.Name = "label6";
            label6.Size = new Size(62, 15);
            label6.TabIndex = 12;
            label6.Text = "IP Address";
            // 
            // groupBoxRTU
            // 
            groupBoxRTU.BackColor = Color.LightCyan;
            groupBoxRTU.Controls.Add(lblRtuClients);
            groupBoxRTU.Controls.Add(btnStartRTU);
            groupBoxRTU.Controls.Add(label5);
            groupBoxRTU.Controls.Add(label4);
            groupBoxRTU.Controls.Add(label3);
            groupBoxRTU.Controls.Add(label2);
            groupBoxRTU.Controls.Add(label1);
            groupBoxRTU.Controls.Add(cboStopBit);
            groupBoxRTU.Controls.Add(cboParity);
            groupBoxRTU.Controls.Add(cboDataBits);
            groupBoxRTU.Controls.Add(cboBaudRate);
            groupBoxRTU.Controls.Add(cboPort);
            groupBoxRTU.Dock = DockStyle.Top;
            groupBoxRTU.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxRTU.Location = new Point(0, 0);
            groupBoxRTU.Name = "groupBoxRTU";
            groupBoxRTU.Size = new Size(270, 240);
            groupBoxRTU.TabIndex = 0;
            groupBoxRTU.TabStop = false;
            groupBoxRTU.Text = "🔌 Modbus RTU Serial Port";
            // 
            // lblRtuClients
            // 
            lblRtuClients.AutoSize = true;
            lblRtuClients.Font = new Font("Segoe UI", 8F);
            lblRtuClients.ForeColor = Color.Navy;
            lblRtuClients.Location = new Point(15, 215);
            lblRtuClients.Name = "lblRtuClients";
            lblRtuClients.Size = new Size(80, 13);
            lblRtuClients.TabIndex = 13;
            lblRtuClients.Text = "RTU: Stopped";
            // 
            // btnStartRTU
            // 
            btnStartRTU.Font = new Font("Segoe UI", 9F);
            btnStartRTU.Location = new Point(155, 176);
            btnStartRTU.Name = "btnStartRTU";
            btnStartRTU.Size = new Size(100, 28);
            btnStartRTU.TabIndex = 13;
            btnStartRTU.Text = "Start RTU";
            btnStartRTU.UseVisualStyleBackColor = true;
            
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F);
            label5.Location = new Point(15, 146);
            label5.Name = "label5";
            label5.Size = new Size(48, 15);
            label5.TabIndex = 8;
            label5.Text = "Stop bit";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F);
            label4.Location = new Point(15, 117);
            label4.Name = "label4";
            label4.Size = new Size(37, 15);
            label4.TabIndex = 9;
            label4.Text = "Parity";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F);
            label3.Location = new Point(15, 88);
            label3.Name = "label3";
            label3.Size = new Size(53, 15);
            label3.TabIndex = 10;
            label3.Text = "Data bits";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F);
            label2.Location = new Point(15, 59);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 11;
            label2.Text = "Baud Rate";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F);
            label1.Location = new Point(15, 25);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 12;
            label1.Text = "Port Name";
            // 
            // cboStopBit
            // 
            cboStopBit.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStopBit.Font = new Font("Segoe UI", 9F);
            cboStopBit.FormattingEnabled = true;
            cboStopBit.Items.AddRange(new object[] { "0", "1", "2", "1.5" });
            cboStopBit.Location = new Point(85, 138);
            cboStopBit.Name = "cboStopBit";
            cboStopBit.Size = new Size(170, 23);
            cboStopBit.TabIndex = 3;
            // 
            // cboParity
            // 
            cboParity.DropDownStyle = ComboBoxStyle.DropDownList;
            cboParity.Font = new Font("Segoe UI", 9F);
            cboParity.FormattingEnabled = true;
            cboParity.Items.AddRange(new object[] { "None", "Odd", "Even", "Mark", "Space" });
            cboParity.Location = new Point(85, 109);
            cboParity.Name = "cboParity";
            cboParity.Size = new Size(170, 23);
            cboParity.TabIndex = 4;
            // 
            // cboDataBits
            // 
            cboDataBits.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDataBits.Font = new Font("Segoe UI", 9F);
            cboDataBits.FormattingEnabled = true;
            cboDataBits.Items.AddRange(new object[] { "5", "6", "7", "8" });
            cboDataBits.Location = new Point(85, 80);
            cboDataBits.Name = "cboDataBits";
            cboDataBits.Size = new Size(170, 23);
            cboDataBits.TabIndex = 5;
            // 
            // cboBaudRate
            // 
            cboBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBaudRate.Font = new Font("Segoe UI", 9F);
            cboBaudRate.FormattingEnabled = true;
            cboBaudRate.Items.AddRange(new object[] { "110", "300", "1200", "2400", "9600", "19200", "38400", "57600", "115200", "230600" });
            cboBaudRate.Location = new Point(85, 51);
            cboBaudRate.Name = "cboBaudRate";
            cboBaudRate.Size = new Size(170, 23);
            cboBaudRate.TabIndex = 6;
            // 
            // cboPort
            // 
            cboPort.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPort.Font = new Font("Segoe UI", 9F);
            cboPort.FormattingEnabled = true;
            cboPort.Location = new Point(85, 22);
            cboPort.Name = "cboPort";
            cboPort.Size = new Size(170, 23);
            cboPort.TabIndex = 7;
            // 
            // groupBoxDevices
            // 
            groupBoxDevices.BackColor = Color.LavenderBlush;
            groupBoxDevices.Controls.Add(lblDeviceCount);
            groupBoxDevices.Controls.Add(btn_AddMTM5M);
            groupBoxDevices.Controls.Add(btn_Add7KT0310);
            groupBoxDevices.Controls.Add(btn_AddTemBreakPro);
            groupBoxDevices.Controls.Add(btn_AddDevice);
            groupBoxDevices.Dock = DockStyle.Bottom;
            groupBoxDevices.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            groupBoxDevices.Location = new Point(0, 411);
            groupBoxDevices.Name = "groupBoxDevices";
            groupBoxDevices.Size = new Size(270, 250);
            groupBoxDevices.TabIndex = 1;
            groupBoxDevices.TabStop = false;
            groupBoxDevices.Text = "📱 Add Devices";
            // 
            // lblDeviceCount
            // 
            lblDeviceCount.AutoSize = true;
            lblDeviceCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDeviceCount.ForeColor = Color.DarkRed;
            lblDeviceCount.Location = new Point(15, 220);
            lblDeviceCount.Name = "lblDeviceCount";
            lblDeviceCount.Size = new Size(64, 15);
            lblDeviceCount.TabIndex = 17;
            lblDeviceCount.Text = "Devices: 0";
            // 
            // btn_AddMTM5M
            // 
            btn_AddMTM5M.BackColor = Color.PaleGoldenrod;
            btn_AddMTM5M.Font = new Font("Segoe UI", 9F);
            btn_AddMTM5M.Location = new Point(15, 160);
            btn_AddMTM5M.Name = "btn_AddMTM5M";
            btn_AddMTM5M.Size = new Size(240, 35);
            btn_AddMTM5M.TabIndex = 16;
            btn_AddMTM5M.Text = "➕ MTM5M Smart MCCB";
            btn_AddMTM5M.UseVisualStyleBackColor = false;
            btn_AddMTM5M.Click += btn_AddMTM5M_Click;
            // 
            // btn_Add7KT0310
            // 
            btn_Add7KT0310.BackColor = Color.LightCyan;
            btn_Add7KT0310.Font = new Font("Segoe UI", 9F);
            btn_Add7KT0310.Location = new Point(15, 120);
            btn_Add7KT0310.Name = "btn_Add7KT0310";
            btn_Add7KT0310.Size = new Size(240, 35);
            btn_Add7KT0310.TabIndex = 15;
            btn_Add7KT0310.Text = "➕ Siemens 7KT0310 Meter";
            btn_Add7KT0310.UseVisualStyleBackColor = false;
            btn_Add7KT0310.Click += btn_Add7KT0310_Click;
            // 
            // btn_AddTemBreakPro
            // 
            btn_AddTemBreakPro.BackColor = Color.LightBlue;
            btn_AddTemBreakPro.Font = new Font("Segoe UI", 9F);
            btn_AddTemBreakPro.Location = new Point(15, 80);
            btn_AddTemBreakPro.Name = "btn_AddTemBreakPro";
            btn_AddTemBreakPro.Size = new Size(240, 35);
            btn_AddTemBreakPro.TabIndex = 14;
            btn_AddTemBreakPro.Text = "➕ TemBreakPro (KRB-5432)";
            btn_AddTemBreakPro.UseVisualStyleBackColor = false;
            btn_AddTemBreakPro.Click += btn_AddTemBreakPro_Click;
            // 
            // btn_AddDevice
            // 
            btn_AddDevice.BackColor = Color.LightGray;
            btn_AddDevice.Font = new Font("Segoe UI", 9F);
            btn_AddDevice.Location = new Point(15, 40);
            btn_AddDevice.Name = "btn_AddDevice";
            btn_AddDevice.Size = new Size(240, 35);
            btn_AddDevice.TabIndex = 13;
            btn_AddDevice.Text = "➕ Generic Device";
            btn_AddDevice.UseVisualStyleBackColor = false;
            btn_AddDevice.Click += btn_AddDevice_Click;
            // 
            // rtxLog
            // 
            rtxLog.BackColor = Color.FromArgb(30, 30, 30);
            rtxLog.BorderStyle = BorderStyle.None;
            rtxLog.Dock = DockStyle.Bottom;
            rtxLog.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtxLog.ForeColor = Color.Lime;
            rtxLog.Location = new Point(0, 498);
            rtxLog.Name = "rtxLog";
            rtxLog.ReadOnly = true;
            rtxLog.Size = new Size(605, 163);
            rtxLog.TabIndex = 3;
            rtxLog.Text = "=== MODBUS RTU/TCP MULTI-PROTOCOL SIMULATOR ===\n📡 Support both RTU (Serial) and TCP/IP (Ethernet)\n🔌 Connect N3uron via Modbus TCP on Port 502";
            // 
            // timerTcpStatus
            // 
            timerTcpStatus.Interval = 1000;
            timerTcpStatus.Tick += timerTcpStatus_Tick;
            // 
            // FrmRTU_Multi
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(875, 661);
            Controls.Add(rtxLog);
            Controls.Add(panel1);
            IsMdiContainer = true;
            Name = "FrmRTU_Multi";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Modbus RTU/TCP Multi-Device Simulator - N3uron Compatible";
            WindowState = FormWindowState.Maximized;
            FormClosing += FrmRTU_Multi_FormClosing;
            Load += FrmRTU_Multi_Load;
            panel1.ResumeLayout(false);
            groupBoxTCP.ResumeLayout(false);
            groupBoxTCP.PerformLayout();
            groupBoxRTU.ResumeLayout(false);
            groupBoxRTU.PerformLayout();
            groupBoxDevices.ResumeLayout(false);
            groupBoxDevices.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private GroupBox groupBoxRTU;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private ComboBox cboStopBit;
        private ComboBox cboParity;
        private ComboBox cboDataBits;
        private ComboBox cboBaudRate;
        private ComboBox cboPort;
        private Button btnStartRTU;
        private GroupBox groupBoxDevices;
        private Button btn_AddDevice;
        private Button btn_AddTemBreakPro;
        private Button btn_Add7KT0310;
        private Button btn_AddMTM5M;
        private RichTextBox rtxLog;
        private GroupBox groupBoxTCP;
        private ComboBox cboTcpIp;
        private Label label6;
        private TextBox txtTcpPort;
        private Label label7;
        private Button btnStartTCP;
        private Label lblDeviceCount;
        private Label lblRtuClients;
        private Label lblTcpClients;
        private System.Windows.Forms.Timer timerTcpStatus;
    }
}