using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette
{
    public class MidionetteController:IDisposable
    {
        private Dictionary<string, InputDevice> _deviceMap=new Dictionary<string, InputDevice>();

        public IEnumerable<InputDevice> OpenedDevices {
            get
            {
                return _deviceMap.Values.Where(device => device.IsOpened);
            }
        }

        public bool OpenDevice(string name)
        {
            var device = GetOrCreate(name);
            var result = device.OpenMidiDevice();
            if (!result)
            {
                device.Dispose();
                return false;
            }
            _deviceMap.Add(device.Name, device);
            return true;
        }

        public void SetReceiverToControlChange(string deviceName,byte channel, byte type, IValueReceiver receiver)
        {
            var device = GetOrCreate(deviceName);
            device.SetReceiverToControlChange(channel, type, receiver);
        }

        private InputDevice GetOrCreate(string deviceName)
        {
            if (!_deviceMap.TryGetValue(deviceName, out InputDevice device))
            {
                device = new InputDevice(deviceName);
                _deviceMap.Add(deviceName, device);
            }
            return device;
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
