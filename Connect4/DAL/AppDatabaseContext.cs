using GameEngine;
using Microsoft.EntityFrameworkCore;


namespace DAL
{
    public class AppDatabaseContext : DbContext
    {
        public DbSet<GameSettings> Settings { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=C:\\Users\\Rommi\\icd0008-2019f\\Connect4\\WebApp\\mydb.db");
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\eparrom\\icd0008-2019f\\Connect4\\WebApp\\mydb.db");
        }

        public AppDatabaseContext(DbContextOptions options) : base(options)
        {
            
        }
        public AppDatabaseContext()
        {
            
        }
        
    }
    
}