using System;
using System.Collections.Generic;
using System.Linq;

namespace EratoWPF
{
    public class PrimeNumbers
    {
        private int processEnd;
        public int ProcessEnd
        {
            get
            {
                return processEnd;
            }
        }
        private int rangeStart;
        private int rangeEnd;
        public List<int> numbers = new List<int>();
        //definintion of a predicate which will be used to remove all uint items from the list which are out of range
        //predicate returns a boolean value dependant on whether the condition is satisfied or not
        private Predicate<int> SmallerThanRangeStart;

        public int RangeStart
        {
            get
            {
                return rangeStart;
            }

            set
            {
                if(value < 2)
                {
                    rangeStart = 2;
                }
                else
                {
                        rangeStart = value;
                }
            }
        }

        public int RangeEnd
        {
            get
            {
                return rangeEnd;
            }

            set
            {
                if(value < 2)
                {
                    rangeEnd = 2;
                }
                else
                {
                    rangeEnd = value;
                }
            }
        }

        public PrimeNumbers(int startOfRange, int endOfRange)
        {
            RangeStart = startOfRange;
            RangeEnd = endOfRange;
            if(RangeStart > RangeEnd)
            {
                int buffer = RangeEnd;
                RangeEnd = RangeStart;
                RangeStart = buffer;
            }
            else
            {
                if(RangeStart == RangeEnd)
                {
                    RangeEnd++;
                }
            }
            FillList();
        }

        public PrimeNumbers(int endOfRange)
        {
            RangeStart = 2;
            RangeEnd = endOfRange;
            if(RangeStart > RangeEnd)
            {
                int buffer = RangeEnd;
                RangeEnd = RangeStart;
                RangeStart = buffer;
            }
            else
            {
                if(RangeStart == RangeEnd)
                {
                    RangeEnd++;
                }
            }
            FillList();
        }

        private void FillList()
        {
            //filling the list of potential numbers starting from 2 up to rangeEnd (including)
            //the condition above is due to requirements of the algorithm
            //the last step of this implementation will be to remove all items in the list which are lesser than rangeEnd
            for(int i = 2; i <= rangeEnd; i++)
            {
                numbers.Add(i);
            }
            processEnd = (int)Math.Sqrt(numbers.Max());
        }

        public List<int> Sieve(CallBackProgress callbackProgress)
        {
            //this algorithm is based on Eratosthenes' Sieve
            //for more details please see https://en.wikipedia.org/wiki/Sieve_of_Eratosthenes
            int currentMinIndex = 0;
            int currentMin = numbers[0];
            int overallMax = numbers.Max();
            //Console.WriteLine(currentMin);
            //Console.WriteLine(overallMax);
            //Console.WriteLine(Math.Sqrt(overallMax));
            //Console.WriteLine(currentMin < Math.Sqrt(overallMax));
            while(currentMin < Math.Sqrt(overallMax))
            {
                //Console.WriteLine("checking min value");
                currentMin = numbers[currentMinIndex];
                int iteration = 2;
                int numbersCount = numbers.Count();
                while(iteration < numbersCount)
                {
                    //Console.WriteLine("checkin multiplication");
                    if(numbers.Contains(iteration * currentMin))
                    {
                        numbers.Remove(iteration * currentMin);
                    }
                    iteration++;
                }
                callbackProgress(currentMin);
                currentMinIndex++;
            }
            //instantiating of a predicate using Lambda expression
            // check value (type uint according to predicate definition) - argument
            // this particular predicate checks if the argument is smaller than rangeStart variable
            // general use of lambda expressions:
            // '=>' divides an expression between input and output, can be understood as "what's before => becomes what's after =>"
            // in this case it is slighty different as Predicate<T> returns boolean value
            // '=>' divides input value and condition to check
            SmallerThanRangeStart = checkValue => checkValue < rangeStart;
            //removing all items which fall out of given range
            numbers.RemoveAll(SmallerThanRangeStart);
            return numbers;
        }

        //delegate to give feedback about total progress
        public delegate void CallBackProgress(int callbackValue);
    }
}
