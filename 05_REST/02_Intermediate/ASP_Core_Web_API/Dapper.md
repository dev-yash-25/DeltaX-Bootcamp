# Dapper — Complete Notes

## Index

- [1. What is Dapper?](#1-what-is-dapper)
  - [1.1 Dapper as a Micro-ORM](#11-dapper-as-a-micro-orm)
  - [1.2 Dapper vs EF Core](#12-dapper-vs-ef-core)
  - [1.3 Basic Dapper Flow](#13-basic-dapper-flow)
- [2. Setup Dapper](#2-setup-dapper)
  - [2.1 NuGet Package](#21-nuget-package)
  - [2.2 Connection String](#22-connection-string)
  - [2.3 Create Database Connection](#23-create-database-connection)
  - [2.4 General Repository Template](#24-general-repository-template)
- [3. Core Dapper Methods](#3-core-dapper-methods)
  - [3.1 `Execute`](#31-execute)
  - [3.2 `ExecuteAsync`](#32-executeasync)
  - [3.3 `ExecuteScalar`](#33-executescalar)
  - [3.4 `ExecuteScalarAsync`](#34-executescalarasync)
  - [3.5 `Query`](#35-query)
  - [3.6 `QueryAsync`](#36-queryasync)
  - [3.7 `QueryFirst`](#37-queryfirst)
  - [3.8 `QueryFirstOrDefault`](#38-queryfirstordefault)
  - [3.9 `QuerySingle`](#39-querysingle)
  - [3.10 `QuerySingleOrDefault`](#310-querysingleordefault)
  - [3.11 `QueryMultiple`](#311-querymultiple)
  - [3.12 `ExecuteReader`](#312-executereader)
- [4. Querying Data](#4-querying-data)
  - [4.1 Query a Single Row](#41-query-a-single-row)
  - [4.2 Query Multiple Rows](#42-query-multiple-rows)
  - [4.3 Query Specific Columns](#43-query-specific-columns)
  - [4.4 Mapping Rules](#44-mapping-rules)
- [5. INSERT, UPDATE & DELETE](#5-insert-update--delete)
  - [5.1 INSERT](#51-insert)
  - [5.2 Get Generated ID](#52-get-generated-id)
  - [5.3 UPDATE](#53-update)
  - [5.4 DELETE](#54-delete)
- [6. Parameters in Dapper](#6-parameters-in-dapper)
  - [6.1 Anonymous Object](#61-anonymous-object)
  - [6.2 POCO Object](#62-poco-object)
  - [6.3 `DynamicParameters`](#63-dynamicparameters)
  - [6.4 Multiple Parameters](#64-multiple-parameters)
  - [6.5 List Parameters / `IN`](#65-list-parameters--in)
- [7. SQL Injection](#7-sql-injection)
- [8. Buffered vs Unbuffered Queries](#8-buffered-vs-unbuffered-queries)
- [9. `ExecuteReader`](#9-executereader)
- [10. Multiple Result Sets — `QueryMultiple`](#10-multiple-result-sets--querymultiple)
- [11. Stored Procedures](#11-stored-procedures)
  - [11.1 Execute a Stored Procedure](#111-execute-a-stored-procedure)
  - [11.2 Stored Procedure Returning Rows](#112-stored-procedure-returning-rows)
  - [11.3 Output Parameters](#113-output-parameters)
- [12. Table-Valued Parameters](#12-table-valued-parameters)
- [13. SQL Functions](#13-sql-functions)
  - [13.1 Scalar SQL Function](#131-scalar-sql-function)
  - [13.2 Table-Valued Function](#132-table-valued-function)
- [14. Transactions](#14-transactions)
  - [14.1 Why Transactions](#141-why-transactions)
  - [14.2 Transaction Template](#142-transaction-template)
- [15. Common Dapper Templates](#15-common-dapper-templates)
  - [15.1 GET All](#151-get-all)
  - [15.2 GET By ID](#152-get-by-id)
  - [15.3 POST](#153-post)
  - [15.4 PUT](#154-put)
  - [15.5 DELETE](#155-delete)
- [16. Dapper in ASP.NET Core Web API](#16-dapper-in-aspnet-core-web-api)
  - [16.1 Recommended Structure](#161-recommended-structure)
  - [16.2 Repository Interface](#162-repository-interface)
  - [16.3 Repository Implementation](#163-repository-implementation)
  - [16.4 Controller](#164-controller)
  - [16.5 DI Registration](#165-di-registration)
- [17. Method Selection Cheat Sheet](#17-method-selection-cheat-sheet)
- [18. Important Dapper Concepts](#18-important-dapper-concepts)
- [19. Final Mental Model](#19-final-mental-model)


<br>

---

<br>

# 1. What is Dapper?

Dapper is a lightweight **Micro-ORM (Object-Relational Mapper)** for .NET.

It sits between your C# application and a relational database and helps you:

- Execute SQL queries.
- Execute stored procedures.
- Pass parameters safely.
- Map query results to C# objects.
- Perform INSERT, UPDATE and DELETE operations.
- Read multiple result sets.
- Work with transactions.

Unlike a full ORM such as EF Core, Dapper does **not** try to hide SQL from you.

### Mental Model

```text
C# Application
      ↓
    Dapper
      ↓
ADO.NET Connection
      ↓
SQL Server
```

Dapper is essentially a set of extension methods over ADO.NET database connections.


<br>

---

<br>

## 1.1 Dapper as a Micro-ORM

An ORM generally maps:

```text
Database Table
      ↕
C# Class

Database Row
      ↕
C# Object
```

Dapper performs this mapping while allowing you to write the SQL yourself.

Example:

```sql
SELECT Id, Title, Description
FROM Books
WHERE Id = @Id
```

Dapper can map the result directly to:

```csharp
Book
```

### Why "Micro-ORM"?

Dapper is intentionally lightweight.

It does not provide the same level of abstraction/features as EF Core such as:

- Change tracking.
- LINQ-to-SQL translation.
- Migrations.
- Full entity relationship management.

Instead:

```text
You control SQL
      +
Dapper handles parameterization + execution + mapping
```


<br>

---

<br>

## 1.2 Dapper vs EF Core

| Feature | Dapper | EF Core |
|---|---|---|
| Type | Micro-ORM | Full ORM |
| SQL | You generally write SQL | EF can generate SQL |
| Mapping | Automatic result mapping | Entity mapping |
| Change Tracking | No | Yes |
| LINQ | No LINQ-to-SQL abstraction | Yes |
| Migrations | No | Yes |
| Performance overhead | Low | Higher abstraction |
| Stored procedures | Very convenient | Supported |
| SQL control | High | Lower/direct SQL also possible |
| Learning SQL | Important | Less necessary for basic CRUD |

### Simple distinction

```text
EF Core
C# → EF Core → SQL → Database

Dapper
C# → SQL + Dapper → Database
```


<br>

---

<br>

## 1.3 Basic Dapper Flow

```text
Controller
    ↓
Repository
    ↓
IDbConnection
    ↓
Dapper Method
    ↓
SQL Query / Stored Procedure
    ↓
SQL Server
    ↓
Rows / Scalar / Affected Rows
    ↓
Dapper Mapping
    ↓
C# Object
```


<br>

---

<br>

# 2. Setup Dapper

## 2.1 NuGet Package

Install:

```text
Dapper
```

For SQL Server, also use the appropriate SQL Server ADO.NET provider, commonly:

```text
Microsoft.Data.SqlClient
```

or, depending on the application's version/setup:

```text
System.Data.SqlClient
```

The important point is:

```text
Dapper
   +
SQL Server provider
   +
Connection string
```


<br>

---

<br>

## 2.2 Connection String

Example `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "BookStoreDB": "Server=.;Database=BookStoreDB;Trusted_Connection=True;"
  }
}
```

For SQL authentication, a connection string may look like:

```json
{
  "ConnectionStrings": {
    "BookStoreDB": "Server=.;Database=BookStoreDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

Never hardcode production credentials in source code.


<br>

---

<br>

## 2.3 Create Database Connection

Basic connection:

```csharp
using System.Data;
using Microsoft.Data.SqlClient;

IDbConnection db =
    new SqlConnection(connectionString);
```

Commonly:

```csharp
using var connection = new SqlConnection(connectionString);

connection.Open();

// Dapper operation
```

For async operations:

```csharp
using var connection = new SqlConnection(connectionString);

await connection.OpenAsync();
```

[!Important]

Dapper does not create the database connection for you. It operates on an ADO.NET connection such as `SqlConnection`.


<br>

---

<br>

## 2.4 General Repository Template

A common pattern:

```csharp
public class BookRepository : IBookRepository
{
    private readonly IConfiguration _configuration;

    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private IDbConnection CreateConnection()
    {
        return new SqlConnection(
            _configuration.GetConnectionString("BookStoreDB")
        );
    }
}
```

Then:

```csharp
using var connection = CreateConnection();
```

and execute Dapper operations on it.


<br>

---

<br>

# 3. Core Dapper Methods

Dapper's most important methods can be understood by asking:

> **What does my SQL return?**

```text
One value
    → ExecuteScalar

One row
    → QueryFirst / QuerySingle

Zero or one row
    → QueryFirstOrDefault / QuerySingleOrDefault

Many rows
    → Query

Several result sets
    → QueryMultiple

INSERT / UPDATE / DELETE
    → Execute

Low-level row reader
    → ExecuteReader
```


<br>

---

<br>

## 3.1 `Execute`

Used mainly for:

```text
INSERT
UPDATE
DELETE
```

It returns the **number of rows affected**.

Example:

```csharp
string sql = @"
    UPDATE Books
    SET Title = @Title
    WHERE Id = @Id";

int rowsAffected = connection.Execute(
    sql,
    new
    {
        Title = "Updated Title",
        Id = 1
    }
);
```

If one row was updated:

```text
rowsAffected = 1
```

### General Template

```csharp
int result = connection.Execute(
    sql,
    parameters
);
```


<br>

---

<br>

## 3.2 `ExecuteAsync`

Async version:

```csharp
int rowsAffected = await connection.ExecuteAsync(
    sql,
    parameters
);
```

Use this in ASP.NET Core applications when your database operation should be asynchronous.


<br>

---

<br>

## 3.3 `ExecuteScalar`

Used when SQL returns **one value**.

Example:

```sql
SELECT COUNT(*)
FROM Books
```

C#:

```csharp
int count = connection.ExecuteScalar<int>(
    "SELECT COUNT(*) FROM Books"
);
```

Output:

```text
5
```

Another example:

```sql
SELECT MAX(Price)
FROM Books
```

```csharp
decimal maxPrice = connection.ExecuteScalar<decimal>(
    "SELECT MAX(Price) FROM Books"
);
```

### Think

```text
SQL
 ↓
One value
 ↓
ExecuteScalar<T>()
```


<br>

---

<br>

## 3.4 `ExecuteScalarAsync`

Async version:

```csharp
int count = await connection.ExecuteScalarAsync<int>(
    "SELECT COUNT(*) FROM Books"
);
```

Generic template:

```csharp
T result = await connection.ExecuteScalarAsync<T>(
    sql,
    parameters
);
```

[!Note]

`ExecuteScalar` is for the first column of the first row of the result.


<br>

---

<br>

## 3.5 `Query`

Used to retrieve rows and map them to objects.

Example:

```csharp
IEnumerable<Book> books = connection.Query<Book>(
    "SELECT Id, Title, Description FROM Books"
);
```

Dapper maps:

```text
SQL column        C# property
-----------       ------------
Id          →     Id
Title       →     Title
Description →     Description
```

### General Template

```csharp
IEnumerable<T> result = connection.Query<T>(
    sql,
    parameters
);
```


<br>

---

<br>

## 3.6 `QueryAsync`

Async version:

```csharp
IEnumerable<Book> books =
    await connection.QueryAsync<Book>(
        "SELECT Id, Title, Description FROM Books"
    );
```

General template:

```csharp
IEnumerable<T> result =
    await connection.QueryAsync<T>(
        sql,
        parameters
    );
```


<br>

---

<br>

## 3.7 `QueryFirst`

Returns the **first row**.

```csharp
Book book = connection.QueryFirst<Book>(
    "SELECT * FROM Books ORDER BY Id"
);
```

If no row exists, it throws an exception.

If multiple rows exist, it simply takes the first.

```text
0 rows      → exception
1 row       → first row
10 rows     → first row
```


<br>

---

<br>

## 3.8 `QueryFirstOrDefault`

Returns the first row if available.

If no row exists:

```text
null
```

Example:

```csharp
Book book = connection.QueryFirstOrDefault<Book>(
    "SELECT * FROM Books WHERE Id = @Id",
    new { Id = 10 }
);
```

This is commonly useful for:

```text
GET /api/books/10
```

because a missing record can be converted into:

```text
404 Not Found
```

### Async

```csharp
Book book = await connection.QueryFirstOrDefaultAsync<Book>(
    sql,
    new { Id = id }
);
```


<br>

---

<br>

## 3.9 `QuerySingle`

Used when the query is expected to return **exactly one row**.

```csharp
Book book = connection.QuerySingle<Book>(
    "SELECT * FROM Books WHERE Id = @Id",
    new { Id = 1 }
);
```

Behavior:

```text
0 rows       → exception
1 row        → success
2+ rows      → exception
```

Use it when your query logically guarantees one row.


<br>

---

<br>

## 3.10 `QuerySingleOrDefault`

Expected result:

```text
0 or 1 row
```

Example:

```csharp
Book book = connection.QuerySingleOrDefault<Book>(
    "SELECT * FROM Books WHERE Id = @Id",
    new { Id = 1 }
);
```

Behavior:

```text
0 rows       → null
1 row        → success
2+ rows      → exception
```

### First vs Single

| Method | 0 rows | 1 row | Multiple rows |
|---|---|---|---|
| `QueryFirst` | Exception | First | First |
| `QueryFirstOrDefault` | `null` | First | First |
| `QuerySingle` | Exception | Row | Exception |
| `QuerySingleOrDefault` | `null` | Row | Exception |

This distinction is important.


<br>

---

<br>

## 3.11 `QueryMultiple`

Used when one SQL command returns **multiple result sets**.

Example SQL:

```sql
SELECT * FROM Books;

SELECT * FROM Authors;
```

C#:

```csharp
using var multi = connection.QueryMultiple(sql);

var books = multi.Read<Book>().ToList();
var authors = multi.Read<Author>().ToList();
```

Flow:

```text
One SQL command
      ↓
Result Set 1 → Read<Book>()
      ↓
Result Set 2 → Read<Author>()
```

### Async

```csharp
using var multi =
    await connection.QueryMultipleAsync(sql);

var books = multi.Read<Book>().ToList();
var authors = multi.Read<Author>().ToList();
```

[!Important]

The order of `Read<T>()` calls must match the order of result sets returned by SQL.


<br>

---

<br>

## 3.12 `ExecuteReader`

`ExecuteReader` provides a lower-level `DbDataReader`/`IDataReader` style result.

Example:

```csharp
using var reader = connection.ExecuteReader(
    "SELECT Id, Title FROM Books"
);

while (reader.Read())
{
    int id = reader.GetInt32(0);
    string title = reader.GetString(1);

    Console.WriteLine($"{id} - {title}");
}
```

This gives you direct access to the reader.

### Difference from `Query`

```text
Query<T>()
   ↓
Dapper maps rows to T

ExecuteReader()
   ↓
You work directly with the reader
```

Use `ExecuteReader` when you need lower-level control over row-by-row reading.


<br>

---

<br>

# 4. Querying Data

## 4.1 Query a Single Row

Example model:

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
```

SQL:

```sql
SELECT Id, Title, Description
FROM Books
WHERE Id = @Id
```

C#:

```csharp
var book = await connection.QueryFirstOrDefaultAsync<Book>(
    sql,
    new { Id = id }
);
```

Result:

```text
Database row
     ↓
Dapper
     ↓
Book object
```


<br>

---

<br>

## 4.2 Query Multiple Rows

```csharp
var books = await connection.QueryAsync<Book>(
    "SELECT Id, Title, Description FROM Books"
);
```

Convert to list when required:

```csharp
var books = (await connection.QueryAsync<Book>(sql))
    .ToList();
```

Output:

```text
List<Book>
```


<br>

---

<br>

## 4.3 Query Specific Columns

You do not need:

```sql
SELECT *
```

You can request only what you need:

```sql
SELECT Id, Title
FROM Books
```

Then:

```csharp
var books = await connection.QueryAsync<BookSummary>(
    sql
);
```

Model:

```csharp
public class BookSummary
{
    public int Id { get; set; }
    public string Title { get; set; }
}
```

### Why?

```text
SELECT *
```

may retrieve unnecessary columns.

Prefer selecting the columns required by the operation.


<br>

---

<br>

## 4.4 Mapping Rules

Dapper automatically maps returned columns to properties based primarily on names.

Example:

```sql
SELECT
    Id,
    Title,
    Description
FROM Books
```

maps to:

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
```

Aliases can be used when names differ:

```sql
SELECT
    BookName AS Title
FROM Books
```

Now:

```text
BookName → Title
```

This is useful when the SQL schema and API model use different names.


<br>

---

<br>

# 5. INSERT, UPDATE & DELETE

## 5.1 INSERT

SQL:

```sql
INSERT INTO Books (Title, Description)
VALUES (@Title, @Description)
```

C#:

```csharp
var sql = @"
    INSERT INTO Books (Title, Description)
    VALUES (@Title, @Description)";

int rowsAffected = await connection.ExecuteAsync(
    sql,
    new
    {
        Title = book.Title,
        Description = book.Description
    }
);
```

Output:

```text
rowsAffected = 1
```


<br>

---

<br>

## 5.2 Get Generated ID

If SQL Server generates an identity ID, use:

```sql
INSERT INTO Books (Title, Description)
VALUES (@Title, @Description);

SELECT CAST(SCOPE_IDENTITY() AS INT);
```

Dapper:

```csharp
int newBookId = await connection.ExecuteScalarAsync<int>(
    sql,
    new
    {
        Title = book.Title,
        Description = book.Description
    }
);
```

Flow:

```text
INSERT
  ↓
SQL Server generates ID
  ↓
SCOPE_IDENTITY()
  ↓
ExecuteScalarAsync<int>()
  ↓
newBookId
```


<br>

---

<br>

## 5.3 UPDATE

SQL:

```sql
UPDATE Books
SET Title = @Title,
    Description = @Description
WHERE Id = @Id
```

C#:

```csharp
int rowsAffected = await connection.ExecuteAsync(
    sql,
    new
    {
        Id = book.Id,
        Title = book.Title,
        Description = book.Description
    }
);
```

Check:

```csharp
if (rowsAffected == 0)
{
    // Book was not found
}
```


<br>

---

<br>

## 5.4 DELETE

SQL:

```sql
DELETE FROM Books
WHERE Id = @Id
```

C#:

```csharp
int rowsAffected = await connection.ExecuteAsync(
    sql,
    new { Id = id }
);
```

Then:

```csharp
if (rowsAffected == 0)
{
    // Nothing was deleted
}
```


<br>

---

<br>

# 6. Parameters in Dapper

Parameters are one of the most important Dapper concepts.

Instead of:

```csharp
string sql =
    "SELECT * FROM Books WHERE Title = '" + title + "'";
```

use:

```csharp
string sql =
    "SELECT * FROM Books WHERE Title = @Title";

var book = await connection.QueryFirstOrDefaultAsync<Book>(
    sql,
    new { Title = title }
);
```

Dapper creates parameterized database commands.


<br>

---

<br>

## 6.1 Anonymous Object

Most common approach:

```csharp
new
{
    Id = id,
    Title = title
}
```

SQL:

```sql
WHERE Id = @Id
AND Title = @Title
```

Dapper matches:

```text
@Id     → Id
@Title  → Title
```

### Template

```csharp
await connection.QueryAsync<T>(
    sql,
    new
    {
        Parameter1 = value1,
        Parameter2 = value2
    }
);
```


<br>

---

<br>

## 6.2 POCO Object

You can pass an existing object:

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
```

SQL:

```sql
UPDATE Books
SET Title = @Title,
    Description = @Description
WHERE Id = @Id
```

Then:

```csharp
await connection.ExecuteAsync(sql, book);
```

Dapper uses matching property names.


<br>

---

<br>

## 6.3 `DynamicParameters`

Useful when you need:

- Explicit parameter configuration.
- Input parameters.
- Output parameters.
- Return values.
- Parameter types/sizes.
- Stored procedure parameters.

Example:

```csharp
var parameters = new DynamicParameters();

parameters.Add("@Id", id);
parameters.Add("@Title", title);

await connection.ExecuteAsync(
    sql,
    parameters
);
```

Output parameter example:

```csharp
parameters.Add(
    "@Result",
    dbType: DbType.Int32,
    direction: ParameterDirection.Output
);
```

Then after execution:

```csharp
int result = parameters.Get<int>("@Result");
```


<br>

---

<br>

## 6.4 Multiple Parameters

SQL:

```sql
SELECT *
FROM Books
WHERE Title = @Title
AND AuthorId = @AuthorId
```

C#:

```csharp
var books = await connection.QueryAsync<Book>(
    sql,
    new
    {
        Title = title,
        AuthorId = authorId
    }
);
```


<br>

---

<br>

## 6.5 List Parameters / `IN`

Dapper can expand enumerable parameters.

SQL:

```sql
SELECT *
FROM Books
WHERE Id IN @Ids
```

C#:

```csharp
var ids = new[] { 1, 2, 3, 4 };

var books = await connection.QueryAsync<Book>(
    sql,
    new { Ids = ids }
);
```

Dapper expands the parameter list appropriately for the query.

[!Important]

Do not construct an `IN` clause by concatenating untrusted strings.


<br>

---

<br>

# 7. SQL Injection

SQL injection occurs when untrusted input is directly concatenated into SQL.

### Dangerous

```csharp
string sql =
    "SELECT * FROM Books WHERE Title = '" + title + "'";
```

If `title` contains malicious SQL, the generated SQL can be altered.

### Safe Parameterized Query

```csharp
string sql =
    "SELECT * FROM Books WHERE Title = @Title";

var books = await connection.QueryAsync<Book>(
    sql,
    new { Title = title }
);
```

Dapper sends the value as a parameter instead of treating it as SQL syntax.

[!Important]

**Do not confuse parameterization with string interpolation.**

Avoid:

```csharp
$"SELECT * FROM Books WHERE Id = {id}"
```

Prefer:

```csharp
"SELECT * FROM Books WHERE Id = @Id"
```

with:

```csharp
new { Id = id }
```


<br>

---

<br>

# 8. Buffered vs Unbuffered Queries

Dapper's query methods are buffered by default.

### Buffered

```csharp
var books = connection.Query<Book>(
    sql,
    buffered: true
);
```

Conceptually:

```text
Database
   ↓
All result rows read
   ↓
Memory
   ↓
Return results
```

Advantages:

- Connection/reader can be released sooner.
- Easy to work with after the query completes.

Disadvantage:

- Large result sets consume more memory.

### Unbuffered

```csharp
var books = connection.Query<Book>(
    sql,
    buffered: false
);
```

Conceptually:

```text
Database
   ↓
Reader remains active
   ↓
Rows consumed as enumerated
```

Useful for very large result sets where you want streaming behavior.

[!Important]

With unbuffered queries, the connection and reader remain important while enumeration is in progress. Do not dispose the connection before finishing enumeration.


<br>

---

<br>

# 9. `ExecuteReader`

`ExecuteReader` is lower-level than Dapper's object mapping methods.

Example:

```csharp
using var reader = connection.ExecuteReader(
    "SELECT Id, Title FROM Books"
);

while (reader.Read())
{
    var id = reader.GetInt32(0);
    var title = reader.GetString(1);

    Console.WriteLine($"{id} - {title}");
}
```

### Query vs ExecuteReader

```text
Query<T>()
    ↓
Dapper handles:
    SQL execution
    +
row reading
    +
object mapping

ExecuteReader()
    ↓
You handle:
    reader
    +
columns
    +
row processing
```

Use `Query<T>` for normal application-level mapping.

Use `ExecuteReader` when direct reader access is required.


<br>

---

<br>

# 10. Multiple Result Sets — `QueryMultiple`

Suppose we need books and authors in one database round trip.

SQL:

```sql
SELECT Id, Title, Description
FROM Books;

SELECT Id, Name
FROM Authors;
```

C#:

```csharp
using var multi = await connection.QueryMultipleAsync(sql);

var books = (await multi.ReadAsync<Book>()).ToList();
var authors = (await multi.ReadAsync<Author>()).ToList();
```

Depending on the Dapper API/version, synchronous `Read<T>()` can also be used on the returned grid reader.

### Flow

```text
Database
   │
   ├── Result Set 1
   │       ↓
   │    Read<Book>()
   │
   └── Result Set 2
           ↓
        Read<Author>()
```

### Why use it?

Instead of:

```text
Query Books
   ↓
Database round trip

Query Authors
   ↓
Database round trip
```

you can send both statements together:

```text
One command
   ↓
Multiple result sets
   ↓
Read each result set
```


<br>

---

<br>

# 11. Stored Procedures

Dapper can execute stored procedures by setting:

```csharp
commandType: CommandType.StoredProcedure
```

## 11.1 Execute a Stored Procedure

SQL Server:

```sql
CREATE PROCEDURE GetBookById
    @Id INT
AS
BEGIN
    SELECT Id, Title, Description
    FROM Books
    WHERE Id = @Id
END
```

C#:

```csharp
var book = await connection.QueryFirstOrDefaultAsync<Book>(
    "GetBookById",
    new { Id = id },
    commandType: CommandType.StoredProcedure
);
```

### Template

```csharp
await connection.QueryAsync<T>(
    "ProcedureName",
    parameters,
    commandType: CommandType.StoredProcedure
);
```


<br>

---

<br>

## 11.1 Execute a Stored Procedure

For a procedure that performs INSERT/UPDATE/DELETE:

```csharp
int rows = await connection.ExecuteAsync(
    "UpdateBook",
    parameters,
    commandType: CommandType.StoredProcedure
);
```


<br>

---

<br>

## 11.2 Stored Procedure Returning Rows

Procedure:

```sql
CREATE PROCEDURE GetBooksByAuthor
    @AuthorId INT
AS
BEGIN
    SELECT Id, Title, Description
    FROM Books
    WHERE AuthorId = @AuthorId
END
```

C#:

```csharp
var books = await connection.QueryAsync<Book>(
    "GetBooksByAuthor",
    new { AuthorId = authorId },
    commandType: CommandType.StoredProcedure
);
```


<br>

---

<br>

## 11.3 Output Parameters

SQL:

```sql
CREATE PROCEDURE GetBookCount
    @AuthorId INT,
    @BookCount INT OUTPUT
AS
BEGIN
    SELECT @BookCount = COUNT(*)
    FROM Books
    WHERE AuthorId = @AuthorId
END
```

C#:

```csharp
var parameters = new DynamicParameters();

parameters.Add("@AuthorId", authorId);

parameters.Add(
    "@BookCount",
    dbType: DbType.Int32,
    direction: ParameterDirection.Output
);

await connection.ExecuteAsync(
    "GetBookCount",
    parameters,
    commandType: CommandType.StoredProcedure
);

int count = parameters.Get<int>("@BookCount");
```

Flow:

```text
C#
 ↓
Input parameters
 ↓
Stored Procedure
 ↓
Output parameter
 ↓
DynamicParameters
 ↓
C#
```

[!Important]

Read the output parameter **after** the command has executed.


<br>

---

<br>

# 12. Table-Valued Parameters

A **Table-Valued Parameter (TVP)** allows a collection of rows to be sent to SQL Server as a parameter.

Useful when you need to send:

```text
1
2
3
4
5
```

or a table-shaped collection in one database call.

### SQL Server User-Defined Table Type

```sql
CREATE TYPE BookIdTable AS TABLE
(
    Id INT
);
```

Stored procedure:

```sql
CREATE PROCEDURE GetBooksByIds
    @BookIds BookIdTable READONLY
AS
BEGIN
    SELECT b.Id, b.Title, b.Description
    FROM Books b
    INNER JOIN @BookIds ids
        ON b.Id = ids.Id;
END
```

C#:

```csharp
var table = new DataTable();

table.Columns.Add("Id", typeof(int));

table.Rows.Add(1);
table.Rows.Add(2);
table.Rows.Add(3);

var parameters = new DynamicParameters();

parameters.Add(
    "@BookIds",
    table.AsTableValuedParameter("BookIdTable")
);

var books = await connection.QueryAsync<Book>(
    "GetBooksByIds",
    parameters,
    commandType: CommandType.StoredProcedure
);
```

### Flow

```text
C# Collection
     ↓
DataTable
     ↓
TVP
     ↓
Stored Procedure
     ↓
SQL Server
```

[!Note]

TVPs are SQL Server-specific functionality; the exact API depends on the database provider and Dapper/provider support.


<br>

---

<br>

# 13. SQL Functions

Dapper can execute SQL that calls database functions.

## 13.1 Scalar SQL Function

Suppose SQL Server has:

```sql
CREATE FUNCTION GetBookTitle
(
    @BookId INT
)
RETURNS NVARCHAR(200)
AS
BEGIN
    DECLARE @Title NVARCHAR(200);

    SELECT @Title = Title
    FROM Books
    WHERE Id = @BookId;

    RETURN @Title;
END
```

Call it:

```csharp
var title = await connection.ExecuteScalarAsync<string>(
    "SELECT dbo.GetBookTitle(@BookId)",
    new { BookId = id }
);
```

Flow:

```text
C#
 ↓
Dapper
 ↓
SELECT dbo.Function(...)
 ↓
SQL Function
 ↓
Scalar value
```


<br>

---

<br>

## 13.2 Table-Valued Function

A table-valued function returns rows.

Example usage:

```sql
SELECT *
FROM dbo.GetBooksByAuthor(@AuthorId)
```

Dapper:

```csharp
var books = await connection.QueryAsync<Book>(
    sql,
    new { AuthorId = authorId }
);
```

So:

```text
Scalar function
    → ExecuteScalar

Table-valued function
    → Query<T>
```


<br>

---

<br>

# 14. Transactions

## 14.1 Why Transactions

Suppose a money-transfer operation performs:

```text
Debit Account A
+
Credit Account B
```

If debit succeeds but credit fails, the database can become inconsistent.

A transaction gives us:

```text
All operations succeed
      ↓
COMMIT

Any critical operation fails
      ↓
ROLLBACK
```

### Mental Model

```text
BEGIN TRANSACTION
       ↓
 Operation 1
       ↓
 Operation 2
       ↓
 Operation 3
       ↓
   Everything OK?
     ↙       ↘
   YES        NO
    ↓          ↓
 COMMIT     ROLLBACK
```


<br>

---

<br>

## 14.2 Transaction Template

```csharp
using var connection = new SqlConnection(connectionString);

await connection.OpenAsync();

using var transaction = connection.BeginTransaction();

try
{
    await connection.ExecuteAsync(
        sql1,
        parameters1,
        transaction: transaction
    );

    await connection.ExecuteAsync(
        sql2,
        parameters2,
        transaction: transaction
    );

    transaction.Commit();
}
catch
{
    transaction.Rollback();
    throw;
}
```

[!Important]

Every database operation that should participate in the transaction must receive the same transaction object:

```csharp
transaction: transaction
```

Otherwise that command may execute outside the intended transaction.


<br>

---

<br>

# 15. Common Dapper Templates

These are the patterns worth memorizing.

## 15.1 GET All

```csharp
public async Task<IEnumerable<Book>> GetAllAsync()
{
    using var connection = CreateConnection();

    const string sql = @"
        SELECT Id, Title, Description
        FROM Books";

    return await connection.QueryAsync<Book>(sql);
}
```


<br>

---

<br>

## 15.2 GET By ID

```csharp
public async Task<Book> GetByIdAsync(int id)
{
    using var connection = CreateConnection();

    const string sql = @"
        SELECT Id, Title, Description
        FROM Books
        WHERE Id = @Id";

    return await connection.QueryFirstOrDefaultAsync<Book>(
        sql,
        new { Id = id }
    );
}
```

Controller:

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetBook(int id)
{
    var book = await _repository.GetByIdAsync(id);

    if (book == null)
        return NotFound();

    return Ok(book);
}
```


<br>

---

<br>

## 15.3 POST

```csharp
public async Task<int> AddAsync(Book book)
{
    using var connection = CreateConnection();

    const string sql = @"
        INSERT INTO Books (Title, Description)
        VALUES (@Title, @Description);

        SELECT CAST(SCOPE_IDENTITY() AS INT);";

    return await connection.ExecuteScalarAsync<int>(
        sql,
        book
    );
}
```

Controller:

```csharp
[HttpPost]
public async Task<IActionResult> Create(Book book)
{
    int id = await _repository.AddAsync(book);

    return CreatedAtAction(
        nameof(GetBook),
        new { id },
        id
    );
}
```


<br>

---

<br>

## 15.4 PUT

```csharp
public async Task<bool> UpdateAsync(Book book)
{
    using var connection = CreateConnection();

    const string sql = @"
        UPDATE Books
        SET Title = @Title,
            Description = @Description
        WHERE Id = @Id";

    int rows = await connection.ExecuteAsync(sql, book);

    return rows > 0;
}
```

Controller:

```csharp
[HttpPut("{id}")]
public async Task<IActionResult> Update(
    int id,
    Book book)
{
    book.Id = id;

    bool updated = await _repository.UpdateAsync(book);

    if (!updated)
        return NotFound();

    return Ok();
}
```


<br>

---

<br>

## 15.5 DELETE

```csharp
public async Task<bool> DeleteAsync(int id)
{
    using var connection = CreateConnection();

    const string sql = @"
        DELETE FROM Books
        WHERE Id = @Id";

    int rows = await connection.ExecuteAsync(
        sql,
        new { Id = id }
    );

    return rows > 0;
}
```

Controller:

```csharp
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    bool deleted = await _repository.DeleteAsync(id);

    if (!deleted)
        return NotFound();

    return NoContent();
}
```


<br>

---

<br>

# 16. Dapper in ASP.NET Core Web API

## 16.1 Recommended Structure

For the same BookStore application:

```text
BookStore.API
│
├── Controllers
│   └── BooksController.cs
│
├── Models
│   ├── Book.cs
│   └── BookModel.cs
│
├── Repository
│   ├── IBookRepository.cs
│   └── BookRepository.cs
│
├── SQL
│   └── ...
│
├── appsettings.json
├── Startup.cs
└── Program.cs
```

Dapper replaces the EF Core database-access portion:

```text
EF Core
  ↓
DbContext
  ↓
DbSet
```

with:

```text
Dapper
  ↓
IDbConnection / SqlConnection
  ↓
SQL
```


<br>

---

<br>

## 16.2 Repository Interface

```csharp
public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();

    Task<Book> GetByIdAsync(int id);

    Task<int> AddAsync(Book book);

    Task<bool> UpdateAsync(Book book);

    Task<bool> DeleteAsync(int id);
}
```

The controller depends on the interface rather than the concrete repository.


<br>

---

<br>

## 16.3 Repository Implementation

```csharp
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

public class BookRepository : IBookRepository
{
    private readonly IConfiguration _configuration;

    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private IDbConnection CreateConnection()
    {
        return new SqlConnection(
            _configuration.GetConnectionString("BookStoreDB")
        );
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        using var connection = CreateConnection();

        const string sql = @"
            SELECT Id, Title, Description
            FROM Books";

        return await connection.QueryAsync<Book>(sql);
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();

        const string sql = @"
            SELECT Id, Title, Description
            FROM Books
            WHERE Id = @Id";

        return await connection.QueryFirstOrDefaultAsync<Book>(
            sql,
            new { Id = id }
        );
    }

    public async Task<int> AddAsync(Book book)
    {
        using var connection = CreateConnection();

        const string sql = @"
            INSERT INTO Books (Title, Description)
            VALUES (@Title, @Description);

            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            book
        );
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        using var connection = CreateConnection();

        const string sql = @"
            UPDATE Books
            SET Title = @Title,
                Description = @Description
            WHERE Id = @Id";

        int rows = await connection.ExecuteAsync(sql, book);

        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = CreateConnection();

        const string sql = @"
            DELETE FROM Books
            WHERE Id = @Id";

        int rows = await connection.ExecuteAsync(
            sql,
            new { Id = id }
        );

        return rows > 0;
    }
}
```


<br>

---

<br>

## 16.4 Controller

```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _repository;

    public BooksController(IBookRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _repository.GetAllAsync();

        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _repository.GetByIdAsync(id);

        if (book == null)
            return NotFound();

        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Book book)
    {
        int id = await _repository.AddAsync(book);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            id
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        Book book)
    {
        book.Id = id;

        bool updated = await _repository.UpdateAsync(book);

        if (!updated)
            return NotFound();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _repository.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
```


<br>

---

<br>

## 16.5 DI Registration

For ASP.NET Core 5:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    services.AddTransient<IBookRepository, BookRepository>();
}
```

Then:

```text
Controller
    ↓
IBookRepository
    ↓
BookRepository
    ↓
Dapper
    ↓
SqlConnection
    ↓
SQL Server
```


<br>

---

<br>

# 17. Method Selection Cheat Sheet

| Requirement | Dapper Method |
|---|---|
| INSERT | `ExecuteAsync` |
| UPDATE | `ExecuteAsync` |
| DELETE | `ExecuteAsync` |
| Number of affected rows | `ExecuteAsync` |
| One scalar value | `ExecuteScalarAsync<T>` |
| Get many rows | `QueryAsync<T>` |
| Get first row | `QueryFirstAsync<T>` |
| Get first or null | `QueryFirstOrDefaultAsync<T>` |
| Exactly one row | `QuerySingleAsync<T>` |
| Exactly one or null | `QuerySingleOrDefaultAsync<T>` |
| Multiple result sets | `QueryMultipleAsync` |
| Direct data reader | `ExecuteReader` |
| Stored procedure | `commandType: CommandType.StoredProcedure` |
| Output parameter | `DynamicParameters` |
| TVP | `DynamicParameters` + `AsTableValuedParameter()` |
| SQL scalar function | `ExecuteScalarAsync<T>` |
| SQL table-valued function | `QueryAsync<T>` |
| Transaction | `BeginTransaction()` + `transaction:` |


<br>

---

<br>

# 18. Important Dapper Concepts

### 1. Dapper does not replace SQL knowledge

You still need to understand:

```text
SELECT
INSERT
UPDATE
DELETE
JOIN
WHERE
GROUP BY
ORDER BY
Stored Procedures
Transactions
Functions
```

Dapper makes executing SQL from C# easier; it does not eliminate SQL.

---

### 2. `Execute` vs `Query`

Remember:

```text
Execute
   ↓
Command affects rows
   ↓
INSERT / UPDATE / DELETE
```

while:

```text
Query
   ↓
Command returns rows
   ↓
C# objects
```

---

### 3. `ExecuteScalar`

```text
One value
```

Example:

```sql
SELECT COUNT(*)
```

---

### 4. `QueryFirst` vs `QuerySingle`

```text
QueryFirst
    → I only care about the first matching row.

QuerySingle
    → There must be exactly one matching row.
```

---

### 5. Parameterization

Always prefer:

```sql
WHERE Id = @Id
```

with:

```csharp
new { Id = id }
```

over SQL string concatenation/interpolation.

---

### 6. Dapper Mapping

```text
SQL Column
    ↓
Matching C# Property
    ↓
C# Object
```

If names differ, use SQL aliases or explicit mapping techniques.

---

### 7. Multiple Result Sets

```text
QueryMultiple
    ↓
Read<T>()
Read<U>()
Read<V>()
```

The `Read` order must correspond to the SQL result-set order.

---

### 8. Affected Rows

`ExecuteAsync()` returns an integer:

```csharp
int rowsAffected
```

Useful for determining whether an UPDATE/DELETE actually matched a row.

```csharp
if (rowsAffected == 0)
{
    return NotFound();
}
```

---

### 9. Transactions

If several database operations must succeed or fail together:

```text
BeginTransaction
       ↓
Execute 1
       ↓
Execute 2
       ↓
Commit / Rollback
```

---

### 10. Connection Lifetime

Typical pattern:

```csharp
using var connection = CreateConnection();
```

Open/use the connection for the operation and dispose it afterward.

For async database calls:

```csharp
await connection.QueryAsync<T>(...);
```


<br>

---

<br>

# 19. Final Mental Model

## Dapper in One Picture

```text
                    ASP.NET CORE API
                           │
                           ↓
                      Controller
                           │
                           ↓
                       Repository
                           │
                           ↓
                  IDbConnection
                 / SqlConnection
                           │
                           ↓
                        DAPPER
                           │
             ┌─────────────┼─────────────┐
             ↓             ↓             ↓
          Query         Execute       Scalar
             │             │             │
             ↓             ↓             ↓
        SELECT          INSERT        COUNT
        SELECT          UPDATE        MAX
        SELECT          DELETE        ID
             │             │             │
             └─────────────┼─────────────┘
                           ↓
                       SQL SERVER
                           │
                           ↓
                  Result / Row Count
                           │
                           ↓
                     Dapper Mapping
                           │
                           ↓
                       C# Object
                           │
                           ↓
                     API Response
```

## The Core Decision Tree

```text
What does SQL return?
        │
        ├── No rows / command operation
        │       ↓
        │    Execute
        │
        ├── One scalar value
        │       ↓
        │    ExecuteScalar
        │
        ├── One row
        │       ↓
        │    QueryFirst / QuerySingle
        │
        ├── Many rows
        │       ↓
        │    Query
        │
        ├── Multiple result sets
        │       ↓
        │    QueryMultiple
        │
        └── Need low-level reader
                ↓
            ExecuteReader
```

## Dapper CRUD Memory Template

```text
GET ALL
    → QueryAsync<T>()

GET BY ID
    → QueryFirstOrDefaultAsync<T>()

POST
    → ExecuteScalarAsync<int>()
      if generated ID is required

PUT
    → ExecuteAsync()
      check rows affected

DELETE
    → ExecuteAsync()
      check rows affected

STORED PROCEDURE
    → commandType: StoredProcedure

OUTPUT PARAMETER
    → DynamicParameters

MULTIPLE RESULT SETS
    → QueryMultiple

TRANSACTION
    → BeginTransaction()
      ↓
      Execute(..., transaction)
      ↓
      Commit / Rollback
```


<br>

---

<br>

> [!Important]
> The supplied course index covers **Dapper Parts 1–16**, including introduction, scalar queries, single/multiple-row queries, multiple result sets, specific columns, CRUD, buffered/unbuffered queries, `ExecuteReader`, stored procedures, TVPs, output parameters, SQL injection, parameter passing, SQL functions, and transactions. The notes above organize those course topics into a reusable ASP.NET Core/Dapper reference and add general templates around them. fileciteturn11file0
