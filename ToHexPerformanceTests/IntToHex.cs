using System;
using ToHexPerformance;
using Xunit;

namespace ToHexPerformanceTests
{
    public class ToHexTest
    {
        [Fact]
        public void IntToHexTest()
        {
            Random r = new Random();

            for (int i = 0; i < 50000; i++)
            {
                int value = r.Next(int.MinValue, int.MaxValue);

                string expected = value.ToString("x");
                string actual = NumberFormatingNew.FormatInt32(value, "x", null);

                Assert.Equal(expected, actual);
            }
        }
    }
}
