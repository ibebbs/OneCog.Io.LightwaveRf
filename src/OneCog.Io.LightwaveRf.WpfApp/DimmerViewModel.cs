using Caliburn.Micro;
using System;

namespace OneCog.Io.LightwaveRf.WpfApp
{
    public class DimmerViewModel : PropertyChangedBase, IDeviceViewModel
    {
        private readonly IDimmer _dimmer;
        private byte _value = 200;

        public DimmerViewModel(IDimmer dimmer)
        {
            _dimmer = dimmer;

            Room = dimmer.Room;
            Device = dimmer.Device;
            
            PowerOn = new DelegateCommand(_ => true, _ => dimmer.On());
            PowerOff = new DelegateCommand(_ => true, _ => dimmer.Off());
        }

        public uint Room { get; private set; }
        public uint Device { get; private set; }

        public byte Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value = value;

                    _dimmer.SetBrightness(_value);
                    NotifyOfPropertyChange(() => Value);
                }
            }
        }

        public DelegateCommand PowerOn { get; private set; }
        public DelegateCommand PowerOff { get; private set; }

    }
}
