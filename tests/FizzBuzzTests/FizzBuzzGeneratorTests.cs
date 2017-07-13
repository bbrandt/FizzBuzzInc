using System.Linq;
using FizzBuzz;
using FluentAssertions;
using NUnit.Framework;

namespace FizzBuzzTests
{
    [TestFixture]
    public class FizzBuzzGeneratorTests
    {
        [Test]
        public void GetNumbers_should_return_fizzbuzz_when_15()
        {
            var fizzBuzzGenerator = new FizzBuzzGenerator();

            fizzBuzzGenerator.GetNumbers(1, 15).ToArray().Should().EndWith("fizzbuzz");
        }

        [Test]
        public void GetNumbers_should_return_fizz_when_3()
        {
            var fizzBuzzGenerator = new FizzBuzzGenerator();

            fizzBuzzGenerator.GetNumbers(1, 3).ToArray().Should().EndWith("fizzbuzz");
        }


        // Add tests to ensure no regression 

        // Allow user to pass in as many number and word pairs as they like to replace the generated numbers

        // Add build script 

        // git repo

        // Configure CI 
    }
}