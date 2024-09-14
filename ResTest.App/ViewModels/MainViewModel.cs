namespace ResTest.App.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		public ViewModelBase CurrentViewModel
		{
			get => _currentViewModel;
			set => SetAndNotify(ref _currentViewModel, value);
		}

		private ViewModelBase _currentViewModel;

		public MainViewModel(APIViewModel apiVM)
		{
			_currentViewModel = apiVM;
		}
	}
}
