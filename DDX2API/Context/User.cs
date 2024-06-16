using System.ComponentModel.DataAnnotations;

namespace DDX2API.Context;

public class User
{
    [Key]
    public int Id { get; set; }
    public List<int> Chats { get; set; }
    public Subscription Pack { get; set; }
    public UserProfile Profile { get; set; }
    
    //Trainer futures
    public bool IsMaster { get; set; }
    public bool UseTrainer { get; set; }
    public int TrainerId { get; set; }
    
    //Personal Data
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Secret { get; set; }
}


public enum Subscription
{
    None = 0,
    Light = 1,
    Smart = 2,
    Infinity = 3,
}