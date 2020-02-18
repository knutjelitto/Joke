namespace Joke.Front.Pony.Err
{
    public class Message
    {
        public Message(ErrorKind kind, string msg)
        {
            Kind = kind;
            Msg = msg;
        }

        public ErrorKind Kind { get; }
        public string Msg { get; }
    }
}
