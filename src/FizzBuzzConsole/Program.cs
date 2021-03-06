﻿using System;
using FizzBuzz.Features.CustomGenerator;
using FizzBuzz.Features.LegacyGenerator;

namespace FizzBuzzConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //// RunLegacyGenerator();
            RunWordGenerator();
        }

        private static void RunWordGenerator()
        {
            var wordGenerator = new WordGenerator(
                new CompositeWordStrategy(
                    new[]
                    {
                        new NumberToWordDivisibleStrategy(3, "fizz"),
                        new NumberToWordDivisibleStrategy(5, "buzz")
                    }));

            foreach (var value in wordGenerator.GetNumbers(1, Int32.MaxValue))
            {
                Console.WriteLine(value);
            }
        }

        private static void RunLegacyGenerator()
        {
            var fizzBuzzer = new FizzBuzzGenerator();

            foreach (var value in fizzBuzzer.GetNumbers(1, Int32.MaxValue))
            {
                Console.WriteLine(value);
            }
        }
    }
}