using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MidionetteSampleGui.Model
{
    class SampleModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private float _value0;
        public float Value0 {
            get => _value0;
            set
            {
                if (!Equals(_value0, value))
                {
                    _value0 = value;
                    RaisePropertyChanged();
                }
            }
        }

        private float _value1;
        public float Value1
        {
            get => _value1;
            set
            {
                if (!Equals(_value1, value))
                {
                    _value1 = value;
                    RaisePropertyChanged();
                }
            }
        }

        private float _value2;
        public float Value2
        {
            get => _value2;
            set
            {
                if (!Equals(_value2, value))
                {
                    _value2 = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected void RaisePropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public SampleModel()
        {
            _value0 = 0.5f;
        }
    }
}
