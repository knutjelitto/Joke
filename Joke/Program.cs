using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using Joke.Front;
using Joke.Front.Pony.Lex;
using Joke.Front.Pony.Syntax;
using Joke.Front.Pony.Visit;
using Joke.Outside;
using Joke.Outside.Build;

namespace Joke
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            //EnsureSources();
            //PonyParse(0, EnumeratePackagePonies());
            PonyParse(0, EnumerateAllPonies());

            Console.Write("(almost) any key ... ");
            Console.ReadKey(true);
        }

        private static void PonyParse(int skip, IEnumerable<FileRef> ponies)
        {
            int no = 0;
            int lines = 0;
            var stats = new Stats();

            foreach (var ponyFile in ponies)
            {
                no += 1;
                if (no <= skip)
                {
                    continue;
                }
                if (!PonyParse(ref lines, no, ponyFile, stats))
                {
                    break;
                }
            }
            Console.WriteLine($"{lines} lines read");
            Console.WriteLine();
            stats.Report(Console.Out);
        }

        private static bool PonyParse(ref int lines, int no, FileRef ponyFile, Stats stats)
        {
            Console.WriteLine($"{no}. {ponyFile}");

            var source = Source.FromFile(ponyFile);
            lines += source.LineCount;
            var tokenizer = new Tokenizer(source);

            try
            {
                var tokens = tokenizer.Tokens().ToList();

                var builder = new StringBuilder();
                foreach (var token in tokens)
                {
                    builder.Append(token.GetClutter(source));
                    builder.Append(token.GetPayload(source));
                }
                var content = source.Content;
                var rebuild = builder.ToString();

                Debug.Assert(content == rebuild);

                var parser = new PonyParser(source, tokens);

                try
                {
                    var module = parser.Module();

                    var visitor = new TokenCoverageVisitor();

                    visitor.Visit(module);

                    Console.WriteLine($"{parser.Tokens.Count} :: {visitor.Set.Cardinality}");

                    //stats.Update(module);

                    return false;
                }
                catch (Exception e)
                {
                    var (line, col) = source.GetLineCol(parser.Offset);

                    ErrorMessage(e, parser.Offset, line, col);

                    return false;
                }
            }
            catch (Exception e)
            {
                var (line, col) = source.GetLineCol(tokenizer.next);

                ErrorMessage(e, tokenizer.next, line, col);

                return false;
            }

            void ErrorMessage(Exception e, int offset, int line, int col)
            {
                var msg = string.IsNullOrWhiteSpace(e.Message) ? string.Empty : $" - {e.Message}";
                Console.WriteLine($"({line},{col}): can't continue @{offset} {msg}");
                var arrow = new string('-', col - 1) + "^";
                if (line > 3) Console.WriteLine($" |{source.GetLine(line - 3).ToString()}");
                if (line > 2) Console.WriteLine($" |{source.GetLine(line - 2).ToString()}");
                if (line > 1) Console.WriteLine($" |{source.GetLine(line - 1).ToString()}");
                Console.WriteLine($" |{source.GetLine(line).ToString()}");
                Console.WriteLine($" |{arrow}");
                Console.WriteLine($" |{source.GetLine(line + 1).ToString()}");
                var at = e.StackTrace?.Split(" at ", StringSplitOptions.RemoveEmptyEntries)[1];
                //var at = e.StackTrace;
                Console.WriteLine($"{at}");
            }
        }

        private static IEnumerable<FileRef> EnumeratePackagePonies()
        {
            var root = DirRef.ProjectDir().Up.Up.Dir("Temp").Dir("ponyc").Dir("packages");

            foreach (var pony in Directory.EnumerateFiles(root, "*.pony", SearchOption.AllDirectories))
            {
                yield return FileRef.From(pony);
            }
        }

        private static IEnumerable<FileRef> EnumerateAllPonies()
        {
            //var root = DirRef.ProjectDir().Up.Up.Dir("Temp").Dir("ponyc").Dir("packages");
            var root = DirRef.ProjectDir().Up.Up.Dir("Temp").Dir("ponyc");
            var root2 = DirRef.ProjectDir().Up.Up.Dir("Temp").Dir("pony-source");

            foreach (var pony in
                Directory.EnumerateFiles(root, "*.pony", SearchOption.AllDirectories).Concat(
                    Directory.EnumerateFiles(root2, "*.pony", SearchOption.AllDirectories)))
            {
                if (pony.Contains(@"\ponycc\test\fixtures\") ||
                    pony.Contains(@"\adv5.pony") ||
                    pony.Contains(@"\bench\bench_pg.pony") ||
                    pony.Contains(@"\examples\clisample.pony"))
                {
                    continue;
                }
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

            repository = temp.Dir("pony-source");
            repository.Ensure();

            string[] sources = new string[] 
            {
                "https://github.com/ponylang/ponyup.git",
                "https://github.com/ponylang/corral.git",
                "https://github.com/ponylang/pony-stable.git",
                "https://github.com/ponylang/appdirs.git",
                "https://github.com/ponylang/net_ssl.git",
                "https://github.com/ponylang/http.git",
                "https://github.com/ponylang/reactive-streams.git",
                "https://github.com/WallarooLabs/pony-kafka.git",
                "https://github.com/dougmacdoug/ponylang-linal.git",
                "https://github.com/WallarooLabs/wallaroo.git",
                "https://github.com/jemc/pony-zmq.git",
                "https://github.com/Theodus/jennet.git",
                "https://github.com/jtfmumm/novitiate.git",
                "https://github.com/mfelsche/ponycheck.git",
                "https://github.com/jemc/jylis.git",
                "https://github.com/oraoto/pony-websocket.git",
                "https://github.com/jemc/ponycc.git",
                "https://github.com/jemc/pony-crdt.git",
                "https://github.com/SeanTAllen/pony-msgpack.git",
                "https://github.com/sylvanc/pony-lecture.git",
                "https://github.com/jemc/pony-sodium.git",
                "https://github.com/jemc/pony-capnp.git",
                "https://github.com/kulibali/kiuatan.git",
                "https://github.com/lisael/pony-postgres.git",
                "https://github.com/mfelsche/ponyfmt.git",
                "https://github.com/joncfoo/pony-sqlite.git",
                "https://github.com/autodidaddict/ponymud.git",
                "https://github.com/ponylang/changelog-tool.git",
                "https://github.com/jemc/pony-pegasus.git",
                "https://github.com/jemc/pony-llvm.git",
                "https://github.com/krig/tinyhorse.git",
                "https://github.com/sylvanc/peg.git",
                "https://github.com/EpicEric/pony-mqtt.git",
                "https://github.com/jemc/pony-rope.git",
                "https://github.com/BrianOtto/pony-gui.git",
                "https://github.com/jemc/pony-jason.git",
                "https://github.com/BrianOtto/pony-win32.git",
                "https://github.com/jkleiser/toy-forth-in-pony.git",
                "https://github.com/emilbayes/pony-endianness.git",
                "https://github.com/sgebbie/pony-graphs.git",
                "https://github.com/lisael/pied.git",
                "https://github.com/ponylang/regex.git",
                "https://github.com/mfelsche/pony-maybe.git",
                "https://github.com/sgebbie/pony-statsd.git",
                "https://github.com/jemc/pony-unsafe.git",
                "https://github.com/lisael/pony-bitarray.git",
                "https://github.com/lisael/pony-bm.git",
                "https://github.com/slayful/sagittarius.git",
                "https://github.com/ponylang/glob.git",
                "https://github.com/jtfmumm/pony-logic.git",
                "https://github.com/sgebbie/pony-tty.git",
                "https://github.com/jtfmumm/pony-queue.git",
                "https://github.com/mfelsche/pony-kv.git",
                "https://github.com/elmattic/pony-toml.git",
                "https://github.com/krig/pony-sform.git",
                "https://github.com/jemc/pony-dict.git",
                "https://github.com/cquinn/ponycli.git",
                "https://github.com/niclash/pink2web.git",
                "https://github.com/andrenth/pony-uuid.git",
                "https://github.com/kulibali/kiuatan-calculator.git",
                "https://github.com/Theodus/pony-stats.git",
                "https://github.com/jtfmumm/microkanren-pony.git",
                "https://github.com/ergl/sss.git",
            };

            foreach (var url in sources)
            {
                var name = Path.GetFileNameWithoutExtension(url);
                GitRunner.Ensure(url, repository.Dir(name));
            }
        }
    }
}
