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

namespace Quest.Characters.Hero
{
    internal class Hero : Character
    {
        private static readonly int TotalInvincibilityFrames = 40;
        private static readonly int MaxHealth = 5;
        private static readonly int MaxVelocityX = 5;
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
        private SpriteSheet attackingLeftSpriteSheet;
        private SpriteSheet attackingRightSpriteSheet;
        private SpriteSheet healthSprite;

        private HeroState previousState;
        private int jumpsLeft;
        private bool jumping;
        private bool attacking;
        private bool finishAttacking;

        public Hero(
            SpriteSheet stationaryLeftSpriteSheet,
            SpriteSheet stationaryRightSpriteSheet,
            SpriteSheet movingLeftSpriteSheet,
            SpriteSheet movingRightSpriteSheet,
            SpriteSheet jumpingLeftSpriteSheet,
            SpriteSheet jumpingRightSpriteSheet,
            SpriteSheet attackingLeftSpriteSheet,
            SpriteSheet attackingRightSpriteSheet,
            SpriteSheet healthSprite,
            Vector2 position,
            PhysicsEngine physicsEngine,
            int width, int height,
            Direction direction)
            : base(position, velocity, maxVelocity, force, physicsEngine, width, height, MaxHealth, 
                  TotalInvincibilityFrames, direction)
        {
            this.stationaryLeftSpriteSheet = stationaryLeftSpriteSheet;
            this.stationaryRightSpriteSheet = stationaryRightSpriteSheet;
            this.movingLeftSpriteSheet = movingLeftSpriteSheet;
            this.movingRightSpriteSheet = movingRightSpriteSheet;
            this.jumpingLeftSpriteSheet = jumpingLeftSpriteSheet;
            this.jumpingRightSpriteSheet = jumpingRightSpriteSheet;
            this.attackingLeftSpriteSheet = attackingLeftSpriteSheet;
            this.attackingRightSpriteSheet = attackingRightSpriteSheet;
            this.healthSprite = healthSprite;

            this.jumpsLeft = MaxJumps;
            this.jumping = false;
            this.attacking = false;
            this.finishAttacking = false;
            this.previousState = HeroState.Stationary;
        }

        public static Hero Build(
            ContentManager content, 
            Vector2 position, 
            Direction direction, 
            PhysicsEngine physicsEngine)
        {
            return new Hero(
                HeroSpriteHelpers.GetStationaryLeftSpriteSheet(content),
                HeroSpriteHelpers.GetStationaryRightSpriteSheet(content),
                HeroSpriteHelpers.GetMovingLeftSpriteSheet(content),
                HeroSpriteHelpers.GetMovingRightSpriteSheet(content),
                HeroSpriteHelpers.GetJumpingLeftSpriteSheet(content),
                HeroSpriteHelpers.GetJumpingRightSpriteSheet(content),
                HeroSpriteHelpers.GetAttackingLeftSpriteSheet(content),
                HeroSpriteHelpers.GetAttackingRightSpriteSheet(content),
                HeroSpriteHelpers.GetHealthSprite(content),
                position,
                physicsEngine,
                HeroWidth,
                HeroHeight,
                direction);
        }

        public override void Update(Level level)
        {
            base.Update(level);

            this.HandleInput();
            this.HandleSpriteSheet();
            this.HandleAttacks(level.Enemies);
            this.HandleEnemies(level.Enemies);
        }

        public override void Draw(Camera camera)
        {
            camera.Begin();
            this.currentSpriteSheet.Draw(camera, (int) this.x, (int) this.y, this.Color);
            camera.End();

            this.DrawHealth(camera);
        }

        private void DrawHealth(Camera camera)
        {
            var startX = 50;
            var startY = 30;
            var space = 2;

            camera.SpriteBatch.Begin();
            for (var i = 0; i < this.Health; i++)
            {
                var rect = new Rectangle(
                    startX + ((this.healthSprite.Width + space) * i), 
                    startY, 
                    this.healthSprite.Width, 
                    this.healthSprite.Height);
                camera.SpriteBatch.Draw(this.healthSprite.Texture, rect, Color.White);
            }
            camera.SpriteBatch.End();
        }

