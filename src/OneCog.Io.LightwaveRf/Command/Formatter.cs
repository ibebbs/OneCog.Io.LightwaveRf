namespace OneCog.Io.LightwaveRf.Command
{
    public interface IFormatter
    {
        string Format(uint id, IDevice device, ICommand command);
        string Format(uint id, string command);
    }

    internal class Formatter : IFormatter
    {
        public string Format(uint id, IDevice device, ICommand command)
        {
            return $"{id:D3},!R{device.Room}D{device.Room}{command.String}";
        }

        public string Format(uint id, string command)
        {
            return $"{id:D3},!{command}";
        }
    }
}
