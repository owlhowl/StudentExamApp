using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StudentExamClient.Converters
{
    internal class StatusVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var statusId = (int)value;
            if (statusId == 1)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
