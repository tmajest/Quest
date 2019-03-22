using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Quest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Characters
{
    internal class SpriteSheet
    {
        private Texture2D texture;

        private int rows;
        private int columns;

        private int totalSpritesPerSheet;
        private int currentSprite;
        private TimeSpan timePerFrame;

        public SpriteSheet(Texture2D texture, TimeSpan timePerFrame, int rows, int columns)
        {
            this.texture = texture;
            this.rows = rows;
            this.columns = columns;
            this.totalSpritesPerSheet = rows * columns;
            this.currentSprite = 0;
            this.timePerFrame = timePerFrame;
        }

        public void Update(GameTime time)
        {
            if (time.TotalMilliseconds() % (long) timePerFrame.TotalMilliseconds == 0)
            {
                this.currentSprite = (this.currentSprite + 1) % totalSpritesPerSheet;
            }
        }

        public void Draw(Camera camera, int x, int y)
        {
            var width = this.texture.Width / this.columns;
            var height = this.texture.Height / this.rows;

            var row = this.currentSprite / this.columns;
            var column = this.currentSprite % this.columns;

            var sourceRectangle = new Rectangle(width * column, height * row, width, height);
            var destinationRectangle = new Rectangle(x, y, width, height);

            camera.Draw(this.texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
