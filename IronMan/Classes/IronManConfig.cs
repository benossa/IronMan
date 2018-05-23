//using IronMan.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan
{
    public class IronManConfig
    {
        public int Type1HueMin { get; set; }
        public int Type1HueMax { get; set; }
        public int Type1ValMin { get; set; }
        public int Type1ValMax { get; set; }
        public int Type2HueMin { get; set; }
        public int Type2HueMax { get; set; }
        public int Type2ValMin { get; set; }
        public int Type2ValMax { get; set; }

        public int CameraBrightness { get; set; }
        public int CameraContrast { get; set; }
        public int CameraSharpness { get; set; }
               
        public int RobotShapeX { get; set; }
        public int RobotShapeY { get; set; }
        public int RobotShapeWidth { get; set; }
        public int RobotShapeHeight { get; set; }
               
        //WA = working Area
        public Point WAMaxStart { get; set; }
        public Point WAMaxMiddle { get; set; }
        public Point WAMaxEnd { get; set; }
               
        public Point WAMinStart { get; set; }
        public Point WAMinMiddle { get; set; }
        public Point WAMinEnd { get; set; }

        public int SizeTresholdMin { get; set; }
        public int SizeTresholxMax { get; set; }
        public int CannyTreshold { get; set; }
        public int CannyTresholdLinking { get; set; }
               
        public int ComPortIndex { get; set; }
        public int BaudRateIndex { get; set; }

        public bool UsePreviousValues { get; set; }

        public int Servo1Def { get; set; }
        public int Servo2Def { get; set; }
        public int Servo3Def { get; set; }
        public int Servo4Def { get; set; }
        public int Servo5Def { get; set; }

        public int Servo1Min { get; set; }
        public int Servo2Min { get; set; }
        public int Servo3Min { get; set; }
        public int Servo4Min { get; set; }
        public int Servo5Min { get; set; }

        public int Servo1Max { get; set; }
        public int Servo2Max { get; set; }
        public int Servo3Max { get; set; }
        public int Servo4Max { get; set; }
        public int Servo5Max { get; set; }

        public int Servo1Val { get; set; }
        public int Servo2Val { get; set; }
        public int Servo3Val { get; set; }
        public int Servo4Val { get; set; }
        public int Servo5Val { get; set; }
    }
}
