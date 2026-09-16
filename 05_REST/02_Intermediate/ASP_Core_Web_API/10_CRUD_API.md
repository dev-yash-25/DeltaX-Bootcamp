# HTTP GET — Retrieve Data from Database

## Index

- [1. What We Are Building](#1-what-we-are-building)
- [2. Basic Application Structure](#2-basic-application-structure)
- [3. Complete Request Flow](#3-complete-request-flow)
- [4. GET All Books](#4-get-all-books)
  - [4.1 Create the API Model](#41-create-the-api-model)
  - [4.2 Repository Interface](#42-repository-interface)
  - [4.3 Inject `BooksDbContext` into Repository](#43-inject-booksdbcontext-into-repository)
  - [4.4 Implement `GetAllBooksAsync()`](#44-implement-getallbooksasync)
  - [4.5 Map Entity to API Model](#45-map-entity-to-api-model)
  - [4.6 Controller](#46-controller)
  - [4.7 Test the API](#47-test-the-api)
  - [4.8 Expected Output](#48-expected-output)
- [5. GET One Book by ID](#5-get-one-book-by-id)
  - [5.1 Repository Interface](#51-repository-interface)
  - [5.2 `FindAsync()`](#52-findasync)
  - [5.3 `FirstAsync()` vs `FirstOrDefaultAsync()`](#53-firstasync-vs-firstordefaultasync)
  - [5.4 Repository Implementation](#54-repository-implementation)
  - [5.5 Controller Route](#55-controller-route)
  - [5.6 Handle `404 Not Found`](#56-handle-404-not-found)
  - [5.7 Test the API](#57-test-the-api)
  - [5.8 Expected Output](#58-expected-output)
- [6. Complete Application Structure](#6-complete-application-structure)
- [7. Complete Code](#7-complete-code)
- [8. Debugging / Breakpoints](#8-debugging--breakpoints)
- [9. Important Points](#9-important-points)
- [10. Final Mental Model](#10-final-mental-model)

<br>

---

<br>

## 1. What We Are Building

We already have a **BookStore.API** application with:

- `Book` entity
- `BooksDbContext`
- `BookRepository`
- `IBookRepository`
- SQL Server database
- Dependency Injection
- EF Core

Now we will create two **HTTP GET** APIs:

### API 1 — Get All Books

```http
GET /api/books
```

Returns all books from the database.

### API 2 — Get One Book

```http
GET /api/books/{id}
```

Returns one book based on its ID.

<br>

---

<br>

## 2. Basic Application Structure

The application is organized using the repository pattern.

```text
BookStore.API
│
├── Controllers
│   └── BooksController.cs
│
├── Data
│   └── BooksDbContext.cs
│
├── Models
│   └── BookModel.cs
│
├── Repository
│   ├── IBookRepository.cs
│   └── BookRepository.cs
│
├── Migrations
│   └── ...
│
├── appsettings.json
├── Startup.cs
└── Program.cs
```

### Responsibility of Each Layer

```text
BooksController
      ↓
IBookRepository
      ↓
BookRepository
      ↓
BooksDbContext
      ↓
EF Core
      ↓
SQL Server
```

| Component | Responsibility |
|---|---|
| `BooksController` | Receives HTTP requests and returns HTTP responses |
| `IBookRepository` | Defines repository operations |
| `BookRepository` | Contains database-access logic |
| `BooksDbContext` | Provides EF Core database context |
| `Book` | Database entity |
| `BookModel` | API model returned to client |
| SQL Server | Stores persistent data |

> [!Important]
> The controller should not directly contain database-querying logic. The repository handles database access.

<br>

---

<br>

## 3. Complete Request Flow

When the client calls:

```http
GET /api/books
```

the request flows through the application like this:

```text
Client / Postman
       ↓
BooksController
       ↓
IBookRepository
       ↓
BookRepository
       ↓
BooksDbContext
       ↓
EF Core
       ↓
SQL Server
       ↓
Book entities
       ↓
BookModel mapping
       ↓
BooksController
       ↓
Ok(...)
       ↓
JSON Response
```

For a single book:

```text
GET /api/books/1
       ↓
BooksController
       ↓
GetBookById(1)
       ↓
BookRepository
       ↓
FindAsync(1)
       ↓
SQL Server
       ↓
Book / null
       ↓
Controller
       ↓
200 OK / 404 Not Found
```

<br>

---

<br>

# 4. GET All Books

The first API retrieves **all records** from the `Books` table.

```http
GET https://localhost:XXXX/api/books
```

<br>

## 4.1 Create the API Model

The database entity already exists.

Example:

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
```

Now create a `Models` folder and create:

```text
BookModel.cs
```

Example:

```csharp
public class BookModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
```

### Why Create a Separate Model?

The database entity and API model are separated:

```text
Database Entity
      ↓
    Book
      ↓
  Mapping
      ↓
  BookModel
      ↓
 API Response
```

> [!Important]
> `Book` represents the database entity, while `BookModel` represents the model returned by the API.

<br>

## 4.2 Repository Interface

Open:

```text
Repository/IBookRepository.cs
```

Add:

```csharp
Task<IEnumerable<BookModel>> GetAllBooksAsync();
```

Example:

```csharp
public interface IBookRepository
{
    Task<IEnumerable<BookModel>> GetAllBooksAsync();
}
```

The interface exposes the operation without exposing how the database query is implemented.

<br>

## 4.3 Inject `BooksDbContext` into Repository

Open:

```text
Repository/BookRepository.cs
```

The repository needs database access, so inject `BooksDbContext` through its constructor.

```csharp
private readonly BooksDbContext _context;

public BookRepository(BooksDbContext context)
{
    _context = context;
}
```

Now the repository can access:

```csharp
_context.Books
```

which represents the `Books` entity set/table.

### Dependency Flow

```text
BookRepository
      ↓
BooksDbContext
      ↓
Books
      ↓
SQL Server
```

<br>

## 4.4 Implement `GetAllBooksAsync()`

Create the asynchronous repository method:

```csharp
public async Task<IEnumerable<BookModel>> GetAllBooksAsync()
{
    var books = await _context.Books.ToListAsync();

    // Mapping will happen here

    return ...;
}
```

### `ToListAsync()`

```csharp
_context.Books.ToListAsync()
```

retrieves the records asynchronously.

```text
_context.Books
      ↓
Books table
      ↓
ToListAsync()
      ↓
List<Book>
```

The database returns `Book` entities. Our API needs `BookModel`, so mapping is required.

<br>

## 4.5 Map Entity to API Model

The database returns:

```text
Book
```

but the API needs:

```text
BookModel
```

### Manual Mapping with `foreach`

```csharp
var books = await _context.Books.ToListAsync();

var bookModels = new List<BookModel>();

foreach (var book in books)
{
    var bookModel = new BookModel
    {
        Id = book.Id,
        Title = book.Title,
        Description = book.Description
    };

    bookModels.Add(bookModel);
}

return bookModels;
```

### Mapping Flow

```text
SQL Server
    ↓
List<Book>
    ↓
foreach
    ↓
BookModel
    ↓
List<BookModel>
```

### Limitation of Manual Mapping

For a model with 50+ properties, manually mapping every property becomes tedious.

The tutorial mentions automated mapping tools such as **AutoMapper** for this problem, to be covered later.

> [!Note]
> Manual mapping is simple and explicit, but becomes repetitive for large models.

<br>

## 4.6 Controller

Open:

```text
Controllers/BooksController.cs
```

Inject the repository:

```csharp
private readonly IBookRepository _bookRepository;

public BooksController(IBookRepository bookRepository)
{
    _bookRepository = bookRepository;
}
```

Create the GET action:

```csharp
[HttpGet]
public async Task<IActionResult> GetAllBooks()
{
    var books = await _bookRepository.GetAllBooksAsync();

    return Ok(books);
}
```

### What Happens?

```text
GET /api/books
      ↓
GetAllBooks()
      ↓
_bookRepository.GetAllBooksAsync()
      ↓
BookRepository
      ↓
Database
      ↓
BookModel collection
      ↓
Ok(books)
      ↓
HTTP 200
```

<br>

## 4.7 Test the API

Run the application.

Suppose the application starts at:

```text
https://localhost:5001
```

Endpoint:

```http
GET https://localhost:5001/api/books
```

The exact port depends on the application's launch settings.

### Test Using Browser

Enter:

```text
https://localhost:XXXX/api/books
```

### Test Using Postman

```text
Method: GET
URL: https://localhost:XXXX/api/books
```

Then click:

```text
Send
```

### Debugging

Set breakpoints in:

```csharp
GetAllBooks()
```

and:

```csharp
GetAllBooksAsync()
```

Observe:

```text
Controller breakpoint
       ↓
Repository breakpoint
       ↓
Database query
       ↓
Returned data
```

> [!Tip]
> Breakpoints are useful for understanding how an HTTP request travels through controller → repository → database.

<br>

## 4.8 Expected Output

Suppose the database contains:

| ID | Title | Description |
|---:|---|---|
| 1 | C# Basics | Learn C# |
| 2 | ASP.NET Core | Learn Web API |
| 3 | SQL | Learn databases |

The API response will be similar to:

```json
[
  {
    "id": 1,
    "title": "C# Basics",
    "description": "Learn C#"
  },
  {
    "id": 2,
    "title": "ASP.NET Core",
    "description": "Learn Web API"
  },
  {
    "id": 3,
    "title": "SQL",
    "description": "Learn databases"
  }
]
```

HTTP status:

```text
200 OK
```

<br>

---

<br>

# 5. GET One Book by ID

Endpoint:

```http
GET /api/books/{id}
```

Example:

```http
GET /api/books/2
```

This returns the book whose ID is `2`.

<br>

## 5.1 Repository Interface

Update `IBookRepository.cs`:

```csharp
public interface IBookRepository
{
    Task<IEnumerable<BookModel>> GetAllBooksAsync();

    Task<BookModel> GetBookById(int id);
}
```

<br>

## 5.2 `FindAsync()`

For a lookup specifically by the **primary key**, the tutorial recommends:

```csharp
FindAsync(id)
```

Example:

```csharp
var book = await _context.Books.FindAsync(id);
```

Flow:

```text
FindAsync(id)
     ↓
Search primary key
     ↓
Book entity / null
```

<br>

## 5.3 `FirstAsync()` vs `FirstOrDefaultAsync()`

EF Core also provides:

```csharp
FirstAsync()
FirstOrDefaultAsync()
```

### `FirstAsync()`

If no matching record exists, `FirstAsync()` throws an exception.

### `FirstOrDefaultAsync()`

If no matching record exists:

```text
No record
   ↓
null
```

This lets the controller handle the missing record explicitly.

> [!Important]
> `FirstAsync()` → throws if no record exists.  
> `FirstOrDefaultAsync()` → returns `null` if no record exists.

For a primary-key lookup, the tutorial uses `FindAsync(id)`.

<br>

## 5.4 Repository Implementation

Implement the method in `BookRepository.cs`:

```csharp
public async Task<BookModel> GetBookById(int id)
{
    var book = await _context.Books.FindAsync(id);

    if (book == null)
    {
        return null;
    }

    var bookModel = new BookModel
    {
        Id = book.Id,
        Title = book.Title,
        Description = book.Description
    };

    return bookModel;
}
```

Flow:

```text
id = 2
 ↓
_context.Books.FindAsync(2)
 ↓
Book entity
 ↓
Map Book → BookModel
 ↓
Return BookModel
```

If the ID doesn't exist:

```text
FindAsync(999)
       ↓
null
       ↓
return null
```

<br>

## 5.5 Controller Route

Use:

```csharp
[HttpGet("{id}")]
```

Complete action:

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetBookById(int id)
{
    var book = await _bookRepository.GetBookById(id);

    if (book == null)
    {
        return NotFound();
    }

    return Ok(book);
}
```

If the controller has:

```csharp
[Route("api/[controller]")]
```

the endpoint becomes:

```text
GET /api/books/{id}
```

For example:

```text
GET /api/books/2
```

<br>

## 5.6 Handle `404 Not Found`

If the client requests:

```http
GET /api/books/999
```

and ID `999` doesn't exist, the repository returns `null`.

The controller checks:

```csharp
if (book == null)
{
    return NotFound();
}
```

Therefore:

```text
Book exists
    ↓
Ok(book)
    ↓
200 OK
```

or:

```text
Book does not exist
    ↓
NotFound()
    ↓
404 Not Found
```

> [!Important]
> A non-existent requested resource is handled with **404 Not Found**.

<br>

## 5.7 Test the API

### Existing ID

Request:

```http
GET https://localhost:XXXX/api/books/2
```

Expected status:

```text
200 OK
```

### Non-Existing ID

Request:

```http
GET https://localhost:XXXX/api/books/999
```

Expected status:

```text
404 Not Found
```

Always test both valid and non-existent IDs.

<br>

## 5.8 Expected Output

### Book Exists

Request:

```http
GET /api/books/2
```

Response:

```json
{
  "id": 2,
  "title": "ASP.NET Core",
  "description": "Learn Web API"
}
```

Status:

```text
200 OK
```

### Book Does Not Exist

Request:

```http
GET /api/books/999
```

Status:

```text
404 Not Found
```

The response body may be empty depending on configuration.

<br>

---

<br>

# 6. Complete Application Structure

After implementing both GET APIs:

```text
BookStore.API
│
├── Controllers
│   └── BooksController.cs
│
├── Data
│   └── BooksDbContext.cs
│
├── Models
│   └── BookModel.cs
│
├── Repository
│   ├── IBookRepository.cs
│   └── BookRepository.cs
│
├── Migrations
│   └── ...
│
├── appsettings.json
├── Startup.cs
└── Program.cs
```

### Layered Architecture

```text
                    CLIENT
                       │
                       │ HTTP GET
                       ↓
              ┌─────────────────┐
              │ BooksController │
              └────────┬────────┘
                       │
                       ↓
              ┌─────────────────┐
              │ IBookRepository │
              └────────┬────────┘
                       │
                       ↓
              ┌─────────────────┐
              │ BookRepository  │
              └────────┬────────┘
                       │
                       ↓
              ┌─────────────────┐
              │ BooksDbContext  │
              └────────┬────────┘
                       │
                       ↓
                    EF Core
                       │
                       ↓
                 SQL Server
```

<br>

---

<br>

# 7. Complete Code

## 7.1 `BookModel.cs`

```csharp
namespace BookStore.API.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
```

<br>

## 7.2 `IBookRepository.cs`

```csharp
using BookStore.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.API.Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookModel>> GetAllBooksAsync();

        Task<BookModel> GetBookById(int id);
    }
}
```

<br>

## 7.3 `BookRepository.cs`

```csharp
using BookStore.API.Data;
using BookStore.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.API.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BooksDbContext _context;

        public BookRepository(BooksDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookModel>> GetAllBooksAsync()
        {
            var books = await _context.Books.ToListAsync();

            var bookModels = new List<BookModel>();

            foreach (var book in books)
            {
                var bookModel = new BookModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description
                };

                bookModels.Add(bookModel);
            }

            return bookModels;
        }

        public async Task<BookModel> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return null;
            }

            var bookModel = new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description
            };

            return bookModel;
        }
    }
}
```

<br>

## 7.4 `BooksController.cs`

```csharp
using BookStore.API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooksAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
    }
}
```

> [!Note]
> Namespace names may differ in the actual BookStore.API project. The important architecture is **Controller → Repository → DbContext → EF Core → SQL Server**.

<br>

---

<br>

# 8. Debugging / Breakpoints

For:

```http
GET /api/books
```

put breakpoints at:

### Controller

```csharp
var books = await _bookRepository.GetAllBooksAsync();
```

### Repository

```csharp
var books = await _context.Books.ToListAsync();
```

Then follow:

```text
Request
  ↓
Controller breakpoint
  ↓
Repository breakpoint
  ↓
EF Core query
  ↓
Database
  ↓
Book entities
  ↓
BookModel mapping
  ↓
Controller
  ↓
Ok(...)
```

For:

```http
GET /api/books/2
```

observe:

```text
Controller
  ↓
GetBookById(2)
  ↓
Repository
  ↓
FindAsync(2)
  ↓
Book / null
  ↓
Controller
  ↓
Ok() / NotFound()
```

<br>

---

<br>

# 9. Important Points

### 9.1 `async` / `await`

Database operations can take time. The tutorial uses asynchronous EF Core operations such as:

```csharp
await _context.Books.ToListAsync();
```

### 9.2 Why Repository?

The controller focuses on:

```text
HTTP Request
HTTP Response
```

while the repository focuses on:

```text
Database operations
```

### 9.3 Why Interface?

The controller depends on:

```csharp
IBookRepository
```

rather than directly depending on the concrete repository implementation.

### 9.4 Why `BookModel`?

It separates:

```text
Database Entity
```

from:

```text
API Response Model
```

### 9.5 `FindAsync()` vs `FirstOrDefaultAsync()`

```text
Primary-key lookup
       ↓
FindAsync()
```

For other filtering conditions, methods such as:

```csharp
FirstOrDefaultAsync(...)
```

can be used.

### 9.6 Missing Record

```text
Record exists
   ↓
200 OK
```

```text
Record does not exist
   ↓
404 Not Found
```

### 9.7 Mapping

For small models, manual mapping is manageable. For large models, automated mapping tools such as AutoMapper can reduce repetitive mapping code; the tutorial indicates this will be covered later.

<br>

---

<br>

# 10. Final Mental Model

## GET All

```text
GET /api/books
       ↓
BooksController
       ↓
GetAllBooks()
       ↓
IBookRepository
       ↓
BookRepository
       ↓
_context.Books
       ↓
ToListAsync()
       ↓
List<Book>
       ↓
Map → BookModel
       ↓
Ok(bookModels)
       ↓
200 OK + JSON array
```

## GET One

```text
GET /api/books/2
       ↓
BooksController
       ↓
GetBookById(2)
       ↓
IBookRepository
       ↓
BookRepository
       ↓
FindAsync(2)
       ↓
Book / null
       ↓
Map → BookModel
       ↓
┌───────────────┴───────────────┐
↓                               ↓
Book found                  Book not found
↓                               ↓
Ok(book)                    NotFound()
↓                               ↓
200 OK                      404 Not Found
```

## Complete BookStore API Architecture

```text
                         CLIENT
                           │
                           │ HTTP GET
                           ↓
                    BooksController
                           │
                           │ DI
                           ↓
                   IBookRepository
                           │
                           ↓
                    BookRepository
                           │
                           │ DI
                           ↓
                    BooksDbContext
                           │
                           ↓
                        EF Core
                           │
                           ↓
                       SQL Server
                           │
                           ↓
                    Database Records
                           │
                           ↓
                     Book Entity
                           │
                           ↓
                      BookModel
                           │
                           ↓
                       JSON Response
```

> [!Important]
> The complete pattern learned here is:
>
> **HTTP Request → Controller → Repository → DbContext → EF Core → Database → Entity → API Model → HTTP Response**
>
> `GET /api/books` → **200 OK + list**
>
> `GET /api/books/{id}` → **200 OK + object** or **404 Not Found**
