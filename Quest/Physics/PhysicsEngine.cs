using Microsoft.Xna.Framework;

using Quest.Levels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Physics
{
    internal class PhysicsEngine
    {
        public Level Level { get; set; }
        
        public PhysicsEngine(Level level)
        {
            this.Level = level;
        }

        public void Move(IMovable movable)
        {
            MoveHorizontal(movable);
            MoveVertical(movable);
        }

        private void MoveHorizontal(IMovable movable)
        {
            var oldVelX = movable.Velocity.X;

            // Add forces in the horizontal direction to update horizontal velocity. Use Clamp() function to ensure that
            // the new velocity doesn't exceed the max velocity in either direction
            var newVelocityX = movable.Velocity.X + movable.Force.X;
            newVelocityX = MathHelper.Clamp(newVelocityX, -movable.MaxVelocity.X, movable.MaxVelocity.X);

            // Add friction to reduce velocity if we're not stationary
            if (movable.Velocity.X != 0)
            {
                newVelocityX = newVelocityX > 0
                    ? Math.Max(0, newVelocityX - movable.Friction)
                    : Math.Min(0, newVelocityX + movable.Friction);
            }
            movable.Velocity = new Vector2(newVelocityX, movable.Velocity.Y);

            // Update our X position according to our horizontal velocity
            var newX = movable.Position.X + newVelocityX;
            movable.Position = new Vector2(newX, movable.Position.Y);

            // After updating our position, check to see if we've collided with a wall.
            // If so, handle the collision
            var intersection = this.Level.Collides(movable.Rectangle);
            if (intersection != Rectangle.Empty)
            {
                // If we were moving to the right, we collided with the left side of the wall. Therefore we
                // must snap the character to the left side of the wall.
                // If we were moving to the left, we collided with the right side of the wall. Therefore we
                // must snap the character to the right side of the wall.
                newX = oldVelX > 0 ? intersection.Left - movable.Rectangle.Width : intersection.Right;
                movable.Position = new Vector2(newX, movable.Position.Y);

                movable.HorizontalCollisionHandler();
            }
        }

        internal virtual void MoveVertical(IMovable movable)
        {
            var oldVelY = movable.Velocity.Y;

            // Add forces in the vertical direction to update vertical velocity. Use Clamp() function to ensure that
            // the new velocity doesn't exceed the max velocity in either direction
            var newVelocityY = MathHelper.Clamp(movable.Velocity.Y + movable.Force.Y + movable.Gravity, -movable.MaxVelocity.Y, movable.MaxVelocity.Y);
            movable.Velocity = new Vector2(movable.Velocity.X, newVelocityY);

            // Update Y position according to Y velocity
            var newY = movable.Position.Y + newVelocityY;
            movable.Position = new Vector2(movable.Position.X, newY);

            // After updating our vertical position, check to see if we've collided with a wall.
            // If so, handle the collision
            var intersection = this.Level.Collides(movable.Rectangle);
            if (intersection != Rectangle.Empty)
            {
                // If we were moving up, we collided with the bottom side of the wall. Therefore we
                // must snap the character to the bottom of the wall.
                // If we were moving down, we collided with the top of the wall. Therefore we
                // must snap the character to the top of the wall.
                newY = oldVelY > 0 ? intersection.Top - movable.Rectangle.Height : intersection.Bottom;

                movable.Position = new Vector2(movable.Position.X, newY);
                movable.Velocity = new Vector2(movable.Velocity.X, 0);

                movable.VerticalCollisionHandler();
            }
        }
    }
}
