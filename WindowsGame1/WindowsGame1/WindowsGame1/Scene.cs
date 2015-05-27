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
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;

namespace WindowsGame1
{
    
    public class Scene
    {
        Dictionary<string,Model> fbxList;
        public List<StaticModel> staticModelsList;
        public List<DynamicModel> dynamicModelsList;

        public List<DrawableBoundingBox> boundingBoxesList;
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
            this.boundingBoxesList = new List<DrawableBoundingBox>();
        }
        
        public Scene()
        {
            this.content = null;
            this.graphicsDevice = null;
            this.camera = null;
            this.staticModelsList = new List<StaticModel>();
            this.dynamicModelsList = new List<DynamicModel>();
            this.fbxList = new Dictionary<string, Model>();
            this.boundingBoxesList = new List<DrawableBoundingBox>();
        }

        public List<DynamicModel> getDynamicModelsList()
        {
            return this.dynamicModelsList;
        }

        public List<StaticModel> getStaticModelsList()
        {
            return this.staticModelsList;
        }

        public List<DrawableBoundingBox> getBoundingBoxesList()
        {
            return this.boundingBoxesList;
        }

        public void Update(GameTime gameTime)
        {
            foreach (DynamicModel n in dynamicModelsList)
            {
                n.Update(gameTime);
            }
        }
        
        public void AddStaticModel(string assetName, Vector3 Positon, Vector3 Rotation, float Scale, string objectName)
        {
            if (!fbxList.ContainsKey(assetName)) 
                fbxList.Add(assetName, content.Load<Model>(assetName));

            this.staticModelsList.Add(new StaticModel(graphicsDevice, fbxList[assetName], Positon, Rotation, Scale, objectName,assetName));
        }

        public void AddDynamicModel(string assetName, Vector3 Positon, Vector3 Rotation, float Scale, String objectName)
        {
            if (!fbxList.ContainsKey(assetName))
                fbxList.Add(assetName, content.Load<Model>(assetName));

            this.dynamicModelsList.Add(new DynamicModel(graphicsDevice, fbxList[assetName], Positon, Rotation, Scale, objectName));
        }
        public void AddBoundingBox(Vector3 min, Vector3 max)
        {
            this.boundingBoxesList.Add(new DrawableBoundingBox(graphicsDevice,min, max));
        }
        public void Draw()
        {
            foreach (DynamicModel n in dynamicModelsList)
                n.Draw(camera);

            foreach (StaticModel n in staticModelsList)
                n.Draw(camera);
        }

        public void LoadFromOldXML(string path)
        {
       
            XmlConverter<List<SceneSaveData>> x = new XmlConverter<List<SceneSaveData>>();

            List<SceneSaveData> dataList = x.Deserialize(path);

            foreach (SceneSaveData n in dataList)
                this.AddStaticModel(n.path, n.Position, n.Rotation, n.Scale, n.Name);
        }
 
    }

   
}
