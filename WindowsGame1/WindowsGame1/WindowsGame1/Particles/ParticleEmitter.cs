
#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace WindowsGame1
{
    public class ParticleEmitter
    {
        #region Fields

        ParticleSystem particleSystem;
        float timeBetweenParticles;
        Vector3 previousPosition;
        float timeLeftOver;

        #endregion


        /// <summary>
        /// Constructs a new particle emitter object.
        /// </summary>
        public ParticleEmitter(ParticleSystem particleSystem, float particlesPerSecond, Vector3 initialPosition)
        {
            this.particleSystem = particleSystem;

            timeBetweenParticles = 1.0f / particlesPerSecond;
            
            previousPosition = initialPosition;
        }


        /// <summary>
        /// Updates the emitter, creating the appropriate number of particles
        /// in the appropriate positions.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (gameTime == null)
                throw new ArgumentNullException("gameTime");
        }

        }
    
}
