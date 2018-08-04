using MidionetteSampleGui.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidionetteSampleGui.Model
{
    class MainModel
    {

        public InputModel Input { get; } = new InputModel();

        public SampleModel SampleObject { get; } = new SampleModel();

        public void AssignControlChangeToReceiver(InputModel.ControlChangeInfo target, string propertyName)
        {
            var receiver = new SimpleRelayReceiver(SampleObject, propertyName);
            Input.AssignControlChangeToReceiver(target, receiver);
        }

    }
}
