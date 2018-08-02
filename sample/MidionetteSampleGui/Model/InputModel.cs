using Elgraiv.Midionette;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidionetteSampleGui.Model
{
    class InputModel
    {
        private MidionetteController _controller;
        private ObservableCollection<string> _deviceNames;
        public ReadOnlyObservableCollection<string> DeviceNames { get; }

        public InputModel()
        {
            _deviceNames = new ObservableCollection<string>();
            DeviceNames = new ReadOnlyObservableCollection<string>(_deviceNames);
        }

        public void RefreshDevices()
        {
            _deviceNames.Clear();
            foreach(var name in MidionetteController.GetConnectedDevices())
            {
                _deviceNames.Add(name);
            }
            
        }

        public void Connect(string deviceName)
        {
            _controller.OpenDevice(deviceName);
        }
    }
}
