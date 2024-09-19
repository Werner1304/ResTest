using System.Text.Json;

namespace ResTest.App.Models
{
	public record SavedRequest(string Name, string URL, List<HeaderData> Headers, string Method, string Body)
	{
		public HttpMethod HttpMethod => HttpMethod.Parse(Method);
	}

	public interface IRequestRepository
	{
		public void Load(string source);
		public void Update(IEnumerable<SavedRequest> requests);
		public List<SavedRequest> GetAll();
		public void Save();
		public void SaveAs(string path);
		public void Delete(string name);
		public void Create(string name, string url, List<HeaderData> headers, HttpMethod method, string body);
	}

	public class JsonRequestRepository : IRequestRepository
	{
		private readonly JsonSerializerOptions _opts = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, WriteIndented = true };

		private string _path = "collection.json";
		private List<SavedRequest> _requests = [];

		public void Load(string source)
		{
			_requests.Clear();
			_path = source;

			if (!File.Exists(source))
				return;

			var text = File.ReadAllText(source, System.Text.Encoding.UTF8);
			_requests = JsonSerializer.Deserialize<List<SavedRequest>>(text, _opts) ?? [];
		}

		public void Update(IEnumerable<SavedRequest> requests)
		{
			_requests.Clear();
			_requests.AddRange(requests);
		}

		public void Save()
		{
			var text = JsonSerializer.Serialize(_requests, _opts);
			File.WriteAllText(_path, text, System.Text.Encoding.UTF8);
		}

		public List<SavedRequest> GetAll() => _requests;

		public void SaveAs(string path)
		{
			_path = path;
			Save();
		}

		public void Delete(string name)
		{
			_requests.RemoveAll(x => x.Name == name);
		}

		public void Create(string name, string url, List<HeaderData> headers, HttpMethod method, string body)
		{
			Delete(name);
			_requests.Add(new SavedRequest(name, url, headers, method.Method, body));
			_requests.Sort((a, b) => a.Name.CompareTo(b.Name));
		}
	}
}
