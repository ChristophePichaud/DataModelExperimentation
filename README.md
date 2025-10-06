# DataModelExperimentation

A comprehensive Entity Framework Core data model for PostgreSQL that describes a Training business domain.

## 📋 Overview

This project contains a complete Entity Framework Core model designed for a training business that manages:
- **Customer and Student Management**: Track organizations and their students
- **Training Courses**: Organize courses with modules
- **Virtual Machine Management**: Provision and manage VMs for hands-on training
- **Billing System**: Monthly invoices and usage-based billing
- **Usage Statistics**: Track daily VM usage and costs
- **Access Control**: Manage admin users and RDP file access

## 🏗️ Architecture

The solution consists of two main projects:

### TrainingDataModel
The core library containing:
- **11 Entity Models**: Customer, Student, TrainingCourse, Module, VirtualMachine, VmType, VmOption, BillingInvoice, DailyUsageStatistic, AdminUser, RdpFile
- **DbContext**: Configured for PostgreSQL with Npgsql
- **Entity Configurations**: Fluent API configurations for relationships and constraints
- **Service Extensions**: Easy dependency injection setup

### TrainingDataModel.Example
A sample console application demonstrating:
- Database seeding with sample data
- Complex queries with eager loading
- Real-world usage scenarios

## 🚀 Quick Start

### Prerequisites
- .NET 9.0 SDK or later
- PostgreSQL 12 or later (for production use)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/ChristophePichaud/DataModelExperimentation.git
cd DataModelExperimentation
```

2. Build the solution:
```bash
dotnet build
```

3. Run the example:
```bash
cd TrainingDataModel.Example
dotnet run
```

## 📦 NuGet Packages

The project uses the following packages:
- **Npgsql.EntityFrameworkCore.PostgreSQL** 9.0.4 - PostgreSQL provider for EF Core
- **Microsoft.EntityFrameworkCore.Design** 9.0.9 - EF Core design-time tools

## 🗄️ Entity Relationship Diagram

```
Customer (1) ──< (N) Student
    │                    │
    │                    └──< (N) RdpFile (N) >── VirtualMachine
    │                                                    │
    ├──< (N) AdminUser                                   │
    │                                                    │
    └──< (N) BillingInvoice                             ├──< (N) DailyUsageStatistic
                                                         │
                                                         └──> (1) VmType (1) ──< (N) VmOption

TrainingCourse (1) ──< (N) Module
    └──< (N) VirtualMachine
```

## 📝 Entity Descriptions

### Core Business Entities

#### Customer
Represents organizations using the training services.
- Unique email addresses
- Contains students, admin users, and invoices
- Snake_case column names (e.g., `created_at`)

#### Student
Individuals attending training courses.
- Can be associated with a customer
- Has access to RDP files for VMs
- Unique email constraint

#### TrainingCourse
Available training offerings.
- Contains modules in sequence
- Can require VMs (flag: `requires_vm`)
- Decimal precision for pricing (10,2)

#### Module
Course content divided into learning modules.
- Ordered by `order_number`
- Unique constraint on (course_id, order_number)

### VM Management

#### VmType
Operating system types (Windows, Linux, etc.).
- One-to-many with VmOption and VirtualMachine
- Unique name constraint

#### VmOption
Configuration templates (SKU, Offer, Version, ISO VHD).
- Azure/cloud-specific fields
- Belongs to a VmType

#### VirtualMachine
Actual VM instances for training.
- Tracks IP address and status
- Associated with training courses
- Has usage statistics and RDP files

### Financial & Usage

#### BillingInvoice
Monthly invoices for customers.
- Unique invoice numbers
- Status tracking (Pending, Paid, etc.)
- Decimal precision for amounts (10,2)

#### DailyUsageStatistic
Daily VM usage tracking.
- Unique constraint per (vm_id, usage_date)
- Decimal precision for hours and costs

### Access Management

#### AdminUser
Administrative users for customers and training agency.
- Can be customer admins or agency admins
- Unique email and username constraints

#### RdpFile
Remote Desktop Protocol files for VM access.
- Links students to specific VMs
- Accessible by students and agency admins

## 💻 Usage Examples

### Configure in ASP.NET Core

```csharp
// Program.cs
using TrainingDataModel;

builder.Services.AddTrainingDataModel(
    builder.Configuration.GetConnectionString("TrainingDb"));
```

### Configure with Custom Options

```csharp
services.AddTrainingDataModel(options =>
    options.UseNpgsql(connectionString, 
        npgsqlOptions => 
        {
            npgsqlOptions.EnableRetryOnFailure();
            npgsqlOptions.CommandTimeout(30);
        }));
```

### Query Examples

```csharp
// Get courses with modules
var courses = await context.TrainingCourses
    .Include(c => c.Modules)
    .ToListAsync();

// Get VM usage statistics
var vmStats = await context.VirtualMachines
    .Include(vm => vm.DailyUsageStatistics)
    .Where(vm => vm.Status == "Running")
    .ToListAsync();

// Get customer invoices
var invoices = await context.BillingInvoices
    .Include(i => i.Customer)
    .Where(i => i.Status == "Pending")
    .ToListAsync();
```

## 🛠️ Database Migrations

### Create Initial Migration

```bash
cd TrainingDataModel
dotnet ef migrations add InitialCreate
```

### Update Database

```bash
dotnet ef database update
```

### Generate SQL Script

```bash
dotnet ef migrations script -o migration.sql
```

## 🔧 Configuration

### Connection String Format

```json
{
  "ConnectionStrings": {
    "TrainingDb": "Host=localhost;Database=training_db;Username=postgres;Password=yourpassword"
  }
}
```

### Environment Variables

```bash
export ConnectionStrings__TrainingDb="Host=localhost;Database=training_db;Username=postgres;Password=yourpassword"
```

## 📊 Database Schema

The model generates PostgreSQL tables with:
- **Snake_case naming**: All columns use snake_case (e.g., `customer_id`, `created_at`)
- **Proper indexing**: Unique indexes on emails, invoice numbers, etc.
- **Foreign key constraints**: Enforced relationships with cascading rules
- **Decimal precision**: Financial fields use NUMERIC(10,2) or NUMERIC(8,2)
- **Audit fields**: `created_at` and `updated_at` on all entities

## 🧪 Testing

The example project includes comprehensive demonstrations:

```bash
cd TrainingDataModel.Example
dotnet run
```

This will:
1. Create an in-memory database
2. Seed sample data for all entities
3. Execute 6 different query scenarios
4. Display formatted results

## 📚 Documentation

Detailed documentation is available in:
- [TrainingDataModel/README.md](TrainingDataModel/README.md) - Detailed model documentation
- XML documentation comments in all entity classes
- Example usage in TrainingDataModel.Example

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

## 📄 License

This project is available for use in training and educational contexts.

## 🔗 Links

- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Npgsql Documentation](https://www.npgsql.org/efcore/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
