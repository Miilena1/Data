using System.ComponentModel.DataAnnotations.Schema;

namespace Data.DataBase.Tables;

[Table("Humans")]
public class Human
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    public override string ToString()
    {
        return $"Id = {Id}, Name = {Name}, Surname = {Surname}";
    }
}