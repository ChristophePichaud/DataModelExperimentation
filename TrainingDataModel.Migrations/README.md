# TrainingDataModel.Migrations

This project contains Entity Framework Core migrations for the TrainingDataModel.

## Usage

- Reference the main data model project.
- Use `dotnet ef migrations add <MigrationName> --project TrainingDataModel.Migrations --startup-project TrainingDataModel.Example` to create migrations.
- Use `dotnet ef database update --project TrainingDataModel.Migrations --startup-project TrainingDataModel.Example` to apply migrations.

## Structure
- `TrainingDataModel.Migrations.csproj`: Migration project file.
- Migrations will be generated in this folder.

## Requirements
- .NET 9.0
- Entity Framework Core 9.x
- Npgsql.EntityFrameworkCore.PostgreSQL 9.x

See the main solution for details on the EF model and context.
