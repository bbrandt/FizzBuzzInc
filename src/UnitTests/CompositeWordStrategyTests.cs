using FizzBuzz;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace UnitTests
{
    [TestFixture]
    internal class CompositeWordStrategy_when_empty
    {
        [Test]
        public void ToString_should_pass_through_value()
        {
            var sut = new CompositeWordStrategy(Enumerable.Empty<IWordStrategy>());

            var actual = sut.Process(new WordModel(5));

            actual.ToString().Should().Be("5");
        }
    }
}