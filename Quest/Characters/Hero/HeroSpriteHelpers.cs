using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Characters.Hero
{
    internal class HeroSpriteHelpers
    {
        private static readonly int SpriteSheetRows = 1;

        private static readonly string StationaryRightPath = Path.Combine("Sprites", "Hero", "stationary-right").ToString();
        private static readonly string StationaryLeftPath = Path.Combine("Sprites", "Hero", "stationary-left").ToString();
        private static readonly int StationarySheetColumns = 3;
        private static TimeSpan StationarySpriteTime = TimeSpan.FromSeconds(1);

        private static readonly string RunningRightPath = Path.Combine("Sprites", "Hero", "running-right").ToString();
        private static readonly string RunningLeftPath = Path.Combine("Sprites", "Hero", "running-left").ToString();
        private static readonly int RunningSheetColumns = 4;
        private static TimeSpan RunningSpriteTime = TimeSpan.FromMilliseconds(150);

        private static readonly string JumpingRightPath = Path.Combine("Sprites", "Hero", "jumping-right").ToString();
        private static readonly string JumpingLeftPath = Path.Combine("Sprites", "Hero", "jumping-left").ToString();
        private static readonly int JumpingSheetColumns = 1;
        private static TimeSpan JumpingSpriteTime = TimeSpan.FromMilliseconds(150);

        private static readonly string AttackingRightPath = Path.Combine("Sprites", "Hero", "attacking-right").ToString();
        private static readonly string AttackingLeftPath = Path.Combine("Sprites", "Hero", "attacking-left").ToString();
        private static readonly int AttackingSheetColumns = 3;
        private static TimeSpan AttackingSpriteTime = TimeSpan.FromMilliseconds(200);


        public static SpriteSheet GetStationaryLeftSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(StationaryLeftPath);
            return new SpriteSheet(texture, StationarySpriteTime, SpriteSheetRows, StationarySheetColumns);
        }

        public static SpriteSheet GetStationaryRightSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(StationaryRightPath);
            return new SpriteSheet(texture, StationarySpriteTime, SpriteSheetRows, StationarySheetColumns);
        }

        public static SpriteSheet GetMovingRightSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(RunningRightPath);
            return new SpriteSheet(texture, RunningSpriteTime, SpriteSheetRows, RunningSheetColumns);
        }

        public static SpriteSheet GetMovingLeftSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(RunningLeftPath);
            return new SpriteSheet(texture, RunningSpriteTime, SpriteSheetRows, RunningSheetColumns);
        }

        public static SpriteSheet GetJumpingLeftSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(JumpingLeftPath);
            return new SpriteSheet(texture, JumpingSpriteTime, SpriteSheetRows, JumpingSheetColumns);
        }

        public static SpriteSheet GetJumpingRightSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(JumpingRightPath);
            return new SpriteSheet(texture, JumpingSpriteTime, SpriteSheetRows, JumpingSheetColumns);
        }

        public static SpriteSheet GetAttackingLeftSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(AttackingLeftPath);
            return new SpriteSheet(texture, AttackingSpriteTime, SpriteSheetRows, AttackingSheetColumns);
        }

        public static SpriteSheet GetAttackingRightSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(AttackingRightPath);
            return new SpriteSheet(texture, AttackingSpriteTime, SpriteSheetRows, AttackingSheetColumns);
        }
    }
}
