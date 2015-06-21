using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace WindowsGame1
{
    public class Mirror
    {


        private GraphicsDevice device;
        private Color[] floorColors = new Color[2] { Color.White, Color.Black };
        Model model;
        Matrix position = Matrix.Identity;
        Matrix rotation;
        private Vector3 offset;
        public Material Material { get; set; }
        String objectName;
        Vector3 rotationVector;
        public Texture2D shadowMap { get; set; }
        public Texture2D reflectionMap { get; set; }
        BasicEffect back;
        public Model fbxModel
        {
            get { return model; }
            set { model = value; }
        }
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
        public float ambientPower { get; set; }
        public float lightPower { get; set; }
        Matrix lightsViewProjectionMatrix;
        public Vector3 lightPos { get; set; }
        public void UpdateLightData()
        {
            Matrix lightsView = Matrix.CreateLookAt(lightPos, new Vector3(0, 3, 0), new Vector3(0, 1, 0));
            Matrix lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 5f, 100f);

            lightsViewProjectionMatrix = lightsView * lightsProjection;
        }
        public float Scale { get; set; }
        public string path { get; set; }
        public Mirror(GraphicsDevice device, Model model, Vector3 position, Vector3 rotationDegrees, float scale, string objectName, string path)
        {
            this.device = device;
            this.model = model;
            this.Scale = scale;
            this.objectName = objectName;
            this.path = path;
            this.offset = position;
            this.rotationVector = rotationDegrees;
            this.Material = new Material();
            this.reflectionMap = new Texture2D(device,2048,2048);
            lightPower = 2f;
            ambientPower = 0.2f;
            lightPos = new Vector3(-20, 20, -20);
            lightPower = 1.2f;
            GenerateTags();
        }
        public void CreateShadowMaps()
        {
            this.shadowMap = new Texture2D(device, 4096, 4096);
        }
        public void SetCustomEffect(Effect effect, bool force = false)
        {
            UpdateLightData();
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toSet = (Effect)effect.Clone();

                    MeshTag tag = ((MeshTag)part.Tag);
                    Material.SetEffectParameters(effect);
                    
                        toSet.SetEffectParameter("xTexture", reflectionMap);

                        toSet.SetEffectParameter("TextureEnabled", true);

                        toSet.SetEffectParameter("DiffuseColor", tag.Color);
                        toSet.SetEffectParameter("SpecularPower", tag.SpecularPower);

                        toSet.Parameters["xLightPos"].SetValue(lightPos);
                        toSet.Parameters["xLightPower"].SetValue(lightPower);
                        toSet.Parameters["xAmbient"].SetValue(ambientPower);
                        toSet.Parameters["xLightsWorldViewProjection"].SetValue(Matrix.Identity * lightsViewProjectionMatrix);


                        part.Effect = toSet;
                    }
            

                
        }
        public void RefreshTexture(Texture2D tex)
        {
            foreach (ModelMesh mesh in model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    part.Effect.SetEffectParameter("xTexture", tex);

        }

        public void Draw(Camera camera, string tech = "Simplest")
        {

            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    Effect effect = meshPart.Effect;

                    this.rotation = Matrix.CreateRotationX(MathHelper.ToRadians(rotationVector.X))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotationVector.Y))
                        * Matrix.CreateRotationZ(MathHelper.ToRadians(rotationVector.Z));

                   
                        effect.CurrentTechnique = effect.Techniques[tech];
                        Matrix worldMatrix = transforms[mesh.ParentBone.Index] * this.rotation * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(offset);

                        effect.SetEffectParameter("xWorld", worldMatrix);
                        effect.SetEffectParameter("xWorldViewProjection", transforms[mesh.ParentBone.Index] * this.rotation * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(offset) * camera.View * camera.Projection);


                        effect.Parameters["xLightPos"].SetValue(lightPos);
                        effect.Parameters["xLightPower"].SetValue(lightPower);
                        effect.Parameters["xAmbient"].SetValue(ambientPower);
                        effect.Parameters["xLightsWorldViewProjection"].SetValue(worldMatrix * lightsViewProjectionMatrix);
                        effect.Parameters["xShadowMap"].SetValue(shadowMap);

                    
                }

                mesh.Draw();
            }
        }



    }
}
