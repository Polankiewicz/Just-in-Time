using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UnderMine.ParticleClasses;

namespace WindowsGame1
{

    class DustSmokeParticleSystem : ParticleSystem
    {

        private new Main.Game Game;

        public DustSmokeParticleSystem(Main.Game game)
            : base(game)
        {
            Game = game;
            Game.Components.Add(this);
            DrawOrder = 100;
        }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "UnderMineContent/white";

            settings.MaxParticles = 100000;

            settings.Duration = TimeSpan.FromSeconds(60);

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 5;
            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = 5;

            settings.Gravity = new Vector3(-1, 1, 0);

            settings.EndVelocity = 0.01f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 1;
            settings.MaxStartSize = 2;

            settings.MinEndSize = 0.1f;
            settings.MaxEndSize = 0.2f;

            settings.BlendState = BlendState.Additive;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            AddParticle(Vector3.Zero, Vector3.Zero);
            SetCamera(Game.ViewMatrix, Game.ProjectionMatrix);
        }
    }
}
