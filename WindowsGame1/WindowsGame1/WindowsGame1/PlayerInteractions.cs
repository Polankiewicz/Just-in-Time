using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class PlayerInteractions
    {
        List<Building> buildingsList;
        Game game;

        public PlayerInteractions(Game game, List<Building> buildingsList)
        {
            this.buildingsList = buildingsList;
            this.game = game;
        }

        public void catchInteraction(Camera camera)
        {
            if (Vector3.Distance(buildingsList[1].Position, camera.Position) < 3.0f)
                game.Exit();


        }

    }
}
