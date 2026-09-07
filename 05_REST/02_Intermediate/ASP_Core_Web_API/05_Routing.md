# Routing

## Index

- [1. What is Routing?](#1-what-is-routing)
- [2. Enable Routing](#2-enable-routing)
- [3. Route on Action Method](#3-route-on-action-method)
- [4. Dynamic Route Values](#4-dynamic-route-values)
- [5. Query String](#5-query-string)
- [6. Token Replacement](#6-token-replacement)
- [7. Base Route at Controller Level](#7-base-route-at-controller-level)
- [8. Route Override using `~/`](#8-route-override-using-)
- [9. Multiple Routes for the Same Resource](#9-multiple-routes-for-the-same-resource)
- [10. Route Constraints](#10-route-constraints)



<br>
<div align = "center">
  <img width="500" alt="image" src="https://github.com/user-attachments/assets/553c84e3-a50a-495b-8718-f381645f3316" />
</div>
<br>





---

<br>

## 1. What is Routing?

**Routing** is the process of matching an incoming HTTP request  to the appropriate **controller action method**.

Or mapping HTTP Request URL with the Resource
   ```
    https://orders.com/url/endpoint   ->  Resource-Order
   ```
   
For example:

```text
GET /api/books/10
```

The routing system examines the URL and HTTP method and determines which action should handle the request.

The collection of routes used by the application is commonly referred to as the **routing table**.


> **Mental Model:**  
> `Request → Routing → Matching Endpoint → Controller Action`



<br>
<div align = "center">
<p>Routing Table</p>
  <img width="500" alt="image" src="https://github.com/user-attachments/assets/d04ed65e-225f-438e-b69e-bee4de75fce3" />
</div>
<br>



<br>

---

## 2. Enable Routing

In ASP.NET Core Web API, routing is enabled in the HTTP request pipeline.

```csharp
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
```

### `UseRouting()`

`UseRouting()` adds routing middleware to the request pipeline.

It is responsible for finding the endpoint that matches the incoming request.

### `MapControllers()`

`MapControllers()` maps controller actions that use **attribute routing**.

So the basic flow is:

```text
HTTP Request
     ↓
UseRouting()
     ↓
Find matching controller/action
     ↓
UseEndpoints()
     ↓
Execute action
```

<br>

---

## 3. Route on Action Method

A route can be defined directly above an action using the `[Route]` attribute.

```csharp
[Route("api/books")]
public string GetBooks()
{
    return "All Books";
}
```

The action can be accessed using:

```text
/api/books
```

### Example

```csharp
using Microsoft.AspNetCore.Mvc;

namespace ConsoleAppone.Controllers
{
    [ApiController]
    public class BooksController : ControllerBase
    {
        [Route("api/books")]
        public string GetBooks()
        {
            return "All Books";
        }
    }
}
```

<br>

---

## 4. Dynamic Route Values

Route values can be made dynamic by placing a parameter inside `{ }`.

### Example

```csharp
[Route("api/books/{id}")]
public string GetBook(int id)
{
    return "Book Id = " + id;
}
```

Now:

```text
/api/books/10
```

matches:

```text
id = 10
```

and returns:

```text
Book Id = 10
```

### Multiple Route Values

Multiple parameters can be used in the same route.

```csharp
[Route("api/books/{id}/author/{authorId}")]
public string GetBookAuthor(int id, int authorId)
{
    return "Book Id = " + id + ", Author Id = " + authorId;
}
```

Example URL:

```text
/api/books/10/author/5
```

Values:

```text
id = 10
authorId = 5
```

<br>

---

## 5. Query String

A **query string** is used after `?` in a URL.

Example:

```text
/api/books?id=10
```

Here:

```text
id = 10
```

is a query-string value.

Multiple query parameters are separated using `&`.

```text
/api/books?authorId=5&year=2025
```

### Route Value vs Query Parameter

**Route value:**

```text
/api/books/10
```

```csharp
[Route("api/books/{id}")]
```

**Query parameter:**

```text
/api/books?id=10
```

The query parameter is not normally written into the route template.

<br>

## Route and Query Parameters 🏷️
```
/api/books/10
```
or
```
/api/books/?id=20
```

### When to use what?



Use **route parameters** when the value identifies the resource being accessed.

```text
/api/books/10
```

Use **query parameters** when the value is used for things such as:

- Filtering
- Searching
- Sorting
- Pagination
- Optional parameters

Example:

```text
/api/books?page=2&pageSize=10
```

<br>

---

## 6. Token Replacement

ASP.NET Core supports tokens such as:

```text
[controller]
[action]
```

These tokens are replaced using the controller and action names.

### Example

```csharp
[Route("api/[controller]/[action]")]
[ApiController]
public class BooksController : ControllerBase
{
    public string GetBooks()
    {
        return "All Books";
    }
}
```

The resulting route becomes:

```text
/api/Books/GetBooks
```

### `[controller]`

For:

```csharp
BooksController
```

`[controller]` becomes:

```text
Books
```

### `[action]`

For:

```csharp
GetBooks()
```

`[action]` becomes:

```text
GetBooks
```

Therefore:

```text
api/[controller]/[action]
```

becomes:

```text
api/Books/GetBooks
```

<br>

---

## 7. Base Route at Controller Level

Instead of repeating the same route prefix on every action, a base route can be placed on the controller.

```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [Route("")]
    public string GetBooks()
    {
        return "All Books";
    }

    [Route("{id}")]
    public string GetBook(int id)
    {
        return "Book Id = " + id;
    }
}
```

The controller-level route acts as the **base route**.

So:

```text
[Route("api/[controller]")]
```

combined with:

```text
[Route("{id}")]
```

produces:

```text
/api/Books/{id}
```

For the first action:

```text
[Route("")]
```

produces:

```text
/api/Books
```

### Why use a controller-level route?

It avoids repeating the same route prefix:

```csharp
[Route("api/books")]
```

on every action.

<br>

---

## 8. Route Override using `~/`

`~/` can be used when you want to define an **absolute route** and avoid combining it with the controller's base route.

Example:

```csharp
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    [Route("~/api/authors")]
    public string GetAuthors()
    {
        return "All Authors";
    }
}
```

The route becomes:

```text
/api/authors
```

instead of:

```text
/api/Books/api/authors
```

> **Important:** `~/` tells ASP.NET Core to treat the route as an absolute route template rather than combining it with the controller-level route.

<br>

---

## 9. Multiple Routes for the Same Resource

An action can have multiple `[Route]` attributes.

```csharp
[Route("api/books")]
[Route("api/library/books")]
public string GetBooks()
{
    return "All Books";
}
```

The same action can now be accessed using either:

```text
/api/books
```

or:

```text
/api/library/books
```

This is useful when an API needs to expose the same resource through more than one URL.

> **Note:** Multiple routes can point to the same action, but different actions should not unintentionally produce the same route because that can create **route ambiguity**.

<br>

---

## 10. Route Constraints

A route constraint restricts which values are considered valid during route matching.


<br>
<div align = "center">
  <p>Route Value Constraints</p>
  <img width="500" alt="image" src="https://github.com/user-attachments/assets/322db3d0-896b-4901-84a0-75f2a714bf95" />
</div>
<br>

For example:

```csharp
[Route("{id:int}")]
```

means that `id` must be an integer.

So:

```text
/api/books/10
```

can match, but:

```text
/api/books/abc
```

will not match that route.

If no other route matches, the result is generally:

```text
404 Not Found
```

### Common Route Constraints

| Constraint | Meaning |
|---|---|
| `int` | Integer |
| `bool` | Boolean |
| `datetime` | Date/time |
| `double` | Double |
| `float` | Float |
| `min(n)` | Minimum numeric value |
| `max(n)` | Maximum numeric value |
| `minlength(n)` | Minimum string length |
| `maxlength(n)` | Maximum string length |
| `length(n)` | Exact string length |
| `range(min,max)` | Numeric range |
| `alpha` | Alphabetic characters |
| `required` | Required value |
| `regex(...)` | Regular-expression constraint |

### Basic Example

```csharp
[Route("{id:int}")]
public string GetById(int id)
{
    return "Hello int " + id;
}
```

Only an integer value can satisfy this route.

<br>

### `min` and `max`

```csharp
[Route("{id:int:min(10):max(100)}")]
```

The route accepts integer values from `10` through `100`.

For example:

```text
/api/books/50
```

matches.

But:

```text
/api/books/5
```

does not satisfy the constraint.

<br>

### `range`

Instead of writing separate `min` and `max` constraints:

```csharp
[Route("{id:int:range(1,100)}")]
```

This accepts integer values in the range:

```text
1 to 100
```

<br>

### String Length Constraints

```csharp
[Route("{id:minlength(3):maxlength(5)}")]
```

The string must contain at least 3 and at most 5 characters.

For exactly 5 characters:

```csharp
[Route("{id:length(5)}")]
```

<br>

### Regex Constraint

A regular expression can also be used.

```csharp
[Route("{id:regex(a(b|c))}")]
```

The route uses the regular expression:

```text
a(b|c)
```

The source example notes that this pattern can accept:

```text
aaabc
```

depending on how the regex matches the route value.

<br>

### Complete Route Constraint Example

```csharp
using Microsoft.AspNetCore.Mvc;

namespace ConsoleAppone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        //[Route("{id:int:min(10):max(100)}")]
        [Route("{id:int:range(1,100)}")]
        public string GetById(int id)
        {
            return "Hello int " + id;
        }

        //[Route("{id:minlength(3):maxlength(5)}")]
        //[Route("{id:length(5)}")] 
        [Route("{id:regex(a(b|c))}")]  // will accespt aaabc 
        public string GetByString(string id)
        {
            return "Hello string " + id;
        }
    }
}
```

### Why use route constraints?

Route constraints help distinguish routes and prevent inappropriate values from matching an action.

For example:

```csharp
[Route("{id:int}")]
```

makes it clear that this route is intended for integer IDs.

> **Important:** Route constraints participate in **route selection**. They are not a replacement for normal application/model validation.

<br>

---

## Key Routing Rules

1. **Routing maps an HTTP request to an endpoint/action.**
2. `UseRouting()` enables routing middleware.
3. `MapControllers()` maps attribute-routed controllers.
4. `[Route]` defines a route template.
5. `{id}` represents a dynamic route value.
6. Query strings are written after `?` and are useful for filtering, searching, sorting, and pagination.
7. `[controller]` and `[action]` are token replacements.
8. A controller-level `[Route]` can provide a common base route.
9. `~/` can override the controller-level route prefix.
10. Multiple `[Route]` attributes can expose one action through multiple URLs.
11. Route constraints restrict which values can match a route.
12. If a constraint fails and no other route matches, the request generally results in `404 Not Found`.
