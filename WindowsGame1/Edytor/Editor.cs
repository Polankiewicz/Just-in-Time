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
using WindowsGame1;

namespace Editor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    
    public class Editor : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private readonly EditorForm edytor;
        Scene actualScene;
        Camera camera;
        BasicEffect effect;
        

        public Editor(EditorForm edytor)
        {
            this.edytor = edytor;
            
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = edytor.ViewportSize.Width,
                PreferredBackBufferHeight = edytor.ViewportSize.Height
            };
            graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            System.Windows.Forms.Control.FromHandle(Window.Handle).VisibleChanged += MainGame_VisibleChanged;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            
       
        }
        void MainGame_VisibleChanged(object sender, System.EventArgs e)
        {
            if (System.Windows.Forms.Control.FromHandle(Window.Handle).Visible)
                System.Windows.Forms.Control.FromHandle(Window.Handle).Visible = false;
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = edytor.CanvasHandle;
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
            effect = new BasicEffect(GraphicsDevice);
            actualScene = new Scene(Content, GraphicsDevice, camera);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //actualScene.AddStaticModel("Models\\scan", new Vector3(10, 0, 10), Vector3.Zero, 0.05f, "scaner");
            //actualScene.AddStaticModel("Models\\7pieter", new Vector3(40, 0, -20), Vector3.Zero, 0.005f, "block");
            //actualScene.AddStaticModel("Models\\7pieter", new Vector3(0, 0, 30), Vector3.Zero, 0.005f, "block");
            //actualScene.AddStaticModel("Models\\7pieter", new Vector3(40, 0, 30), Vector3.Zero, 0.005f, "block");
           
            //actualScene.AddStaticModel("Models\\7pieter", new Vector3(60, 0, 30), new Vector3(0, 90, 0), 0.005f, "block");
            //actualScene.AddStaticModel("Models\\7pieter", new Vector3(20, 0, 0), new Vector3(0, 90, 0), 0.005f, "block");
            //actualScene.AddStaticModel("Models\\shop", new Vector3(40, 0, 0), new Vector3(0, -45, 0), 0.01f, "shop");
            //edytor.SetDataSource(actualScene.staticModelsList);
            // TODO: use this.Content to load your game content here
            var listNames = new List<string>();
            listNames.Add("Models\\scan");
            listNames.Add("Models\\7pieter");
            listNames.Add("Models\\shop");
            edytor.SetComboBoxSource(listNames, actualScene);
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
           //playerInteractions.catchInteraction(camera);
         //   camera.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // fixing GraphicsDevice after spriteBatch.Begin() method
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            
            // TODO: Add your drawing code here
            actualScene.Draw();
            base.Draw(gameTime);
        }
    }
}
