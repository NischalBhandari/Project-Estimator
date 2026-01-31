# AI Project Estimation Platform (Backend)

This project is an **AI-powered project estimation and cost planning platform** built using:

- ASP.NET Core Web API (.NET 8)
- PostgreSQL Database
- Entity Framework Core (EF Core)
- AI-assisted task + subtask generation (planned)

The goal of this platform is to allow users to:

- Create projects and tasks
- Generate subtasks using AI
- Estimate time based on resources
- Assign costs to resources and overhead expenses
- Export full project estimates in PDF/Excel formats

---

## Project Setup

### Requirements

Make sure you have the following installed:

- **.NET SDK 8**
- **PostgreSQL Server**
- **pgAdmin / DBeaver** (optional for UI)
- EF Core CLI Tools (`dotnet-ef`)

---

## Installing PostgreSQL

### Option 1: Official Installer (Windows)

Download PostgreSQL:

https://www.postgresql.org/download/windows/

During installation:

- Username: `postgres`
- Default port: `5432`
- Set a password you remember

---

### Option 2: Docker Installation (Recommended for Developers)

```bash
docker run --name pg-estimator \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=planner_db \
  -p 5432:5432 \
  -d postgres
```

---

## Setting Up PostgreSQL in ASP.NET Core

### Install Required NuGet Packages

For **.NET 8**, use version **8.x** packages:

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.4
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.4
```

---

## Configure Connection String

Add this inside `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=planner_db;Username=postgres;Password=postgres123"
  }
}
```

---

## Register PostgreSQL DbContext

In `Program.cs`, configure EF Core:

```csharp
using Microsoft.EntityFrameworkCore;
using MyProjectPlanner.Data;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
```

### What This Does

This line tells ASP.NET Core:

✅ Use PostgreSQL as the database  
✅ Register `AppDbContext` so controllers can access it  
✅ Read DB credentials from `appsettings.json`

---

## Creating Models (Entities)

EF Core requires models before migrations.

### Project Model

File: `Models/Project.cs`

```csharp
using System;
using System.Collections.Generic;

namespace MyProjectPlanner.Models
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation: One Project → Many Tasks
        public List<TaskItem> Tasks { get; set; } = new();
    }
}
```

---

### Task Model

File: `Models/TaskItem.cs`

```csharp
using System;

namespace MyProjectPlanner.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public double EstimatedHours { get; set; }

        // Foreign Key
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
```

---

## Creating DbContext

File: `Data/AppDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using MyProjectPlanner.Models;

namespace MyProjectPlanner.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {}

        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
    }
}
```

---

## Running Migrations

### Install EF Tooling

If `dotnet ef` is not found, install:

```bash
dotnet tool install --global dotnet-ef
```

Verify:

```bash
dotnet ef --version
```

---

### Create the First Migration

```bash
dotnet ef migrations add InitialCreate
```

This generates a migration file that defines database tables.

---

### Apply Migration to PostgreSQL

```bash
dotnet ef database update
```

This creates the actual tables in PostgreSQL:

- Projects
- Tasks
- __EFMigrationsHistory

---

## Viewing Tables in pgAdmin

To see table structure:

```
Servers
 → Databases
   → planner_db
     → Schemas
       → public
         → Tables
           → Projects
             → Columns
```

To see data:

Right-click table → View/Edit Data → All Rows

To view SQL definition:

Right-click table → Scripts → CREATE Script

---

## Updating Models Later

When adding new fields to a model:

### Step 1: Modify C# Model

```csharp
public decimal Budget { get; set; }
public string CurrencyCode { get; set; } = "NPR";
```

### Step 2: Create Migration

```bash
dotnet ef migrations add AddBudgetToProject
```

### Step 3: Update Database

```bash
dotnet ef database update
```

---

## Planned Next Features

- AI task and subtask generation using OpenAI API
- Resource cost allocation (hourly rates)
- Overhead costs (servers, laptops, electricity)
- Estimate export (PDF, Excel, CSV)
- SaaS subscription tiers

---

## Future Database Entities

Upcoming models will include:

- SubTask
- Resource
- CostItem
- EstimateSummary
- ExportHistory

---

## Author

Backend setup and idea by: **Nischal Bhandari**  
Platform: BoomConsole Project Estimation Tool

---
