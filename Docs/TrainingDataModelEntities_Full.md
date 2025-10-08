# TrainingDataModel Entities — Full Explication

This document provides a comprehensive description of all entities in the TrainingDataModel, including their properties and relationships.

---
## AdminUser
- **Id**: int, Primary key
- **UserId**: int, references User
- **CreatedAt**: DateTime, Account creation date
- **IsActive**: bool, Whether the admin is active

---
## BillingInvoice
- **Id**: int, Primary key
- **Amount**: decimal, Invoice amount
- **Date**: DateTime, Invoice date
- **CustomerId**: int, Linked customer
- **Status**: string, Invoice status (e.g., Paid, Pending)
- **PaidDate**: DateTime, Date paid

---
## Customer
- **Id**: int, Primary key
- **Name**: string, Customer name
- **Email**: string, Customer email address
- **Address**: string, Customer address
- **Phone**: string, Customer phone number

---
## DailyUsageStatistic
- **Id**: int, Primary key
- **Date**: DateTime, Usage date
- **Usage**: decimal, Usage value
- **UserId**: int, Linked user
- **ModuleId**: int, Linked module

---
## Module
- **Id**: int, Primary key
- **Name**: string, Module name
- **Description**: string, Module description

---
## RdpFile
- **Id**: int, Primary key
- **Path**: string, File path
- **UserId**: int, Linked user
- **CreatedAt**: DateTime, Creation date

---
## Student
- **Id**: int, Primary key
- **UserId**: int, references User
- **EnrollmentDate**: DateTime, Enrollment date

---
## Trainer
- **Id**: int, Primary key
- **UserId**: int, references User
- **HireDate**: DateTime, Hire date

---
## TrainingCourse
- **Id**: int, Primary key
- **Title**: string, Course title
- **Description**: string, Course description
- **StartDate**: DateTime, Start date
- **EndDate**: DateTime, End date
# TrainerTrainingCourse
- **TrainerId**: int, Primary key, references Trainer
- **TrainingCourseId**: int, Primary key, references TrainingCourse

---
## User
- **Id**: int, Primary key
- **Name**: string, User name
- **Email**: string, User email address
- **Role**: string, User role (e.g., Student, Trainer, Admin)
- **CreatedAt**: DateTime, Account creation date

---
## VirtualMachine
- **Id**: int, Primary key
- **Name**: string, VM name
- **VmTypeId**: int, Linked VM type
- **VmOptionId**: int, Linked VM option
- **OwnerUserId**: int, Owner user
- **CreatedAt**: DateTime, Creation date

## VirtualMachineModel
- **Id**: int, Primary key
- **Name**: string, VM name

---
## VmOption
- **Id**: int, Primary key
- **OptionName**: string, Option name
- **Description**: string, Option description

---
## VmType
- **Id**: int, Primary key
- **TypeName**: string, Type name
- **Description**: string, Type description

---
# StudentTrainingCourse
- **StudentId**: int, Primary key, references Student
- **TrainingCourseId**: int, Primary key, references TrainingCourse
- **EnrollmentDate**: DateTime, Enrollment date

# Relationships
- **Customer** 1---* **BillingInvoice**: A customer can have multiple invoices.
- **Customer** 1---* **AdminUser**: A customer can have multiple admin users.
- **Customer** 1---* **User**: A customer can have multiple users (if applicable).
- **User** 1---* **AdminUser**: Each admin user is linked to a user.
- **User** 1---* **Student**: Each student is linked to a user.
- **User** 1---* **Trainer**: Each trainer is linked to a user.
- **User** 1---* **DailyUsageStatistic**: A user can have multiple usage statistics.
- **User** 1---* **RdpFile**: A user can have multiple RDP files.
- **User** 1---* **VirtualMachine**: A user can own multiple virtual machines.
- **VirtualMachine** *---1 **VmType**: Each VM has one type.
- **VirtualMachine** *---1 **VmOption**: Each VM has one option.
- **Customer** 1---* **VirtualMachineModel**: Each Customer can create multiple VirtualMachineModel
- **Student** 1---* **StudentTrainingCourse**: A student can be enrolled in multiple courses.
- **TrainingCourse** 1---* **StudentTrainingCourse**: A course can have multiple students enrolled.
- **Trainer** 1---* **TrainerTrainingCourse**: A trainer can teach multiple courses.
- **TrainingCourse** 1---* **TrainerTrainingCourse**: A course can have multiple trainers.
- **Module** 1---* **DailyUsageStatistic**: A module can have multiple usage statistics.
