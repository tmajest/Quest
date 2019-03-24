using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Levels.Tiles
{
    internal class TileFactory
    {
        private Dictionary<char, Texture2D> textureMap;

        public TileFactory(Dictionary<char, Texture2D> textureMap)
        {
            this.textureMap = textureMap;
        }

        public Tile Create(char c, int x, int y)
        {
            if (c == 'T')
            {
                return new TreeTile(textureMap['T'], x, y);
            }
            else if (c == 'G')
            {
                return new GrassTile(textureMap['G'], x, y);
            }
            else if (c == 'D')
            {
                return new DirtTile(textureMap['D'], x, y);
            }
            else
            {
                throw new Exception("Tile " + c + " not recognized.");
            }
        }
    }
}
