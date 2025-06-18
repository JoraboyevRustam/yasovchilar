using SozTopar.ViewModels;

namespace SozTopar.Views;

public partial class GamePage : ContentPage
{
	public GamePage(GamePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private async void OnBackClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}