using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace Lab3Q1
{
    class Program
    {
        static void Main(string[] args)
        {
            // map and mutex for thread safety
            Mutex mutex = new Mutex();
            
            // Dictionaries for single and multi threads
            Dictionary<string, int> wcountsSingleThread = new Dictionary<string, int>();
            Dictionary<string, int> wcountsMultiThread = new Dictionary<string, int>();

            var filenames = new List<string> {
                "../../data/shakespeare_antony_cleopatra.txt",
                "../../data/shakespeare_hamlet.txt",
                "../../data/shakespeare_julius_caesar.txt",
                "../../data/shakespeare_king_lear.txt",
                "../../data/shakespeare_macbeth.txt",
                "../../data/shakespeare_merchant_of_venice.txt",
                "../../data/shakespeare_midsummer_nights_dream.txt",
                "../../data/shakespeare_much_ado.txt",
                "../../data/shakespeare_othello.txt",
                "../../data/shakespeare_romeo_and_juliet.txt",
            };

            /* wcountsSingleThread.Add("Rob", 10);
             wcountsSingleThread.Add("Bob", 12);
             wcountsSingleThread.Add("Tob", 16);
             List<Tuple<int, string>> sortedByValueList = HelperFunctions.SortCharactersByWordcount(wcountsSingleThread);*/

            //=============================================================
            // YOUR IMPLEMENTATION HERE TO COUNT WORDS IN SINGLE THREAD
            //=============================================================
            // Start stopwatch
            Stopwatch s1 = new Stopwatch();
            s1.Start();

            // for each file run the count characters words
            foreach (var file in filenames)
            {
                HelperFunctions.CountCharacterWords(file, mutex, wcountsSingleThread);
            }
            s1.Stop();

            // turn the dictionary into a sorted list and print the list
            List<Tuple<int, string>> sortedByValueList = HelperFunctions.SortCharactersByWordcount(wcountsSingleThread);
            HelperFunctions.PrintListofTuples(sortedByValueList);

            Console.WriteLine("SingleThread is  Done!");

            //=============================================================
            // YOUR IMPLEMENTATION HERE TO COUNT WORDS IN MULTIPLE THREADS
            //=============================================================
            
            List<Thread> Threads = new List<Thread>();
            int temp;

            // start the stopwatch for multi thread
            Stopwatch s2 = new Stopwatch();
            s2.Start();

            // for each file create a new thread and run the count characters method
            foreach (var file in filenames)
            {
                Thread newT = new Thread(() => HelperFunctions.CountCharacterWords(file, mutex, wcountsMultiThread));
                newT.Start();
                Threads.Add(newT);
            }

            // join all of the threads and stop the stopwatch
            foreach (Thread t in Threads)
            {
                t.Join();
            }
            s2.Stop();
           
            // convert the mulithread dictionary into a sorted list and print it
            List<Tuple<int, string>> sortedByValueListMulti = HelperFunctions.SortCharactersByWordcount(wcountsMultiThread);
            HelperFunctions.PrintListofTuples(sortedByValueListMulti);

            Console.WriteLine( "MultiThread is Done!");

            // Run the check results method to ensure both lists are the same, if different print the different entries from list 1
            List<Tuple<int, string>> inconsistencies = HelperFunctions.checkResuls(sortedByValueList, sortedByValueListMulti);
            if(inconsistencies.Count == 0)
            {
                Console.WriteLine("No inconsistencies between the lists");
            }
            else
            {
                Console.WriteLine("These items in List 1 did not matchup with List 2:");
                HelperFunctions.PrintListofTuples(inconsistencies);
            }


            Console.WriteLine("The time taken for the single thread is {0}", s1.Elapsed);
            Console.WriteLine("The time taken for the multi thread is {0}", s2.Elapsed);
            Console.WriteLine("Speed factor is {0}",(s1.Elapsed.TotalSeconds / s2.Elapsed.TotalSeconds));
            Console.WriteLine("Hit Enter to end:");
            Console.ReadLine();
            return;
        }
    }
}
