using System;
using System.Collections.Generic;
using System.Linq;

using IOPath = System.IO.Path;
using IODirectory = System.IO.Directory;

namespace Joke.Joke.Tools
{
    public class DirRef
    {
        private DirRef(string path)
        {
            Path = path;
            //EnsureDirectory();
        }

        public string Path { get; }
        public string BaseName => IOPath.GetFileNameWithoutExtension(Path);
        public string FileName => IOPath.GetFileName(Path);
        public string Directory => IOPath.GetDirectoryName(Path) ?? ".";

        public FileRef File(string fileName)
        {
            return FileRef.From(IOPath.Combine(Path, fileName).Replace("\\", "/"));
        }

        public DirRef Dir(string dirName)
        {
            return From(IOPath.Combine(Path, dirName).Replace("\\", "/"));
        }

        public DirRef Up => From(Directory);

        public static DirRef From(string name)
        {
            return new DirRef(name);
        }

        public static implicit operator string(DirRef dir)
        {
            return dir.Path;
        }

        public override string ToString()
        {
            return Path;
        }

        public bool Exists => IODirectory.Exists(Path);

        public IEnumerable<FileRef> Files(string searchPattern = "*.*", bool recurse = false)
        {
            foreach (var fileName in IODirectory.EnumerateFiles(Path, searchPattern, recurse ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly))
            {
                yield return File(fileName);
            }
        }

        public void Ensure()
        {
            var dirPath = Path;
            if (!string.IsNullOrEmpty(dirPath) && !IODirectory.Exists(dirPath))
            {
                IODirectory.CreateDirectory(dirPath);
            }
        }


        public static DirRef ProjectDir()
        {
            string? cwd = Environment.CurrentDirectory;

            while (IODirectory.Exists(cwd) && !IODirectory.EnumerateFiles(cwd, "*.csproj").Any())
            {
                cwd = IOPath.GetDirectoryName(cwd);
            }

            var projectDir = From((cwd ?? string.Empty).Replace("\\", "/"));

            return projectDir;
        }
    }
}
