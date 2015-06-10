using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera camera;
        Floor floor;
        BasicEffect effect;
        DynamicModel hand;
        RenderTarget2D renderTarget;
        Texture2D shadowMap;
        PlayerInteractions playerInteractions;


        //display texts
        SpriteFont spriteFont;
        HudTexts hudTexts = new HudTexts();
       
        Scene actualScene;
        Matrix cameraWorldMartix;
        Matrix handWorldMatrix;
        

        Hud hud = new Hud();
        private Texture2D[] hudTab = new Texture2D[7];
        private Texture2D hudBloody;
        private Texture2D hudGameOver;
        private Texture2D hudMenuGame;
        private Texture2D hudMenuMain;
        Effect simpleEffect;
        RasterizerState wireFrameState;
        private Texture2D hudPointer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            camera = new Camera(this, new Vector3(10f, 1f, 5f), Vector3.Zero, 5f);
            Components.Add(camera);
            //  graphics.ToggleFullScreen();
            floor = new Floor(GraphicsDevice, 20, 20);
            effect = new BasicEffect(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            actualScene = new Scene(Content, GraphicsDevice, camera);


            base.Initialize();
        }
       
        protected override void LoadContent()
        {
          actualScene.LoadFromXML("../../../../scene.xml");
          //  actualScene.AddStaticModel("Models\\test", new Vector3(0), new Vector3(0), 1, "test");
            simpleEffect = Content.Load<Effect>("Effects\\shadows");
           
           // simpleEffect.CurrentTechnique = simpleEffect.Techniques["Simplest"];
           
            foreach (StaticModel m in actualScene.staticModelsList)
            {
                m.SetCustomEffect(simpleEffect);
            }



            //jako parametr do konstruktora przekazuje sie liste nazw modeli, domyslnie odpalana jest pierwsza;
            var tmp = new List<string>();
            tmp.Add("Models\\przeciwnik");
            tmp.Add("Models\\przeciwnik");
            actualScene.AddEnemy(tmp, new Vector3(10, 0.2f, 10), new Vector3(0, 180, 0), 0.005f, "enemy", camera);

            var temp = new List<Model>();
            temp.Add(Content.Load<Model>("Models\\hand"));
            hand = new DynamicModel(GraphicsDevice, temp, new Vector3(1, 1.2f, 1), new Vector3(-45, 90, 90), 0.02f, "hand");
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            renderTarget = new RenderTarget2D(GraphicsDevice, 2048, 2048, true, GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

           // renderTarget = new RenderTarget2D(GraphicsDevice, 1024, 1024, true, GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            // hud texts
            spriteFont = Content.Load<SpriteFont>("Sprites\\PressXtoInteract");
            hudTab[6] = Content.Load<Texture2D>("Sprites\\6");
            hudTab[5] = Content.Load<Texture2D>("Sprites\\5");
            hudTab[4] = Content.Load<Texture2D>("Sprites\\4");
            hudTab[3] = Content.Load<Texture2D>("Sprites\\3");
            hudTab[2] = Content.Load<Texture2D>("Sprites\\2");
            hudTab[1] = Content.Load<Texture2D>("Sprites\\1");
            hudTab[0] = Content.Load<Texture2D>("Sprites\\0");
            hudBloody = Content.Load<Texture2D>("Sprites\\Bloody");
            hudGameOver = Content.Load<Texture2D>("Sprites\\GameOver");
            hudMenuGame = Content.Load<Texture2D>("Sprites\\MenuGame");
            hudMenuMain = Content.Load<Texture2D>("Sprites\\MenuMain");
            hudPointer = Content.Load<Texture2D>("Sprites\\Pointer");

            // set all objects to interact with player (Distance)
            playerInteractions = new PlayerInteractions(this, hudTexts, actualScene.getStaticModelsList(), actualScene.getDynamicModelsList());

            wireFrameState = new RasterizerState()
            {
                FillMode = FillMode.WireFrame,
                CullMode = CullMode.None,
            };

            // camera/player collisions with everything
            camera.setCameraCollision(actualScene);
            foreach (var x in actualScene.staticModelsList)
            {
                x.shadowMap = shadowMap;
                
            }
            DrawShadowMaps();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
           
            // TODO: Add your update logic here


            playerInteractions.catchInteraction(camera);

            actualScene.Update(gameTime);
            hand.Update(gameTime);

            base.Update(gameTime);
        }
        void DrawShadowMaps()
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            foreach (var x in actualScene.staticModelsList)
            {
                x.SetCustomEffect(simpleEffect, false);
                x.Draw(camera, "ShadowMap");
            }


            GraphicsDevice.SetRenderTarget(null);
            shadowMap = (Texture2D)renderTarget;
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);
         
           
          //  GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);
            //using (SpriteBatch sprite = new SpriteBatch(GraphicsDevice))
            //{
            //    sprite.Begin();
            //    sprite.Draw(shadowMap, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 0.4f, SpriteEffects.None, 1);
            //    sprite.End();
            //}
          //  shadowMap = null;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            hudTexts.drawText(spriteBatch, spriteFont);
            GraphicsDevice.RasterizerState = wireFrameState;

            // CreateDrawableBoxes();
            foreach (var x in actualScene.boundingBoxesList)
                x.Draw(camera);

            // fixing GraphicsDevice after spriteBatch.Begin() method
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
           
         //   actualScene.Draw();
            foreach (var x in actualScene.staticModelsList)
            {
                foreach (ModelMesh mesh in x.fbxModel.Meshes)
                {

                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        if (!(meshPart.Effect is BasicEffect))
                            meshPart.Effect.Parameters["xShadowMap"].SetValue(shadowMap);
                        else
                        {
                            x.Draw(camera, "Simplest");
                            break;

                        }
                    }

                }
                x.Draw(camera, "ShadowedScene");
            }
           
       //     cameraWorldMartix = Matrix.Invert(camera.View);
       //     handWorldMatrix = cameraWorldMartix;

       //     handWorldMatrix.Translation += (cameraWorldMartix.Forward * 1.4f) +
       //                                (-cameraWorldMartix.Down * 0.1f) +
       //                                (-cameraWorldMartix.Right * 0.3f);

       //     hand.Model = handWorldMatrix;

       ////     hud.drawHud(spriteBatch, hudTab[camera.BulletsAmount], this);
       //   //  hud.drawPointer(spriteBatch, hudPointer, this);

       //     hand.Draw(camera);
       //     handWorldMatrix = cameraWorldMartix;

       //     handWorldMatrix.Translation += (cameraWorldMartix.Forward * 1.4f) +
       //                                 (-cameraWorldMartix.Down * 0.1f) +
       //                                (cameraWorldMartix.Right * 0.9f);

       //     hand.Model = handWorldMatrix;


       //     hand.Draw(camera);

           // DrawShadowMaps();
            base.Draw(gameTime);
        }

    }
}
