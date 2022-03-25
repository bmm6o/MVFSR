
using MVFSR;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("MVFSR.Test")]


for (int registerWidth = 4; registerWidth <= 16; ++registerWidth)
{
    int max = 0;

    for (var taps = 1; taps < 1 << registerWidth; taps++)
    {
        var tapWeight = MajorityFeedback.PopCount(taps);

        if (tapWeight < 3 || tapWeight % 2 == 0) // need an odd number of taps for majority rule
            continue;

        for (int startVal = 0; startVal < 1 << registerWidth; startVal++)
        {
            var reg = new ShiftRegister(new MajorityFeedback(taps), startVal, registerWidth);

            int length = CycleFinder.RunUntilCycle(reg);
            if (length > max)
            {
                Console.WriteLine("taps " + taps.ToBinary(registerWidth) + " and startValue " + startVal.ToBinary(registerWidth) + " gives cycle length " + length);
                max = length;
            }
        }
    }
    Console.WriteLine($"for width {registerWidth}, max cycle length is {max}");
}


