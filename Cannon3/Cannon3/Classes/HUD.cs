using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cannon3
{
    class HUD
    {
        private Point position;
        private Texture2D texture;
        private int width;
        private int height;

        #region properties

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        #endregion

        public void draw(SpriteBatch spriteBatch)
        {
            Vector2 positionVector = new Vector2(this.position.X, this.position.Y);
            Vector2 originVector = new Vector2(0, 0);
            Rectangle sourceRectangle = new Rectangle(0,0,this.Width,this.Height);

            spriteBatch.Draw(texture,
                             positionVector,
                             sourceRectangle,
                             Color.White,
                             0.0f,
                             originVector,
                             1.0f,
                             SpriteEffects.None,
                             0.5f);
        }
    }
}
