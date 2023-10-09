using AspNetTest.Models;
using System.Text.Json.Serialization;

namespace AspNetTest;

public class AboutMe
{
    public AboutMe() 
    {
        Abilities = new List<Abilities>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string SecondName { get; set; }
    public int Age { get; set; }    
    public virtual ICollection<Abilities> Abilities { get; set; }

}
