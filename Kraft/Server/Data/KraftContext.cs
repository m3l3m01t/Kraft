using Microsoft.EntityFrameworkCore;

namespace Kraft.Server.Data
{
    public class KraftContext : DbContext
    {
        public KraftContext (DbContextOptions<KraftContext> options)
            : base(options)
        {
        }

        public DbSet<Kraft.Shared.Cluster> Cluster { get; set; }
    }
}
