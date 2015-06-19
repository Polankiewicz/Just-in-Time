using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class Bullet
    {
        float radius;
        float distance;
        float speed;
        BoundingSphere _boundingSphere;

        public BoundingSphere boundingSphere
        {
            get { return _boundingSphere; }
            set { _boundingSphere = value; }
        }


        public Bullet(Vector3 center)
        {
            radius = 1.1f; // 0.1f
            speed = 5f;
            distance = 55f; 

            _boundingSphere = new BoundingSphere(center, radius);
        }

        public bool checkIfBulletIsTooFarAway(Vector3 cameraPosition)
        {
            if (Vector3.Distance(_boundingSphere.Center, cameraPosition) > distance)
                return true;

            return false;
        }

    }
}
