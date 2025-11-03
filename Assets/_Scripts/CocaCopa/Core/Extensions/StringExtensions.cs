using System;

namespace CocaCopa.Core.Text {
    /// <summary>
    /// Represents a scaled integer: Value / Scale == original decimal.
    /// </summary>
    public readonly struct ScaledInt {
        public readonly int Value;
        public readonly int Scale;
        public readonly bool Success;

        public ScaledInt(int value, int scale, bool success) {
            Value = value;
            Scale = scale;
            Success = success;
        }

        public override string ToString() => Success ? $"{Value}/{Scale}" : "Invalid";
    }

    public static class StringExtensions {
        /// <summary>
        /// Parses a decimal string using the provided decimal separator into (value, scale).
        /// </summary>
        /// <param name="text"></param>
        /// <param name="decimalChar"></param>
        /// <returns>Success=false on invalid input or overflow; otherwise Success=true</returns>
        public static ScaledInt TryParseScaledInt(this string text, char decimalChar) {
            if (string.IsNullOrWhiteSpace(text))
                return new ScaledInt(0, 1, success: false);

            ReadOnlySpan<char> s = text.AsSpan().Trim();

            int sign = 1;
            int i = 0;

            if (s.Length > 0 && (s[0] == '+' || s[0] == '-')) {
                if (s[0] == '-') sign = -1;
                i++;
            }

            if (i >= s.Length) return new ScaledInt(0, 1, false);

            int value = 0;
            int scale = 1;
            bool seenDecimal = false;
            int decimals = 0;

            for (; i < s.Length; i++) {
                char c = s[i];

                if (c == decimalChar) {
                    if (seenDecimal) return new ScaledInt(0, 1, false); // second decimal
                    seenDecimal = true;
                    continue;
                }

                if (c < '0' || c > '9')
                    return new ScaledInt(0, 1, false);

                int digit = c - '0';

                // value = value * 10 + digit; (checked to avoid overflow)
                try {
                    checked { value = value * 10 + digit; }
                }
                catch (OverflowException) {
                    return new ScaledInt(0, 1, false);
                }

                if (seenDecimal) {
                    // scale *= 10; bound decimals to something sane (e.g., 9)
                    if (decimals == 9) return new ScaledInt(0, 1, false); // prevent crazy scales
                    scale *= 10;
                    decimals++;
                }
            }

            // Edge cases like "." or "-." → invalid
            if (value == 0 && !ContainsAnyDigit(s, decimalChar))
                return new ScaledInt(0, 1, false);

            return new ScaledInt(sign * value, scale, true);
        }

        private static bool ContainsAnyDigit(ReadOnlySpan<char> s, char decimalChar) {
            foreach (var ch in s)
                if (ch != '+' && ch != '-' && ch != decimalChar && ch >= '0' && ch <= '9')
                    return true;
            return false;
        }
    }
}
