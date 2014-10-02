using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Dracula
{
    class Program
    {
        static void Main(string[] args)
        {
            var dateToText = new Dictionary<string, List<StringBuilder>>(); 

            using (var sr = new StreamReader(@"C:\Users\Reza\Documents\Dracula\Web\Dracula.htm"))
            {
                string line = "";
                StringBuilder value = new StringBuilder();
                string key = "";

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();

                    if (line.StartsWith("<p><i>"))
                    {
                        if (value.Length != 0 && key != "")
                        {
                            if (dateToText.ContainsKey(key))
                            {
                                dateToText[key].Add(value);
                                
                            }
                            else
                            {
                                var valueList = new List<StringBuilder>();
                                valueList.Add(value);
                                dateToText.Add(key, valueList);
                                
                            }
                            
                            value = new StringBuilder();
                        }

                        key = Regex.Replace(line, "</i>.*$", "");
                        key = Regex.Replace(key, "^<p><i>", "");
                        string endOfLine = Regex.Replace(line, "^.*</i>", "");
                        value.AppendLine(endOfLine);
                        Console.WriteLine(key);
                    }
                    else
                    {
                        value.AppendLine(line);
                    }
                }
            }
            Console.ReadKey();
        }
    }
}