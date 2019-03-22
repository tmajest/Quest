using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Quest.Levels;
using Quest.Physics;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Characters
{
    internal class Hero : MovingSprite
    {
        private static readonly string StationaryRightPath = Path.Combine("Sprites", "stationary-right").ToString();
        private static readonly string StationaryLeftPath = Path.Combine("Sprites", "stationary-left").ToString();
        private static readonly int StationarySheetColumns = 3;
        private static TimeSpan StationarySpriteTime = TimeSpan.FromSeconds(1);

        private static readonly string RunningRightPath = Path.Combine("Sprites", "running-right").ToString();
        private static readonly string RunningLeftPath = Path.Combine("Sprites", "running-left").ToString();
        private static readonly int RunningSheetColumns = 4;
        private static TimeSpan RunningSpriteTime = TimeSpan.FromMilliseconds(150);

        private static readonly string JumpingRightPath = Path.Combine("Sprites", "jumping-right").ToString();
        private static readonly string JumpingLeftPath = Path.Combine("Sprites", "jumping-left").ToString();
        private static readonly int JumpingSheetColumns = 1;
        private static TimeSpan JumpingSpriteTime = TimeSpan.FromMilliseconds(150);

        private static readonly int SpriteSheetRows = 1;

        private static readonly int MaxVelocityX = 6;
        private static readonly int MaxVelocityY = 20;

        private static readonly int HeroWidth = 46;
        private static readonly int HeroHeight = 64;

        private static readonly Vector2 maxVelocity = new Vector2(MaxVelocityX, MaxVelocityY);
        private static readonly Vector2 velocity = new Vector2(0, 0);
        private static readonly Vector2 force = new Vector2(0, 0);

        private static readonly int MaxJumps = 1;
        private static readonly float JumpForce = -16;

        private SpriteSheet currentSpriteSheet;
        private SpriteSheet stationaryLeftSpriteSheet;
        private SpriteSheet stationaryRightSpriteSheet;
        private SpriteSheet movingLeftSpriteSheet;
        private SpriteSheet movingRightSpriteSheet;
        private SpriteSheet jumpingLeftSpriteSheet;
        private SpriteSheet jumpingRightSpriteSheet;

        private int jumpsLeft;
        private Keys previousKey;
        private bool jumping;

        public Hero(
            SpriteSheet stationaryLeftSpriteSheet,
            SpriteSheet stationaryRightSpriteSheet,
            SpriteSheet movingLeftSpriteSheet,
            SpriteSheet movingRightSpriteSheet,
            SpriteSheet jumpingLeftSpriteSheet,
            SpriteSheet jumpingRightSpriteSheet,
            Vector2 position,
            PhysicsEngine physicsEngine,
            int width, int height,
            Direction direction)
            : base(position, velocity, maxVelocity, force, physicsEngine, width, height, direction)
        {
            this.stationaryLeftSpriteSheet = stationaryLeftSpriteSheet;
            this.stationaryRightSpriteSheet = stationaryRightSpriteSheet;
            this.movingLeftSpriteSheet = movingLeftSpriteSheet;
            this.movingRightSpriteSheet = movingRightSpriteSheet;
            this.jumpingLeftSpriteSheet = jumpingLeftSpriteSheet;
            this.jumpingRightSpriteSheet = jumpingRightSpriteSheet;

            this.jumpsLeft = MaxJumps;
            this.previousKey = Keys.None;
            this.jumping = false;
        }

        public static Hero Build(
            ContentManager content, 
            Vector2 position, 
            Direction direction, 
            PhysicsEngine physicsEngine)
        {
            return new Hero(
                GetStationaryLeftSpriteSheet(content),
                GetStationaryRightSpriteSheet(content),
                GetMovingLeftSpriteSheet(content),
                GetMovingRightSpriteSheet(content),
                GetJumpingLeftSpriteSheet(content),
                GetJumpingRightSpriteSheet(content),
                position,
                physicsEngine,
                HeroWidth,
                HeroHeight,
                direction);
        }

        public override void Update(GameTime time, Level level)
        {
            base.Update(time, level);

            var currentKey = this.HandleInput();
            this.HandleSpriteSheet(currentKey, time);

            this.previousKey = currentKey;
        }

        public void Draw(Camera camera)
        {
            camera.Begin();
            this.currentSpriteSheet.Draw(camera, (int) this.x, (int) this.y);
            camera.End();
        }

        private Keys HandleInput()
        {
            Keys currentKey;
            this.forceY = 0;
            this.forceX = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && this.jumpsLeft > 0)
            {
                this.jumpsLeft--;
                this.forceY = JumpForce;
                this.jumping = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space) && !this.jumping && this.jumpsLeft == 0)
            {
                this.jumpsLeft = MaxJumps;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                forceX = this.jumping ? MaxVelocityX / 4 : MaxVelocityX / 2;
                currentKey = Keys.Right;
                this.direction = Direction.Right;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                forceX = this.jumping ? -MaxVelocityX / 4 : -MaxVelocityX / 2;
                currentKey = Keys.Left;
                this.direction = Direction.Left;
            }
            else
            {
                currentKey = Keys.None;
            }

            return currentKey;
        }

        private void HandleSpriteSheet(Keys currentKey, GameTime time)
        {
            if (this.jumping)
            {
                this.currentSpriteSheet = this.direction == Direction.Right ? jumpingRightSpriteSheet : jumpingLeftSpriteSheet;
                this.currentSpriteSheet.Update(time);
            }
            else if (Math.Abs(this.velocityX) > 0)
            {
                // The character is moving, so select the moving sprite sheet according to which direction we're facing
                this.currentSpriteSheet = this.direction == Direction.Right ? movingRightSpriteSheet : movingLeftSpriteSheet;
            }
            else
            {
                // The character is stationary, default to the stationary sprite sheet
                this.currentSpriteSheet = this.direction == Direction.Right ? stationaryRightSpriteSheet : stationaryLeftSpriteSheet;
            }

            if (currentKey == previousKey)
            {
                this.currentSpriteSheet.Update(time);
            }
        }

        public override void VerticalCollisionHandler()
        {
            this.jumping = false;
        }

        private static SpriteSheet GetStationaryLeftSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(StationaryLeftPath);
            return new SpriteSheet(texture, StationarySpriteTime, SpriteSheetRows, StationarySheetColumns);
        }

        private static SpriteSheet GetStationaryRightSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(StationaryRightPath);
            return new SpriteSheet(texture, StationarySpriteTime, SpriteSheetRows, StationarySheetColumns);
        }

        private static SpriteSheet GetMovingRightSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(RunningRightPath);
            return new SpriteSheet(texture, RunningSpriteTime, SpriteSheetRows, RunningSheetColumns);
        }

        private static SpriteSheet GetMovingLeftSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(RunningLeftPath);
            return new SpriteSheet(texture, RunningSpriteTime, SpriteSheetRows, RunningSheetColumns);
        }

        private static SpriteSheet GetJumpingLeftSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(JumpingLeftPath);
            return new SpriteSheet(texture, JumpingSpriteTime, SpriteSheetRows, JumpingSheetColumns);
        }

        private static SpriteSheet GetJumpingRightSpriteSheet(ContentManager content)
        {
            var texture = content.Load<Texture2D>(JumpingRightPath);
            return new SpriteSheet(texture, JumpingSpriteTime, SpriteSheetRows, JumpingSheetColumns);
        }
    }
}
