using Microsoft.EntityFrameworkCore;
using TrainingDataModel.Entities;

namespace TrainingDataModel.Data
{
    /// <summary>
    /// Database context for the Training business model
    /// </summary>
    public class TrainingDbContext : DbContext
    {
        public TrainingDbContext(DbContextOptions<TrainingDbContext> options)
            : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<TrainingCourse> TrainingCourses { get; set; } = null!;
        public DbSet<Module> Modules { get; set; } = null!;
        public DbSet<VmType> VmTypes { get; set; } = null!;
        public DbSet<VmOption> VmOptions { get; set; } = null!;
        public DbSet<VirtualMachine> VirtualMachines { get; set; } = null!;
        public DbSet<BillingInvoice> BillingInvoices { get; set; } = null!;
        public DbSet<DailyUsageStatistic> DailyUsageStatistics { get; set; } = null!;
        public DbSet<AdminUser> AdminUsers { get; set; } = null!;
        public DbSet<RdpFile> RdpFiles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasMany(e => e.Students)
                    .WithOne(e => e.Customer)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(e => e.AdminUsers)
                    .WithOne(e => e.Customer)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(e => e.BillingInvoices)
                    .WithOne(e => e.Customer)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasMany(e => e.RdpFiles)
                    .WithOne(e => e.Student)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure TrainingCourse entity
            modelBuilder.Entity<TrainingCourse>(entity =>
            {
                entity.Property(e => e.Price).HasPrecision(10, 2);
                entity.HasMany(e => e.Modules)
                    .WithOne(e => e.TrainingCourse)
                    .HasForeignKey(e => e.TrainingCourseId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.VirtualMachines)
                    .WithOne(e => e.TrainingCourse)
                    .HasForeignKey(e => e.TrainingCourseId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Module entity
            modelBuilder.Entity<Module>(entity =>
            {
                entity.HasIndex(e => new { e.TrainingCourseId, e.OrderNumber }).IsUnique();
            });

            // Configure VmType entity
            modelBuilder.Entity<VmType>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasMany(e => e.VmOptions)
                    .WithOne(e => e.VmType)
                    .HasForeignKey(e => e.VmTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.VirtualMachines)
                    .WithOne(e => e.VmType)
                    .HasForeignKey(e => e.VmTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure VirtualMachine entity
            modelBuilder.Entity<VirtualMachine>(entity =>
            {
                entity.HasMany(e => e.DailyUsageStatistics)
                    .WithOne(e => e.VirtualMachine)
                    .HasForeignKey(e => e.VirtualMachineId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.RdpFiles)
                    .WithOne(e => e.VirtualMachine)
                    .HasForeignKey(e => e.VirtualMachineId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure BillingInvoice entity
            modelBuilder.Entity<BillingInvoice>(entity =>
            {
                entity.Property(e => e.TotalAmount).HasPrecision(10, 2);
                entity.HasIndex(e => e.InvoiceNumber).IsUnique();
            });

            // Configure DailyUsageStatistic entity
            modelBuilder.Entity<DailyUsageStatistic>(entity =>
            {
                entity.Property(e => e.HoursUsed).HasPrecision(8, 2);
                entity.Property(e => e.Cost).HasPrecision(10, 2);
                entity.HasIndex(e => new { e.VirtualMachineId, e.UsageDate }).IsUnique();
            });

            // Configure AdminUser entity
            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Configure RdpFile entity
            modelBuilder.Entity<RdpFile>(entity =>
            {
                entity.HasIndex(e => e.FileName);
            });
        }
    }
}
