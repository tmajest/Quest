﻿using Microsoft.Xna.Framework;
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
    internal class Hero : MovingSprite
    {
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

        private Keys previousKey;
        private HeroState previousState;
        private int jumpsLeft;
        private bool jumping;
        private bool attacking;

        public Hero(
            SpriteSheet stationaryLeftSpriteSheet,
            SpriteSheet stationaryRightSpriteSheet,
            SpriteSheet movingLeftSpriteSheet,
            SpriteSheet movingRightSpriteSheet,
            SpriteSheet jumpingLeftSpriteSheet,
            SpriteSheet jumpingRightSpriteSheet,
            SpriteSheet attackingLeftSpriteSheet,
            SpriteSheet attackingRightSpriteSheet,
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
            this.attackingLeftSpriteSheet = attackingLeftSpriteSheet;
            this.attackingRightSpriteSheet = attackingRightSpriteSheet;

            this.jumpsLeft = MaxJumps;
            this.previousKey = Keys.None;
            this.jumping = false;
            this.attacking = false;
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
                position,
                physicsEngine,
                HeroWidth,
                HeroHeight,
                direction);
        }

        public override void Update(Level level)
        {
            base.Update(level);

            var currentKey = this.HandleInput();
            this.HandleSpriteSheet(currentKey);

            this.previousKey = currentKey;
        }

        public override void Draw(Camera camera)
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

            if (Keyboard.GetState().IsKeyDown(Keys.F) && !this.attacking)
            {
                this.attacking = true;
                forceX = (direction == Direction.Right) ? MaxVelocityX : -MaxVelocityX;
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

        private void HandleSpriteSheet(Keys currentKey)
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
                if (this.currentSpriteSheet.Done)
                {
                    this.currentSpriteSheet = direction == Direction.Right ? stationaryRightSpriteSheet : stationaryLeftSpriteSheet;
                    state = HeroState.Stationary;
                    this.currentSpriteSheet.Reset();
                    this.attacking = false;
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

        public override void VerticalCollisionHandler()
        {
            this.jumping = false;
        }
    }
}
