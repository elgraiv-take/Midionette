using Elgraiv.Midionette;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MidionetteSampleGui.Model
{
    class InputModel
    {
        public class ControlChangeInfo
        {
            public string DeviceName { get; }
            public byte Channel { get; }
            public byte Id { get; }
            public ControlChangeInfo(string deviceName,byte channel,byte id)
            {
                DeviceName = deviceName;
                Channel = channel;
                Id = id;
            }
        }

        private MidionetteController _controller;
        private ObservableCollection<string> _deviceNames;
        public ReadOnlyObservableCollection<string> DeviceNames { get; }

        private ObservableCollection<MidionetteInputDevice> _connectedDevices;
        public ReadOnlyObservableCollection<MidionetteInputDevice> ConnectedDevices { get; }

        private ObservableCollection<ControlChangeInfo> _controlChanges;
        public ReadOnlyObservableCollection<ControlChangeInfo> ControlChanges { get; }

        public InputModel()
        {
            _deviceNames = new ObservableCollection<string>();
            DeviceNames = new ReadOnlyObservableCollection<string>(_deviceNames);
            _connectedDevices = new ObservableCollection<MidionetteInputDevice>();
            ConnectedDevices = new ReadOnlyObservableCollection<MidionetteInputDevice>(_connectedDevices);
            _controlChanges = new ObservableCollection<ControlChangeInfo>();
            ControlChanges = new ReadOnlyObservableCollection<ControlChangeInfo>(_controlChanges);
            _controller = new MidionetteController();

            BindingOperations.EnableCollectionSynchronization(_controlChanges, new object());
            BindingOperations.EnableCollectionSynchronization(ControlChanges, new object());
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
            var device=_controller.OpenDevice(deviceName);
            if (device != null)
            {
                if (!_connectedDevices.Contains(device))
                {
                    _connectedDevices.Add(device);
                    device.NewControlChangedDetected += Device_NewControlChangedDetected;
                }
            }
        }

        private void Device_NewControlChangedDetected(object sender, MessageTypeEventArgs e)
        {
            if(sender is MidionetteInputDevice inputDevice)
            {

                _controlChanges.Add(new ControlChangeInfo(inputDevice.Name, e.Channel, e.Param));
            }
        }

        public void AssignControlChangeToReceiver(ControlChangeInfo target,IValueReceiver receiver)
        {
            _controller.SetReceiverToControlChange(target.DeviceName, target.Channel, target.Id, receiver);
        }
    }
}
