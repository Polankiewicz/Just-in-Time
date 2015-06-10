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
        Game1 game;
        List<Bullet> listOfBullets;
        public List<string> tmp = new List<string>();

        // TEMP lists
        List<BoundingBox> staticBoundingSpheresList = new List<BoundingBox>();
        List<BoundingSphere> dynamicBoundingSpheresList = new List<BoundingSphere>();

        public CameraCollisions(Camera camera, Game1 game, List<Bullet> listOfBullets)
        {
            this.camera = camera;
            this.actualScene = game.actualScene;
            this.staticModelsList = this.actualScene.getStaticModelsList();
            this.dynamicModelsList = this.actualScene.getDynamicModelsList();
            this.boundingBoxesList = this.actualScene.getBoundingBoxesList();
            this.game = game;
            
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
                        game.LoadSceneFromXml("../../../../scene2.xml");
                        tmp.Add("Models\\przeciwnik");
                        actualScene.AddEnemy(tmp, new Vector3(10, 0.2f, 10), new Vector3(0, 180, 0), 0.005f, "enemy", camera);
                      //  actualScene.LoadFromXML("../../../../scene2.xml");

                        camera.Position = new Vector3(9.3f, 1.5f, -3f);

                        camera.clearCameraLookAt();

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
