﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    public class Camera : GameComponent
    {
        // camera
        private Vector3 cameraPosition;
        private Vector3 cameraRotation;
        private float cameraSpeed;
        public Vector3 cameraLookAt { get; set; }
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
        // gravity
        private float fallingspeed;
        // list of bullets
        List<Bullet> listOfBullets;
        private int bulletsAmount;
        public List<StaticModel> equipment = new List<StaticModel>();
        private int hp = 199;
        public Rectangle healthRectangle;
        //Properties

        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }
        public Matrix rotationMatrix { get; set; }
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

        public int BulletsAmount
        {
            get { return bulletsAmount; }
            set { bulletsAmount = value; }
        }

        public Matrix View
        {
            get { return Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up); }
        }


        public Camera(Game game, Vector3 position, Vector3 rotation, float speed)
            : base(game)
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
            fallingspeed = 0;
            listOfBullets = new List<Bullet>();
            bulletsAmount = 12;
        }

        public void setCameraCollision(Scene actualScene)
        {
            cameraCollisions = new CameraCollisions(this, (Game1)game, listOfBullets);
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
            rotationMatrix = Matrix.CreateRotationX(cameraRotation.X) * Matrix.CreateRotationY(cameraRotation.Y);
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

        public void clearCameraLookAt()
        {
            mouseRotationBuffer = Vector3.Zero;
            cameraLookAt = Vector3.Zero;
            Rotation = Vector3.Zero;
            holderForMouseRotation = Vector3.Zero;
        }

        //update method
        public override void Update(GameTime gameTime)
        {
            if (game.IsActive || game.ToString() == "Editor.Editor")
            {
                // update bounding sphares position for collision with player/camera
                if (game.ToString() != "Editor.Editor")
                    cameraCollisions.updateBoundingSpherePosition();

                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; // delta time

                currentMouseState = Mouse.GetState();

                //////////////////////////// handle movement ////////////////////////////////////////
                KeyboardState ks = Keyboard.GetState();
                gamePad = GamePad.GetState(PlayerIndex.One);

                Vector3 moveVector = Vector3.Zero;

                // left ThumbStick control
                moveVector.Z = gamePad.ThumbSticks.Left.Y/3;
                moveVector.X = -gamePad.ThumbSticks.Left.X/3;

                // keyboard and DPad control
                if (ks.IsKeyDown(Keys.W) || gamePad.DPad.Up == ButtonState.Pressed)
                    moveVector.Z = 0.5f;
                if (ks.IsKeyDown(Keys.S) || gamePad.DPad.Down == ButtonState.Pressed)
                    moveVector.Z = -0.5f;
                if (ks.IsKeyDown(Keys.A) || gamePad.DPad.Left == ButtonState.Pressed)
                    moveVector.X = 0.5f;
                if (ks.IsKeyDown(Keys.D) || gamePad.DPad.Right == ButtonState.Pressed)
                    moveVector.X = -0.5f;

                if (game.ToString() == "Editor.Editor") //editor only controls
                {
                    if (ks.IsKeyDown(Keys.T))
                        moveVector.Y = 100;
                    if (ks.IsKeyDown(Keys.Y))
                        moveVector.Y = -100;
                }

                // TEMP
                if (ks.IsKeyDown(Keys.T))
                    moveVector.Y = 100;
                if (ks.IsKeyDown(Keys.Y))
                    moveVector.Y = -100;

                // gravity
                if (game.ToString() != "Editor.Editor")
                {
                    //fallingspeed -= 1;
                    moveVector.Y = -0.5f;

                    // floor
                    if (!cameraCollisions.cameraNextMoveCollisionDetectWithFloor(PreviewMove(moveVector))
                            || !cameraCollisions.cameraNextMoveCollisionDetectWithStairs(PreviewMove(moveVector)))
                    {
                        moveVector.Y = 0;
                        //fallingspeed = 0;
                    }

                    // stairs
                    if (!cameraCollisions.cameraNextMoveCollisionDetectWithStairs(PreviewMove(moveVector)))
                    {
                        moveVector.Y = 0.5f;
                    }

                }

                if (moveVector != Vector3.Zero)
                {
                    //normalize the vector
                    moveVector.Normalize();
                    moveVector *= dt * cameraSpeed;

                    // simulate next move and check for collision
                    if (game.ToString() == "Editor.Editor") Move(moveVector);
                    else if (cameraCollisions.cameraNextMoveCollisionDetect(PreviewMove(moveVector)))
                    {
                        Move(moveVector);
                    }
                }

                //////////////////////////// handle rotation ////////////////////////////////////////
                float deltaX, deltaY;


                if (game.ToString() != "Editor.Editor" || ks.IsKeyDown(Keys.Q) || currentMouseState.RightButton == ButtonState.Pressed)
                {
                    if (currentMouseState != prevMouseState)
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
                        holderForMouseRotation.Z = 0;
                        Rotation = holderForMouseRotation;

                        deltaX = 0;
                        deltaY = 0;
                        Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);

                        prevMouseState = currentMouseState;
                    }
                }
                if (gamePad.ThumbSticks.Right != Vector2.Zero)
                {
                    //Rotation = new Vector3(MathHelper.Clamp(-gamePad.ThumbSticks.Right.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)),
                    //-gamePad.ThumbSticks.Right.X, 0);
                    
                    if (holderForMouseRotation.X < MathHelper.ToRadians(-75.0f))
                        holderForMouseRotation.X = holderForMouseRotation.X - (holderForMouseRotation.X - MathHelper.ToRadians(-75.0f));
                    if (holderForMouseRotation.X > MathHelper.ToRadians(55.0f))
                        holderForMouseRotation.X = holderForMouseRotation.X - (holderForMouseRotation.X - MathHelper.ToRadians(55.0f));

                    holderForMouseRotation.X += -MathHelper.Clamp((gamePad.ThumbSticks.Right.Y/3)*2, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)) * 0.05f;
                    holderForMouseRotation.Y += MathHelper.WrapAngle(-gamePad.ThumbSticks.Right.X/3)*2 * 0.05f;
                    holderForMouseRotation.Z = 0;
                    Rotation = holderForMouseRotation;
                }


                //////////////////////////// handle shooting ////////////////////////////////////////
                if (game.ToString() != "Editor.Editor")
                {
                    // new bullet
                    if (gamePad.Triggers.Right == 1f || currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (bulletsAmount > 0)
                        {
                            if (listOfBullets.Count == 0) // shooting only when previous bullet was destroyed
                            {
                                listOfBullets.Add(new Bullet(Position));
                                bulletsAmount--;
                            }
                        }
                        else
                            bulletsAmount = 6;
                    }

                    // update bullet position
                    for (int i = 0; i < listOfBullets.Count; i++)
                    {
                        // TODO: przeniesc to do klasy Bullet pod jakas metode i przerobic na konkretne strzelanie tam gdzie sie paczy gracz
                        // i sie zamieni ciagle tworzenie wektora na jakiegos tam stalego bo pamiec :P
                        float LookAtX = cameraLookAt.X;

                        if (Rotation.Y < -1.7f || Rotation.Y > 1.7f)
                            LookAtX = -LookAtX;

                        BoundingSphere xxx = listOfBullets[i].boundingSphere;

                        xxx.Center.Z += 10 * dt * Rotation.X * LookAtX;
                        xxx.Center.X += 10 * dt * Rotation.Y * cameraLookAt.Y;
                        xxx.Center.Y = Position.Y;
                        listOfBullets[i].boundingSphere = xxx;

                        //Position = listOfBullets[i].boundingSphere.Center;
                    }

                    // check for bullet collision
                    cameraCollisions.bulletCollisionWithEnemy();

                    // delete bullet if is too dar away
                    for (int i = 0; i < listOfBullets.Count; i++)
                    {
                        if (listOfBullets[i].checkIfBulletIsTooFarAway(Position))
                            listOfBullets.RemoveAt(i);
                        //throw new Exception("cameraLookAt " + Rotation);
                    }
                }


                healthRectangle = new Rectangle(28, 103, Hp, 29);

                base.Update(gameTime);
            }
        }


    }
}
