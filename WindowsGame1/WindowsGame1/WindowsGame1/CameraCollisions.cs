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
        List<DrawableBoundingBox> boundingBoxesList;

        Scene actualScene;

        // TEMP lists
        List<BoundingBox> staticBoundingSpheresList = new List<BoundingBox>();
        List<BoundingSphere> dynamicBoundingSpheresList = new List<BoundingSphere>();

        public CameraCollisions(Camera camera, Scene actualScene)
        {
            this.camera = camera;
            this.staticModelsList = actualScene.getStaticModelsList();
            this.dynamicModelsList = actualScene.getDynamicModelsList();
            this.boundingBoxesList = actualScene.getBoundingBoxesList();
            this.actualScene = actualScene;

            cameraBoundingSphere = new BoundingSphere(camera.Position, 0.50f);

            // TEMP - collision for dynamic models
            for (int i = 0; i < dynamicModelsList.Count; i++)
            {
                if (dynamicModelsList[i].Name == "enemy")
                    dynamicBoundingSpheresList.Add(new BoundingSphere(dynamicModelsList[i].Position, 0.50f));
            }

        }

        // TEMP - update enemy position
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

            // check for collision with boundingBoxes from DrawableBoundingBox

            foreach (var box in actualScene.boundingBoxesList)
            {
                // skip collision with floor
                if (box.name.Equals("floor")) //throw new Exception();
                    continue;

                if (cameraBoundingSphere.Intersects(box.boundingBox))
                    return false;
            }

            return true;
        }

        public bool cameraNextMoveCollisionDetectWithFloor(Vector3 nextCameraMove)
        {
            cameraBoundingSphere.Center = nextCameraMove;

            foreach (var box in actualScene.boundingBoxesList)
            {
                // skip collision if not floor object
                if (!box.name.Equals("floor"))
                    continue;

                if (cameraBoundingSphere.Intersects(box.boundingBox)) //throw new Exception();
                    return false;
            }

            return true;
        }


    }


}
