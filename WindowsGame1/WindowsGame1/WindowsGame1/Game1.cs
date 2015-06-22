using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
        RenderTarget2D renderTarget, reflectionRenderTarget,glassRenderTarget;
        Texture2D shadowMap, reflectionMap,glassMap;
        PlayerInteractions playerInteractions;
        ParticleSystem timeParticles;
      
        //display texts
        SpriteFont spriteFont;
        HudTexts hudTexts = new HudTexts();

        public Scene actualScene { get; set; }
        Matrix cameraWorldMartix;
        Matrix handWorldMatrix;
        Matrix reflectionViewMatrix;
        Matrix refractionViewMatrix;

        Hud hud = new Hud();
        private Texture2D[] hudTab = new Texture2D[14];
        private Texture2D[] hudMenuGame = new Texture2D[6];
        private Texture2D[] hudMenuMain = new Texture2D[5];
        private Texture2D hudPointer;
        private Texture2D hudPoison;
        private Texture2D hudKey;
        private Texture2D hudText;
        private Texture2D hudTree;
        private Texture2D hudHealth2;
        private Texture2D hudHealth;
        private Texture2D hudGameOver;
        public bool drawMainMenu = true;

        Skybox skybox;
        Effect simpleEffect;
        RasterizerState wireFrameState;
        Mirror mirror;
        String p;
        Glass glass;

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
            p = path;
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

            }
            mirror = new Mirror(GraphicsDevice, Content.Load<Model>("Models\\lustro"), new Vector3(-10, 0.2f, 1), new Vector3(0), 0.02f, "mirror", "Models\\lustro");
            mirror.SetCustomEffect(Content.Load<Effect>("Effects\\mirror"));
            glass = new Glass(GraphicsDevice, Content.Load<Model>("Models\\glass"), new Vector3(-10, -0.4f, -2), new Vector3(0), 0.06f, "glass", "Models\\glass");
            glass.SetCustomEffect(Content.Load<Effect>("Effects\\glass"));
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
            
            DrawReflectionMap();
            mirror.RefreshTexture(reflectionMap);
        }



        protected override void LoadContent()
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(GraphicsDevice, 4096, 4096, true, GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            reflectionRenderTarget = new RenderTarget2D(GraphicsDevice, 2048, 2048, true, GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            glassRenderTarget = new RenderTarget2D(GraphicsDevice, 2048, 2048, true, GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            //  actualScene.AddStaticModel("Models\\test", Vector3 nwew(0), new Vector3(0), 1, "test");
            simpleEffect = Content.Load<Effect>("Effects\\shadows");

           
            

            // simpleEffect.CurrentTechnique = simpleEffect.Techniques["Simplest"];

            LoadSceneFromXml("../../../../scene.xml");

            skybox = new Skybox("Skyboxes\\333Sunset", Content);

           // actualScene.AddStaticModel("Models\\lustro", new Vector3(-10, 1.2f, 1), new Vector3(0), 0.02f, "mirror"); //= new StaticModel(GraphicsDevice, Content.Load<Model>("Models\\lustro"), new Vector3(-10, 1.2f, 1), new Vector3(0), 0.02f, "mirror", "Models\\lustro");
          
            

            var temp = new List<Model>();
            temp.Add(Content.Load<Model>("Models\\righthand\\pull"));
            temp.Add(Content.Load<Model>("Models\\righthand\\pulled"));
            temp.Add(Content.Load<Model>("Models\\righthand\\shot"));
            hand = new DynamicModel(GraphicsDevice, temp, new Vector3(-10, 1.2f, 1), new Vector3(0), 0.02f, "hand");

            // renderTarget = new RenderTarget2D(GraphicsDevice, 1024, 1024, true, GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            // hud texts
            spriteFont = Content.Load<SpriteFont>("Sprites\\PressXtoInteract");

            hudTab[13] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_add_6");
            hudTab[12] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_add_5");
            hudTab[11] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_add_4");
            hudTab[10] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_add_3");
            hudTab[9] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_add_2");
            hudTab[8] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_add_1");
            hudTab[7] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_add_0");
            hudTab[6] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_bullet_6");
            hudTab[5] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_bullet_5");
            hudTab[4] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_bullet_4");
            hudTab[3] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_bullet_3");
            hudTab[2] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_bullet_2");
            hudTab[1] = Content.Load<Texture2D>("Sprites\\Bullets\\gun_bullet_1");
            hudTab[0] = Content.Load<Texture2D>("Sprites\\Bullets\\gun");

            hudMenuGame[0] = Content.Load<Texture2D>("Sprites\\GameMenu\\pause_state_0");
            hudMenuGame[1] = Content.Load<Texture2D>("Sprites\\GameMenu\\pause_back_02");
            hudMenuGame[2] = Content.Load<Texture2D>("Sprites\\GameMenu\\pause_main_menu_02");
            hudMenuGame[3] = Content.Load<Texture2D>("Sprites\\GameMenu\\pause_credits_02");
            hudMenuGame[4] = Content.Load<Texture2D>("Sprites\\GameMenu\\pause_about_02");
            hudMenuGame[5] = Content.Load<Texture2D>("Sprites\\GameMenu\\pause_exit_02");

            hudMenuMain[0] = Content.Load<Texture2D>("Sprites\\MainMenu\\0");
            hudMenuMain[1] = Content.Load<Texture2D>("Sprites\\MainMenu\\1");
            hudMenuMain[2] = Content.Load<Texture2D>("Sprites\\MainMenu\\2");
            hudMenuMain[3] = Content.Load<Texture2D>("Sprites\\MainMenu\\3");
            hudMenuMain[4] = Content.Load<Texture2D>("Sprites\\MainMenu\\4");

            hudPointer = Content.Load<Texture2D>("Sprites\\celownik");
            //hudGameOver = Content.Load<Texture2D>("Sprites\\GameOver");
            hudHealth = Content.Load<Texture2D>("Sprites\\health_2px");
            hudHealth2 = Content.Load<Texture2D>("Sprites\\health_frame");
            hudPoison = Content.Load<Texture2D>("Sprites\\hudPoison");
            hudKey = Content.Load<Texture2D>("Sprites\\hudKey");
            hudText = Content.Load<Texture2D>("Sprites\\hText");
            hudTree = Content.Load<Texture2D>("Sprites\\sadzonka");
            

            // set all objects to interact with player (Distance)
            playerInteractions = new PlayerInteractions(this, hudTexts, actualScene.getStaticModelsList(), actualScene.getDynamicModelsList(), p);
           
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
            if (playerInteractions.pastCondition == true && playerInteractions.onceBool == true)
            {
                playerInteractions.backCondition = false;
                playerInteractions.onceBool = false;
                simpleEffect = Content.Load<Effect>("Effects\\sepia");
                if (p.Equals("../../../../scene2.xml"))
                {
                    actualScene.unloadContent();
                    LoadSceneFromXml("../../../../scene2.xml");
                }
                if (p.Equals("../../../../scene.xml"))
                {
                    actualScene.unloadContent();
                    LoadSceneFromXml("../../../../scene.xml");
                }
                
            }
            if (playerInteractions.pastCondition == false && playerInteractions.onceBool == true)
            {
                playerInteractions.backCondition = false;
                playerInteractions.onceBool = false;
                simpleEffect = Content.Load<Effect>("Effects\\shadows");
                if (p.Equals("../../../../scene2.xml"))
                {
                    actualScene.unloadContent();
                    LoadSceneFromXml("../../../../scene2.xml");
                }
                if (p.Equals("../../../../scene.xml"))
                {
                    actualScene.unloadContent();
                    LoadSceneFromXml("../../../../scene.xml");
                }
            }

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
        void DrawReflectionMap()
        {
            GraphicsDevice.SetRenderTarget(reflectionRenderTarget);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            Vector3 symetrical = new Vector3(2 * mirror.Position.X - camera.Position.X, camera.Position.Y , 2 * mirror.Position.Z - camera.Position.Z);
            Vector3 mirrorCameraPos = new Vector3(mirror.Position.X, mirror.Position.Y , mirror.Position.Z);

            Vector3 up = Vector3.Cross(symetrical - mirrorCameraPos, camera.Position );

            reflectionViewMatrix = Matrix.CreateLookAt(mirrorCameraPos, symetrical, up);

            foreach (var x in actualScene.staticModelsList)
            {
                if (Vector3.Distance(mirror.Position, x.Position) < 10)
                {
                    x.SetCustomEffect(simpleEffect, false);
                    x.Draw(reflectionViewMatrix, Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                GraphicsDevice.Viewport.AspectRatio,
                0.1f,
                100.0f), "ShadowedScene");
                }
            }

            GraphicsDevice.SetRenderTarget(null);
            reflectionMap = (Texture2D)reflectionRenderTarget;
            
            mirror.RefreshTexture(reflectionMap);
            //Stream stream = new FileStream("test.png", FileMode.Create);
            //reflectionRenderTarget.SaveAsPng(stream, 2048, 2048);
            //stream.Close();

        }
        void DrawGlassMap()
        {
            GraphicsDevice.SetRenderTarget(glassRenderTarget);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
           
            Matrix glassViewMatrix = Matrix.CreateLookAt(camera.Position, glass.Position, Vector3.Up);
            //Matrix glassViewMatrix = camera.View;
            //glassViewMatrix.Translation += camera.View.Forward * Vector3.Distance(glass.Position, camera.Position);
            foreach (var x in actualScene.staticModelsList)
            {
                if (Vector3.Distance(mirror.Position, x.Position) < 25)
                {
                    x.SetCustomEffect(simpleEffect, false);
                    x.Draw(glassViewMatrix, Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                GraphicsDevice.Viewport.AspectRatio,
                0.1f,
                100.0f), "ShadowedScene");
                }
            }

            GraphicsDevice.SetRenderTarget(null);
            glassMap = (Texture2D)glassRenderTarget;

            glass.RefreshTexture(glassMap);
            //Stream stream = new FileStream("test.png", FileMode.Create);
            //reflectionRenderTarget.SaveAsPng(stream, 2048, 2048);
            //stream.Close();

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
            DrawReflectionMap();
            DrawGlassMap();
            // fixing GraphicsDevice after spriteBatch.Begin() method
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
           
            RasterizerState originalRasterizerState = graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rasterizerState;
            skybox.Draw(camera.View, camera.Projection, camera.Position);

            graphics.GraphicsDevice.RasterizerState = originalRasterizerState;
           

            actualScene.Draw();
            glass.Draw(camera);
            mirror.Draw(camera);
         //   hand.Draw(camera);

            cameraWorldMartix = Matrix.Invert(camera.View);
            handWorldMatrix = cameraWorldMartix;

            handWorldMatrix.Translation += (cameraWorldMartix.Forward * 0.33f) +
                                       (cameraWorldMartix.Down * 0.25f) +
                                       (cameraWorldMartix.Right * 0.15f);

            hand.Model = handWorldMatrix;
            hand.Draw(camera);

            if (playerInteractions.drawFight)
            {
            hud.drawHud(spriteBatch, hudTab[camera.BulletsAmount], this);
            hud.drawHud(spriteBatch, hudPointer, this);
            hud.drawHud(spriteBatch, hudHealth2, this);
            hud.drawHealth(spriteBatch, hudHealth, this, camera);
            }
            if (playerInteractions.drawMenu == true)
            {
               hud.drawHud(spriteBatch, hudMenuGame[playerInteractions.gMenu], this);
            }
            if (playerInteractions.drawText == true)
            {
                hud.drawHud(spriteBatch, hudText, this);
            }

            if (drawMainMenu == true)
            {
                hud.drawHud(spriteBatch, hudMenuMain[playerInteractions.mMenu], this);
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


        Vector3 tempParticleVector1 = new Vector3(-6.5f, 5.4f, 1.8f);
        Vector3 tempParticleVector2 = new Vector3(9f, 5.4f, 1.8f);
        Vector3 tempParticleVector3 = new Vector3(46f, 0f, 10f);

        void UpdateTimeParticle()
        {
            const int timeParticlesPerFrame = 1;

            // Create a number of time particles, randomly positioned around a circle.
            for (int i = 0; i < timeParticlesPerFrame; i++)
            {
                timeParticles.AddParticle(tempParticleVector1, Vector3.Zero); // wnetrze
                timeParticles.AddParticle(tempParticleVector1, Vector3.Zero); // wnetrze
                timeParticles.AddParticle(tempParticleVector2, Vector3.Zero); // sadzonka
            }

        }
    }
}
