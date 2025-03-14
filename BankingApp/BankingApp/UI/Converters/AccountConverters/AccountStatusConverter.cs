using System.Globalization;
using BankingApp.Entities.Banking;

namespace BankingApp.UI.Converters;

public class AccountStatusConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AccountStatus status)
        {
            return status switch
            {
                AccountStatus.Active => "Активен",
                AccountStatus.Blocked => "Заблокирован",
                AccountStatus.Closed => "Закрыт",
                _ => "Неизвестный статус"
            };
        }
        return "Неизвестный статус";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}