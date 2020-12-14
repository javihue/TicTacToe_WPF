using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PRISMAPP.Converters
{
    public class MarkerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var marker = value as Marker[];
            var index = int.Parse(parameter.ToString());
            if (marker != null && index >= 0 && index < marker.Length)
            {
                switch (marker[index])
                {
                    case Marker.Empty:
                        return "";
                    case Marker.Player1:
                        return "B";
                    case Marker.Player2:
                        return "J";
                }
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
