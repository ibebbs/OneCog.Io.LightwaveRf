using System.ComponentModel;

namespace OneCog.Io.LightwaveRf.WpfApp
{
    public interface IDeviceViewModel : INotifyPropertyChanged
    {
        uint Room { get; }
        uint Device { get; }
    }
}