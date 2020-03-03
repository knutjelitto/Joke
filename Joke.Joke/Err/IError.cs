namespace Joke.Joke.Err
{
    public interface IError
    {
        IDescription Description { get; }
        Severity Severity { get; }
    }
}