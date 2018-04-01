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
        private string SerialLine { get; set; }
        private System.Timers.Timer timmer { get; set; }

        private bool UseRemote { get; set; }
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            
            serial = new SerialPort();
            cbSerialPort.SelectedIndex = 3;
            cbBaudRate.SelectedIndex = 1;
            SerialLine = "";
            SetupTimer();
            //Thread TimerThread = new Thread(new ThreadStart(SetupTimer));
            //TimerThread.IsBackground = true;
            //TimerThread.Start();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            serial.Close();
        }

        private void SetupTimer()
        {
            timmer = new System.Timers.Timer();
            timmer.Enabled = true;
            timmer.Interval = 100;
            timmer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Tick);
            //timmer.Interval += new EventHandler(timer_Tick);
            timmer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (!serial.IsOpen) return;
            SerialLine = serial.ReadLine();


            if (SerialLine.Contains("CH1Input"))
            {
                Servo1Set(SerialLine.Substring(SerialLine.IndexOf(":"), SerialLine.IndexOf(";")));
            }
                tbSerialMonitor.Text += SerialLine + Environment.NewLine;
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
            Servo1Set(Servo1Scroll.Value.ToString());
        }

        private void cbUseRemoteControl_CheckedChanged(object sender, EventArgs e)
        {
            
            UseRemote = cbUseRemoteControl.Checked;
            Servo1Scroll.Enabled = !UseRemote;
            Servo2Scroll.Enabled = !UseRemote;
            Servo3Scroll.Enabled = !UseRemote;
            Servo4Scroll.Enabled = !UseRemote;
            Servo5Scroll.Enabled = !UseRemote;
            Servo6Scroll.Enabled = !UseRemote;

            if (!serial.IsOpen) return;
            if (UseRemote)
                serial.WriteLine("RemoteON");
            else
                serial.WriteLine("RemoteOFF");
        }

        private void Servo1Set(string Angle)
        {
            serial.WriteLine(Angle);
        }
    }
}
