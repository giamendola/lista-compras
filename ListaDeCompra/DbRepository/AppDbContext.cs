using ListaDeCompra.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaContatos.DbRepository 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
   
        public DbSet<Produtos> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Contato>().HasData(new Produtos { id = 1, name = "Giovanni", isComplete = false, secret = "", createDate = DateTime.Now });
        }


    }

    
}