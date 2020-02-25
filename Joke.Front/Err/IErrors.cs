namespace Joke.Front.Err
{
    public interface IErrors : IDescription
    {
        void Add(IError error);
    }
}
