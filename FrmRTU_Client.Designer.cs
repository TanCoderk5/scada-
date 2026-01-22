namespace Modbus_Slave
{
    partial class FrmRTU_Client
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
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtInputRegisters = new TextBox();
            txtHoldingRegisters = new TextBox();
            txtDiscreteInputs = new TextBox();
            txtCoils = new TextBox();
            txtAddress = new TextBox();
            btn_Initial = new Button();
            panel2 = new Panel();
            btn_Switch = new Button();
            btn_AutoData = new Button();
            GV_Register = new DataGridView();
            TimerData = new System.Windows.Forms.Timer(components);
            TimerAutoData = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GV_Register).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Azure;
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtInputRegisters);
            panel1.Controls.Add(txtHoldingRegisters);
            panel1.Controls.Add(txtDiscreteInputs);
            panel1.Controls.Add(txtCoils);
            panel1.Controls.Add(txtAddress);
            panel1.Controls.Add(btn_Initial);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(537, 53);
            panel1.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(376, 8);
            label5.Name = "label5";
            label5.Size = new Size(82, 15);
            label5.TabIndex = 2;
            label5.Text = "InputRegisters";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(273, 8);
            label4.Name = "label4";
            label4.Size = new Size(97, 15);
            label4.TabIndex = 2;
            label4.Text = "HoldingRegisters";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(185, 8);
            label3.Name = "label3";
            label3.Size = new Size(82, 15);
            label3.TabIndex = 2;
            label3.Text = "DiscreteInputs";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(111, 8);
            label2.Name = "label2";
            label2.Size = new Size(33, 15);
            label2.TabIndex = 2;
            label2.Text = "Coils";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 8);
            label1.Name = "label1";
            label1.Size = new Size(49, 15);
            label1.TabIndex = 2;
            label1.Text = "Address";
            // 
            // txtInputRegisters
            // 
            txtInputRegisters.Location = new Point(376, 24);
            txtInputRegisters.Name = "txtInputRegisters";
            txtInputRegisters.Size = new Size(84, 23);
            txtInputRegisters.TabIndex = 1;
            txtInputRegisters.Text = "100";
            // 
            // txtHoldingRegisters
            // 
            txtHoldingRegisters.Location = new Point(280, 24);
            txtHoldingRegisters.Name = "txtHoldingRegisters";
            txtHoldingRegisters.Size = new Size(84, 23);
            txtHoldingRegisters.TabIndex = 1;
            txtHoldingRegisters.Text = "100";
            // 
            // txtDiscreteInputs
            // 
            txtDiscreteInputs.Location = new Point(183, 24);
            txtDiscreteInputs.Name = "txtDiscreteInputs";
            txtDiscreteInputs.Size = new Size(84, 23);
            txtDiscreteInputs.TabIndex = 1;
            txtDiscreteInputs.Text = "100";
            // 
            // txtCoils
            // 
            txtCoils.Location = new Point(93, 24);
            txtCoils.Name = "txtCoils";
            txtCoils.Size = new Size(84, 23);
            txtCoils.TabIndex = 1;
            txtCoils.Text = "100";
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(3, 24);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(84, 23);
            txtAddress.TabIndex = 1;
            txtAddress.Text = "1";
            // 
            // btn_Initial
            // 
            btn_Initial.Dock = DockStyle.Right;
            btn_Initial.Location = new Point(462, 0);
            btn_Initial.Name = "btn_Initial";
            btn_Initial.Size = new Size(75, 53);
            btn_Initial.TabIndex = 0;
            btn_Initial.Text = "Initial";
            btn_Initial.UseVisualStyleBackColor = true;
            btn_Initial.Click += btn_Initial_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.LightYellow;
            panel2.Controls.Add(btn_Switch);
            panel2.Controls.Add(btn_AutoData);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 283);
            panel2.Name = "panel2";
            panel2.Size = new Size(537, 33);
            panel2.TabIndex = 1;
            // 
            // btn_Switch
            // 
            btn_Switch.BackColor = Color.Red;
            btn_Switch.Dock = DockStyle.Left;
            btn_Switch.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btn_Switch.ForeColor = Color.White;
            btn_Switch.Location = new Point(0, 0);
            btn_Switch.Name = "btn_Switch";
            btn_Switch.Size = new Size(54, 33);
            btn_Switch.TabIndex = 2;
            btn_Switch.Text = "OFF";
            btn_Switch.UseVisualStyleBackColor = false;
            btn_Switch.Click += btn_Switch_Click;
            // 
            // btn_AutoData
            // 
            btn_AutoData.Dock = DockStyle.Right;
            btn_AutoData.Location = new Point(450, 0);
            btn_AutoData.Name = "btn_AutoData";
            btn_AutoData.Size = new Size(87, 33);
            btn_AutoData.TabIndex = 1;
            btn_AutoData.Text = "AutoData";
            btn_AutoData.UseVisualStyleBackColor = true;
            btn_AutoData.Click += btn_AutoData_Click;
            // 
            // GV_Register
            // 
            GV_Register.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GV_Register.Dock = DockStyle.Fill;
            GV_Register.Location = new Point(0, 53);
            GV_Register.Name = "GV_Register";
            GV_Register.Size = new Size(537, 230);
            GV_Register.TabIndex = 2;
            GV_Register.CellBeginEdit += GV_Register_CellBeginEdit;
            GV_Register.CellEndEdit += GV_Register_CellEndEdit;
            // 
            // TimerData
            // 
            TimerData.Interval = 1000;
            TimerData.Tick += TimerData_Tick;
            // 
            // TimerAutoData
            // 
            TimerAutoData.Interval = 1000;
            TimerAutoData.Tick += TimerAutoData_Tick;
            // 
            // FrmRTU_Client
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(537, 316);
            Controls.Add(GV_Register);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "FrmRTU_Client";
            Text = "FrmRTU_Client";
            FormClosed += FrmRTU_Client_FormClosed;
            Load += FrmRTU_Client_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GV_Register).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btn_Initial;
        private TextBox txtDiscreteInputs;
        private TextBox txtCoils;
        private TextBox txtAddress;
        private TextBox txtHoldingRegisters;
        private TextBox txtInputRegisters;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Panel panel2;
        private Button btn_AutoData;
        private DataGridView GV_Register;
        private System.Windows.Forms.Timer TimerData;
        private Button btn_Switch;
        private System.Windows.Forms.Timer TimerAutoData;
    }
}