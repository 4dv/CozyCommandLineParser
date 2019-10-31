using System;
using System.Collections.Generic;
using System.Linq;
using CozyCommandLineParser.Attributes;

namespace SampleProject
{
    public class Commands
    {
        [Option]
        public bool Verbose { get; set; }


        [Command("sum", Description = "Calc sum of numbers from input")]
        public double CalcSum(params double[] nums)
        {
            VerbosePrint($"Calc sum of provided {nums.Length} numbers");
            return nums.Sum();
        }

        [Command("cumsum", Description = "Cumulative sum (running total) of numbers")]
        public IEnumerable<double> CumSum(params double[] nums)
        {
            VerbosePrint($"Calc cumulative sum of provided {nums.Length} numbers");
            double sum = 0;
            return nums.Select(v => sum += v);
        }

        private void VerbosePrint(string message)
        {
            if(Verbose) Console.WriteLine(message);
        }
    }
}