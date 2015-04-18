using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class CameraCollisions
    {
        BoundingSphere b1;
        BoundingSphere b2;
        Camera camera;
        Enemy enemy;

        public CameraCollisions(Camera camera, Enemy enemy)
        {
            this.camera = camera;
            this.enemy = enemy;
            
            b1 = new BoundingSphere(camera.Position, 0.50f);
            b2 = new BoundingSphere(enemy.Position, 0.50f);
        }

        public void updateBoundingSpherePosition()
        {
            b2.Center = enemy.Position;
        }

        public bool cameraNextMoveCollisionDetect(Vector3 nextCameraMove)
        {
            b1.Center = nextCameraMove;

            if (b1.Intersects(b2))
                return false;
            else
                return true;
        }


    }
}
