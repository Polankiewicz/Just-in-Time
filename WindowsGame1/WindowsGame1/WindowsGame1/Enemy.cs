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
             if (condition)
             {
                 LookAt(c);
                 if (GetDistance(c) > 1.7f)
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
            if (this.model != modelList[1])
            {
                this.model = modelList[1];
                this.SwitchAnimation(1);
            }
        }


        public void MoveToPlayer(Camera c)
        {
            Console.WriteLine("move");
            this.Position = Vector3.Lerp(this.Position, c.Position, moveSpeed);
            this.Model = Matrix.CreateTranslation(this.Position.X, height, this.Position.Z);
            if (this.model != modelList[0])
            {
                this.model = modelList[0];
                this.SwitchAnimation(0);
            }
        }

        public float GetDistance(Camera c)
        {
            float distance = Vector3.Distance(this.Position, c.Position);
            Console.WriteLine(distance);
            return distance;
        }

       public void LookAt(Camera c)
        {
            float tmp = 2.1f;
            Vector3 dif = Vector3.Subtract(this.Position, c.Position);
            float angle = (float)Math.Atan2(dif.X, dif.Z);
            this.rotation = Matrix.CreateRotationX(MathHelper.ToRadians(0))
                            * Matrix.CreateRotationY(this.Rotation.Y + angle + tmp)
                            * Matrix.CreateRotationZ(MathHelper.ToRadians(0));

        }
        
    }



    
}