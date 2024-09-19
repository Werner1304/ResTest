using ResTest.App.Models;

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
}
