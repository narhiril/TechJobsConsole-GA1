﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechJobsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create two Dictionary vars to hold info for menu and data

            // Top-level menu options
            Dictionary<string, string> actionChoices = new Dictionary<string, string>();
            actionChoices.Add("search", "Search");
            actionChoices.Add("list", "List");

            // Column options
            Dictionary<string, string> columnChoices = new Dictionary<string, string>();
            columnChoices.Add("core competency", "Skill");
            columnChoices.Add("employer", "Employer");
            columnChoices.Add("location", "Location");
            columnChoices.Add("position type", "Position Type");
            columnChoices.Add("all", "All");

            Console.WriteLine("Welcome to LaunchCode's TechJobs App!");

            // Allow user to search/list until they manually quit with ctrl+c
            while (true)
            {

                string actionChoice = GetUserSelection("View Jobs", actionChoices);

                if (actionChoice.Equals("list"))
                {
                    string columnChoice = GetUserSelection("List", columnChoices);

                    if (columnChoice.Equals("all"))
                    {
                        PrintJobs(JobData.FindAll());
                    }
                    else
                    {
                        List<string> results = JobData.FindAll(columnChoice);

                        Console.WriteLine("\n*** All " + columnChoices[columnChoice] + " Values ***");
                        foreach (string item in results)
                        {
                            Console.WriteLine(item);
                        }
                    }
                }
                else // choice is "search"
                {
                    // How does the user want to search (e.g. by skill or employer)
                    string columnChoice = GetUserSelection("Search", columnChoices);

                    // What is their search term?
                    Console.Write("\nSearch term: ");
                    string searchTerm = Console.ReadLine();

                    List<Dictionary<string, string>> searchResults;

                    // Fetch results
                    if (columnChoice.Equals("all"))
                    {
                        searchResults = JobData.FindByValue(searchTerm);
                        PrintJobs(searchResults);
                    }
                    else
                    {
                        searchResults = JobData.FindByColumnAndValue(columnChoice, searchTerm);
                        PrintJobs(searchResults, columnChoice);
                    }
                }
            }
        }

        /*
         * Returns the key of the selected item from the choices Dictionary
         */
        private static string GetUserSelection(string choiceHeader, Dictionary<string, string> choices)
        {
            int choiceIdx = -1; //initialize to an invalid value
            bool isValidChoice = false;
            string[] choiceKeys = new string[choices.Count];

            int i = 0;
            foreach (KeyValuePair<string, string> choice in choices)
            {
                choiceKeys[i] = choice.Key;
                i++;
            }

            do
            {
                Console.WriteLine("\n" + choiceHeader + " by:");

                for (int j = 0; j < choiceKeys.Length; j++)
                {
                    Console.WriteLine(j + " - " + choices[choiceKeys[j]]);
                }

                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();
                try
                {
                    choiceIdx = int.Parse(input);
                }
                catch (FormatException) //prevents a crash on non-numeric input
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                    continue; //no point checking further
                }

                if (choiceIdx < 0 || choiceIdx >= choiceKeys.Length)
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
                else
                {
                    isValidChoice = true;
                }

            } while (!isValidChoice);

            return choiceKeys[choiceIdx];
        }

        /// <summary>
        /// Prints provided list of jobs without sorting.
        /// </summary>
        /// <param name="someJobs"></param>
        private static void PrintJobs(List<Dictionary<string, string>> someJobs)
        {
            if (someJobs.Count == 0)
            {
                Console.WriteLine("No results found.");
            }
            else
            {
                StringBuilder output = new StringBuilder();
                foreach(Dictionary<string, string> job in someJobs)
                {
                    output.AppendLine("*****");
                    foreach (KeyValuePair<string, string> infoField in job)
                    {
                        output.AppendLine($"{infoField.Key}: {infoField.Value}");
                    }
                }
                Console.WriteLine(output.ToString() + $"*****\n{someJobs.Count} matches found.");
            }
        }

        /// <summary>
        /// Sorts alphabetically within provided column before printing list of jobs
        /// </summary>
        /// <param name="someJobs"></param>
        /// <param name="sortColumn"></param>
        private static void PrintJobs(List<Dictionary<string, string>> someJobs, string sortColumn)
        {
            var sortedJobs = someJobs.OrderBy(job => job[sortColumn]).ToList();
            PrintJobs(sortedJobs);
        }
    }
}
