

# Add a New Controller in ASP.NET Core


## Index

* [1. What is a Controller?](#1-what-is-a-controller)
* [2. Controller Naming Convention](#2-controller-naming-convention)
* [3. `ControllerBase`](#3-controllerbase)
* [4. `Controller` vs `ControllerBase`](#4-controller-vs-controllerbase)
* [5. `[ApiController]` Attribute](#5-apicontroller-attribute)
* [6. Creating the `Controllers` Folder](#6-creating-the-controllers-folder)
* [7. Creating a Controller](#7-creating-a-controller)
* [8. Controller Routing](#8-controller-routing)
* [9. Action Methods](#9-action-methods)
* [10. Avoiding Route Ambiguity](#10-avoiding-route-ambiguity)
* [11. Resource-Oriented Thinking](#11-resource-oriented-thinking-%EF%B8%8F) 🏷️
* [12. `UseDeveloperExceptionPage`](#12-usedeveloperexceptionpage)
* [13. Complete Controller Example](#13-complete-controller-example)



<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/8f62a432-f25d-459c-bc10-430a1d68eb6f" />
</div>
<br>

---

<br>

## 1. What is a Controller?

A **controller handles HTTP requests and returns HTTP responses**.

```text
Client
   ↓
HTTP Request
   ↓
Controller
   ↓
Action Method
   ↓
HTTP Response
   ↓
Client
```

Example:

```text
GET /employees
        ↓
EmployeeController
        ↓
GetEmployees()
        ↓
200 OK
```

### In simple words

> **Controller = entry point for handling API requests.**

<br>

---

<br>


## 2. Controller Naming Convention

Web API controllers should end with:

```text
Controller
```

### Examples

```text
EmployeeController
MovieController
ActorController
ProductController
```

Example:

```csharp
public class EmployeeController
{
}
```

The important convention is:

```text
<ResourceName>Controller
```

<br>

---

<br>


## 3. `ControllerBase`

Web API controllers should inherit from:

```csharp
ControllerBase
```

Example:

```csharp
public class EmployeeController : ControllerBase
{
}
```

`ControllerBase` provides functionality needed by Web APIs, such as methods for creating HTTP responses.

Examples:

```csharp
return Ok();
```

```csharp
return Created(...);
```

```csharp
return BadRequest();
```

```csharp
return NotFound();
```

So:

```text
ControllerBase
      ↓
API-related controller functionality
      ↓
HTTP responses
```

<br>

---

<br>


## 4. `Controller` vs `ControllerBase`



<br>
<img width="500"  alt="image" src="https://github.com/user-attachments/assets/eb1b69d6-e32b-4010-9eca-af26ff333384" />
<br>


There are two commonly encountered base classes:

| Class            | Used for                    |
| ---------------- | --------------------------- |
| `ControllerBase` | Web API                     |
| `Controller`     | MVC applications with Views |

`Controller` inherits from `ControllerBase` and adds MVC/View-related functionality.



```
┌──────────┐     HTTP      ┌──────────────┐     ┌──────────────┐     ┌──────────────┐     ┌────────────┐
│  CLIENT  │ ────────────> │  CONTROLLER  │ ──> │   SERVICE    │ ──> │  REPOSITORY  │ ──> │  DATABASE  │
└──────────┘               └──────────────┘     └──────────────┘     └──────────────┘     └────────────┘
     ↑                            │
     │                            │
     │                            ↓
     └────────────────────── JSON RESPONSE

Controller Does not directly interact with Model, its just a simplification used,
It gets response through Service and repo layers 

```

> MVC is an architectural pattern. A Web API can use the MVC framework without actually having the “View” part of MVC.

<br>
<img width="500" alt="image" src="https://github.com/user-attachments/assets/2a8b3aa3-e013-40df-8f64-4a161285c058" />
<br>

A Web API normally doesn't need Views because it returns data such as JSON.

### Remember

```text
Web API → ControllerBase
MVC     → Controller
```

<br>

---

<br>


## 5. `[ApiController]` Attribute

<br>
<img width="600" alt="image" src="https://github.com/user-attachments/assets/185a7208-2dd2-4af2-873b-4e18d002fefa" />
<br>

A Web API controller should use:

```csharp
[ApiController]
```

Example:

```csharp
[ApiController]
public class EmployeeController : ControllerBase
{
}
```

### What does `[ApiController]` provide?

It enables Web API-specific behaviors such as:

* Attribute routing
* Automatic `400 Bad Request` responses for model validation errors
* Automatic parameter binding from sources such as:
  * URL
  * Request body
  * Headers

### Mental model

```text
[ApiController]
       ↓
"Treat this class as a Web API controller"
```

<br>

---

<br>


## 6. Creating the `Controllers` Folder

Create a `Controllers` folder at the root of the application.

Example:

```text
MyWebApi
│
├── Controllers
│   └── EmployeeController.cs
│
├── Program.cs
├── Startup.cs
├── appsettings.json
└── MyWebApi.csproj
```

The `Controllers` folder is the conventional place for controller classes.

<br>

---

<br>


## 7. Creating a Controller

Create a class such as:

```text
TestController.cs
```

Then:

```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class TestController : ControllerBase
{
}
```

There are now three important pieces:

```csharp
[ApiController]
public class TestController : ControllerBase
```

### Meaning

```text
[ApiController]
      ↓
Web API behavior

TestController
      ↓
Controller naming convention

ControllerBase
      ↓
Base functionality for Web APIs
```

<br>

---

<br>


# 8. Controller Routing

A controller needs a route that maps a URL to it.

Use the `[Route]` attribute:

```csharp
[Route("test")]
```

Example:

```csharp
[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
}
```

Now the controller is associated with:

```text
/test
```

So the basic relationship is:

```text
/test
  ↓
TestController
```

<br>

---

<br>


## 9. Action Methods

A controller can contain multiple **action methods**.

An action method is a method that handles an HTTP request.

Example:

```csharp
[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
```

Here:

```text
GET /test
     ↓
TestController
     ↓
Get()
     ↓
200 OK
```

### Multiple actions

You can have multiple actions:

```csharp
[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }

    [HttpGet("employee")]
    public IActionResult GetEmployee()
    {
        return Ok();
    }
}
```

Now:

```text
GET /test
GET /test/employee
```

are different endpoints.

<br>

---

<br>


# 10. Avoiding Route Ambiguity

Suppose multiple actions have the same route and HTTP method:

```csharp
[HttpGet]
public IActionResult Get()
{
    return Ok();
}

[HttpGet]
public IActionResult GetEmployee()
{
    return Ok();
}
```

Both potentially represent:

```text
GET /test
```

ASP.NET Core cannot determine which action should handle the request.

This can result in an **ambiguous endpoint/action match**.

### Solution

Give actions unique routes:

```csharp
[HttpGet]
public IActionResult Get()
{
    return Ok();
}

[HttpGet("employee")]
public IActionResult GetEmployee()
{
    return Ok();
}
```

Now:

```text
GET /test
GET /test/employee
```

### Important

> **Multiple actions are allowed in a controller, but their routing must be unambiguous.**

<br>

---

<br>


# 11. Resource-Oriented Thinking 🏷️

When designing Web APIs, think in terms of **resources**.

For example, suppose your application manages employees.

The resource is:

```text
Employee
```

So you could have:

```text
GET    /employees
GET    /employees/10
POST   /employees
PUT    /employees/10
DELETE /employees/10
```

These operations belong naturally to:

```text
EmployeeController
```

### Don't think primarily:

```text
"I need a GetEmployees() method."
```

Think:

```text
"What employee resource does my API expose?"
```

Then decide the appropriate HTTP method and endpoint.

This resource-oriented thinking becomes important when designing **RESTful APIs**.

<br>

---

<br>


# 12. `UseDeveloperExceptionPage`

During development, you can enable:

```csharp
app.UseDeveloperExceptionPage();
```

Usually:

```csharp
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
```

### Purpose

It provides detailed error information while developing/debugging the application.

It is **middleware**, so it is separate from controller routing.

<br>

---

<br>


# 13. Complete Controller Example

A simple ASP.NET Core 5 Web API controller:

```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("employees")]
public class EmployeeController : ControllerBase
{
    [HttpGet]
    public IActionResult GetEmployees()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployee(int id)
    {
        return Ok();
    }
}
```

The endpoints are:

```text
GET /employees
GET /employees/10
```

### How ASP.NET Core processes it

```text
GET /employees/10
       ↓
Routing
       ↓
EmployeeController
       ↓
GetEmployee(int id)
       ↓
return Ok()
       ↓
HTTP 200
```

<br>

---

<br>


# Overview

| Concept                       | Meaning                               |
| ----------------------------- | ------------------------------------- |
| Controller                    | Handles API requests                  |
| `Controller`                  | MVC controller with View support      |
| `ControllerBase`              | Base class for Web API controllers    |
| `[ApiController]`             | Enables Web API-specific behavior     |
| `[Route]`                     | Defines URL routing                   |
| Action                        | Method that handles a request         |
| `Controllers` folder          | Conventional location for controllers |
| Resource                      | Entity exposed by the API             |
| `UseDeveloperExceptionPage()` | Detailed development-time errors      |

### Controller formula

```text
[ApiController]
       +
[Route(...)]
       +
ControllerBase
       +
Action Methods
       ↓
Web API Controller
```

### One-line mental model

> **A controller groups actions for a resource, `[Route]` maps URLs to it, and `[ApiController]` enables Web API behavior.**
