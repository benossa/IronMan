//using IRIS.Classes;
using IRIS.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIS
{
    public class IRISConfig
    {
        public int Type1HueMin { get; set; }
        public int Type1HueMax { get; set; }
        public int Type1ValMin { get; set; }
        public int Type1ValMax { get; set; }
        public int Type2HueMin { get; set; }
        public int Type2HueMax { get; set; }
        public int Type2ValMin { get; set; }
        public int Type2ValMax { get; set; }

        public double PixelsPerCM { get; set; }
        public double BaseLengthMM { get; set; }
        public double BaseHeightMM { get; set; }
        public double ArmLengthMM { get; set; }

        public int CameraBrightness { get; set; }
        public int CameraContrast { get; set; }
        public int CameraSharpness { get; set; }
               
        public int RobotShapeX { get; set; }
        public int RobotShapeY { get; set; }
        public int RobotShapeCenterRotation { get; set; }
        public int RobotShapeWidth { get; set; }
        public int RobotShapeHeight { get; set; }
        public PickupPoint PickupPointStart { get; set; }
        public PickupPoint PickupPointEnd { get; set; }
        public int PickupPointSamples { get; set; }
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
        public int CameraSpeedms { get; set; }

        public int MatrixSizeX { get; set; }
        public int MatrixSizeY { get; set; }

        public bool UsePreviousValues { get; set; }
        public bool UseCalibrating { get; set; }

        public PickupPoint ServoDefaultPosition { get; set; }
        public PickupPoint ServoMinPosition { get; set; }
        public PickupPoint ServoMaxPosition { get; set; }
        public PickupPoint ServoPreviousPosition { get; set; }
        public PickupPoint DropType1Position { get; set; }
        public PickupPoint DropType2Position { get; set; }
    }
}
