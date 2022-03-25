
using MVFSR;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("MVFSR.Test")]

const int registerWidth = 16;
//int tapWeight = 5;
int max = 0;

for (var taps = 1; taps < 1 << registerWidth; taps++)
{
    var tapWeight = MajorityFeedback.PopCount(taps);

    if (tapWeight % 2 == 0)
        continue;

    var fast = new ShiftRegister(new MajorityFeedback(taps), 0, registerWidth);
    var slow = new ShiftRegister(new MajorityFeedback(taps), 0, registerWidth);

    int length = CycleFinder.RunUntilCycle(fast, slow);
    if (length > max)
    {
        Console.WriteLine("taps " + Convert.ToString(taps, 2).PadLeft(registerWidth, '0') + " gives cycle length " + length);
        max = length;
    }
}
Console.WriteLine("max: " + max);

