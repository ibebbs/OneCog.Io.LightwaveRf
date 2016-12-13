namespace OneCog.Io.LightwaveRf.Response
{
    public interface IFailure : IResponse
    {
        string Error { get; }
    }

    internal class Failure : IFailure
    {
        public Failure(uint id, string error)
        {
            Id = id;
            Error = error;
        }

        public uint Id { get; private set; }
        public string Error { get; private set; }
    }
}
