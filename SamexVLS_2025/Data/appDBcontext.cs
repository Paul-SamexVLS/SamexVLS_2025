using Microsoft.EntityFrameworkCore;
using SamexVLS_2025.Models;

namespace SamexVLS_2025.Data
{
    public class appDBcontext : DbContext
    {
        public appDBcontext(DbContextOptions<appDBcontext> options) : base(options) { }

        public DbSet<MR23_cotizacion> Quotes { get; set; }
        public DbSet<MR23_cotizacion_detalles> QuotesDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<MR23_cotizacion>()
            .HasMany(p => p.QuoteDetails)
            .WithOne(c => c.QuoteParent)
            .HasForeignKey(c => c.ParentId);
          

        }

    }
}
