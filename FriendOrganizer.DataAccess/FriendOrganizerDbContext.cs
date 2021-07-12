using FriendOrganizer.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FriendOrganizer.DataAccess
{
    public class FriendOrganizerDbContext : DbContext
    {
        public FriendOrganizerDbContext() : base("FriendOrganizerDb")
        {

        }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public DbSet<FriendPhoneNumber> FriendPhoneNumbers { get; set; }
        public DbSet<Meeting> Meetings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Fluent API adding constraint
            //modelBuilder.Entity<Friend>()
            //    .Property(f => f.FirstName)
            //    .IsRequired()
            //    .HasMaxLength(50);

            // Fluent API if separating configuration in a class file
            //modelBuilder.Configurations.Add(new FriendConfiguration());
        }
    }

    // Fluent API if separating configuration in a class file
    //public class FriendConfiguration : EntityTypeConfiguration<Friend>
    //{
    //    public FriendConfiguration()
    //    {
    //        Property(f => f.FirstName)
    //            .IsRequired()
    //            .HasMaxLength(50);
    //    }
    //}
}
