using CommunityToolkit.Mvvm.Messaging;
using ControleFinanceiro.Models;
using ControleFinanceiro.Repositories;

namespace ControleFinanceiro.View;

public partial class TransactionList : ContentPage
{
	private ITransactionRepository _transactionRepository;
	public TransactionList(ITransactionRepository transactionRepository)
	{
		InitializeComponent();

		_transactionRepository = transactionRepository;

		Reload();

		WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
		{
			Reload();
		});
	}

	private void Reload()
	{
		var items = _transactionRepository.GetAll();
		CollectionViewTransactions.ItemsSource = items;

		double income = items.Where(a => a.Type == TransactionType.Income).Sum(a => a.Value);
		double expense = items.Where(a => a.Type == TransactionType.Expense).Sum(a => a.Value);
		double balance = income - expense;

		LabelIncome.Text = income.ToString("C");
		LabelExpense.Text = expense.ToString("C");
		LabelBalance.Text = balance.ToString("C");
	}

	private void OnButtonClickedGoToTransacationAdd(object sender, EventArgs args)
	{
		/*
		 * Publisher - Subscribers -> 
		 * OK - TransactionAdd -> Publisher -> Cadastro (Mensagem > Transaction).
		 * OK - Subscribers -> TransactionList (Receba o Transaction).
		*/

		var transactionAdd = Handler.MauiContext.Services.GetService<TransactionAdd>();
		Navigation.PushModalAsync(transactionAdd);
	}

	private void TapGestureRecognizerTappedToTransactionEdit(object sender, EventArgs e)
	{
		var grid = (Grid)sender;
		var gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];
		Transaction transaction = (Transaction)gesture.CommandParameter;

		var transacationEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
		transacationEdit.SetTransactionToEdit(transaction);
		Navigation.PushModalAsync(transacationEdit);
	}

	private async void TapGestureRecognizerTappedToDelete(object sender, EventArgs e)
	{
		bool result = await App.Current.MainPage.DisplayAlert("Excluir!", "Tem certeza que deseja excluir?", "Sim", "Não");

		if (result)
		{
			var border = (Border)sender;
			var gesture = (TapGestureRecognizer)border.GestureRecognizers[0];
			Transaction transaction = (Transaction)gesture.CommandParameter;
			_transactionRepository.Delete(transaction);

			Reload();
		}
	}
}