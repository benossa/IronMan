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
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlueRectangleImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedRectangleImageBox)).BeginInit();
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
            this.originalImageBox.Size = new System.Drawing.Size(371, 331);
            this.originalImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.originalImageBox.TabIndex = 3;
            this.originalImageBox.TabStop = false;
            // 
            // BlueRectangleImageBox
            // 
            this.BlueRectangleImageBox.Location = new System.Drawing.Point(766, 8);
            this.BlueRectangleImageBox.Name = "BlueRectangleImageBox";
            this.BlueRectangleImageBox.Size = new System.Drawing.Size(371, 355);
            this.BlueRectangleImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BlueRectangleImageBox.TabIndex = 4;
            this.BlueRectangleImageBox.TabStop = false;
            // 
            // RedRectangleImageBox
            // 
            this.RedRectangleImageBox.Location = new System.Drawing.Point(389, 9);
            this.RedRectangleImageBox.Name = "RedRectangleImageBox";
            this.RedRectangleImageBox.Size = new System.Drawing.Size(371, 355);
            this.RedRectangleImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RedRectangleImageBox.TabIndex = 5;
            this.RedRectangleImageBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 370);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Performance";
            // 
            // ShapeDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 392);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RedRectangleImageBox);
            this.Controls.Add(this.BlueRectangleImageBox);
            this.Controls.Add(this.originalImageBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.label1);
            this.Name = "ShapeDetection";
            this.Text = "ShapeDetection";
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BlueRectangleImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedRectangleImageBox)).EndInit();
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
    }
}