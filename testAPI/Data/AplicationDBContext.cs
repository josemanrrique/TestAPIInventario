using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace testAPI.Data
{
    public class AplicationDBContext : DbContext
    {
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Actividades> Actividades { get; set; }
        public AplicationDBContext(DbContextOptions<AplicationDBContext> option) : base(option)
        {

        }
    }
}
