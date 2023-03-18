using CommunityToolkit.Mvvm.Messaging;
using ControleFinanceiro.Models;
using ControleFinanceiro.Repositories;
using System.Text;

namespace ControleFinanceiro.View;

public partial class TransactionAdd : ContentPage
{
	private ITransactionRepository _transactionRepository;
	public TransactionAdd(ITransactionRepository transactionRepository)
	{
		InitializeComponent();
		_transactionRepository = transactionRepository;
	}

	private void TapGestureRecognizerTapped(object sender, EventArgs e)
	{
		Navigation.PopModalAsync();
	}

	private void OnSaveButtonClicked(object sender, EventArgs args)
	{
		if (!IsValidData())
			return;

		SaveTransactionInDatabase();

		Navigation.PopModalAsync();
		WeakReferenceMessenger.Default.Send<string>(string.Empty);

		var count = _transactionRepository.GetAll().Count;
		App.Current.MainPage.DisplayAlert("Mensagem!", $"Existem {count} registro(s) no banco!", "OK");
	}

	private void SaveTransactionInDatabase()
	{
		Transaction transaction = new Transaction()
		{
			Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expense,
			Name = EntryName.Text,
			Date = DatePickerDate.Date,
			Value = double.Parse(EntryValue.Text)
		};

		_transactionRepository.Add(transaction);
	}

	private bool IsValidData()
	{
		StringBuilder sb = new StringBuilder();
		bool valid = true;

		if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
		{
			sb.AppendLine("O campo 'Nome' precisa ser preenchido.\n");
			valid = false;
		}

		if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text))
		{
			sb.AppendLine("O campo 'Valor' precisa ser preenchido.\n");
			valid = false;

		}

		double result;
		if (!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out result))
		{
			sb.AppendLine("O campo 'Valor' é inválido.\n");
			valid = false;

		}

		if (!valid)
			LabelError.IsVisible = true;
		LabelError.Text = sb.ToString();

		return valid;
	}
}