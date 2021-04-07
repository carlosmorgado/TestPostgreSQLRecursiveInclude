using System;
using Microsoft.EntityFrameworkCore;

namespace TestPostgreSQLRecursiveInclude
{
    public class TestPostgreSQLDbConstext : DbContext, IDisposable
    {
        private const string connectionString = "Host=localhost;Database=postgres;Username=postgres;Password=MySuperSecretPassword!";

        public TestPostgreSQLDbConstext()
            : base(new DbContextOptionsBuilder<TestPostgreSQLDbConstext>()
                .UseNpgsql(connectionString)
                .Options)
        {
        }

        public DbSet<TestClass> TestClasses { get; set; }

        public DbSet<TestClassLink> TestClassLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<TestClass>()
                .HasKey(testClass => testClass.Id);

            modelBuilder
                .Entity<TestClassLink>()
                .HasKey(testClassLink => testClassLink.Id);
            modelBuilder
                .Entity<TestClassLink>()
                .HasOne(testClassLink => testClassLink.FromTestClass)
                .WithMany(testClass => testClass.Parents)
                .HasForeignKey(testClassLink => testClassLink.FromTestClassId);
            modelBuilder
                .Entity<TestClassLink>()
                .HasOne(testClassLink => testClassLink.ToTestClass)
                .WithMany(testClass => testClass.Children)
                .HasForeignKey(testClassLink => testClassLink.ToTestClassId);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
