namespace DDX2API.Context;

public class Workout
{
    public int Id { get; set; }
    public WorkoutLevel Level { get; set; }
    public String Title { get; set; }
    public String ImageUrl { get; set; }
}

public enum WorkoutLevel
{
    Beginning = 0, 
    Average = 1, 
    Advanced = 2
}