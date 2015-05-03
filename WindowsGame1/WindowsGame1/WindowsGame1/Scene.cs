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
    class Scene
    {
        Dictionary<string,Model> fbxList;
        List<StaticModel> staticModelsList;
        List<DynamicModel> dynamicModelsList;
        
        ContentManager content {get; set; }

        GraphicsDevice graphicsDevice;
        Camera camera;

        public Scene(ContentManager content, GraphicsDevice GraphicsDevice, Camera Camera)
        {
            this.content = content;
            this.graphicsDevice = GraphicsDevice;
            this.camera = Camera;
            this.staticModelsList = new List<StaticModel>();
            this.dynamicModelsList = new List<DynamicModel>();
            this.fbxList = new Dictionary<string, Model>();
        }

        public List<DynamicModel> getDynamicModelsList()
        {
            return this.dynamicModelsList;
        }

        public List<StaticModel> getStaticModelsList()
        {
            return this.staticModelsList;
        }

        public void Update(GameTime gameTime)
        {
            foreach (DynamicModel n in dynamicModelsList)
            {
                n.Update(gameTime);
            }
        }

        public void AddStaticModel(string assetName, Vector3 Positon, Vector3 Rotation, float Scale)
        {
            if (!fbxList.ContainsKey(assetName)) 
                fbxList.Add(assetName, content.Load<Model>(assetName));
            
            this.staticModelsList.Add(new StaticModel(graphicsDevice,fbxList[assetName], Positon,Rotation,Scale));
        }

        public void AddDynamicModel(string assetName, Vector3 Positon, Vector3 Rotation, float Scale)
        {
            if (!fbxList.ContainsKey(assetName))
                fbxList.Add(assetName, content.Load<Model>(assetName));

            this.dynamicModelsList.Add(new DynamicModel(graphicsDevice, fbxList[assetName], Positon, Rotation, Scale));
        }

        public void Draw()
        {
            foreach (DynamicModel n in dynamicModelsList)
                n.Draw(camera);

            foreach (StaticModel n in staticModelsList)
                n.Draw(camera);
        }

        
    }

   
}
