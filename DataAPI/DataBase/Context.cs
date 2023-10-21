using DataAPI.DataBase.Tables;
using Microsoft.EntityFrameworkCore;

namespace DataAPI.DataBase
{
    public class Context : DbContext
    {
        private readonly IConfiguration _configuration;

        public Context(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public DbSet<Human> Humans { get; set; }
    }
}