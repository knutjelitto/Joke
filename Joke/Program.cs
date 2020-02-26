using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Joke.Compiler;
using Joke.Front;
using Joke.Front.Err;
using Joke.Front.Pony.Lexing;
using Joke.Front.Pony.Parsing;
using Joke.Outside;
using Joke.Outside.Build;

namespace Joke
{
    internal class Program
    {
        internal static void Main()
        {
            //EnsureSources();
            //PonyParse(0, EnumerateBuiltinPonies());
            //PonyParse(0, EnumeratePackagePonies());
            //PonyParse(0, EnumerateAllPonies());
            //PonyTest();
            PonyExample("mandelbrot");

            Console.Write("(almost) any key ... ");
            Console.ReadKey(true);
        }

        private static void PonyTest()
        {
            var fix = DirRef.ProjectDir().Dir("..").Dir("Joke.Front.Pony").Dir("Fixtures").Dir("Expressions");

            Debug.Assert(fix.File("Ops.pony").Exists());

            var errors = new ErrorAccu();
            var context = new CompilerContext(Packages);

            var module = new PonyPackage(context, errors, Packages.Dir("builtin"));
        }

        private static void PonyExample(string packageName)
        {
            var packageDir = Examples.Dir(packageName);

            var errors = new ErrorAccu();
            var context = new CompilerContext(Packages);
            var module = new PonyPackage(context, errors, packageDir);
        }

        private static void PonyParse(int skip, IEnumerable<FileRef> ponies)
        {
            int no = 0;
            int lines = 0;

            var errors = new ErrorAccu();

            foreach (var ponyFile in ponies)
            {
                no += 1;
                if (no <= skip)
                {
                    continue;
                }
                errors.Clear();
                if (!PonyParse(errors, ref lines, no, ponyFile))
                {
                    break;
                }
            }
            Console.WriteLine($"{lines} lines read");
            Console.WriteLine();
        }

        private static bool PonyParse(ErrorAccu errors, ref int lines, int no, FileRef ponyFile)
        {
            Console.WriteLine($"{no}. {ponyFile}");

            var source = Source.FromFile(ponyFile);
            lines += source.LineCount;

            var tokenizer = new PonyTokenizer(errors, source);

            try
            {
                tokenizer.Tokenize();
            }
            catch (JokeException joke)
            {
                joke.Error.Description.Describe(Console.Out);

                return false;
            }

            var parser = new PonyParser(errors, source, tokenizer.Tokens);

            try
            {
                var module = parser.Module();

                errors.Describe(Console.Out);

                return errors.NoError();
            }
            catch (JokeException joke)
            {
                joke.Error.Description.Describe(Console.Out);

                return false;
            }

            //var visitor = new TokenCoverageVisitor();
            //visitor.Visit(module);
            //Console.WriteLine($"{parser.Tokens.Count} :: {visitor.Set.Cardinality}");
            //stats.Update(module);
        }

        private static DirRef Temp => DirRef.ProjectDir().Up.Up.Dir("Temp");
        private static DirRef Packages => Temp.Dir("ponyc").Dir("packages");
        private static DirRef Examples => Temp.Dir("ponyc").Dir("examples");

        private static IEnumerable<FileRef> EnumerateBuiltinPonies()
        {
            return EnumeratePonies(Temp.Dir("ponyc").Dir("packages").Dir("builtin"));
        }

        private static IEnumerable<FileRef> EnumeratePackagePonies()
        {
            return EnumeratePonies(Temp.Dir("ponyc").Dir("packages"));
        }

        private static IEnumerable<FileRef> EnumerateAllPonies()
        {
            return EnumeratePonies(Temp.Dir("ponyc"), Temp.Dir("pony-source"));
        }

        private static IEnumerable<FileRef> EnumeratePonies(params DirRef[] roots)
        {
            foreach (var root in roots)
            {
                foreach (var pony in Directory.EnumerateFiles(root, "*.pony", SearchOption.AllDirectories))
                {
                    if (pony.Contains(@"\ponycc\test\fixtures\") ||
                        pony.Contains(@"\adv5.pony") ||
                        pony.Contains(@"\bench\bench_pg.pony") ||
                        pony.Contains(@"\examples\clisample.pony") ||
                        pony.Contains(@"\pony-stats\stats\test.pony") ||
                        pony.Contains(@"\pony-queue\queue.pony"))
                    {
                        continue;
                    }
                    yield return FileRef.From(pony);
                }
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
