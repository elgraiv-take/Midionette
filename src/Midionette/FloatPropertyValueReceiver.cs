using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette
{
    public class FloatPropertyValueReceiver : PropertyValueReceiverBase
    {
        public FloatPropertyValueReceiver(string name, object targetObject, string propertyName) 
            : base(name, targetObject, propertyName)
        {
        }

        protected override object ConvertValue(byte value)
        {
            return value / 127.0f;
        }
    }
}
