using System.Threading.Tasks;

namespace OneCog.Io.LightwaveRf
{
    public interface IDimmer : IDevice
    {
        Task On();
        Task Off();
        Task SetBrightness(byte level);
    }

    internal class Dimmer : IDimmer
    {
        private readonly IConnection _connection;

        public Dimmer(IConnection connection, uint room, uint device)
        {
            _connection = connection;

            Room = room;
            Device = device;
        }

        public async Task On()
        {
            await _connection.SendAsync(this, Command.Factory.Instance.PowerOn());
        }

        public async Task Off()
        {
            await _connection.SendAsync(this, Command.Factory.Instance.PowerOff());
        }

        public async Task SetBrightness(byte level)
        {
            await _connection.SendAsync(this, Command.Factory.Instance.SetDimmerLevel(level));
        }

        public uint Device { get; private set; }

        public uint Room { get; private set; }
    }
}
