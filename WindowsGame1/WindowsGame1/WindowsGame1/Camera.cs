using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    public class Camera : GameComponent
    {
        private Vector3 cameraPosition;
        private Vector3 cameraRotation;
        private float cameraSpeed;
        private Vector3 cameraLookAt;
        //mouse
        private Vector3 mouseRotationBuffer;
        private Vector3 holderForMouseRotation;
        private MouseState currentMouseState;
        private MouseState prevMouseState;
        //private float mouseRotationSpeed;
        private Game game;
        // gamePad
        private GamePadState gamePad;
        // collisions
        CameraCollisions cameraCollisions;


        //Properties

        public Vector3 Position
        {
            get { return cameraPosition; }
            set
            {
                cameraPosition = value;
                UpdateLookAt();
            }
        }

        public Vector3 Rotation
        {
            get { return cameraRotation; }
            set
            {
                cameraRotation = value;
                UpdateLookAt();
            }
        }

        public Matrix Projection
        {
            get;
            protected set;
        }

        public Matrix View
        {
            get { return Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up); }
        }


        public Camera(Game game, Vector3 position, Vector3 rotation, float speed) : base(game)
        {
            cameraSpeed = speed;

            //setup projection matrix
            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 
                Game.GraphicsDevice.Viewport.AspectRatio, 
                0.05f, 
                1000.0f);

            MoveTo(position, rotation);
            this.game = game;
            
            prevMouseState = Mouse.GetState();

        }

        public void setCameraCollision(List<DynamicModel> dynamicModelsList, List<StaticModel> staticModelsList)
        {
            cameraCollisions = new CameraCollisions(this, dynamicModelsList, staticModelsList);
            // maybe player interactions also should be here... :v
        }

        //set camera's position and rotation
        private void MoveTo(Vector3 pos, Vector3 rot)
        {
            Position = pos;
            Rotation = rot;
        }

        //update the look at vector
        private void UpdateLookAt()
        {
            //build a rotation matrix
            Matrix rotationMatrix = Matrix.CreateRotationX(cameraRotation.X) * Matrix.CreateRotationY(cameraRotation.Y);
            //build look at offset vector
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);
            //update our camera's look at vector
            cameraLookAt = cameraPosition + lookAtOffset;
        }


        //method that simulate movement
        private Vector3 PreviewMove(Vector3 amount)
        {
            Matrix rotate = Matrix.CreateRotationY(cameraRotation.Y);
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);
            return cameraPosition + movement;
        }

        //method that actually moves the camera
        public void Move(Vector3 scale)
        {
            MoveTo(PreviewMove(scale), Rotation);
        }


        //update method
        public override void Update(GameTime gameTime)
        {
            if (game.IsActive)
            {
                
                // update bounding sphares position for collision with player/camera
                cameraCollisions.updateBoundingSpherePosition();


                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; // delta time

                currentMouseState = Mouse.GetState();


                //////////////////////////// handle movement ////////////////////////////////////////
                KeyboardState ks = Keyboard.GetState();
                gamePad = GamePad.GetState(PlayerIndex.One);

                Vector3 moveVector = Vector3.Zero;

                // left ThumbStick control
                moveVector.Z = gamePad.ThumbSticks.Left.Y;
                moveVector.X = -gamePad.ThumbSticks.Left.X;

                // keyboard and DPad control
                if (ks.IsKeyDown(Keys.W) || gamePad.DPad.Up == ButtonState.Pressed)
                    moveVector.Z = 1;
                if (ks.IsKeyDown(Keys.S) || gamePad.DPad.Down == ButtonState.Pressed)
                    moveVector.Z = -1;
                if (ks.IsKeyDown(Keys.A) || gamePad.DPad.Left == ButtonState.Pressed)
                    moveVector.X = 1;
                if (ks.IsKeyDown(Keys.D) || gamePad.DPad.Right == ButtonState.Pressed)
                    moveVector.X = -1;
                if (ks.IsKeyDown(Keys.T))
                    moveVector.Y= 100;

                //gravity
                //moveVector.Y = -100;

                if (moveVector != Vector3.Zero)
                {
                    //normalize the vector
                    moveVector.Normalize();
                    moveVector *= dt * cameraSpeed;

                    // simulate next move and check for collision
                    if (cameraCollisions.cameraNextMoveCollisionDetect(PreviewMove(moveVector)))
                    {
                        Move(moveVector);
                    }


                }

                //////////////////////////// handle rotation ////////////////////////////////////////
                float deltaX, deltaY;

                

                if (currentMouseState != prevMouseState )
                {
                    //cache mouse location
                    deltaX = currentMouseState.X - (Game.GraphicsDevice.Viewport.Width / 2);
                    deltaY = currentMouseState.Y - (Game.GraphicsDevice.Viewport.Height / 2);

                    mouseRotationBuffer.X -= 0.03f * deltaX * dt;
                    mouseRotationBuffer.Y -= 0.03f * deltaY * dt;

                    if (mouseRotationBuffer.Y < MathHelper.ToRadians(-55.0f))
                        mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(-55.0f));
                    if (mouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                        mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));

                    //Rotation = new Vector3(-MathHelper.Clamp(mouseRotationBuffer.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)),
                    //    MathHelper.WrapAngle(mouseRotationBuffer.X), 0);
                    holderForMouseRotation.X = -MathHelper.Clamp(mouseRotationBuffer.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f));
                    holderForMouseRotation.Y = MathHelper.WrapAngle(mouseRotationBuffer.X);
                    holderForMouseRotation.Z =0;
                    Rotation = holderForMouseRotation;

                    deltaX = 0;
                    deltaY = 0;

                }
                if (gamePad.ThumbSticks.Right != Vector2.Zero)
                {
                    //Rotation = new Vector3(MathHelper.Clamp(-gamePad.ThumbSticks.Right.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)),
                    //-gamePad.ThumbSticks.Right.X, 0);

                    if (holderForMouseRotation.X < MathHelper.ToRadians(-75.0f))
                        holderForMouseRotation.X = holderForMouseRotation.X - (holderForMouseRotation.X - MathHelper.ToRadians(-75.0f));
                    if (holderForMouseRotation.X > MathHelper.ToRadians(55.0f))
                        holderForMouseRotation.X = holderForMouseRotation.X - (holderForMouseRotation.X - MathHelper.ToRadians(55.0f));

                    holderForMouseRotation.X += -MathHelper.Clamp(gamePad.ThumbSticks.Right.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)) * 0.05f;
                    holderForMouseRotation.Y += MathHelper.WrapAngle(-gamePad.ThumbSticks.Right.X) * 0.05f;
                    holderForMouseRotation.Z = 0;
                    Rotation = holderForMouseRotation;

                    
                }

                Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
                

                prevMouseState = currentMouseState;

                base.Update(gameTime);
            }
        }


    }
}
