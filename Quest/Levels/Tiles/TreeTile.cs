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
    internal class TreeTile : Tile
    {
        public static readonly string TexturePath = Path.Combine("Tiles", "tree").ToString();

        public override bool Passable => true;

        public TreeTile(Texture2D texture, int x, int y) : base(texture, x, y)
        {
        }

        public static Texture2D GetTexture(ContentManager content)
        {
            return content.Load<Texture2D>(TexturePath);
        }
    }
}
