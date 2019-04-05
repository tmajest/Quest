using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest
{
    internal class FrameCounter
    {
        public int Frame { get; private set; }

        public FrameCounter()
        {
            this.Frame = 0;
        }

        public void Update()
        {
            this.Frame++;
            if (this.Frame < 0)
            {
                this.Reset();
            }
        }

        public void Reset()
        {
            this.Frame = 0;
        }
    }
}
