namespace Modbus_Slave
{
    partial class FrmRTU
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            HR = new Label();
            HR_10 = new Label();
            HR_3 = new Label();
            HR_2 = new Label();
            HR_1 = new Label();
            HR_0 = new Label();
            rtxLog = new RichTextBox();
            panel1 = new Panel();
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
            btnData = new Button();
            btnStop = new Button();
            btnStart = new Button();
            cboDeviceAddress = new ComboBox();
            label6 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // HR
            // 
            HR.BorderStyle = BorderStyle.FixedSingle;
            HR.Font = new Font("Arial", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 163);
            HR.Location = new Point(8, 141);
            HR.Name = "HR";
            HR.Size = new Size(524, 39);
            HR.TabIndex = 7;
            HR.Text = "0";
            HR.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // HR_10
            // 
            HR_10.BorderStyle = BorderStyle.FixedSingle;
            HR_10.Font = new Font("Arial", 26.25F, FontStyle.Bold);
            HR_10.Location = new Point(432, 62);
            HR_10.Name = "HR_10";
            HR_10.Size = new Size(100, 70);
            HR_10.TabIndex = 8;
            HR_10.Text = "0";
            HR_10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HR_3
            // 
            HR_3.BorderStyle = BorderStyle.FixedSingle;
            HR_3.Font = new Font("Arial", 26.25F, FontStyle.Bold);
            HR_3.Location = new Point(326, 61);
            HR_3.Name = "HR_3";
            HR_3.Size = new Size(100, 70);
            HR_3.TabIndex = 9;
            HR_3.Text = "0";
            HR_3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HR_2
            // 
            HR_2.BorderStyle = BorderStyle.FixedSingle;
            HR_2.Font = new Font("Arial", 26.25F, FontStyle.Bold);
            HR_2.Location = new Point(220, 61);
            HR_2.Name = "HR_2";
            HR_2.Size = new Size(100, 70);
            HR_2.TabIndex = 10;
            HR_2.Text = "0";
            HR_2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HR_1
            // 
            HR_1.BorderStyle = BorderStyle.FixedSingle;
            HR_1.Font = new Font("Arial", 26.25F, FontStyle.Bold);
            HR_1.Location = new Point(114, 61);
            HR_1.Name = "HR_1";
            HR_1.Size = new Size(100, 70);
            HR_1.TabIndex = 11;
            HR_1.Text = "0";
            HR_1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HR_0
            // 
            HR_0.BorderStyle = BorderStyle.FixedSingle;
            HR_0.Font = new Font("Arial", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 163);
            HR_0.Location = new Point(8, 61);
            HR_0.Name = "HR_0";
            HR_0.Size = new Size(100, 70);
            HR_0.TabIndex = 12;
            HR_0.Text = "0";
            HR_0.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rtxLog
            // 
            rtxLog.Dock = DockStyle.Bottom;
            rtxLog.Location = new Point(0, 258);
            rtxLog.Name = "rtxLog";
            rtxLog.Size = new Size(570, 130);
            rtxLog.TabIndex = 6;
            rtxLog.Text = "";
            // 
            // panel1
            // 
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(cboStopBit);
            panel1.Controls.Add(cboParity);
            panel1.Controls.Add(cboDataBits);
            panel1.Controls.Add(cboBaudRate);
            panel1.Controls.Add(cboPort);
            panel1.Controls.Add(btnData);
            panel1.Controls.Add(btnStop);
            panel1.Controls.Add(btnStart);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(570, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(248, 388);
            panel1.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 146);
            label5.Name = "label5";
            label5.Size = new Size(48, 15);
            label5.TabIndex = 2;
            label5.Text = "Stop bit";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 117);
            label4.Name = "label4";
            label4.Size = new Size(37, 15);
            label4.TabIndex = 2;
            label4.Text = "Parity";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 88);
            label3.Name = "label3";
            label3.Size = new Size(53, 15);
            label3.TabIndex = 2;
            label3.Text = "Data bits";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 59);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 2;
            label2.Text = "Baud Rate";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 25);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 2;
            label1.Text = "Port Name";
            // 
            // cboStopBit
            // 
            cboStopBit.FormattingEnabled = true;
            cboStopBit.Items.AddRange(new object[] { "0", "1", "2", "1.5" });
            cboStopBit.Location = new Point(85, 138);
            cboStopBit.Name = "cboStopBit";
            cboStopBit.Size = new Size(137, 23);
            cboStopBit.TabIndex = 1;
            // 
            // cboParity
            // 
            cboParity.FormattingEnabled = true;
            cboParity.Items.AddRange(new object[] { "None", "Odd", "Even", "Mark", "Space" });
            cboParity.Location = new Point(85, 109);
            cboParity.Name = "cboParity";
            cboParity.Size = new Size(137, 23);
            cboParity.TabIndex = 1;
            // 
            // cboDataBits
            // 
            cboDataBits.FormattingEnabled = true;
            cboDataBits.Items.AddRange(new object[] { "5", "6", "7", "8" });
            cboDataBits.Location = new Point(85, 80);
            cboDataBits.Name = "cboDataBits";
            cboDataBits.Size = new Size(137, 23);
            cboDataBits.TabIndex = 1;
            // 
            // cboBaudRate
            // 
            cboBaudRate.FormattingEnabled = true;
            cboBaudRate.Items.AddRange(new object[] { "110", "300", "1200", "2400", "9600", "19200", "38400", "57600", "115200", "230600" });
            cboBaudRate.Location = new Point(85, 51);
            cboBaudRate.Name = "cboBaudRate";
            cboBaudRate.Size = new Size(137, 23);
            cboBaudRate.TabIndex = 1;
            // 
            // cboPort
            // 
            cboPort.FormattingEnabled = true;
            cboPort.Location = new Point(85, 22);
            cboPort.Name = "cboPort";
            cboPort.Size = new Size(137, 23);
            cboPort.TabIndex = 1;
            // 
            // btnData
            // 
            btnData.Location = new Point(45, 321);
            btnData.Name = "btnData";
            btnData.Size = new Size(75, 23);
            btnData.TabIndex = 0;
            btnData.Text = "Data";
            btnData.UseVisualStyleBackColor = true;
            btnData.Click += btnData_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(126, 186);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 0;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(23, 186);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // cboDeviceAddress
            // 
            cboDeviceAddress.FormattingEnabled = true;
            cboDeviceAddress.Items.AddRange(new object[] { "1", "2", "3", "4", "5" });
            cboDeviceAddress.Location = new Point(78, 17);
            cboDeviceAddress.Name = "cboDeviceAddress";
            cboDeviceAddress.Size = new Size(137, 23);
            cboDeviceAddress.TabIndex = 1;
            cboDeviceAddress.SelectedIndexChanged += cboDeviceAddress_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(8, 20);
            label6.Name = "label6";
            label6.Size = new Size(42, 15);
            label6.TabIndex = 2;
            label6.Text = "Device";
            // 
            // FrmRTU
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(818, 388);
            Controls.Add(HR);
            Controls.Add(HR_10);
            Controls.Add(HR_3);
            Controls.Add(HR_2);
            Controls.Add(label6);
            Controls.Add(HR_1);
            Controls.Add(HR_0);
            Controls.Add(rtxLog);
            Controls.Add(panel1);
            Controls.Add(cboDeviceAddress);
            Name = "FrmRTU";
            Text = "Frm_RTU";
            Load += FrmRTU_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label HR;
        private Label HR_10;
        private Label HR_3;
        private Label HR_2;
        private Label HR_1;
        private Label HR_0;
        private RichTextBox rtxLog;
        private Panel panel1;
        private Button btnData;
        private Button btnStop;
        private Button btnStart;
        private ComboBox cboStopBit;
        private ComboBox cboParity;
        private ComboBox cboDataBits;
        private ComboBox cboBaudRate;
        private ComboBox cboPort;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private ComboBox cboDeviceAddress;
        private Label label6;
    }
}