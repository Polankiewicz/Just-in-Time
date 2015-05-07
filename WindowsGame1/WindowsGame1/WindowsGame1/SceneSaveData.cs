using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace WindowsGame1
{
    public class SceneSaveData
    {
        public float Scale { get; set; }
        public string Name { get; set; }
        public string path { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public SceneSaveData()
        {

        }
    }
}
