using Xunit;

namespace MVFSR.Test
{
    public class ShiftRegisterTest
    {

        [Fact]
        public void ItCorrectlyShiftsState()
        {
            var feedback = new ConstantFeedback(1);
            var mvfsr = new ShiftRegister(feedback, 0, 7);

            Assert.Equal(0, mvfsr.State);

            mvfsr.Update();

            Assert.Equal(0b1000000, mvfsr.State);

            mvfsr.Update();
            mvfsr.Update();

            Assert.Equal(0b1110000, mvfsr.State);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(7, 3)]
        [InlineData(0x7fffffff, 31)]
        [InlineData(0b01010101010101010101010101010101, 16)]
        public void ItCorrectlyCalculatesPopulation(int i, int popCount)
        {
            Assert.Equal(popCount, MajorityFeedback.PopCount(i));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0xff, 1)]
        [InlineData(0b01010101, 1)]
        [InlineData(0b10001011, 0)]
        public void ItCorrectlyCalculatesMajority(int state, int majority)
        {
            var feedback = new MajorityFeedback(0b01010101);

            Assert.Equal(majority, 1 - feedback.CalculateFeedback(state)); // 1- because feedback is opposite of majority
        }
    }

    internal class ConstantFeedback : IFeedback
    {
        private int feedbackValue;
        public ConstantFeedback(int feedbackValue)
        {
            this.feedbackValue = feedbackValue;
        }
        public int CalculateFeedback(int state)
        {
            return feedbackValue;
        }
    }
}