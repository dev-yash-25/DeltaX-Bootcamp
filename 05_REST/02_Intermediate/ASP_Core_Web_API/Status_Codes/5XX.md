# Status Code 5XX - 500, 502 & 503

> [!Tip]
> **When asked: What's the core difference between 4xx and 5xx?**
>
> * **4xx** → **Client's fault.** Bad request, missing auth, wrong resource.
> * **5xx** → **Server's fault.** The client did everything right — the *server* broke, crashed, or couldn't cope.

> [!Note]
> **500 vs 502 vs 503 — quick distinction**
>
> * `500 Internal Server Error` → **Your own app** threw an unhandled exception.
> * `502 Bad Gateway` → **Your server (acting as a gateway/proxy) got an invalid response from an upstream server** it depends on.
> * `503 Service Unavailable` → **Your server is up, but temporarily can't handle the request** (overloaded, down for maintenance, restarting).

<br>

## Index

[Base - Example Code](#example-controller-code)

1. [HTTP 500 — Internal Server Error](#1-http-500--internal-server-error)
2. [HTTP 502 — Bad Gateway](#2-http-502--bad-gateway)
3. [HTTP 503 — Service Unavailable](#3-http-503--service-unavailable)
4. [500 vs 502 vs 503 — Decision Flow](#500-vs-502-vs-503--decision-flow)
5. [Final Parameter Cheat Sheet](#final-parameter-cheat-sheet)
6. [Final Memory Trick](#final-memory-trick)

<br>

---

<br>

## 1. HTTP 500 — Internal Server Error

### Purpose

`500 Internal Server Error` means:

> Something went wrong inside the server while processing the request, and no more specific error applies.

> [!Important]
> You almost never return `500` manually on purpose — it's the **default/fallback** for unhandled exceptions. ASP.NET returns this automatically if your action throws and nothing catches it.

### Return examples

```csharp
return StatusCode(500);
```

```csharp
return StatusCode(500, "Something went wrong on our end.");
```

### Letting ASP.NET generate it for you (the normal case)

```csharp
[HttpGet("{id}")]
public IActionResult GetAnimal(int id)
{
    var animal = animals.First(x => x.Id == id); // throws if not found

    return Ok(animal);
    // Unhandled InvalidOperationException here
    // → ASP.NET automatically returns 500
}
```

### Response

```http
500 Internal Server Error
```

### Parameters

| Parameter | Required?  | Meaning       |
| --------- | ---------- | ------------- |
| `value`   | ❌ Optional | Response body |

### Mental model

```text
Unhandled exception
       ↓
500 Internal Server Error
       ↓
"Server broke, not your fault"
```

<br>

---

<br>

## 2. HTTP 502 — Bad Gateway

### Purpose

`502 Bad Gateway` means:

> The server, while acting as a gateway or proxy, received an invalid or no response from an upstream server it needed to talk to.

For example: your API calls a downstream microservice or third-party API, and that call fails or returns garbage.

### Return example

```csharp
return StatusCode(502, "Upstream service returned an invalid response.");
```

### Example usage

```csharp
[HttpGet("weather")]
public async Task<IActionResult> GetWeather()
{
    HttpResponseMessage response;

    try
    {
        response = await _httpClient.GetAsync("https://upstream-weather-api/current");
    }
    catch (HttpRequestException)
    {
        return StatusCode(502, "Could not reach the weather service.");
    }

    if (!response.IsSuccessStatusCode)
        return StatusCode(502, "Weather service returned an unexpected response.");

    var data = await response.Content.ReadAsStringAsync();
    return Ok(data);
}
```

> [!Note]
> `502` is mainly meaningful when **your app is itself a client to another server** (reverse proxy, gateway, API calling another API). If your app has no upstream dependency, you'll rarely produce this yourself — it's more commonly seen coming from nginx/IIS/load balancers in front of a crashed app.

### Response

```http
502 Bad Gateway
```

### Mental model

```text
Your server → calls Upstream server
                    ↓
             Upstream fails / invalid response
                    ↓
             502 Bad Gateway
```

<br>

---

<br>

## 3. HTTP 503 — Service Unavailable

### Purpose

`503 Service Unavailable` means:

> The server is temporarily unable to handle the request (overloaded, under maintenance, restarting) — but should be able to later.

### Return examples

```csharp
return StatusCode(503);
```

```csharp
return StatusCode(503, "Service is under maintenance. Please try again later.");
```

### With a `Retry-After` header (best practice)

```csharp
[HttpGet("")]
public IActionResult GetAnimals()
{
    if (_maintenanceMode)
    {
        Response.Headers.Append("Retry-After", "120"); // seconds
        return StatusCode(503, "Service temporarily unavailable — try again in 2 minutes.");
    }

    return Ok(animals);
}
```

> [!Tip]
> `Retry-After` tells the client *how long to wait* before retrying — either seconds (`120`) or an HTTP date. It's optional but makes `503` far more useful to API consumers.

### Response

```http
503 Service Unavailable
Retry-After: 120
```

### Parameters

| Parameter | Required?  | Meaning       |
| --------- | ---------- | ------------- |
| `value`   | ❌ Optional | Response body |

### Mental model

```text
StatusCode(503)
      ↓
503 Service Unavailable
      ↓
"I'm up, but can't handle this right now — retry later"
```

<br>

---

<br>

## 500 vs 502 vs 503 — Decision Flow

```text
Did YOUR code throw an unhandled exception?
        │
        ├── Yes → 500 Internal Server Error
        │
        └── No
             │
             Were you waiting on an upstream/dependency
             server that failed or returned garbage?
                  │
                  ├── Yes → 502 Bad Gateway
                  │
                  └── No
                       │
                       Is your server just temporarily
                       overloaded / down for maintenance?
                            │
                            └── Yes → 503 Service Unavailable
```

<br>

---

<br>

## Final Parameter Cheat Sheet

| Method              | Required parameters | Optional parameters | Returned how                              |
| -------------------- | -------------------- | --------------------- | ------------------------------------------ |
| `StatusCode(500,…)`  | `statusCode`          | `value`                 | Usually automatic (unhandled exception)     |
| `StatusCode(502,…)`  | `statusCode`          | `value`                 | Manual — upstream/dependency call failed    |
| `StatusCode(503,…)`  | `statusCode`          | `value`                 | Manual — add `Retry-After` header if known  |

<br>

---

<br>

## Final Memory Trick

```text
500 → Internal Server Error
      ↓
      "MY code crashed" (unhandled exception, usually automatic)

502 → Bad Gateway
      ↓
      "The OTHER server I called broke"

503 → Service Unavailable
      ↓
      "I'm fine, just busy/down right now — retry later"
```

<br>

---

<br>

# Example Controller Code

```csharp
using ConsoleAppone.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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

        private static bool _maintenanceMode = false;
        private readonly HttpClient _httpClient = new();


        // =====================================================
        // 1. 500 Internal Server Error
        // =====================================================

        [HttpGet("crash-demo/{id}")]
        public IActionResult CrashDemo(int id)
        {
            try
            {
                // .First() throws InvalidOperationException if no match,
                // instead of returning null like FirstOrDefault()
                var animal = animals.First(x => x.Id == id);
                return Ok(animal);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unexpected error while fetching the animal.");
            }
        }

        /*
         Without the try/catch, an unhandled exception here
         would still result in ASP.NET returning 500 by default —
         this version just lets you control the response body.
         */


        // =====================================================
        // 2. 502 Bad Gateway
        // =====================================================

        [HttpGet("upstream-demo")]
        public async Task<IActionResult> UpstreamDemo()
        {
            HttpResponseMessage response;

            try
            {
                response = await _httpClient.GetAsync("https://some-upstream-service/api/data");
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, "Could not reach the upstream service.");
            }

            if (!response.IsSuccessStatusCode)
                return StatusCode(502, "Upstream service returned an invalid response.");

            var data = await response.Content.ReadAsStringAsync();
            return Ok(data);
        }


        // =====================================================
        // 3. 503 Service Unavailable
        // =====================================================

        [HttpGet("")]
        public IActionResult GetAnimals()
        {
            if (_maintenanceMode)
            {
                Response.Headers.Append("Retry-After", "120");
                return StatusCode(503, "Service temporarily unavailable — try again in 2 minutes.");
            }

            return Ok(animals);
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
