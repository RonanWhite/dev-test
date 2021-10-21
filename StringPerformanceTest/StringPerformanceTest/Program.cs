using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StringPerformanceTest
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        string[] Lines;

        public int NumberOfLines;

        [Params("Bacon", "pork", "prosciutto")]
        public string SearchValue;

        [Params("Files/Bacon10.txt", "Files/Bacon25.txt", "Files/Bacon50.txt")]
        public string FileToRead;

        [GlobalSetup]
        public void GlobalSetup()
        {
            string fileLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FileToRead);
            Lines = File.ReadAllLines(fileLocation);
            NumberOfLines = Lines.Count();
        }


        [Benchmark]
        public int CountOccurrences()
        {
            int totalCount = 0;
            int currentIndex = 0;

            for (int i = 0; i < NumberOfLines; i++)
            {
                // loops through all lines of the document
                currentIndex = 0;

                while ((currentIndex = Lines[i].IndexOf(SearchValue, currentIndex)) != -1) // find index of substring in line from previous index
                {
                    // found occurrence of substring in string
                    totalCount += 1;
                    currentIndex += 1; // increment so it doesnt find the same index next loop
                }

            }


            return totalCount;

            #region Other solutions
            /// Other slower solutions I tried
            /// Below uses regular expressions
            /*

            Regex regex = new Regex(SearchValue, RegexOptions.IgnoreCase);

            for (int i = 0; i < NumberOfLines; i++)
            {
                totalCount += regex.Matches(Lines[i]).Count;
            }
            */

            /// This method uses split
            /*
            for (int i = 0; i < NumberOfLines; i++)
            {
                // split returns an array 
                totalCount += Lines[i].Split(new string[] { SearchValue }, StringSplitOptions.None).Count() - 1;
            }
            */

            /// finds the word, and then removes it. compares original length of line with removed
            /// version and uses that to count how many occurrences of the word was in the line
            /*
            for (int i = 0; i < NumberOfLines; i++)
            {
                totalCount += (Lines[i].Length - Lines[i].Replace(SearchValue, "").Length) / SearchValue.Length;
            }
            */

            #endregion
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }
}
