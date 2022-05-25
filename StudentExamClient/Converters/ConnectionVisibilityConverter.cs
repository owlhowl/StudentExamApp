using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StudentExamClient.Converters
{
    public class ConnectionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isConnected = (bool)value;

            return isConnected ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
