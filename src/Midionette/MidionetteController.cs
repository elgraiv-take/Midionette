using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette
{
    public class MidionetteController:IDisposable
    {
        private Dictionary<string, MidionetteInputDevice> _deviceMap=new Dictionary<string, MidionetteInputDevice>();

        public IEnumerable<MidionetteInputDevice> OpenedDevices {
            get
            {
                return _deviceMap.Values.Where(device => device.IsOpened);
            }
        }

        public MidionetteInputDevice OpenDevice(string name)
        {
            var device = GetOrCreate(name);
            if (device.IsOpened)
            {
                return device;
            }
            var result = device.OpenMidiDevice();
            if (!result)
            {
                device.Dispose();
                return null;
            }
            return device;
        }

        public void SetReceiverToControlChange(string deviceName,byte channel, byte type, IValueReceiver receiver)
        {
            var device = GetOrCreate(deviceName);
            device.SetReceiverToControlChange(channel, type, receiver);
        }

        private MidionetteInputDevice GetOrCreate(string deviceName)
        {
            if (!_deviceMap.TryGetValue(deviceName, out MidionetteInputDevice device))
            {
                device = new MidionetteInputDevice(deviceName);
                _deviceMap.Add(deviceName, device);
            }
            return device;
        }

        public static ReadOnlyCollection<string> GetConnectedDevices()
        {
            var devices = new List<string>(Device.MidiInput.GetDevices().Select(device => device.Name));
            return devices.AsReadOnly();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach(var device in _deviceMap.Values)
                    {
                        device?.Dispose();
                    }
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
