using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.Midionette
{
    public interface IValueReceiver
    {
        string Name { get; }

        void OnValueReceived(byte value);
    }
}
