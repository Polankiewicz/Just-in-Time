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
        Camera camera;
        BoundingSphere cameraBoundingSphere;

        BoundingSphere enemyBoundingSphere;  // temp
        
        List<StaticModel> staticModelsList;
        List<DynamicModel> dynamicModelsList;

        List<BoundingSphere> staticBoundingSpheresList = new List<BoundingSphere>();
        List<BoundingSphere> dynamicBoundingSpheresList = new List<BoundingSphere>();

        public CameraCollisions(Camera camera, List<DynamicModel> dynamicModelsList, List<StaticModel> staticModelsList)
        {
            this.camera = camera;
            this.staticModelsList = staticModelsList;
            this.dynamicModelsList = dynamicModelsList;

            cameraBoundingSphere = new BoundingSphere(camera.Position, 0.50f);
            enemyBoundingSphere = new BoundingSphere(dynamicModelsList[0].Position, 0.50f);

        }

        public void updateBoundingSpherePosition()
        {
            enemyBoundingSphere.Center = dynamicModelsList[0].Position;
        }

        public bool cameraNextMoveCollisionDetect(Vector3 nextCameraMove)
        {
            cameraBoundingSphere.Center = nextCameraMove;

            if (cameraBoundingSphere.Intersects(enemyBoundingSphere))
                return false;
            else
                return true;
        }


    }


}
