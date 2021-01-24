using System;
using Xunit;
using Moq;
using FluentAssertions;

namespace NotesApp.Tools.Tests
{
    public class NumberGeneratorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(19)]
        [InlineData(20)]
        public void GeneratePositiveLong_InvalidArgument_ThrowsArgumentOutOfRangeException(int testArgument)
        {
            //arrange

            //act
            Action act = () => NumberGenerator.GeneratePositiveLong(testArgument);

            //assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().WithMessage("Length must be positive value (Parameter 'length')");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(18)]
        public void GeneratePositiveLong_ValidArgument_ReturnsANumberWithAGivenNumberOfDigits(int testArgument)
        {
            //arrange

            //act
            long result = NumberGenerator.GeneratePositiveLong(testArgument);

            //assert
            result.ToString().Should().HaveLength(testArgument);
        }
    }
}
