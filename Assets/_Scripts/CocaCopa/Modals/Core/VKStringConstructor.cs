using System;
using System.Diagnostics;
using CocaCopa.Logger;

namespace CocaCopa.Modal.Core {
    public class VKStringConstructor {
        private const int MaxDecimal = 2;
        private string virtualString;
        private NumpadData data;
        private int carretIndex;
        private bool onDecimals;

        public int CarretIndex => carretIndex;

        public VKStringConstructor() {
            virtualString = string.Empty;
            data = new NumpadData();
        }

        public NumpadData VirtualString(char newChar, ConstructionMode mode) {
            virtualString = DecimalNumpad_Add(newChar);
            data = ExtractNumpadDate(virtualString);
            return data;
        }

        public NumpadData EraseLastChar() {
            if (carretIndex == 0) { return data; }
            carretIndex--;

            // Special case where the carret is on the decimal symbol
            // Move the carret one pos extra to the left to avoid it
            if (carretIndex == virtualString.Length - 1 - MaxDecimal) {
                carretIndex--;
                onDecimals = false;
            }

            // Removing a decimal - Replace all characters
            // to the right side of the carret with a '0'
            if (onDecimals) {
                var rmCount = virtualString.Length - carretIndex;
                virtualString = virtualString[..^rmCount];
                for (int i = 0; i < rmCount; i++) {
                    virtualString += '0';
                }
            }
            // Remove the last character of the left side str
            else {
                string[] parts = virtualString.Split('.');
                string newStr;
                if (parts[0].Length > 1) {
                    newStr = parts[0][..^1];
                    newStr += $".{parts[1]}";
                }
                else { newStr = string.Empty; }

                virtualString = newStr;
            }

            data = ExtractNumpadDate(virtualString);
            return data;
        }

        private static NumpadData ExtractNumpadDate(string currentStr) {
            string vString = currentStr;
            int vValue = 0;
            int decCount = 0;
            if (vString.Contains(".")) {
                var parts = vString.Split('.');
                decCount = parts[1].Length;
                vValue = int.TryParse(parts[0] + parts[1], out int val) ? val : 0;
            }
            else if (int.TryParse(vString, out int val)) { vValue = val; }

            return new(vString, vValue, decCount);
        }

        private string DecimalNumpad_Add(char newChar) {
            // Reject more than 2 decimals
            if (virtualString.Contains('.') && carretIndex == virtualString.Length && onDecimals) {
                return virtualString;
            }

            // First Input. Special case of '.' or '0'
            // We are immediately on decimals
            if (virtualString.Length == 0 && (newChar.Equals('.') || newChar.Equals('0'))) {
                carretIndex = 2;
                onDecimals = true;
                return "0.00";
            }
            // Else, construct the string normally
            else if (virtualString.Length == 0) {
                carretIndex = 1;
                return $"{newChar}.00";
            }

            // String-wise nothing new. Just update the system
            // to know that the user will type decimal numbers next
            if (newChar.Equals('.') && !onDecimals) {
                carretIndex++;
                onDecimals = true;
                return virtualString;
            }
            // If we are already in the decimal zone, return
            else if (newChar.Equals('.') && onDecimals) {
                return virtualString;
            }

            // Insert the given character to the carret position
            string newStr = "";
            for (int i = 0; i < virtualString.Length; i++) {
                newStr += virtualString[i];
                if (carretIndex == i + 1) {
                    newStr += newChar;
                }
            }
            // Decimals case - After inserting the new char, remove
            // the last one to keep the length of the str the same
            if (onDecimals) { newStr = newStr[..^1]; }

            carretIndex++;
            return newStr;
        }
    }
}
