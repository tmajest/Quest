using Microsoft.Xna.Framework;

using Quest.Characters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Physics
{
    internal interface IMovable
    {
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        Vector2 MaxVelocity { get; set; }
        Vector2 Force { get; set; }
        Vector2 DamageForce { get; set; }

        float Gravity { get; }
        float Friction { get; }

        HealthState HealthState { get; }

        Rectangle Rectangle { get; }

        void HorizontalCollisionHandler();
        void VerticalCollisionHandler();
    }
}
