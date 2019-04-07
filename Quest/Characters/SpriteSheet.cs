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
        private int frameTime;
        private FrameCounter frameCounter;

        private bool loop;

        public bool Done { get; private set; }
        public int CurrentSprite { get; private set; }

        public SpriteSheet(Texture2D texture, int frameTime, int rows, int columns, bool loop)
        {
            this.texture = texture;
            this.rows = rows;
            this.columns = columns;
            this.totalSpritesPerSheet = rows * columns;
            this.frameTime = frameTime;
            this.loop = loop;
            this.frameCounter = new FrameCounter();

            this.Done = false;
            this.CurrentSprite = 0;
        }

        public void Reset()
        {
            this.CurrentSprite = 0;
            this.frameCounter.Reset();
            this.Done = false;
        }

        public void Update()
        {
            this.frameCounter.Update();

            if (this.frameCounter.Frame % this.frameTime == 0)
            {
                this.CurrentSprite++;
                if (this.CurrentSprite >= this.totalSpritesPerSheet)
                {
                    if (this.loop)
                    {
                        this.Reset();
                    }
                    else
                    {
                        this.CurrentSprite = this.totalSpritesPerSheet - 1;
                        this.Done = true;
                    }
                }
            }
        }

        public void Draw(Camera camera, int x, int y, Color color)
        {
            var width = this.texture.Width / this.columns;
            var height = this.texture.Height / this.rows;

            var row = this.CurrentSprite / this.columns;
            var column = this.CurrentSprite % this.columns;

            var sourceRectangle = new Rectangle(width * column, height * row, width, height);
            var destinationRectangle = new Rectangle(x, y, width, height);

            camera.Draw(this.texture, destinationRectangle, sourceRectangle, color);
        }
    }
}
