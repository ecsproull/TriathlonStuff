using Microsoft.EntityFrameworkCore;

namespace EdsTriathlonStuff.App_Code
{
    public class SwimDataContext : DbContext
    {
        public SwimDataContext(DbContextOptions<SwimDataContext> options) : base(options)
        {
        }

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<SwimSet> SwimSets { get; set; }
        public DbSet<WokoutSet> WorkoutSets { get; set; }
    }
}
