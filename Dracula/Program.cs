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
            var dateToText = new Dictionary<string, List<string>>();
            using (var sr = new StreamReader(@"C:\Users\Richard\Documents\Visual Studio 2013\Projects\Dracula\Dracula\dracula.htm"))
            {
                string line = "";
                var value = new StringBuilder();
                string key = "";
                string lastValidDate = "";
                string lastMedium = "";
                string lastCharacter = "";

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
                        Console.WriteLine(lastValidDate + " " + lastMedium);
                        if (value.Length == 0)
                        {
                            value.Clear();
                        }
                    }
                    //------------------END OF DATE IF STATEMENT ======= BEGIN OF TYPE SWITCH ----------------------------------------
                    else if (line.Contains("</small></h2>") || line.Contains("\"letra\""))
                    {
                        var formMedSwitch = Regex.Replace(line, @"<[^>]*>", "");

                        //TODO NEED TO FIGURE OUT HOW TO DEAL WITH TO/FROM
                        if (formMedSwitch.Contains("Journal") || formMedSwitch.Contains("JOURNAL"))
                        {
                            lastMedium = "Journal";
                        }
                        if (formMedSwitch.Contains("DIARY") || formMedSwitch.Contains("Diary"))
                        {
                            lastMedium = "Diary";
                        }
                        if (formMedSwitch.Contains("LETTER") || formMedSwitch.Contains("Letter"))
                        {
                            lastMedium = "Letter";
                        }
                        if (formMedSwitch.Contains("MEMORANDUM") || formMedSwitch.Contains("Memorandum"))
                        {
                            lastMedium = "Memorandum";
                        }
                        if (formMedSwitch.Contains("Report"))
                        {
                            lastMedium = "Report";
                        }
                        if (formMedSwitch.Contains("Gazette")) //TODO NEED TO IDENTIFY WHICH GAZETTE!
                        {
                            lastMedium = "News Paper Cuttings";
                        }
                        if (formMedSwitch.Contains("Telegram"))
                        {
                            lastMedium = "Telegram";
                        }
                        if (formMedSwitch.Contains("Interview"))
                        {
                            lastMedium = "Interview";
                        }
                        if (formMedSwitch.Contains("Note"))
                        {
                            lastMedium = "Note";
                        }
                        Console.WriteLine(lastValidDate + " " + lastMedium);
                    }

                    {
                        value.AppendLine(line);
                    }
                }

            }
            Console.ReadLine();
        }


    }
}
