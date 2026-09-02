# REST APIs — ASP.NET Core Web API

## 1. What is an API?

**API (Application Programming Interface)** is a way for one software/application to communicate with another.

Example:

```text
Frontend / Mobile App
        |
        | HTTP Request
        v
ASP.NET Core Web API
        |
        v
Database
        |
        v
ASP.NET Core Web API
        |
        | HTTP Response
        v
Frontend / Mobile App
```

An API exposes functionality/data without exposing how the internal implementation works.

<br>

# 2. ASP.NET Core Web API

**ASP.NET Core Web API** is a framework for building HTTP-based APIs using C# and .NET.

It is commonly used to build:

- REST APIs
- Backend services
- Microservices
- APIs consumed by web/mobile applications

Example:

```http
GET /api/movies/10
```

The client sends an HTTP request and the ASP.NET Core application processes it and returns an HTTP response.

<br>

# 3. REST

**REST = Representational State Transfer**

REST is an **architectural style**, not a programming language or framework.

A RESTful API generally:

- represents data as **resources**
- identifies resources using **URIs**
- uses standard **HTTP methods**
- uses standard **HTTP status codes**
- is **stateless**
- commonly exchanges data as JSON

Example resource:

```text
Movie
```

REST-style endpoints:

```http
GET    /api/movies
GET    /api/movies/10
POST   /api/movies
PUT    /api/movies/10
DELETE /api/movies/10
```

The URL identifies the **resource**; the HTTP method describes the **operation**.

<br>

# 4. REST API vs Normal/General API

An API is a broad concept.

A REST API is an API designed around REST principles and HTTP.

| General API | REST API |
|---|---|
| Broad concept | Specific architectural style |
| Can use different communication mechanisms | Uses HTTP |
| Can define custom operations | Prefer standard HTTP methods |
| Resource-oriented design is not required | Resource-oriented |
| Rules depend on implementation | Follows REST constraints |

So:

```text
API
├── REST API
├── SOAP API
├── GraphQL API
└── Other API styles
```

<br>

# 5. HTTP

**HTTP (HyperText Transfer Protocol)** is the communication protocol commonly used by REST APIs.

Communication consists of:

```text
Client
   |
   | HTTP Request
   v
Server
   |
   | HTTP Response
   v
Client
```

<br>

# 6. HTTP Request

An HTTP request can contain:

```text
Request
├── Method
├── URL
├── Headers
└── Body
```

Example:

```http
POST /api/movies HTTP/1.1
Content-Type: application/json

{
    "name": "Inception",
    "year": 2010
}
```

### Main components

**Method**

```text
POST
```

Specifies what kind of operation is requested.

**URL**

```text
/api/movies
```

Identifies the target resource.

**Headers**

Contain metadata about the request.

Example:

```http
Content-Type: application/json
Authorization: Bearer <token>
```

**Body**

Contains data sent to the server.

Usually used with `POST`, `PUT`, or `PATCH`.

<br>

# 7. HTTP Response

An HTTP response generally contains:

```text
Response
├── Status Code
├── Headers
└── Body
```

Example:

```http
HTTP/1.1 200 OK
Content-Type: application/json

{
    "id": 10,
    "name": "Inception",
    "year": 2010
}
```

<br>

# 8. HTTP Methods / Verbs

HTTP methods describe the intended operation.

| Method | Typical purpose |
|---|---|
| GET | Retrieve resource |
| POST | Create resource |
| PUT | Replace/update resource |
| PATCH | Partially update resource |
| DELETE | Delete resource |

### GET

Retrieve data.

```http
GET /api/movies
```

```http
GET /api/movies/10
```

### POST

Create a new resource.

```http
POST /api/movies
```

Body:

```json
{
    "name": "Inception",
    "year": 2010
}
```

### PUT

Replace/update an existing resource.

```http
PUT /api/movies/10
```

### PATCH

Partially update an existing resource.

```http
PATCH /api/movies/10
```

Example:

```json
{
    "year": 2011
}
```

### DELETE

Delete a resource.

```http
DELETE /api/movies/10
```

<br>

# 9. HTTP Status Codes

Status codes communicate the result of a request.

They are grouped into categories:

| Range | Category |
|---|---|
| 1xx | Informational |
| 2xx | Success |
| 3xx | Redirection |
| 4xx | Client error |
| 5xx | Server error |

