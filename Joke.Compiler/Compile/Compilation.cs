﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Joke.Compiler.Tree;
using Joke.Front;
using Joke.Front.Err;
using Joke.Front.Pony.Lexing;
using Joke.Front.Pony.ParseTree;
using Joke.Front.Pony.Parsing;
using Joke.Outside;

namespace Joke.Compiler.Compile
{
    public class Compilation
    {
        public readonly LookupList<string, Package> Packages = new LookupList<string, Package>();
        private const string Builtin = "builtin";
        private const string Ponies = "*.pony";

        public Compilation(CompileContext context)
        {
            Context = context;
        }

        public CompileContext Context { get; }
        public Errors Errors => Context.Errors;
        public IndentWriter Logger => Context.Logger;

        public Package PreparePackage(DirRef packageDir, string name, bool isBuiltin = false)
        {
            var builtin = isBuiltin ? null : UsePackage(Builtin);

            var package = new Package(Context, packageDir, name, builtin);

            Load(package);

            LoadClosure();

            return package;
        }

        private void Load(Package package)
        {
            Logger.WriteLine($"load package {package.Name}");

            foreach (var unitFile in package.PackageDir.Files(Ponies))
            {
                Logger.WriteLine($" .. load unit {unitFile.FileName}");
                var unit = LoadUnit(package, unitFile);
                package.Units.Add(unitFile, unit);
                unit.DiscoverMembers();
            }
        }

        private void LoadClosure()
        {
            var i = 0;
            while (i < Packages.Count)
            {
                Load(Packages[i]);
                i += 1;
            }
        }

        private Package UsePackage(string packageName)
        {
            var packageDir = Context.FindPackageDir(packageName);

            if (!Packages.TryGetValue(packageDir, out var package))
            {
                package = new Package(Context, packageDir, packageName);
                Packages.Add(packageDir, package);
            }

            return package;
        }

        private Unit LoadUnit(Package package, FileRef unitFile)
        {
            var source = Source.FromFile(unitFile);
            var tokenizer = new PonyTokenizer(Errors, source);
            tokenizer.Tokenize();
            var parser = new PonyParser(Errors, source, tokenizer.Tokens);
            var ptUnit = parser.Unit();
            var unit = new Unit(ptUnit, unitFile, package);
            foreach (var use in unit.Source.Uses)
            {
                switch (use)
                {
                    case PtUseUri useUri:
                        unit.Uses.Add(DecodeUseUri(useUri));
                        break;
                    case PtUseFfi useFfi:
                        unit.Uses.Add(DecodeUseFfi(useFfi));
                        break;
                    default:
                        throw new System.InvalidOperationException();
                }
            }
            return unit;
        }

        private IUse DecodeUseUri(PtUseUri use)
        {
            var uri = use.Uri.Value;

            var parts = uri.Split(':');

            Debug.Assert(parts.Length == 1 || parts.Length == 2);

            var schema = parts.Length == 1 ? "package" : parts[0];
            var value = parts.Length == 1 ? parts[0] : parts[1];

            Debug.Assert(schema.Length > 0 && value.Length > 0);

            var alias = use.Name?.Value;

            switch (schema)
            {
                case "package":
                    var package = UsePackage(uri);
                    return new UsePackage(use, alias, value, package);
                case "lib":
                    if (alias != null)
                    {
                        Errors.Help.Add(
                            use.Span,
                            ErrNo.Err003,
                            $"nonsense alias in ``use \"lib:{value}\"´´");
                    }
                    return new UseLib(use, value);
            }

            throw new System.InvalidOperationException();
        }

        private IUse DecodeUseFfi(PtUseFfi use)
        {
            if (use.Name != null) throw new System.NotImplementedException();
            if (use.Partial) throw new System.NotImplementedException();

            string name;
            if (use.FfiName.Name is PtString str)
            {
                name = str.Value;
            }
            else if (use.FfiName.Name is PtIdentifier id)
            {
                name = id.Value;
            }
            else
            {
                throw new System.InvalidOperationException();
            }

            if (use.TypeArguments.Arguments.Count != 1)
            {
                Errors.Help.Add(
                    use.TypeArguments.Span,
                    ErrNo.Err005,
                    "return of a foreign function call must have exactly one type");
            }

            var result = new AnyType(use.TypeArguments.Arguments[0]);

            var items = new List<Parameter>();
            bool ellipsis = false;
            foreach (var prm in use.Parameters.Items)
            {
                if (ellipsis)
                {
                    Errors.Help.Add(
                        use.Parameters.Items.OfType<PtEllipsisParameter>().First().Span,
                        ErrNo.Err004,
                        "ellipis ``...´´ must be last in foreign function declaration");
                }
                switch (prm)
                {
                    case PtEllipsisParameter ell:
                        ellipsis = true;
                        break;
                    case PtRegularParameter reg:
                        items.Add(new Parameter(reg, reg.Name.Value, new AnyType(reg.Type), reg.Value == null ? null : new AnyExpression(reg.Value)));
                        break;
                }
            }

            var parameters = new ForeignParameters(use.Parameters, items, ellipsis);

            var function = new ForeignFunctionDeclaration(use, name, parameters, result);

            return new UseForeignFunction(use, function);
        }
    }
}
