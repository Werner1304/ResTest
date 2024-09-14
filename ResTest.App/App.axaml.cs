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
			var services = new ServiceCollection()
				.AddSingleton<IFormattingService, BuiltinFormatter>()
				.AddSingleton<IRequestService, HttpRequestService>()
				.AddSingleton<APIViewModel>()
				.AddSingleton<MainViewModel>()
				.AddSingleton<MainWindow>()
				.BuildServiceProvider(new ServiceProviderOptions() { ValidateOnBuild = true });

			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				desktop.MainWindow = services.GetRequiredService<MainWindow>();
			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}