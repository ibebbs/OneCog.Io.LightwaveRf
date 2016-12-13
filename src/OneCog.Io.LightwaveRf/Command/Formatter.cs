namespace OneCog.Io.LightwaveRf.Command
{
    public interface IFormatter
    {
        string Format(uint id, IDevice device, ICommand command);
    }

    internal class Formatter : IFormatter
    {
        public string Format(uint id, IDevice device, ICommand command)
        {
            return $"{id},!R{device.Room}D{device.Room}{command.String}";
        }
    }
}
