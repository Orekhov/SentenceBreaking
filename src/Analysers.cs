using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SentenceBreaking
{
    public interface IAnalyser
    {
        void check(string par);
    }

    public abstract class TextAnalyser : IAnalyser
    {
        public TextAnalyser(TextStorage ts) { this.ts = ts; }
        protected TextStorage ts;
        public abstract void check(string regExp);
    }

    class FiltersAnalyser: TextAnalyser
    {
        public FiltersAnalyser(TextStorage ts) :base(ts) {}

        public void showTextWithSeparators(bool onlyValidSeparators, ConsoleColor c = ConsoleColor.DarkGreen)
        {
            for (int i = 0; i < ts.text.Length; i++)
            {
                if (containsIndex(i) && (!onlyValidSeparators || indexBelongsToMaxValidity(i)))
                    Console.BackgroundColor = c;
                else
                    Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(ts.text[i]);
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("\n");
        }

        public void addSeparators(string regExp)
        {
            MatchCollection allMatches = Regex.Matches(ts.text, regExp);
            foreach (Match m in allMatches)
            {
                if (!containsAnyIndexFromRange(m.Index, m.Length))
                    ts.separators.Add(new Separator(m.Index, m.Length));
            }
        }

        override public void check(string regExp)
        {
            MatchCollection allMatches = Regex.Matches(ts.text, regExp);
            foreach (Match m in allMatches)
            {
                Separator s;
                if (getSepByRange(m.Index, m.Length, out s))
                    incrementValidity(s);
            }
            show1(ts.text, allMatches);
        }

        private void incrementValidity(Separator s)
        {
            for (int i = 0; i < ts.separators.Count; i++)
                if (ts.separators[i].position == s.position)
                    ts.separators[i].validity += 1;
        }

        private bool getSepByIndex(int index, out Separator sep)
        {
            foreach (Separator s in ts.separators)
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
            int max = ts.separators.Select(r => r.validity).Max();
            Separator s;
            getSepByIndex(index, out s);
            return (s.validity == max);
        }

        private bool containsIndex(int index)
        {
            if (ts.separators.Count == 0)
                return false;
            foreach (Separator s in ts.separators)
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

        public void showValInOrder()
        {
            Console.WriteLine(" ");
            foreach (Separator s in ts.separators.OrderBy(r => r.position))
                Console.Write(" " + s.validity + " ");
            Console.Write(" " + ts.separators.Sum(s => s.validity) + " ");
        }

        void show1(string text, MatchCollection myMatch)
        {
            for (int i = 0; i < text.Length; i++)
            {
                foreach (Match m in myMatch)
                {
                    if ((i >= m.Index) && (i < m.Index + m.Length))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.Write(text[i]);
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
