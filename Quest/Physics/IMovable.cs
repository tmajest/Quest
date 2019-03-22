using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Physics
{
    public interface IMovable
    {
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        Vector2 MaxVelocity { get; set; }
        Vector2 Force { get; set; }

        float Gravity { get; }
        float Friction { get; }

        Rectangle Rectangle { get; }

        void HorizontalCollisionHandler();
        void VerticalCollisionHandler();
    }
}
