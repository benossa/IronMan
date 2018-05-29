using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIS
{
    public class PickupObject
    {
        public string Type { get; set; }
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public double Size { get; set; }
        public float Angle { get; set; }
        public bool InRange { get; set; }
    }
}
