using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class Enemy
    {
        Game game;
        private int hp = 5;
        private float moveSpeed = 0.2f;
        List<DynamicModel> dynamicModelsList;
        


        public Enemy(Game game, List<DynamicModel> dynamicModelsList)
        {
            this.dynamicModelsList = dynamicModelsList;
            this.game = game;
        }

        public int Hp 
        {
            get { return hp; }
            set { hp = value; } 
        }


        public void EnemyAI(Camera c)
        {
            // if (trigger.id.collision && trigger.id == enemy.id)    
            
            for (int i = 0; i < dynamicModelsList.Count; i++)
            {
                // destroy scaner
                if (dynamicModelsList[i].Name == "enemy")
                { 
                    DynamicModel d = dynamicModelsList[i];
                    // while (hp > 0)
                    float distance = GetDistance(d, c);
                    MoveToPlayer(d, c);
                    if(distance < 1.5f)
                    {
                        AttackPlayer(d, c);
                    }
                }
            }
            
        }

        public void AttackPlayer(DynamicModel d, Camera c)
        {

            Console.WriteLine("Atakuj!");
        }
 

        public void MoveToPlayer(DynamicModel d, Camera c)
        {
            
            Console.WriteLine(d.Position);
        }

        public float GetDistance(DynamicModel d, Camera c)
        {
            float distance = Vector3.Distance(d.Position, c.Position);
            Console.WriteLine(distance);
            Console.WriteLine(c.Position);
            return distance;
        }

        void AttackPlayer()
        {
            
        }

    }

    
}