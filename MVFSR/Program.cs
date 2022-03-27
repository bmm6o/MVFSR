
using MVFSR;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("MVFSR.Test")]

public class Program
{

    public static void Main(string[] args)
    {
        for (int registerWidth = 4; registerWidth <= 16; ++registerWidth)
        {
            FindShortestCycles(registerWidth);
            FindLongestCycles(registerWidth);
        }
    }

    public static void FindShortestCycles(int registerWidth)
    {
        int longestCycle = 0;
        int allOnes = (1 << registerWidth) - 1;
        for (var taps = 1; taps < 1 << registerWidth; taps++)
        {
            // for each tap configuration, figure out the shortest cycle
            int minCycle = int.MaxValue;

            var tapWeight = MajorityFeedback.PopCount(taps);

            if (tapWeight < 3 || tapWeight % 2 == 0) // need an odd number of taps for majority rule
                continue;

            HashSet<int> visitedStates = new HashSet<int>();
            for (int startVal = 0; startVal < 1 << registerWidth; startVal++)
            {
                if (visitedStates.Contains(startVal) || visitedStates.Contains(allOnes ^ startVal)) // symmetry between x and ~x
                    continue; // already accounted for the cycle this value is on

                var reg = new ShiftRegister(new MajorityFeedback(taps), startVal, registerWidth);

                int length = CycleFinder.RunUntilCycle(reg, visitedStates, minCycle);
                if (length < minCycle)
                {
                    //Console.WriteLine("taps " + taps.ToBinary(registerWidth) + " and startValue " + startVal.ToBinary(registerWidth) + " gives cycle length " + length);
                    minCycle = length;
                }
            }

            // does this tap configuration have a bigger minimum cycle?
            if (minCycle > longestCycle)
            {
                longestCycle = minCycle;
                //Console.WriteLine("taps " + taps.ToBinary(registerWidth) + " has minimum cycle length " + minCycle);
            }
        }
        Console.WriteLine($"for width {registerWidth}, min cycle length is {longestCycle}");
    }

    public static void FindLongestCycles(int registerWidth)
    {
        int maxCycle = 0;
        int allOnes = (1 << registerWidth) - 1;

        for (var taps = 1; taps < 1 << registerWidth; taps++)
        {
            var tapWeight = MajorityFeedback.PopCount(taps);

            if (tapWeight < 3 || tapWeight % 2 == 0) // need an odd number of taps for majority rule
                continue;

            HashSet<int> visitedStates = new HashSet<int>();
            for (int startVal = 0; startVal < 1 << registerWidth; startVal++)
            {
                if (visitedStates.Contains(startVal) || visitedStates.Contains(allOnes ^ startVal)) // symmetry between x and ~x
                    continue;

                var reg = new ShiftRegister(new MajorityFeedback(taps), startVal, registerWidth);

                int length = CycleFinder.RunUntilCycle(reg, visitedStates);
                if (length > maxCycle)
                {
                    //Console.WriteLine("taps " + taps.ToBinary(registerWidth) + " and startValue " + startVal.ToBinary(registerWidth) + " gives cycle length " + length);
                    maxCycle = length;
                }
            }
        }
        Console.WriteLine($"for width {registerWidth}, max cycle length is {maxCycle}");
    }
}
