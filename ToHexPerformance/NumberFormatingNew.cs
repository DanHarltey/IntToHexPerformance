using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace ToHexPerformance
{
    public static class NumberFormatingNew
    {

        public static unsafe string FormatInt32(int value, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            char fmt = ParseFormatSpecifier(format, out int digits);

            // The fmt-(X-A+10) hack has the effect of dictating whether we produce uppercase or lowercase
            // hex numbers for a-f. 'X' as the fmt code produces uppercase. 'x' as the format code produces lowercase.
            return Int32ToHexStr(value, (char)(fmt - ('X' - 'A' + 10)));
        }

        private static unsafe string Int32ToHexStr(int value, char hexBase)
        {
            uint uValue = (uint)value;

            int bufferLength = Math.Max(1, CountHexDigits(uValue));

            ulong state = hexBase;
            state <<= 32;
            state |= uValue;

            string result = string.Create(bufferLength, state, action);

            return result;
        }

        private static SpanAction<char, ulong> action = Int32ToHexChars;

        private static void Int32ToHexChars(Span<char> buffer, ulong state)
        {
            int hexBase = (int)(state >> 32);
            //uint value = (uint)state;

            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                byte digit = (byte)(state & 0xF);
                buffer[i] = (char)(digit + (digit < 10 ? (byte)'0' : hexBase));
                state >>= 4;
            }
        }

        internal static unsafe char ParseFormatSpecifier(ReadOnlySpan<char> format, out int digits)
        {
            char c = default;
            if (format.Length > 0)
            {
                // If the format begins with a symbol, see if it's a standard format
                // with or without a specified number of digits.
                c = format[0];
                if ((uint)(c - 'A') <= 'Z' - 'A' ||
                    (uint)(c - 'a') <= 'z' - 'a')
                {
                    // Fast path for sole symbol, e.g. "D"
                    if (format.Length == 1)
                    {
                        digits = -1;
                        return c;
                    }

                    if (format.Length == 2)
                    {
                        // Fast path for symbol and single digit, e.g. "X4"
                        int d = format[1] - '0';
                        if ((uint)d < 10)
                        {
                            digits = d;
                            return c;
                        }
                    }
                    else if (format.Length == 3)
                    {
                        // Fast path for symbol and double digit, e.g. "F12"
                        int d1 = format[1] - '0', d2 = format[2] - '0';
                        if ((uint)d1 < 10 && (uint)d2 < 10)
                        {
                            digits = d1 * 10 + d2;
                            return c;
                        }
                    }

                    // Fallback for symbol and any length digits.  The digits value must be >= 0 && <= 99,
                    // but it can begin with any number of 0s, and thus we may need to check more than two
                    // digits.  Further, for compat, we need to stop when we hit a null char.
                    int n = 0;
                    int i = 1;
                    while (i < format.Length && (((uint)format[i] - '0') < 10) && n < 10)
                    {
                        n = (n * 10) + format[i++] - '0';
                    }

                    // If we're at the end of the digits rather than having stopped because we hit something
                    // other than a digit or overflowed, return the standard format info.
                    if (i == format.Length || format[i] == '\0')
                    {
                        digits = n;
                        return c;
                    }
                }
            }

            // Default empty format to be "G"; custom format is signified with '\0'.
            digits = -1;
            return format.Length == 0 || c == '\0' ? // For compat, treat '\0' as the end of the specifier, even if the specifier extends beyond it.
                'G' :
                '\0';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountHexDigits(ulong value)
        {
            return (64 - BitOperations.LeadingZeroCount(value | 1) + 3) >> 2;
        }
    }
}
