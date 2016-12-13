namespace OneCog.Io.LightwaveRf.Command
{
    public interface ICounter
    {
        uint Next();
    }

    internal class Counter : ICounter
    {
        private object _lock = new object();
        private uint _counter = 0;

        public uint Next()
        {
            lock (_lock)
            {
                return _counter++;
            }
        }
     }
}
