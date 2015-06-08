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

        List<Bullet> listOfBullets;

        // TEMP lists
        List<BoundingBox> staticBoundingSpheresList = new List<BoundingBox>();
        List<BoundingSphere> dynamicBoundingSpheresList = new List<BoundingSphere>();

        public CameraCollisions(Camera camera, Scene actualScene, List<Bullet> listOfBullets)
        {
            this.camera = camera;
            this.staticModelsList = actualScene.getStaticModelsList();
            this.dynamicModelsList = actualScene.getDynamicModelsList();
            this.boundingBoxesList = actualScene.getBoundingBoxesList();
            this.actualScene = actualScene;
            this.listOfBullets = listOfBullets;

            cameraBoundingSphere = new BoundingSphere(camera.Position, 0.50f);

            // TEMP - collision for dynamic models
            for (int i = 0; i < dynamicModelsList.Count; i++)
            {
                if (dynamicModelsList[i].Name == "enemy")
                    dynamicBoundingSpheresList.Add(new BoundingSphere(dynamicModelsList[i].Position, 1.0f));
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
                    dynamicBoundingSpheresList[i] = xxx;
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
            for (int i = 0; i < boundingBoxesList.Count; i++)
            {
                // skip collision with floor & stairs
                if (boundingBoxesList[i].name.Equals("floor") || boundingBoxesList[i].name.Equals("stairs")) //throw new Exception();
                    continue;

                // load new scene
                if(boundingBoxesList[i].name.Equals("scene2"))
                {
                    if(cameraBoundingSphere.Intersects(boundingBoxesList[i].boundingBox))
                    {
                        actualScene.unloadContent();
                        actualScene.LoadFromXML("../../../../scene2.xml");
                        return true;
                    }
                }
                
                // other boundingBoxes
                if (cameraBoundingSphere.Intersects(boundingBoxesList[i].boundingBox))
                    return false;
            }

            return true;
        }

        public bool cameraNextMoveCollisionDetectWithFloor(Vector3 nextCameraMove)
        {
            cameraBoundingSphere.Center = nextCameraMove;

            for (int i = 0; i < boundingBoxesList.Count; i++)
            {
                // skip collision if not floor object
                if (!boundingBoxesList[i].name.Equals("floor"))
                    continue;

                if (cameraBoundingSphere.Intersects(boundingBoxesList[i].boundingBox)) //throw new Exception();
                    return false;
            }

            return true;
        }

        public bool cameraNextMoveCollisionDetectWithStairs(Vector3 nextCameraMove)
        {
            cameraBoundingSphere.Center = nextCameraMove;

            for (int i = 0; i < boundingBoxesList.Count; i++)
            {
                // skip collision if not stairs object
                if (!boundingBoxesList[i].name.Equals("stairs"))
                    continue;

                if (cameraBoundingSphere.Intersects(boundingBoxesList[i].boundingBox)) //throw new Exception();
                    return false;
            }

            return true;
        }

        public void bulletCollisionWithEnemy()
        {
            for (int i = 0; i < listOfBullets.Count; i++)
            {
                // collision with dynamic models (enemy)
                for (int j = 0; j < dynamicBoundingSpheresList.Count; j++)
                {
                    if (dynamicBoundingSpheresList[j].Intersects(listOfBullets[i].boundingSphere))
                    {
                        // remove model
                        dynamicModelsList.RemoveAt(0);
                        // remove bounding box for this model
                        dynamicBoundingSpheresList.RemoveAt(j);

                        //throw new Exception("wykryto kolizje pocisku z przeciwnikiem, liczba pociskow: " + listOfBullets.Count);
                    }
                }
                
                // temp
                if (boundingBoxesList[0].boundingBox.Intersects(listOfBullets[i].boundingSphere))
                    throw new Exception("wykryto kolizje pocisku z blokiem, liczba pociskow: " + listOfBullets.Count);
            }
        }




    }


}
