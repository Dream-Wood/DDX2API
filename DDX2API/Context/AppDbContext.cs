using DDX2API.Context;
using Microsoft.EntityFrameworkCore;

namespace DDX2API;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<WorkoutSet> WorkoutSets { get; set; }
    public DbSet<Workout> Workouts { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
        
    }
}