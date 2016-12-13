using OneCog.Net;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OneCog.Io.LightwaveRf
{
    public interface IConnection : IDisposable
    {
        Task ConnectAsync();
        Task<IResponse> SendAsync(IDevice device, ICommand command);
    }

    internal class Connection : IConnection
    {
        private readonly string _ipAddress;
        private readonly Command.ICounter _commandCounter;
        private readonly Command.IFormatter _commandFormatter;
        private readonly Command.ISerializer _commandSerializer;
        private readonly Response.IParser _responseParser;
        private readonly Response.ISerializer _responseSerializer;

        private UdpClient _udpClient;

        public Connection(string ipAddress, Command.ICounter commandCounter = null, Command.IFormatter commandFormatter = null, Command.ISerializer commandSerializer = null, Response.IParser responseParser = null, Response.ISerializer responseSerializer = null)
        {
            _ipAddress = ipAddress;
            _commandCounter = commandCounter ?? new Command.Counter();
            _commandFormatter = commandFormatter ?? new Command.Formatter();
            _commandSerializer = commandSerializer ?? new Command.Serializer();
            _responseParser = responseParser ?? new Response.Parser();
            _responseSerializer = responseSerializer ?? new Response.Serializer();
        }

        public void Dispose()
        {
            if (_udpClient != null)
            {
                _udpClient.Dispose();
                _udpClient = null;
            }
        }

        public async Task ConnectAsync()
        {
            _udpClient = new UdpClient();

            string localIpAddress = Util.LocalHostNames().FirstOrDefault();

            await _udpClient.ConnectAsync(new UriBuilder("udp", localIpAddress, 9761).Uri, new UriBuilder("udp", _ipAddress, 9760).Uri, CancellationToken.None);
        }

        public async Task<IResponse> SendAsync(IDevice device, ICommand command)
        {
            uint id = _commandCounter.Next();
            byte[] sendBuffer = _commandSerializer.Serialize(_commandFormatter.Format(id, device, command));

            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            byte[] receiveBuffer = new byte[1024];
            Task<int> responseTask = _udpClient.ReadAsync(receiveBuffer, cts.Token);

            await _udpClient.WriteAsync(sendBuffer, cts.Token);

            int receivedBytes = 0;
            while ((receivedBytes = await responseTask) != 0)
            {
                IResponse response = _responseParser.Parse(_responseSerializer.Deserialize(receiveBuffer.Take(receivedBytes).ToArray())).FirstOrDefault();

                if (response != null && response.Id.Equals(id))
                {
                    return response;
                }
                else
                {
                    responseTask = _udpClient.ReadAsync(receiveBuffer, cts.Token);
                }
            }

            throw new OperationCanceledException();
        }
    }
}
