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
        private KeyboardState currentKeyboardState;
        private KeyboardState lastKeyboardState;
        public bool drawMenu;
        public bool drawText;
       

        public PlayerInteractions(Game game, HudTexts hudTexts, List<StaticModel> staticModelsList, List<DynamicModel> dynamicModelsList)
        {
            this.staticModelsList = staticModelsList;
            this.dynamicModelsList = dynamicModelsList;
            this.game = game;
            this.hudTexts = hudTexts;
        }

        public void catchInteraction(Camera camera, Game1 g)
        {
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && (lastKeyboardState.IsKeyUp(Keys.Escape)))
            {
                Console.WriteLine("esc");
                if (drawMenu == false)
                {
                    drawMenu = true;
                }
                else
                {
                    drawMenu = false;
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.M) && (lastKeyboardState.IsKeyUp(Keys.M)) && drawMenu == true)
            {
                g.Exit();
            }

            for (int i = 0; i < staticModelsList.Count; i++)
            {

                if (staticModelsList[i].Name == "poison_box" && Vector3.Distance(staticModelsList[i].Position, camera.Position) < 1.5f)
                {
                    drawText = true;

                    if ((GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed) || (currentKeyboardState.IsKeyDown(Keys.E) && (lastKeyboardState.IsKeyUp(Keys.E))))
                    {
                        camera.equipment.Add(staticModelsList[i]);
                        staticModelsList.RemoveAt(i);
                        drawText = false;
                    }

                }

                if (staticModelsList[i].Name == "shop" && Vector3.Distance(staticModelsList[i].Position, camera.Position) < 1.5f)
                {
                    for (int j = 0; j < camera.equipment.Count; j++)
                    {
                        if(camera.equipment[j].Name == "poison_box")
                        {
                            drawText = true;
                            if ((GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed) || (currentKeyboardState.IsKeyDown(Keys.E) && (lastKeyboardState.IsKeyUp(Keys.E))))
                            {
                                camera.equipment.Add(staticModelsList[i]);
                                camera.equipment.RemoveAt(j);
                                drawText = false;
                            }
                        }
                    }

                }

                if (staticModelsList[i].Name == "klucz" && Vector3.Distance(staticModelsList[i].Position, camera.Position) < 1.0f)
                {
                    drawText = true;
                    if ((GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed) || (currentKeyboardState.IsKeyDown(Keys.E) && (lastKeyboardState.IsKeyUp(Keys.E))))
                    {
                        camera.equipment.Add(staticModelsList[i]);
                        staticModelsList.RemoveAt(i);
                        drawText = false;
                    }

                }

                if (staticModelsList[i].Name == "sadzonka2" && Vector3.Distance(staticModelsList[i].Position, camera.Position) < 3.0f)
                {
                    drawText = true;
                    if ((GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed) || (currentKeyboardState.IsKeyDown(Keys.E) && (lastKeyboardState.IsKeyUp(Keys.E))))
                    {
                        camera.equipment.Add(staticModelsList[i]);
                        staticModelsList.RemoveAt(i);
                        drawText = false;
                    }

                }
            } 

            for (int i = 0; i < dynamicModelsList.Count; i++)
            {
                if (dynamicModelsList[i].Name == "enemy")
                {
                    Enemy a = (Enemy)dynamicModelsList[i];
                    a.EnemyAI(camera);
                    for (int j = 0; j < staticModelsList.Count; j++)
                    {                       
                        if (staticModelsList[j].Name == "trigger1" && Vector3.Distance(staticModelsList[j].Position, camera.Position) < 1.5f)
                        {
                            a.Condition = true;
                        }
                    }
                }
            }

        }

    }
}
