using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace ResTest.App.Models
{
	public interface IDialogService
	{
		Task<string?> OpenFileAsync();
	}

	public class DialogService : IDialogService
	{
		private Window _target;

		public DialogService(Window target)
		{
			_target = target;
		}

		public async Task<string?> OpenFileAsync()
		{
			var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
			{
				Title = "Open File",
				AllowMultiple = false,
			});

			if (files.Count <= 0)
				return null;

			var file = files[0];

			var localPath = file.TryGetLocalPath();
			if (localPath is null)
				return null;

			return localPath;
		}
	}
}
