namespace Modbus_Slave
{
    partial class Frm7KT0310
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
            label1 = new Label();
            txtAddress = new TextBox();
            btn_Initial = new Button();
            panel2 = new Panel();
            btn_AutoData = new Button();
            btn_RandomData = new Button();
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
            panel1.BackColor = Color.LightCyan;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtAddress);
            panel1.Controls.Add(btn_Initial);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(684, 53);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 8);
            label1.Name = "label1";
            label1.Size = new Size(91, 15);
            label1.TabIndex = 2;
            label1.Text = "Slave ID (1-255)";
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(12, 24);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(84, 23);
            txtAddress.TabIndex = 1;
            txtAddress.Text = "10";
            // 
            // btn_Initial
            // 
            btn_Initial.Dock = DockStyle.Right;
            btn_Initial.Location = new Point(609, 0);
            btn_Initial.Name = "btn_Initial";
            btn_Initial.Size = new Size(75, 53);
            btn_Initial.TabIndex = 0;
            btn_Initial.Text = "Initial";
            btn_Initial.UseVisualStyleBackColor = true;
            btn_Initial.Click += btn_Initial_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Honeydew;
            panel2.Controls.Add(btn_RandomData);
            panel2.Controls.Add(btn_AutoData);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 417);
            panel2.Name = "panel2";
            panel2.Size = new Size(684, 33);
            panel2.TabIndex = 1;
            // 
            // btn_AutoData
            // 
            btn_AutoData.Dock = DockStyle.Right;
            btn_AutoData.Location = new Point(597, 0);
            btn_AutoData.Name = "btn_AutoData";
            btn_AutoData.Size = new Size(87, 33);
            btn_AutoData.TabIndex = 1;
            btn_AutoData.Text = "AutoData";
            btn_AutoData.UseVisualStyleBackColor = true;
            btn_AutoData.Click += btn_AutoData_Click;
            // 
            // btn_RandomData
            // 
            btn_RandomData.Dock = DockStyle.Right;
            btn_RandomData.Location = new Point(510, 0);
            btn_RandomData.Name = "btn_RandomData";
            btn_RandomData.Size = new Size(87, 33);
            btn_RandomData.TabIndex = 2;
            btn_RandomData.Text = "Random";
            btn_RandomData.UseVisualStyleBackColor = true;
            btn_RandomData.Click += btn_RandomData_Click;
            // 
            // GV_Register
            // 
            GV_Register.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GV_Register.Dock = DockStyle.Fill;
            GV_Register.Location = new Point(0, 53);
            GV_Register.Name = "GV_Register";
            GV_Register.Size = new Size(684, 364);
            GV_Register.TabIndex = 2;
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
            // Frm7KT0310
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 450);
            Controls.Add(GV_Register);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Frm7KT0310";
            Text = "Siemens 7KT0310 Multifunction Meter";
            FormClosed += Frm7KT0310_FormClosed;
            Load += Frm7KT0310_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GV_Register).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btn_Initial;
        private TextBox txtAddress;
        private Label label1;
        private Panel panel2;
        private Button btn_AutoData;
        private Button btn_RandomData;
        private DataGridView GV_Register;
        private System.Windows.Forms.Timer TimerData;
        private System.Windows.Forms.Timer TimerAutoData;
    }
}