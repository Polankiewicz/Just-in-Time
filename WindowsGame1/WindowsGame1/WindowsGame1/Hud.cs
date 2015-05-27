using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class Hud
    {
        public void drawHud(SpriteBatch spriteBatch, Texture2D t2, Camera c)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(t2, new Rectangle((int)c.Position.X, (int)c.Position.Y, 1366, 768), Color.White);
            spriteBatch.End();
        }
    }
}
