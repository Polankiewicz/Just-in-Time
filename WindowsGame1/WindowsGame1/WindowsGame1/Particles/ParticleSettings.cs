using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WindowsGame1
{
    /// <summary>
    /// Settings class describes all the tweakable options used
    /// to control the appearance of a particle system.
    /// </summary>
    public class ParticleSettings
    {
        // Name of the texture used by this particle system.
        public string TextureName = null;


        // Maximum number of particles that can be displayed at one time.
        public int MaxParticles = 10;


        // How long these particles will last.
        public TimeSpan Duration = TimeSpan.FromSeconds(1);

        // Range of values controlling how much X and Z axis velocity to give each
        // particle. Values for individual particles are randomly chosen from somewhere
        // between these limits.
        public float MinHorizontalVelocity = 0;
        public float MaxHorizontalVelocity = 1;


        // Range of values controlling how much Y axis velocity to give each particle.
        // Values for individual particles are randomly chosen from somewhere between
        // these limits.
        public float MinVerticalVelocity = 0;
        public float MaxVerticalVelocity = 1;


       
        // Controls how the particle velocity will change over their lifetime. If set
        // to 1, particles will keep going at the same speed as when they were created.
        // If set to 0, particles will come to a complete stop right before they die.
        // Values greater than 1 make the particles speed up over time.
        public float EndVelocity = 1;


        // Range of values controlling the particle color and alpha. Values for
        // individual particles are randomly chosen from somewhere between these limits.
        public Color MinColor = Color.White;
        public Color MaxColor = Color.White;



        // Range of values controlling how big the particles are when first created.
        // Values for individual particles are randomly chosen from somewhere between
        // these limits.
        public float MinStartSize = 10;
        public float MaxStartSize = 50;


        // Range of values controlling how big particles become at the end of their
        // life. Values for individual particles are randomly chosen from somewhere
        // between these limits.
        public float MinEndSize = 50;
        public float MaxEndSize = 100;


        // Alpha blending settings.
        public BlendState BlendState = BlendState.NonPremultiplied;
    }
}
