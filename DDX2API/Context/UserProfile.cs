using System.ComponentModel.DataAnnotations;

namespace DDX2API.Context;

public class UserProfile
{
    [Key]
    public int UserId { get; set; }
    public UserSex Sex { get; set; }
    public UserLevel Level { get; set; }
    public string Name { get; set; }
    
    //Trainer futures
    public string PlaylistLink { get; set; }
    public string Description { get; set; }
    public int[] LikeWorkoutSets { get; set; }
    
    //options
    public int Height { get; set; }
    public int Weight { get; set; }
    public int Chest { get; set; }
    public int Waist { get; set; }
    public int Hips { get; set; }
    
    //Personal Data
    public string City { get; set; }
}

public enum UserSex
{
    Male = 0,
    Female = 1,
}

public enum UserLevel
{
    Beginning = 0, 
    Average = 1, 
    Advanced = 2
}