using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace WindowsGame1
{
    public class BoundingBoxSaveData
    {

        public Vector3 min { get; set; }
        public Vector3 max { get; set; }

        public BoundingBoxSaveData() { }
        public BoundingBoxSaveData(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

    }
}
