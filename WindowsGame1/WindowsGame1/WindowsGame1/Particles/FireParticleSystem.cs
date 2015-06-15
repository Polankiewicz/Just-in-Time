
#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace WindowsGame1
{
    /// <summary>
    /// Custom particle system for creating a flame effect.
    /// </summary>
    class FireParticleSystem : ParticleSystem
    {
        public FireParticleSystem(Game game, ContentManager content): base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Sprites\\TimeParticle";

            settings.MaxParticles = 10;

            settings.Duration = TimeSpan.FromSeconds(2);

            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 1;

            settings.MinVerticalVelocity = -1;
            settings.MaxVerticalVelocity = 1;

            // Set gravity upside down, so the flames will 'fall' upward.
            //settings.Gravity = new Vector3(0, 15, 0);

            settings.MinColor = new Color(255, 255, 255, 10);
            settings.MaxColor = new Color(255, 255, 255, 40);

            settings.MinStartSize = 1;
            settings.MaxStartSize = 2;

            settings.MinEndSize = 3;
            settings.MaxEndSize = 4;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
