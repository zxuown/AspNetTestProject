using System.Text.Json.Serialization;

namespace AspNetTest.Models;

public class NewsItem
{
	public int Id { get; set; }
	public string Header { get; set; }

	public string? Image { get; set; }
	[JsonIgnore]
	public string FullUrl => "/news/" + Image;

	public DateTime Date { get; set; }

	public string ShortText { get; set; }
	
	public string Text { get; set; }


}
