using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Levels
{
    internal class Level
    {
        private String path;
        private int level;

        private Tile[][] grid;
        private int rows;
        private int columns;
        private int tileWidth;
        private int tileHeight;

        public Rectangle SourceRectangle => new Rectangle(0, 0, columns * tileWidth, rows * tileHeight);
        public Dictionary<char, Texture2D> textureMap { get; set; }

        public Level(String path, Dictionary<char, Texture2D> textureMap, int level)
        {
            this.path = path;
            this.textureMap = textureMap;
            this.level = level;

            this.tileWidth = textureMap.First().Value.Width;
            this.tileHeight = textureMap.First().Value.Height;
            this.parseGrid();
        }

        public void Draw(Camera camera)
        {
            camera.SpriteBatch.Begin();
            foreach (var row in this.grid)
            {
                foreach (var tile in row)
                {
                    if (tile != null)
                    {
                        tile.Draw(camera);
                    }
                }
            }
            camera.SpriteBatch.End();
        }

        public Rectangle Collides(Rectangle rect)
        {
            var startX = Math.Max(0, (rect.Top / tileHeight) - 1);
            var endX = Math.Min(this.rows - 1, (rect.Bottom / tileHeight) + 1);
            var startY = Math.Max(0, (rect.Left / tileWidth) - 1);
            var endY = Math.Min(this.columns - 1, (rect.Right / tileWidth) + 1);

            Rectangle intersection = Rectangle.Empty;
            for (var i = startX; i <= endX; i++)
            {
                for (var j = startY; j <= endY; j++)
                {
                    var tile = grid[i][j];
                    if (tile == null || tile.Passable)
                    {
                        continue;
                    }

                    if (tile.SourceRectangle.Intersects(rect))
                    {
                        return tile.SourceRectangle;
                    }
                }
            }

            return intersection;
        }

        private void parseGrid()
        {
            var lines = File.ReadAllLines(this.path);
            var data = lines[0].Split(' ');
            this.rows = int.Parse(data[0]);
            this.columns = int.Parse(data[1]);

            this.grid = new Tile[rows][];


            for (var i = 0; i < rows; i++)
            {
                var line = lines[i + 1];
                this.grid[i] = new Tile[columns];
                for (var j = 0; j < columns; j++)
                {
                    if (j >= line.Length)
                    {
                        break;
                    }

                    var c = line[j];
                    if (this.textureMap.ContainsKey(c))
                    {
                        if (c == 'T')
                        {
                            var passable2 = true;
                        }
                        var texture = this.textureMap[c];
                        var passable = c == 'T';
                        this.grid[i][j] = new Tile(texture, passable, j * tileWidth, i * tileHeight);
                    }
                }
            }
        }


    }
}
