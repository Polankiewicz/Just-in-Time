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

       
        PlayerInteractions playerInteractions;

        //display texts
        SpriteFont spriteFont;
        HudTexts hudTexts = new HudTexts();

        Scene actualScene;
        

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

            actualScene.AddStaticModel("Models\\scan", new Vector3(10, 0, 10), Vector3.Zero, 0.05f);
            actualScene.AddStaticModel("Models\\7pieter", new Vector3(0, 0, -10), Vector3.Zero, 0.005f);
            actualScene.AddStaticModel("Models\\7pieter", new Vector3(50, 0, 30), new Vector3(0, 90, 0), 0.005f);
            actualScene.AddStaticModel("Models\\7pieter", new Vector3(0, 0, 30), Vector3.Zero, 0.005f);
            actualScene.AddStaticModel("Models\\7pieter", new Vector3(-50, 0, 20), new Vector3(0, 45, 0), 0.005f);
            actualScene.AddStaticModel("Models\\7pieter", new Vector3(-40, 0, 60), new Vector3(0, 135, 0), 0.005f);
           
            actualScene.AddStaticModel("Models\\shop", new Vector3(-20, 0, 18), new Vector3(0, 135, 0), 0.01f);

            for (int i = -5; i < 10; i++) //proste tworzenie podlogi z elementu sidewalk_grass
            {
                for (int j = 0; j < 20; j++)
                {
                    actualScene.AddStaticModel("Models\\sidewalk_grass", new Vector3(-50 + j * 6.25f, 0, i * 5), new Vector3(0, 0, 0), 0.001f);

                }
            }

            actualScene.AddDynamicModel("Models\\przeciwnik", new Vector3(10, 0.2f, 10), new Vector3(0, 180, 0), 0.005f);

            

            //texts
            spriteFont = Content.Load<SpriteFont>("Sprites\\PressXtoInteract");

            // set all objects to interact with player
            playerInteractions = new PlayerInteractions(this, hudTexts, actualScene.getStaticModelsList());


            // collisions
            //////////////////////////////////////// TODO: set staticModels and dynamicModels //////////////////////////////////////////////
            camera.setCameraCollision(null,null); 
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
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            hudTexts.drawText(spriteBatch, spriteFont); //draw gui texts
         
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
           
    
            actualScene.Draw();
           

            base.Draw(gameTime);

            
        }

    }
}
