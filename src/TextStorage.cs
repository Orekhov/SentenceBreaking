using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            separators = new List<Separator>();
            text = System.IO.File.ReadAllText("input.txt", Encoding.UTF8);
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
        public List<Separator> separators;
        public string text;
    }
}
