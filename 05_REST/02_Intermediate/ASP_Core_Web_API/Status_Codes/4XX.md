# Status Code 4XX - 400, 401, 403, 404, 405, 406, 407, 408 & 409

> [!Tip]
> **When asked: What's the difference between all these 4xx codes?**
>
> They all mean **"the client did something the server can't/won't process"** — but each one tells the client *what specifically* was wrong.
>
> | Code | Meaning in one line |
> | ---- | -------------------- |
> | 400 | Request itself is malformed/invalid |
> | 401 | You are not authenticated (no/invalid credentials) |
> | 403 | You are authenticated, but not allowed |
> | 404 | Resource doesn't exist |
> | 405 | Resource exists, but this HTTP method isn't allowed on it |
> | 406 | Server can't return data in the format client asked for |
> | 407 | Like 401, but for a proxy in front of the server |
> | 408 | Client took too long to send the request |
> | 409 | Request conflicts with current state of the resource |

> [!Note]
> **401 vs 403 — the classic mix-up**
>
> * `401 Unauthorized` → "**Who are you?**" (missing/bad credentials — log in)
> * `403 Forbidden` → "**I know who you are, but no.**" (valid credentials, insufficient permission)

<br>

## Index

[Base - Example Code](#example-controller-code)

1. [HTTP 400 — Bad Request](#1-http-400--bad-request)
2. [HTTP 401 — Unauthorized](#2-http-401--unauthorized)
3. [HTTP 403 — Forbidden](#3-http-403--forbidden)
4. [HTTP 404 — Not Found](#4-http-404--not-found)
5. [HTTP 405 — Method Not Allowed](#5-http-405--method-not-allowed)
6. [HTTP 406 — Not Acceptable](#6-http-406--not-acceptable)
7. [HTTP 407 — Proxy Authentication Required](#7-http-407--proxy-authentication-required)
8. [HTTP 408 — Request Timeout](#8-http-408--request-timeout)
9. [HTTP 409 — Conflict](#9-http-409--conflict)
10. [401 vs 403 vs 404 — Decision Flow](#401-vs-403-vs-404--decision-flow)
11. [Final Parameter Cheat Sheet](#final-parameter-cheat-sheet)
12. [Final Memory Trick](#final-memory-trick)


Important
400, 401, 403, 404, 505, 409
<br>

---

<br>

## 1. HTTP 400 — Bad Request

### Purpose

`400 Bad Request` means:

> The server cannot process the request because it is malformed, missing required data, or fails validation.

This is your **catch-all for "the client sent garbage."**

### Return examples

```csharp
return BadRequest();
```

```csharp
return BadRequest(error: "Name is required.");
```

```csharp
return BadRequest(error: ModelState);
```

### Response

```http
400 Bad Request
```

Body (example):

```json
{
  "error": "Name is required."
}
```

### Parameters

| Parameter | Required?  | Meaning                          |
| --------- | ---------- | --------------------------------- |
| `error`   | ❌ Optional | Response body (string, object, or `ModelState`) |

### Mental model

```text
BadRequest()
    ↓
400 Bad Request
    ↓
Optionally return why it's bad
```

<br>

---

<br>

## 2. HTTP 401 — Unauthorized

### Purpose

`401 Unauthorized` means:

> The client is not authenticated — no valid credentials were provided.

> [!Important]
> Despite the name, this is really "**Unauthenticated**." It fires *before* the server even checks what you're allowed to do.

### Return examples

```csharp
return Unauthorized();
```

```csharp
return Unauthorized(value: "Invalid token.");
```

### Response

```http
401 Unauthorized
```

Typically paired with a `WWW-Authenticate` header telling the client how to authenticate (added automatically by ASP.NET's auth middleware, e.g. `[Authorize]`).

### Parameters

| Parameter | Required?  | Meaning        |
| --------- | ---------- | -------------- |
| `value`   | ❌ Optional | Response body  |

### Mental model

```text
Unauthorized()
     ↓
401 Unauthorized
     ↓
"Who are you? Log in first."
```

<br>

---

<br>

## 3. HTTP 403 — Forbidden

### Purpose

`403 Forbidden` means:

> The client is authenticated, but does not have permission to access this resource.

<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/c9969a65-c7a0-4ef8-9c19-e79b749c65c9" />
</div>
<br>

### Return examples

```csharp
return Forbid();
```

```csharp
return Forbid(authenticationSchemes: "Bearer");
```

> [!Note]
> `Forbid()` is designed to work with **cookie/challenge-based auth** (it tells the auth handler to run its "forbidden" logic). If you just want a plain 403 with a custom body, use:
>
> ```csharp
> return StatusCode(403, "You don't have permission to do this.");
> ```

### Response

```http
403 Forbidden
```

### Parameters

| Parameter             | Required?  | Meaning                                  |
| ---------------------- | ---------- | ----------------------------------------- |
| `authenticationSchemes` | ❌ Optional | Which auth scheme(s) should handle the forbid response |

### Mental model

```text
Forbid()
    ↓
403 Forbidden
    ↓
"I know who you are — you still can't."
```

<br>

---

<br>

## 4. HTTP 404 — Not Found

### Purpose

`404 Not Found` means:

> The requested resource does not exist (or the server won't reveal that it does).

### Return examples

```csharp
return NotFound();
```

```csharp
return NotFound(value: animal);
```

```csharp
return NotFound(value: $"Animal with id {id} was not found.");
```

### Your example

```csharp
var animal = animals.FirstOrDefault(x => x.Id == id);

if (animal == null)
    return NotFound();
```

### Response

```http
404 Not Found
```

### Parameters

| Parameter | Required?  | Meaning       |
| --------- | ---------- | ------------- |
| `value`   | ❌ Optional | Response body |

### Mental model

```text
NotFound()
    ↓
404 Not Found
    ↓
Optionally explain what wasn't found
```

<br>

---

<br>

## 5. HTTP 405 — Method Not Allowed

### Purpose

`405 Method Not Allowed` means:

> The resource exists, but the HTTP method used (GET/POST/PUT/DELETE...) isn't supported on it.

> [!Note]
> ASP.NET Core has **no dedicated `MethodNotAllowed()` helper**. In practice you rarely return this manually — it's usually generated automatically by routing when no action matches the verb. If you need it explicitly:

### Return example

```csharp
return StatusCode(405, "GET is not supported on this endpoint.");
```

### Response

```http
405 Method Not Allowed
Allow: GET, POST
```

The `Allow` header (listing supported methods) should ideally be included, though ASP.NET won't add it for you on a manual `StatusCode(405)` call.

### Mental model

```text
StatusCode(405)
      ↓
405 Method Not Allowed
      ↓
"Resource exists, wrong verb"
```

<br>

---

<br>

## 6. HTTP 406 — Not Acceptable

### Purpose

`406 Not Acceptable` means:

> The server cannot produce a response matching the `Accept` header the client sent (e.g. client asked for `application/xml`, server only has `application/json`).

### Return example

```csharp
return StatusCode(406, "Requested format not supported.");
```

> [!Tip]
> This is usually handled automatically for you by ASP.NET's content negotiation when you enable:
>
> ```csharp
> builder.Services.AddControllers(options =>
> {
>     options.ReturnHttpNotAcceptable = true;
> });
> ```
>
> With that flag on, ASP.NET returns `406` itself if it can't satisfy the `Accept` header — you don't need to return it manually.

### Mental model

```text
Accept: application/xml
       ↓
Server only supports JSON
       ↓
406 Not Acceptable
```

<br>

---

<br>

## 7. HTTP 407 — Proxy Authentication Required

### Purpose

`407 Proxy Authentication Required` means:

> Same idea as `401`, but the client must authenticate with a **proxy** sitting in front of the server, not the server itself.

### Return example

```csharp
return StatusCode(407, "Proxy authentication required.");
```

> [!Note]
> You will almost never return this from a normal ASP.NET Web API controller — it's raised by the proxy/gateway layer, not your application code. Included here for completeness of the 4xx range.

### Mental model

```text
StatusCode(407)
      ↓
407 Proxy Authentication Required
      ↓
"Authenticate with the proxy first"
```

<br>

---

<br>

## 8. HTTP 408 — Request Timeout

### Purpose

`408 Request Timeout` means:

> The client took too long to send the complete request, so the server gave up waiting.

### Return example

```csharp
return StatusCode(408, "Request timed out.");
```

> [!Note]
> Like `405`/`406`, there's no dedicated helper method. This status is more commonly generated by the web server / load balancer (Kestrel, IIS, nginx) than by your controller code.

### Mental model

```text
StatusCode(408)
      ↓
408 Request Timeout
      ↓
"You were too slow"
```

<br>

---

<br>

## 9. HTTP 409 — Conflict

### Purpose

`409 Conflict` means:

> The request conflicts with the current state of the resource (e.g. duplicate unique key, version mismatch, concurrent edit).

### Return examples

```csharp
return Conflict();
```

```csharp
return Conflict(error: "An animal with this name already exists.");
```

### Example usage

```csharp
[HttpPost("")]
public IActionResult CreateAnimal(Animal animal)
{
    var exists = animals.Any(x => x.Name == animal.Name);

    if (exists)
        return Conflict($"An animal named '{animal.Name}' already exists.");

    animal.Id = animals.Count + 1;
    animals.Add(animal);

    return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
}
```

### Response

```http
409 Conflict
```

### Parameters

| Parameter | Required?  | Meaning       |
| --------- | ---------- | ------------- |
| `error`   | ❌ Optional | Response body |

### Mental model

```text
Conflict()
    ↓
409 Conflict
    ↓
"Request clashes with current state"
```

<br>

---

<br>

## 401 vs 403 vs 404 — Decision Flow

```text
Is the client authenticated?
        │
        ├── No  → 401 Unauthorized
        │
        └── Yes
             │
             Is the client allowed to access this resource?
                  │
                  ├── No  → 403 Forbidden
                  │
                  └── Yes
                       │
                       Does the resource exist?
                            │
                            ├── No  → 404 Not Found
                            │
                            └── Yes → proceed (200/etc.)
```

> [!Tip]
> Some APIs deliberately return `404` instead of `403` for resources a user isn't allowed to see — to avoid *confirming the resource even exists*. That's a security/design choice, not a strict rule.

<br>

---

<br>

## Final Parameter Cheat Sheet

| Method               | Required parameters | Optional parameters | Dedicated helper? |
| --------------------- | -------------------- | --------------------- | ------------------ |
| `BadRequest()`        | None                 | `error`                | ✅ Yes |
| `Unauthorized()`      | None                 | `value`                 | ✅ Yes |
| `Forbid()`            | None                 | `authenticationSchemes` | ✅ Yes |
| `NotFound()`          | None                 | `value`                 | ✅ Yes |
| `StatusCode(405, …)`  | `statusCode`          | `value`                 | ❌ No — use `StatusCode()` |
| `StatusCode(406, …)`  | `statusCode`          | `value`                 | ❌ No — or enable `ReturnHttpNotAcceptable` |
| `StatusCode(407, …)`  | `statusCode`          | `value`                 | ❌ No |
| `StatusCode(408, …)`  | `statusCode`          | `value`                 | ❌ No |
| `Conflict()`          | None                 | `error`                 | ✅ Yes |

<br>

---

<br>

## Final Memory Trick

```text
400 → BadRequest
      ↓
      "Your request is malformed"

401 → Unauthorized
      ↓
      "Who are you?" (no/bad credentials)

403 → Forbid
      ↓
      "I know you — still no" (bad permissions)

404 → NotFound
      ↓
      "That doesn't exist"

405 → StatusCode(405)
      ↓
      "Wrong HTTP verb for this resource"

406 → StatusCode(406) / ReturnHttpNotAcceptable
      ↓
      "Can't match your Accept header"

407 → StatusCode(407)
      ↓
      "Authenticate with the proxy"

408 → StatusCode(408)
      ↓
      "You took too long"

409 → Conflict
      ↓
      "Clashes with the current state"
```

<br>

---

<br>

# Example Controller Code

```csharp
using ConsoleAppone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAppone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private static List<Animal> animals = new()
        {
            new Animal { Id = 1, Name = "Dog" },
            new Animal { Id = 2, Name = "Cat" },
            new Animal { Id = 3, Name = "Lion" }
        };


        // =====================================================
        // 1. 400 Bad Request
        // =====================================================

        [HttpPost("")]
        public IActionResult CreateAnimal(Animal animal)
        {
            if (string.IsNullOrWhiteSpace(animal.Name))
                return BadRequest("Name is required.");

            var exists = animals.Any(x => x.Name == animal.Name);

            // =====================================================
            // 9. 409 Conflict
            // =====================================================
            if (exists)
                return Conflict($"An animal named '{animal.Name}' already exists.");

            animal.Id = animals.Count + 1;
            animals.Add(animal);

            return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
        }


        // =====================================================
        // 2. 401 Unauthorized (manual check, illustrative)
        // =====================================================

        [HttpGet("secure")]
        public IActionResult GetSecureAnimals(bool isAuthenticated)
        {
            if (!isAuthenticated)
                return Unauthorized("You must log in to view this resource.");

            return Ok(animals);
        }


        // =====================================================
        // 3. 403 Forbidden (manual check, illustrative)
        // =====================================================

        [HttpDelete("{id}/admin-only")]
        public IActionResult DeleteAnimalAdminOnly(int id, bool isAdmin)
        {
            if (!isAdmin)
                return StatusCode(403, "Only admins can delete animals.");

            var animal = animals.FirstOrDefault(x => x.Id == id);

            if (animal == null)
                return NotFound();

            animals.Remove(animal);
            return NoContent();
        }


        // =====================================================
        // 4. 404 Not Found
        // =====================================================

        [HttpGet("{id}", Name = "GetAnimalById")]
        public IActionResult GetAnimal(int id)
        {
            var animal = animals.FirstOrDefault(x => x.Id == id);

            if (animal == null)
                return NotFound($"Animal with id {id} was not found.");

            return Ok(animal);
        }


        /*
         =====================================================
         5-8. 405 / 406 / 407 / 408
         =====================================================
         These are rarely returned manually inside a controller
         action — they're generated by routing, content
         negotiation middleware, or the proxy/server layer.
         Shown here only for reference:
        */

        [HttpGet("unsupported-format")]
        public IActionResult TestNotAcceptable()
        {
            return StatusCode(406, "Requested format not supported.");
        }
    }
}
```

Model

```csharp
namespace ConsoleAppone.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
```
