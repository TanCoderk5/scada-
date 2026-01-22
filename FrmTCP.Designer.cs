namespace Modbus_Slave
{
    partial class FrmTCP
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnStart = new Button();
            btnStop = new Button();
            panel1 = new Panel();
            txtIP = new TextBox();
            label2 = new Label();
            btnData = new Button();
            txtPort = new TextBox();
            label1 = new Label();
            rtxLog = new RichTextBox();
            HR_0 = new Label();
            HR_1 = new Label();
            HR_2 = new Label();
            HR_3 = new Label();
            HR_10 = new Label();
            HR = new Label();
            TimerAutoData = new System.Windows.Forms.Timer(components);
            btn_AutoData = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(25, 79);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(128, 79);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 0;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(txtIP);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(btn_AutoData);
            panel1.Controls.Add(btnData);
            panel1.Controls.Add(btnStop);
            panel1.Controls.Add(txtPort);
            panel1.Controls.Add(btnStart);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(552, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(248, 450);
            panel1.TabIndex = 2;
            // 
            // txtIP
            // 
            txtIP.Location = new Point(75, 44);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(153, 23);
            txtIP.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(25, 47);
            label2.Name = "label2";
            label2.Size = new Size(17, 15);
            label2.TabIndex = 1;
            label2.Text = "IP";
            // 
            // btnData
            // 
            btnData.Location = new Point(25, 127);
            btnData.Name = "btnData";
            btnData.Size = new Size(75, 23);
            btnData.TabIndex = 0;
            btnData.Text = "Data";
            btnData.UseVisualStyleBackColor = true;
            btnData.Click += btnData_Click;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(75, 15);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(153, 23);
            txtPort.TabIndex = 0;
            txtPort.Text = "502";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 18);
            label1.Name = "label1";
            label1.Size = new Size(29, 15);
            label1.TabIndex = 1;
            label1.Text = "Port";
            // 
            // rtxLog
            // 
            rtxLog.Dock = DockStyle.Bottom;
            rtxLog.Location = new Point(0, 232);
            rtxLog.Name = "rtxLog";
            rtxLog.Size = new Size(552, 218);
            rtxLog.TabIndex = 3;
            rtxLog.Text = "";
            // 
            // HR_0
            // 
            HR_0.BorderStyle = BorderStyle.FixedSingle;
            HR_0.Font = new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point, 163);
            HR_0.Location = new Point(12, 31);
            HR_0.Name = "HR_0";
            HR_0.Size = new Size(100, 70);
            HR_0.TabIndex = 4;
            HR_0.Text = "0";
            HR_0.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HR_1
            // 
            HR_1.BorderStyle = BorderStyle.FixedSingle;
            HR_1.Font = new Font("Arial", 18F, FontStyle.Bold);
            HR_1.Location = new Point(118, 31);
            HR_1.Name = "HR_1";
            HR_1.Size = new Size(100, 70);
            HR_1.TabIndex = 4;
            HR_1.Text = "0";
            HR_1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HR_2
            // 
            HR_2.BorderStyle = BorderStyle.FixedSingle;
            HR_2.Font = new Font("Arial", 18F, FontStyle.Bold);
            HR_2.Location = new Point(224, 31);
            HR_2.Name = "HR_2";
            HR_2.Size = new Size(100, 70);
            HR_2.TabIndex = 4;
            HR_2.Text = "0";
            HR_2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HR_3
            // 
            HR_3.BorderStyle = BorderStyle.FixedSingle;
            HR_3.Font = new Font("Arial", 18F, FontStyle.Bold);
            HR_3.Location = new Point(330, 31);
            HR_3.Name = "HR_3";
            HR_3.Size = new Size(100, 70);
            HR_3.TabIndex = 4;
            HR_3.Text = "0";
            HR_3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HR_10
            // 
            HR_10.BorderStyle = BorderStyle.FixedSingle;
            HR_10.Font = new Font("Arial", 18F, FontStyle.Bold);
            HR_10.Location = new Point(436, 32);
            HR_10.Name = "HR_10";
            HR_10.Size = new Size(100, 70);
            HR_10.TabIndex = 4;
            HR_10.Text = "0";
            HR_10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // HR
            // 
            HR.BorderStyle = BorderStyle.FixedSingle;
            HR.Font = new Font("Arial", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 163);
            HR.Location = new Point(12, 111);
            HR.Name = "HR";
            HR.Size = new Size(524, 39);
            HR.TabIndex = 4;
            HR.Text = "0";
            HR.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TimerAutoData
            // 
            TimerAutoData.Tick += TimerAutoData_Tick;
            // 
            // btn_AutoData
            // 
            btn_AutoData.Location = new Point(128, 127);
            btn_AutoData.Name = "btn_AutoData";
            btn_AutoData.Size = new Size(75, 23);
            btn_AutoData.TabIndex = 0;
            btn_AutoData.Text = "Auto Data";
            btn_AutoData.UseVisualStyleBackColor = true;
            btn_AutoData.Click += btn_AutoData_Click;
            // 
            // FrmTCP
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(HR);
            Controls.Add(HR_10);
            Controls.Add(HR_3);
            Controls.Add(HR_2);
            Controls.Add(HR_1);
            Controls.Add(HR_0);
            Controls.Add(rtxLog);
            Controls.Add(panel1);
            Name = "FrmTCP";
            Text = "Form1";
            Load += FrmTCP_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnStart;
        private Button btnStop;
        private Panel panel1;
        private TextBox txtIP;
        private Label label2;
        private TextBox txtPort;
        private Label label1;
        private RichTextBox rtxLog;
        private Label HR_0;
        private Label HR_1;
        private Label HR_2;
        private Label HR_3;
        private Label HR_10;
        private Button btnData;
        private Label HR;
        private System.Windows.Forms.Timer TimerAutoData;
        private Button btn_AutoData;
    }
}
