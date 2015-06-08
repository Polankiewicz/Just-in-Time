using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class Enemy : DynamicModel
    {
        private int hp = 5;
        private float moveSpeed = 0.01f;
        private int id;
        private bool condition = false;
        private float height = 0.2f;


        public Enemy(GraphicsDevice device, List<Model> modelList, Vector3 position, Vector3 rotationDegrees, float scale, String objectName, Camera c, int i)
            : base(device, modelList, position, rotationDegrees, scale, objectName)
        {
            this.id = i;
        }

        public int Hp 
        {
            get { return hp; }
            set { hp = value; } 
        }

        public bool Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public void EnemyAI(Camera c)
         {
             // if (trigger.id.collision && trigger.id == enemy.id)
             if (condition)
             {
                // LookAt(c);
                 if (GetDistance(c) > 1.9f)
                 {
                     MoveToPlayer(c);
                 }
                 if (GetDistance(c) < 2f)
                 {
                     AttackPlayer(c);
                 }
             }
                
                 //this.destroy();
             
         }

        public void AttackPlayer(Camera c)
        {
            Console.WriteLine("Atakuj!");
            //a tutaj zamiast atakuj mozna zrobic cos takiego np by zmienic animacje:
            //this.model = modelList[1];
        }


        public void MoveToPlayer(Camera c)
        {
            Console.WriteLine("move");
            this.Position = Vector3.Lerp(this.Position, c.Position, moveSpeed);
            this.Model = Matrix.CreateTranslation(this.Position.X, height, this.Position.Z);
        }

        public float GetDistance(Camera c)
        {
            float distance = Vector3.Distance(this.Position, c.Position);
            Console.WriteLine(distance);
            return distance;
        }

       /*public void LookAt(Camera c)
        {
           // this.Rotation = new Vector3(0,TurnToFace(this.Position, c.Position, this.Rotation.Y, 0.02f),0);

            this.rotation =  Matrix.CreateRotationX(MathHelper.ToRadians(0))
                            * Matrix.CreateRotationY(this.Rotation.Y + MathHelper.ToRadians(TurnToFace(this.Position, c.Position, this.Rotation.Y, 2f)))
                            * Matrix.CreateRotationZ(MathHelper.ToRadians(0));

           // TurnToFace(this.Position, c.Position, this.Rotation.Y, 2f);
            
        }

        private static float TurnToFace(Vector3 position, Vector3 faceThis,
            float currentAngle, float turnSpeed)
        {
            // consider this diagram:
            //         C 
            //        /|
            //      /  |
            //    /    | y
            //  / o    |
            // S--------
            //     x
            // 
            // where S is the position of the spot light, C is the position of the cat,
            // and "o" is the angle that the spot light should be facing in order to 
            // point at the cat. we need to know what o is. using trig, we know that
            //      tan(theta)       = opposite / adjacent
            //      tan(o)           = y / x
            // if we take the arctan of both sides of this equation...
            //      arctan( tan(o) ) = arctan( y / x )
            //      o                = arctan( y / x )
            // so, we can use x and y to find o, our "desiredAngle."
            // x and y are just the differences in position between the two objects.
            float x = faceThis.X - position.X;
            float z = faceThis.Z - position.Z;

            // we'll use the Atan2 function. Atan will calculates the arc tangent of 
            // y / x for us, and has the added benefit that it will use the signs of x
            // and y to determine what cartesian quadrant to put the result in.
            // http://msdn2.microsoft.com/en-us/library/system.math.atan2.aspx
            float desiredAngle = (float)Math.Atan2(z, x);

            // so now we know where we WANT to be facing, and where we ARE facing...
            // if we weren't constrained by turnSpeed, this would be easy: we'd just 
            // return desiredAngle.
            // instead, we have to calculate how much we WANT to turn, and then make
            // sure that's not more than turnSpeed.

            // first, figure out how much we want to turn, using WrapAngle to get our
            // result from -Pi to Pi ( -180 degrees to 180 degrees )
            float difference = WrapAngle(desiredAngle - currentAngle);

            // clamp that between -turnSpeed and turnSpeed.
            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            // so, the closest we can get to our target is currentAngle + difference.
            // return that, using WrapAngle again.
            float anangle = WrapAngle(currentAngle + difference);
            return anangle;
        }

       private static float WrapAngle(float radians)
       {
           while (radians < -MathHelper.Pi)
           {
               radians += MathHelper.TwoPi;
           }
           while (radians > MathHelper.Pi)
           {
               radians -= MathHelper.TwoPi;
           }
           return radians;
       }*/
        
    }



    
}