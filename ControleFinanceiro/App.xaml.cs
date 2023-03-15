using ControleFinanceiro.View;

namespace ControleFinanceiro;

public partial class App : Application
{
	public App(TransactionList listPage)
	{
		InitializeComponent();

		MainPage = new NavigationPage(listPage);
	}
}
