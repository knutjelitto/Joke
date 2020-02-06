using System;
using System.Collections.Generic;
using System.IO;
using Joke.Front;
using Joke.Front.Joke;
using Joke.Front.Pony;
using Joke.Outside;
using Joke.Outside.Build;

namespace Joke
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            //EnsureSources();
            //EnumeratePonies();
            PonyParse();

            Console.Write("(almost) any key ... ");
            Console.ReadKey(true);
        }

        private static void PonyParse()
        {
            foreach (var ponyFile in EnumeratePonies())
            {
                if (!PonyParse(ponyFile))
                {
                    break;
                }
                break;
            }
        }

        private static bool PonyParse(FileRef ponyFile)
        {
            Console.WriteLine($"{ponyFile}");

            var source = Source.FromFile(ponyFile);
            var scanner = new PonyScanner(source);
            var parser = new PonyParser(scanner);

            try
            {
                parser.Parse();

                return true;
            }
            catch (NotImplementedException)
            {
                Console.WriteLine($"can't continue @{scanner.Current}");

                return false;
            }
        }

        private static DirRef PonyBuiltin()
        {
            return DirRef.ProjectDir().Up.Up.Dir("Temp").Dir("ponyc").Dir("packages").Dir("builtin");
        }

        private static IEnumerable<FileRef> EnumeratePonies()
        {
            var root = DirRef.ProjectDir().Up.Up.Dir("Temp").Dir("ponyc").Dir("packages");

            foreach (var pony in Directory.EnumerateFiles(root, "*.pony", SearchOption.AllDirectories))
            {
                yield return FileRef.From(pony);
            }
        }

        private static void EnsureSources()
        {
            var thisProject = DirRef.ProjectDir();
            var temp = thisProject.Up.Up.Dir("Temp");

            if (!temp.Exists)
            {
                throw new InvalidOperationException();
            }

            var repository = temp.Dir("JSONTestSuite");
            GitRunner.Ensure("https://github.com/nst/JSONTestSuite.git", repository);

            repository = temp.Dir("ponyc");
            GitRunner.Ensure("https://github.com/ponylang/ponyc.git", repository);
        }
    }
}
