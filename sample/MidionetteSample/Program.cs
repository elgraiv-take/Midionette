using Elgraiv.Midionette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidionetteSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var input=new MidiInput();
            input.MidiDataReceived += Input_MidiDataReceived;
        }

        private static void Input_MidiDataReceived(object sender, MidiDataEventArgs e)
        {
            
        }
    }
}
