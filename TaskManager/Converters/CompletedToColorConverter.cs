using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace TaskManager.Converters
{
    public class CompletedToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isCompleted = value is bool b && b;

            return isCompleted ? (SolidColorBrush)App.Current.FindResource("Brush.AccentGreen") : (SolidColorBrush)App.Current.FindResource("Brush.AccentBlue");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack is not supported.");
        }
    }
}
