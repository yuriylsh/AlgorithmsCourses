using System;
using System.Collections.Generic;
using System.Linq;

namespace KaratsubaMultiplication
{
    internal static class Utils
    {
        private const int IntZeroChar = '0';

        public static List<char> MathOperation(ReadOnlySpan<char> first, ReadOnlySpan<char> second, Func<int, int, int> operate)
        {
            var inputLength = Math.Max(first.Length, second.Length);
            var result = new List<char>(inputLength + 1);
            var firstPadding = inputLength - first.Length;
            var secondPadding = inputLength - second.Length;
           
            for(var i = inputLength - 1; i >= 0; i--)
            {
                var firstIndex = i - firstPadding;
                var firstNumberAtPosition =  i < firstPadding ? 0 : (firstIndex  < first.Length ? first[firstIndex].ToInt() : 0);

                var secondIndex = i - secondPadding;
                var secondNumberAtPosition = i < secondPadding ? 0 : (secondIndex  < second.Length ? second[secondIndex].ToInt() : 0);
                var operationResult = operate(firstNumberAtPosition, secondNumberAtPosition);
                result.Add(operationResult.ToCharDigit());
            }

            return result;
        }

        public static int ToInt(this char c) => c - '0';

        public static char ToCharDigit(this int x) => (char)(x + IntZeroChar);

        public static string GetMathOperationResult(this IEnumerable<char> input)
            => new string(input.Reverse().ToArray());

        public static ReadOnlySpan<char> GrowNumberStringToLength(in ReadOnlySpan<char> numberString, int length) => numberString.Length == length
            ? numberString
            : ZeroesString(length - numberString.Length) + new string(numberString);

        public static string ByTenToPower(this string input, int power) => input + ZeroesString(power);

        private static string ZeroesString(int count)
        {
            var zeroesArray = new char[count];
            for (var i = 0; i < count; i++)
            {
                zeroesArray[i] = '0';
            }
            return new string(zeroesArray);
        }

        public static string TrimLeadingZeroes(this string final) => final[0] == '0' ? new string(final.SkipWhile(ch => ch == '0').ToArray()) : final;

        public static string DoSubstraction(this string first, string second) => KaratsubaMultiplicationTests.Substract(first, second);

        public static string DoAddition(this string first, string second) => KaratsubaMultiplicationTests.Add(first, second);
    }
}