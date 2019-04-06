using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using Quest.Characters;
using Quest.Levels.Tiles;
using Quest.Physics;

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
        private static readonly int TileWidth = 64;
        private static readonly int TileHeight = 64;

        private ContentManager content;
        private String path;
        private int level;

        private Tile[][] grid;
        private int rows;
        private int columns;

        public Rectangle SourceRectangle => new Rectangle(0, 0, columns * TileWidth, rows * TileHeight);
        public PhysicsEngine PhysicsEngine { get; private set; }
        public List<MovingSprite> Enemies { get; private set; }

        private TileFactory tileFactory;
        private EnemyFactory enemyFactory;

        public Level(ContentManager content, String path, TileFactory tileFactory, EnemyFactory enemyFactory, int level)
        {
            this.content = content;
            this.path = path;
            this.tileFactory = tileFactory;
            this.enemyFactory = enemyFactory;
            this.level = level;
            this.PhysicsEngine = new PhysicsEngine(this);
            this.Enemies = new List<MovingSprite>();

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
            var startX = Math.Max(0, (rect.Top / TileHeight) - 1);
            var endX = Math.Min(this.rows - 1, (rect.Bottom / TileHeight) + 1);
            var startY = Math.Max(0, (rect.Left / TileWidth) - 1);
            var endY = Math.Min(this.columns - 1, (rect.Right / TileWidth) + 1);

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
                        tile.Color = Color.Red;
                        intersection = tile.SourceRectangle;
                    }
                    else
                    {
                        tile.Color = Color.White;
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
                    if (c != ' ')
                    {
                        var tile = this.tileFactory.Create(c, j * TileWidth, i * TileHeight);
                        if (tile != null)
                        {
                            this.grid[i][j] = tile;
                        }
                        else
                        {
                            var enemy = this.enemyFactory.Create(this.PhysicsEngine, this.content, c, j * TileWidth, i * TileHeight);
                            if (enemy != null)
                            {
                                this.Enemies.Add(enemy);
                            }
                        }
                    }
                }
            }
        }
    }
}
