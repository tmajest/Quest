using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Levels.Tiles
{
    internal class GrassTile : Tile
    {
        public static readonly string TopGrassPath = Path.Combine("Tiles", "smallgrass3").ToString();
        public static readonly string BottomGrassPath = Path.Combine("Tiles", "smallgrass2").ToString();

        public override bool Passable => false;

        public GrassTile(Texture2D texture, int x, int y) : base(texture, x, y)
        {
        }

        public static Texture2D GetTopTexture(ContentManager content)
        {
            return content.Load<Texture2D>(TopGrassPath);
        }

        public static Texture2D GetBottomTexture(ContentManager content)
        {
            return content.Load<Texture2D>(BottomGrassPath);
        }
    }
}
