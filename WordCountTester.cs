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
                string[] test_lines = { "Simple test", "test double  spaces", "test word that ends with.", "  this has extra starting space", " extra end space  ", "index starts 1", "" };
                int[] expected_list = { 2, 3, 5, 5, 3, 3, 0 };
                for (int i =0; i < test_lines.Length; i++)
                {
                    string line = test_lines[i];
                    int expectedResults = expected_list[i];
                    int startIdx = 0;
                    if (i > 4)
                    {
                        startIdx = 1;
                    }
                    
                    WCTester(line, startIdx, expectedResults);
                }
                     

            }
            catch (UnitTestException e)
            {
                Console.WriteLine(e);
            }
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
