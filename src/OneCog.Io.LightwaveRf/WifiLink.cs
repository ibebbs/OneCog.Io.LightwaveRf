using System;
using System.Threading.Tasks;

namespace OneCog.Io.LightwaveRf
{
    public interface IWifiLink : IDisposable
    {
        Task ConnectAsync();

        Task PairAsync();

        Task SendAsync(string command);

        ISocket Socket(uint room, uint device);

        IDimmer Dimmer(uint room, uint device);
    }

    public class WifiLink : IWifiLink, IDevice
    {
        private IConnection _connection;

        uint IDevice.Room
        {
            get { return 0; }
        }

        uint IDevice.Device
        {
            get { return 0; }
        }

        public WifiLink(IConnection connection)
        {
            _connection = connection;
        }

        public WifiLink(string ipAddress) : this(new Connection(ipAddress)) { }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public Task ConnectAsync()
        {
            return _connection.ConnectAsync();
        }

        public Task RegisterAsync()
        {
            return _connection.RegisterAsync();
        }

        public Task SendAsync(string command)
        {
            return _connection.SendAsync(command);
        }

        public async Task PairAsync()
        {
            await _connection.SendAsync(this, Command.Factory.Instance.Pair());
        }

        public ISocket Socket(uint room, uint device)
        {
            return new Socket(_connection, room, device);
        }

        public IDimmer Dimmer(uint room, uint device)
        {
            return new Dimmer(_connection, room, device);
        }
    }
}
