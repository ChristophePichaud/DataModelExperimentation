# Training Data Model

This project contains an Entity Framework Core model for PostgreSQL that describes a training business domain.

## Overview

The model represents a comprehensive training business system with the following features:
- Customer and student management
- Training courses with modules
- Virtual machine provisioning and management
- Billing and invoicing
- Usage statistics tracking
- Admin user management
- RDP file access for remote connections

## Entity Relationships

### Core Entities

#### Customer
- Represents organizations using the training services
- Has many: Students, AdminUsers, BillingInvoices

#### Student
- Individuals attending training courses
- Belongs to: Customer (optional)
- Has many: RdpFiles

#### TrainingCourse
- Available training offerings
- Has many: Modules, VirtualMachines
- Can optionally require VMs (RequiresVm flag)

#### Module
- Course content divided into modules
- Belongs to: TrainingCourse
- Has order_number for sequencing

### Virtual Machine Management

#### VmType
- VM operating system types (e.g., Windows, Linux)
- Has many: VmOptions, VirtualMachines

#### VmOption
- Configuration options for VMs (SKU, Offer, Version, ISO VHD)
- Belongs to: VmType

#### VirtualMachine
- VM instances for training courses
- Belongs to: TrainingCourse (optional), VmType
- Has many: DailyUsageStatistics, RdpFiles

### Billing & Statistics

#### BillingInvoice
- Monthly invoices for customers
- Belongs to: Customer

#### DailyUsageStatistic
- Daily VM usage tracking
- Belongs to: VirtualMachine
- Tracks hours used and associated costs

### Access Management

#### AdminUser
- Administrative users for customers and training agency
- Belongs to: Customer (optional)
- Has IsTrainingAgencyAdmin flag for agency admins

#### RdpFile
- Remote Desktop Protocol files for VM access
- Belongs to: VirtualMachine, Student (optional)
- Accessible by students and training agency admins

## Usage

### Installation

Add the NuGet packages:
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Configuration

In your application's `Program.cs` or `Startup.cs`:

```csharp
using TrainingDataModel;

// Add to services
builder.Services.AddTrainingDataModel(
    "Host=localhost;Database=training_db;Username=postgres;Password=yourpassword");
```

Or with custom configuration:

```csharp
builder.Services.AddTrainingDataModel(options =>
    options.UseNpgsql(connectionString, 
        npgsqlOptions => 
        {
            npgsqlOptions.EnableRetryOnFailure();
            npgsqlOptions.MigrationsAssembly("YourMigrationsProject");
        }));
```

### Creating Migrations

```bash
# Add initial migration
dotnet ef migrations add InitialCreate --project TrainingDataModel

# Update database
dotnet ef database update --project TrainingDataModel
```

### Using in Code

```csharp
public class TrainingService
{
    private readonly TrainingDbContext _context;

    public TrainingService(TrainingDbContext context)
    {
        _context = context;
    }

    public async Task<List<TrainingCourse>> GetCoursesAsync()
    {
        return await _context.TrainingCourses
            .Include(c => c.Modules)
            .Include(c => c.VirtualMachines)
            .ToListAsync();
    }
}
```

## Database Schema

The model uses PostgreSQL conventions:
- Snake_case column names (e.g., `created_at`, `training_course_id`)
- Lowercase table names
- Proper foreign key constraints
- Unique indexes on email addresses and key identifiers
- Cascade deletes where appropriate

## Key Features

1. **Flexible Customer Hierarchy**: Students and admins can be associated with customers or operate independently
2. **VM Management**: Complete VM lifecycle with types, options, and usage tracking
3. **Billing Integration**: Monthly invoicing system with status tracking
4. **Access Control**: RDP files linked to both VMs and students for controlled access
5. **Audit Fields**: Created_at and updated_at timestamps on all entities
6. **Data Integrity**: Proper foreign key relationships and unique constraints

## Requirements

- .NET 9.0 or later
- PostgreSQL 12 or later
- Entity Framework Core 9.0 or later
- Npgsql 9.0 or later
