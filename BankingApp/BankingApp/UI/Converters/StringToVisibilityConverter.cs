using System.Globalization;

namespace BankingApp.UI.Converters;

public class StringToVisibilityConverter:IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !string.IsNullOrWhiteSpace(value as string) ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}