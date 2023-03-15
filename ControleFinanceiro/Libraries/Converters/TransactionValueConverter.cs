using ControleFinanceiro.Models;
using System.Globalization;

namespace ControleFinanceiro.Libraries.Converters
{
    public class TransactionValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Transaction transaction = (Transaction)value;
            if (transaction == null)
                return null;

            if (transaction.Type == TransactionType.Income)
                return transaction.Value.ToString("C");
            else
                return $"- {transaction.Value.ToString("C")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
