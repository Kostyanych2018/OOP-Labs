using System.Globalization;
using BankingApp.DTOs;

namespace BankingApp.UI.Converters;

public class CustomerTypeToVisibilityConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is CustomerType customerType && parameter is CustomerType parameterType)
        {
            return customerType == parameterType;
        }

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}