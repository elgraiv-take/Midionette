﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette
{
    public class InputDevice : IDisposable
    {

        private Device.MidiInput _midiDevice;
        public string Name { get; }
        public bool IsOpened { get; private set; } = false;

        private Dictionary<byte, IValueReceiver>[] _receiverMap;
        
        public IEnumerable<(byte channel,byte type)> NonAssignedControlChange
        {
            get
            {
                for (byte i = 0; i < MidiConstant.MidiMaxChannelNum; i++)
                {
                    lock (_receiverMap[i])
                    {
                        foreach (var pair in _receiverMap[i])
                        {
                            if (pair.Value is NullValueReceiver)
                            {
                                yield return (i, pair.Key);
                            }
                        }
                    }
                }
            }
        }

        internal InputDevice(string name)
        {
            Name = name;
            _midiDevice = new Device.MidiInput();
            _midiDevice.MidiDataReceived += OnMidiDataReceived;
            _receiverMap = new Dictionary<byte, IValueReceiver>[MidiConstant.MidiMaxChannelNum];
            for(var i = 0; i < MidiConstant.MidiMaxChannelNum; i++)
            {
                _receiverMap[i] = new Dictionary<byte, IValueReceiver>();
            }
        }

        internal bool OpenMidiDevice()
        {
            var devices = Device.MidiInput.GetDevices();
            var deviceInfo=devices.Where((info) => info.Name.Equals(Name)).FirstOrDefault();
            if (string.IsNullOrEmpty(deviceInfo.Name))
            {
                return false;
            }
            if (!_midiDevice.Initialize(deviceInfo.Id))
            {
                return false;
            }
            if (!_midiDevice.Start())
            {
                return false;
            }
            IsOpened = true;
            return true;
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
                lock (_receiverMap[channel])
                {
                    if (_receiverMap[channel].TryGetValue(e.Data.MidiData0, out IValueReceiver receiver))
                    {
                        receiver.OnValueReceived(e.Data.MidiData1);
                    }
                    else
                    {
                        _receiverMap[channel].Add(e.Data.MidiData1, NullValueReceiver.Value);
                    }
                }
                
                
            }
        }

        public void SetReceiverToControlChange(byte channel,byte type,IValueReceiver receiver)
        {
            if (!(channel < MidiConstant.MidiMaxChannelNum))
            {
                throw new ArgumentException("Channel MUST be 0~15",nameof(channel));
            }
            lock (_receiverMap[channel])
            {
                _receiverMap[channel].Add(type, receiver);
            }
                
        }

        public void ClearNonAssigneed()
        {

        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _midiDevice.Dispose();
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
