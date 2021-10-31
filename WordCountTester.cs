using System;
using System.Collections.Generic;
namespace Lab3Q1
{
    public class WordCountTester
    {
        static int Main()
        {
            try
            {
                //=================================================
                // Implement your tests here. Check all the edge case scenarios.
                // Create a large list which iterates over WCTester
                //=================================================

                // list of tests which includes just a simple test, tests with double spaces at the beginning/middle/end,
                // tests which start at index 1, index longer than string length, negative index, index after first word, and index at last word
                string[] test_lines = { "Simple test", "test double  spaces", "  this has extra starting space", " extra end space  ", "index starts 1", "", "index longer than string", "index skips first word", "index at end word", "index negative" };
                int[] expected_list = { 2, 3, 5, 3, 3, 0, 0, 3, 1, 0};
                int[] index = { 0, 0, 0, 0, 1, 0, 25, 5, 15 , -1};

                // loop through the tests
                for (int i=0; i < test_lines.Length; i++)
                {
                    WCTester(test_lines[i], index[i], expected_list[i]);
                }
                     

            }
            catch (UnitTestException e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Ran through all of the tests");
            Console.WriteLine("Hit Enter to end:");
            Console.ReadLine();
            return 0;
        }


        /**
         * Tests word_count for the given line and starting index
         * @param line line in which to search for words
         * @param start_idx starting index in line to search for words
         * @param expected expected answer
         * @throws UnitTestException if the test fails
         */
        static void WCTester(string line, int start_idx, int expected)
        {

            //=================================================
            // Implement: comparison between the expected and
            // the actual word counter results
            //=================================================
            int result = 0;
            result = HelperFunctions.WordCount(ref line, start_idx);
            
            if (result != expected)
            {
                throw new Lab3Q1.UnitTestException(ref line, start_idx, result, expected, String.Format("UnitTestFailed: result:{0} expected:{1}, line: {2} starting from index {3}", result, expected, line, start_idx));
            }

        }
    }
}
