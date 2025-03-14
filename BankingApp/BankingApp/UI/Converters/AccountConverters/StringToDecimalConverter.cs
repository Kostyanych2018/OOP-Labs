using System.Globalization;

namespace BankingApp.UI.Converters;

public class StringToDecimalConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal decimalValue)
        {
            return decimalValue.ToString();
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            if (decimal.TryParse(stringValue, out decimal result))
            {
                return result;      
            }
        }
        return 0m;
    }
}