        private void HandleInput()
        {
            this.forceY = 0;
            this.forceX = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.F) && !this.attacking && !this.finishAttacking)
            {
                this.attacking = true;
                return;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && this.jumpsLeft > 0)
            {
                this.jumpsLeft--;
                this.forceY = JumpForce;
                this.jumping = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space) && !this.jumping && !this.attacking && this.jumpsLeft == 0)
            {
                this.jumpsLeft = MaxJumps;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !this.attacking)
            {
                forceX = this.jumping ? MaxVelocityX / 4 : MaxVelocityX / 2;
                this.direction = Direction.Right;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !this.attacking)
            {
                forceX = this.jumping ? -MaxVelocityX / 4 : -MaxVelocityX / 2;
                this.direction = Direction.Left;
            }
        }

        private void HandleSpriteSheet()
        {
            HeroState state = HeroState.Stationary;

            if (this.attacking)
            {
                state = HeroState.Attacking;
                if (this.previousState != HeroState.Attacking)
                {
                    this.currentSpriteSheet = direction == Direction.Right ? attackingRightSpriteSheet : attackingLeftSpriteSheet;
                    this.currentSpriteSheet.Reset();
                    state = HeroState.Attacking;
                }
                else if (this.currentSpriteSheet.CurrentSprite == 1)
                {
                    forceX = (direction == Direction.Right) ? MaxVelocityX / 2 : -MaxVelocityX / 2;
                }
                else if (this.currentSpriteSheet.Done)
                {
                    this.currentSpriteSheet = direction == Direction.Right ? stationaryRightSpriteSheet : stationaryLeftSpriteSheet;
                    state = HeroState.Stationary;
                    this.currentSpriteSheet.Reset();
                    this.attacking = false;
                }
                else
                {
                    forceX = 0;
                }
            }
            else if (this.jumping)
            {
                this.currentSpriteSheet = direction == Direction.Right ? jumpingRightSpriteSheet : jumpingLeftSpriteSheet;
                state = HeroState.Jumping;

                if (this.previousState != HeroState.Jumping)
                {
                    this.currentSpriteSheet.Reset();
                }
            }
            else if (this.velocityX != 0)
            {
                // The character is moving, so select the moving sprite sheet according to which direction we're facing
                this.currentSpriteSheet = direction == Direction.Right ? movingRightSpriteSheet : movingLeftSpriteSheet;
                state = HeroState.Running;
            }
            else
            {
                // The character is stationary, default to the stationary sprite sheet
                this.currentSpriteSheet = direction == Direction.Right ? stationaryRightSpriteSheet : stationaryLeftSpriteSheet;
                state = HeroState.Stationary;
            }

            this.currentSpriteSheet.Update();
            this.previousState = state;
        }

        private void HandleAttacks(List<Character> enemies)
        {
            if (!this.attacking || this.currentSpriteSheet.CurrentSprite == 0)
            {
                return;
            }

            foreach (var enemy in enemies)
            {
                if (AttackLanded(enemy))
                {
                    enemy.Damage();
                    if (this.direction == Direction.Right)
                    {
                        enemy.DamageForce = new Vector2(50, -10);
                    }
                    else
                    {
                        enemy.DamageForce = new Vector2(-50, -10);
                    }
                }
            }
        }

        private bool AttackLanded(Character enemy)
        {
            if (enemy.HealthState == HealthState.Damaged || enemy.HealthState == HealthState.Dying)
            {
                return false;
            }

            var attackRectangle = this.direction == Direction.Right
                ? new Rectangle(this.Rectangle.X - 20, this.Rectangle.Y, this.Rectangle.Width, this.Rectangle.Height)
                : new Rectangle(this.Rectangle.X + 20, this.Rectangle.Y, this.Rectangle.Width, this.Rectangle.Height);

            return attackRectangle.Intersects(enemy.Rectangle);
        }

        private void HandleEnemies(List<Character> enemies)
        {
            if (this.attacking || this.HealthState == HealthState.Damaged)
            {
                return;
            }

            foreach (var enemy in enemies) 
            {
                if (enemy.HealthState == HealthState.Damaged || enemy.HealthState == HealthState.Dying)
                {
                    continue;
                }
                else if (enemy.Rectangle.Intersects(this.Rectangle))
                {
                    this.DamageForce = enemy.Rectangle.Left > this.Rectangle.Left
                        ? enemy.DamageForce = new Vector2(-50, -20)
                        : enemy.DamageForce = new Vector2(50, -20);

                    this.Damage();
                    return;
                }
            }
        }

        public override void VerticalCollisionHandler()
        {
            this.jumping = false;
        }
    }
}
