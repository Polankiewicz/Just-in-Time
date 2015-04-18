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

        List<StaticModel> staticModelsList = new List<StaticModel>();
        Model buildingModel,skaner,enemyModel,sidewalk,shop;

        Enemy enemy;
        PlayerInteractions playerInteractions;

        //display texts
        SpriteFont spriteFont;
        HudTexts hudTexts = new HudTexts();

        


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

            base.Initialize();
        }

        protected override void LoadContent()
        {
           
            

            //objects
            buildingModel = Content.Load<Model>("Models\\7pieter");
            skaner = Content.Load<Model>("Models\\scan");
            enemyModel = Content.Load<Model>("Models\\przeciwnik");
            sidewalk = Content.Load<Model>("Models\\sidewalk_grass");
            shop = Content.Load<Model>("Models\\shop");
            enemy = new Enemy(GraphicsDevice, enemyModel, new Vector3(10, 0.2f, 10), new Vector3(0, 180, 0), 0.005f);


            staticModelsList.Add(new StaticModel(GraphicsDevice, skaner, new Vector3(10, 0, 10), Vector3.Zero, 0.05f));
            staticModelsList.Add(new StaticModel(GraphicsDevice,buildingModel,new Vector3(0,0,-10),Vector3.Zero, 0.005f));
            //buildingsList.Add(new Building(GraphicsDevice,buildingModel,new Vector3(64,1.75f,16),new Vector3(0,90,0), 0.005f));
            //buildingsList.Add(new Building(GraphicsDevice, buildingModel, new Vector3(-24, 1.75f, -24), new Vector3(0, 90, 0), 0.005f));
            //buildingsList.Add(new Building(GraphicsDevice,buildingModel,new Vector3(0,1.75f,-64),new Vector3(0, 0,0), 0.005f));
            //buildingsList.Add(new Building(GraphicsDevice,buildingModel,new Vector3(100,0,0)));
            staticModelsList.Add(new StaticModel(GraphicsDevice, shop, new Vector3(-10,0,20),new Vector3(0,90,0),0.01f));
            for (int i = -5; i < 10; i++ ) //proste tworzenie podlogi z elemenu sidewalk_grass
            {
                for (int j = 0; j < 20; j++)
                {
                    staticModelsList.Add(new StaticModel(GraphicsDevice, sidewalk, new Vector3(j*6.25f, 0, i * 5), new Vector3(0,j*180, 0),0.001f));
                    
                }
            }

            //texts
            spriteFont = Content.Load<SpriteFont>("Sprites\\PressXtoInteract");

            // set all objects to interact with player
            playerInteractions = new PlayerInteractions(this, hudTexts, staticModelsList);


            // collisions
            //////////////////////////////////////// TODO: set staticModels and dynamicModels //////////////////////////////////////////////
            camera.setCameraCollision(enemy);
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
    
            enemy.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            //floor.Draw(camera, effect);


            hudTexts.drawText(spriteBatch, spriteFont); //draw gui texts
            // spriteBatch.Begin() set GraphicsDevice.DepthStencilState to DepthStencilState.None
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            enemy.Draw(camera);
          
            foreach (StaticModel b in staticModelsList) b.Draw(camera);

            base.Draw(gameTime);

            
        }

    }
}
