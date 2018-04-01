using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronMan
{

    public partial class Main : Form
    {
        private SerialPort serial { get; set; }
        private System.Windows.Forms.Timer timmer { get; set; }
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            timmer = new System.Windows.Forms.Timer();
            serial = new SerialPort();
            cbSerialPort.SelectedIndex = 3;
            cbBaudRate.SelectedIndex = 1;

            //timmer.Enabled = true;
            //timmer.Interval = 1000;
            //timmer.Tick += new EventHandler(timer_Tick);
            //timmer.Start();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            serial.Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (serial.IsOpen)
                tbSerialMonitor.Text += serial.ReadLine() + Environment.NewLine;
        }

        private void btnOpenSerial_Click(object sender, EventArgs e)
        {
            if (!serial.IsOpen)
            {
                serial.BaudRate = int.Parse(cbBaudRate.Text);
                serial.PortName = cbSerialPort.Text;
                serial.Open();

                // posalji default vrijednosti 
                serial.WriteLine(Servo1Scroll.Value.ToString());
            }
            lblPortStatus.Text = serial.IsOpen ? "Opened" : "Closed";
        }

        private void btnCloseSerial_Click(object sender, EventArgs e)
        {
            if (serial.IsOpen)
                serial.Close();
            lblPortStatus.Text = serial.IsOpen ? "Open" : "Closed";
        }

        private void Servo1Scroll_ValueChanged(object sender, EventArgs e)
        {
            lblServo1Value.Text = "Servo 1: " + Servo1Scroll.Value.ToString();
        }

        private void Servo1Scroll_MouseCaptureChanged(object sender, EventArgs e)
        {
            serial.WriteLine(Servo1Scroll.Value.ToString());
        }
    }
}
