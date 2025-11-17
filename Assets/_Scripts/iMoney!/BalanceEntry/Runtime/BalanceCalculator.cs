using System.Globalization;
using CocaCopa.Core.Numerics;
using CocaCopa.Logger.API;

namespace iMoney.BalanceEntry.Runtime {
    internal class BalanceCalculator {
        private enum Operation { Add, Subtract };

        public static string Add(string current, string amount) => Sum(current, amount, Operation.Add);
        public static string Subtract(string current, string amount) => Sum(current, amount, Operation.Subtract);

        private static string Sum(string num1, string num2, Operation op) {
            if (num1.Contains("€")) num1 = num1.Replace("€", "");
            if (num2.Contains("€")) num2 = num2.Replace("€", "");

            ScaledInt scaledNum1 = ScaledIntParser.TryParseScaledInt(num1, '.');
            ScaledInt scaledNum2 = ScaledIntParser.TryParseScaledInt(num2, '.');

            if (scaledNum1.Success && scaledNum2.Success) {
                float sum = op == Operation.Add
                ? (float)scaledNum1.Value / scaledNum1.Scale + (float)scaledNum2.Value / scaledNum2.Scale
                : (float)scaledNum1.Value / scaledNum1.Scale - (float)scaledNum2.Value / scaledNum2.Scale;

                if (sum < 0f) sum = 0f;
                if (sum > 99999.99f) sum = 99999.99f;

                var format = new NumberFormatInfo { NumberDecimalSeparator = "." };
                return sum.ToString("0.00", format);
            }

            return string.Empty;
        }
    }
}
