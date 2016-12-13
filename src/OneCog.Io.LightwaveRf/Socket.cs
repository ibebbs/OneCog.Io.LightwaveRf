using System.Threading.Tasks;

namespace OneCog.Io.LightwaveRf
{
    public interface ISocket : IDevice
    {
        Task On();
        Task Off();
    }

    internal class Socket : ISocket
    {
        private readonly IConnection _connection;

        public Socket(IConnection connection, uint room, uint device)
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

        public uint Device { get; private set; }

        public uint Room { get; private set; }
    }
}
