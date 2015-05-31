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
        private void GenerateTags()
        {
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (part.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect)part.Effect;
                        MeshTag tag = new MeshTag(effect.DiffuseColor, effect.Texture,
                        effect.SpecularPower);
                        part.Tag = tag;
                    }
        }
        public float Scale { get; set; }
        public string path { get; set; }
        public StaticModel(GraphicsDevice device, Model model, Vector3 position, Vector3 rotationDegrees, float scale, string objectName, string path)
        {
            this.device = device;
            this.model = model;
            this.Scale = scale;
            this.objectName = objectName;
            this.path = path;
            this.offset = position;
            this.rotationVector = rotationDegrees;
            GenerateTags();
        }
        public void SetCustomEffect(Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toSet = effect;

                    MeshTag tag = ((MeshTag)part.Tag);

                    if (tag.Texture != null)
                    {
                        toSet.SetEffectParameter("BasicTexture", tag.Texture);
                        toSet.SetEffectParameter("TextureEnabled", true);
                       
                    }
                    else
                        toSet.SetEffectParameter("TextureEnabled", true);

                    toSet.SetEffectParameter("DiffuseColor",tag.Color);
                    toSet.SetEffectParameter("SpecularPower",tag.SpecularPower);

                    part.Effect = toSet;
                }
        }
        

        //build our vertex buffer
        public void Draw(Camera camera)
        {
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    Effect effect = meshPart.Effect;
                    if (effect is BasicEffect)
                    {
                        this.rotation = Matrix.CreateRotationX(MathHelper.ToRadians(rotationVector.X))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotationVector.Y))
                        * Matrix.CreateRotationZ(MathHelper.ToRadians(rotationVector.Z));
                        ((BasicEffect)effect).World = transforms[mesh.ParentBone.Index] * this.rotation * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(offset);

                        ((BasicEffect)effect).View = camera.View;

                        ((BasicEffect)effect).LightingEnabled = true; // turn on the lighting subsystem.
                        ((BasicEffect)effect).DirectionalLight0.DiffuseColor = new Vector3(0.4f, 0.4f, 0.49f); // a red light
                        ((BasicEffect)effect).DirectionalLight0.Direction = new Vector3(-1, -1, 0.75f);  // coming along the x-axis
                        ((BasicEffect)effect).DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);
                        ((BasicEffect)effect).AmbientLightColor = new Vector3(0.05f, 0.05f, 0.05f);
                        ((BasicEffect)effect).Projection = camera.Projection;
                    }
                    else
                    {
                        effect.SetEffectParameter("World",
                                transforms[mesh.ParentBone.Index] * this.rotation * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(offset)
                            );
                        effect.SetEffectParameter("View",camera.View);
                        effect.SetEffectParameter("Projection",camera.Projection);
                        effect.SetEffectParameter("CameraPosition", camera.Position);

                    }
                }

                mesh.Draw();
            }
        }
    }
}
