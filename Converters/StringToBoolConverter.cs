using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace STFREYA.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert the current status to a boolean for IsChecked
            return value is string stringValue && parameter is string targetValue && stringValue == targetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Only update the Status if the RadioButton is selected
            if (value is bool isChecked && isChecked && parameter is string targetValue)
            {
                //Debug.WriteLine($"ConvertBack: Changing Status to {targetValue}");
                return targetValue; // Return the selected value
            }
            //Debug.WriteLine($"ConvertBack: Ignored update for unchecked RadioButton");
            return Binding.DoNothing; // Prevent resetting the Status when unchecking
        }
    }
}
