using System.Globalization;
using BankingApp.DTOs;

namespace BankingApp.UI.Converters;

public class StatusToVisibilityConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is RegistrationStatus status && parameter is RegistrationStatus paramStatus)
        {
            return status == paramStatus;
        }

        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}