using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AspNetTest.Models;

public class Abilities
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int Level { get; set; }
    public string? Image { get; set; }
    [JsonIgnore]
    public string FullUrl => "/Uploads/" + Image;
}
