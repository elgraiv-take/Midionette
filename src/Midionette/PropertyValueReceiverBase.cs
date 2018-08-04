using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette
{
    public abstract class PropertyValueReceiverBase : IValueReceiver
    {
        public string Name { get; }

        private MethodInfo _setter;
        private object _target;

        public PropertyValueReceiverBase(string name,object targetObject,string propertyName)
        {
            Name = name;
            var property = targetObject.GetType().GetProperty(propertyName);
            if (property==null)
            {
                throw new TargetException($"Property \"{property}\" NOT Found in Target Object");
            }
            _setter = property.SetMethod;
            _target = targetObject;
        }

        public void OnValueReceived(byte value)
        {
            _setter.Invoke(_target, new object[] { ConvertValue(value) });
        }

        abstract protected object ConvertValue(byte value);
    }
}
