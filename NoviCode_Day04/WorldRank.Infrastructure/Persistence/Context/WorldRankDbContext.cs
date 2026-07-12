using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldRank.Domain.Entities;

namespace WorldRank.Infrastructure.Persistence.Context
{
    public partial class WorldRankDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        public WorldRankDbContext(DbContextOptions<WorldRankDbContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(x =>
            {
                x.ToTable("Players");
                x.HasKey(x => x.Id);
                x.Property(y => y.Id).ValueGeneratedNever();
                x.Property(y => y.Name).HasMaxLength(100).IsRequired();
                x.Property(y => y.Score).IsRequired();
            });
            

            modelBuilder.Entity<Wallet>(x =>
            {
                x.ToTable("Wallets");
                x.HasKey(w => w.Id);
                x.Property(w => w.Id).ValueGeneratedNever(); 
                x.Property(w => w.PlayerId).IsRequired();  
                x.Property(w => w.Balance).HasColumnType("decimal(18,2)").IsRequired();
                x.Property(w => w.Currency).IsRequired();
                x.Property(w => w.IsBlocked).IsRequired();
            });
            base.OnModelCreating(modelBuilder);
        }
    } }
