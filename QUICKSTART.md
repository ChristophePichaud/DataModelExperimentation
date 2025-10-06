# Quick Start Guide - Training Data Model

## Installation

```bash
# Clone repository
git clone https://github.com/ChristophePichaud/DataModelExperimentation.git
cd DataModelExperimentation

# Build solution
dotnet build

# Run example
cd TrainingDataModel.Example
dotnet run
```

## Add to Your Project

### 1. Reference the Library

```bash
dotnet add reference path/to/TrainingDataModel/TrainingDataModel.csproj
```

Or add package reference (if published to NuGet):

```bash
dotnet add package TrainingDataModel
```

### 2. Configure Services

**appsettings.json**:
```json
{
  "ConnectionStrings": {
    "TrainingDb": "Host=localhost;Database=training_db;Username=postgres;Password=yourpassword"
  }
}
```

**Program.cs**:
```csharp
using TrainingDataModel;

var builder = WebApplication.CreateBuilder(args);

// Add TrainingDbContext
builder.Services.AddTrainingDataModel(
    builder.Configuration.GetConnectionString("TrainingDb"));

var app = builder.Build();
```

### 3. Use in Controllers/Services

```csharp
public class CourseService
{
    private readonly TrainingDbContext _context;

    public CourseService(TrainingDbContext context)
    {
        _context = context;
    }

    public async Task<List<TrainingCourse>> GetAllCoursesAsync()
    {
        return await _context.TrainingCourses
            .Include(c => c.Modules)
            .ToListAsync();
    }
}
```

## Common Operations

### Create a New Customer
```csharp
var customer = new Customer
{
    Name = "New Company",
    Email = "contact@company.com",
    Phone = "+1-555-0123",
    Address = "123 Main St"
};

_context.Customers.Add(customer);
await _context.SaveChangesAsync();
```

### Create a Training Course
```csharp
var course = new TrainingCourse
{
    Name = "Azure Fundamentals",
    Description = "Learn cloud basics",
    DurationHours = 24,
    Price = 1500.00m,
    RequiresVm = true
};

_context.TrainingCourses.Add(course);
await _context.SaveChangesAsync();
```

### Add Modules to Course
```csharp
var modules = new[]
{
    new Module
    {
        Name = "Introduction",
        OrderNumber = 1,
        DurationHours = 4,
        TrainingCourseId = course.Id
    },
    new Module
    {
        Name = "Advanced Topics",
        OrderNumber = 2,
        DurationHours = 20,
        TrainingCourseId = course.Id
    }
};

_context.Modules.AddRange(modules);
await _context.SaveChangesAsync();
```

### Track VM Usage
```csharp
var usage = new DailyUsageStatistic
{
    VirtualMachineId = vmId,
    UsageDate = DateTime.Today,
    HoursUsed = 8.5m,
    Cost = 34.00m
};

_context.DailyUsageStatistics.Add(usage);
await _context.SaveChangesAsync();
```

### Create Invoice
```csharp
var invoice = new BillingInvoice
{
    InvoiceNumber = $"INV-{DateTime.Now:yyyyMM}-001",
    CustomerId = customerId,
    InvoiceDate = DateTime.Today,
    DueDate = DateTime.Today.AddDays(30),
    TotalAmount = 5000.00m,
    Status = "Pending"
};

_context.BillingInvoices.Add(invoice);
await _context.SaveChangesAsync();
```

## Querying Data

### Get Courses with Modules
```csharp
var courses = await _context.TrainingCourses
    .Include(c => c.Modules.OrderBy(m => m.OrderNumber))
    .Where(c => c.RequiresVm)
    .ToListAsync();
```

### Get Customer Details
```csharp
var customer = await _context.Customers
    .Include(c => c.Students)
    .Include(c => c.BillingInvoices)
    .Include(c => c.AdminUsers)
    .FirstOrDefaultAsync(c => c.Id == customerId);
```

### Get VM Usage Report
```csharp
var report = await _context.VirtualMachines
    .Include(v => v.VmType)
    .Include(v => v.DailyUsageStatistics)
    .Select(v => new
    {
        v.Name,
        v.Status,
        TypeName = v.VmType.Name,
        TotalHours = v.DailyUsageStatistics.Sum(s => s.HoursUsed),
        TotalCost = v.DailyUsageStatistics.Sum(s => s.Cost)
    })
    .ToListAsync();
```

### Get Pending Invoices
```csharp
var pending = await _context.BillingInvoices
    .Include(i => i.Customer)
    .Where(i => i.Status == "Pending" && i.DueDate < DateTime.Today)
    .OrderBy(i => i.DueDate)
    .ToListAsync();
```

### Get Student Access Rights
```csharp
var student = await _context.Students
    .Include(s => s.RdpFiles)
        .ThenInclude(r => r.VirtualMachine)
            .ThenInclude(v => v.TrainingCourse)
    .FirstOrDefaultAsync(s => s.Id == studentId);
```

## Database Migrations

### Create Initial Migration
```bash
cd TrainingDataModel
dotnet ef migrations add InitialCreate
```

### Update Database
```bash
dotnet ef database update
```

### Rollback Migration
```bash
dotnet ef database update PreviousMigrationName
```

### Generate SQL Script
```bash
dotnet ef migrations script -o migration.sql
```

### Remove Last Migration (if not applied)
```bash
dotnet ef migrations remove
```

## Testing with In-Memory Database

```csharp
// In test project
services.AddDbContext<TrainingDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));
```

## Docker PostgreSQL Setup

```bash
docker run --name training-postgres \
  -e POSTGRES_PASSWORD=yourpassword \
  -e POSTGRES_DB=training_db \
  -p 5432:5432 \
  -d postgres:16
```

Connection string:
```
Host=localhost;Database=training_db;Username=postgres;Password=yourpassword
```

## Environment Variables

### Linux/Mac
```bash
export ConnectionStrings__TrainingDb="Host=localhost;Database=training_db;Username=postgres;Password=yourpassword"
```

### Windows PowerShell
```powershell
$env:ConnectionStrings__TrainingDb="Host=localhost;Database=training_db;Username=postgres;Password=yourpassword"
```

## Troubleshooting

### Connection Issues
- Verify PostgreSQL is running
- Check firewall settings
- Validate connection string
- Ensure database exists

### Migration Errors
- Check model consistency
- Review pending migrations
- Verify database permissions
- Check for conflicting changes

### Build Errors
- Restore NuGet packages: `dotnet restore`
- Clean solution: `dotnet clean`
- Rebuild: `dotnet build`

## Performance Tips

### Use AsNoTracking for Read-Only Queries
```csharp
var courses = await _context.TrainingCourses
    .AsNoTracking()
    .ToListAsync();
```

### Batch Operations
```csharp
_context.Students.AddRange(studentList);
await _context.SaveChangesAsync();
```

### Lazy Loading (if enabled)
```csharp
// Configure in DbContext
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseLazyLoadingProxies();
}
```

## Resources

- [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Npgsql Documentation](https://www.npgsql.org/efcore/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

## Support

For issues or questions:
1. Check the example project
2. Review entity documentation
3. Open an issue on GitHub
