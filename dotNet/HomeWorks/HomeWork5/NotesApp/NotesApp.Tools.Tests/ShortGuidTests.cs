using FluentAssertions;
using System;
using Xunit;

namespace NotesApp.Tools.Tests
{
    public class ShortGuidTests
    {
        [Fact]
        public void ToShortIdFromShortId_GuidToStringToGuid_InitialAndConvertedGuidsAreEqual()
        {
            //arrange
            Guid testGuid = Guid.NewGuid();

            //act
            string f_result = testGuid.ToShortId();
            Guid? s_result = f_result.FromShortId();

            //assert
            s_result.Should().Be(testGuid);        
        }

        [Fact]
        public void ToShortIdFromShortId_ToShortIdResultPlusTail_InitialAndConvertedGuidsAreEqual()
        {
            //arrange
            Guid testGuid = Guid.NewGuid();

            //act
            string f_result = testGuid.ToShortId();
            f_result += "==";
            Guid? s_result = f_result.FromShortId();

            //assert
            s_result.Should().Be(testGuid);
        }

        [Fact]
        public void FromShortId_GuidConvertedToString_InitialAndResultingGuidsAreEqual()
        {
            //arrange
            Guid testGuid = Guid.NewGuid();
            string guidConvertedToString = testGuid.ToString();

            //act
            Guid? result = guidConvertedToString.FromShortId();

            //assert
            result.Should().Be(testGuid);
        }

        [Theory]
        [InlineData("ifdsfdsfdsgsdgfdg")]
        [InlineData("k0z41gC/RWqk6wAAT+")]
        [InlineData("1234567890")]
        [InlineData("")]
        public void FromShortId_InvalidShortGuid_ThrowsFormatException(string invalidShortGuid)
        {
            //arrange

            //act
            Action act = () => invalidShortGuid.FromShortId();

            //assert
            act.Should().ThrowExactly<FormatException>().WithMessage("Given string is not a GUID or short base64 GUID");
        }

        [Fact]
        public void FromShortId_ArgumentIsNull_ReturnsNull()
        {
            //arrange
            string testShortGuid = null;

            //act
            Guid? result = testShortGuid.FromShortId();

            //assert
            result.Should().NotHaveValue();
        }

    }
}
