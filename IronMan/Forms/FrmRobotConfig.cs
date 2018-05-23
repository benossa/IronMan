using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronMan.Forms
{
    public partial class FrmRobotConfig : Form
    {
        public IronManConfig IMC { get; set; }
        public FrmRobotConfig(IronManConfig _IMC)
        {
            InitializeComponent();
            IMC = _IMC;
        }

        private void FrmRobotConfig_Load(object sender, EventArgs e)
        {
            tbWASSX.Text = IMC.WAMinStart.X.ToString();
            tbWASMX.Text = IMC.WAMinMiddle.X.ToString();
            tbWASEX.Text = IMC.WAMinEnd.X.ToString();

            tbWASSY.Text = IMC.WAMinStart.Y.ToString();
            tbWASMY.Text = IMC.WAMinMiddle.Y.ToString();
            tbWASEY.Text = IMC.WAMinEnd.Y.ToString();

            tbWAESX.Text = IMC.WAMaxStart.X.ToString();
            tbWAEMX.Text = IMC.WAMaxMiddle.X.ToString();
            tbWAEEX.Text = IMC.WAMaxEnd.X.ToString();

            tbWAESY.Text = IMC.WAMaxStart.Y.ToString();
            tbWAEMY.Text = IMC.WAMaxMiddle.Y.ToString();
            tbWAEEY.Text = IMC.WAMaxEnd.Y.ToString();

            tbSR1.Text = IMC.Servo1Def.ToString();
            tbSR2.Text = IMC.Servo2Def.ToString();
            tbSR3.Text = IMC.Servo3Def.ToString();
            tbSR4.Text = IMC.Servo4Def.ToString();
            tbSR5.Text = IMC.Servo5Def.ToString();
        }

        private void FrmRobotConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            IMC.WAMinStart = new Point(int.Parse(tbWASSX.Text), int.Parse(tbWASSY.Text));
            IMC.WAMinMiddle = new Point(int.Parse(tbWASMX.Text), int.Parse(tbWASMY.Text));
            IMC.WAMinEnd = new Point(int.Parse(tbWASEX.Text), int.Parse(tbWASEY.Text));


            IMC.WAMaxStart  = new Point(int.Parse(tbWAESX.Text), int.Parse(tbWAESY.Text));
            IMC.WAMaxMiddle = new Point(int.Parse(tbWAEMX.Text), int.Parse(tbWAEMY.Text));
            IMC.WAMaxEnd   = new Point(int.Parse(tbWAEEX.Text), int.Parse(tbWAEEY.Text));

            IMC.Servo1Def = int.Parse(tbSR1.Text);
            IMC.Servo2Def  = int.Parse(tbSR2.Text);
            IMC.Servo3Def   = int.Parse(tbSR3.Text);
            IMC.Servo4Def    = int.Parse(tbSR4.Text);
            IMC.Servo5Def = int.Parse(tbSR5.Text);
        }

    }
}
