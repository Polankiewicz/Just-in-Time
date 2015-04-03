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

        List<Building> buildingsList = new List<Building>();
        Model buildingModel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            camera = new Camera(this, new Vector3(10f, 1f, 5f), Vector3.Zero, 5f);
            Components.Add(camera);
          //  graphics.ToggleFullScreen();
            floor = new Floor(GraphicsDevice, 20, 20);
            effect = new BasicEffect(GraphicsDevice);
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
           
            spriteBatch = new SpriteBatch(GraphicsDevice);

            buildingModel = Content.Load<Model>("Models\\7pieter");
            Model skaner = Content.Load<Model>("Models\\scan");
            buildingsList.Add(new Building(GraphicsDevice,buildingModel,new Vector3(0,1.75f,0),Vector3.Zero));
         //   buildingsList.Add(new Building(GraphicsDevice,buildingModel,new Vector3(64,1.75f,16),new Vector3(0,90,0)));
           // buildingsList.Add(new Building(GraphicsDevice,buildingModel,new Vector3(-24,1.75f,-24),new Vector3(0,90,0)));
            //buildingsList.Add(new Building(GraphicsDevice,buildingModel,new Vector3(0,1.75f,-64),new Vector3(0, 0,0)));
         //   buildingsList.Add(new Building(GraphicsDevice,buildingModel,new Vector3(100,0,0)));
            buildingsList.Add(new Building(GraphicsDevice,skaner,new Vector3(-10,2,-10),Vector3.Zero));            
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

            floor.Draw(camera, effect);

            foreach (Building b in buildingsList) b.Draw(camera);
            base.Draw(gameTime);
        }
    }
}
