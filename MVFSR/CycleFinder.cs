using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVFSR
{
    internal class CycleFinder
    {
        public static int RunUntilCycle(ShiftRegister fast, ShiftRegister slow)
        {
            int counter = 0;
            do
            {
                fast.Update();
                fast.Update();
                slow.Update();
                ++counter;
            }
            while (fast.State != slow.State);

            //Console.WriteLine("slow and fast have state " + fast.ToString() + $" after {counter} steps");

            counter = 0;
            //Console.WriteLine(slow.ToString());
            do
            {
                fast.Update();
                fast.Update();
                slow.Update();
                //Console.WriteLine(slow.ToString());
                ++counter;
            }
            while (fast.State != slow.State);

            //Console.WriteLine("slow and fast have state " + fast.ToString() + $" after {counter} additional steps");

            return counter;
        }
    }
}
