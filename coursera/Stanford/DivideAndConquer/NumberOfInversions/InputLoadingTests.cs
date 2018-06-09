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
        public static int[] LoadInput(string fileName) => 
            File.ReadLines(Path.Combine(CurrentDirectoryPath, fileName))
                .TakeWhile(line => line.Length > 0)
                .Select(int.Parse)
                .ToArray();

        [Fact]
        private void GettingSampleInput()
        {
            var result = LoadInput("sampleinput.txt");

            result.Should().Equal(3, 1, 5, 4, 2);
        }

        [Fact]
        private void GettingProblemInput()
        {
            var result = LoadInput("probleminput.txt");

            result.Should().HaveCount(100_000);
        }

        private static string _currentDirectoryPath;

        private static string CurrentDirectoryPath 
        {
            get
            {
                if(_currentDirectoryPath == null)
                {
                    var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                    var path = Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
                    _currentDirectoryPath = Path.GetDirectoryName(path);

                }
                return _currentDirectoryPath;
            }
        }

        [Theory]
        [InlineData("sampleinput.txt")]
        [InlineData("probleminput.txt")]
        private void LoadingSampleInputTests(string fileName)
        {
            var filePath = Path.Combine(CurrentDirectoryPath, fileName);

            File.Exists(filePath).Should().BeTrue();
        }
    }
}
