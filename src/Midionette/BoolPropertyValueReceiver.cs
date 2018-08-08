using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette
{
    public class BoolPropertyValueReceiver : PropertyValueReceiverBase
    {
        public BoolPropertyValueReceiver(string name, object targetObject, string propertyName) 
            : base(name, targetObject, propertyName)
        {
        }

        protected override object ConvertValue(byte value)
        {
            return value > 63;
        }
    }
}
