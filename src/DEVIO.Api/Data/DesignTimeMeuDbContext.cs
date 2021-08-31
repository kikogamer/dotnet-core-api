using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DEVIO.Api.Data
{
    public class DesignTimeMeuDbContext : IDesignTimeDbContextFactory<MeuDbContext>
    {
        private readonly IConfiguration _configuration;

        public DesignTimeMeuDbContext()
        {

        }
        public DesignTimeMeuDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public MeuDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MeuDbContext>();
            // pass your design time connection string here
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            return new MeuDbContext(optionsBuilder.Options);
        }
    }
}
