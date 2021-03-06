﻿using System;

using Joke.Front.Err;
using Joke.Outside;

namespace Joke.Compiler.Compile
{
    public class CompileContext
    {
        public CompileContext(Errors errors, DirRef packagesDir)
        {
            Errors = errors;
            PackagesDir = packagesDir;
            Logger = new IndentWriter(Console.Out, " .. ");
        }

        public Errors Errors { get; }
        public DirRef PackagesDir { get; }
        public IndentWriter Logger { get; }

        public DirRef FindPackageDir(string packageName)
        {
            var packageDir = PackagesDir.Dir(packageName);
            return packageDir;
        }
    }
}
