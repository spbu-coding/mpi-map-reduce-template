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
    internal class Document
    {
        public Document(String[] words)
        {
            this.words = words;
        }
        public String[] words;
    }

    internal class Program
    {
        static void check(Intracommunicator communicator, Document[] documents, WordCount expected)
        {
            var actualWordCount = Executor.execute(documents[communicator.Rank], communicator);
            if (communicator.Rank == 0 && !actualWordCount.Equals(expected))
            {
                throw new Exception();
            }
            
            if (communicator.Rank > 0 && actualWordCount != null)
            {
                throw new Exception();
            }
        }
        static void a(Intracommunicator communicator)
        {
            var documents = new Document[]
            {
                new Document(new String[] {"a", "b"}),
                new Document(new String[] {"b", "c"}),
                new Document(new String[] {"c", "d", "d"}),
                new Document(new String[] {"d", "e"}),
            };
            var expectedWordCount = new WordCount(new Dictionary<string, int>
            {
                {"a", 1},
                {"b", 2},
                {"c", 2},
                {"d", 3},
                {"e", 1}
            });
            check(communicator, documents, expectedWordCount);
        }
        static void b(Intracommunicator communicator)
        {
            var documents = new Document[]
            {
                null, 
                new Document(new String[] {"a", "b", "b", "b", "c", "b"}),
                new Document(new String[] {"b", "c", "b", "b"}),
                null
            };
            var expectedWordCount = new WordCount(new Dictionary<string, int>
            {
                {"a", 1},
                {"b", 7},
                {"c", 2}
            });
            check(communicator, documents, expectedWordCount);
        }
        static void c(Intracommunicator communicator)
        {
            var documents = new Document[]
            { 
                null, 
                null, 
                null,
                null
            };
            var expectedWordCount = new WordCount(new Dictionary<string, int>
            {
            });
            check(communicator, documents, expectedWordCount);
        }
        static void d(Intracommunicator communicator)
        {
            var documents = new Document[]
            {
                new Document(new String[] {"hello", "world"}),
                new Document(new String[] {"good", "bye", "world"}),
                new Document(new String[] {"hello", "again"}),
                
            };
            var expectedWordCount = new WordCount(new Dictionary<string, int>
            {
                {"hello", 2},
                {"world", 2},
                {"good", 1},
                {"bye", 1},
                {"again", 1}
            });
            check(communicator, documents, expectedWordCount);
        }
        public static void Main(string[] args)
        {
            MPI.Environment.Run(ref args, communicator =>
            {
                a(communicator);
                b(communicator);
            });
        }
    }
}
