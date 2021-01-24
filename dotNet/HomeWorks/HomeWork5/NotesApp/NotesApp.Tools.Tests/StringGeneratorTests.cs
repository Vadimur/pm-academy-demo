using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NotesApp.Tools.Tests
{
    public class StringGeneratorTests
    {
        [Fact]
        public void GenerateNumbersString_ArgumentIsZero_ReturnsEmptyString()
        {
            //arrange
            int testArgument = 0;
            bool allowLeadingZero = false;

            //act
            string result = StringGenerator.GenerateNumbersString(testArgument, allowLeadingZero);

            //assert
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-10)]
        public void GenerateNumbersString_InvalidArgument_ThrowsArgumentOutOfRangeException(int testArgument)
        {
            //arrange
            bool allowLeadingZero = false;

            //act
            Action act = () => StringGenerator.GenerateNumbersString(testArgument, allowLeadingZero);

            //assert
            act.Should().ThrowExactly<ArgumentOutOfRangeException>().WithMessage("Length must be positive value (Parameter 'length')");
        }

        [Fact]
        public void GenerateNumbersString_AllowLeadingZeroIsFalse_ReturnsStringWithoutLeadingZero()
        {
            //arrange
            int testArgument = 5;
            bool allowLeadingZero = false;

            //act
            string result = StringGenerator.GenerateNumbersString(testArgument, allowLeadingZero);

            //assert
            result.Should().NotStartWith("0");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(17)]
        public void GenerateNumbersString_ValidArgument_ReturnsAStringWithAGivenNumberOfCharacters(int testArgument)
        {
            //arrange
            bool allowLeadingZero = false;

            //act
            string result = StringGenerator.GenerateNumbersString(testArgument, allowLeadingZero);

            //assert
            result.Should().HaveLength(testArgument);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(17)]
        public void GenerateNumbersString_ValidArgument_ReturnsANumbericStringWithAGivenNumberOfCharacters(int testArgument)
        {
            //arrange
            bool allowLeadingZero = false;

            //act
            string result = StringGenerator.GenerateNumbersString(testArgument, allowLeadingZero);

            //assert
            result.Should().HaveLength(testArgument);
            Assert.True(long.TryParse(result, out long testNumber));
        }
    }
}
