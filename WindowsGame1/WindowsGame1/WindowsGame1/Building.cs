﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace WindowsGame1
{
    class Building
    {
        
        
        private GraphicsDevice device;
        private Color[] floorColors = new Color[2] { Color.White, Color.Black };
        Model model;
        Matrix position = Matrix.Identity;
        Matrix rotation;
        private Vector3 offset;
        float scale = 0.005f;

        public Vector3 Position
        {
            get { return offset; }
        }

        public Building(GraphicsDevice device, Model model, Vector3 position, Vector3 rotationDegrees, float scale)
        {
            this.device = device;
            this.model = model;
            this.scale = scale;

            offset.X = position.X;
            offset.Y = position.Y;
            offset.Z = position.Z;
            this.rotation =  Matrix.CreateRotationX(MathHelper.ToRadians(rotationDegrees.X))
                            * Matrix.CreateRotationY(MathHelper.ToRadians(rotationDegrees.Y))
                            * Matrix.CreateRotationZ(MathHelper.ToRadians(rotationDegrees.Z));
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
                    effect.World = transforms[mesh.ParentBone.Index] * this.rotation * Matrix.CreateScale(scale) * Matrix.CreateTranslation(offset);
                    
                    effect.View = camera.View;

                    effect.Projection = camera.Projection;
                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }
                mesh.Draw();
                // hehe test
            }

        }

       



    }
    
}
