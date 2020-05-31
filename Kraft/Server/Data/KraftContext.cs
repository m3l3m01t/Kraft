using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kraft.Shared;

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
