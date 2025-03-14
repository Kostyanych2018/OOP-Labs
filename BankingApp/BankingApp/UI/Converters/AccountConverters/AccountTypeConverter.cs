using System.Globalization;
using BankingApp.Entities.Banking;

namespace BankingApp.UI.Converters;

public class AccountTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AccountType account)
        {
            return account switch
            {
                AccountType.Checking => "Расчетный счет",
                AccountType.Savings => "Накопительный счет",
                AccountType.Salary => "Зарплатный счет",
                _ => "Неизвестный тип счета"
            };
        }

        return "Неизвестный тип счета";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}