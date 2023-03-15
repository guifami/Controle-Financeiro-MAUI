using System.Globalization;

namespace ControleFinanceiro.Libraries.Converters
{
    public class TransactionNameColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Color.FromArgb("#FFFFFF");

            var ramdom = new Random();
            var color = String.Format("#FF{0:X6}", ramdom.Next(0x1000000));
            return Color.FromArgb(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
