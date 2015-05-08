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
        public SceneSaveData(string path, string Name, float scale, Vector3 positon, Vector3 rotation)
        {
            this.Scale = scale;
            this.Position = positon;
            this.Rotation = rotation;
            this.Name = Name;
            this.path = path;
        }
    }
}
