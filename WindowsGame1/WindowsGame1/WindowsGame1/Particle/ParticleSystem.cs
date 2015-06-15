using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace WindowsGame1
{
    public abstract class ParticleSystem : DrawableGameComponent
    {
        // Settings class controls the appearance and animation of this particle system.
        protected readonly ParticleSettings _settings = new ParticleSettings();

        // Custom effect for drawing particles. This computes the particle
        // animation entirely in the vertex shader: no per-particle CPU work required!
        protected Effect _particleEffect;


        // Shortcuts for accessing frequently changed effect parameters.
        EffectParameter _effectViewParameter;
        EffectParameter _effectProjectionParameter;
        protected EffectParameter _effectViewportScaleParameter;
        protected EffectParameter _effectTimeParameter;


        // An array of particles, treated as a circular queue.
        protected ParticleVertex[] _particles;


        // A vertex buffer holding our particles. This contains the same data as
        // the particles array, but copied across to where the GPU can access it.
        protected DynamicVertexBuffer _vertexBuffer;


        // Index buffer turns sets of four vertices into particle quads (pairs of triangles).
        protected IndexBuffer _indexBuffer;


        protected int _firstActiveParticle;
        protected int _firstNewParticle;
        protected int _firstFreeParticle;
        int _firstRetiredParticle;


        // Store the current time, in seconds.
        protected float _currentTime;


        // Count how many times Draw has been called. This is used to know
        // when it is safe to retire old particles back into the free list.
        protected int _drawCounter;


        // Shared random number generator.
        static readonly Random Random = new Random();
        internal Main.Game _game;


        /// <summary>
        /// Constructor.
        /// </summary>
        protected ParticleSystem(Main.Game game)
            : base(game)
        {
            _game = game;
        }
        //wot is that??

        /// <summary>
        /// Initializes the component.
        /// </summary >
        public override void Initialize()
        {
            InitializeSettings(_settings);

            // Allocate the particle array, and fill in the corner fields (which never change).
            _particles = new ParticleVertex[_settings.MaxParticles * 4];

            for (int i = 0; i < _settings.MaxParticles; i++)
            {
                _particles[i * 4 + 0].Corner = new Short2(-1, -1);
                _particles[i * 4 + 1].Corner = new Short2(1, -1);
                _particles[i * 4 + 2].Corner = new Short2(1, 1);
                _particles[i * 4 + 3].Corner = new Short2(-1, 1);
            }

            base.Initialize();
        }


        /// <summary>
        /// Derived particle system classes should override this method
        /// and use it to initalize their tweakable settings.
        /// </summary>
        protected abstract void InitializeSettings(ParticleSettings settings);


        /// <summary>
        /// Loads graphics for the particle system.
        /// </summary>
        protected override void LoadContent()
        {
            LoadParticleEffect();

            // Create a dynamic vertex buffer.
            _vertexBuffer = new DynamicVertexBuffer(GraphicsDevice, ParticleVertex.VertexDeclaration,
                                                   _settings.MaxParticles * 4, BufferUsage.WriteOnly);

            // Create and populate the index buffer.
            ushort[] indices = new ushort[_settings.MaxParticles * 6];

            for (int i = 0; i < _settings.MaxParticles; i++)
            {
                indices[i * 6 + 0] = (ushort)(i * 4 + 0);
                indices[i * 6 + 1] = (ushort)(i * 4 + 1);
                indices[i * 6 + 2] = (ushort)(i * 4 + 2);

                indices[i * 6 + 3] = (ushort)(i * 4 + 0);
                indices[i * 6 + 4] = (ushort)(i * 4 + 2);
                indices[i * 6 + 5] = (ushort)(i * 4 + 3);
            }

            _indexBuffer = new IndexBuffer(GraphicsDevice, typeof(ushort), indices.Length, BufferUsage.WriteOnly);

            _indexBuffer.SetData(indices);
        }


        /// <summary>
        /// Helper for loading and initializing the particle effect.
        /// </summary>
        void LoadParticleEffect()
        {
            Effect effect = _game.Content.Load<Effect>("UnderMineContent/Efekty/ParticleEffect");

            // If we have several particle systems, the content manager will return
            // a single shared effect instance to them all. But we want to preconfigure
            // the effect with parameters that are specific to this particular
            // particle system. By cloning the effect, we prevent one particle system
            // from stomping over the parameter settings of another.

            _particleEffect = effect.Clone();

            EffectParameterCollection parameters = _particleEffect.Parameters;

            // Look up shortcuts for parameters that change every frame.
            _effectViewParameter = parameters["View"];
            _effectProjectionParameter = parameters["Projection"];
            _effectViewportScaleParameter = parameters["ViewportScale"];
            _effectTimeParameter = parameters["CurrentTime"];

            // Set the values of parameters that do not change.
            parameters["Duration"].SetValue((float)_settings.Duration.TotalSeconds);
            parameters["DurationRandomness"].SetValue(_settings.DurationRandomness);
            parameters["Gravity"].SetValue(_settings.Gravity);
            parameters["EndVelocity"].SetValue(_settings.EndVelocity);
            parameters["MinColor"].SetValue(_settings.MinColor.ToVector4());
            parameters["MaxColor"].SetValue(_settings.MaxColor.ToVector4());

            parameters["RotateSpeed"].SetValue(
                new Vector2(_settings.MinRotateSpeed, _settings.MaxRotateSpeed));

            parameters["StartSize"].SetValue(
                new Vector2(_settings.MinStartSize, _settings.MaxStartSize));

            parameters["EndSize"].SetValue(
                new Vector2(_settings.MinEndSize, _settings.MaxEndSize));

            // Load the particle texture, and set it onto the effect.
            Texture2D texture = _game.Content.Load<Texture2D>(_settings.TextureName);

            parameters["Texture"].SetValue(texture);
        }


        /// <summary>
        /// Updates the particle system.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (gameTime == null)
                throw new ArgumentNullException("gameTime");

            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            RetireActiveParticles();
            FreeRetiredParticles();

            // If we let our timer go on increasing for ever, it would eventually
            // run out of floating point precision, at which point the particles
            // would render incorrectly. An easy way to prevent this is to notice
            // that the time value doesn't matter when no particles are being drawn,
            // so we can reset it back to zero any time the active queue is empty.

            if (_firstActiveParticle == _firstFreeParticle)
                _currentTime = 0;

            if (_firstRetiredParticle == _firstActiveParticle)
                _drawCounter = 0;
        }


        /// <summary>
        /// Helper for checking when active particles have reached the end of
        /// their life. It moves old particles from the active area of the queue
        /// to the retired section.
        /// </summary>
        void RetireActiveParticles()
        {
            float particleDuration = (float)_settings.Duration.TotalSeconds;

            while (_firstActiveParticle != _firstNewParticle)
            {
                // Is this particle old enough to retire?
                // We multiply the active particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                float particleAge = _currentTime - _particles[_firstActiveParticle * 4].Time;

                if (particleAge < particleDuration)
                    break;

                // Remember the time at which we retired this particle.
                _particles[_firstActiveParticle * 4].Time = _drawCounter;

                // Move the particle from the active to the retired queue.
                _firstActiveParticle++;

                if (_firstActiveParticle >= _settings.MaxParticles)
                    _firstActiveParticle = 0;
            }
        }


        /// <summary>
        /// Helper for checking when retired particles have been kept around long
        /// enough that we can be sure the GPU is no longer using them. It moves
        /// old particles from the retired area of the queue to the free section.
        /// </summary>
        void FreeRetiredParticles()
        {
            while (_firstRetiredParticle != _firstActiveParticle)
            {
                // Has this particle been unused long enough that
                // the GPU is sure to be finished with it?
                // We multiply the retired particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                int age = _drawCounter - (int)_particles[_firstRetiredParticle * 4].Time;

                // The GPU is never supposed to get more than 2 frames behind the CPU.
                // We add 1 to that, just to be safe in case of buggy drivers that
                // might bend the rules and let the GPU get further behind.
                if (age < 3)
                    break;

                // Move the particle from the retired to the free queue.
                _firstRetiredParticle++;

                if (_firstRetiredParticle >= _settings.MaxParticles)
                    _firstRetiredParticle = 0;
            }
        }


        /// <summary>
        /// Draws the particle system.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Restore the vertex buffer contents if the graphics device was lost.
            if (_vertexBuffer.IsContentLost)
            {
                _vertexBuffer.SetData(_particles);
            }

            // If there are any particles waiting in the newly added queue,
            // we'd better upload them to the GPU ready for drawing.
            if (_firstNewParticle != _firstFreeParticle)
            {
                AddNewParticlesToVertexBuffer();
            }

            // If there are any active particles, draw them now!
            if (_firstActiveParticle != _firstFreeParticle)
            {
                _game.GraphicsDevice.BlendState = _settings.BlendState;
                _game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

                // Set an effect parameter describing the viewport size. This is
                // needed to convert particle sizes into screen space point sizes.
                _effectViewportScaleParameter.SetValue(new Vector2(0.5f / _game.GraphicsDevice.Viewport.AspectRatio, -0.5f));

                // Set an effect parameter describing the current time. All the vertex
                // shader particle animation is keyed off this value.
                _effectTimeParameter.SetValue(_currentTime);

                // Set the particle vertex and index buffer.
                _game.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
                _game.GraphicsDevice.Indices = _indexBuffer;

                // Activate the particle effect.
                foreach (EffectPass pass in _particleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    if (_firstActiveParticle < _firstFreeParticle)
                    {
                        // If the active particles are all in one consecutive range,
                        // we can draw them all in a single call.
                        _game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                     _firstActiveParticle * 4, (_firstFreeParticle - _firstActiveParticle) * 4,
                                                     _firstActiveParticle * 6, (_firstFreeParticle - _firstActiveParticle) * 2);
                    }
                    else
                    {
                        // If the active particle range wraps past the end of the queue
                        // back to the start, we must split them over two draw calls.
                        _game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                     _firstActiveParticle * 4, (_settings.MaxParticles - _firstActiveParticle) * 4,
                                                     _firstActiveParticle * 6, (_settings.MaxParticles - _firstActiveParticle) * 2);

                        if (_firstFreeParticle > 0)
                        {
                            _game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                         0, _firstFreeParticle * 4,
                                                         0, _firstFreeParticle * 2);
                        }
                    }
                }

                // Reset some of the renderstates that we changed,
                // so as not to mess up any other subsequent drawing.
                _game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }

            _drawCounter++;
        }


        /// <summary>
        /// Helper for uploading new particles from our managed
        /// array to the GPU vertex buffer.
        /// </summary>
        public void AddNewParticlesToVertexBuffer()
        {
            int stride = ParticleVertex.SizeInBytes;

            if (_firstNewParticle < _firstFreeParticle)
            {
                // If the new particles are all in one consecutive range,
                // we can upload them all in a single call.
                _vertexBuffer.SetData(_firstNewParticle * stride * 4, _particles,
                                     _firstNewParticle * 4,
                                     (_firstFreeParticle - _firstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);
            }
            else
            {
                // If the new particle range wraps past the end of the queue
                // back to the start, we must split them over two upload calls.
                _vertexBuffer.SetData(_firstNewParticle * stride * 4, _particles,
                                     _firstNewParticle * 4,
                                     (_settings.MaxParticles - _firstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);

                if (_firstFreeParticle > 0)
                {
                    _vertexBuffer.SetData(0, _particles,
                                         0, _firstFreeParticle * 4,
                                         stride, SetDataOptions.NoOverwrite);
                }
            }

            // Move the particles we just uploaded from the new to the active queue.
            _firstNewParticle = _firstFreeParticle;
        }


        /// <summary>
        /// Sets the camera view and projection matrices
        /// that will be used to draw this particle system.
        /// </summary>
        public void SetCamera(Matrix view, Matrix projection)
        {
            _effectViewParameter.SetValue(view);
            _effectProjectionParameter.SetValue(projection);
        }


        /// <summary>
        /// Adds a new particle to the system.
        /// </summary>
        public void AddParticle(Vector3 position, Vector3 velocity)
        {
            // Figure out where in the circular queue to allocate the new particle.
            int nextFreeParticle = _firstFreeParticle + 1;

            if (nextFreeParticle >= _settings.MaxParticles)
                nextFreeParticle = 0;

            // If there are no free particles, we just have to give up.
            if (nextFreeParticle == _firstRetiredParticle)
                return;

            // Adjust the input velocity based on how much
            // this particle system wants to be affected by it.
            velocity *= _settings.EmitterVelocitySensitivity;

            // Add in some random amount of horizontal velocity.
            float horizontalVelocity = MathHelper.Lerp(_settings.MinHorizontalVelocity,
                                                       _settings.MaxHorizontalVelocity,
                                                       (float)Random.NextDouble());

            double horizontalAngle = Random.NextDouble() * MathHelper.TwoPi;

            velocity.X += horizontalVelocity * (float)Math.Cos(horizontalAngle);
            velocity.Z += horizontalVelocity * (float)Math.Sin(horizontalAngle);

            // Add in some random amount of vertical velocity.
            velocity.Y += MathHelper.Lerp(_settings.MinVerticalVelocity,
                                          _settings.MaxVerticalVelocity,
                                          (float)Random.NextDouble());

            // Choose four random control values. These will be used by the vertex
            // shader to give each particle a different size, rotation, and color.
            Color randomValues = new Color((byte)Random.Next(255),
                                           (byte)Random.Next(255),
                                           (byte)Random.Next(255),
                                           (byte)Random.Next(255));

            // Fill in the particle vertex structure.
            for (int i = 0; i < 4; i++)
            {
                _particles[_firstFreeParticle * 4 + i].Position = position;
                _particles[_firstFreeParticle * 4 + i].Velocity = velocity;
                _particles[_firstFreeParticle * 4 + i].Random = randomValues;
                _particles[_firstFreeParticle * 4 + i].Time = _currentTime;
            }

            _firstFreeParticle = nextFreeParticle;
        }

    }
}