using Microsoft.EntityFrameworkCore;
using Projektarbete_WebApi_EJ_JA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projektarbete_WebApi_EJ_JA.Data
{
    public class GeoMessageDBContext : DbContext
    {
        public GeoMessageDBContext(DbContextOptions<GeoMessageDBContext> options) : base(options) { }
        public DbSet<GeoMessage> GeoMessages { get; set; }
    }
}
