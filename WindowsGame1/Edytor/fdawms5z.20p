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
         

            var listNames = new List<string>();
            listNames.Add("Models\\scan");
            listNames.Add("Models\\7pieter");
            listNames.Add("Models\\shop");
            listNames.Add("Models\\sidewalk_grass");
            listNames.Add("Models\\2klatki-13pieter");
            listNames.Add("Models\\2klatki-3pietra");
            listNames.Add("Models\\2klatki-7pieter");
            listNames.Add("Models\\3klatki-5pieter");
            listNames.Add("Models\\metal_fence");
            listNames.Add("Models\\street_dumbster");
            listNames.Add("Models\\street_lantern");
           
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
        /// 

        ///////////////////////////////////////////////////////////////////// Variables for camera move/rotation ///////////////////////////////
        GamePadState gamePad;
        Vector3 holderForMouseRotation;
        MouseState currentMouseState;
        MouseState prevMouseState;
        Vector3 mouseRotationBuffer;

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            /////////////////////////////////////////////////////////////////// Camera magic ///////////////////////////////////////////////////
            KeyboardState ks = Keyboard.GetState();
            gamePad = GamePad.GetState(PlayerIndex.One);
            
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; // delta time
            Vector3 moveVector = Vector3.Zero;

            // left ThumbStick control
            moveVector.Z = gamePad.ThumbSticks.Left.Y;
            moveVector.X = -gamePad.ThumbSticks.Left.X;

            if (ks.IsKeyDown(Keys.W) )
                moveVector.Z = 1;
            if (ks.IsKeyDown(Keys.S) )
                moveVector.Z = -1;
            if (ks.IsKeyDown(Keys.A) )
                moveVector.X = 1;
            if (ks.IsKeyDown(Keys.D) )
                moveVector.X = -1;
            if (ks.IsKeyDown(Keys.T))
                moveVector.Y = 100;
            if (ks.IsKeyDown(Keys.Y))
                moveVector.Y = -100;

            if (moveVector != Vector3.Zero)
            {
                //normalize the vector
                moveVector.Normalize();
                moveVector *= dt * 50f;
                camera.Move(moveVector);
            }


            if (gamePad.ThumbSticks.Right != Vector2.Zero)
            {
                if (holderForMouseRotation.X < MathHelper.ToRadians(-75.0f))
                    holderForMouseRotation.X = holderForMouseRotation.X - (holderForMouseRotation.X - MathHelper.ToRadians(-75.0f));
                if (holderForMouseRotation.X > MathHelper.ToRadians(55.0f))
                    holderForMouseRotation.X = holderForMouseRotation.X - (holderForMouseRotation.X - MathHelper.ToRadians(55.0f));

                holderForMouseRotation.X += -MathHelper.Clamp(gamePad.ThumbSticks.Right.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)) * 0.05f;
                holderForMouseRotation.Y += MathHelper.WrapAngle(-gamePad.ThumbSticks.Right.X) * 0.05f;
                holderForMouseRotation.Z = 0;
                camera.Rotation = holderForMouseRotation;
            }

            float deltaX, deltaY;
            currentMouseState = Mouse.GetState();

            if (currentMouseState != prevMouseState && ks.IsKeyDown(Keys.Q))
            {
                //cache mouse location
                deltaX = currentMouseState.X - (this.GraphicsDevice.Viewport.Width / 2);
                deltaY = currentMouseState.Y - (this.GraphicsDevice.Viewport.Height / 2);

                mouseRotationBuffer.X -= 0.03f * deltaX * dt;
                mouseRotationBuffer.Y -= 0.03f * deltaY * dt;

                if (mouseRotationBuffer.Y < MathHelper.ToRadians(-55.0f))
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(-55.0f));
                if (mouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));

                holderForMouseRotation.X = -MathHelper.Clamp(mouseRotationBuffer.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f));
                holderForMouseRotation.Y = MathHelper.WrapAngle(mouseRotationBuffer.X);
                holderForMouseRotation.Z = 0;
                camera.Rotation = holderForMouseRotation;

                deltaX = 0;
                deltaY = 0;

                Mouse.SetPosition(this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height / 2);
                prevMouseState = currentMouseState;
            }

            /////////////////////////////////////////////////////////////////////////// End of magic camera control //////////////////////////////


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
