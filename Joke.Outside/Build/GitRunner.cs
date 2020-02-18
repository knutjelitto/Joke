using System;

namespace Joke.Outside.Build
{
    public static class GitRunner
    {
        public static void Ensure(string url, DirRef repository)
        {
            var runner = new Runner();

            var cd = Environment.CurrentDirectory;

            if (repository.Dir(".git").Exists)
            {
                Environment.CurrentDirectory = repository;

                runner.Run("git", "git", "pull --progress");
            }
            else
            {

                Environment.CurrentDirectory = repository.Up;

                runner.Run("git", "git", $"clone {url} --progress");

            }

            Environment.CurrentDirectory = cd;
        }
    }
}
