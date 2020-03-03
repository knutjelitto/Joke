namespace Joke.Joke.Err
{
    public interface IErrors : IDescription
    {
        void Add(IError error);
    }
}
