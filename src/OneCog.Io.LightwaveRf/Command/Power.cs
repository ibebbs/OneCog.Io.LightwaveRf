namespace OneCog.Io.LightwaveRf.Command
{
    internal class Power : ICommand
    {
        public Power(bool on)
        {
            String = on ? "F1" : "F0";
        }

        public string String { get; private set; }
    }
}
