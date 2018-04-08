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
            this.BlueRectangleImageBox = new System.Windows.Forms.PictureBox();
            this.RedRectangleImageBox = new System.Windows.Forms.PictureBox();
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
            this.tbSizetreshold = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tbCannyTreshold = new System.Windows.Forms.TextBox();
            this.tbCannyTresholdLink = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlueRectangleImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedRectangleImageBox)).BeginInit();
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File:";
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Location = new System.Drawing.Point(44, 6);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(186, 20);
            this.fileNameTextBox.TabIndex = 1;
            this.fileNameTextBox.Text = "Slika1.png";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(236, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // originalImageBox
            // 
            this.originalImageBox.Location = new System.Drawing.Point(12, 32);
            this.originalImageBox.Name = "originalImageBox";
            this.originalImageBox.Size = new System.Drawing.Size(371, 332);
            this.originalImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.originalImageBox.TabIndex = 3;
            this.originalImageBox.TabStop = false;
            // 
            // BlueRectangleImageBox
            // 
            this.BlueRectangleImageBox.Location = new System.Drawing.Point(389, 33);
            this.BlueRectangleImageBox.Name = "BlueRectangleImageBox";
            this.BlueRectangleImageBox.Size = new System.Drawing.Size(371, 331);
            this.BlueRectangleImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BlueRectangleImageBox.TabIndex = 4;
            this.BlueRectangleImageBox.TabStop = false;
            // 
            // RedRectangleImageBox
            // 
            this.RedRectangleImageBox.Location = new System.Drawing.Point(766, 32);
            this.RedRectangleImageBox.Name = "RedRectangleImageBox";
            this.RedRectangleImageBox.Size = new System.Drawing.Size(371, 332);
            this.RedRectangleImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RedRectangleImageBox.TabIndex = 5;
            this.RedRectangleImageBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(277, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Performance";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(381, 17);
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
            this.groupBox1.Location = new System.Drawing.Point(6, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(436, 98);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HUE Filter";
            // 
            // Bar2
            // 
            this.Bar2.Location = new System.Drawing.Point(6, 49);
            this.Bar2.Maximum = 255;
            this.Bar2.Name = "Bar2";
            this.Bar2.Size = new System.Drawing.Size(378, 45);
            this.Bar2.TabIndex = 15;
            this.Bar2.MouseCaptureChanged += new System.EventHandler(this.SetBlueSliders);
            // 
            // Bar1
            // 
            this.Bar1.Location = new System.Drawing.Point(6, 13);
            this.Bar1.Maximum = 255;
            this.Bar1.Name = "Bar1";
            this.Bar1.Size = new System.Drawing.Size(378, 45);
            this.Bar1.TabIndex = 14;
            this.Bar1.MouseCaptureChanged += new System.EventHandler(this.SetBlueSliders);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(382, 61);
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
            this.groupBox2.Location = new System.Drawing.Point(6, 123);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(436, 102);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "VAL Filter";
            // 
            // Bar4
            // 
            this.Bar4.Location = new System.Drawing.Point(6, 55);
            this.Bar4.Maximum = 255;
            this.Bar4.Name = "Bar4";
            this.Bar4.Size = new System.Drawing.Size(378, 45);
            this.Bar4.TabIndex = 17;
            this.Bar4.MouseCaptureChanged += new System.EventHandler(this.SetBlueSliders);
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
            this.Bar3.Location = new System.Drawing.Point(6, 19);
            this.Bar3.Maximum = 255;
            this.Bar3.Name = "Bar3";
            this.Bar3.Size = new System.Drawing.Size(378, 45);
            this.Bar3.TabIndex = 16;
            this.Bar3.MouseCaptureChanged += new System.EventHandler(this.SetBlueSliders);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(381, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Max";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Location = new System.Drawing.Point(12, 370);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(457, 234);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Blue Filter";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Location = new System.Drawing.Point(487, 370);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(457, 234);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Red Filter";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.Bar6);
            this.groupBox5.Controls.Add(this.Bar5);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Location = new System.Drawing.Point(6, 19);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(436, 98);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "HUE Filter";
            // 
            // Bar6
            // 
            this.Bar6.Location = new System.Drawing.Point(6, 49);
            this.Bar6.Maximum = 255;
            this.Bar6.Name = "Bar6";
            this.Bar6.Size = new System.Drawing.Size(378, 45);
            this.Bar6.TabIndex = 15;
            this.Bar6.MouseCaptureChanged += new System.EventHandler(this.SetRedSliders);
            // 
            // Bar5
            // 
            this.Bar5.Location = new System.Drawing.Point(6, 13);
            this.Bar5.Maximum = 255;
            this.Bar5.Name = "Bar5";
            this.Bar5.Size = new System.Drawing.Size(378, 45);
            this.Bar5.TabIndex = 14;
            this.Bar5.MouseCaptureChanged += new System.EventHandler(this.SetRedSliders);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(381, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Max";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(384, 61);
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
            this.groupBox6.Location = new System.Drawing.Point(6, 123);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(436, 102);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "VAL Filter";
            // 
            // Bar8
            // 
            this.Bar8.Location = new System.Drawing.Point(6, 55);
            this.Bar8.Maximum = 255;
            this.Bar8.Name = "Bar8";
            this.Bar8.Size = new System.Drawing.Size(378, 45);
            this.Bar8.TabIndex = 17;
            this.Bar8.MouseCaptureChanged += new System.EventHandler(this.SetRedSliders);
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
            this.Bar7.Location = new System.Drawing.Point(6, 19);
            this.Bar7.Maximum = 255;
            this.Bar7.Name = "Bar7";
            this.Bar7.Size = new System.Drawing.Size(378, 45);
            this.Bar7.TabIndex = 16;
            this.Bar7.MouseCaptureChanged += new System.EventHandler(this.SetRedSliders);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(381, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Max";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(557, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Plavi kvadrati";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(925, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Crveni kvadrati";
            // 
            // tbCoordinates
            // 
            this.tbCoordinates.Location = new System.Drawing.Point(950, 376);
            this.tbCoordinates.Multiline = true;
            this.tbCoordinates.Name = "tbCoordinates";
            this.tbCoordinates.Size = new System.Drawing.Size(187, 228);
            this.tbCoordinates.TabIndex = 18;
            // 
            // tbSizetreshold
            // 
            this.tbSizetreshold.Location = new System.Drawing.Point(105, 621);
            this.tbSizetreshold.Name = "tbSizetreshold";
            this.tbSizetreshold.Size = new System.Drawing.Size(55, 20);
            this.tbSizetreshold.TabIndex = 19;
            this.tbSizetreshold.Text = "2000";
            this.tbSizetreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label13.Location = new System.Drawing.Point(9, 621);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(90, 17);
            this.label13.TabIndex = 20;
            this.label13.Text = "Size treshold";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label14.Location = new System.Drawing.Point(359, 622);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(165, 17);
            this.label14.TabIndex = 21;
            this.label14.Text = "Canny Threshold Linking";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label15.Location = new System.Drawing.Point(173, 621);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(116, 17);
            this.label15.TabIndex = 22;
            this.label15.Text = "Canny Threshold";
            // 
            // tbCannyTreshold
            // 
            this.tbCannyTreshold.Location = new System.Drawing.Point(290, 620);
            this.tbCannyTreshold.Name = "tbCannyTreshold";
            this.tbCannyTreshold.Size = new System.Drawing.Size(51, 20);
            this.tbCannyTreshold.TabIndex = 23;
            this.tbCannyTreshold.Text = "180";
            this.tbCannyTreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbCannyTresholdLink
            // 
            this.tbCannyTresholdLink.Location = new System.Drawing.Point(525, 621);
            this.tbCannyTresholdLink.Name = "tbCannyTresholdLink";
            this.tbCannyTresholdLink.Size = new System.Drawing.Size(51, 20);
            this.tbCannyTresholdLink.TabIndex = 24;
            this.tbCannyTresholdLink.Text = "120";
            this.tbCannyTresholdLink.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ShapeDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1149, 650);
            this.Controls.Add(this.tbCannyTresholdLink);
            this.Controls.Add(this.tbCannyTreshold);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tbSizetreshold);
            this.Controls.Add(this.tbCoordinates);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RedRectangleImageBox);
            this.Controls.Add(this.BlueRectangleImageBox);
            this.Controls.Add(this.originalImageBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.label1);
            this.Name = "ShapeDetection";
            this.Text = "ShapeDetection";
            this.Load += new System.EventHandler(this.ShapeDetection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlueRectangleImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedRectangleImageBox)).EndInit();
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox originalImageBox;
        private System.Windows.Forms.PictureBox BlueRectangleImageBox;
        private System.Windows.Forms.PictureBox RedRectangleImageBox;
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
        private System.Windows.Forms.TextBox tbSizetreshold;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbCannyTreshold;
        private System.Windows.Forms.TextBox tbCannyTresholdLink;
    }
}