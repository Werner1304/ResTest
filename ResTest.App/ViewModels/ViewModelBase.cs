using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ResTest.App.ViewModels
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public void Notify([CallerMemberName] string? property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

		public void SetAndNotify<T>(ref T prop, T value, [CallerMemberName] string? property = null)
		{
			prop = value;
			Notify(property);
		}
	}
}
