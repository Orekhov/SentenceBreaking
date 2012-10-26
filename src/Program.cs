using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SentenceBreaking
{

    class Program
    {
        static List<String> filters;
        static FiltersAnalyser fa;
        static void Main(string[] args)
        {
            try
            {
                filters = new List<string>(System.IO.File.ReadAllLines("filters", Encoding.UTF8));
                fa = new FiltersAnalyser(TextStorage.Instance);
                string[] separators = System.IO.File.ReadAllLines("separators", Encoding.UTF8);
                foreach (string sep in separators)
                    fa.addSeparators(sep);

                Console.WriteLine(" \n Text with all separators: ");
                fa.showTextWithSeparators(true);

                for (int i = 0; i < filters.Count; i++)
                {
                    Console.WriteLine(" \n filter {0}: ", i);
                    fa.check(filters[i]);
                    //tsa.showTextWithSeparators(true);
                    fa.showValInOrder();
                    Console.WriteLine("\n");
                }

                Console.WriteLine(" \n Result: ");
                fa.showTextWithSeparators(true, ConsoleColor.DarkMagenta);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
