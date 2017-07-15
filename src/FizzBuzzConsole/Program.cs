using System;
using FizzBuzz;

namespace FizzBuzzConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fizzBuzzer = new FizzBuzzGenerator();

            foreach (var value in fizzBuzzer.GetNumbers(1, Int32.MaxValue))
            {
                Console.WriteLine(value);
            }

            Console.ReadLine();
        }
    }
}