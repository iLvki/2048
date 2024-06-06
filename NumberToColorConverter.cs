using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dwazeroczteryosiem
{
    public class NumberToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int number)
            {
                return number switch
                {
                    0 => Colors.LightGray,
                    2 => Colors.LightYellow,
                    4 => Colors.LightGoldenrodYellow,
                    8 => Colors.Orange,
                    16 => Colors.DarkOrange,
                    32 => Colors.Red,
                    64 => Colors.DarkRed,
                    128 => Colors.LightGreen,
                    256 => Colors.Green,
                    512 => Colors.DarkGreen,
                    1024 => Colors.LightBlue,
                    2048 => Colors.Blue,
                    _ => Colors.Black // For numbers higher than 2048
                };
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
