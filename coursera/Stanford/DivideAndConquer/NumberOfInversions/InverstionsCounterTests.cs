using System;
using FluentAssertions;
using Xunit;

namespace NumberOfInversions
{
    public class InverstionsCounterTests
    {
        public static uint CountInversions(int[] input)
            => SortAndCountInversions(input, 0, input.Length -1);

        private static uint SortAndCountInversions(int[] input, int leftIndex, int rightIndex)
        {
            if (leftIndex == rightIndex) return 0;

            var middle = (leftIndex + rightIndex) / 2;
            var leftInversions = SortAndCountInversions(input, leftIndex, middle);
            var rightInversions = SortAndCountInversions(input, middle + 1, rightIndex);
            var splitInversions = MergeAndCountInversions(input, leftIndex, middle, rightIndex);
            return leftInversions + rightInversions + splitInversions;
        }

        private static uint MergeAndCountInversions(Span<int> array, int leftIndex, int middleIndex, int rightIndex)
        {
            Span<int> leftArray = array.Slice(leftIndex, middleIndex - leftIndex + 1).ToArray();
            Span<int> rightArray = array.Slice(middleIndex + 1, rightIndex - middleIndex).ToArray();

            int leftCounter = 0, rightCounter = 0;
            uint splitInversions = 0;
            for (var i = leftIndex; i <= rightIndex; i++)
            {
                var copyRestSink = array.Slice(i + 1);
                var isInversion = leftArray[leftCounter] > rightArray[rightCounter];
                if (isInversion)
                {
                    splitInversions += (uint)(leftArray.Length - leftCounter);
                    array[i] = rightArray[rightCounter++];
                    if(CopyRestOfFirstIfSecondIsEmpty(leftArray.Slice(leftCounter), rightArray.Slice(rightCounter), copyRestSink)) break;
                }
                else
                {
                    array[i] = leftArray[leftCounter++];
                    if(CopyRestOfFirstIfSecondIsEmpty(rightArray.Slice(rightCounter), leftArray.Slice(leftCounter), copyRestSink)) break;
                }
            }

            return splitInversions;
        }

        private static bool CopyRestOfFirstIfSecondIsEmpty(Span<int> first, Span<int> second, Span<int> sink)
        {
            if (second.IsEmpty)
            {
                first.CopyTo(sink);
            }
            return second.IsEmpty;
        }

        [Theory]
        [InlineData(InputLoading.SampleInputFileName, 5)]
        [InlineData(InputLoading.ProblemInputFileName, 2407905288)]
        private void Count_NonEmptyInput_ReturnsCorrectNumberOfInversions(string fileName, uint count) 
            => CountInversions(InputLoading.LoadInput(fileName)).Should().Be(count);
    }
}