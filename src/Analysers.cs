using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SentenceBreaking
{
    public interface IAnalyser
    {
        void check();
    }

    public abstract class TextAnalyser : IAnalyser
    {
        public TextAnalyser(TextStorage ts) { this.ts = ts; }
        protected TextStorage ts;
        public abstract void check();
    }

    class FiltersAnalyser : TextAnalyser
    {
        public FiltersAnalyser(TextStorage ts)
            : base(ts)
        {
            filters = new List<string>(System.IO.File.ReadAllLines("filters", Encoding.UTF8));
        }

        override public void check()
        {
            for (int i = 0; i < filters.Count; i++)
            {
                Console.WriteLine(" \n filter {0}: ", i + 1);
                checkNext(filters[i]);
                ts.showValInOrder();
            }
        }

        private void checkNext(string regExp)
        {
            MatchCollection allMatches = Regex.Matches(ts.text, regExp);
            foreach (Match m in allMatches)
            {
                int pos = ts.getIndexByRange(m.Index, m.Length);
                if (pos!=-1)
                    ts.incrementValidity(pos);
            }
            ts.showMatches(allMatches);
        }

        private List<String> filters;
    }
}
