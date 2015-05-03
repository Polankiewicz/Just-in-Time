using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class PlayerInteractions
    {
        //interactive objects
        List<StaticModel> buildingsList;
        int buildingsListSize;

        Game game;

        HudTexts hudTexts;

        public PlayerInteractions(Game game, HudTexts hudTexts, List<StaticModel> buildingsList)
        {
            this.buildingsList = buildingsList;
            buildingsListSize = buildingsList.Count;
            this.game = game;

            this.hudTexts = hudTexts;
        }

        public void catchInteraction(Camera camera)
        {
            // destroy scaner
            if (buildingsList.Count >= buildingsListSize && Vector3.Distance(buildingsList[0].Position, camera.Position) < 3.0f)
            {
                hudTexts.DisplayText = "Press X to destroy scaner!";

                if(GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
                    buildingsList.RemoveAt(0);
            }
            else
            {
                hudTexts.DisplayText = "";
            }
            
            
            
            
        }

    }
}
