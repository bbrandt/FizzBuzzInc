using System;
using FizzBuzz;

namespace ConsoleApp1
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