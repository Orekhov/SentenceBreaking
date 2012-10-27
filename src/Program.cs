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
                ts.showTextWithSeparators(true);

                FiltersAnalyser fa = new FiltersAnalyser(ts);
                fa.check();

                Console.WriteLine(" \n Result: ");
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
