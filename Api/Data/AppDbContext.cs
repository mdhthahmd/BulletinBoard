using Microsoft.EntityFrameworkCore;
using Models;
using System;

namespace Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base (options)
        {
        }

        public DbSet<Bulletin> Bulletins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<Bulletin>().HasData( new Bulletin {  
                Id =  Guid.NewGuid(),
                CreatedBy = 1,
                CreatedAt = DateTime.Now,
                HeadingText = "Bulletin Header",
                Content = "This is the Content of the Bulletin",
                Status = Status.Active
             });
        }
    }
}