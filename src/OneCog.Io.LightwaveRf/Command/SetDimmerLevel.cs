namespace OneCog.Io.LightwaveRf.Command
{
    internal class SetDimmerLevel : ICommand
    {
        public SetDimmerLevel(byte level)
        {
            String = $"FdP{level.ToString("x")}";
        }

        public string String { get; private set; }
    }
}
