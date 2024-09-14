using Avalonia.Controls;
using ResTest.App.ViewModels;

namespace ResTest.App
{
	public partial class MainWindow : Window
	{
		public MainWindow(MainViewModel model)
		{
			InitializeComponent();
			DataContext = model;
		}
	}
}