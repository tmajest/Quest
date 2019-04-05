using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Quest.Levels;
using Quest.Physics;
using Quest.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Characters
{
    internal class Bug : MovingSprite
    {
        private static readonly string WalkingRightPath = Path.Combine("Sprites", "Bug", "bug-right").ToString();
        private static readonly string WalkingLeftPath = Path.Combine("Sprites", "Bug", "bug-left").ToString();

        private static readonly int MaxVelocityX = 2;

        private static readonly Vector2 maxVelocity = new Vector2(MaxVelocityX, 20);
        private static readonly Vector2 velocity = new Vector2(0, 0);
        private static readonly Vector2 force = new Vector2(0, 0);

        private static readonly int SpriteSheetRows = 1;
        private static readonly int SpriteSheetColumns = 2;

        private static readonly int BugWidth = 64;
        private static readonly int BugHeight = 64;

        private FrameCounter frameCounter;

        private static int WalkTime = 24;

        private SpriteSheet currentSpriteSheet;
        private SpriteSheet stationaryLeftSpriteSheet;
        private SpriteSheet stationaryRightSpriteSheet;
        private SpriteSheet movingLeftSpriteSheet;
        private SpriteSheet movingRightSpriteSheet;

        public override float Friction => 0.02f;

        public Bug(
            SpriteSheet stationaryLeftSheet,
            SpriteSheet stationaryRightSheet,
            SpriteSheet movingLeftSheet,
            SpriteSheet movingRightSheet,
            PhysicsEngine physicsEngine,
            Vector2 position,
            int width, int height,
            Direction direction) 
            : base(position, velocity, maxVelocity, force, physicsEngine, width, height, direction)
        {
            this.currentSpriteSheet = stationaryRightSheet;
            this.stationaryLeftSpriteSheet = stationaryLeftSheet;
            this.stationaryRightSpriteSheet = stationaryRightSheet;
            this.movingLeftSpriteSheet = movingLeftSheet;
            this.movingRightSpriteSheet = movingRightSheet;

            this.frameCounter = new FrameCounter();
        }

        public static Bug Build(
            ContentManager content, 
            Vector2 position, 
            Direction direction, 
            PhysicsEngine physicsEngine)
        {
            return new Bug(
                GetStationaryLeftSpriteSheet(content),
                GetStationaryRightSpriteSheet(content),
                GetMovingLeftSpriteSheet(content),
                GetMovingRightSpriteSheet(content),
                physicsEngine,
                position,
                BugWidth,
                BugHeight,
                direction);
        }

        public override void Update(Level level)
        {
            this.frameCounter.Update();
            if (this.frameCounter.Frame % WalkTime == 0)
            {
                // Apply force to move the bug
                this.velocityX += this.direction == Direction.Right ? MaxVelocityX : -MaxVelocityX;
            }

            this.UpdateCurrentSpriteSheet();
            base.Update(level);
        }

        internal void UpdateCurrentSpriteSheet()
        {
            if (Math.Abs(this.velocityX) > 0)
            {
                // The character is moving, so select the moving sprite sheet according to which direction we're facing
                this.currentSpriteSheet = this.direction == Direction.Right ? movingRightSpriteSheet : movingLeftSpriteSheet;
            }
            else
            {
                // The character is stationary, default to the stationary sprite sheet
                this.currentSpriteSheet = this.direction == Direction.Right ? stationaryRightSpriteSheet : stationaryLeftSpriteSheet;
            }

            this.currentSpriteSheet.Update();
        }

        public void Draw(Camera camera)
        {
            camera.Begin();
            this.currentSpriteSheet.Draw(camera, (int) this.x, (int) this.y);
            camera.End();
        }

        public override void HorizontalCollisionHandler()
        {
            this.direction = this.direction == Direction.Right ? Direction.Left : Direction.Right;
            this.velocityX *= -1;
            this.forceX *= -1;
        }

        internal static SpriteSheet GetMovingLeftSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(WalkingLeftPath);
            return new SpriteSheet(texture, WalkTime, SpriteSheetRows, SpriteSheetColumns, loop: true);
        }

        internal static SpriteSheet GetStationaryLeftSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(WalkingLeftPath);
            return new SpriteSheet(texture, WalkTime, SpriteSheetRows, SpriteSheetColumns, loop: true);
        }

        internal static SpriteSheet GetMovingRightSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(WalkingRightPath);
            return new SpriteSheet(texture, WalkTime, SpriteSheetRows, SpriteSheetColumns, loop: true);
        }

        internal static SpriteSheet GetStationaryRightSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(WalkingRightPath);
            return new SpriteSheet(texture, WalkTime, SpriteSheetRows, SpriteSheetColumns, loop: true);
        }
    }
}
