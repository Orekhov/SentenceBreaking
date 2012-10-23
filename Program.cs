using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApplication3
{
    class Separator
    {
        public Separator(int p, int l, int v = 0) { position = p; length = l; validity = v; }
        public int position;
        public int length;
        public int validity;
    };

    class TextSeparatorsAnalyser
    {
        public TextSeparatorsAnalyser(string text) { separators = new List<Separator>(); this.text = text; }

        public void showTextWithSeparators(bool onlyValidSeparators, ConsoleColor c = ConsoleColor.DarkGreen)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (containsIndex(i) && (!onlyValidSeparators || indexBelongsToMaxValidity(i)))
                    Console.BackgroundColor = c;
                else
                    Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(text[i]);
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("\n");
        }

        public void addSeparators(string regExp)
        {
            MatchCollection allMatches = Regex.Matches(text, regExp);
            foreach (Match m in allMatches)
            {
                if (!containsAnyIndexFromRange(m.Index, m.Length))
                    separators.Add(new Separator(m.Index, m.Length));
            }
        }

        public void check(string regExp)
        {
            MatchCollection allMatches = Regex.Matches(text, regExp);
            foreach (Match m in allMatches)
            {
                Separator s;
                if (getSepByRange(m.Index, m.Length, out s))
                    incrementValidity(s);
            }
        }

        private void incrementValidity(Separator s)
        {
            for (int i = 0; i < separators.Count; i++)
                if (separators[i].position == s.position)
                    separators[i].validity += 1;
        }

        private bool getSepByIndex(int index, out Separator sep)
        {
            foreach (Separator s in separators)
                if ((index >= s.position) && (index < s.position + s.length))
                {
                    sep = s;
                    return true;
                }
            sep = new Separator(-1, -1, -1);
            return false;
        }

        private bool getSepByRange(int indexBeggining, int length, out Separator sep)
        {
            for (int i = indexBeggining; i < indexBeggining + length; i++)
            {
                Separator s;
                if (getSepByIndex(i, out s))
                {
                    sep = s;
                    return true;
                }
            }
            sep = new Separator(-1, -1, -1);
            return false;
        }

        private bool indexBelongsToMaxValidity(int index)
        {
            int max = separators.Select(r => r.validity).Max();
            Separator s;
            getSepByIndex(index, out s);
            return (s.validity == max);
        }

        private bool containsIndex(int index)
        {
            if (separators.Count == 0)
                return false;
            foreach (Separator s in separators)
                if ((index >= s.position) && (index < s.position + s.length))
                    return true;
            return false;
        }

        private bool containsAnyIndexFromRange(int indexBeggining, int length)
        {
            for (int i = indexBeggining; i < indexBeggining + length; i++)
                if (containsIndex(i))
                    return true;
            return false;
        }

        private List<Separator> separators;
        private string text;
    };

    class Program
    {
        static List<String> filters;
        static TextSeparatorsAnalyser tsa;
        static void Main(string[] args)
        {
            try
            {
                filters = new List<string>(System.IO.File.ReadAllLines("filters", Encoding.UTF8));
                tsa = new TextSeparatorsAnalyser(System.IO.File.ReadAllText("input.txt", Encoding.UTF8));
                string[] separators = System.IO.File.ReadAllLines("separators", Encoding.UTF8);
                foreach (string sep in separators)
                    tsa.addSeparators(sep);

                Console.WriteLine(" \n Text with all separators: ");
                tsa.showTextWithSeparators(true);

                for (int i = 0; i < filters.Count; i++)
                {
                    tsa.check(filters[i]);
                    Console.WriteLine(" \n filter {0}: ", i);
                    tsa.showTextWithSeparators(true);
                }

                Console.WriteLine(" \n Result: ");
                tsa.showTextWithSeparators(true, ConsoleColor.DarkMagenta);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
