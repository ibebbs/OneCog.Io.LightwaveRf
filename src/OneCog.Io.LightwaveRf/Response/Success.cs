namespace OneCog.Io.LightwaveRf.Response
{
    public interface ISuccess : IResponse
    {

    }

    internal class Success : ISuccess
    {
        public Success(uint id)
        {
            Id = id;
        }

        public uint Id { get; private set; }
    }
}
