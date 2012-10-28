using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SentenceBreaking
{
    public class Separator
    {
        public Separator(int p, int l, int v = 0) { position = p; length = l; validity = v; }
        public int position;
        public int length;
        public int validity;
    };

    // Singleton
    public class TextStorage
    {
        protected TextStorage()
        {
            indexToPositionCache = new Dictionary<int, int>();
            separators = new List<Separator>();
            text = System.IO.File.ReadAllText("input.txt", Encoding.UTF8);
            string[] sepRegExps = System.IO.File.ReadAllLines("separators", Encoding.UTF8);
            foreach (string regExp in sepRegExps)
            {
                MatchCollection allMatches = Regex.Matches(text, regExp);
                foreach (Match m in allMatches)
                {
                    if (!containsAnyIndexFromRange(m.Index, m.Length))
                    {
                        separators.Add(new Separator(m.Index, m.Length));
                        for (int i = m.Index; i < m.Index + m.Length; i++)
                            indexToPositionCache.Add(i, m.Index);
                    }
                }
            }
        }

        private sealed class SingletonCreator
        {
            private static readonly TextStorage instance = new TextStorage();
            public static TextStorage Instance { get { return instance; } }
        }

        public static TextStorage Instance
        {
            get { return SingletonCreator.Instance; }
        }

        private List<Separator> separators;
        public readonly string text;
        private Dictionary<int,int> indexToPositionCache;

        public int getIndexByRange(int indexBeggining, int length)
        {
            for (int i=indexBeggining; i<indexBeggining+length;i++)
                if ( indexToPositionCache.ContainsKey(i) )
                    return indexToPositionCache[i];
            return -1;
        }

        public void incrementValidity(int pos)
        {
            for (int i = 0; i < separators.Count; i++)
                if (separators[i].position == pos)
                {
                    separators[i].validity += 1;
                    return;
                }
        }

        public void showTextWithSeparators(bool onlyValidSeparators, ConsoleColor c = ConsoleColor.DarkGreen)
        {
            int maxValidity = separators.Select(r => r.validity).Max();
            List<int> maxValIndexes = new List<int>();
            foreach (Separator s in separators)
            {
                if (s.validity == maxValidity)
                {
                    for (int i = s.position; i < s.position + s.length; i++)
                        maxValIndexes.Add(i);
                }
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (containsIndex(i) && (!onlyValidSeparators || maxValIndexes.Contains(i)))
                    Console.BackgroundColor = c;
                else
                    Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(text[i]);
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("\n");
        }

        public void showMatches(MatchCollection myMatch)
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

        public void showValInOrder()
        {
            Console.WriteLine(" ");
            foreach (Separator s in separators.OrderBy(r => r.position))
                Console.Write(" " + s.validity + " ");
            Console.Write(" " + separators.Sum(s => s.validity) + " ");
            Console.WriteLine("\n");
        }

        private bool containsAnyIndexFromRange(int indexBeggining, int length)
        {
            for (int i = indexBeggining; i < indexBeggining + length; i++)
                if (containsIndex(i))
                    return true;
            return false;
        }

        private bool containsIndex(int index)
        {
            return indexToPositionCache.ContainsKey(index);
        }
    }
}
