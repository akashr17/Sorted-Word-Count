using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Diagnostics;


namespace Lab3Q1
{
    public class HelperFunctions
    {
        /**
         * Counts number of words, separated by spaces, in a line.
         * @param line string in which to count words
         * @param start_idx starting index to search for words
         * @return number of words in the line
         */
        public static int WordCount(ref string line, int start_idx)
        {
            // YOUR IMPLEMENTATION HERE
            int count = 0;
            try
            {
                // throw exceptions for starting index errors
                if (start_idx > line.Length)
                {
                    throw new Exception("Starting index out of range of line");
                }
                if (start_idx < 0)
                {
                    throw new Exception("Starting index is negative");
                }

                // create the testing string by substringing based on the index
                string testing_line = line.Substring(start_idx);

                // split the string based on spaces and count the number of words - the spaces
                string[] words = testing_line.Split(' ');
                count = words.Length;
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i] == "")
                    {
                        count--;
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return count;

        }


        /**
        * Reads a file to count the number of words each actor speaks.
        *
        * @param filename file to open
        * @param mutex mutex for protected access to the shared wcounts map
        * @param wcounts a shared map from character -> word count
        */
        public static void CountCharacterWords(string filename,
                                 Mutex mutex,
                                 Dictionary<string, int> wcounts)
        {

            //===============================================
            //  IMPLEMENT THIS METHOD INCLUDING THREAD SAFETY
            //===============================================

            string line;  // for storing each line read from the file
            string character = "";  // empty character to start

            System.IO.StreamReader file = new System.IO.StreamReader(filename);

            while ((line = file.ReadLine()) != null)
            {
                //=================================================
                // YOUR JOB TO ADD WORD COUNT INFORMATION TO MAP
                //=================================================

                // check if dialogue line and check the starting index
                int startingIdx = IsDialogueLine(line, ref character);
                if (startingIdx != -1)
                {
                    // Add in the mutex wait to begin blocking the additition to Dictionary section
                    mutex.WaitOne();

                    // based on starting index and character, check wordcount and if dict countains a character,
                    // increase the wordcount otherwise create new character and add
                    if ((startingIdx > 0) && (character != ""))
                    {

                        int wordcount = WordCount(ref line, startingIdx);

                        if (wcounts.ContainsKey(character))
                        {
                            wcounts[character] += wordcount;
                        }
                        else
                        {
                            wcounts.Add(character, wordcount);
                        }
                    }
                    // release the mutex since we are done handling data sensitive section
                    mutex.ReleaseMutex();

                }

                // Is the line a dialogueLine?
                //    If yes, get the index and the character name.
                //      if index > 0 and character not empty
                //        get the word counts
                //          if the key exists, update the word counts
                //          else add a new key-value to the dictionary
                //    reset the character

            }

            file.Close();

            // Close the file
        }



        /**
         * Checks if the line specifies a character's dialogue, returning
         * the index of the start of the dialogue.  If the
         * line specifies a new character is speaking, then extracts the
         * character's name.
         *
         * Assumptions: (doesn't have to be perfect)
         *     Line that starts with exactly two spaces has
         *       CHARACTER. <dialogue>
         *     Line that starts with exactly four spaces
         *       continues the dialogue of previous character
         *
         * @param line line to check
         * @param character extracted character name if new character,
         *        otherwise leaves character unmodified
         * @return index of start of dialogue if a dialogue line,
         *      -1 if not a dialogue line
         */
        static int IsDialogueLine(string line, ref string character)
        {

            // new character
            if (line.Length >= 3 && line[0] == ' '
                && line[1] == ' ' && line[2] != ' ')
            {
                // extract character name

                int start_idx = 2;
                int end_idx = 3;
                while (end_idx <= line.Length && line[end_idx - 1] != '.')
                {
                    ++end_idx;
                }

                // no name found
                if (end_idx >= line.Length)
                {
                    return 0;
                }

                // extract character's name
                character = line.Substring(start_idx, end_idx - start_idx - 1);
                return end_idx;
            }

            // previous character
            if (line.Length >= 5 && line[0] == ' '
                && line[1] == ' ' && line[2] == ' '
                && line[3] == ' ' && line[4] != ' ')
            {
                // continuation
                return 4;
            }

            return 0;
        }

        /**
         * Sorts characters in descending order by word count
         *
         * @param wcounts a map of character -> word count
         * @return sorted vector of {character, word count} pairs
         */
        public static List<Tuple<int, string>> SortCharactersByWordcount(Dictionary<string, int> wordcount)
        {

            // Implement sorting by word count here
            List<Tuple<int, string>> sortedByValueList = new List<Tuple<int, string>>();

            // for each character in the dictionary(which is ordered by descending", add to the list
            foreach (KeyValuePair<string, int> character in wordcount.OrderByDescending(key => key.Value))
            {
                sortedByValueList.Add(new Tuple<int, string>(character.Value, character.Key));
            }
            return sortedByValueList;
        }


        /**
         * Prints the List of Tuple<int, string>
         *
         * @param sortedList
         * @return Nothing
         */
        public static void PrintListofTuples(List<Tuple<int, string>> sortedList)
        {
            foreach (Tuple<int, string> item in sortedList)
            {
                Console.WriteLine("Words = {0}, Character = {1}", item.Item1, item.Item2);

            }
            // Implement printing here

        }

        // Created new method for checking the single thread and multi thread lists
        public static List<Tuple<int,string>> checkResuls(List<Tuple<int, string>> singleList, List<Tuple<int, string>> multiList)
        {
            List<Tuple<int, string>> mistakes = new List<Tuple<int,string>>();
            bool exists;

            // for the items in the single list, check if it exists in the multi list 
            foreach (var item in singleList)
            {
                exists = (multiList.Exists(x => x.Item1 == item.Item1) && multiList.Exists(x => x.Item2 == item.Item2));

                // if it doesnt exist in the second list add to the mistakes list
                if (!exists)
                {
                    mistakes.Add(item);
                }

            }
            return mistakes;
        }
    }
}
