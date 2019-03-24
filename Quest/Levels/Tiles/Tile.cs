using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Levels.Tiles
{
    internal abstract class Tile
    {
        public abstract bool Passable { get; }

        public Texture2D Texture { get; private set; }
        public Rectangle SourceRectangle { get; private set; }
        public Color Color { get; private set; }

        public int X => this.SourceRectangle.X;
        public int Y => this.SourceRectangle.Y;
        public int Width => this.Texture.Width;
        public int Height => this.Texture.Height;

        public Tile(Texture2D texture, int x, int y)
        {
            this.Texture = texture;
            this.SourceRectangle = new Rectangle(x, y, texture.Width, texture.Height);
            this.Color = Color.White;
        }

        public void Draw(Camera camera)
        {
            camera.Draw(this.Texture, this.SourceRectangle, this.Color);
        }
    }
}
