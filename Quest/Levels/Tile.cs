using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Levels
{
    internal class Tile
    {
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Color Color { get; set; }
        public bool Passable { get; set; }

        public int X => this.SourceRectangle.X;
        public int Y => this.SourceRectangle.Y;
        public int Width => this.Texture.Width;
        public int Height => this.Texture.Height;

        public Tile(Texture2D texture, bool passable, int x, int y)
        {
            this.Texture = texture;
            this.Passable = passable;
            this.SourceRectangle = new Rectangle(x, y, Texture.Width, Texture.Height);
            this.Color = Color.White;
        }

        public void Draw(Camera camera)
        {
            camera.Draw(this.Texture, this.SourceRectangle, this.Color);
        }
    }
}
