using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using ResTest.App.Models;
using ResTest.App.ViewModels;

namespace ResTest.App
{
	public partial class App : Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			var window = new MainWindow();

			var services = new ServiceCollection()
				.AddSingleton<IFormattingService, BuiltinFormatter>()
				.AddSingleton<IRequestRepository, JsonRequestRepository>()
				.AddSingleton<IRequestService, HttpRequestService>()
				.AddSingleton<IDialogService, DialogService>(s => new DialogService(window))
				.AddSingleton<APIViewModel>()
				.AddSingleton<MainViewModel>()
				.BuildServiceProvider(new ServiceProviderOptions() { ValidateOnBuild = true });

			window.DataContext = services.GetRequiredService<MainViewModel>();

			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = window;
			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}