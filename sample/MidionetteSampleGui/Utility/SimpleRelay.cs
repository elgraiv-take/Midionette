using Elgraiv.Midionette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MidionetteSampleGui.Utility
{
    class SimpleRelayReceiver:IValueReceiver
    {
        private MethodInfo _setter;
        private object _target;
        public SimpleRelayReceiver(object targetObject,string propertyName)
        {
            var property= targetObject.GetType().GetProperty(propertyName);
            _setter=property.SetMethod;
            _target = targetObject;
            Name = propertyName;
        }

        public string Name { get; }

        public void OnValueReceived(byte value)
        {
            _setter.Invoke(_target, new object[] { value });
        }

    }
}
