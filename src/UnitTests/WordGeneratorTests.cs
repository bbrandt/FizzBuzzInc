using System;
using System.Linq;
using FizzBuzz;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class WordGeneratorTests
    {
        IWordGenerator GetFizzBuzzWordGenerator()
        {
            return new WordGenerator(
                new CompositeWordStrategy(
                    new[]
                    {
                        new NumberToWordDivisibleStrategy(3, "fizz"),
                        new NumberToWordDivisibleStrategy(5, "buzz")
                    }));
        }

        [Test]
        public void GetNumbers_should_return_buzz_when_5()
        {
            var fizzBuzzGenerator = GetFizzBuzzWordGenerator();

            fizzBuzzGenerator.GetNumbers(4, 5).ToArray().Should().EndWith("buzz");
        }

        [Test]
        public void GetNumbers_should_return_fizz_when_3()
        {
            var fizzBuzzGenerator = GetFizzBuzzWordGenerator();

            fizzBuzzGenerator.GetNumbers(1, 3).ToArray().Should().EndWith("fizz");
        }

        [Test]
        public void GetNumbers_should_return_fizzbuzz_when_15()
        {
            var fizzBuzzGenerator = GetFizzBuzzWordGenerator();

            fizzBuzzGenerator.GetNumbers(1, 15).ToArray().Should().EndWith("fizzbuzz");
        }

        [Test]
        public void GetNumbers_should_support_single_number()
        {
            var fizzBuzzGenerator = GetFizzBuzzWordGenerator();

            fizzBuzzGenerator.GetNumbers(2, 2).ToArray().Should().EndWith("2");
        }

        [Test]
        public void GetNumbers_should_support_up_to_max_int()
        {
            var fizzBuzzGenerator = GetFizzBuzzWordGenerator();

            fizzBuzzGenerator.GetNumbers(int.MaxValue - 5, int.MaxValue).ToArray().Should()
                .StartWith((int.MaxValue - 5).ToString()).And.EndWith(int.MaxValue.ToString());
        }

        [Test]
        public void GetNumbers_should_throw_when_reversed()
        {
            var fizzBuzzGenerator = GetFizzBuzzWordGenerator();

            Action act = () => fizzBuzzGenerator.GetNumbers(57, 54).ToArray();

            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void GetNumbers_should_support_negative_numbers()
        {
            var fizzBuzzGenerator = GetFizzBuzzWordGenerator();

            fizzBuzzGenerator.GetNumbers(-3, -1).ToArray().Should()
                .StartWith("fizz").And.EndWith("-1");
        }
    }
}