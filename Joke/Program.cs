using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Joke.Front;
using Joke.Front.Pony;
using Joke.Front.Pony.Lex;
using Joke.Outside;
using Joke.Outside.Build;

namespace Joke
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            //EnsureSources();
            PonyParse(0);

            Console.Write("(almost) any key ... ");
            Console.ReadKey(true);
        }

        private static void PonyParse(int skip = 0)
        {
            int no = 0;
            int lines = 0;

            // foreach (var ponyFile in EnumeratePonies())
            foreach (var ponyFile in EnumeratePackagePonies())
            {
                no += 1;
                if (no <= skip)
                {
                    continue;
                }
                if (!PonyParse(ref lines, no, ponyFile))
                {
                    break;
                }
            }
            Console.WriteLine($"{lines} lines read");
        }

        private static bool PonyParse(ref int lines, int no, FileRef ponyFile)
        {
            Console.WriteLine($"{no}. {ponyFile}");

            var source = Source.FromFile(ponyFile);
            lines += source.LineCount;
            var tokenizer = new Tokenizer(source);

#if true
            try
            {
                var tokens = tokenizer.Tokens().ToList();

                return true;
            }
            catch (Exception e)
            {
                var (line, col) = source.GetLineCol(tokenizer.index);

                var msg = string.IsNullOrWhiteSpace(e.Message) ? string.Empty : $" - {e.Message}";
                Console.WriteLine($"({line},{col}): can't continue @{tokenizer.index}{msg}");
                var arrow = new string('-', col - 1) + "^";
                if (line > 1)
                {
                    Console.WriteLine($" |{source.GetLine(line - 1).ToString()}");
                }
                Console.WriteLine($" |{source.GetLine(line).ToString()}");
                Console.WriteLine($" |{arrow}");
                Console.WriteLine($" |{source.GetLine(line + 1).ToString()}");
                var at = e.StackTrace?.Split(" at ", StringSplitOptions.RemoveEmptyEntries)[1];
                Console.WriteLine($"{at}");

                return false;
            }
#else
            var scanner = new PonyScanner(source);
            var parser = new PonyParser(scanner);

            try
            {
                parser.Parse();

                return true;
            }
            catch (Exception e)
            {
                var (line, col) = source.GetLineCol(scanner.Current);

                var msg = string.IsNullOrWhiteSpace(e.Message) ? string.Empty : $" - {e.Message}";
                Console.WriteLine($"({line},{col}): can't continue @{scanner.Current}{msg}");
                var arrow = new string('-', col-1) + "^";
                Console.WriteLine($" |{source.GetLine(line-1).ToString()}");
                Console.WriteLine($" |{source.GetLine(line).ToString()}");
                Console.WriteLine($" |{arrow}");
                Console.WriteLine($" |{source.GetLine(line+1).ToString()}");
                var at = e.StackTrace?.Split(" at ", StringSplitOptions.RemoveEmptyEntries)[1];
                Console.WriteLine($"{at}");

                return false;
            }
#endif
        }

        private static IEnumerable<FileRef> EnumeratePackagePonies()
        {
            var root = DirRef.ProjectDir().Up.Up.Dir("Temp").Dir("ponyc").Dir("packages");

            foreach (var pony in Directory.EnumerateFiles(root, "*.pony", SearchOption.AllDirectories))
            {
                yield return FileRef.From(pony);
            }
        }

        private static IEnumerable<FileRef> EnumeratePonies()
        {
            //var root = DirRef.ProjectDir().Up.Up.Dir("Temp").Dir("ponyc").Dir("packages");
            var root = DirRef.ProjectDir().Up.Up.Dir("Temp").Dir("ponyc");
            var root2 = DirRef.ProjectDir().Up.Up.Dir("Temp").Dir("pony-source");

            foreach (var pony in
                Directory.EnumerateFiles(root, "*.pony", SearchOption.AllDirectories).Concat(
                    Directory.EnumerateFiles(root2, "*.pony", SearchOption.AllDirectories)))
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

            var ponySources = temp.Dir("pony-source");
            ponySources.Ensure();

            repository = ponySources.Dir("ponyup");
            GitRunner.Ensure("https://github.com/ponylang/ponyup.git", repository);

            repository = ponySources.Dir("corral");
            GitRunner.Ensure("https://github.com/ponylang/corral.git", repository);

            repository = ponySources.Dir("pony-stable");
            GitRunner.Ensure("https://github.com/ponylang/pony-stable.git", repository);

            repository = ponySources.Dir("appdirs");
            GitRunner.Ensure("https://github.com/ponylang/appdirs.git", repository);

            repository = ponySources.Dir("net_ssl");
            GitRunner.Ensure("https://github.com/ponylang/net_ssl.git", repository);

            repository = ponySources.Dir("http");
            GitRunner.Ensure("https://github.com/ponylang/http.git", repository);

            repository = ponySources.Dir("reactive-streams");
            GitRunner.Ensure("https://github.com/ponylang/reactive-streams.git", repository);

            repository = ponySources.Dir("pony-kafka");
            GitRunner.Ensure("https://github.com/WallarooLabs/pony-kafka.git", repository);

            repository = ponySources.Dir("ponylang-linal");
            GitRunner.Ensure("https://github.com/dougmacdoug/ponylang-linal.git", repository);

            repository = ponySources.Dir("wallaroo");
            GitRunner.Ensure("https://github.com/WallarooLabs/wallaroo.git", repository);

            // 
        }
    }
}
