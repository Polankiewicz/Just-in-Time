using System;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{

    public class ParticleEmitter : Entity
    {
        #region Fields

        readonly ParticleSystem _particleSystem;
        readonly float _timeBetweenParticles;
        Vector3 _previousPosition;
        float _timeLeftOver;
        private Main.Game _game;

        #endregion


        /// <summary>
        /// Constructs a new particle emitter object.
        /// </summary>
        public ParticleEmitter(Main.Game game, ParticleSystem particleSystem,
                               float particlesPerSecond, Vector3 initialPosition) : base(game)
        {
            _game = game;
            _particleSystem = particleSystem;

            _timeBetweenParticles = 1.0f / particlesPerSecond;

            _previousPosition = initialPosition;
        }


        /// <summary>
        /// Updates the emitter, creating the appropriate number of particles
        /// in the appropriate positions.
        /// </summary>
        public void Update(GameTime gameTime, Vector3 newPosition)
        {
            if (gameTime == null)
                throw new ArgumentNullException("gameTime");

            // Work out how much time has passed since the previous update.
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime > 0)
            {
                // Work out how fast we are moving.
                Vector3 velocity = (newPosition - _previousPosition) / elapsedTime;

                // If we had any time left over that we didn't use during the
                // previous update, add that to the current elapsed time.
                float timeToSpend = _timeLeftOver + elapsedTime;

                // Counter for looping over the time interval.
                float currentTime = -_timeLeftOver;

                // Create particles as long as we have a big enough time interval.
                while (timeToSpend > _timeBetweenParticles)
                {
                    currentTime += _timeBetweenParticles;
                    timeToSpend -= _timeBetweenParticles;

                    // Work out the optimal position for this particle. This will produce
                    // evenly spaced particles regardless of the object speed, particle
                    // creation frequency, or game update rate.
                    float mu = currentTime / elapsedTime;

                    Vector3 position = Vector3.Lerp(_previousPosition, newPosition, mu);

                    // Create the particle.
                    _particleSystem.AddParticle(position, velocity);
                }

                // Store any time we didn't use, so it can be part of the next update.
                _timeLeftOver = timeToSpend;
            }

            _previousPosition = newPosition;
        }
    }
}
