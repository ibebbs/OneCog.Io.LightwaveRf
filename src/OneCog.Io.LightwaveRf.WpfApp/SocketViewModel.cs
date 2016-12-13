using Caliburn.Micro;

namespace OneCog.Io.LightwaveRf.WpfApp
{
    public class SocketViewModel : PropertyChangedBase, IDeviceViewModel
    {
        public SocketViewModel(ISocket socket)
        {
            Room = socket.Room;
            Device = socket.Device;
            PowerOn = new DelegateCommand(_ => true, _ => socket.On());
            PowerOff = new DelegateCommand(_ => true, _ => socket.Off());
        }

        public uint Room { get; private set; }
        public uint Device { get; private set; }
        public DelegateCommand PowerOn { get; private set; }
        public DelegateCommand PowerOff { get; private set; }
    }
}
