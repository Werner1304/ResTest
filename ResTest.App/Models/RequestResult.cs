using System.Net;
using System.Net.Mime;

namespace ResTest.App.Models
{
	public record HeaderData(string Key, string Value);

	public record RequestResult(HttpStatusCode Status, TimeSpan Duration, string Body, ContentType ContentType, List<HeaderData> Headers);
}
