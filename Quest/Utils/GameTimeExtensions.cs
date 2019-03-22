using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Utils
{
    internal static class GameTimeExtensions
    {
        public static long TotalMilliseconds(this GameTime time)
        {
            return (long) time.TotalGameTime.TotalMilliseconds;
        }
    }
}
