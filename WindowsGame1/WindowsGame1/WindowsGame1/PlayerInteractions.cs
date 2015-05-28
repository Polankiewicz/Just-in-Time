using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
   public class PlayerInteractions
    {
        List<StaticModel> staticModelsList;
        List<DynamicModel> dynamicModelsList;
        Game game;
        HudTexts hudTexts;

        public PlayerInteractions(Game game, HudTexts hudTexts, List<StaticModel> staticModelsList, List<DynamicModel> dynamicModelsList)
        {
            this.staticModelsList = staticModelsList;
            this.dynamicModelsList = dynamicModelsList;
            this.game = game;
            this.hudTexts = hudTexts;
        }

        public void catchInteraction(Camera camera)
        {

            for (int i = 0; i < staticModelsList.Count; i++)
            {
                // destroy scaner
                if (staticModelsList[i].Name == "scaner" && Vector3.Distance(staticModelsList[i].Position, camera.Position) < 3.0f)
                {
                    hudTexts.DisplayText = "Press X to destroy scaner!";

                    if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
                        staticModelsList.RemoveAt(i);
                }
                else
                {
                    hudTexts.DisplayText = "";
                }

                // next interaction
                // ...
            } 
            for (int i = 0; i < dynamicModelsList.Count; i++)
            {
                if (dynamicModelsList[i].Name == "enemy")
                {
                    Enemy a = (Enemy)dynamicModelsList[i];
                    a.EnemyAI(camera);
                }
            }

        }

    }
}
