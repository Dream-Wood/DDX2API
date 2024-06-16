using System.ComponentModel.DataAnnotations;

namespace DDX2API.Context;

public class Chat
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public int[] Users { get; set; }
    public List<Message> Messages { get; set; }
}