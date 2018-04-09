namespace IronMan
{
    partial class ShapeDetection
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
            this.label1 = new System.Windows.Forms.Label();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.originalImageBox = new System.Windows.Forms.PictureBox();
            this.Type1mageBox = new System.Windows.Forms.PictureBox();
            this.Type2ImageBox = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Bar2 = new System.Windows.Forms.TrackBar();
            this.Bar1 = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Bar4 = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.Bar3 = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.Bar6 = new System.Windows.Forms.TrackBar();
            this.Bar5 = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.Bar8 = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.Bar7 = new System.Windows.Forms.TrackBar();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tbCoordinates = new System.Windows.Forms.TextBox();
            this.tbSizeMin = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tbCannyTreshold = new System.Windows.Forms.TextBox();
            this.tbCannyTresholdLink = new System.Windows.Forms.TextBox();
            this.tbSizeMax = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cbCameras = new System.Windows.Forms.ComboBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.btnTimer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Type1mageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Type2ImageBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar3)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar5)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar7)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File:";
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Location = new System.Drawing.Point(40, 18);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(186, 20);
            this.fileNameTextBox.TabIndex = 1;
            this.fileNameTextBox.Text = "Slika2.png";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(638, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 27);
            this.button1.TabIndex = 2;
            this.button1.Text = "Detect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ReadFromWebcam);
            // 
            // originalImageBox
            // 
            this.originalImageBox.Location = new System.Drawing.Point(12, 103);
            this.originalImageBox.Name = "originalImageBox";
            this.originalImageBox.Size = new System.Drawing.Size(371, 336);
            this.originalImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.originalImageBox.TabIndex = 3;
            this.originalImageBox.TabStop = false;
            // 
            // Type1mageBox
            // 
            this.Type1mageBox.Location = new System.Drawing.Point(389, 103);
            this.Type1mageBox.Name = "Type1mageBox";
            this.Type1mageBox.Size = new System.Drawing.Size(371, 335);
            this.Type1mageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Type1mageBox.TabIndex = 4;
            this.Type1mageBox.TabStop = false;
            // 
            // Type2ImageBox
            // 
            this.Type2ImageBox.Location = new System.Drawing.Point(766, 103);
            this.Type2ImageBox.Name = "Type2ImageBox";
            this.Type2ImageBox.Size = new System.Drawing.Size(371, 336);
            this.Type2ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Type2ImageBox.TabIndex = 5;
            this.Type2ImageBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Performance";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(381, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Min";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Bar2);
            this.groupBox1.Controls.Add(this.Bar1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(6, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(436, 102);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HUE Filter";
            // 
            // Bar2
            // 
            this.Bar2.Location = new System.Drawing.Point(6, 55);
            this.Bar2.Maximum = 255;
            this.Bar2.Name = "Bar2";
            this.Bar2.Size = new System.Drawing.Size(378, 45);
            this.Bar2.TabIndex = 15;
            this.Bar2.MouseCaptureChanged += new System.EventHandler(this.SetType1Sliders);
            // 
            // Bar1
            // 
            this.Bar1.Location = new System.Drawing.Point(6, 16);
            this.Bar1.Maximum = 255;
            this.Bar1.Name = "Bar1";
            this.Bar1.Size = new System.Drawing.Size(378, 45);
            this.Bar1.TabIndex = 14;
            this.Bar1.MouseCaptureChanged += new System.EventHandler(this.SetType1Sliders);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(382, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Max";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Bar4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.Bar3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(6, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(436, 106);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "VAL Filter";
            // 
            // Bar4
            // 
            this.Bar4.Location = new System.Drawing.Point(6, 57);
            this.Bar4.Maximum = 255;
            this.Bar4.Name = "Bar4";
            this.Bar4.Size = new System.Drawing.Size(378, 45);
            this.Bar4.TabIndex = 17;
            this.Bar4.MouseCaptureChanged += new System.EventHandler(this.SetType1Sliders);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(381, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Min";
            // 
            // Bar3
            // 
            this.Bar3.Location = new System.Drawing.Point(6, 22);
            this.Bar3.Maximum = 255;
            this.Bar3.Name = "Bar3";
            this.Bar3.Size = new System.Drawing.Size(378, 45);
            this.Bar3.TabIndex = 16;
            this.Bar3.MouseCaptureChanged += new System.EventHandler(this.SetType1Sliders);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(381, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Max";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Location = new System.Drawing.Point(12, 445);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(457, 238);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Type 1 Filter";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Location = new System.Drawing.Point(487, 444);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(457, 238);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Type 2 Filter";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.Bar6);
            this.groupBox5.Controls.Add(this.Bar5);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Location = new System.Drawing.Point(6, 16);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(436, 102);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "HUE Filter";
            // 
            // Bar6
            // 
            this.Bar6.Location = new System.Drawing.Point(6, 53);
            this.Bar6.Maximum = 255;
            this.Bar6.Name = "Bar6";
            this.Bar6.Size = new System.Drawing.Size(378, 45);
            this.Bar6.TabIndex = 15;
            this.Bar6.MouseCaptureChanged += new System.EventHandler(this.SetType2Sliders);
            // 
            // Bar5
            // 
            this.Bar5.Location = new System.Drawing.Point(6, 19);
            this.Bar5.Maximum = 255;
            this.Bar5.Name = "Bar5";
            this.Bar5.Size = new System.Drawing.Size(378, 45);
            this.Bar5.TabIndex = 14;
            this.Bar5.MouseCaptureChanged += new System.EventHandler(this.SetType2Sliders);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(381, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Max";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(384, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Min";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.Bar8);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.Bar7);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Location = new System.Drawing.Point(6, 120);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(436, 106);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "VAL Filter";
            // 
            // Bar8
            // 
            this.Bar8.Location = new System.Drawing.Point(6, 57);
            this.Bar8.Maximum = 255;
            this.Bar8.Name = "Bar8";
            this.Bar8.Size = new System.Drawing.Size(378, 45);
            this.Bar8.TabIndex = 17;
            this.Bar8.MouseCaptureChanged += new System.EventHandler(this.SetType2Sliders);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(381, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Min";
            // 
            // Bar7
            // 
            this.Bar7.Location = new System.Drawing.Point(6, 22);
            this.Bar7.Maximum = 255;
            this.Bar7.Name = "Bar7";
            this.Bar7.Size = new System.Drawing.Size(378, 45);
            this.Bar7.TabIndex = 16;
            this.Bar7.MouseCaptureChanged += new System.EventHandler(this.SetType2Sliders);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(381, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Max";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(552, 84);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Type 1";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(935, 84);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Type 2";
            // 
            // tbCoordinates
            // 
            this.tbCoordinates.Location = new System.Drawing.Point(950, 449);
            this.tbCoordinates.Multiline = true;
            this.tbCoordinates.Name = "tbCoordinates";
            this.tbCoordinates.Size = new System.Drawing.Size(187, 265);
            this.tbCoordinates.TabIndex = 18;
            // 
            // tbSizeMin
            // 
            this.tbSizeMin.Location = new System.Drawing.Point(100, 697);
            this.tbSizeMin.Name = "tbSizeMin";
            this.tbSizeMin.Size = new System.Drawing.Size(55, 20);
            this.tbSizeMin.TabIndex = 19;
            this.tbSizeMin.Text = "4000";
            this.tbSizeMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label13.Location = new System.Drawing.Point(9, 697);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(90, 17);
            this.label13.TabIndex = 20;
            this.label13.Text = "Size treshold";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label14.Location = new System.Drawing.Point(453, 699);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(165, 17);
            this.label14.TabIndex = 21;
            this.label14.Text = "Canny Threshold Linking";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label15.Location = new System.Drawing.Point(267, 698);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(116, 17);
            this.label15.TabIndex = 22;
            this.label15.Text = "Canny Threshold";
            // 
            // tbCannyTreshold
            // 
            this.tbCannyTreshold.Location = new System.Drawing.Point(384, 697);
            this.tbCannyTreshold.Name = "tbCannyTreshold";
            this.tbCannyTreshold.Size = new System.Drawing.Size(51, 20);
            this.tbCannyTreshold.TabIndex = 23;
            this.tbCannyTreshold.Text = "180";
            this.tbCannyTreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbCannyTresholdLink
            // 
            this.tbCannyTresholdLink.Location = new System.Drawing.Point(619, 698);
            this.tbCannyTresholdLink.Name = "tbCannyTresholdLink";
            this.tbCannyTresholdLink.Size = new System.Drawing.Size(51, 20);
            this.tbCannyTresholdLink.TabIndex = 24;
            this.tbCannyTresholdLink.Text = "120";
            this.tbCannyTresholdLink.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbSizeMax
            // 
            this.tbSizeMax.Location = new System.Drawing.Point(195, 697);
            this.tbSizeMax.Name = "tbSizeMax";
            this.tbSizeMax.Size = new System.Drawing.Size(55, 20);
            this.tbSizeMax.TabIndex = 25;
            this.tbSizeMax.Text = "12000";
            this.tbSizeMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label16.Location = new System.Drawing.Point(155, 698);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(40, 17);
            this.label16.TabIndex = 26;
            this.label16.Text = "< = >";
            // 
            // cbCameras
            // 
            this.cbCameras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCameras.FormattingEnabled = true;
            this.cbCameras.Location = new System.Drawing.Point(439, 17);
            this.cbCameras.Name = "cbCameras";
            this.cbCameras.Size = new System.Drawing.Size(193, 21);
            this.cbCameras.TabIndex = 27;
            this.cbCameras.SelectionChangeCommitted += new System.EventHandler(this.cbCameras_SelectionChangeCommitted);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnTimer);
            this.groupBox7.Controls.Add(this.button2);
            this.groupBox7.Controls.Add(this.label17);
            this.groupBox7.Controls.Add(this.fileNameTextBox);
            this.groupBox7.Controls.Add(this.cbCameras);
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.button1);
            this.groupBox7.Location = new System.Drawing.Point(12, 12);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(810, 50);
            this.groupBox7.TabIndex = 28;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Source";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(232, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 27);
            this.button2.TabIndex = 29;
            this.button2.Text = "Detect";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.ReadFromImage);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(379, 21);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(54, 13);
            this.label17.TabIndex = 28;
            this.label17.Text = "WebCam:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(155, 87);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(72, 13);
            this.label18.TabIndex = 29;
            this.label18.Text = "Source image";
            // 
            // btnTimer
            // 
            this.btnTimer.Location = new System.Drawing.Point(725, 15);
            this.btnTimer.Name = "btnTimer";
            this.btnTimer.Size = new System.Drawing.Size(75, 27);
            this.btnTimer.TabIndex = 30;
            this.btnTimer.Text = "Start";
            this.btnTimer.UseVisualStyleBackColor = true;
            this.btnTimer.Click += new System.EventHandler(this.btnTimer_Click);
            // 
            // ShapeDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1149, 722);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tbSizeMax);
            this.Controls.Add(this.tbCannyTresholdLink);
            this.Controls.Add(this.tbCannyTreshold);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tbSizeMin);
            this.Controls.Add(this.tbCoordinates);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Type2ImageBox);
            this.Controls.Add(this.Type1mageBox);
            this.Controls.Add(this.originalImageBox);
            this.Name = "ShapeDetection";
            this.Text = "ShapeDetection";
            this.Load += new System.EventHandler(this.ShapeDetection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Type1mageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Type2ImageBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar3)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar5)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Bar8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Bar7)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox originalImageBox;
        private System.Windows.Forms.PictureBox Type1mageBox;
        private System.Windows.Forms.PictureBox Type2ImageBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar Bar1;
        private System.Windows.Forms.TrackBar Bar2;
        private System.Windows.Forms.TrackBar Bar4;
        private System.Windows.Forms.TrackBar Bar3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TrackBar Bar6;
        private System.Windows.Forms.TrackBar Bar5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TrackBar Bar8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar Bar7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbCoordinates;
        private System.Windows.Forms.TextBox tbSizeMin;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbCannyTreshold;
        private System.Windows.Forms.TextBox tbCannyTresholdLink;
        private System.Windows.Forms.TextBox tbSizeMax;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cbCameras;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnTimer;
    }
}