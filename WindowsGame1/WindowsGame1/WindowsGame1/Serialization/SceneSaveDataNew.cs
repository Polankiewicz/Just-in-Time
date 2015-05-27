using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace WindowsGame1
{
    public class SceneSaveDataNew
    {
        public List<SceneSaveData> modelsList;
        public List<BoundingBoxSaveData> boundingBoxesList;
        public SceneSaveDataNew() {
        modelsList = new List<SceneSaveData>();
        boundingBoxesList = new List<BoundingBoxSaveData>();
        }

        public void AddModel(string path, string Name, float scale, Vector3 positon, Vector3 rotation)
        {

            modelsList.Add(new SceneSaveData(path, Name, scale, positon, rotation));

        }
        public void AddBoundingBox(Vector3 min, Vector3 max, String name)
        {
            boundingBoxesList.Add(new BoundingBoxSaveData(min, max, name));
        }


    }
}
