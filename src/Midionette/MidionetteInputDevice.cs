using Elgraiv.Midionette.Internal;
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

        private Dictionary<MidiMessageTarget, IValueReceiver> _valueReceiverMap;
        private Dictionary<MidiMessageTarget, IKeyReceiver> _keyReceiverMap;

        public IEnumerable<MidiMessageTarget> NonAssignedControlChange
        {
            get
            {
                lock (_valueReceiverMap)
                {
                    foreach (var pair in _valueReceiverMap)
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
        public event EventHandler<MessageTypeEventArgs> NewNoteDetected;

        internal MidionetteInputDevice(string name)
        {
            Name = name;
            _midiDevice = new Device.MidiInput();
            _midiDevice.MidiDataReceived += OnMidiDataReceived;
            _valueReceiverMap = new Dictionary<MidiMessageTarget, IValueReceiver>();
            _keyReceiverMap = new Dictionary<MidiMessageTarget, IKeyReceiver>();
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
            var target = new MidiMessageTarget(channel, e.Data.MidiData0);
            switch (type)
            {
                case CommandType.ControlChange:
                    lock (_valueReceiverMap)
                    {
                        if (_valueReceiverMap.TryGetValue(target, out IValueReceiver receiver))
                        {
                            receiver.OnValueReceived(e.Data.MidiData1);
                        }
                        else
                        {
                            _valueReceiverMap.Add(target, NullValueReceiver.Value);
                            NewControlChangedDetected?.Invoke(this, new MessageTypeEventArgs(channel, e.Data.MidiData0));
                        }
                    }
                    break;
                case CommandType.NoteOn:
                    lock (_keyReceiverMap)
                    {
                        if(_keyReceiverMap.TryGetValue(target,out IKeyReceiver receiver))
                        {
                            receiver.OnKeyDown(e.Data.MidiData1);
                        }
                        else
                        {
                            _keyReceiverMap.Add(target, NullKeyReceiver.Value);
                            NewNoteDetected?.Invoke(this, new MessageTypeEventArgs(channel, e.Data.MidiData0));
                        }
                    }
                    break;
                case CommandType.NoteOff:
                    lock (_keyReceiverMap)
                    {
                        if (_keyReceiverMap.TryGetValue(target, out IKeyReceiver receiver))
                        {
                            receiver.OnKeyUp(e.Data.MidiData1);
                        }
                        else
                        {
                            _keyReceiverMap.Add(target, NullKeyReceiver.Value);
                            NewNoteDetected?.Invoke(this, new MessageTypeEventArgs(channel, e.Data.MidiData0));
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
            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }
            if (!(channel < MidiConstant.MidiMaxChannelNum))
            {
                throw new ArgumentException("Channel MUST be 0~15", nameof(channel));
            }
            lock (_valueReceiverMap)
            {
                _valueReceiverMap[new MidiMessageTarget(channel, type)] = receiver;
            }

        }

        public void SetReceiverToNote(byte channel, byte type, IKeyReceiver receiver)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }
            if (!(channel < MidiConstant.MidiMaxChannelNum))
            {
                throw new ArgumentException("Channel MUST be 0~15", nameof(channel));
            }
            lock (_keyReceiverMap)
            {
                _keyReceiverMap[new MidiMessageTarget(channel, type)] = receiver;
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
