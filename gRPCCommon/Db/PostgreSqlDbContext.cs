using Microsoft.EntityFrameworkCore;

namespace gRPCCommon.Db
{
    public class PostgreSqlDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgreSqlDbContext).Assembly);
        }
    }
}