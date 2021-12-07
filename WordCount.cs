using System;
using System.Collections.Generic;
using System.Linq;

namespace MapReduce
{
    
    public class WordCount
    {
        public WordCount(Dictionary<string, int> wordCount)
        {
            this.wordCount = wordCount;
        }

        public Dictionary<string, int> wordCount;
        
    }
}
