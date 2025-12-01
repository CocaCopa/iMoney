using System.Globalization;
using CocaCopa.Core.Numerics;

namespace iMoney.App.BalanceEntry.Runtime {
    internal class BalanceCalculator {
        private enum Operation { Add, Subtract };

        public static string Add(string current, string amountToAdd) => Sum(current, amountToAdd, Operation.Add);
        public static string Subtract(string current, string amountToSubtract) => Sum(current, amountToSubtract, Operation.Subtract);

        private static string Sum(string num1, string num2, Operation op) {
            ScaledInt scaledNum1 = ParseAmount(num1);
            ScaledInt scaledNum2 = ParseAmount(num2);

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

        public static ScaledInt ParseAmount(string amount) {
            if (amount.Contains("€")) { amount = amount.Replace("€", ""); }
            return ScaledIntParser.TryParseScaledInt(amount, '.');
        }

        public static bool TryParseAmount(string amount, out int amountInt, out int amountScale) {
            amountInt = amountScale = 0;
            ScaledInt scaledAmount = ParseAmount(amount);
            if (scaledAmount.Success) {
                amountInt = scaledAmount.Value;
                amountScale = scaledAmount.Scale;
                return true;
            }
            return false;
        }
    }
}
