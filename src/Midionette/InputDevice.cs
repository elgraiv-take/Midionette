using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette
{
    public class InputDevice
    {
        private Device.MidiInput _midiDevice;
        public string Name
        {
            get
            {
                return _midiDevice.Name;
            }
        }

        internal InputDevice()
        {
            _midiDevice = new Device.MidiInput();
            _midiDevice.MidiDataReceived += OnMidiDataReceived;
        }

        private void OnMidiDataReceived(object sender, Device.MidiDataEventArgs e)
        {

        }

        public void AddReceiverToControlChange(int channel,int type,IValueReceiver receiver)
        {

        }

    }
}
