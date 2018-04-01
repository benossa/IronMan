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
        private string SerialData { get; set; }

        private bool UseRemote { get; set; }

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            
            cbSerialPort.SelectedIndex = 3;
            cbBaudRate.SelectedIndex = 1;
            SerialData = "";
            //SetupTimer();
            //Thread TimerThread = new Thread(new ThreadStart(SetupTimer));
            //TimerThread.IsBackground = true;
            //TimerThread.Start();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            serial.Close();
        }

        private void ReadSerialData(object sender, SerialDataReceivedEventArgs e)
        {
            if (!serial.IsOpen) return;
            SerialData = serial.ReadLine();

            if (SerialData.Contains("CH1Input") && UseRemote)
                SendCommands(1,SerialData.Substring(SerialData.IndexOf(":") + 1));


            if (SerialData.Contains("CH2Input") && UseRemote)
                SendCommands(2,SerialData.Substring(SerialData.IndexOf(":") + 1));

        }

        private void btnOpenSerial_Click(object sender, EventArgs e)
        {
            OpenSerialPort();
        }

        private void btnCloseSerial_Click(object sender, EventArgs e)
        {
            if (serial.IsOpen)
                serial.Close();
            lblPortStatus.Text = serial.IsOpen ? "Open" : "Closed";
        }

        #region 
        private void Servo1Scroll_ValueChanged(object sender, EventArgs e)
        {
            lblServo1Value.Text = "Servo 1: " + Servo1Scroll.Value.ToString();
        }

        private void Servo2Scroll_ValueChanged(object sender, EventArgs e)
        {
            lblServo2Value.Text = "Servo 2: " + Servo2Scroll.Value.ToString();
        }

        private void Servo1Scroll_MouseCaptureChanged(object sender, EventArgs e)
        {
            SendCommands(1, Servo1Scroll.Value.ToString());
        }

        private void Servo2Scroll_MouseCaptureChanged(object sender, EventArgs e)
        {
            SendCommands(2, Servo2Scroll.Value.ToString());
        }
    #endregion Namjestanje interfejsa

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

        private void OpenSerialPort()
        {
            if (serial != null)
                if (serial.IsOpen)
                    serial.Close();

            serial = new SerialPort(cbSerialPort.Text, int.Parse(cbBaudRate.Text),Parity.None,8,StopBits.One);
            serial.Open();
            serial.DiscardOutBuffer();
            serial.DiscardInBuffer();
            serial.DataReceived += ReadSerialData;
            SerialData = "";
            lblPortStatus.Text = serial.IsOpen ? "Opened" : "Closed";
        }

        private void SendCommands(int ServoNumber, string Value)
        {
            if(ServoNumber == 1)
            {
                serial.WriteLine(Value);
                serial.WriteLine("0");
            }
            else if(ServoNumber == 2)
            {
                serial.WriteLine("0");
                serial.WriteLine(Value);
            }
        }

        
    }
}
