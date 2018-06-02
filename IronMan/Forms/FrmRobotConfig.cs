using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRIS.Forms
{
    public partial class FrmRobotConfig : Form
    {
        public IRISConfig IRC { get; set; }
        public FrmRobotConfig(IRISConfig _IRC)
        {
            InitializeComponent();
            IRC = _IRC;
        }

        private void FrmRobotConfig_Load(object sender, EventArgs e)
        {
            tbWASSX.Text = IRC.WAMinStart.X.ToString();
            tbWASMX.Text = IRC.WAMinMiddle.X.ToString();
            tbWASEX.Text = IRC.WAMinEnd.X.ToString();

            tbWASSY.Text = IRC.WAMinStart.Y.ToString();
            tbWASMY.Text = IRC.WAMinMiddle.Y.ToString();
            tbWASEY.Text = IRC.WAMinEnd.Y.ToString();

            tbWAESX.Text = IRC.WAMaxStart.X.ToString();
            tbWAEMX.Text = IRC.WAMaxMiddle.X.ToString();
            tbWAEEX.Text = IRC.WAMaxEnd.X.ToString();

            tbWAESY.Text = IRC.WAMaxStart.Y.ToString();
            tbWAEMY.Text = IRC.WAMaxMiddle.Y.ToString();
            tbWAEEY.Text = IRC.WAMaxEnd.Y.ToString();

            tbSR1.Text = IRC.ServoDefaultPosition.Servo1Val.ToString();
            tbSR2.Text = IRC.ServoDefaultPosition.Servo2Val.ToString();
            tbSR3.Text = IRC.ServoDefaultPosition.Servo3Val.ToString();
            tbSR4.Text = IRC.ServoDefaultPosition.Servo4Val.ToString();
            tbSR5.Text = IRC.ServoDefaultPosition.Servo5Val.ToString();

            tbRobotPosX.Text = IRC.RobotShapeX.ToString();
            tbRobotPosY.Text = IRC.RobotShapeY.ToString();
            tbRobotPosWidth.Text = IRC.RobotShapeWidth.ToString();
            tbRobotPosHeight.Text = IRC.RobotShapeHeight.ToString();

            tbDrop1S1.Text = IRC.DropType1Position.Servo1Val.ToString();
            tbDrop1S2.Text = IRC.DropType1Position.Servo2Val.ToString();
            tbDrop1S3.Text = IRC.DropType1Position.Servo3Val.ToString();
            tbDrop1S4.Text = IRC.DropType1Position.Servo4Val.ToString();
            tbDrop1S5.Text = IRC.DropType1Position.Servo5Val.ToString();

            tbDrop2S1.Text = IRC.DropType2Position.Servo1Val.ToString();
            tbDrop2S2.Text = IRC.DropType2Position.Servo2Val.ToString();
            tbDrop2S3.Text = IRC.DropType2Position.Servo3Val.ToString();
            tbDrop2S4.Text = IRC.DropType2Position.Servo4Val.ToString();
            tbDrop2S5.Text = IRC.DropType2Position.Servo5Val.ToString();
        }

        private void FrmRobotConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            IRC.WAMinStart = new Point(int.Parse(tbWASSX.Text), int.Parse(tbWASSY.Text));
            IRC.WAMinMiddle = new Point(int.Parse(tbWASMX.Text), int.Parse(tbWASMY.Text));
            IRC.WAMinEnd = new Point(int.Parse(tbWASEX.Text), int.Parse(tbWASEY.Text));


            IRC.WAMaxStart  = new Point(int.Parse(tbWAESX.Text), int.Parse(tbWAESY.Text));
            IRC.WAMaxMiddle = new Point(int.Parse(tbWAEMX.Text), int.Parse(tbWAEMY.Text));
            IRC.WAMaxEnd   = new Point(int.Parse(tbWAEEX.Text), int.Parse(tbWAEEY.Text));

            IRC.ServoDefaultPosition.Servo1Val = int.Parse(tbSR1.Text);
            IRC.ServoDefaultPosition.Servo2Val = int.Parse(tbSR2.Text);
            IRC.ServoDefaultPosition.Servo3Val = int.Parse(tbSR3.Text);
            IRC.ServoDefaultPosition.Servo4Val = int.Parse(tbSR4.Text);
            IRC.ServoDefaultPosition.Servo5Val = int.Parse(tbSR5.Text);

            IRC.RobotShapeX = int.Parse(tbRobotPosX.Text);
            IRC.RobotShapeY = int.Parse(tbRobotPosY.Text);
            IRC.RobotShapeWidth = int.Parse(tbRobotPosWidth.Text);
            IRC.RobotShapeHeight = int.Parse(tbRobotPosHeight.Text);

            IRC.DropType1Position.Servo1Val = int.Parse(tbDrop1S1.Text);
            IRC.DropType1Position.Servo2Val = int.Parse(tbDrop1S2.Text);
            IRC.DropType1Position.Servo3Val = int.Parse(tbDrop1S3.Text);
            IRC.DropType1Position.Servo4Val = int.Parse(tbDrop1S4.Text);
            IRC.DropType1Position.Servo5Val = int.Parse(tbDrop1S5.Text);

            IRC.DropType2Position.Servo1Val = int.Parse(tbDrop2S1.Text);
            IRC.DropType2Position.Servo2Val = int.Parse(tbDrop2S2.Text);
            IRC.DropType2Position.Servo3Val = int.Parse(tbDrop2S3.Text);
            IRC.DropType2Position.Servo4Val = int.Parse(tbDrop2S4.Text);
            IRC.DropType2Position.Servo5Val = int.Parse(tbDrop2S5.Text);
        }

    }
}
