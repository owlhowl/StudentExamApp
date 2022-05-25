using System;
using System.Globalization;
using System.Windows.Data;

namespace StudentExamClient.Converters
{
    public class LocalDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dt = (DateTime)value;

            return $"{dt.Day}.{dt.Month}.{dt.Year} {dt.ToShortTimeString()}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
