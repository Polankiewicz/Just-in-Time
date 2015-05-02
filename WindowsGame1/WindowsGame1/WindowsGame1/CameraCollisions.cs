using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace WindowsGame1
{
    class CameraCollisions
    {
        BoundingSphere cameraBoundingSphere;
        BoundingSphere b2;
        Camera camera;
        DynamicModel enemy; // sie wyjebie i zamieni na dynamicModelsList
        List<StaticModel> staticModelsList;
        List<BoundingSphere> staticBoundingSpheresList = new List<BoundingSphere>();

        public CameraCollisions(Camera camera, DynamicModel enemy, List<StaticModel> staticModelsList)
        {
            this.camera = camera;
            this.enemy = enemy;
            this.staticModelsList = staticModelsList;

            cameraBoundingSphere = new BoundingSphere(camera.Position, 0.50f);
            b2 = new BoundingSphere(enemy.Position, 0.50f);


            //for(int i = 0; i < staticModelsList.Count; i++)
            //{
                //if (staticModelsList[i].ToString)

                //Console.WriteLine( staticModelsList[i].GetType() );
            //}
        }

        public void updateBoundingSpherePosition()
        {
            b2.Center = enemy.Position;
        }

        public bool cameraNextMoveCollisionDetect(Vector3 nextCameraMove)
        {
            cameraBoundingSphere.Center = nextCameraMove;

            if (cameraBoundingSphere.Intersects(b2))
                return false;
            else
                return true;
        }


    }


}