## Frequently used codes

### 200 OK

Request succeeded.

```http
GET /api/movies/10
→ 200 OK
```

### 201 Created

A new resource was successfully created.

```http
POST /api/movies
→ 201 Created
```

### 204 No Content

Request succeeded but there is no response body.

Common for successful DELETE or update operations.

```http
DELETE /api/movies/10
→ 204 No Content
```

### 400 Bad Request

The request is invalid.

Example:

```text
Invalid input
Invalid request format
Validation failure
```

### 401 Unauthorized

The client is not authenticated or authentication credentials are invalid.

```text
"Who are you?"
```

### 403 Forbidden

The client is authenticated but does not have permission.

```text
"I know who you are, but you cannot do this."
```

### 404 Not Found

Requested resource does not exist.

```http
GET /api/movies/999
→ 404 Not Found
```

### 409 Conflict

Request conflicts with the current state of the resource.

Example:

```text
Creating a user with an email that already exists.
```

### 500 Internal Server Error

Unexpected error occurred on the server.

<br>

# 10. Resource-Oriented URL Design

REST APIs should generally use **nouns**, not actions, in URLs.

Good:

```http
GET /api/movies
POST /api/movies
GET /api/movies/10
DELETE /api/movies/10
```

Avoid action-based URLs such as:

```http
GET /api/getMovies
POST /api/createMovie
POST /api/deleteMovie
```

The HTTP method already describes the operation.

Think:

```text
URL  → What resource?
Method → What should happen to it?
```

<br>

# 11. Route Parameters

A **route parameter** identifies a specific resource.

```http
GET /api/movies/10
```

Here:

```text
10 → movieId
```

ASP.NET Core:

```csharp
[HttpGet("{id}")]
public IActionResult GetMovie(int id)
{
    ...
}
```

URL:

```text
/api/movies/10
```

Use route parameters when the value identifies a specific resource.

<br>

# 12. Query Parameters

Query parameters are used for **filtering, searching, sorting, pagination, and optional parameters**.

Example:

```http
GET /api/movies?year=2025
```

Multiple parameters:

```http
GET /api/movies?year=2025&genre=Action
```

### Pagination

```http
GET /api/movies?page=2&pageSize=20
```

### Sorting

```http
GET /api/movies?sortBy=rating&order=desc
```

### Search

```http
GET /api/movies?search=inception
```

### Rule of thumb

Use:

```text
Route parameter → identify a resource
Query parameter → filter/modify the collection request
Body → send substantial data to create/update a resource
```

<br>

# 13. Request Body

The request body contains data being sent to the server.

Commonly used for:

```text
POST
PUT
PATCH
```

Example:

```http
POST /api/movies
Content-Type: application/json

{
    "name": "Inception",
    "year": 2010,
    "rating": 8.8
}
```

In ASP.NET Core:

```csharp
[HttpPost]
public IActionResult CreateMovie(Movie movie)
{
    ...
}
```

For larger/structured input, use a DTO rather than exposing database entities directly.

<br>

# 14. Route vs Query vs Body

Suppose we have a movie API.

### Specific movie

```http
GET /api/movies/10
```

`10` → route parameter.

### Filter movies

```http
GET /api/movies?genre=Action&year=2025
```

`genre` and `year` → query parameters.

### Create movie

```http
POST /api/movies
```

Data → request body.

```json
{
    "name": "Inception",
    "year": 2010
}
```

<br>

# 15. Pagination

Do not return thousands/millions of records in one response.

Instead, return data in pages.

Example:

```http
GET /api/movies?page=2&pageSize=20
```

Meaning:

```text
page     = which page
pageSize = records per page
```

Typical response:

```json
{
    "items": [
        ...
    ],
    "page": 2,
    "pageSize": 20,
    "totalCount": 150
}
```

Pagination improves:

- performance
- response size
- database load
- client experience

<br>

# 16. Statelessness

One of the important REST constraints is **statelessness**.

Each request should contain the information required to process it.

The server should not depend on remembering the client's previous request in order to understand the current request.

Example:

```http
GET /api/orders/100
Authorization: Bearer <token>
```

The server can process this request using the information in the current request and server-side data.

Stateless does **not** mean the server cannot have a database or persistent data.

It means the server should not depend on storing conversational client state between requests.

