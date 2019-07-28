using System;
using ToHexPerformance;
using Xunit;

namespace ToHexPerformanceTests
{
    public class ToHexTest
    {
        const int testSize = 100_000_000;
        [Fact]
        public void IntToHexTest()
        {
            Random r = new Random();

            for (int i = 0; i < testSize; i++)
            {
                int value = r.Next(int.MinValue, int.MaxValue);

                string expected = value.ToString("x");
                string actual = NumberFormatingNew.FormatInt32(value, "x", null);

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void IntToUpperHexTest()
        {
            Random r = new Random();

            for (int i = 0; i < testSize; i++)
            {
                int value = r.Next(int.MinValue, int.MaxValue);

                string expected = value.ToString("X");
                string actual = NumberFormatingNew.FormatInt32(value, "X", null);

                Assert.Equal(expected, actual);
            }
        }


        [Fact]
        public void CompactState()
        {
            const uint value = 1;
            const int hexValue = 84;

            ulong compact = hexValue;
            compact <<= 32;
            compact |= value;

            int actualHex = (int)(compact >> 32);
            Assert.Equal(hexValue, actualHex);

            uint actualValue = (uint)compact;
            Assert.Equal(value, actualValue);
        }
    }
}
