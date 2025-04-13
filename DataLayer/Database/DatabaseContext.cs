using DataLayer.Model.Option;
using DataLayer.Model.OptionList;
using DataLayer.Model.Users;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<MixedOptionListDBEntry> MixedOptionLists { get; set; }
        public DbSet<MixedOptionDBEntry> MixedOptions { get; set; }
        public DbSet<UserOptionListDBEntry> UserOptionLists { get; set; }
        public DbSet<UserOptionDBEntry> UserOptions { get; set; }
        public DbSet<SystemOptionListDBEntry> SystemOptionLists { get; set; }
        public DbSet<UserDBEntry> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string solutionFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string databaseFile = "OptionManager.db";
            string databasePath = Path.Combine(solutionFolder, databaseFile);
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MixedOptionListDBEntry>()
                .Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<MixedOptionDBEntry>()
                .Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserOptionListDBEntry>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserOptionDBEntry>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<SystemOptionListDBEntry>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserDBEntry>().Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<MixedOptionListDBEntry>()
                .HasMany(list => list.Options)
                .WithOne(option => option.MixedOptionList)
                .HasForeignKey(option => option.MixedOptionListDBEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserOptionListDBEntry>()
                .HasMany(list => list.Options)
                .WithOne(option => option.UserOptionList)
                .HasForeignKey(option => option.UserOptionListDBEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MixedOptionListDBEntry>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserDBEntryId);

            modelBuilder.Entity<MixedOptionDBEntry>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserDBEntryId);

            modelBuilder.Entity<UserOptionListDBEntry>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserDBEntryId);

            modelBuilder.Entity<UserOptionDBEntry>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserDBEntryId);

            modelBuilder.Entity<SystemOptionListDBEntry>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserDBEntryId);
        }
    }
}
