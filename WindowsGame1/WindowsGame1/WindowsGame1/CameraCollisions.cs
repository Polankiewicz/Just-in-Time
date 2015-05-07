using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace WindowsGame1
{
   public class CameraCollisions
    {
        Camera camera;
        BoundingSphere cameraBoundingSphere;
        
        List<StaticModel> staticModelsList;
        List<DynamicModel> dynamicModelsList;

        List<BoundingBox> staticBoundingSpheresList = new List<BoundingBox>();
        List<BoundingSphere> dynamicBoundingSpheresList = new List<BoundingSphere>();

        BoundingBox box;

        public CameraCollisions(Camera camera, List<DynamicModel> dynamicModelsList, List<StaticModel> staticModelsList)
        {
            this.camera = camera;
            this.staticModelsList = staticModelsList;
            this.dynamicModelsList = dynamicModelsList;

            cameraBoundingSphere = new BoundingSphere(camera.Position, 0.50f);
            


            for(int i=0; i<dynamicModelsList.Count; i++)
            {
                if(dynamicModelsList[i].Name == "enemy")
                    dynamicBoundingSpheresList.Add(new BoundingSphere(dynamicModelsList[i].Position, 0.50f));
            }
            /*
            for (int i = 0; i < staticModelsList.Count; i++)
            {
                if (staticModelsList[i].Name == "block")
                    staticBoundingSpheresList.Add(new BoundingBox(staticModelsList[i].Position + new Vector3(-20f, 0, 0), 
                        staticModelsList[i].Position + new Vector3(40f,40f,10f)));
            }
            */
        }

        public void updateBoundingSpherePosition()
        {
            for (int i = 0; i < dynamicModelsList.Count; i++)
            {
                if (dynamicModelsList[i].Name == "enemy")
                {
                    BoundingSphere xxx = dynamicBoundingSpheresList[i];
                    xxx.Center = dynamicModelsList[i].Position;
                    dynamicBoundingSpheresList[0] = xxx;
                    //dynamicBoundingSpheresList[0].Center = dynamicModelsList[i].Position; //???
                }
                    
            }
        }

        public bool cameraNextMoveCollisionDetect(Vector3 nextCameraMove)
        {
            cameraBoundingSphere.Center = nextCameraMove;

            // check for collision with dynamic models
            for (int i = 0; i < dynamicBoundingSpheresList.Count; i++)
            {
                if (cameraBoundingSphere.Intersects(dynamicBoundingSpheresList[i]))
                    return false;
            }

            // check for collision with static models
            for (int i = 0; i < staticBoundingSpheresList.Count; i++)
            {
                if (cameraBoundingSphere.Intersects(staticBoundingSpheresList[i]))
                    return false;
            }

            return true;
        }


    }


}
