using MidionetteSampleGui.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MidionetteSampleGui.ViewModel
{
    class ControlChangeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is InputModel.ControlChangeInfo controlChange)
            {
                return $"{controlChange.DeviceName}:{controlChange.Channel}:{controlChange.Id}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
