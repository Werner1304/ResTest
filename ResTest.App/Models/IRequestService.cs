using System.Net.Mime;

namespace ResTest.App.Models
{
	public interface IRequestService
	{
		public Task<RequestResult> SendRequestAsync(string url, HttpMethod method, string body, IEnumerable<HeaderData> headers);
	}

	public class HttpRequestService : IRequestService, IDisposable
	{
		private HttpClient _httpClient = new HttpClient();

		public async Task<RequestResult> SendRequestAsync(string url, HttpMethod method, string body, IEnumerable<HeaderData> headers)
		{
			var req = new HttpRequestMessage(method, url);
			req.Content = new StringContent(body, System.Text.Encoding.UTF8);

			foreach (var (k, v) in headers)
			{
				if (string.IsNullOrEmpty(k))
					continue;

				req.Headers.Add(k, v);
			}

			var start = DateTime.UtcNow;
			using var res = await _httpClient.SendAsync(req);
			var elapsed = DateTime.UtcNow - start;

			var responseBody = await res.Content.ReadAsStringAsync();
			var responseHeaders = new List<HeaderData>();

			foreach (var (k, v) in res.Headers)
				responseHeaders.Add(new HeaderData(k, string.Join(',', v)));

			var contentType = res.Content.Headers.ContentType?.MediaType ?? "text/plain";
			return new RequestResult(res.StatusCode, elapsed, responseBody, new ContentType(contentType), responseHeaders);
		}

		public void Dispose()
		{
			_httpClient.Dispose();
		}
	}
}
