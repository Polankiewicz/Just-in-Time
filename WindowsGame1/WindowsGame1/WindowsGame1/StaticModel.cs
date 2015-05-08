using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace WindowsGame1
{
    public class StaticModel
    {
        
        
        private GraphicsDevice device;
        private Color[] floorColors = new Color[2] { Color.White, Color.Black };
        Model model;
        Matrix position = Matrix.Identity;
        Matrix rotation;
        private Vector3 offset;
        
        String objectName;
        Vector3 rotationVector;
      
        public String Name
        {
            get { return objectName; }
            set { objectName = value; }
        }

        public Vector3 Position
        {
            get { return offset; }
            set { offset = value; }
        }
        public Vector3 Rotation
        {
            get { return rotationVector; }
            set { rotationVector = value; }
        }
        public float Scale { get; set; }
        public string path { get; set; }
        public StaticModel(GraphicsDevice device, Model model, Vector3 position, Vector3 rotationDegrees, float scale, string objectName,string path)
        {
            this.device = device;
            this.model = model;
            this.Scale = scale;
            this.objectName = objectName;
            this.path = path;
            this.offset = position;
            this.rotationVector = rotationDegrees;
        }
        public StaticModel()
        {
            this.device = null;
            this.model = null;
            this.Scale = 0;
            this.objectName = null;

            this.offset = new Vector3();
            this.rotationVector = new Vector3();
        }
        //build our vertex buffer
        public void Draw(Camera camera)
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
           
            foreach (ModelMesh mesh in model.Meshes)
            {

                foreach (BasicEffect effect in mesh.Effects)
                {
                    // effect.EnableDefaultLighting();
                    this.rotation = Matrix.CreateRotationX(MathHelper.ToRadians(rotationVector.X))
                          * Matrix.CreateRotationY(MathHelper.ToRadians(rotationVector.Y))
                          * Matrix.CreateRotationZ(MathHelper.ToRadians(rotationVector.Z));
                    effect.World = transforms[mesh.ParentBone.Index] * this.rotation * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(offset);

                    effect.View = camera.View;

                    effect.LightingEnabled = true; // turn on the lighting subsystem.
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.4f, 0.4f, 0.49f); // a red light
                    effect.DirectionalLight0.Direction = new Vector3(-1, -1, 0.75f);  // coming along the x-axis
                    effect.DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);
                    effect.AmbientLightColor = new Vector3(0.05f, 0.05f, 0.05f);
                    effect.Projection = camera.Projection;

                }
                mesh.Draw();
                // hehe test
            }

        }

       



    }
    
}
