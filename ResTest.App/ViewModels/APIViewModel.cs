using CommunityToolkit.Mvvm.Input;
using ResTest.App.Models;
using System.Collections.ObjectModel;

namespace ResTest.App.ViewModels
{
	public abstract class ResultBaseViewModel
	{
		public abstract string Status { get; }
		public abstract string Duration { get; }
		public abstract string Body { get; }
		public abstract IEnumerable<HeaderData> Headers { get; }
	}

	public class ResultViewModel : ResultBaseViewModel
	{
		public override string Status => $"{_result.Status} {(int)_result.Status}";
		public override string Duration => $"{_result.Duration.TotalMilliseconds:.} ms";
		public override string Body => _body;
		public override IEnumerable<HeaderData> Headers => _result.Headers;

		private RequestResult _result;
		private string _body;

		public ResultViewModel(RequestResult result, IFormattingService formatter)
		{
			_result = result;
			_body = formatter.ApplyFormatting(result.Body, result.ContentType);
		}
	}

	public class EmptyResultViewModel : ResultBaseViewModel
	{
		public override string Status => "";
		public override string Duration => "";
		public override string Body => "";
		public override IEnumerable<HeaderData> Headers => [];
	}

	public class ExceptionResultViewModel : ResultBaseViewModel
	{
		public override string Status => _exception.Message;
		public override string Duration => "";
		public override string Body => "";
		public override IEnumerable<HeaderData> Headers => [];

		private Exception _exception;

		public ExceptionResultViewModel(Exception exception)
		{
			_exception = exception;
		}
	}

	public partial class APIViewModel : ViewModelBase
	{
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

		public ObservableCollection<HeaderData> Headers => _headers;

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

		public APIViewModel(IRequestService requestService, IFormattingService formatter)
		{
			_requestService = requestService;
			_formatter = formatter;
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
