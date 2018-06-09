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
        [InlineData("4", "12", "48")]
        [InlineData("12", "34", "408")]
        [InlineData("41", "110", "4510")]
        [InlineData("1328", "3476", "4616128")]
        [InlineData("66729899957367990709339108031112", "32340867005124202389268050080611", "2158102819786481370385134761940486771461182643704842409095969432")]
        [InlineData("3141592653589793238462643383279502884197169399375105820974944592", "2718281828459045235360287471352662497757247093699959574966967627", "8539734222673567065463550869546574495034888535765114961879601127067743044893204848617875072216249073013374895871952806582723184")]
        [InlineData("54912929456625402256174854408349005059747367418045281224869677103460587001371838509047325775339825409089700424382586661886548455", "46159761840763928340146255755371712831239294454920928659343103820160462080093196044837908676121279113445132081834876936121057294", "2534767745696498721291598226410808420875926624404989594109011804054230420328163716811636469827804550673802278410568911221048870743264706668346786117132264407551450821426489441139772838020127634525234251586349419324977858145598895358201970734412370962180770")]
        public void MultiplyTests(string first, string second, string expectedResult) => Multiply(first, second).Should().Be(expectedResult);

        public static string Multiply(ReadOnlySpan<char> first, ReadOnlySpan<char> second)
        {
            ReadOnlySpan<char> zeroSpan = "0";
            var n = Math.Max(first.Length, second.Length);
            if(n == 1)
            {
                return (first[0].ToInt() * second[0].ToInt()).ToString();
            }
            var halfN = n / 2;
            var b = first.TakeLast(halfN);
            var a = first.Slice(0, first.Length - b.Length);
            if(a.IsEmpty) a = zeroSpan;
            var d = second.TakeLast(halfN);
            var c = second.Slice(0, second.Length - d.Length);
            if(c.IsEmpty) c = zeroSpan;
            var ac = Multiply(a, c);
            var bd = Multiply(b, d);
            var aPlusBTimesCPlusD = Multiply(Add(a, b), Add(c, d));
            var gaussTrick = aPlusBTimesCPlusD.DoSubstraction(ac).DoSubstraction(bd);
            var result = ac.ByTenToPower(halfN * 2).DoAddition(gaussTrick.ByTenToPower(halfN)).DoAddition(bd);
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
