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
            
            foreach (var file in filenames)
            {
                HelperFunctions.CountCharacterWords(file, mutex, wcountsSingleThread);
            }
            List<Tuple<int, string>> sortedByValueList = HelperFunctions.SortCharactersByWordcount(wcountsSingleThread);
            HelperFunctions.PrintListofTuples(sortedByValueList);

            Console.WriteLine( "SingleThread is  Done!");
            //=============================================================
            // YOUR IMPLEMENTATION HERE TO COUNT WORDS IN MULTIPLE THREADS
            //=============================================================

            List<Thread> Threads = new List<Thread>();
            int temp;
            foreach (var file in filenames)
            {
                Thread newT = new Thread(() => HelperFunctions.CountCharacterWords(file, mutex, wcountsMultiThread));
                newT.Start();
                Threads.Add(newT);
            }

            foreach (Thread t in Threads)
            {
                t.Join();
            }
           /* for (int i = 0; i < filenames.Count; i++)
            {
                temp = i;
                t[temp] = new Thread(() => HelperFunctions.CountCharacterWords(filenames[temp], mut1, wcountsMultiThread));
                t[temp].Start();
               
            }*/
           /* for (int i = 0; i < filenames.Count; i++)
            {
                t[i].Join();
            }*/


            List<Tuple<int, string>> sortedByValueListMulti = HelperFunctions.SortCharactersByWordcount(wcountsMultiThread);

            HelperFunctions.PrintListofTuples(sortedByValueListMulti);

            Console.WriteLine( "MultiThread is Done!");
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
            Console.WriteLine("Hit Enter to end:");
            Console.ReadLine();
           return;
        }
    }
}
