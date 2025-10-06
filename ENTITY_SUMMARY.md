# Training Data Model - Entity Relationship Summary

## Entity Overview

This document provides a comprehensive overview of all entities in the Training Data Model.

## Entity List

### 1. Customer
**Purpose**: Organizations using training services  
**Key Fields**: Name, Email, Phone, Address  
**Relationships**:
- Has many Students
- Has many AdminUsers  
- Has many BillingInvoices

### 2. Student
**Purpose**: Individuals attending training courses  
**Key Fields**: Name, Email, Phone  
**Relationships**:
- Belongs to Customer (optional)
- Has many RdpFiles

### 3. TrainingCourse
**Purpose**: Available training offerings  
**Key Fields**: Name, Description, DurationHours, Price, RequiresVm  
**Relationships**:
- Has many Modules
- Has many VirtualMachines

### 4. Module
**Purpose**: Course content divided into learning modules  
**Key Fields**: Name, Description, OrderNumber, DurationHours  
**Relationships**:
- Belongs to TrainingCourse

### 5. VmType
**Purpose**: VM operating system types (Windows, Linux)  
**Key Fields**: Name, Description  
**Relationships**:
- Has many VmOptions
- Has many VirtualMachines

### 6. VmOption
**Purpose**: VM configuration options (SKU, Offer, Version, ISO VHD)  
**Key Fields**: Name, Sku, Offer, Version, IsoVhd  
**Relationships**:
- Belongs to VmType

### 7. VirtualMachine
**Purpose**: VM instances for training courses  
**Key Fields**: Name, IpAddress, Status  
**Relationships**:
- Belongs to TrainingCourse (optional)
- Belongs to VmType
- Has many DailyUsageStatistics
- Has many RdpFiles

### 8. BillingInvoice
**Purpose**: Monthly invoices for customers  
**Key Fields**: InvoiceNumber, InvoiceDate, DueDate, TotalAmount, Status  
**Relationships**:
- Belongs to Customer

### 9. DailyUsageStatistic
**Purpose**: Daily VM usage tracking  
**Key Fields**: UsageDate, HoursUsed, Cost  
**Relationships**:
- Belongs to VirtualMachine

### 10. AdminUser
**Purpose**: Administrative users for customers and training agency  
**Key Fields**: Name, Email, Username, IsTrainingAgencyAdmin  
**Relationships**:
- Belongs to Customer (optional)

### 11. RdpFile
**Purpose**: Remote Desktop Protocol files for VM access  
**Key Fields**: FileName, FilePath  
**Relationships**:
- Belongs to VirtualMachine
- Belongs to Student (optional)

## Key Features

### Unique Constraints
- Customer: Email
- Student: Email
- BillingInvoice: InvoiceNumber
- VmType: Name
- AdminUser: Email, Username
- Module: (TrainingCourseId, OrderNumber)
- DailyUsageStatistic: (VirtualMachineId, UsageDate)

### Delete Behaviors
- **Cascade**: BillingInvoice (from Customer), Module (from TrainingCourse), DailyUsageStatistic (from VirtualMachine), RdpFile (from VirtualMachine), VmOption (from VmType)
- **SetNull**: Student (from Customer), AdminUser (from Customer), VirtualMachine (from TrainingCourse), RdpFile (from Student)
- **Restrict**: VirtualMachine (from VmType)

### Decimal Precision
- TrainingCourse.Price: NUMERIC(10, 2)
- BillingInvoice.TotalAmount: NUMERIC(10, 2)
- DailyUsageStatistic.HoursUsed: NUMERIC(8, 2)
- DailyUsageStatistic.Cost: NUMERIC(10, 2)

### Audit Fields
All entities include:
- `created_at`: Timestamp when record was created
- `updated_at`: Timestamp when record was last updated (nullable)

## Database Naming Conventions

- **Tables**: Lowercase with underscores (e.g., `training_courses`)
- **Columns**: Snake_case (e.g., `customer_id`, `created_at`)
- **Foreign Keys**: Referenced table name + `_id` (e.g., `training_course_id`)

## Common Query Patterns

### 1. Get Course with Modules
```csharp
var course = await context.TrainingCourses
    .Include(c => c.Modules)
    .FirstOrDefaultAsync(c => c.Id == courseId);
```

### 2. Get Customer with Invoices
```csharp
var customer = await context.Customers
    .Include(c => c.BillingInvoices)
    .FirstOrDefaultAsync(c => c.Id == customerId);
```

### 3. Get VM with Usage Statistics
```csharp
var vm = await context.VirtualMachines
    .Include(v => v.DailyUsageStatistics)
    .Include(v => v.VmType)
    .FirstOrDefaultAsync(v => v.Id == vmId);
```

### 4. Get Student with RDP Access
```csharp
var student = await context.Students
    .Include(s => s.RdpFiles)
        .ThenInclude(r => r.VirtualMachine)
    .FirstOrDefaultAsync(s => s.Id == studentId);
```

### 5. Get All VM Types with Options
```csharp
var vmTypes = await context.VmTypes
    .Include(vt => vt.VmOptions)
    .ToListAsync();
```

## Integration Points

### Billing Integration
- BillingInvoices track monthly charges
- DailyUsageStatistics provide detailed cost breakdown
- Can aggregate usage costs per customer

### Access Control
- AdminUsers manage customers or entire agency
- RdpFiles link students to specific VMs
- Training agency admins have system-wide access

### Resource Management
- VmTypes define available OS platforms
- VmOptions specify configuration templates
- VirtualMachines represent actual instances
- Usage tracking enables cost monitoring

## Migration Strategy

### Initial Setup
1. Create database in PostgreSQL
2. Run `dotnet ef migrations add InitialCreate`
3. Run `dotnet ef database update`

### Updating Schema
1. Modify entity models
2. Run `dotnet ef migrations add <MigrationName>`
3. Review generated migration
4. Run `dotnet ef database update`

### Production Deployment
1. Generate SQL script: `dotnet ef migrations script`
2. Review SQL for production compatibility
3. Apply using database deployment tools
4. Backup data before major changes
