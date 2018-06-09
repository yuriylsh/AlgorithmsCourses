using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace NumberOfInversions
{
    public class InputLoading
    {
        public const string SampleInputFileName = "sampleinput.txt";
        public const string ProblemInputFileName = "probleminput.txt";

        public static int[] LoadInput(string fileName) => 
            File.ReadLines(GetInputPath(fileName))
                .TakeWhile(line => line.Length > 0)
                .Select(int.Parse)
                .ToArray();

        [Fact]
        private void LoadInput_Sample_ReturnsSampleInputAsArray()
            => LoadInput(SampleInputFileName).Should().Equal(3, 1, 5, 4, 2);

        [Fact]
        private void LoadInput_Problem_ReturnsProblemInputAsArray()
            => LoadInput(ProblemInputFileName).Should().HaveCount(100_000);

        private static string GetInputPath(string fileName) => Path.Combine(CurrentDirectoryPath, fileName);

        private static string CurrentDirectoryPath => _currentDirectoryPath 
            ?? (_currentDirectoryPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path)));

        private static string _currentDirectoryPath; 

        [Theory]
        [InlineData(SampleInputFileName)]
        [InlineData(ProblemInputFileName)]
        private void InputFilesExistTests(string fileName)
            => File.Exists(GetInputPath(fileName)).Should().BeTrue();
    }
}
