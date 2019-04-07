using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Quest.Levels;
using Quest.Physics;
using Quest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Characters
{
    internal class Character : IMovable
    {
        protected float x;
        protected float y;
        protected float velocityX;
        protected float velocityY;
        protected float maxVelocityX;
        protected float maxVelocityY;
        protected float forceX { get; set; }
        protected float forceY { get; set; }
        protected float damageForceX { get; set; }
        protected float damageForceY { get; set; }
        protected Direction direction;
        public Color Color { get; set; }

        private int width;
        private int height;

        private PhysicsEngine physicsEngine;

        public Vector2 Position
        {
            get { return new Vector2(this.x, this.y); }
            set { this.x = value.X; this.y = value.Y; }
        }

        public Vector2 Velocity
        {
            get { return new Vector2(this.velocityX, this.velocityY); }
            set { this.velocityX = value.X; this.velocityY = value.Y; }
        }

        public Vector2 MaxVelocity
        {
            get { return new Vector2(this.maxVelocityX, this.maxVelocityY); }
            set { this.maxVelocityX = value.X; this.maxVelocityY = value.Y; }
        }

        public Vector2 Force
        {
            get { return new Vector2(this.forceX, this.forceY); }
            set { this.forceX = value.X; this.forceY = value.Y; }
        }

        public Vector2 DamageForce
        {
            get { return new Vector2(this.damageForceX, this.damageForceY); }
            set { this.damageForceX = value.X; this.damageForceY = value.Y; }
        }

        public int Health { get; set; }
        public HealthState HealthState { get; set; }
        public HealthState PreviousHealthState { get; set; }

        private int invincibleFrames;
        private int totalInvincibilityFrames;

        public virtual float Gravity => PhysicsConstants.Gravity;
        public virtual float Friction => PhysicsConstants.Friction;

        public Rectangle Rectangle => new Rectangle((int) this.x, (int) this.y, this.width, this.height);

        public Character(
            Vector2 position,
            Vector2 velocity,
            Vector2 maxVelocity,
            Vector2 force,
            PhysicsEngine physicsEngine,
            int width, int height, 
            int health,int totalInvincibilityFrames,
            Direction direction)
        {
            this.x = position.X;
            this.y = position.Y;
            this.velocityX = velocity.X;
            this.velocityY = velocity.Y;
            this.maxVelocityX = maxVelocity.X;
            this.maxVelocityY = maxVelocity.Y;
            this.forceX = force.X;
            this.forceY = force.Y;

            this.physicsEngine = physicsEngine;

            this.width = width;
            this.height = height;
            this.Health = health;
            this.invincibleFrames = 0;
            this.totalInvincibilityFrames = totalInvincibilityFrames;
            this.direction = direction;
            this.Color = Color.White;

            this.HealthState = HealthState.Normal;
            this.PreviousHealthState = this.HealthState;
        }


        public virtual void Update(Level level)
        {
            this.PreviousHealthState = this.HealthState;

            physicsEngine.Move(this);
            if (this.HealthState == HealthState.Damaged && invincibleFrames > 0)
            {
                invincibleFrames--;
                if (invincibleFrames == 0)
                {
                    this.HealthState = HealthState.Normal;
                    this.Color = Color.White;
                }
            }
        }

        public virtual void Draw(Camera camera)
        {
        }

        public void Damage()
        {
            if (this.HealthState != HealthState.Dying && this.HealthState != HealthState.Damaged)
            {
                this.Color = Color.Red;
                this.HealthState = HealthState.Damaged;
                this.invincibleFrames = totalInvincibilityFrames;
                this.Health--;

                if (this.Health == 0)
                {
                    this.HealthState = HealthState.Dying;
                    this.Color = Color.White;
                }
            }
        }

        public virtual void HorizontalCollisionHandler()
        {
            // Do nothing for base class
        }

        public virtual void VerticalCollisionHandler()
        {
            // Do nothing for base class
        }
    }
}
