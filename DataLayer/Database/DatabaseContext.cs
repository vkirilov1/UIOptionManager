using DataLayer.Model.Option;
using DataLayer.Model.OptionList;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<MixedOptionListDBEntry> MixedOptionLists {  get; set; }
        public DbSet<MixedOptionDBEntry> MixedOptions { get; set; }
        public DbSet<UserOptionListDBEntry> UserOptionLists { get; set; }
        public DbSet<UserOptionDBEntry> UserOptions { get; set; }

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

            MixedOptionListDBEntry workLocationList = new()
            {
                Id = 1,
                Name = "WorkLocations",
                Description = "Shows options the employee's preferred work location",
                SystemIdType = "WorkLocation"
            };

            UserOptionListDBEntry preferredRoleList = new()
            {
                Id = 1,
                Name = "Roles",
                Description = "User can freely enter options for prefferedRole and choose one"
            };

            MixedOptionDBEntry sofiaLocation = new()
            {
                Id = 1,
                Value = "Sofia",
                SystemId = 1,
                MixedOptionListDBEntryId = 1
            };

            MixedOptionDBEntry plovdivLocation = new()
            {
                Id = 2,
                Value = "Plovdiv",
                SystemId = 2,
                MixedOptionListDBEntryId = 1
            };

            MixedOptionDBEntry varnaLocation = new()
            {
                Id = 3,
                Value = "Varna",
                SystemId = 3,
                MixedOptionListDBEntryId = 1
            };

            //FOR TESTING PURPOSES ONLY
            MixedOptionDBEntry userDefinedTestLocation = new()
            {
                Id = 4,
                Value = "TestLocation",
                SystemId = null,
                MixedOptionListDBEntryId = 1
            };

            //FOR TESTING PURPOSES ONLY
            UserOptionDBEntry userDefinedTestRole = new()
            {
                Id = 1,
                Value = "TestRole",
                UserOptionListDBEntryId = 1
            };

            modelBuilder.Entity<MixedOptionListDBEntry>()
                .HasData(workLocationList);

            modelBuilder.Entity<UserOptionListDBEntry>()
                .HasData(preferredRoleList);

            modelBuilder.Entity<MixedOptionDBEntry>()
                .HasData(sofiaLocation, plovdivLocation, varnaLocation, userDefinedTestLocation);

            modelBuilder.Entity<UserOptionDBEntry>()
                .HasData(userDefinedTestRole);
        }
    }
}
