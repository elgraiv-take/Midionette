using MidionetteSampleGui.Model;
using MidionetteSampleGui.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MidionetteSampleGui.ViewModel
{
    class MainViewModel:INotifyPropertyChanged
    {

        private MainModel _model;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyObservableCollection<string> DeviceNames
        {
            get
            {
                return _model.Input.DeviceNames;
            }
        }

        public ICommand RefreshCommand { get; }

        public float Value0
        {
            get => _model.SampleObject.Value0;
            set
            {
                _model.SampleObject.Value0 = value;
            }
        }
        public float Value1
        {
            get => _model.SampleObject.Value1;
            set
            {
                _model.SampleObject.Value1 = value;
            }
        }
        public float Value2
        {
            get => _model.SampleObject.Value2;
            set
            {
                _model.SampleObject.Value2 = value;
            }
        }

        public MainViewModel(MainModel model)
        {
            _model = model;
            RefreshCommand = new DelegateCommand(RefreshDevices);
            _model.SampleObject.PropertyChanged += SampleObject_PropertyChanged;
        }

        private void SampleObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var changedProperty = string.Empty;
            switch (e.PropertyName)
            {
                case nameof(_model.SampleObject.Value0):
                    changedProperty = nameof(Value0);
                    break;
                case nameof(_model.SampleObject.Value1):
                    changedProperty = nameof(Value1);
                    break;
                case nameof(_model.SampleObject.Value2):
                    changedProperty = nameof(Value2);
                    break;

            }
            if (!string.IsNullOrEmpty(changedProperty))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(changedProperty));
            }
        }

        public void RefreshDevices()
        {
            _model.Input.RefreshDevices();
        }
    }
}
