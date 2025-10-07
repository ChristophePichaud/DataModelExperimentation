using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrainingDataModel;
using TrainingDataModel.Data;
using TrainingDataModel.Entities;

namespace TrainingDataModel.Example;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Training Data Model Example ===\n");

        // Setup dependency injection
        var services = new ServiceCollection();
        
        // Configure logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        // Add the TrainingDbContext with PostgreSQL database
        var connectionString = "Host=localhost;Database=trainingdbExp;Username=postgres;Password=admin";
        services.AddDbContext<TrainingDbContext>(options =>
            options.UseNpgsql(connectionString));

        var serviceProvider = services.BuildServiceProvider();

        // Get the DbContext
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TrainingDbContext>();

        // Seed sample data
        await SeedDataAsync(context);

        // Demonstrate queries
        await DemonstrateQueriesAsync(context);

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static async Task SeedDataAsync(TrainingDbContext context)
    {
        Console.WriteLine("Seeding sample data...\n");

        // Create VM Types
        var windowsType = new VmType
        {
            Name = "Windows",
            Description = "Windows-based virtual machines"
        };

        var linuxType = new VmType
        {
            Name = "Linux",
            Description = "Linux-based virtual machines"
        };

        context.VmTypes.AddRange(windowsType, linuxType);
        await context.SaveChangesAsync();

        // Create VM Options
        var windowsOption = new VmOption
        {
            Name = "Windows Server 2022",
            Sku = "2022-datacenter",
            Offer = "WindowsServer",
            Version = "latest",
            VmTypeId = windowsType.Id
        };

        var linuxOption = new VmOption
        {
            Name = "Ubuntu 22.04 LTS",
            Sku = "22_04-lts",
            Offer = "UbuntuServer",
            Version = "latest",
            VmTypeId = linuxType.Id
        };

        context.VmOptions.AddRange(windowsOption, linuxOption);
        await context.SaveChangesAsync();

        // Create Customer
        var customer = new Customer
        {
            Name = "Acme Corporation",
            Email = "contact@acme.com",
            Phone = "+1-555-0100",
            Address = "123 Business St, Tech City, TC 12345",
            CreatedAt = DateTime.UtcNow
        };

        context.Customers.Add(customer);
        await context.SaveChangesAsync();

        // Create Admin Users
        var adminUser = new AdminUser
        {
            Name = "John Admin",
            Email = "john.admin@acme.com",
            Username = "jadmin",
            CustomerId = customer.Id,
            CreatedAt = DateTime.UtcNow
        };

        var agencyAdmin = new AdminUser
        {
            Name = "Sarah Agency",
            Email = "sarah@trainingagency.com",
            Username = "sagency",
            CreatedAt = DateTime.UtcNow
        };

        context.AdminUsers.AddRange(adminUser, agencyAdmin);
        await context.SaveChangesAsync();
        // Create Users
        var superUser = new User
        {
            Name = "Super Admin2",
            Email = "sa2@corp.com",
            Password = "admin", // Replace with hashed password in production
            UserType = UserType.SuperAdmin,
            CreatedAt = DateTime.UtcNow
        };
        var trainerUser = new User
        {
            Name = "Trainer Bob",
            Email = "bob.trainer@acme.com",
            Password = "trainerpass",
            UserType = UserType.Trainer,
            CreatedAt = DateTime.UtcNow
        };
        context.Users.AddRange(superUser, trainerUser);
        await context.SaveChangesAsync();

        // Create Trainers
        var trainer = new Trainer
        {
            Name = "Bob Trainer",
            CustomerId = customer.Id,
            CreatedAt = DateTime.UtcNow
        };
        context.Trainers.Add(trainer);
        await context.SaveChangesAsync();

        // Create Students
        var student1 = new Student
        {
            Name = "Alice Developer",
            Email = "alice@acme.com",
            Phone = "+1-555-0101",
            CustomerId = customer.Id,
            CreatedAt = DateTime.UtcNow
        };

        var student2 = new Student
        {
            Name = "Bob Engineer",
            Email = "bob@acme.com",
            Phone = "+1-555-0102",
            CustomerId = customer.Id,
            CreatedAt = DateTime.UtcNow
        };

        context.Students.AddRange(student1, student2);
        await context.SaveChangesAsync();

        // Create Training Courses
        var dotnetCourse = new TrainingCourse
        {
            Name = ".NET 9 Fundamentals",
            Description = "Comprehensive introduction to .NET 9 development",
            DurationHours = 40,
            Price = 2500.00m,
            RequiresVm = true,
            CreatedAt = DateTime.UtcNow
        };

        var cloudCourse = new TrainingCourse
        {
            Name = "Cloud Architecture",
            Description = "Learn cloud computing principles and Azure services",
            DurationHours = 32,
            Price = 3000.00m,
            RequiresVm = true,
            CreatedAt = DateTime.UtcNow
        };

        context.TrainingCourses.AddRange(dotnetCourse, cloudCourse);
        await context.SaveChangesAsync();

        // Create Modules
        var modules = new[]
        {
            new Module
            {
                Name = "Introduction to .NET",
                Description = "Overview of the .NET ecosystem",
                OrderNumber = 1,
                DurationHours = 8,
                TrainingCourseId = dotnetCourse.Id,
                CreatedAt = DateTime.UtcNow
            },
            new Module
            {
                Name = "C# Fundamentals",
                Description = "Core C# programming concepts",
                OrderNumber = 2,
                DurationHours = 16,
                TrainingCourseId = dotnetCourse.Id,
                CreatedAt = DateTime.UtcNow
            },
            new Module
            {
                Name = "ASP.NET Core",
                Description = "Building web applications with ASP.NET Core",
                OrderNumber = 3,
                DurationHours = 16,
                TrainingCourseId = dotnetCourse.Id,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Modules.AddRange(modules);
        await context.SaveChangesAsync();

        // Create Virtual Machines
        var vm1 = new VirtualMachine
        {
            Name = "training-vm-001",
            IpAddress = "10.0.1.10",
            Status = "Running",
            TrainingCourseId = dotnetCourse.Id,
            VmTypeId = windowsType.Id,
            CreatedAt = DateTime.UtcNow
        };

        var vm2 = new VirtualMachine
        {
            Name = "training-vm-002",
            IpAddress = "10.0.1.11",
            Status = "Running",
            TrainingCourseId = cloudCourse.Id,
            VmTypeId = linuxType.Id,
            CreatedAt = DateTime.UtcNow
        };

        context.VirtualMachines.AddRange(vm1, vm2);
        await context.SaveChangesAsync();

        // Create RDP Files
        var rdpFile1 = new RdpFile
        {
            FileName = "training-vm-001.rdp",
            FilePath = "/rdp-files/training-vm-001.rdp",
            VirtualMachineId = vm1.Id,
            StudentId = student1.Id,
            CreatedAt = DateTime.UtcNow
        };

        var rdpFile2 = new RdpFile
        {
            FileName = "training-vm-002.rdp",
            FilePath = "/rdp-files/training-vm-002.rdp",
            VirtualMachineId = vm2.Id,
            StudentId = student2.Id,
            CreatedAt = DateTime.UtcNow
        };

        context.RdpFiles.AddRange(rdpFile1, rdpFile2);
        await context.SaveChangesAsync();

        // Create Daily Usage Statistics
        var usageStats = new[]
        {
            new DailyUsageStatistic
            {
                VirtualMachineId = vm1.Id,
                UsageDate = DateTime.UtcNow.AddDays(-1),
                HoursUsed = 8.5m,
                Cost = 34.00m
            },
            new DailyUsageStatistic
            {
                VirtualMachineId = vm1.Id,
                UsageDate = DateTime.UtcNow,
                HoursUsed = 6.0m,
                Cost = 24.00m
            },
            new DailyUsageStatistic
            {
                VirtualMachineId = vm2.Id,
                UsageDate = DateTime.UtcNow.AddDays(-1),
                HoursUsed = 7.0m,
                Cost = 28.00m
            }
        };

        context.DailyUsageStatistics.AddRange(usageStats);
        await context.SaveChangesAsync();

        // Create Billing Invoice
        var invoice = new BillingInvoice
        {
            InvoiceNumber = "INV-2024-001",
            CustomerId = customer.Id,
            InvoiceDate = DateTime.UtcNow.Date,
            DueDate = DateTime.UtcNow.Date.AddDays(30),
            TotalAmount = 5500.00m,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        context.BillingInvoices.Add(invoice);
        await context.SaveChangesAsync();

        Console.WriteLine("Sample data seeded successfully!\n");
    }

    static async Task DemonstrateQueriesAsync(TrainingDbContext context)
    {
        Console.WriteLine("=== Query Examples ===\n");

        // 1. Get all training courses with their modules
        Console.WriteLine("1. Training Courses with Modules:");
        var coursesWithModules = await context.TrainingCourses
            .Include(c => c.Modules)
            .ToListAsync();

        foreach (var course in coursesWithModules)
        {
            Console.WriteLine($"   - {course.Name} ({course.DurationHours}h, ${course.Price})");
            foreach (var module in course.Modules.OrderBy(m => m.OrderNumber))
            {
                Console.WriteLine($"     {module.OrderNumber}. {module.Name} ({module.DurationHours}h)");
            }
        }

        // 2. Get customer with their students
        Console.WriteLine("\n2. Customers and Students:");
        var customersWithStudents = await context.Customers
            .Include(c => c.Students)
            .ToListAsync();

        foreach (var customer in customersWithStudents)
        {
            Console.WriteLine($"   - {customer.Name} ({customer.Email})");
            Console.WriteLine($"     Students: {string.Join(", ", customer.Students.Select(s => s.Name))}");
        }

        // 3. Get VMs with their type and usage statistics
        Console.WriteLine("\n3. Virtual Machines with Usage:");
        var vmsWithStats = await context.VirtualMachines
            .Include(vm => vm.VmType)
            .Include(vm => vm.DailyUsageStatistics)
            .ToListAsync();

        foreach (var vm in vmsWithStats)
        {
            var totalHours = vm.DailyUsageStatistics.Sum(s => s.HoursUsed);
            var totalCost = vm.DailyUsageStatistics.Sum(s => s.Cost);
            Console.WriteLine($"   - {vm.Name} ({vm.VmType.Name}) - {vm.Status}");
            Console.WriteLine($"     Total Usage: {totalHours}h, Total Cost: ${totalCost}");
        }

        // 4. Get students with their RDP files
        Console.WriteLine("\n4. Students with RDP Access:");
        var studentsWithRdp = await context.Students
            .Include(s => s.RdpFiles)
                .ThenInclude(r => r.VirtualMachine)
            .ToListAsync();

        foreach (var student in studentsWithRdp)
        {
            Console.WriteLine($"   - {student.Name} ({student.Email})");
            foreach (var rdp in student.RdpFiles)
            {
                Console.WriteLine($"     RDP: {rdp.FileName} -> {rdp.VirtualMachine.Name}");
            }
        }

        // 5. Get pending invoices
        Console.WriteLine("\n5. Pending Invoices:");
        var pendingInvoices = await context.BillingInvoices
            .Include(i => i.Customer)
            .Where(i => i.Status == "Pending")
            .ToListAsync();

        foreach (var invoice in pendingInvoices)
        {
            Console.WriteLine($"   - {invoice.InvoiceNumber} - {invoice.Customer.Name}");
            Console.WriteLine($"     Amount: ${invoice.TotalAmount}, Due: {invoice.DueDate:yyyy-MM-dd}");
        }

        // 6. Get VM types with their options
        Console.WriteLine("\n6. VM Types and Options:");
        var vmTypesWithOptions = await context.VmTypes
            .Include(vt => vt.VmOptions)
            .ToListAsync();

        foreach (var vmType in vmTypesWithOptions)
        {
            Console.WriteLine($"   - {vmType.Name}");
            foreach (var option in vmType.VmOptions)
            {
                Console.WriteLine($"     * {option.Name} (SKU: {option.Sku}, Offer: {option.Offer})");
            }
        }
    }
}