<br>

# 17. ASP.NET Core Controller

A controller handles incoming HTTP requests.

Example:

```csharp
[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetMovies()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetMovie(int id)
    {
        return Ok();
    }

    [HttpPost]
    public IActionResult CreateMovie(Movie movie)
    {
        return Created();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(int id)
    {
        return NoContent();
    }
}
```

The controller connects HTTP requests to application logic.

A common architecture is:

```text
HTTP Request
     ↓
Controller
     ↓
Service
     ↓
Repository / Data Access
     ↓
Database
```

<br>

# 18. `[ApiController]`

`[ApiController]` marks a controller as an API controller.

It provides useful API-specific behavior such as:

- automatic model validation responses
- binding-source inference
- improved parameter binding behavior

Example:

```csharp
[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
}
```

<br>

# 19. `ControllerBase`

For APIs, controllers generally inherit from:

```csharp
ControllerBase
```

Example:

```csharp
public class MoviesController : ControllerBase
{
}
```

`ControllerBase` provides API-related functionality such as:

```csharp
Ok()
BadRequest()
NotFound()
Created()
NoContent()
```

<br>

# 20. Common API Response Methods

Instead of manually constructing every HTTP response:

```csharp
return Ok(movie);
```

```csharp
return NotFound();
```

```csharp
return BadRequest();
```

```csharp
return Created();
```

```csharp
return NoContent();
```

Examples:

```csharp
if (movie == null)
    return NotFound();

return Ok(movie);
```

<br>

# 21. Typical CRUD API

For a `Movie` resource:

| Operation | Method | Endpoint |
|---|---|--|
| Get all | GET | `/api/movies` |
| Get one | GET | `/api/movies/{id}` |
| Create | POST | `/api/movies` |
| Replace/update | PUT | `/api/movies/{id}` |
| Partial update | PATCH | `/api/movies/{id}` |
| Delete | DELETE | `/api/movies/{id}` |

This is the basic pattern to remember:

```text
Collection:
GET    /movies
POST   /movies

Individual resource:
GET    /movies/{id}
PUT    /movies/{id}
PATCH  /movies/{id}
DELETE /movies/{id}
```

<br>

# 22. Mental Model for Designing REST APIs

When designing an API, ask these questions in order:

### 1. What are my resources?

Example:

```text
Customer
Order
Product
Movie
Actor
```

### 2. What identifies each resource?

```text
Customer → customerId
Order    → orderId
Movie    → movieId
```

### 3. What operations are required?

```text
Create
Read
Update
Delete
Search
Filter
```

### 4. Which HTTP method represents the operation?

```text
Create → POST
Read   → GET
Update → PUT/PATCH
Delete → DELETE
```

### 5. Where should input go?

```text
Resource identity → Route
Filtering/search → Query
Create/update data → Body
```

### 6. What status code should be returned?

```text
Success → 2xx
Invalid client request → 4xx
Server failure → 5xx
```

<br>

# 23. Core REST Principles to Remember

For interviews and practical development, remember:

1. **Resources** are the central concept.
2. Use **URIs to identify resources**.
3. Use **HTTP methods** to express operations.
4. Use appropriate **HTTP status codes**.
5. Keep APIs **stateless**.
6. Prefer **nouns over verbs** in resource URLs.
7. Use **query parameters** for filtering, sorting, searching, and pagination.
8. Use **route parameters** to identify specific resources.
9. Use the **request body** for structured create/update data.
10. Return consistent, meaningful responses.
11. Use pagination for large collections.
12. Keep controller responsibilities focused; business logic should normally live in services rather than controllers.

<br>

# Quick Revision

```text
REST
 ↓
Resources + HTTP

Resource:
Movie

Endpoints:
GET    /api/movies
GET    /api/movies/10
POST   /api/movies
PUT    /api/movies/10
PATCH  /api/movies/10
DELETE /api/movies/10

Route:
 /movies/10
         ↑
      identity

Query:
 /movies?genre=Action&page=2
         ↑
      filtering/pagination

Body:
POST /movies

{
    "name": "Inception",
    "year": 2010
}

Response:
Status Code + Headers + Body

200 → Success
201 → Created
204 → Success, no body
400 → Bad request
401 → Not authenticated
403 → Not permitted
404 → Not found
409 → Conflict
500 → Server error
```
