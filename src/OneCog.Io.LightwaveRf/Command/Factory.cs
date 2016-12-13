namespace OneCog.Io.LightwaveRf.Command
{
    public interface IFactory
    {
        ICommand Pair();
        ICommand PowerOn();
        ICommand PowerOff();
        ICommand SetDimmerLevel(byte level);
    }

    internal class Factory : IFactory
    {
        public static readonly IFactory Instance = new Factory();

        public ICommand Pair()
        {
            return new Pair();
        }

        public ICommand PowerOff()
        {
            return new Power(false);
        }

        public ICommand PowerOn()
        {
            return new Power(false);
        }

        public ICommand SetDimmerLevel(byte level)
        {
            return new SetDimmerLevel(level);
        }
    }
}
