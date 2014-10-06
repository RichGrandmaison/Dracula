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

            var textDataParsing = false;

            DBConnection db = new DBConnection();
            db.ConnectToDatabase();

            using (var sr = new StreamReader(@"../dracula.htm"))
            {
                string line;
                var value = new StringBuilder();
                string lastValidDate = "";
                string lastMedium = "";
                string lastAuthor = "";
                string lastReceiver = "";
                

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                 
                    if (line.StartsWith("<p><i>"))
                    {
                        if (textDataParsing) //Was parsing text
                        {
                            textDataParsing = false;
                            db.Add(value, lastAuthor, lastValidDate, lastMedium, lastReceiver);
                            value = new StringBuilder();
                        }

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

                        //Add Remainder of line to the Text Data
                        string remainder = Regex.Replace(line, "^.*</i>", "");

                        if (!String.IsNullOrEmpty(remainder))
                        {
                            string appendLine = Regex.Replace(line, "^.*</i>", "");
                            appendLine = Regex.Replace(appendLine, @"<[^>]*>", "");
                            value.AppendLine(appendLine);
                        }

                        dates.Add("+" + lastValidDate + " " + lastMedium);
                        counter2++;

                        Console.WriteLine(lastValidDate + " " + lastMedium + " " + lastAuthor + " " + lastReceiver);

                    }
                    //------------------END OF DATE IF STATEMENT ======= BEGIN OF TYPE SWITCH ----------------------------------------
                    else if (line.Contains("</small></h2>") || line.Contains("\"letra\""))
                    {
                        if (textDataParsing) //Was parsing text
                        {
                            textDataParsing = false;
                            db.Add(value, lastAuthor, lastValidDate, lastMedium, lastReceiver);
                            value = new StringBuilder();
                        }

                        var nameWithMedium = Regex.Replace(line, @"<[^>]*>", "");

                        //TODO NEED TO FIGURE OUT HOW TO DEAL WITH TO/FROM
                        
                        lastMedium = FindMedium(nameWithMedium);
                        //Check if lastMedium is a type which contains a Recipient)
                        bool hasRecipient = lastMedium == "Letter" || lastMedium == "Telegram" ||
                                            lastMedium == "Report" || lastMedium == "Note"
                                            || lastMedium == "Phonograph Diary";
                        if (hasRecipient)
                        {      
                            string[] separator = new string[] {"to"};
                            string[] senderReceiver = nameWithMedium.Split(separator, StringSplitOptions.None);
                            lastAuthor = FindAuthor(senderReceiver[0]);
                            if (senderReceiver.Length > 1)
                                lastReceiver = FindAuthor(senderReceiver[1]);
                        }
                        else
                        {
                            lastAuthor = FindAuthor(nameWithMedium);
                            lastReceiver = "";
                        }
                        
                        var lastValidName = Regex.Replace(nameWithMedium, lastMedium, "");
                        
                        names.Add(lastValidName);

                        Console.WriteLine(lastValidDate + " " + lastMedium + " " + lastAuthor + " " + lastReceiver);
                        counter++;
                    }
                    else   //Text Data. 
                    {
                        textDataParsing = true;
                        var appendLine = Regex.Replace(line, @"<[^>]*>", "");
                        value.AppendLine(appendLine);
                    }
                }
            }

            Console.WriteLine(counter);
            Console.WriteLine(counter2);

            Console.ReadKey();        
        }

        private static void AddToDatabase(DBConnection db)
        {
            throw new NotImplementedException();
        }

        private static string FindMedium(string nameWithMedium)
        {
            if (nameWithMedium.ToLower().Contains("journal"))
            {
                return "Journal";
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
            if (nameWithMedium.ToLower().Contains("dailygraph"))
            {
                return "The Dailygraph";
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
            if (nameWithMedium.ToLower().Contains("demeter"))
            {
                return "Log of the Demeter";
            }

            return "";
        }
        private static string FindAuthor(string nameWithMedium)
        {
         
            if (nameWithMedium.ToLower().Contains("jonathan harker"))
            {
                return "Jonathan Harker";
            }
            if (nameWithMedium.ToLower().Contains("seward"))
            {
                return "Dr. Seward";
            }
            if (nameWithMedium.ToLower().Contains("dracula"))
            {
                return "Dracula";
            }
            if (nameWithMedium.ToLower().Contains("lucy westenra"))
            {
                return "Lucy Westenra";
            }
            if (nameWithMedium.ToLower().Contains("mina murray"))
            {
                return "Mina Murray";
            }
            if (nameWithMedium.ToLower().Contains("quincey p. morris"))
            {
                return "Quincey P. Morris";
            }
            if (nameWithMedium.ToLower().Contains("arthur holmwood"))
            {
                return "Arthur Holmwood";
            }
            if (nameWithMedium.ToLower().Contains("mina harker"))
            {
                return "Mina Harker";
            }
            if (nameWithMedium.ToLower().Contains("mitchell"))
            {
                return "Mitchell, Sons & Candy";
            }
            if (nameWithMedium.ToLower().Contains("helsing"))
            {
                return "Abraham Van Helsing";
            }
            if (nameWithMedium.ToLower().Contains("godalming"))
            {
                return "Lord Godalming";
            }
            if (nameWithMedium.ToLower().Contains("patrick hennessey"))
            {
                return "Patrick Hennessey";
            }
            if (nameWithMedium.ToLower().Contains("sister agatha"))
            {
                return "Sister Agatha";
            }
            if (nameWithMedium.ToLower().Contains("wilhelmina"))
            {
                return "Mina Murray";
            }
            if (nameWithMedium.ToLower().Contains("billington"))
            {
                return "Samuel F. Billington & Sons, Solicitors";
            }
            if (nameWithMedium.ToLower().Contains("rufus"))
            {
                return "Rufus Smith, Lloyd's London";
            }
            if (nameWithMedium.ToLower().Contains("messrs"))
            {
                return "Messrs. Carter, Paterson & Co., London";
            }
            return "";        
        }
    }
}
