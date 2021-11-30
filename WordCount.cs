using System;
using System.Collections.Generic;
using System.Linq;

namespace MapReduce
{
    
    public class WordCount
    {
        public WordCount(Dictionary<String, int> wordCount)
        {
            this.wordCount = wordCount;
        }

        public Dictionary<String, int> wordCount;
        
    }
}
