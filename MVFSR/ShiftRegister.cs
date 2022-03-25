using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVFSR
{
    internal class ShiftRegister
    {
        public int State { get; private set; }

        public int Size { get; private set; }

        private IFeedback feedback;

        public ShiftRegister(IFeedback feedback, int state, int size)
        {
            this.feedback = feedback;
            this.State = state;
            this.Size = size;
        }

        public void Update()
        {
            int next = feedback.CalculateFeedback(State);
            State = State >> 1 | (next << Size - 1);
        }

        public override string ToString()
        {
            return Convert.ToString(State, 2).PadLeft(Size, '0');
        }
    }

    internal interface IFeedback
    {
        int CalculateFeedback(int state);
    }

    internal class MajorityFeedback : IFeedback
    {
        public int TapBitfield { get; private set; }

        private int tapCount;

        public MajorityFeedback(int tapBitfield)
        {
            TapBitfield = tapBitfield;
            tapCount = PopCount(tapBitfield);

            if (tapCount % 2 == 0)
                throw new ArgumentException("need an odd tap weight, 0x" + TapBitfield.ToString("X") + " has weight " + tapCount);
        }

        public int CalculateFeedback(int state)
        {
            int ones = PopCount(state & TapBitfield);
            int majority = 2 * ones > tapCount ? 1 : 0;
            return 1 - majority;
        }

        internal static int PopCount(int i)
        {
            int count = 0;
            while (i != 0)
            {
                count += (i & 1);
                i >>= 1;
            }
            return count;
        }
    }
}
