using System.Globalization;
using BankingApp.Entities.Banking;

namespace BankingApp.UI.Converters;

public class AccountStatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AccountStatus status)
        {
            return status switch
            {
                AccountStatus.Active => Colors.Green,
                AccountStatus.Blocked => Colors.Red,
                AccountStatus.Closed => Colors.Gray,
                _ => Colors.Black
            };
        }
        return Colors.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}