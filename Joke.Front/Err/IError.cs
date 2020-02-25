namespace Joke.Front.Err
{
    public interface IError
    {
        IDescription Description { get; }
        Severity Severity { get; }
    }
}