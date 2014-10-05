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
                        
                        Console.WriteLine(lastValidDate + " " + lastMedium);
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
                return "News Paper Cuttings";
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
    }
}
