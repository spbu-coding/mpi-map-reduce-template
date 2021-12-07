using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using MPI;

namespace MapReduce
{
    
    internal class Program
    {
        private static void Check(Intracommunicator communicator, Document?[] documents, WordCount expected)
        {
            var actualWordCount = Executor.Execute(documents[communicator.Rank], communicator);
            if (communicator.Rank == 0 && !actualWordCount.Equals(expected))
            {
                throw new Exception();
            }
            
            if (communicator.Rank > 0 && actualWordCount != null)
            {
                throw new Exception();
            }
        }
        private static void Test1(Intracommunicator communicator)
        {
            var documents = new Document[]
            {
                new (new [] {"a", "b"}),
                new (new [] {"b", "c"}),
                new (new [] {"c", "d", "d"}),
                new (new [] {"d", "e"}),
            };
            var expectedWordCount = new WordCount(new Dictionary<string, int>
            {
                {"a", 1},
                {"b", 2},
                {"c", 2},
                {"d", 3},
                {"e", 1}
            });
            Check(communicator, documents, expectedWordCount);
        }
        private static void Test2(Intracommunicator communicator)
        {
            var documents = new Document?[]
            {
                null, 
                new (new [] {"a", "b", "b", "b", "c", "b"}),
                new (new [] {"b", "c", "b", "b"}),
                null
            };
            var expectedWordCount = new WordCount(new Dictionary<string, int>
            {
                {"a", 1},
                {"b", 7},
                {"c", 2}
            });
            Check(communicator, documents, expectedWordCount);
        }
        private static void Test3(Intracommunicator communicator)
        {
            var documents = new Document?[]
            { 
                null, 
                null, 
                null,
                null
            };
            var expectedWordCount = new WordCount(new Dictionary<string, int>());
            Check(communicator, documents, expectedWordCount);
        }
        private static void Test4(Intracommunicator communicator)
        {
            var documents = new Document?[]
            {
                new (new [] {"hello", "world"}),
                new (new [] {"good", "bye", "world"}),
                new (new [] {"hello", "again"}),
                null
            };
            var expectedWordCount = new WordCount(new Dictionary<string, int>
            {
                {"hello", 2},
                {"world", 2},
                {"good", 1},
                {"bye", 1},
                {"again", 1}
            });
            Check(communicator, documents, expectedWordCount);
        }
        public static void Main(string[] args)
        {
            MPI.Environment.Run(ref args, communicator =>
            {
                Test1(communicator);
                Test2(communicator);
                Test3(communicator);
                Test4(communicator);
            });
        }
    }
}