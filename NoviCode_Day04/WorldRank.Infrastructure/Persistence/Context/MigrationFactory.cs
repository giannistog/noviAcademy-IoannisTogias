using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldRank.Infrastructure.Persistence.Context
{
    public class WorldRankDbContextContextFactory : IDesignTimeDbContextFactory<WorldRankDbContext>
    {
        public WorldRankDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WorldRankDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=WorldRank;Integrated Security=true;TrustServerCertificate=true");

            return new WorldRankDbContext(optionsBuilder.Options);
        }
    }
}

