using System.Globalization;
using BankingApp.DTOs;

namespace BankingApp.UI.Converters;

public class RegistrationStatusToColorConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is RegistrationStatus registrationStatus)
        {
            return registrationStatus switch
            {
                RegistrationStatus.Approved => Colors.Green,
                RegistrationStatus.Rejected => Colors.Red,
                RegistrationStatus.Pending => Colors.Yellow,
                _ => Colors.Gray
            };
        }
        return Colors.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}