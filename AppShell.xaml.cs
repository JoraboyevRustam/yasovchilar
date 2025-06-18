namespace SozTopar;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		Routing.RegisterRoute(nameof(Views.GamePage), typeof(Views.GamePage));
	}
}