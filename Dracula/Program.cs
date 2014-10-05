using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Dracula
{
    class Program
    {
        static void Main()
        {
            int counter = 0;
            int counter2 = 0;
            var names = new List<string>();
            var dates = new List<string>();

            var dateToText = new Dictionary<string, List<StringBuilder>>();

            using (var sr = new StreamReader(@"../dracula.htm"))
            {
                string line;
                var value = new StringBuilder();
                string lastValidDate = "";
                string lastMedium = "";
                string lastAuthor = "";

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();

                    if (line.StartsWith("<p><i>"))
                    {
                        string formattedDate = Regex.Replace(line, "</i>.*$", "");
                        formattedDate = Regex.Replace(formattedDate, "^<p><i>", "");
                        if (Regex.IsMatch(formattedDate, @"\.[A-Za-z\s]*\.$"))
                        {
                            string formattedDate2 = Regex.Replace(formattedDate, @"\.[A-Za-z\s]*\.$", "");
                            formattedDate = formattedDate2 + ".";
                        }

                        if (Regex.IsMatch(formattedDate, @"^\d+\s[A-Z][a-z]+\."))
                        {
                            lastValidDate = formattedDate;
                        }

                        dates.Add("+" + lastValidDate + " " + lastMedium);
                        counter2++;

                        if (value.Length != 0 )
                        {
                            //Add to data.
                            value = new StringBuilder();
                        }

                    }
                    //------------------END OF DATE IF STATEMENT ======= BEGIN OF TYPE SWITCH ----------------------------------------
                    else if (line.Contains("</small></h2>") || line.Contains("\"letra\""))
                    {
                        var nameWithMedium = Regex.Replace(line, @"<[^>]*>", "");

                        //TODO NEED TO FIGURE OUT HOW TO DEAL WITH TO/FROM
                        
                        lastMedium = FindMedium(nameWithMedium);
                        lastAuthor = FindAuthor(nameWithMedium);
                        
                        Console.WriteLine(lastValidDate + " " + lastMedium + " " + lastAuthor);

                        var lastValidName = Regex.Replace(nameWithMedium, lastMedium, "");
                        
                        names.Add(lastValidName);

                        Console.WriteLine(lastValidDate + " " + lastMedium);
                        counter++;

                        if (value.Length != 0)
                        {
                            //Add to data
                            if (dateToText.ContainsKey(lastValidName))
                            {
                                dateToText[lastValidName].Add(value);
                            }
                            else
                            {
                                var valueList = new List<StringBuilder>();
                                valueList.Add(value);
                                dateToText.Add(lastValidName, valueList);
                            }
                            value = new StringBuilder();
                        }
                    }
                    else   //Text Data. 
                    {
                        value.AppendLine(line);
                    }
                }
            }

            Console.WriteLine(counter);
            Console.WriteLine(counter2);

            Console.ReadKey();        
        }

        private static string FindMedium(string nameWithMedium)
        {
            if (nameWithMedium.ToLower().Contains("journal"))
            {
                return "journal";
            }

            if (nameWithMedium.ToLower().Contains("diary") )
            {
                if (nameWithMedium.ToLower().Contains("phonograph"))
                {
                    return "Phonograph Diary";
                }
                return "Diary";
            }
            if (nameWithMedium.ToLower().Contains("letter"))
            {
                return "Letter";
            }
            if (nameWithMedium.ToLower().Contains("memorandum"))
            {
                return "Memorandum";
            }
            if (nameWithMedium.ToLower().Contains("report"))
            {
                return "Report";
            }
            if (nameWithMedium.ToLower().Contains("gazette")) //TODO NEED TO IDENTIFY WHICH GAZETTE!
            {
                if (nameWithMedium.ToLower().Contains("pall mall"))
                    return "The Pall Mall Gazette";
               return "The Westminster Gazette";
            }
            if (nameWithMedium.ToLower().Contains("telegram"))
            {
                return "Telegram";
            }
            if (nameWithMedium.ToLower().Contains("interview"))
            {
                return "Interview";
            }
            if (nameWithMedium.ToLower().Contains("note"))
            {
                return "Note";
            }

            return "";
        }
        private static string FindAuthor(string nameWithMedium)
        {
            if (nameWithMedium.ToLower().Contains("jonathan harker"))
            {
                return "jonathan harker";
            }
            if (nameWithMedium.ToLower().Contains("Seward"))
            {
                return "dr. seward";
            }
            if (nameWithMedium.ToLower().Contains("Lucy Westerna"))
            {
                return "lucy westerna";
            }
            if (nameWithMedium.ToLower().Contains("Quincey P. Morris"))
            {
                return "quincey p. morris";
            }
            if (nameWithMedium.ToLower().Contains("Arthur Holmwood"))
            {
                return "arthur holmwood";
            }
            if (nameWithMedium.ToLower().Contains("Mina Harker"))
            {
                return "mina harker";
            }
            if (nameWithMedium.ToLower().Contains("Mitchell"))
            {
                return "mitchell, sons and candy";
            }
            if (nameWithMedium.ToLower().Contains("Helsing"))
            {
                return "abraham van helsing";
            }
            if (nameWithMedium.ToLower().Contains("Godalming"))
            {
                return "lord godalming";
            }
            if (nameWithMedium.ToLower().Contains("Patrick Hennessey"))
            {
                return "patrick hennessey";
            }
            return "";        
        }
    }
}
