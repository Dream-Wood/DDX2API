using System.ComponentModel.DataAnnotations;

namespace DDX2API.Context;

public class Message
{
    [Key]
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public ContentType Type { get; set; }
    public string Context { get; set; }
    public string Date_Create { get; set; }
}

public enum ContentType
{
    Text = 0,
    Image = 1,
    Video = 2,
    Short = 3,
    Emoji = 4,
    Link = 5,
    Workout = 6,
}