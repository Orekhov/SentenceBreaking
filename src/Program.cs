using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SentenceBreaking
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TextStorage ts = TextStorage.Instance;
                Console.WriteLine(" \n Text with all separators: ");
                ts.showTextWithSeparators(false);

                TextAnalyser ta = new FiltersAnalyser(ts, 1);
                ta.check();
                ta = new ExclusionsAnalyser(ts, -1);
                ta.check();

                Console.WriteLine(" \n\n Result: ");
                ts.showTextWithSeparators(true, ConsoleColor.DarkMagenta);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
