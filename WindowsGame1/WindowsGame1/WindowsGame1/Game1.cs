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
        public DynamicModel hand { get; set; }
        RenderTarget2D renderTarget;
        Texture2D shadowMap;
        PlayerInteractions playerInteractions;
        ParticleSystem timeParticles;


        //display texts
        SpriteFont spriteFont;
        HudTexts hudTexts = new HudTexts();

        public Scene actualScene { get; set; }
        Matrix cameraWorldMartix;
        Matrix handWorldMatrix;


        Hud hud = new Hud();
        private Texture2D[] hudTab = new Texture2D[7];
        private Texture2D hudBloody;
        private Texture2D hudGameOver;
        private Texture2D hudMenuGame;
        private Texture2D hudMenuMain;
        private Texture2D hudPointer;
        private Texture2D hudPoison;
        private Texture2D hudKey;
        private Texture2D hudText;
        private Texture2D hudTree;
        private Texture2D hudHealth;

        Effect simpleEffect;
        RasterizerState wireFrameState;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            timeParticles = new TimeParticleSystem(this, Content);
            Components.Add(timeParticles);

        }

        protected override void Initialize()
        {
            camera = new Camera(this, new Vector3(-15f, 1f, 5f), Vector3.Zero, 5f);
            camera = new Camera(this, new Vector3(-20f, 1f, -5f), Vector3.Zero, 5f);
            Components.Add(camera);
            //  graphics.ToggleFullScreen();
            floor = new Floor(GraphicsDevice, 20, 20);
            effect = new BasicEffect(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            actualScene = new Scene(Content, GraphicsDevice, camera);


            base.Initialize();
        }
        public void LoadSceneFromXml(string path)
        {
            actualScene.LoadFromXML(path);
            //if (path.Contains("scene2"))
            //{
            //    var tmp = new List<string>();
            //    tmp.Add("Models\\human_chodzenie");
            //    tmp.Add("Models\\human_cios");

            //    actualScene.AddEnemy(tmp, new Vector3(7, 3.5f, 2), new Vector3(0, 180, 0), 0.005f, "enemy", camera);
            //    actualScene.AddEnemy(tmp, new Vector3(8, 3.5f, 0), new Vector3(0, 180, 0), 0.005f, "enemy", camera);
            //}
            if (path.Equals("../../../../scene.xml"))
            {
                //jako parametr do konstruktora przekazuje sie liste nazw modeli, domyslnie odpalana jest pierwsza;
                var tmp = new List<string>();
                tmp.Add("Models\\enemy\\enemy_walk");
                tmp.Add("Models\\enemy\\enemy_punch");

                actualScene.AddEnemy(tmp, new Vector3(10, 0.2f, 10), new Vector3(0, 180, 0), 0.005f, "enemy", camera);

                var temp = new List<Model>();
                temp.Add(Content.Load<Model>("Models\\prawa_wyciaganie"));
                hand = new DynamicModel(GraphicsDevice, temp, new Vector3(-10, 1.2f, 1), new Vector3(0), 0.02f, "hand");
            }

            foreach (StaticModel m in actualScene.staticModelsList)
            {
                m.SetCustomEffect(simpleEffect);
                if (path.Contains("scene2"))
                    m.CreateShadowMaps();
            }
            DrawShadowMaps();
            actualScene.shadowMap = shadowMap;
            foreach (var x in actualScene.staticModelsList)
                x.shadowMap = shadowMap;
        }

        protected override void LoadContent()
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(GraphicsDevice, 4096, 4096, true, GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);

            //  actualScene.AddStaticModel("Models\\test", new Vector3(0), new Vector3(0), 1, "test");
            simpleEffect = Content.Load<Effect>("Effects\\shadows");

            // simpleEffect.CurrentTechnique = simpleEffect.Techniques["Simplest"];

            LoadSceneFromXml("../../../../scene.xml");





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
            hudPoison = Content.Load<Texture2D>("Sprites\\hudPoison");
            hudKey = Content.Load<Texture2D>("Sprites\\hudKey");
            hudText = Content.Load<Texture2D>("Sprites\\hText");
            hudTree = Content.Load<Texture2D>("Sprites\\sadzonka");
            hudHealth = Content.Load<Texture2D>("Sprites\\HP");

            // set all objects to interact with player (Distance)
            playerInteractions = new PlayerInteractions(this, hudTexts, actualScene.getStaticModelsList(), actualScene.getDynamicModelsList());

            wireFrameState = new RasterizerState()
            {
                FillMode = FillMode.WireFrame,
                CullMode = CullMode.None,
            };

            // camera/player collisions with everything
            camera.setCameraCollision(actualScene);


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

            playerInteractions.catchInteraction(camera, this);

            actualScene.Update(gameTime);
            hand.Update(gameTime);
            UpdateTimeParticle();

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
            //foreach (var x in actualScene.boundingBoxesList)
            //    x.Draw(camera);

            // fixing GraphicsDevice after spriteBatch.Begin() method
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;


            actualScene.Draw();
            //     hand.Draw(camera);

            //     cameraWorldMartix = Matrix.Invert(camera.View);
            //     handWorldMatrix = cameraWorldMartix;

            //     handWorldMatrix.Translation += (cameraWorldMartix.Forward * 1.4f) +
            //                                (-cameraWorldMartix.Down * 0.1f) +
            //                                (-cameraWorldMartix.Right * 0.3f);

            //     hand.Model = handWorldMatrix;

            if (playerInteractions.drawFight)
            {
            hud.drawHud(spriteBatch, hudTab[camera.BulletsAmount], this);
            hud.drawPointer(spriteBatch, hudPointer, this);
                hud.drawHealth(spriteBatch, hudHealth, this, camera);
            }
            if (playerInteractions.drawMenu == true)
            {
                hud.drawHud(spriteBatch, hudMenuGame, this);
            }
            if (playerInteractions.drawText == true)
            {
                hud.drawHud(spriteBatch, hudText, this);
            }


            for (int i = 0; i < camera.equipment.Count; i++)
            {
                if (camera.equipment[i].Name == "klucz")
                {
                    hud.drawHud(spriteBatch, hudKey, this);
                }
                if (camera.equipment[i].Name == "poison_box")
                {
                    hud.drawHud(spriteBatch, hudPoison, this);
                }

                if (camera.equipment[i].Name == "sadzonka2")
                {
                    hud.drawHud(spriteBatch, hudTree, this);
                }
            }

           /* if (camera.Hp <= 0)
            {
                hud.drawHud(spriteBatch, hudGameOver, this);
            }*/

            //     hand.Draw(camera);
            //     handWorldMatrix = cameraWorldMartix;

            //  hand.Model = handWorldMatrix;



            timeParticles.SetCamera(camera.View, camera.Projection);
            // DrawShadowMaps();
            base.Draw(gameTime);
        }


        Vector3 tempParticleVector1 = new Vector3(-13f, 5f, 6f);
        Vector3 tempParticleVector2 = new Vector3(46f, 0f, 10f);

        void UpdateTimeParticle()
        {
            const int timeParticlesPerFrame = 1;

            // Create a number of time particles, randomly positioned around a circle.
            for (int i = 0; i < timeParticlesPerFrame; i++)
            {
                timeParticles.AddParticle(tempParticleVector1, Vector3.Zero); // trucizna
                timeParticles.AddParticle(tempParticleVector2, Vector3.Zero); // sadzonka
            }

        }
    }
}
