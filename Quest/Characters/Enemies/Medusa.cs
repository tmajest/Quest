using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Quest.Levels;
using Quest.Physics;
using Quest.Utils;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Characters.Enemies
{
    internal class Medusa : Character
    {
        private static readonly int TotalInvincibilityFrames = 15;
        private static readonly int MaxHealth = 5;

        private static readonly string WalkingRightPath = Path.Combine("Sprites", "Medusa", "medusa-right").ToString();
        private static readonly string WalkingLeftPath = Path.Combine("Sprites", "Medusa", "medusa-left").ToString();

        private static readonly int MaxVelocityX = 1;

        private static readonly Vector2 maxVelocity = new Vector2(MaxVelocityX, 5);
        private static readonly Vector2 velocity = new Vector2(1, 0);
        private static readonly Vector2 force = new Vector2(1, 0);

        private static readonly int SpriteSheetRows = 1;
        private static readonly int SpriteSheetColumns = 6;

        private static int WalkTime = 6;
        private static readonly int MedusaWidth = 128;
        private static readonly int MedusaHeight = 128;

        private SpriteSheet currentSpriteSheet;
        private SpriteSheet stationaryLeftSpriteSheet;
        private SpriteSheet stationaryRightSpriteSheet;
        private SpriteSheet movingLeftSpriteSheet;
        private SpriteSheet movingRightSpriteSheet;

        public override float Friction => 0.02f;

        public Medusa(
            SpriteSheet stationaryLeftSheet,
            SpriteSheet stationaryRightSheet,
            SpriteSheet movingLeftSheet,
            SpriteSheet movingRightSheet,
            PhysicsEngine physicsEngine,
            Vector2 position,
            int width, int height,
            Direction direction) 
            : base(position, velocity, maxVelocity, force, physicsEngine, width, height, MaxHealth, 
                  TotalInvincibilityFrames, direction)
        {
            this.currentSpriteSheet = stationaryRightSheet;
            this.stationaryLeftSpriteSheet = stationaryLeftSheet;
            this.stationaryRightSpriteSheet = stationaryRightSheet;
            this.movingLeftSpriteSheet = movingLeftSheet;
            this.movingRightSpriteSheet = movingRightSheet;
        }

        public static Medusa Build(
            ContentManager content, 
            Vector2 position, 
            Direction direction, 
            PhysicsEngine physicsEngine)
        {
            return new Medusa(
                GetStationaryLeftSpriteSheet(content),
                GetStationaryRightSpriteSheet(content),
                GetMovingLeftSpriteSheet(content),
                GetMovingRightSpriteSheet(content),
                physicsEngine,
                position,
                MedusaWidth,
                MedusaHeight,
                direction);
        }

        public override void Update(Level level)
        {
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

        public override void Draw(Camera camera)
        {
            camera.Begin();
            this.currentSpriteSheet.Draw(camera, (int) this.x, (int) this.y, this.Color);
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
