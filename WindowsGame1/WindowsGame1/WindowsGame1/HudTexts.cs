using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class HudTexts
    {
        String text = "";

        public String DisplayText
        {
            set { text = value; }
        }

        public void drawText(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, text, new Vector2(300, 200), Color.Black);
            spriteBatch.End();
        }


    }
}
