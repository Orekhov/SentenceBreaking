using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SentenceBreaking
{
    public abstract class TextAnalyser
    {
        public TextAnalyser(TextStorage ts) { this.ts = ts; }
        protected TextStorage ts;
        public abstract void check();
    }

    public abstract class PatternAnalyser : TextAnalyser
    {
        public PatternAnalyser(TextStorage ts, int weight) : base(ts) { this.weight = weight; }
        protected int weight;
    }

    public abstract class LexicalAnalyser : TextAnalyser
    {
        public LexicalAnalyser(TextStorage ts) : base(ts) { }
    }

    class FiltersAnalyser : PatternAnalyser
    {
        public FiltersAnalyser(TextStorage ts, int weight)
            : base(ts,weight)
        {
            filters = new List<string>(System.IO.File.ReadAllLines("filters", Encoding.UTF8));
        }

        override public void check()
        {
            for (int i = 0; i < filters.Count; i++)
            {
                Console.WriteLine(" \n\n filter {0}: ", i + 1);
                ts.checkText(filters[i], weight);
            }
        }

        private List<String> filters;
    }

    class ExclusionsAnalyser : PatternAnalyser
    {
        public ExclusionsAnalyser(TextStorage ts, int weight)
            : base(ts,weight)
        {
            exclusions = new List<string>(System.IO.File.ReadAllLines("exclusions", Encoding.UTF8));
        }
        override public void check()
        {
            for (int i = 0; i < exclusions.Count; i++)
            {
                Console.WriteLine(" \n\n exclusion {0}: ", i + 1);
                ts.checkText(exclusions[i], weight);
            }
        }
        private List<String> exclusions;
    }
}
