namespace Joke.Front.Err
{
    public class Error : IError
    {
        public Error(Severity severity, ErrNo no, IDescription description)
        {
            Severity = severity;
            No = no;
            Description = description;
        }

        public Severity Severity { get; }
        public ErrNo No { get; }
        public IDescription Description { get; }
    }
}
