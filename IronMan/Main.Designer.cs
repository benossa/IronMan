namespace IronMan
{
    partial class Main
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
            this.cbSerialPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPortStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSerialMonitor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbBaudRate = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOpenSerial = new System.Windows.Forms.Button();
            this.btnCloseSerial = new System.Windows.Forms.Button();
            this.lblServo1Value = new System.Windows.Forms.Label();
            this.Servo1Scroll = new System.Windows.Forms.HScrollBar();
            this.Servo2Scroll = new System.Windows.Forms.HScrollBar();
            this.lblServo2Value = new System.Windows.Forms.Label();
            this.Servo3Scroll = new System.Windows.Forms.HScrollBar();
            this.lblServo3Value = new System.Windows.Forms.Label();
            this.Servo6Scroll = new System.Windows.Forms.HScrollBar();
            this.lblServo6Value = new System.Windows.Forms.Label();
            this.Servo5Scroll = new System.Windows.Forms.HScrollBar();
            this.lblServo5Value = new System.Windows.Forms.Label();
            this.Servo4Scroll = new System.Windows.Forms.HScrollBar();
            this.lblServo4Value = new System.Windows.Forms.Label();
            this.cbUseRemoteControl = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbSerialPort
            // 
            this.cbSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSerialPort.FormattingEnabled = true;
            this.cbSerialPort.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9"});
            this.cbSerialPort.Location = new System.Drawing.Point(12, 30);
            this.cbSerialPort.Name = "cbSerialPort";
            this.cbSerialPort.Size = new System.Drawing.Size(177, 21);
            this.cbSerialPort.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "COM Port";
            // 
            // lblPortStatus
            // 
            this.lblPortStatus.AutoSize = true;
            this.lblPortStatus.Location = new System.Drawing.Point(145, 9);
            this.lblPortStatus.Name = "lblPortStatus";
            this.lblPortStatus.Size = new System.Drawing.Size(39, 13);
            this.lblPortStatus.TabIndex = 2;
            this.lblPortStatus.Text = "Closed";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(80, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Port Status:";
            // 
            // tbSerialMonitor
            // 
            this.tbSerialMonitor.Location = new System.Drawing.Point(195, 30);
            this.tbSerialMonitor.Multiline = true;
            this.tbSerialMonitor.Name = "tbSerialMonitor";
            this.tbSerialMonitor.Size = new System.Drawing.Size(357, 236);
            this.tbSerialMonitor.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(342, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Serial Monitor";
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBaudRate.FormattingEnabled = true;
            this.cbBaudRate.Items.AddRange(new object[] {
            "9600",
            "115200"});
            this.cbBaudRate.Location = new System.Drawing.Point(12, 76);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(177, 21);
            this.cbBaudRate.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Baud Rate";
            // 
            // btnOpenSerial
            // 
            this.btnOpenSerial.Location = new System.Drawing.Point(12, 112);
            this.btnOpenSerial.Name = "btnOpenSerial";
            this.btnOpenSerial.Size = new System.Drawing.Size(177, 23);
            this.btnOpenSerial.TabIndex = 8;
            this.btnOpenSerial.Text = "Open Serial";
            this.btnOpenSerial.UseVisualStyleBackColor = true;
            this.btnOpenSerial.Click += new System.EventHandler(this.btnOpenSerial_Click);
            // 
            // btnCloseSerial
            // 
            this.btnCloseSerial.Location = new System.Drawing.Point(12, 141);
            this.btnCloseSerial.Name = "btnCloseSerial";
            this.btnCloseSerial.Size = new System.Drawing.Size(177, 23);
            this.btnCloseSerial.TabIndex = 9;
            this.btnCloseSerial.Text = "Close Serial";
            this.btnCloseSerial.UseVisualStyleBackColor = true;
            this.btnCloseSerial.Click += new System.EventHandler(this.btnCloseSerial_Click);
            // 
            // lblServo1Value
            // 
            this.lblServo1Value.AutoSize = true;
            this.lblServo1Value.Location = new System.Drawing.Point(124, 275);
            this.lblServo1Value.Name = "lblServo1Value";
            this.lblServo1Value.Size = new System.Drawing.Size(53, 13);
            this.lblServo1Value.TabIndex = 10;
            this.lblServo1Value.Text = "Servo 1 : ";
            // 
            // Servo1Scroll
            // 
            this.Servo1Scroll.Location = new System.Drawing.Point(15, 292);
            this.Servo1Scroll.Maximum = 189;
            this.Servo1Scroll.Minimum = 1;
            this.Servo1Scroll.Name = "Servo1Scroll";
            this.Servo1Scroll.Size = new System.Drawing.Size(268, 27);
            this.Servo1Scroll.SmallChange = 5;
            this.Servo1Scroll.TabIndex = 11;
            this.Servo1Scroll.Value = 90;
            this.Servo1Scroll.ValueChanged += new System.EventHandler(this.Servo1Scroll_ValueChanged);
            this.Servo1Scroll.MouseCaptureChanged += new System.EventHandler(this.Servo1Scroll_MouseCaptureChanged);
            // 
            // Servo2Scroll
            // 
            this.Servo2Scroll.Location = new System.Drawing.Point(15, 353);
            this.Servo2Scroll.Maximum = 189;
            this.Servo2Scroll.Minimum = 1;
            this.Servo2Scroll.Name = "Servo2Scroll";
            this.Servo2Scroll.Size = new System.Drawing.Size(268, 27);
            this.Servo2Scroll.SmallChange = 5;
            this.Servo2Scroll.TabIndex = 13;
            this.Servo2Scroll.Value = 90;
            this.Servo2Scroll.ValueChanged += new System.EventHandler(this.Servo2Scroll_ValueChanged);
            this.Servo2Scroll.MouseCaptureChanged += new System.EventHandler(this.Servo2Scroll_MouseCaptureChanged);
            // 
            // lblServo2Value
            // 
            this.lblServo2Value.AutoSize = true;
            this.lblServo2Value.Location = new System.Drawing.Point(124, 335);
            this.lblServo2Value.Name = "lblServo2Value";
            this.lblServo2Value.Size = new System.Drawing.Size(53, 13);
            this.lblServo2Value.TabIndex = 12;
            this.lblServo2Value.Text = "Servo 2 : ";
            // 
            // Servo3Scroll
            // 
            this.Servo3Scroll.Location = new System.Drawing.Point(15, 412);
            this.Servo3Scroll.Maximum = 189;
            this.Servo3Scroll.Minimum = 1;
            this.Servo3Scroll.Name = "Servo3Scroll";
            this.Servo3Scroll.Size = new System.Drawing.Size(268, 27);
            this.Servo3Scroll.SmallChange = 5;
            this.Servo3Scroll.TabIndex = 15;
            this.Servo3Scroll.Value = 90;
            // 
            // lblServo3Value
            // 
            this.lblServo3Value.AutoSize = true;
            this.lblServo3Value.Location = new System.Drawing.Point(124, 396);
            this.lblServo3Value.Name = "lblServo3Value";
            this.lblServo3Value.Size = new System.Drawing.Size(53, 13);
            this.lblServo3Value.TabIndex = 14;
            this.lblServo3Value.Text = "Servo 3 : ";
            // 
            // Servo6Scroll
            // 
            this.Servo6Scroll.Location = new System.Drawing.Point(287, 412);
            this.Servo6Scroll.Maximum = 189;
            this.Servo6Scroll.Minimum = 1;
            this.Servo6Scroll.Name = "Servo6Scroll";
            this.Servo6Scroll.Size = new System.Drawing.Size(268, 27);
            this.Servo6Scroll.SmallChange = 5;
            this.Servo6Scroll.TabIndex = 21;
            this.Servo6Scroll.Value = 90;
            // 
            // lblServo6Value
            // 
            this.lblServo6Value.AutoSize = true;
            this.lblServo6Value.Location = new System.Drawing.Point(396, 396);
            this.lblServo6Value.Name = "lblServo6Value";
            this.lblServo6Value.Size = new System.Drawing.Size(53, 13);
            this.lblServo6Value.TabIndex = 20;
            this.lblServo6Value.Text = "Servo 6 : ";
            // 
            // Servo5Scroll
            // 
            this.Servo5Scroll.Location = new System.Drawing.Point(287, 353);
            this.Servo5Scroll.Maximum = 189;
            this.Servo5Scroll.Minimum = 1;
            this.Servo5Scroll.Name = "Servo5Scroll";
            this.Servo5Scroll.Size = new System.Drawing.Size(268, 27);
            this.Servo5Scroll.SmallChange = 5;
            this.Servo5Scroll.TabIndex = 19;
            this.Servo5Scroll.Value = 90;
            // 
            // lblServo5Value
            // 
            this.lblServo5Value.AutoSize = true;
            this.lblServo5Value.Location = new System.Drawing.Point(396, 335);
            this.lblServo5Value.Name = "lblServo5Value";
            this.lblServo5Value.Size = new System.Drawing.Size(53, 13);
            this.lblServo5Value.TabIndex = 18;
            this.lblServo5Value.Text = "Servo 5 : ";
            // 
            // Servo4Scroll
            // 
            this.Servo4Scroll.Location = new System.Drawing.Point(287, 292);
            this.Servo4Scroll.Maximum = 189;
            this.Servo4Scroll.Minimum = 1;
            this.Servo4Scroll.Name = "Servo4Scroll";
            this.Servo4Scroll.Size = new System.Drawing.Size(268, 27);
            this.Servo4Scroll.SmallChange = 5;
            this.Servo4Scroll.TabIndex = 17;
            this.Servo4Scroll.Value = 90;
            // 
            // lblServo4Value
            // 
            this.lblServo4Value.AutoSize = true;
            this.lblServo4Value.Location = new System.Drawing.Point(396, 275);
            this.lblServo4Value.Name = "lblServo4Value";
            this.lblServo4Value.Size = new System.Drawing.Size(53, 13);
            this.lblServo4Value.TabIndex = 16;
            this.lblServo4Value.Text = "Servo 4 : ";
            // 
            // cbUseRemoteControl
            // 
            this.cbUseRemoteControl.AutoSize = true;
            this.cbUseRemoteControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cbUseRemoteControl.Location = new System.Drawing.Point(23, 181);
            this.cbUseRemoteControl.Name = "cbUseRemoteControl";
            this.cbUseRemoteControl.Size = new System.Drawing.Size(154, 21);
            this.cbUseRemoteControl.TabIndex = 22;
            this.cbUseRemoteControl.Text = "Use Remote Control";
            this.cbUseRemoteControl.UseVisualStyleBackColor = true;
            this.cbUseRemoteControl.CheckedChanged += new System.EventHandler(this.cbUseRemoteControl_CheckedChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(564, 450);
            this.Controls.Add(this.cbUseRemoteControl);
            this.Controls.Add(this.Servo6Scroll);
            this.Controls.Add(this.lblServo6Value);
            this.Controls.Add(this.Servo5Scroll);
            this.Controls.Add(this.lblServo5Value);
            this.Controls.Add(this.Servo4Scroll);
            this.Controls.Add(this.lblServo4Value);
            this.Controls.Add(this.Servo3Scroll);
            this.Controls.Add(this.lblServo3Value);
            this.Controls.Add(this.Servo2Scroll);
            this.Controls.Add(this.lblServo2Value);
            this.Controls.Add(this.Servo1Scroll);
            this.Controls.Add(this.lblServo1Value);
            this.Controls.Add(this.btnCloseSerial);
            this.Controls.Add(this.btnOpenSerial);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbBaudRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSerialMonitor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblPortStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbSerialPort);
            this.Name = "Main";
            this.Text = "Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbSerialPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPortStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSerialMonitor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbBaudRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOpenSerial;
        private System.Windows.Forms.Button btnCloseSerial;
        private System.Windows.Forms.Label lblServo1Value;
        private System.Windows.Forms.HScrollBar Servo1Scroll;
        private System.Windows.Forms.HScrollBar Servo2Scroll;
        private System.Windows.Forms.Label lblServo2Value;
        private System.Windows.Forms.HScrollBar Servo3Scroll;
        private System.Windows.Forms.Label lblServo3Value;
        private System.Windows.Forms.HScrollBar Servo6Scroll;
        private System.Windows.Forms.Label lblServo6Value;
        private System.Windows.Forms.HScrollBar Servo5Scroll;
        private System.Windows.Forms.Label lblServo5Value;
        private System.Windows.Forms.HScrollBar Servo4Scroll;
        private System.Windows.Forms.Label lblServo4Value;
        private System.Windows.Forms.CheckBox cbUseRemoteControl;
    }
}