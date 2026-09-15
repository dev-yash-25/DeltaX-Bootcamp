# Entity Framework Core (EF Core)

## Index

- [1. What is Entity Framework Core?](#1-what-is-entity-framework-core)
- [2. Why EF Core?](#2-why-ef-core)
- [3. ORM — Object-Relational Mapping](#3-orm--object-relational-mapping)
- [4. EF Core Development Approaches](#4-ef-core-development-approaches)
  - [4.1 Code-First](#41-code-first)
  - [4.2 Database-First](#42-database-first)
- [5. Installing EF Core](#5-installing-ef-core)
- [6. DbContext](#6-dbcontext)
  - [6.1 Creating the DbContext](#61-creating-the-dbcontext)
  - [6.2 DbSet and Database Tables](#62-dbset-and-database-tables)
- [7. Database Connection String](#7-database-connection-string)
  - [7.1 Using OnConfiguring](#71-using-onconfiguring)
  - [7.2 Registering DbContext in Startup.cs](#72-registering-dbcontext-in-startupcs)
  - [7.3 Reading Connection String from appsettings.json](#73-reading-connection-string-from-appsettingsjson)
- [8. EF Core Migrations](#8-ef-core-migrations)
  - [8.1 Add a Migration](#81-add-a-migration)
  - [8.2 Up() and Down()](#82-up-and-down)
  - [8.3 Update the Database](#83-update-the-database)
  - [8.4 Migration History](#84-migration-history)
- [9. Complete EF Core Flow](#9-complete-ef-core-flow)
- [10. Important Commands Cheat Sheet](#10-important-commands-cheat-sheet)

<br>

---

<br>

## 1. What is Entity Framework Core?

**Entity Framework Core (EF Core)** is Microsoft's official **Object-Relational Mapping (ORM)** technology for .NET applications.

It provides a bridge between:

```text
.NET Application
      ↕
   EF Core
      ↕
Relational Database
```

For example:

```text
ASP.NET Core Web API
        ↓
     EF Core
        ↓
    SQL Server
```

Real-world applications need **data persistence**. Instead of storing data only in memory, applications need a database to store and retrieve information.

<br>

<div align="center">
  <img width="600" alt="Entity Framework Core overview" src="https://github.com/user-attachments/assets/6cbb1886-e0fc-4d79-9bea-beeb116f2fd8" />
</div>

<br>

---

<br>

## 2. Why EF Core?

### 2.1 Data Persistence

An application cannot rely only on in-memory data.

```text
Application Memory
       ↓
Data lost when application stops
```

A database provides persistent storage:

```text
Application
     ↓
 Database
     ↓
Persistent data
```

### 2.2 Database Interaction

ASP.NET Core applications need a technology/framework to communicate with relational databases such as:

- SQL Server
- MySQL
- Other RDBMS

**EF Core** provides this database interaction layer.

### 2.3 Open Source and Database Support

EF Core is:

- Open source
- Microsoft's official ORM for .NET
- Compatible with relational databases through database providers

<br>

---

<br>

## 3. ORM — Object-Relational Mapping

**ORM** stands for **Object-Relational Mapping**.

It bridges two different perspectives:

```text
Object-Oriented Application
          ↕
        ORM
          ↕
Relational Database
```

### Mapping

EF Core maps application objects to database structures.

| Application | Database |
|---|---|
| Class | Table |
| Property | Column |
| Object | Row |

Example:

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
```

Conceptually:

```text
Book class
   ↓
Books table

Id          → ID
Title       → Title
Description → Description
```

Without an ORM, manually creating and maintaining corresponding classes and database mappings for every table can become repetitive and difficult.

> [!Important]
> EF Core allows developers to work with database data using .NET objects instead of manually handling every database interaction.

<br>

---

<br>

## 4. EF Core Development Approaches

EF Core supports two major approaches:

1. **Code-First**
2. **Database-First**

<br>

### 4.1 Code-First

In **Code-First**, you start with your application classes.

```text
C# Classes
    ↓
EF Core
    ↓
Database Schema
```

### Steps

1. Create entity classes.
2. Define their properties.
3. Configure the model.
4. Create migrations.
5. Apply migrations to generate/update the database.

Example:

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
```

> [!Tip]
> Code-First is useful when you prefer designing the application's data model primarily through code.

<br>

### 4.2 Database-First

In **Database-First**, the database already exists.

```text
Existing Database
       ↓
    EF Core
       ↓
C# Classes / Models
```

EF Core commands can generate classes and properties based on the existing database structure.

### When to Use

Database-First is useful when:

- The database already exists.
- You are working with an existing database schema.
- You want EF Core to generate model classes from that schema.

### Comparison

| | Code-First | Database-First |
|---|---|---|
| Starting point | C# classes | Existing database |
| Main direction | Code → Database | Database → Code |
| Common use | New applications | Existing databases |

<br>

---

<br>

## 5. Installing EF Core

For the ASP.NET Core 5.0 + SQL Server setup in this tutorial, install these NuGet packages.

### 5.1 Microsoft.EntityFrameworkCore.SqlServer

Used for connecting EF Core to **SQL Server**.

It also brings required dependencies such as:

- `Microsoft.EntityFrameworkCore.Relational`
- `Microsoft.Data.SqlClient`
- `Microsoft.EntityFrameworkCore`

### 5.2 Microsoft.EntityFrameworkCore.Tools

Required for EF Core-related commands such as:

```text
Add-Migration
Update-Database
```

### 5.3 Microsoft.EntityFrameworkCore.Design

Required for creating and managing **migrations**.

### Installation Steps

1. Right-click the project in **Solution Explorer**.
2. Select **Manage NuGet Packages**.
3. Open the **Browse** tab.
4. Search for the package.
5. Select **Install**.
6. Accept dependency changes if prompted.
7. Repeat for the remaining packages.

> [!Important]
> For this tutorial's SQL Server setup, the three key packages are:
>
> `Microsoft.EntityFrameworkCore.SqlServer`  
> `Microsoft.EntityFrameworkCore.Tools`  
> `Microsoft.EntityFrameworkCore.Design`

<br>

---

<br>

## 6. DbContext

`DbContext` is one of the core components of EF Core.

It is responsible for working with the database and the application's EF Core model.

Think of it as the main bridge between:

```text
Application Models
        ↕
    DbContext
        ↕
     Database
```

<br>

### 6.1 Creating the DbContext

A common project structure is:

```text
BookStore.API
│
├── Controllers
├── Models
├── Repository
├── Data
│   └── BookStoreContext.cs
└── Startup.cs
```

Create a `Data` folder and add:

```text
BookStoreContext.cs
```

The context inherits from:

```csharp
DbContext
```

and accepts:

```csharp
DbContextOptions<BookStoreContext>
```

General structure:

```csharp
using Microsoft.EntityFrameworkCore;

public class BookStoreContext : DbContext
{
    public BookStoreContext(DbContextOptions<BookStoreContext> options)
        : base(options)
    {
    }
}
```

The options are passed to the base `DbContext`.

<br>

### 6.2 DbSet and Database Tables

Suppose the application has:

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
```

Add a `DbSet` to the context:

```csharp
public DbSet<Book> Books { get; set; }
```

Conceptually:

```text
Book class
    ↓
DbSet<Book> Books
    ↓
Books table
```

The `DbSet` provides EF Core with the entity set that participates in the model/database mapping.

In the tutorial, the property name `Books` is used for the table name.

<br>

---

<br>

## 7. Database Connection String

EF Core needs to know **which database it should connect to**.

Example:

```text
Server=.;
Database=BookStoreApi;
Integrated Security=True
```

### Important Parts

```text
Server=.
       ↓
Local SQL Server

Database=BookStoreApi
       ↓
Database name

Integrated Security=True
       ↓
Windows authentication
```

<br>

### 7.1 Using OnConfiguring

One way to configure the database connection is by overriding `OnConfiguring()` inside the context.

Conceptually:

```csharp
protected override void OnConfiguring(
    DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer(
        "Server=.;Database=BookStoreApi;Integrated Security=True");
}
```

The tutorial notes that the database name should be defined and `Integrated Security=True` can be used for local Windows authentication.

> [!Important]
> Hardcoding a connection string directly inside source code is not a good real-world practice.

<br>

### 7.2 Registering DbContext in Startup.cs

Creating the `DbContext` class is not enough.

ASP.NET Core's DI container must also know how to create it.

Register it inside:

```text
Startup.cs
    ↓
ConfigureServices()
```

Conceptually:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<BookStoreContext>(
        options => options.UseSqlServer(
            Configuration.GetConnectionString("BookStoreDB")
        )
    );

    services.AddControllers();
}
```

The tutorial also suggests moving connection configuration to `Startup.cs` instead of keeping it inside `OnConfiguring()`.

<br>

### 7.3 Reading Connection String from appsettings.json

Instead of hardcoding the connection string, store it in:

```text
appsettings.json
```

Example:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "ConnectionStrings": {
    "BookStoreDB": "Server=.;Database=BookStoreApi;Integrated Security=True"
  },
  "AllowedHosts": "*"
}
```

### Accessing Configuration

ASP.NET Core provides:

```csharp
IConfiguration
```

which can be used to access configuration values.

Retrieve the connection string using:

```csharp
Configuration.GetConnectionString("BookStoreDB")
```

### Complete Registration

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<BookStoreContext>(
            options => options.UseSqlServer(Configuration.GetConnectionString("BookStoreDB"))
            );
    services.AddControllers();
    services.AddTransient<IBookRepository, BookRepository>();
}
```

Flow:

```text
appsettings.json
      ↓
IConfiguration
      ↓
GetConnectionString("BookStoreDB")
      ↓
UseSqlServer(...)
      ↓
BookStoreContext
      ↓
SQL Server
```

> [!Tip]
> Keeping connection strings in configuration improves maintainability and avoids hardcoding environment-specific database settings directly into the context.

<br>

---

<br>

## 8. EF Core Migrations

**Migrations** allow EF Core to track changes to the application's model and apply those changes to the database schema.

For a Code-First workflow:

```text
C# Model
   ↓
Migration
   ↓
Database Schema
```

Suppose initially:

```text
Book
├── ID
├── Title
└── Description
```

Later:

```text
Book
├── ID
├── Title
├── Description
└── Price
```

A new migration can represent this schema change.

<br>

<div align="center">
  <img width="600" alt="EF Core migration" src="https://github.com/user-attachments/assets/c32f7f4b-2bb7-4d52-85fc-6ea0870cb550" />
</div>

<br>

### 8.1 Add a Migration

Use the Package Manager Console.

Command:

```powershell
Add-Migration <MigrationName>
```

Example:

```powershell
add-migration init
```

This generates a:

```text
Migrations
```

folder.

The migration represents the changes EF Core needs to apply to the database.

### Typical Flow

```text
Modify C# model
      ↓
Add-Migration
      ↓
Migration generated
      ↓
Update-Database
      ↓
Database updated
```

<br>

### 8.2 Up() and Down()

A migration contains two important methods.

#### Up()

Defines changes that should be **applied**.

```text
Up()
 ↓
Apply schema changes
```

#### Down()

Defines changes that should be **reverted**.

```text
Down()
 ↓
Revert schema changes
```

So:

```text
Up()   → Apply migration
Down() → Revert migration
```

### Primary Key Convention

The tutorial notes that if an entity has a property named:

```csharp
ID
```

EF Core automatically treats it as the primary key.

<br>

### 8.3 Update the Database

After creating the migration:

```powershell
Update-Database
```

Example:

```powershell
update-database
```

Flow:

```text
Migration
    ↓
Update-Database
    ↓
SQL Server
    ↓
Database schema updated
```

After this, the database appears in SQL Server Management Studio (SSMS).

<br>

### 8.4 Migration History

After applying migrations, SQL Server contains the application tables and an EF Core migration history table:

```text
__EFMigrationsHistory
```

This table tracks which migrations have already been applied.

Example:

```text
SQL Server Database
│
├── Books
│
└── __EFMigrationsHistory
```

This allows EF Core to determine which migrations are pending when the database is updated again.

### Future Changes

Whenever you:

- Add a new entity/table.
- Add a property.
- Modify the model/schema.

create another migration and update the database.

```text
Change Model
     ↓
Add-Migration NewChange
     ↓
Update-Database
     ↓
Database synchronized
```

> [!Important]
> `Add-Migration` creates the migration instructions.  
> `Update-Database` applies pending migrations to the database.

<br>

---

<br>

## 9. Complete EF Core Flow

For the BookStore.API application:

```text
                 BOOKSTORE.API
                      │
                      ↓
                 C# Models
                      │
                      ↓
                 DbContext
                      │
                      ↓
              EF Core Mapping
                      │
                      ↓
                 Migration
                      │
                      ↓
              Update-Database
                      │
                      ↓
                 SQL Server
```

### Application Runtime Flow

```text
Controller
    ↓
Repository
    ↓
DbContext
    ↓
EF Core
    ↓
SQL Server
    ↓
Data
    ↑
EF Core maps database data
    ↑
DbContext
    ↑
Repository
    ↑
Controller
    ↑
API Response
```

### Code-First Development Flow

```text
1. Create Model
       ↓
2. Create DbContext
       ↓
3. Configure SQL Server
       ↓
4. Register DbContext in DI
       ↓
5. Add Migration
       ↓
6. Update Database
       ↓
7. Database Created / Updated
```

<br>

---

<br>

## 10. Important Commands Cheat Sheet

| Command | Purpose |
|---|---|
| `Add-Migration Initial` | Creates a new migration |
| `Update-Database` | Applies pending migrations to the database |

### Important EF Core Components

| Component | Responsibility |
|---|---|
| **Entity / Model** | Represents application data |
| **`DbContext`** | Main EF Core database/model context |
| **`DbSet<T>`** | Represents an entity set/table |
| **Connection String** | Defines database connection information |
| **Migration** | Records model/schema changes |
| **`Up()`** | Applies migration changes |
| **`Down()`** | Reverts migration changes |
| **`__EFMigrationsHistory`** | Tracks applied migrations |

### Final Mental Model

```text
                 ASP.NET CORE API
                        │
                        ↓
                   Repository
                        │
                        ↓
                    DbContext
                        │
                        ↓
                     EF Core
                  ↙           ↘
            Model Mapping    Migrations
                  ↓             ↓
              SQL Server ← Update-Database
```

> [!Important]
> **EF Core = ORM + database interaction + model mapping + migrations.**
>
> The key Code-First cycle is:
>
> `Model → Migration → Update-Database → SQL Server`
