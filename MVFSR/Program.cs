
using MVFSR;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("MVFSR.Test")]

for (int registerWidth = 4; registerWidth <= 30; ++registerWidth)
{
    int max = 0;

    for (var taps = 1; taps < 1 << registerWidth; taps++)
    {
        var tapWeight = MajorityFeedback.PopCount(taps);

        if (tapWeight % 2 == 0) // need an odd number of taps for majority rule
            continue;

        var fast = new ShiftRegister(new MajorityFeedback(taps), 0, registerWidth);
        var slow = new ShiftRegister(new MajorityFeedback(taps), 0, registerWidth);

        int length = CycleFinder.RunUntilCycle(fast, slow);
        if (length > max)
        {
            //Console.WriteLine("taps " + Convert.ToString(taps, 2).PadLeft(registerWidth, '0') + " gives cycle length " + length);
            max = length;
        }
    }
    Console.WriteLine($"for width {registerWidth}, max cycle length is {max}");
}


