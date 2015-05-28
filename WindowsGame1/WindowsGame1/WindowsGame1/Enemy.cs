﻿using Microsoft.Xna.Framework;
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
        private float moveSpeed = 0.2f;
        



        public Enemy(GraphicsDevice device, List<Model> modelList, Vector3 position, Vector3 rotationDegrees, float scale, String objectName, Camera c)
            : base(device, modelList, position, rotationDegrees, scale, objectName)
        {
           
        }

        public int Hp 
        {
            get { return hp; }
            set { hp = value; } 
        }


        public void EnemyAI(Camera c)
         {
             // if (trigger.id.collision && trigger.id == enemy.id)    
             if (GetDistance(c) < 3f)
             { 
                MoveToPlayer(c);
                if (GetDistance(c) < 1.5f)
                {
                    AttackPlayer(c);
                }
                
                 //this.destroy();
             }
             
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
           
        }

        public float GetDistance(Camera c)
        {
            float distance = Vector3.Distance(this.Position, c.Position);
            Console.WriteLine(distance);
            return distance;
        }


    }

    
}