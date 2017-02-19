using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EratoWPF;
using System.Diagnostics;

namespace PrimeNumbersSieveTest
{
    class Program
    {
        static uint rangeStart, rangeEnd;
        static Stopwatch timer;
        static void Main(string[] args)
        {

            //header
            Console.WriteLine("******************************************");
            Console.WriteLine("* PrimeNumbers Class Sieve Method Tester *");
            Console.WriteLine("******************************************\n");

            //range boundries input that will be passed to PrimeNumbers class constructor
            Console.WriteLine("Start of range [type uint 0..4294967295]: ");
            try
            {
                rangeStart = uint.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Error reading uint.\nStart of your range set to default!");
                rangeStart = 0;
            }
            Console.WriteLine("End of range [type uint 0..4294967295]: ");
            try
            {
                rangeEnd = uint.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Error reading uint.\nEnd of your range set to default!");
                rangeStart = 0;
            }

            //creating an instance of PrimeNumbers class with given boundries
            PrimeNumbers testInstance = new PrimeNumbers(rangeStart, rangeEnd);
            Console.WriteLine("Test instance created.\nStart of range: {0}\nEnd of range: {1}",testInstance.RangeStart,testInstance.RangeEnd);
            timer = new Stopwatch();
            //running Sieve of Eratosthenes alogorithm
            timer.Start();
            testInstance.Sieve(WriteProcessed,WriteEleminated);
            timer.Stop();
            //display results
            Console.WriteLine("QTY of prime numbers found: {0}",testInstance.numbers.Count());
            Console.WriteLine("Time elapsed: {0} ms", timer.ElapsedMilliseconds);
            Console.WriteLine("Ticks elapsed: {0}", timer.ElapsedTicks);
            Console.WriteLine("Prime numbers found: ");
            foreach(uint i in testInstance.numbers)
            {
                Console.Write("{0}, ", i);
            }
            Console.Write("\b\b \b\nHit any key to quit...");
            Console.ReadKey();
        }

        private static void WriteEleminated(uint _a)
        {
            Console.WriteLine("Eleminated: {0}", _a);
        }

        private static void WriteProcessed(uint _a)
        {
            Console.WriteLine("Processed: {0}", _a);
        }
    }
}
