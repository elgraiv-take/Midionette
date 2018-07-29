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

        private Dictionary<byte, IValueReceiver>[] _receiverMap;
        
        public IEnumerable<(byte channel,byte type)> NonAssignedControlChange
        {
            get
            {
                for (byte i = 0; i < MidiConstant.MidiMaxChannelNum; i++)
                {
                    foreach(var pair in _receiverMap[i])
                    {
                        if (pair.Value is NullValueReceiver)
                        {
                            yield return (i, pair.Key);
                        }
                    }
                }
            }
        }

        internal InputDevice()
        {
            _midiDevice = new Device.MidiInput();
            _midiDevice.MidiDataReceived += OnMidiDataReceived;
            _receiverMap = new Dictionary<byte, IValueReceiver>[MidiConstant.MidiMaxChannelNum];
            for(var i = 0; i < MidiConstant.MidiMaxChannelNum; i++)
            {
                _receiverMap[i] = new Dictionary<byte, IValueReceiver>();
            }
        }

        private void OnMidiDataReceived(object sender, Device.MidiDataEventArgs e)
        {
            //ひとまず単純なコントロールチェンジ
            if (
                e.Data.Status >= MidiConstant.CommandIdControlChange &&
                e.Data.Status < MidiConstant.CommandIdControlChange + MidiConstant.MidiMaxChannelNum
                )
            {
                var channel = e.Data.Status - MidiConstant.CommandIdControlChange;

                if(_receiverMap[channel].TryGetValue(e.Data.MidiData0,out IValueReceiver receiver))
                {
                    receiver.OnValueReceived(e.Data.MidiData1);
                }
                else
                {
                    _receiverMap[channel].Add(e.Data.MidiData1, NullValueReceiver.Value);
                }
                
            }
        }

        public void SetReceiverToControlChange(byte channel,byte type,IValueReceiver receiver)
        {
            if (!(channel < MidiConstant.MidiMaxChannelNum))
            {
                throw new ArgumentException("Channel MUST be 0~15",nameof(channel));
            }
            _receiverMap[channel].Add(type, receiver);
        }

        public void ClearNonAssigneed()
        {

        }

    }
}
