using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Cannon3
{
    class GameItem
    {
        private Point position;
        private Point velocity;
        private Texture2D texture;

        #region properties

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public Point Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Point Origin
        {
            get
            {
                return new Point(texture.Width / 2, texture.Height / 2);

            }
        }

        #endregion

        public void update()
        {
            this.position = new Point(this.position.X + this.velocity.X,
                                      this.position.Y + this.velocity.Y);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Vector2 positionVector = new Vector2(this.position.X,this.position.Y);
            Vector2 originVector = new Vector2(this.Origin.X, this.Origin.Y);

            spriteBatch.Draw(texture,
                             positionVector,
                             null,
                             Color.White,
                             0.0f,
                             originVector,
                             1.0f,
                             SpriteEffects.None,
                             0.5f);
        }
    }
}
