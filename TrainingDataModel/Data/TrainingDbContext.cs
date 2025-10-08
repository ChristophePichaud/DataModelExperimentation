using Microsoft.EntityFrameworkCore;
using TrainingDataModel.Entities;

namespace TrainingDataModel.Data
{
    /// <summary>
    /// Database context for the Training business model
    /// </summary>
    public class TrainingDbContext : DbContext
    {
        // Static method for initial data seeding
        private static void SeedInitialData(ModelBuilder modelBuilder)
        {
            // Super AdminUser
            var staticCreatedAt = new DateTime(2025, 10, 7, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<AdminUser>().HasData(new AdminUser
            {
                Id = 1,
                Email = "sa@corp.com",
                Username = "Super Admin",
                Name = "Super Admin",
                CustomerId = null, // Not linked to a specific customer
                CreatedAt = staticCreatedAt,
                UpdatedAt = null
            });

            // Initial data seeding for User entity
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Name = "Super Admin",
                Email = "sa@corp.com",
                Password = "admin", // Replace with hashed password in production
                UserType = UserType.SuperAdmin,
                CreatedAt = staticCreatedAt,
                UpdatedAt = null
            });
        }
        public TrainingDbContext(DbContextOptions<TrainingDbContext> options)
            : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<TrainingCourse> TrainingCourses { get; set; } = null!;
    public DbSet<StudentTrainingCourse> StudentTrainingCourses { get; set; } = null!;
        public DbSet<Module> Modules { get; set; } = null!;
        public DbSet<VmType> VmTypes { get; set; } = null!;
        public DbSet<VmOption> VmOptions { get; set; } = null!;
        public DbSet<VirtualMachine> VirtualMachines { get; set; } = null!;
        public DbSet<BillingInvoice> BillingInvoices { get; set; } = null!;
        public DbSet<DailyUsageStatistic> DailyUsageStatistics { get; set; } = null!;
    public DbSet<AdminUser> AdminUsers { get; set; } = null!;
    public DbSet<RdpFile> RdpFiles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Trainer> Trainers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Trainer entity
            modelBuilder.Entity<Trainer>(entity =>
            {
                entity.Property(e => e.UserId).IsRequired();
                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Trainers)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

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
                entity.Property(e => e.UserId).IsRequired();
                entity.HasMany(e => e.RdpFiles)
                    .WithOne(e => e.Student)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                // Many-to-many: Student <-> TrainingCourse
                entity.HasMany<StudentTrainingCourse>()
                    .WithOne()
                    .HasForeignKey(stc => stc.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
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
                // Many-to-many: TrainingCourse <-> Student
                entity.HasMany<StudentTrainingCourse>()
                    .WithOne()
                    .HasForeignKey(stc => stc.TrainingCourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure StudentTrainingCourse join entity
            modelBuilder.Entity<StudentTrainingCourse>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.TrainingCourseId });
                entity.HasOne<Student>()
                    .WithMany()
                    .HasForeignKey(e => e.StudentId);
                entity.HasOne<TrainingCourse>()
                    .WithMany()
                    .HasForeignKey(e => e.TrainingCourseId);
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
                entity.Property(e => e.UserId).IsRequired();
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Configure RdpFile entity
            modelBuilder.Entity<RdpFile>(entity =>
            {
                entity.HasIndex(e => e.FileName);
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(256).IsRequired();
                entity.Property(e => e.UserType).IsRequired();
            });

            // Call static method to seed initial data only once
            SeedInitialData(modelBuilder);
        }
    }
}
