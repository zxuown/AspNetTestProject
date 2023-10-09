using AspNetTest.Models;
using System.Diagnostics.Metrics;
using System.Text.Json;
using AspNetTest.Models;
namespace AspNetTest.Service;

public class AboutMeService
{
    private string _file;
    public AboutMeService(string file)
    {
        _file = file;
        Load();
    }
    public AboutMe aboutMe { get; private set; }

    private void Load()
    {
        if (!File.Exists(_file))
        {
            aboutMe = new();
            //{
            //    Name = "Andriy",
            //    SecondName = "Pavlovich",
            //    Age = 16,
            //    Abilities = new()
            //    {
            //        new Abilities
            //        {
            //            Name ="OOP",
            //            Level = 50,
            //        }
            //    }
            //};
        }
        else
        {
            aboutMe = JsonSerializer.Deserialize<AboutMe>(File.ReadAllText(_file));
        }
    }

    public void SaveChanges()
    {
        File.WriteAllText(_file, JsonSerializer.Serialize(aboutMe));
    }

    public void Add(Abilities abilities)
    {
        abilities.Id = aboutMe.Abilities.Count == 0 ? 1 : 1 + aboutMe.Abilities.Max(x => x.Id);
        aboutMe.Abilities.Add(abilities);
    }

}
