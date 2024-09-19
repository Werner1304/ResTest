using CommunityToolkit.Mvvm.Input;
using ResTest.App.Models;
using System.Collections.ObjectModel;

namespace ResTest.App.ViewModels
{
	public partial class APIViewModel : ViewModelBase
	{
		public ObservableCollection<SavedRequest> SavedRequests
		{
			get => _savedRequests;
			set => SetAndNotify(ref _savedRequests, value);
		}

		public SavedRequest? SelectedRequest
		{
			get => _selectedRequest;
			set
			{
				SetAndNotify(ref _selectedRequest, value);

				if (value is null)
					return;

				URL = value.URL;
				Method = value.HttpMethod;
				Body = value.Body;
				Headers = new ObservableCollection<HeaderData>(value.Headers);
				RequestName = value.Name;
			}
		}

		public string RequestName
		{
			get => _requestName;
			set => SetAndNotify(ref _requestName, value);
		}

		public HttpMethod[] HttpMethods => [
			HttpMethod.Get,
			HttpMethod.Post,
			HttpMethod.Put,
			HttpMethod.Delete,
		];

		public string URL
		{
			get => _url;
			set => SetAndNotify(ref _url, value);
		}

		public HttpMethod Method
		{
			get => _method;
			set => SetAndNotify(ref _method, value);
		}

		public string Body
		{
			get => _body;
			set => SetAndNotify(ref _body, value);
		}

		public ObservableCollection<HeaderData> Headers
		{
			get => _headers;
			set => SetAndNotify(ref _headers, value);
		}

		public ResultBaseViewModel Result
		{
			get => _result;
			set => SetAndNotify(ref _result, value);
		}

		private string _url = "https://localhost:8443";
		private HttpMethod _method = HttpMethod.Get;
		private string _body = "";
		private ObservableCollection<HeaderData> _headers = [];
		private IRequestService _requestService;
		private IFormattingService _formatter;
		private ResultBaseViewModel _result = new EmptyResultViewModel();
		private IRequestRepository _requestRepository;
		private IDialogService _dialogService;
		private SavedRequest? _selectedRequest;
		private ObservableCollection<SavedRequest> _savedRequests = [];
		private string _requestName = "";

		public APIViewModel(IRequestService requestService, IFormattingService formatter, IRequestRepository requestRepository, IDialogService dialogService)
		{
			_requestService = requestService;
			_formatter = formatter;
			_requestRepository = requestRepository;
			_dialogService = dialogService;
		}

		[RelayCommand]
		private void SaveRequest()
		{
			var name = RequestName.Trim();

			if (RequestName.Length == 0)
				return;

			_requestRepository.Create(name, URL, Headers.ToList(), Method, Body);
			_requestRepository.Save();
			SavedRequests = new ObservableCollection<SavedRequest>(_requestRepository.GetAll());
		}

		[RelayCommand]
		private void DeleteRequest()
		{
			if (SelectedRequest is null)
				return;

			_requestRepository.Delete(SelectedRequest.Name);
			_requestRepository.Save();
			SelectedRequest = null;
			SavedRequests = new ObservableCollection<SavedRequest>(_requestRepository.GetAll());
		}

		[RelayCommand]
		private async Task LoadCollection()
		{
			var file = await _dialogService.OpenFileAsync();
			if (file is null)
				return;

			_requestRepository.Load(file);
			SavedRequests = new ObservableCollection<SavedRequest>(_requestRepository.GetAll());
		}

		[RelayCommand]
		private async Task SendRequest()
		{
			try
			{
				var result = await _requestService.SendRequestAsync(URL, Method, Body, _headers);
				Result = new ResultViewModel(result, _formatter);
			}
			catch (Exception ex)
			{
				Result = new ExceptionResultViewModel(ex);
			}
		}

		[RelayCommand]
		private void AddEmptyHeader()
		{
			Headers.Add(new HeaderData("", ""));
		}
	}
}
