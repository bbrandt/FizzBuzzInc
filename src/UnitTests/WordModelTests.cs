using FizzBuzz;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    internal class WordModelTests
    {
        [Test]
        public void ToString_should_output_number()
        {
            var sut = new WordModel(42);

            sut.ToString().Should().Be("42");
        }

        [Test]
        public void ToString_should_output_single_word()
        {
            var sut = new WordModel(345);

            sut = sut.Add("foo");

            sut.ToString().Should().Be("foo");
        }

        [Test]
        public void ToString_should_output_two_word()
        {
            var sut = new WordModel(345);

            sut = sut.Add("foo").Add("bar");

            sut.ToString().Should().Be("foobar");
        }
    }
}