namespace AspNetTest.Models;

public class ToDoList
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }

    public bool Done { get; set; }
    public virtual User User { get; set; }


}
