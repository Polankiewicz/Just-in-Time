﻿using Microsoft.Xna.Framework;
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
        String p;
        private KeyboardState currentKeyboardState;
        private KeyboardState lastKeyboardState;

        private GamePadState currentGamepadState;
        private GamePadState lastGamepadState;
        public bool drawMenu = false;
        public bool drawText = false;
        public bool drawFight = false;
        public bool backCondition = false;
        public bool pastCondition = false;
        public bool onceBool = false;
        public int gMenu = 0;
        public int mMenu = 0;
       

        public PlayerInteractions(Game game, HudTexts hudTexts, List<StaticModel> staticModelsList, List<DynamicModel> dynamicModelsList, String p)
        {
            this.staticModelsList = staticModelsList;
            this.dynamicModelsList = dynamicModelsList;
            this.game = game;
            this.hudTexts = hudTexts;
            this.p = p;
        }

        public void catchInteraction(Camera camera, Game1 g)
        {
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            lastGamepadState = currentGamepadState;
            currentGamepadState = GamePad.GetState(PlayerIndex.One);

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && (lastKeyboardState.IsKeyUp(Keys.Escape)))
            {
                if (drawMenu == false)
                {
                    gMenu = 0;
                    drawMenu = true;
                }
                else
                {
                    drawMenu = false;
                }
            }

            if (drawMenu == true && currentKeyboardState.IsKeyDown(Keys.Up) && (lastKeyboardState.IsKeyUp(Keys.Up)) || (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed))
            {
                if(gMenu > 0)
                {
                    gMenu--;
                }
                
            }

            if (drawMenu == true && currentKeyboardState.IsKeyDown(Keys.Down) && (lastKeyboardState.IsKeyUp(Keys.Down)) || (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed))
            {
                if (gMenu < 5)
                {
                    gMenu++;
                }
            }

            if (gMenu == 1 && drawMenu == true && currentKeyboardState.IsKeyDown(Keys.Enter) && (lastKeyboardState.IsKeyUp(Keys.Enter)) || (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed))
            {
                drawMenu = false;
            }

            if (gMenu == 2 && drawMenu == true && currentKeyboardState.IsKeyDown(Keys.Enter) && (lastKeyboardState.IsKeyUp(Keys.Enter)) || (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed))
            {
                drawMenu = false;
                mMenu = 0;
                g.drawMainMenu = true;
            }

            if (gMenu == 5 && drawMenu == true && currentKeyboardState.IsKeyDown(Keys.Enter) && (lastKeyboardState.IsKeyUp(Keys.Enter)) || (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed))
            {
                g.Exit();
            }

            if (g.drawMainMenu == true && currentKeyboardState.IsKeyDown(Keys.Up) && (lastKeyboardState.IsKeyUp(Keys.Up)) || (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed))
            {
                if (mMenu > 0)
                {
                    mMenu--;
                }

            }

            if (g.drawMainMenu == true && currentKeyboardState.IsKeyDown(Keys.Down) && (lastKeyboardState.IsKeyUp(Keys.Down)) || (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed))
            {
                if (mMenu < 4)
                {
                    mMenu++;
                }
            }

            if (mMenu == 1 && g.drawMainMenu == true && currentKeyboardState.IsKeyDown(Keys.Enter) && (lastKeyboardState.IsKeyUp(Keys.Enter)))
            {
                g.drawMainMenu = false;
            }

            if (mMenu == 4 && g.drawMainMenu == true && currentKeyboardState.IsKeyDown(Keys.Enter) && (lastKeyboardState.IsKeyUp(Keys.Enter)))
            {
                g.Exit();
            }

            if ((currentKeyboardState.IsKeyDown(Keys.B) && (lastKeyboardState.IsKeyUp(Keys.B))) || (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed))
            {
                if(pastCondition == true)
                {
                    pastCondition = false;
                    onceBool = true;
                    return;
                }
                if(pastCondition == false && backCondition == true)
                {
                    pastCondition = true;
                    backCondition = false;
                    onceBool = true;
                    return;
                }
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

                if (staticModelsList[i].Name == "miska" && Vector3.Distance(staticModelsList[i].Position, camera.Position) < 1.5f)
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

                if (staticModelsList[i].Name == "trigger2" && Vector3.Distance(staticModelsList[i].Position, camera.Position) < 2f)
                {
                    backCondition = true;
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
                            drawFight = a.Condition;
                        }
                    }
                }
            }

        }

    }
}
