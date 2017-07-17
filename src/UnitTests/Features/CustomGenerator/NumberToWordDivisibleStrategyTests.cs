using System;
using FizzBuzz.Features.CustomGenerator;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTests.Features.CustomGenerator
{
    [TestFixture]
    public class NumberToWordDivisibleStrategyTests
    {
        [Test]
        public void Process_should_support_negative()
        {
            var sut = new NumberToWordDivisibleStrategy(-2, "foo");

            sut.Process(new WordModel(4)).ToString().Should().Be("foo");
        }

        [Test]
        public void Process_should_throw_for_zero()
        {
            Action act = () => new NumberToWordDivisibleStrategy(0, "bar");

            act.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}