using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Quest.Levels;
using Quest.Physics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Characters
{
    internal class AnimatedSprite
    {
        private const float JumpForce = -16;
        private const float MaxVelocityX = 6;
        private const float MaxVelocityY = 20;
        private const int MaxJumps = 1;
        private const int ScreenHeight = 720;

        public SpriteSheet CurrentSheet { get; set; }
        private SpriteSheet stationaryRightSheet;
        private SpriteSheet stationaryLeftSheet;
        private SpriteSheet runningRightSheet;
        private SpriteSheet runningLeftSheet;
        private SpriteSheet jumpingRightSheet;
        private SpriteSheet jumpingLeftSheet;

        private Keys previousKey;
        private bool jumping;

        private int x;
        private int y;
        private int width;
        private int height;

        public int X => x;
        public int Y => y;
        public Vector2 Velocity => new Vector2(velocityX, velocityY);
        public Rectangle SourceRectangle => new Rectangle(this.x, this.y, this.width, this.height);

        private float velocityX;
        private float velocityY;
        private float forceX;
        private float forceY;

        private int jumpsLeft;
        private int direction;

        public AnimatedSprite(
            SpriteSheet stationaryRightSheet, 
            SpriteSheet stationaryLeftSheet, 
            SpriteSheet runningRightSheet, 
            SpriteSheet runningLeftSheet, 
            SpriteSheet jumpingRightSheet, 
            SpriteSheet jumpingLeftSheet, 
            int x, int y,
            int width, int height)
        {
            this.CurrentSheet = stationaryRightSheet;
            this.stationaryRightSheet = stationaryRightSheet;
            this.stationaryLeftSheet = stationaryLeftSheet;
            this.runningRightSheet = runningRightSheet;
            this.runningLeftSheet = runningLeftSheet;
            this.jumpingRightSheet = jumpingRightSheet;
            this.jumpingLeftSheet = jumpingLeftSheet;

            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            this.velocityX = 0;
            this.velocityY = 0;
            this.forceX = 0;
            this.forceY = 0;

            this.direction = 1;
            this.jumpsLeft = 1;

            this.previousKey = Keys.None;
            this.jumping = false;
        }

        public void Update(GameTime time, Level level)
        {
            var currentKey = this.HandleInput();
            this.HandleForces(level);
            this.HandleSpriteSheet(currentKey, time);

            this.previousKey = currentKey;
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
                this.direction = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                forceX = this.jumping ? -MaxVelocityX / 4 : -MaxVelocityX / 2;
                currentKey = Keys.Left;
                this.direction = -1;
            }
            else
            {
                currentKey = Keys.None;
            }

            return currentKey;
        }

        private void HandleForces(Level level)
        {
            // Handle X direction

            this.velocityX = MathHelper.Clamp(forceX + this.velocityX, -MaxVelocityX, MaxVelocityX);
            this.x = (int) Math.Round(this.x + velocityX);

            // Handle horizontal collisions
            var intersection = level.Collides(this.SourceRectangle);
            if (intersection != Rectangle.Empty)
            {
                this.x = this.velocityX > 0 ? intersection.Left - this.width : intersection.Right;
            }

            // Add friction in X direction
            if (this.velocityX != 0)
            {
                var frictionForce = this.jumping ? PhysicsConstants.Friction / 4 : PhysicsConstants.Friction;
                this.velocityX = this.velocityX > 0
                    ? Math.Max(0, this.velocityX - frictionForce)
                    : Math.Min(0, this.velocityX + frictionForce);
            }

            // Handle Y direction
            this.velocityY += this.forceY;
            this.velocityY = MathHelper.Clamp(this.velocityY + PhysicsConstants.Gravity, -MaxVelocityY, MaxVelocityY);
            this.y = (int) Math.Round(this.y + velocityY);

            // Handle vertical collisions
            intersection = level.Collides(this.SourceRectangle);
            if (intersection != Rectangle.Empty)
            {
                this.y = this.velocityY > 0 ? intersection.Top - this.height : intersection.Bottom;
                this.jumping = false;
                this.velocityY = 0;
            }
        }

        private void HandleSpriteSheet(Keys currentKey, GameTime time)
        {
            if (this.jumping)
            {
                CurrentSheet = this.direction == 1 ? jumpingRightSheet : jumpingLeftSheet;
            }
            else if (Math.Abs(this.velocityX) > 0)
            {
                this.CurrentSheet = this.direction == 1 ? runningRightSheet : runningLeftSheet;
            }
            else
            {
                this.CurrentSheet = this.direction == 1 ? stationaryRightSheet : stationaryLeftSheet;
            }

            if (currentKey == previousKey)
            {
                this.CurrentSheet.Update(time);
            }
        }

        public void Draw(Camera camera)
        {
            camera.SpriteBatch.Begin();
            this.CurrentSheet.Draw(camera, this.x, this.y);
            camera.SpriteBatch.End();
        }
    }
}
