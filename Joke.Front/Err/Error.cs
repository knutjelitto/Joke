namespace Joke.Front.Err
{
    public class Error
    {
        public Error(Severity severity, IDescription description)
        {
            Severity = severity;
            Description = description;
        }

        public Severity Severity { get; }
        public IDescription Description { get; }
    }
}
