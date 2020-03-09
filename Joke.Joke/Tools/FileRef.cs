using System.Diagnostics;

using IOFile = System.IO.File;

namespace Joke.Joke.Tools
{
    public class FileRef
    {
        private FileRef(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public bool OnPladde => IOFile.Exists(this);

        public bool Exists()
        {
            return IOFile.Exists(this);
        }

        public string GetContent()
        {
            return IOFile.ReadAllText(this);
        }

        public bool NewerThan(FileRef dst)
        {
            Debug.Assert(IOFile.Exists(this));

            return !IOFile.Exists(dst) || IOFile.GetLastWriteTime(this) > IOFile.GetLastWriteTime(dst);
        }

        public FileRef ForceAddExtension(string newExtension)
        {
            return new FileRef(Path + newExtension);
        }

        public FileRef ForceChangeExtension(string newExtension)
        {
            return new FileRef(System.IO.Path.ChangeExtension(Path, newExtension));
        }

        public static FileRef From(string name)
        {
            return new FileRef(name);
        }

        public FileRef AddExtension(string newExtension)
        {
            newExtension = newExtension.StartsWith(".") ? newExtension : "." + newExtension;

            return ForceAddExtension(newExtension);
        }

        protected FileRef ChangeExtension(string newExtension)
        {
            newExtension = newExtension.StartsWith(".") ? newExtension : "." + newExtension;

            if (System.IO.Path.GetExtension(Path) != newExtension)
            {
                return ForceChangeExtension(newExtension);
            }

            return this;
        }

        public FileRef With(string extension)
        {
            return ChangeExtension(extension);
        }

        public FileRef Add(string extension)
        {
            return AddExtension(extension);
        }

        public string BaseName => System.IO.Path.GetFileNameWithoutExtension(Path);
        public string FileName => System.IO.Path.GetFileName(Path);
        public string Directory => System.IO.Path.GetDirectoryName(Path) ?? string.Empty;


        public static implicit operator string(FileRef file)
        {
            return file.Path;
        }

        public override string ToString()
        {
            return Path;
        }

        private FileRef EnsureDirectory()
        {
            var dirPath = System.IO.Path.GetDirectoryName(Path);
            if (!string.IsNullOrEmpty(dirPath) && !System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
            return this;
        }
    }
}
