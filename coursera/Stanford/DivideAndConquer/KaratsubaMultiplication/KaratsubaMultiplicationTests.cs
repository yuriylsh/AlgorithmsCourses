using System;
using FluentAssertions;
using Xunit;
using static KaratsubaMultiplication.Utils;

namespace KaratsubaMultiplication
{
    public class KaratsubaMultiplicationTests
    {
        [Theory]
        [InlineData("2", "4", "8")]
        [InlineData("12", "34", "408")]
        [InlineData("41", "110", "4510")]
        [InlineData("1328", "3476", "4616128")]
        [InlineData("3141592653589793238462643383279502884197169399375105820974944592", "2718281828459045235360287471352662497757247093699959574966967627", "8539734222741967155080191038739493933468613928983910241397394344529084662710073354519280733807692469334393378112290702582823184")]
        public void MultiplyTests(string first, string second, string expectedResult) => Multiply(first, second).Should().Be(expectedResult);

        public static string Multiply(ReadOnlySpan<char> first, ReadOnlySpan<char> second)
        {
            var n = Math.Max(first.Length, second.Length);
            first = GrowNumberStringToLength(first, n);
            second = GrowNumberStringToLength(second, n);
            if(n == 1)
            {
                return (first[0].ToInt() * second[0].ToInt()).ToString();
            }
            var halfN = n / 2;
            var a = first.Slice(0, halfN);
            var b = first.Slice(halfN);
            var c = second.Slice(0, halfN);
            var d = second.Slice(halfN);
            var ac = Multiply(a, c);
            var bd = Multiply(b, d);
            var aPlusBTimesCPlusD = Multiply(Add(a, b), Add(c, d));
            var third = aPlusBTimesCPlusD.DoSubstraction(ac).DoSubstraction(bd);
            var result = ac.ByTenToPower(n).DoAddition(third.ByTenToPower(n % 2 == 0 ? halfN : halfN + 1)).DoAddition(bd);
            return result;
        }

        [Theory]
        [InlineData("1", "2", "3")]
        [InlineData("19", "1", "20")]
        [InlineData("999", "9", "1008")]
        public void AddTests(string first, string second, string expectedResult) => Add(first, second).Should().Be(expectedResult);

        public static string Add(ReadOnlySpan<char> first, ReadOnlySpan<char> second)
        {
            var carryOver = 0;

            var result = MathOperation(first, second, (x, y) =>
            {
                var addResult = x + y + carryOver;
                carryOver = addResult / 10;
                return carryOver == 0 ? addResult : addResult - (carryOver * 10);
            });

            if(carryOver != 0)
            {
                result.Add(carryOver.ToCharDigit());
            }
            
            return result.GetMathOperationResult();
        }

        [Theory]
        [InlineData("3", "2", "1")]
        [InlineData("10", "1", "9")]
        [InlineData("1008", "9", "999")]
        [InlineData("18", "8", "10")]
        public void SubstractTest(string first, string second, string expectedResult) => Substract(first, second).Should().Be(expectedResult);

        public static string Substract(ReadOnlySpan<char> first, ReadOnlySpan<char> second)
        {
            var needsToBorrow = false;
            var result = MathOperation(first, second, (x, y) =>
            {
                if(needsToBorrow)
                {
                    x = x - 1;
                }

                if(y > x)
                {
                    needsToBorrow = true;
                    return x + 10 - y;
                }

                needsToBorrow = false;
                return (x - y);
            });
            
            return result.GetMathOperationResult().TrimLeadingZeroes();
        }
        
    }
}
