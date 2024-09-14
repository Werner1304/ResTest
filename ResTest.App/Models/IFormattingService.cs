using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ResTest.App.Models
{
	public interface IFormattingService
	{
		public string ApplyFormatting(string input, ContentType type);
	}

	public class BuiltinFormatter : IFormattingService
	{
		private readonly JsonSerializerOptions _jsonOpts = new JsonSerializerOptions() { WriteIndented = true, };

		public string ApplyFormatting(string input, ContentType type)
		{
			switch (type.MediaType)
			{
				case "application/json":
					{
						var o = JsonSerializer.Deserialize<JsonNode>(input);
						return JsonSerializer.Serialize(o, _jsonOpts);
					}
				default:
					return input;
			}
		}
	}
}
