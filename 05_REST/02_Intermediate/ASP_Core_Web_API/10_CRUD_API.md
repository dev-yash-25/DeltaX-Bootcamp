# HTTP Database CRUD APIs — GET, POST, PUT, PATCH & DELETE
## ASP.NET Core 5.0 Web API + Entity Framework Core

## Index

- [1. What We Are Building](#1-what-we-are-building)
- [2. Application Structure](#2-application-structure)
- [3. Layer Responsibilities](#3-layer-responsibilities)
- [4. Common Request Flow](#4-common-request-flow)
- [5. GET — Retrieve Data](#5-get--retrieve-data)
  - [5.1 GET All Books](#51-get-all-books)
  - [5.2 GET One Book by ID](#52-get-one-book-by-id)
  - [5.3 `FindAsync()` vs `FirstAsync()` vs `FirstOrDefaultAsync()`](#53-findasync-vs-firstasync-vs-firstordefaultasync)
  - [5.4 GET Testing & Output](#54-get-testing--output)
- [6. POST — Create a New Book](#6-post--create-a-new-book)
  - [6.1 Repository Implementation](#61-repository-implementation)
  - [6.2 Controller Action](#62-controller-action)
  - [6.3 `CreatedAtAction()`](#63-createdataction)
  - [6.4 POST Testing & Output](#64-post-testing--output)
- [7. PUT — Update an Existing Book](#7-put--update-an-existing-book)
  - [7.1 What Is Required](#71-what-is-required)
  - [7.2 EF Core Change Tracking](#72-ef-core-change-tracking)
  - [7.3 Controller Implementation](#73-controller-implementation)
  - [7.4 Testing & Output](#74-testing--output)
- [8. PUT — Update Using One Database Call](#8-put--update-using-one-database-call)
- [9. PATCH — Partially Update a Book](#9-patch--partially-update-a-book)
  - [9.1 PUT vs PATCH](#91-put-vs-patch)
  - [9.2 Dependencies](#92-dependencies)
  - [9.3 Newtonsoft JSON Configuration](#93-newtonsoft-json-configuration)
  - [9.4 Controller Implementation](#94-controller-implementation)
  - [9.5 `JsonPatchDocument`](#95-jsonpatchdocument)
  - [9.6 Postman Testing](#96-postman-testing)
  - [9.7 PATCH Operations](#97-patch-operations)
- [10. DELETE — Delete a Book](#10-delete--delete-a-book)
  - [10.1 Delete by Primary Key](#101-delete-by-primary-key)
  - [10.2 Delete by Non-Primary Field](#102-delete-by-non-primary-field)
  - [10.3 Controller Implementation](#103-controller-implementation)
  - [10.4 Testing & Output](#104-testing--output)
- [11. Complete CRUD Summary](#11-complete-crud-summary)
- [12. Status Codes](#12-status-codes)
- [13. Debugging & Verification Pattern](#13-debugging--verification-pattern)
- [14. Important Concepts to Remember](#14-important-concepts-to-remember)
- [15. Final Mental Model](#15-final-mental-model)


<br>

---

<br>



# 1. What We Are Building

We have a **BookStore.API** application connected to SQL Server through **Entity Framework Core**.

We now implement the complete database CRUD operations:

| Operation | HTTP Method | Endpoint |
|---|---|---|
| Read all | `GET` | `/api/books` |
| Read one | `GET` | `/api/books/{id}` |
| Create | `POST` | `/api/books` |
| Complete update | `PUT` | `/api/books/{id}` |
| Partial update | `PATCH` | `/api/books/{id}` |
| Delete | `DELETE` | `/api/books/{id}` |

The core architecture is:

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
Entity Framework Core
       ↓
SQL Server
```


<br>

---

<br>

# 2. Application Structure

The application is organized using the **Repository Pattern**.

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

### Layered architecture

```text
                    CLIENT
                       │
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

> [!Important]
> The controller handles **HTTP requests/responses**. Database-access logic belongs in the repository.


<br>

---

<br>

# 3. Layer Responsibilities

| Component | Responsibility |
|---|---|
| `BooksController` | Receives HTTP requests and returns HTTP responses |
| `IBookRepository` | Defines database operations available to the controller |
| `BookRepository` | Implements database-access logic |
| `BooksDbContext` | Provides EF Core access to the database |
| `Book` | Database entity |
| `BookModel` | API model used for communication with the client |
| EF Core | ORM that translates between objects and relational database operations |
| SQL Server | Persistent storage |

### Why use an interface?

The controller depends on:

```csharp
IBookRepository
```

instead of directly depending on:

```csharp
BookRepository
```

This keeps the controller decoupled from the concrete implementation.

### Why use `BookModel`?

It separates the:

```text
Database Entity
```

from the:

```text
API Model
```

Conceptually:

```text
Book Entity
    ↓
Mapping
    ↓
BookModel
    ↓
JSON Response
```

For small models, manual mapping is manageable. For larger models, automated mapping tools such as AutoMapper can reduce repetitive mapping code.


<br>

---

<br>

# 4. Common Request Flow

A database API follows this general pattern:

```text
HTTP Request
     ↓
Controller
     ↓
Repository Interface
     ↓
Repository Implementation
     ↓
DbContext
     ↓
EF Core
     ↓
Database
     ↓
Entity
     ↓
Model / Response
     ↓
HTTP Response
```

The controller should not directly contain database-querying logic.

### `async` / `await`

Database operations can take time, so asynchronous EF Core operations are used:

```csharp
await _context.Books.ToListAsync();
```

Other operations similarly use:

```csharp
await _context.Books.FindAsync(id);
```

and:

```csharp
await _context.SaveChangesAsync();
```


<br>

---

<br>

# 5. GET — Retrieve Data

GET is used to **read** existing resources.

There are two APIs:

```text
GET /api/books
```

and:

```text
GET /api/books/{id}
```

---

## 5.1 GET All Books

### Endpoint

```http
GET /api/books
```

Example:

```http
GET https://localhost:XXXX/api/books
```

### Step 1 — API Model

The database entity may look like:

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
```

Create:

```text
Models/BookModel.cs
```

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

### Step 2 — Repository Interface

```csharp
Task<IEnumerable<BookModel>> GetAllBooksAsync();
```

Complete interface portion:

```csharp
public interface IBookRepository
{
    Task<IEnumerable<BookModel>> GetAllBooksAsync();
}
```

### Step 3 — Inject `BooksDbContext`

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

### Step 4 — Retrieve the records

```csharp
var books = await _context.Books.ToListAsync();
```

`ToListAsync()` asynchronously retrieves the records.

### Step 5 — Map `Book` → `BookModel`

```csharp
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

### Complete `GetAllBooksAsync()`

```csharp
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
```

### Step 6 — Controller

```csharp
[HttpGet]
public async Task<IActionResult> GetAllBooks()
{
    var books = await _bookRepository.GetAllBooksAsync();

    return Ok(books);
}
```

### Flow

```text
GET /api/books
      ↓
GetAllBooks()
      ↓
_bookRepository.GetAllBooksAsync()
      ↓
BookRepository
      ↓
_context.Books.ToListAsync()
      ↓
List<Book>
      ↓
Map → BookModel
      ↓
Ok(books)
      ↓
200 OK
```

---

## 5.2 GET One Book by ID

### Endpoint

```http
GET /api/books/{id}
```

Example:

```http
GET /api/books/2
```

### Step 1 — Repository Interface

```csharp
Task<BookModel> GetBookById(int id);
```

Complete interface:

```csharp
public interface IBookRepository
{
    Task<IEnumerable<BookModel>> GetAllBooksAsync();

    Task<BookModel> GetBookById(int id);
}
```

### Step 2 — Find by Primary Key

For a primary-key lookup:

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

### Step 3 — Handle `null`

```csharp
if (book == null)
{
    return null;
}
```

### Step 4 — Map the entity

```csharp
var bookModel = new BookModel
{
    Id = book.Id,
    Title = book.Title,
    Description = book.Description
};

return bookModel;
```

### Complete Repository Method

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

### Step 5 — Controller

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

### Existing record

```text
Book exists
   ↓
Ok(book)
   ↓
200 OK
```

### Missing record

```text
Book does not exist
   ↓
NotFound()
   ↓
404 Not Found
```

> [!Important]
> A requested resource that does not exist is handled with `404 Not Found`.

---

## 5.3 `FindAsync()` vs `FirstAsync()` vs `FirstOrDefaultAsync()`

### `FindAsync()`

Best suited to a primary-key lookup:

```csharp
FindAsync(id)
```

### `FirstAsync()`

Returns the first matching record.

If no record exists, it throws an exception.

### `FirstOrDefaultAsync()`

Returns the first matching record, or:

```text
null
```

if no record exists.

```text
FirstAsync()
→ throws if no match

FirstOrDefaultAsync()
→ null if no match

FindAsync()
→ primary-key lookup
```

---

## 5.4 GET Testing & Output

Suppose the database contains:

| ID | Title | Description |
|---:|---|---|
| 1 | C# Basics | Learn C# |
| 2 | ASP.NET Core | Learn Web API |
| 3 | SQL | Learn databases |

### GET all

```http
GET /api/books
```

Response:

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

Status:

```text
200 OK
```

### GET one

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

### Non-existent ID

```http
GET /api/books/999
```

Status:

```text
404 Not Found
```

The response body may be empty depending on configuration.

### Postman / Browser

The exact port depends on `launchSettings.json`.

Example:

```text
https://localhost:5001/api/books
```

For Postman:

```text
Method: GET
URL: https://localhost:XXXX/api/books
```

Then:

```text
Send
```


<br>

---

<br>

# 6. POST — Create a New Book

POST is used to **create a new resource**.

### Endpoint

```http
POST /api/books
```

The new book's data is sent in the request body.

Example:

```json
{
    "title": "The Alchemist",
    "description": "A story about following your dreams."
}
```

---

## 6.1 Repository Implementation

The repository works with the database `Book` entity, while the controller receives a `BookModel`.

Therefore:

```text
BookModel
   ↓ mapping
Book
   ↓
Database
```

### Step 1 — Create the entity

```csharp
var book = new Book
{
    Title = bookModel.Title,
    Description = bookModel.Description
};
```

Do **not** manually set the primary key:

```csharp
Id = ...
```

The database generates it.

### Step 2 — Add to EF Core context

```csharp
_context.Books.Add(book);
```

This adds the entity to the EF Core context/tracking system.

It does not by itself permanently persist the row.

### Step 3 — Save

```csharp
await _context.SaveChangesAsync();
```

This persists the new record.

### Step 4 — Get generated ID

After saving:

```csharp
book.Id
```

contains the database-generated ID.

The repository can return this ID to the controller.

### Repository pattern

Conceptually:

```csharp
public async Task<int> AddNewBook(BookModel bookModel)
{
    var book = new Book
    {
        Title = bookModel.Title,
        Description = bookModel.Description
    };

    _context.Books.Add(book);

    await _context.SaveChangesAsync();

    return book.Id;
}
```

> [!Important]
> `Add()` prepares/tracks the entity for insertion. `SaveChangesAsync()` is what persists the change to the database.

---

## 6.2 Controller Action

Use:

```csharp
[HttpPost]
```

and:

```csharp
[FromBody]
```

to receive the JSON request body.

Conceptually:

```csharp
[HttpPost]
public async Task<IActionResult> AddNewBook([FromBody] BookModel bookModel)
{
    // add book
}
```

Flow:

```text
POST /api/books
      ↓
JSON body
      ↓
[FromBody]
      ↓
BookModel
      ↓
Repository
      ↓
Book entity
      ↓
Add()
      ↓
SaveChangesAsync()
      ↓
Generated ID
```

---

## 6.3 `CreatedAtAction()`

A successful creation should normally communicate:

```text
201 Created
```

rather than simply:

```text
200 OK
```

ASP.NET Core provides:

```csharp
CreatedAtAction()
```

The tutorial describes three important arguments:

```text
CreatedAtAction(
    1. Get action name,
    2. Route values,
    3. Response object
)
```

Example:

```csharp
return CreatedAtAction(
    nameof(GetBookById),
    new { id = bookId, controller = "Books" },
    bookId
);
```

If the generated ID is:

```text
13
```

the corresponding GET endpoint is:

```text
GET /api/books/13
```

So the response communicates both that the resource was created and where it can be retrieved.

---

## 6.4 POST Testing & Output

### Step 1 — Postman

Select:

```text
POST
```

Example:

```text
POST http://localhost:<port>/api/books
```

### Step 2 — Body

Select:

```text
Body
  → raw
  → JSON
```

### Step 3 — Request body

```json
{
    "title": "The Alchemist",
    "description": "A story about following your dreams."
}
```

### Step 4 — Response

Expected:

```text
201 Created
```

### Step 5 — Database verification

Check SQL Server and confirm the record exists.

### Generated IDs

Identity IDs do not necessarily fill gaps caused by deletion.

For example:

```text
10
11
12
```

After deleting `10`, a later insert may receive:

```text
13
```

rather than reusing `10`.

### Step 6 — Verify with GET

```text
GET /api/books
```

and:

```text
GET /api/books/13
```

Example:

```json
{
    "id": 13,
    "title": "The Alchemist",
    "description": "A story about following your dreams."
}
```


<br>

---

<br>

# 7. PUT — Update an Existing Book

PUT is used to update an existing resource.

Example:

```http
PUT /api/books/13
```

Request body:

```json
{
    "title": "The Alchemist - Updated",
    "description": "Updated description."
}
```

---

## 7.1 What Is Required?

Two pieces of information are required:

### 1. Unique ID

Identifies:

```text
Which record?
```

Example:

```text
13
```

### 2. Updated data model

Specifies:

```text
What should it become?
```

So:

```text
Route
  ↓
id = 13

Body
  ↓
updated BookModel
```

---

## 7.2 EF Core Change Tracking

The traditional approach is:

```text
1. Fetch existing record
2. Modify its properties
3. Save changes
```

Example:

```csharp
var book = await _context.Books.FindAsync(id);

book.Title = bookModel.Title;
book.Description = bookModel.Description;

await _context.SaveChangesAsync();
```

When EF Core retrieves the entity through the context, it tracks it.

If you modify:

```csharp
book.Title = bookModel.Title;
```

EF Core detects the change.

Then:

```csharp
await _context.SaveChangesAsync();
```

persists the update.

Mental model:

```text
Database
   ↓
FindAsync()
   ↓
Tracked Book
   ↓
Change properties
   ↓
SaveChangesAsync()
   ↓
Database UPDATE
```

---

## 7.3 Controller Implementation

Use:

```csharp
[HttpPut("{id}")]
```

and:

```csharp
[FromBody]
```

Conceptually:

```csharp
[HttpPut("{id}")]
public async Task<IActionResult> UpdateBook(
    int id,
    [FromBody] BookModel bookModel)
{
    // update
}
```

### Steps

```text
PUT /api/books/13
       ↓
Receive id = 13
       ↓
Receive BookModel from body
       ↓
Find book
       ↓
Change properties
       ↓
SaveChangesAsync()
       ↓
200 OK
```

---

## 7.4 Testing & Output

### Request

```text
PUT http://localhost:<port>/api/books/13
```

Body:

```json
{
    "title": "The Alchemist - Updated",
    "description": "Updated description."
}
```

Expected response:

```text
200 OK
```

Then verify:

```text
GET /api/books/13
```

Expected:

```json
{
    "id": 13,
    "title": "The Alchemist - Updated",
    "description": "Updated description."
}
```

> [!Tip]
> After a write operation, using GET to verify the persisted state is a simple and useful testing habit.


<br>

---

<br>

# 8. PUT — Update Using One Database Call

The traditional update can involve:

```text
Database Call #1
      ↓
Fetch existing record

Database Call #2
      ↓
Update record
```

The tutorial demonstrates an optimization that avoids fetching the existing object first.

### Traditional

```text
Find existing entity
        ↓
Modify
        ↓
Save
```

### One-call approach

Use the known ID to create an entity representing the existing record:

```csharp
var book = new Book
{
    Id = bookId,
    Title = bookModel.Title,
    Description = bookModel.Description
};
```

Then mark the entity as modified and save it.

Conceptually:

```text
Book object
   ↓
Id = existing ID
   ↓
Updated properties
   ↓
Entity marked Modified
   ↓
SaveChangesAsync()
```

A typical EF Core implementation is:

```csharp
_context.Books.Update(book);

await _context.SaveChangesAsync();
```

### Why this can be faster

There is no initial:

```csharp
FindAsync()
```

database read.

So the intended flow becomes:

```text
Known ID
   ↓
Create entity with ID
   ↓
Mark Modified
   ↓
SaveChangesAsync()
   ↓
Database UPDATE
```

> [!Important]
> This approach avoids the initial read, but it also means the application is not checking the current database values before updating. If existence/conflict validation is required, an explicit lookup or other database strategy may still be appropriate.

### Verification

```text
GET /api/books/13
       ↓
Copy current values
       ↓
Modify values
       ↓
PUT /api/books/13
       ↓
200 OK
       ↓
GET /api/books/13
       ↓
Verify updated values
```


<br>

---

<br>

# 9. PATCH — Partially Update a Book

PATCH is used for **partial updates**.

Example:

```http
PATCH /api/books/13
```

Instead of sending the complete resource, the client sends the changes to apply.

---

## 9.1 PUT vs PATCH

| | PUT | PATCH |
|---|---|---|
| Purpose | Update resource | Partially update resource |
| Request style | Updated representation | Operations/changes |
| One property | Usually send the model representation | Can change only that property |
| Multiple properties | Yes | Yes |
| All properties | Yes | Possible |

### Mental model

```text
PUT
→ "Here is the updated resource."

PATCH
→ "Here are the changes I want to make."
```

Example PATCH:

```json
[
    {
        "op": "replace",
        "path": "/title",
        "value": "New Title"
    }
]
```

The description is not changed.

> [!Important]
> PATCH is useful when you want to modify one or a few properties without replacing the complete resource representation.

---

## 9.2 Dependencies

For JSON Patch in ASP.NET Core 5.0, install:

1. `Microsoft.AspNetCore.JsonPatch`
2. `Microsoft.AspNetCore.Mvc.NewtonsoftJson`

### `Microsoft.AspNetCore.JsonPatch`

Provides:

```csharp
JsonPatchDocument
```

which represents the patch operations.

### `Microsoft.AspNetCore.Mvc.NewtonsoftJson`

Provides Newtonsoft JSON integration required for the JSON Patch implementation used in the tutorial.

---

## 9.3 Newtonsoft JSON Configuration

Inside `ConfigureServices()` in `Startup.cs`:

```csharp
services.AddControllers().AddNewtonsoftJson();
```

Example:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers().AddNewtonsoftJson();
}
```

This enables the required Newtonsoft JSON formatter.

---

## 9.4 Controller Implementation

Use:

```csharp
[HttpPatch("{id}")]
```

The ID comes from the route and the patch document comes from the request body.

Complete tutorial example:

```csharp
[HttpPatch("{id}")]
public async Task<IActionResult> UpdateBookPatch(
    [FromRoute] int id,
    [FromBody] JsonPatchDocument bookModel)
{
    var book = await _bookRepository.GetBookById(id);

    if (book == null)
        return NotFound();

    bookModel.ApplyTo(book);

    await _bookRepository.UpdateBook(book);

    return Ok();
}
```

### Flow

```text
PATCH Request
      ↓
Route ID
      +
JsonPatchDocument
      ↓
Get existing book
      ↓
ApplyTo(book)
      ↓
UpdateBook()
      ↓
Save changes
      ↓
200 OK
```

### Why fetch the existing object?

The patch operations need a target object to modify:

```csharp
bookModel.ApplyTo(book);
```

Therefore:

```text
Get existing object
      ↓
Apply patch
      ↓
Save
```

If the book does not exist:

```csharp
return NotFound();
```

returns:

```text
404 Not Found
```

---

## 9.5 `JsonPatchDocument`

A JSON Patch request is an **array of operations**.

Each operation can contain:

```text
op
path
value
```

### `op`

Defines the operation.

Example:

```json
"op": "replace"
```

### `path`

Identifies the property.

Example:

```json
"path": "/title"
```

### `value`

Contains the new value.

Example:

```json
"value": "New Title Value"
```

Complete operation:

```json
{
    "op": "replace",
    "path": "/title",
    "value": "New Title Value"
}
```

Meaning:

```text
Replace the title
with
"New Title Value"
```

---

## 9.6 Postman Testing

### Step 1 — Select PATCH

```text
PATCH http://localhost:<port>/api/books/13
```

### Step 2 — Body

```text
Body
  → raw
  → JSON
```

### Step 3 — Send operations

```json
[
  {
    "op": "replace",
    "path": "/title",
    "value": "New Title Value"
  },
  {
    "op": "replace",
    "path": "/description",
    "value": "New Description Value"
  }
]
```

Two operations are executed:

```text
Operation 1
→ replace /title

Operation 2
→ replace /description
```

### Updating only one property

```json
[
  {
    "op": "replace",
    "path": "/title",
    "value": "Only Title Changed"
  }
]
```

The description remains unchanged.

### Response

Successful update:

```text
200 OK
```

### Verification

```text
GET /api/books/13
```

Confirm that the requested properties changed.

---

## 9.7 PATCH Operations

The tutorial demonstrates `replace` and `remove`.

### Replace

```json
[
    {
        "op": "replace",
        "path": "/title",
        "value": "New Title"
    }
]
```

### Remove

```json
[
    {
        "op": "remove",
        "path": "/description"
    }
]
```

A `remove` operation can clear/remove a specific field where the target model/property supports it.

### Multiple operations

You can include multiple objects in the JSON array:

```json
[
    {
        "op": "replace",
        "path": "/title",
        "value": "New Title"
    },
    {
        "op": "replace",
        "path": "/description",
        "value": "New Description"
    }
]
```

Therefore PATCH can update:

```text
One property
Multiple properties
All properties
```

without requiring the client to send the complete model in the same manner as PUT.


<br>

---

<br>

# 10. DELETE — Delete a Book

DELETE removes an existing resource.

Example:

```http
DELETE /api/books/3
```

When deleting by primary key, the ID is enough to identify the record.

---

## 10.1 Delete by Primary Key

If the primary key is already known:

```text
ID = 3
```

we do not necessarily need to fetch the entire entity first.

### Repository interface

```csharp
Task DeleteBookAsync(int id);
```

### Repository implementation

```csharp
public async Task DeleteBookAsync(int id)
{
    var book = new Book() { Id = id };

    _context.Books.Remove(book);

    await _context.SaveChangesAsync();
}
```

### What happens?

#### Step 1 — Create entity with ID

```csharp
var book = new Book() { Id = id };
```

Only the ID is needed.

#### Step 2 — Mark for deletion

```csharp
_context.Books.Remove(book);
```

#### Step 3 — Persist deletion

```csharp
await _context.SaveChangesAsync();
```

### Flow

```text
DELETE /api/books/3
      ↓
id = 3
      ↓
Book { Id = 3 }
      ↓
Remove()
      ↓
SaveChangesAsync()
      ↓
DELETE
```

### Compared with fetching first

Traditional:

```text
GET book with ID 3
        ↓
Retrieve entire object
        ↓
Delete
```

Optimized approach demonstrated:

```text
Known ID
   ↓
Book { Id = 3 }
   ↓
Remove()
   ↓
SaveChangesAsync()
```

This avoids the extra fetch.

---

## 10.2 Delete by Non-Primary Field

If deletion is based on something other than the primary key, such as:

```text
Title
```

the application first needs to locate the matching record.

Conceptually:

```text
Title
  ↓
Query database
  ↓
Find matching Book
  ↓
Remove Book
  ↓
SaveChangesAsync()
```

The tutorial describes using:

```csharp
.Where(...)
```

to locate the record.

### Comparison

| Identification | Fetch required? |
|---|---|
| Primary key already known | Not necessarily |
| Non-primary field | Usually yes, to locate the entity |

---

## 10.3 Controller Implementation

The controller needs only the ID.

No request body is required.

```csharp
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteBook([FromRoute] int id)
{
    await _bookRepository.DeleteBookAsync(id);

    return Ok();
}
```

Flow:

```text
DELETE Request
      ↓
BooksController
      ↓
id = 3
      ↓
DeleteBookAsync(3)
      ↓
Book { Id = 3 }
      ↓
Remove()
      ↓
SaveChangesAsync()
      ↓
Database
      ↓
200 OK
```

---

## 10.4 Testing & Output

### Step 1 — Check database

Suppose:

```text
Id    Title
1     Book A
2     Book B
3     Book C
```

### Step 2 — Postman

Select:

```text
DELETE
```

### Step 3 — Endpoint

```text
DELETE http://localhost:<port>/api/books/3
```

### Step 4 — Send

The repository executes:

```csharp
DeleteBookAsync(3);
```

### Step 5 — Response

Expected:

```text
200 OK
```

### Step 6 — Verify with GET

```text
GET /api/books/3
```

Expected:

```text
404 Not Found
```

This confirms that the resource is no longer available through the GET-by-ID endpoint.


<br>

---

<br>

# 11. Complete CRUD Summary

After these tutorials, the BookStore API supports:

```text
CREATE
POST

READ
GET

UPDATE
PUT

PARTIAL UPDATE
PATCH

DELETE
DELETE
```

| Operation | HTTP Method | Endpoint | Purpose | Typical Success |
|---|---|---|---|---|
| Create | `POST` | `/api/books` | Add new book | `201 Created` |
| Read all | `GET` | `/api/books` | Get all books | `200 OK` |
| Read one | `GET` | `/api/books/{id}` | Get one book | `200 OK` |
| Update | `PUT` | `/api/books/{id}` | Update resource | `200 OK` |
| Partial update | `PATCH` | `/api/books/{id}` | Update selected properties | `200 OK` |
| Delete | `DELETE` | `/api/books/{id}` | Remove resource | `200 OK` |

### CRUD mental model

```text
GET
→ Give me data

POST
→ Create new data

PUT
→ Update the resource

PATCH
→ Change specific parts

DELETE
→ Remove the resource
```


<br>

---

<br>

# 12. Status Codes

| Scenario | Status |
|---|---|
| GET existing resource | `200 OK` |
| GET non-existent resource | `404 Not Found` |
| POST successful creation | `201 Created` |
| PUT successful update | `200 OK` |
| PATCH successful update | `200 OK` |
| DELETE successful deletion | `200 OK` |
| GET after successful deletion | `404 Not Found` |

> [!Note]
> These are the status codes demonstrated/recommended in the tutorials. In production APIs, exact response choices can vary based on API design.


<br>

---

<br>

# 13. Debugging & Verification Pattern

Debugging is easiest when you follow the request through each layer.

## GET All

Put breakpoints at:

```csharp
var books = await _bookRepository.GetAllBooksAsync();
```

and:

```csharp
var books = await _context.Books.ToListAsync();
```

Flow:

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

## GET One

```text
GET /api/books/2
       ↓
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

## POST

```text
POST /api/books
       ↓
BookModel
       ↓
Repository
       ↓
Book entity
       ↓
Add()
       ↓
SaveChangesAsync()
       ↓
Generated ID
       ↓
201 Created
```

## PUT

```text
PUT /api/books/13
       ↓
Route ID + BookModel
       ↓
Repository
       ↓
Find / Update
       ↓
SaveChangesAsync()
       ↓
200 OK
```

## PATCH

```text
PATCH /api/books/13
       ↓
JsonPatchDocument
       ↓
Get existing object
       ↓
ApplyTo()
       ↓
Update
       ↓
SaveChangesAsync()
       ↓
200 OK
```

## DELETE

```text
DELETE /api/books/3
       ↓
Route ID
       ↓
DeleteBookAsync(3)
       ↓
Book { Id = 3 }
       ↓
Remove()
       ↓
SaveChangesAsync()
       ↓
200 OK
```

### Recommended verification cycle

```text
POST
 ↓
GET

PUT
 ↓
GET

PATCH
 ↓
GET

DELETE
 ↓
GET
 ↓
404 Not Found
```

This confirms the actual persisted state rather than only checking the immediate HTTP response.


<br>

---

<br>

# 14. Important Concepts to Remember

## 14.1 `Add()` vs `SaveChangesAsync()`

```csharp
_context.Books.Add(book);
```

tracks/prepares the entity for insertion.

```csharp
await _context.SaveChangesAsync();
```

persists the pending change.

---

## 14.2 Database-generated ID

For POST, do not manually set the identity primary key.

```csharp
var book = new Book
{
    Title = bookModel.Title,
    Description = bookModel.Description
};
```

After:

```csharp
await _context.SaveChangesAsync();
```

the generated ID is available:

```csharp
book.Id
```

---

## 14.3 `CreatedAtAction()`

For creation:

```text
POST
 ↓
201 Created
 ↓
Point toward GET endpoint
```

Example:

```csharp
return CreatedAtAction(
    nameof(GetBookById),
    new { id = bookId, controller = "Books" },
    bookId
);
```

---

## 14.4 PUT needs the resource ID + updated data

```text
Route
 ↓
Which resource?

Body
 ↓
What should it become?
```

Example:

```text
PUT /api/books/13
```

with:

```json
{
    "title": "Updated Title",
    "description": "Updated Description"
}
```

---

## 14.5 PATCH describes changes

```text
PUT
→ updated resource representation

PATCH
→ operations to apply
```

JSON Patch:

```json
[
    {
        "op": "replace",
        "path": "/title",
        "value": "New Title"
    }
]
```

Important fields:

```text
op
path
value
```

---

## 14.6 DELETE by primary key can avoid a fetch

Known:

```text
Id = 3
```

can be represented as:

```csharp
var book = new Book() { Id = id };
```

then:

```csharp
_context.Books.Remove(book);
await _context.SaveChangesAsync();
```

---

## 14.7 Missing resources

For GET-by-ID:

```text
Record exists
   ↓
200 OK
```

```text
Record doesn't exist
   ↓
404 Not Found
```

PATCH explicitly checks for a missing record before applying the patch.

---

## 14.8 Mapping

The application separates:

```text
Book
```

from:

```text
BookModel
```

and manually maps properties:

```csharp
var bookModel = new BookModel
{
    Id = book.Id,
    Title = book.Title,
    Description = book.Description
};
```

For larger applications, automated mapping can reduce repetitive code.

---

## 14.9 Primary-key lookup

For a primary-key lookup:

```csharp
FindAsync(id)
```

is the natural EF Core method.

For other conditions:

```csharp
FirstOrDefaultAsync(...)
```

or query methods such as:

```csharp
Where(...)
```

can be used.

---

# 15. Final Mental Model

## Complete REST API

```text
                         BOOKSTORE API
                              │
        ┌─────────────────────┼─────────────────────┐
        │                     │                     │
       READ                  WRITE                DELETE
        │                     │                     │
       GET              POST / PUT / PATCH       DELETE
        │                     │                     │
        ↓                     ↓                     ↓
   Read resource         Create/Update         Remove resource
        │                     │                     │
        └─────────────────────┼─────────────────────┘
                              ↓
                       BooksController
                              ↓
                       IBookRepository
                              ↓
                       BookRepository
                              ↓
                       BooksDbContext
                              ↓
                         Entity Framework
                              ↓
                           SQL Server
```

### GET All

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

### GET One

```text
GET /api/books/2
       ↓
BooksController
       ↓
GetBookById(2)
       ↓
BookRepository
       ↓
FindAsync(2)
       ↓
Book / null
       ↓
┌───────────────┴───────────────┐
↓                               ↓
Book found                  Book not found
↓                               ↓
Ok(book)                    NotFound()
↓                               ↓
200 OK                      404 Not Found
```

### POST

```text
POST /api/books
       ↓
JSON body
       ↓
BookModel
       ↓
Map → Book
       ↓
Add()
       ↓
SaveChangesAsync()
       ↓
Generated ID
       ↓
CreatedAtAction()
       ↓
201 Created
```

### PUT

```text
PUT /api/books/{id}
       ↓
Route ID + JSON body
       ↓
BookModel
       ↓
Find / Update
       ↓
SaveChangesAsync()
       ↓
200 OK
```

### PATCH

```text
PATCH /api/books/{id}
       ↓
JsonPatchDocument
       ↓
Get existing object
       ↓
ApplyTo()
       ↓
Update
       ↓
SaveChangesAsync()
       ↓
200 OK
```

### DELETE

```text
DELETE /api/books/{id}
       ↓
Route ID
       ↓
Book { Id = id }
       ↓
Remove()
       ↓
SaveChangesAsync()
       ↓
200 OK
```

## The complete pattern

```text
HTTP Request
     ↓
Controller
     ↓
Repository
     ↓
DbContext
     ↓
EF Core
     ↓
Database
     ↓
Entity
     ↓
Model / Mapping
     ↓
HTTP Response
```

> [!Important]
> The main thing to understand is not just the five HTTP verbs. It is the **responsibility flow**: the controller handles HTTP, the repository handles data access, `DbContext` represents the EF Core database session, EF Core communicates with SQL Server, and the API converts the resulting data into the response model.
