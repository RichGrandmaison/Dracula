using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dracula
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 0;
            int counter2 = 0;

            using (var sr = new StreamReader(@"../dracula.htm"))
            {
                string line = "";
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
                        else
                        {
                            formattedDate = lastValidDate;
                        }
                        Console.WriteLine("+" + lastValidDate + " " + lastMedium);
                        counter2++;
                        if (value.Length == 0)
                        {
                            value.Clear();
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
                        counter++;
                    }

                    {
                        value.AppendLine(line);
                    }
                }
            }

            Console.WriteLine(counter);
            Console.WriteLine(counter2);
            Console.ReadLine();        
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
