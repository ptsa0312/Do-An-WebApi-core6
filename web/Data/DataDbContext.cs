using Microsoft.EntityFrameworkCore;
using System;
using web.Models.DTO;

namespace web.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> dbContextOptions) : base(dbContextOptions)
        {
            //constructor
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmActors>().HasKey(ma => new { ma.FilmId, ma.ActorId });

            modelBuilder.Entity<FilmActors>().HasOne(ma => ma.Film).WithMany(m => m.FilmActors).HasForeignKey(ma => ma.FilmId);

            modelBuilder.Entity<FilmActors>().HasOne(ma => ma.Actor).WithMany(a => a.FilmActors).HasForeignKey(ma => ma.ActorId);

            modelBuilder.Entity<FilmDirectors>().HasKey(md => new { md.FilmId, md.DirectorId });

            modelBuilder.Entity<FilmDirectors>().HasOne(md => md.Film).WithMany(m => m.FilmDirectors).HasForeignKey(md => md.FilmId);

            modelBuilder.Entity<FilmDirectors>().HasOne(md => md.Director).WithMany(d => d.FilmDirectors).HasForeignKey(md => md.DirectorId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Films> Films { get; set; }

        public DbSet<Directors> Directors { get; set; }

        public DbSet<Actors> Actors { get; set; }

        public DbSet<FilmActors> FilmActors { get; set; }

        public DbSet<FilmDirectors> FilmDirectors { get; set; }

        public DbSet<Image> Images { get; set; }
    }
}
