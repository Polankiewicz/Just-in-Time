using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SkinnedModel;
namespace WindowsGame1
{
    class DynamicModel
    {
        private GraphicsDevice device;
        private Color[] floorColors = new Color[2] { Color.White, Color.Black };
        Model model;
        Matrix position;
        Matrix rotation;
        private Vector3 offset;

        float scale = 0.005f;
        String objectName;

        AnimationPlayer enemy;// This calculates the Matrices of the animation
        AnimationClip enemyClip;// This contains the keyframes of the animation
        SkinningData enemySkin;// This contains all the skinning data

        public String Name
        {
            get { return objectName;  }
        }

        public Vector3 Position
        {
            get { return offset; }
            set { offset = value; }
        }

        public Matrix Model
        {
            get { return position; }
            set { position = value; }
        }

        public DynamicModel(GraphicsDevice device, Model model, Vector3 position, Vector3 rotationDegrees, float scale, String objectName)
        {
            this.position = Matrix.Identity;
            this.device = device;
            this.model = model;
            this.scale = scale;
            this.objectName = objectName;

            offset.X = position.X;
            offset.Y = position.Y;
            offset.Z = position.Z;
            this.position = Matrix.CreateTranslation(position);
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
            Matrix[] boneTransforms = new Matrix[model.Bones.Count];

            model.CopyAbsoluteBoneTransformsTo(boneTransforms);  
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect a in mesh.Effects)
                {
                    a.EnableDefaultLighting();
                    a.SetBoneTransforms(bones);
                    a.World = boneTransforms[mesh.ParentBone.Index] *  Matrix.CreateScale(scale) * this.rotation * this.position;// Matrix.CreateTranslation(offset);
                    a.View = camera.View;
                    a.Projection = camera.Projection;
                    
                    a.DirectionalLight0.DiffuseColor = new Vector3(0.4f, 0.4f, 0.49f); // a red light
                    a.DirectionalLight0.Direction = new Vector3(-1, -1, 0.75f);  // coming along the x-axis
                    a.DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);
                    a.AmbientLightColor = new Vector3(0.05f, 0.05f, 0.05f);
                    a.SpecularColor = new Vector3(0.25f);
                    a.SpecularPower = 16;
                }

                mesh.Draw();
            }

        }

    }
}
