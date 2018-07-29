using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette
{
    public class MidionetteController
    {
        private Dictionary<string, InputDevice> _deviceMap=new Dictionary<string, InputDevice>();

        public IEnumerable<InputDevice> ConncectedDevices { get; }


    }
}
