using FizzBuzz;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class WordGeneratorTests
    {
        [Test]
        public void GetNumber_should_support_customizable_word()
        {
            var sut = new WordGenerator(
                new CompositeWordStrategy(
                    new[]
                    {
                        new NumberToWordDivisibleStrategy(4, "foo"),
                        new NumberToWordDivisibleStrategy(7, "bar"),
                        new NumberToWordDivisibleStrategy(13, "baz"),
                        new NumberToWordDivisibleStrategy(23, "qux")
                    }));

            sut.GetNumbers(3, 25).ShouldBeEquivalentTo(new[]
            {
                "3", "foo", "5", "6", "bar", "foo", "9", "10", "11", "foo", "baz", "bar", "15", "foo", "17", "18", "19",
                "foo", "bar", "22", "qux", "foo", "25"
            });
        }

        [Test]
        public void GetNumber_should_support_multiple_combinations()
        {
            var sut = new WordGenerator(
                new CompositeWordStrategy(
                    new[]
                    {
                        new NumberToWordDivisibleStrategy(2, "foo"),
                        new NumberToWordDivisibleStrategy(4, "bar"),
                        new NumberToWordDivisibleStrategy(8, "baz")
                    }));

            sut.GetNumbers(7, 9).ShouldBeEquivalentTo(new[] { "7", "foobarbaz", "9" });
        }

        [Test]
        public void GetNumber_should_support_duplicates()
        {
            var sut = new WordGenerator(
                new CompositeWordStrategy(
                    new[]
                    {
                        new NumberToWordDivisibleStrategy(3, "foo"),
                        new NumberToWordDivisibleStrategy(3, "bar"),
                        new NumberToWordDivisibleStrategy(3, "baz"),
                        new NumberToWordDivisibleStrategy(3, "qux")
                    }));

            sut.GetNumbers(2, 3).ShouldBeEquivalentTo(new[] { "2", "foobarbazqux" });
        }
    }
}