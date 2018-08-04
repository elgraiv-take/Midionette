using System;
using System.Collections.Generic;
using System.Linq;

namespace Elgraiv.Midionette
{
    public class MidionetteInputDevice : IDisposable
    {

        private Device.MidiInput _midiDevice;
        public string Name { get; }
        public bool IsOpened { get; private set; } = false;

        private Dictionary<MidiMessageTarget, IValueReceiver> _receiverMap;

        public IEnumerable<MidiMessageTarget> NonAssignedControlChange
        {
            get
            {
                lock (_receiverMap)
                {
                    foreach (var pair in _receiverMap)
                    {
                        if (pair.Value is NullValueReceiver)
                        {
                            yield return pair.Key;
                        }
                    }
                }
            }
        }

        public event EventHandler<MessageTypeEventArgs> NewControlChangedDetected;

        internal MidionetteInputDevice(string name)
        {
            Name = name;
            _midiDevice = new Device.MidiInput();
            _midiDevice.MidiDataReceived += OnMidiDataReceived;
            _receiverMap = new Dictionary<MidiMessageTarget, IValueReceiver>();
        }

        internal bool OpenMidiDevice()
        {
            var devices = Device.MidiInput.GetDevices();
            var deviceInfo = devices.Where((info) => info.Name.Equals(Name)).FirstOrDefault();
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

        private enum CommandType
        {
            Unknown,
            ControlChange,
            NoteOn,
            NoteOff,
        }

        private void OnMidiDataReceived(object sender, Device.MidiDataEventArgs e)
        {
            //ひとまず単純なコントロールチェンジ
            var type = GetCommandType(e.Data.Status, out byte channel);
            switch (type)
            {
                case CommandType.ControlChange:
                    lock (_receiverMap)
                    {
                        if (_receiverMap.TryGetValue(new MidiMessageTarget(channel, e.Data.MidiData0), out IValueReceiver receiver))
                        {
                            receiver.OnValueReceived(e.Data.MidiData1);
                        }
                        else
                        {
                            _receiverMap.Add(new MidiMessageTarget(channel, e.Data.MidiData0), NullValueReceiver.Value);
                            NewControlChangedDetected?.Invoke(this, new MessageTypeEventArgs(channel, e.Data.MidiData0));
                        }
                    }
                    break;
                default:
                    break;
            }

        }
        private static readonly List<(byte status, CommandType type)> s_commandTypeList = new List<(byte status, CommandType type)>()
        {
            (MidiConstant.CommandIdControlChange,CommandType.ControlChange),
            (MidiConstant.CommandIdNoteOn,CommandType.NoteOn),
            (MidiConstant.CommandIdNoteOff,CommandType.NoteOff),
        };
        private CommandType GetCommandType(byte status, out byte channel)
        {
            foreach (var command in s_commandTypeList)
            {
                if (status >= command.status && status < command.status + MidiConstant.MidiMaxChannelNum)
                {
                    channel = (byte)(status - command.status);
                    return command.type;
                }
            }
            channel = 0;
            return CommandType.Unknown;
        }

        public void SetReceiverToControlChange(byte channel, byte type, IValueReceiver receiver)
        {
            if (!(channel < MidiConstant.MidiMaxChannelNum))
            {
                throw new ArgumentException("Channel MUST be 0~15", nameof(channel));
            }
            lock (_receiverMap)
            {
                _receiverMap[new MidiMessageTarget(channel, type)] = receiver;
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
