using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using Quest.Characters;
using Quest.Characters.Enemies;
using Quest.Physics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Levels
{
    internal class EnemyFactory
    {
        public MovingSprite Create(PhysicsEngine engine, ContentManager content, char c, int x, int y)
        {
            if (c == 'B')
            {
                return Bug.Build(content, new Vector2(x, y), Direction.Left, engine);
            }
            else if (c == 'M')
            {
                return Medusa.Build(content, new Vector2(x, y), Direction.Left, engine);
            }

            return null;
        }
    }
}
