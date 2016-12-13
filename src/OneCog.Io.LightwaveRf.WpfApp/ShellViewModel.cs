using Caliburn.Micro;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OneCog.Io.LightwaveRf.WpfApp
{
    public class ShellViewModel : Screen, IShell
    {
        private readonly ObservableCollection<IDeviceViewModel> _devices;

        private WifiLink _wifiLink;

        private string _ipAddress = "192.168.1.59";
        private int _room;
        private int _device;
        private bool _isConnected;

        public ShellViewModel()
        {
            _devices = new ObservableCollection<IDeviceViewModel>();

            ConnectCommand = new DelegateCommand(_ => !_isConnected, async _ => await Connect());
            AddSocketCommand = new DelegateCommand(_ => _isConnected, _ => _devices.Add(new SocketViewModel(_wifiLink.Socket((uint)_room, (uint)_device))));
            AddDimmerCommand = new DelegateCommand(_ => _isConnected, _ => _devices.Add(new DimmerViewModel(_wifiLink.Dimmer((uint)_room, (uint)_device))));
        }

        private async Task Connect()
        {
            _wifiLink = new WifiLink(_ipAddress);
            await _wifiLink.ConnectAsync();
            //await _wifiLink.PairAsync();
        }

        public IEnumerable<IDeviceViewModel> Devices
        {
            get { return _devices; }
        }

        public string IPAddress
        {
            get { return _ipAddress; }
            set
            {
                if (value != _ipAddress)
                {
                    _ipAddress = value;

                    NotifyOfPropertyChange(() => IPAddress);
                }
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            private set
            {
                if (value != _isConnected)
                {
                    _isConnected = value;

                    NotifyOfPropertyChange(() => _isConnected);
                    ConnectCommand.RaiseCanExecuteChanged();
                    AddSocketCommand.RaiseCanExecuteChanged();
                    AddDimmerCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public int Room
        {
            get { return _room; }
            set
            {
                if (value != _room)
                {
                    _room = value;

                    NotifyOfPropertyChange(() => Room);
                }
            }
        }

        public int Device
        {
            get { return _device; }
            set
            {
                if (value != _device)
                {
                    _device = value;

                    NotifyOfPropertyChange(() => Device);
                }
            }
        }

        public DelegateCommand ConnectCommand { get; private set; }
        public DelegateCommand AddSocketCommand { get; private set; }
        public DelegateCommand AddDimmerCommand { get; private set; }
    }
}