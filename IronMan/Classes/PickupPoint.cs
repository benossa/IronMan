using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Classes
{
    public class PickupPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public int Servo1Val { get; set; }
        public int Servo2Val { get; set; }
        public int Servo3Val { get; set; }
        public bool IsValid { get; set; }
        public bool HasObject { get; set; }
        public string ObjectType { get; set; }

        public PickupPoint()
        {

        }
        public PickupPoint(int Servo1, int Servo2, int Servo3, bool _IsValid, int _X, int _Y, int _StartX, int _StartY, int _EndX, int _EndY)
        {
            Servo1Val = Servo1;
            Servo2Val = Servo2;
            Servo3Val = Servo3;
            IsValid = _IsValid;
            X = _X;
            Y = _Y;
            StartX = _StartX;
            EndX = _EndX;
            StartY = _StartY;
            EndY = _EndY;
        }
    }
}
