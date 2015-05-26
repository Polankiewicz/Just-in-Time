using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
   public class DrawableBoundingBox
    {
        public Vector3 min {get;set;}
        public Vector3 max { get; set; }

        GraphicsDevice device;
        GeometricPrimitive primitive;
        BoundingBox boundingBox;

        public DrawableBoundingBox(GraphicsDevice device, Vector3 min, Vector3 max)
        {
            this.device = device;
            this.min = min;
            this.max = max;

            boundingBox = new BoundingBox(this.min, this.max);
            primitive = new WireBox(this.device, this.min, this.max);

        }

       public void Draw(Camera camera)
       {
           primitive.Draw(camera);
       }

    }
}
