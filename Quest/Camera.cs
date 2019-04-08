using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Quest.Levels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest
{
    internal class Camera
    {
        public float X { get; set; }
        public float Y { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public Color Filter = Color.White;

        public SpriteBatch SpriteBatch { get; set; }

        public Camera(SpriteBatch spriteBatch, float x,float y, int screenWidth, int screenHeight)
        {
            this.SpriteBatch = spriteBatch;
            this.X = 0;
            this.Y = 0;
            this.ScreenWidth = screenWidth;
            this.ScreenHeight = screenHeight;
        }

        public void Update(Level level, Vector2 playerSpeed, Rectangle playerRectangle)
        {
            var playerRelativePositionX = playerRectangle.X - this.X * 1.0;
            if (playerRelativePositionX < ScreenWidth * 0.20)
            {
                this.X = Math.Max(0, playerRectangle.X - (ScreenWidth * 0.2f));
            }
            else if (playerRelativePositionX + playerRectangle.Width > ScreenWidth * 0.80)
            {
                this.X = Math.Min(level.Rectangle.Right - ScreenWidth, playerRectangle.Right - (ScreenWidth * 0.8f));
            }

            var playerRelativePositionY = playerRectangle.Y - this.Y * 1.0;
            if (playerRelativePositionY < ScreenHeight * 0.2)
            {
                this.Y = Math.Max(0, playerRectangle.Y - (ScreenHeight * 0.2f));
            }
            else if (playerRelativePositionY + playerRectangle.Height > ScreenHeight * 0.70)
            {
                this.Y = Math.Min(level.Rectangle.Bottom - ScreenHeight, playerRectangle.Bottom - (ScreenHeight * 0.7f));
            }
        }

        public void Begin()
        {
            this.SpriteBatch.Begin();
        }

        public void End()
        {
            this.SpriteBatch.End();
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            if (destinationRectangle.Right < this.X || destinationRectangle.Left > this.X + ScreenWidth)
            {
                // The object isn't within the bounds of the camera, so don't draw it
                return;
            }

            var cameraRectangle = new Rectangle(
                (int) (destinationRectangle.X - this.X),
                (int) (destinationRectangle.Y - this.Y),
                texture.Width,
                texture.Height);

            this.SpriteBatch.Draw(texture, cameraRectangle, this.Filter);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color)
        {
            var cameraRectangle = new Rectangle(
                (int) (destinationRectangle.X - this.X),
                (int) (destinationRectangle.Y - this.Y),
                destinationRectangle.Width,
                destinationRectangle.Height);

            this.SpriteBatch.Draw(texture, cameraRectangle, sourceRectangle, color);
        }
    }
}
