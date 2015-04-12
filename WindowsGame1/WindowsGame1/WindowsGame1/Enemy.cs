﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SkinnedModel;
namespace WindowsGame1
{
    class Enemy
    {
        private GraphicsDevice device;
        private Color[] floorColors = new Color[2] { Color.White, Color.Black };
        Model model;
        Matrix position = Matrix.Identity;
        Matrix rotation;
        private Vector3 offset;
        float scale = 0.005f;

        AnimationPlayer enemy;// This calculates the Matrices of the animation
        AnimationClip enemyClip;// This contains the keyframes of the animation
        SkinningData enemySkin;// This contains all the skinning data

        public Vector3 Position
        {
            get { return offset; }
        }

        public Enemy(GraphicsDevice device, Model model, Vector3 position, Vector3 rotationDegrees, float scale)
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

            enemySkin = model.Tag as SkinningData;

            enemy = new AnimationPlayer(enemySkin);
            enemyClip = enemySkin.AnimationClips["Take 001"];
            enemy.StartClip(enemyClip);
        }
        public void Update(GameTime gameTime)
        {
            enemy.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
        }

        //build our vertex buffer,\
        public void Draw(Camera camera)
        {
            Matrix[] bones = enemy.GetSkinTransforms();

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect a in mesh.Effects)
                {
                    a.EnableDefaultLighting();
                    a.SetBoneTransforms(bones);
                    a.World = this.rotation * Matrix.CreateScale(scale) * Matrix.CreateTranslation(offset);
                    a.View = camera.View;
                    a.Projection = camera.Projection;

                    a.SpecularColor = new Vector3(0.25f);
                    a.SpecularPower = 16;
                }

                mesh.Draw();
            }

        }

    }
}
