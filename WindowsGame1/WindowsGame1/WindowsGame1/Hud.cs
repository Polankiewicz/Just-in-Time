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

        public void drawHud(SpriteBatch spriteBatch, Texture2D t2, Game g)
        {
            spriteBatch.Begin();
            //spriteBatch.Draw(t2, , Color.White);
            spriteBatch.Draw(t2, g.GraphicsDevice.Viewport.Bounds , Color.White);
            spriteBatch.End();
        }
        public void drawPointer(SpriteBatch spriteBatch, Texture2D t2, Game g)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(t2, new Vector2((g.GraphicsDevice.Viewport.Width / 2 )-40, (g.GraphicsDevice.Viewport.Height / 2)-40), Color.White);
            spriteBatch.End();
        }

        public void drawHealth(SpriteBatch spriteBatch, Texture2D t2, Game g, Camera c)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(t2, c.healthRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
