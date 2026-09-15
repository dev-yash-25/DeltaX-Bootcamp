# Dependency Injection (DI)

## Index

- [1. What is Dependency Injection?](#1-what-is-dependency-injection)
- [2. Web Application Architecture](#2-web-application-architecture)
- [3. The Problem: Tight Coupling](#3-the-problem-tight-coupling)
- [4. Dependency Injection as the Solution](#4-dependency-injection-as-the-solution)
- [5. Configuring DI in ASP.NET Core](#5-configuring-di-in-aspnet-core)
- [6. Service Lifetimes](#6-service-lifetimes)
  - [6.1 Singleton](#61-singleton)
  - [6.2 Scoped](#62-scoped)
  - [6.3 Transient](#63-transient)
- [7. Constructor, Method and Property Injection](#7-constructor-method-and-property-injection)
- [8. Singleton — `AddSingleton()`](#8-singleton--addsingleton)
- [9. Scoped — `AddScoped()`](#9-scoped--addscoped)
- [10. Transient — `AddTransient()`](#10-transient--addtransient)
- [11. `TryAddSingleton()`, `TryAddScoped()` and `TryAddTransient()`](#11-tryaddsingleton-tryaddscoped-and-tryaddtransient)
- [12. Resolve a Service Directly in an Action Method](#12-resolve-a-service-directly-in-an-action-method)
- [13. DI Mental Model](#13-di-mental-model)

<br>

---

<br>


## 1. What is Dependency Injection?

**Dependency Injection (DI)** is a design pattern used in ASP.NET Core to provide a class with the dependencies it needs instead of having the class create those dependencies itself.

> Dependency Injection (DI) in C# is a design pattern where a class receives its required dependencies from an external source rather than creating them itself using the new keyword

DI promotes:
- **Loose coupling**
- **Inversion of Control (IoC)**
- Easier maintenance
- Easier unit testing
- Easy replacement of implementations

### Basic Idea

Without DI:

```text
Controller
   |
   └── new ProductRepository()
```

The controller creates and directly depends on a concrete implementation.

With DI:

```text
Controller
   |
   └── IProductRepository
             ↑
       DI Container
             ↑
    ProductRepository
```

The controller depends on the **interface**, while ASP.NET Core provides the actual implementation.

> [!Important]
> Instead of a class creating its dependency using `new`, the framework provides the dependency.

<br>

<div align="center">
  <img width="600" alt="Dependency Injection architecture" src="https://github.com/user-attachments/assets/595c23cb-8191-49b7-b92f-5db05a99c798" />
</div>

<br>



---

<br>



## 2. Web Application Architecture

A typical web application can be divided into layers.

### 2.1 Controllers

Controllers:

- Contain public methods / action methods.
- Handle requests from clients.
- Expose API endpoints.

```text
Client
  ↓
Controller
```

### 2.2 Repository Layer

The repository layer:

- Manages application/data-access logic.
- Coordinates database interactions.
- Provides a separation between controllers and data access.

```text
Controller
    ↓
Repository
    ↓
Database
```

### 2.3 Services

Services represent reusable/global application components such as:

- Logging
- Email
- Other application-level functionality


<br>

---

<br>



## 3. The Problem: Tight Coupling

A traditional approach manually creates dependencies using the `new` keyword.

For example:

```csharp
var repository = new ProductRepository();
```

This creates **tight coupling** between the consumer and the concrete implementation.

### Problems with Tight Coupling

#### 3.1 Difficult to Maintain

Suppose the application initially uses:

```text
ProductRepository
```

and later needs to replace it with another implementation.

If every controller creates the repository manually:

```csharp
new ProductRepository();
```

you have to find and change every place where it was created.

The same problem occurs with services such as email:

```text
LocalEmailService
        ↓
ThirdPartyEmailService
```

Changing the implementation becomes difficult when controllers directly instantiate the concrete class.

#### 3.2 Difficult to Unit Test

When a controller directly uses:

```csharp
new ProductRepository();
```

it is difficult to replace that repository with a **mock** during unit testing.

DI solves this by allowing the controller to depend on an abstraction such as:

```csharp
IProductRepository
```

and then injecting a real or mock implementation.

> [!Important]
> **Tight coupling → difficult maintenance + difficult testing**


<br>

---

<br>


## 4. Dependency Injection as the Solution

DI promotes **Inversion of Control (IoC)** and loose coupling.

Instead of:

```text
Class creates dependency
```

DI uses:

```text
Framework / DI Container
          ↓
     creates dependency
          ↓
      injects it
          ↓
       Class
```

### 4.1 Typical DI Flow

#### Step 1 — Create an Interface

```csharp
public interface IProductRepository
{
    // methods
}
```

#### Step 2 — Implement the Interface

```csharp
public class ProductRepository : IProductRepository
{
    // implementation
}
```

#### Step 3 — Request the Interface in the Controller

Instead of:

```csharp
var repository = new ProductRepository();
```

the controller asks for:

```csharp
IProductRepository
```

through its constructor.

#### Step 4 — Register the Mapping

Tell ASP.NET Core:

```text
IProductRepository → ProductRepository
```

The DI container can then create and provide the required object.

### 4.2 Why the Controller Becomes Loosely Coupled

The controller knows:

```text
IProductRepository
```

but does not need to know:

```text
How ProductRepository is implemented
```

Therefore, the implementation can be replaced without changing the controller's dependency declaration.


<br>

---

<br>



## 5. Configuring DI in ASP.NET Core

ASP.NET Core has a **built-in Dependency Injection container**.

The service provider is represented by:

```text
IServiceProvider
```

The DI container:

1. Stores service registrations.
2. Creates service instances.
3. Resolves requested dependencies.
4. Injects them into controllers/services/actions.

### 5.1 Registering Services

In ASP.NET Core 5.0, services are commonly registered in:

```text
Startup.cs
    ↓
ConfigureServices()
```

Example:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    services.AddSingleton<IProductRepository, ProductRepository>();
}
```

The registration tells the container:

```text
When IProductRepository is requested
              ↓
provide ProductRepository
```


<br>

---

<br>



## 6. Service Lifetimes

ASP.NET Core provides three main DI service lifetimes:

1. **Singleton**
2. **Scoped**
3. **Transient**

They determine **how long a service object lives** and **when a new instance is created**.

| Lifetime | Object created | Easy analogy |
|---|---|---|
| **Singleton** | Once for the entire application | 🏢 One office for everyone |
| **Scoped** | Once per HTTP request | 🛒 One shopping cart per customer visit |
| **Transient** | Every time it is requested | 🥤 New disposable cup every time |

### Quick Mental Model

```text
Singleton
Application
└── One instance
    ├── Request 1
    ├── Request 2
    └── Request 3


Scoped
Request 1
└── One instance

Request 2
└── New instance


Transient
Request 1
├── Instance A
├── Instance B
└── Instance C
```

> [!Important]
> The major difference between the three lifetimes is **when a new object is created**.


<br>

---

<br>



## 6.1 Singleton

A **Singleton** service has one instance for the entire application lifetime.

```text
Application starts
       ↓
ProductRepository instance created
       ↓
Same instance reused
       ↓
Request 1 ─┐
Request 2 ─┼── Same instance
Request 3 ─┘
```

Registration:

```csharp
services.AddSingleton<IProductRepository, ProductRepository>();
```

### Key Behavior

- One instance for the application.
- Same instance is reused across requests.
- State stored inside the service can persist across requests.
- The instance is lost when the application stops/restarts.


<br>

---

<br>



## 6.2 Scoped

A **Scoped** service creates one instance for each HTTP request.

```text
Request 1
└── ProductRepository instance A

Request 2
└── ProductRepository instance B

Request 3
└── ProductRepository instance C
```

However, if the same scoped service is requested multiple times **within the same request**, the same instance is used.

```text
Request 1
├── Controller → Instance A
├── Service    → Instance A
└── Other      → Instance A
```

Registration:

```csharp
services.AddScoped<IProductRepository, ProductRepository>();
```

> [!Important]
> **Scoped = one instance per HTTP request**, not one instance per application.


<br>

---

<br>



## 6.3 Transient

A **Transient** service creates a **new instance every time the service is requested**.

```text
Request
├── Request of service → Instance A
├── Request of service → Instance B
└── Request of service → Instance C
```

Registration:

```csharp
services.AddTransient<IProductRepository, ProductRepository>();
```

### Key Behavior

- New instance every time it is requested.
- Instances are not shared.
- State is therefore not shared between separate transient instances.

Example:

```text
Controller A
├── Uses service → Instance A
├── Uses service → Instance B
└── Uses service → Instance C
```

If data is added to Instance A, Instance B does not automatically have that data.


<br>

---

<br>


## 7. Constructor, Method and Property Injection

A dependency can be injected through different mechanisms.

> [!Note]
> Dependency can be created using **Constructor, Method or Property** injection.

### 7.1 Constructor Injection

The dependency is provided through the constructor.

```csharp
public ProductController(IProductRepository productRepository)
{
    _productRepository = productRepository;
}
```

This is the common approach for dependencies required throughout a controller/class.

### 7.2 Method / Action Injection

A dependency can be requested only by a specific method.

```csharp
public IActionResult GetName(
    [FromServices] IProductRepository productRepository)
{
    var name = productRepository.GetName();

    return Ok(name);
}
```

This is useful when the dependency is needed only by one action.

### 7.3 Property Injection

The source notes mention property injection as another way of providing dependencies.

<br>

---

<br>



## 8. Singleton — `AddSingleton()`

### 8.1 Registering a Singleton

```csharp
services.AddSingleton<IProductRepository, ProductRepository>();
```

General form:

```csharp
//               Generally Interface,   Repository 
services.AddSingleton<Type of Service, Implementation>();
```

This tells the ASP.NET Core DI container to create one `ProductRepository` instance and reuse it for the application's lifetime.

### 8.2 Complete Registration Example

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddTransient<CustomMiddleware>();

    services.AddSingleton<IProductRepository, ProductRepository>();
}
```

### 8.3 Verifying Singleton Behavior

The tutorial demonstrates injecting the same interface twice:

```csharp
using ConsoleAppone.Models;
using ConsoleAppone.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleAppone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // Created 2 fields for holding a single instance
        private readonly IProductRepository _productRepository;
        private readonly IProductRepository _productRepository1; 

        // Fields acting on a single instance (SingleTon)
        public ProductController(IProductRepository productRepository, IProductRepository productRepository1)
        {
            _productRepository = productRepository;
            _productRepository1 = productRepository1;
        }

        [HttpPost("")]
        public IActionResult AddProduct([FromBody] Product product)
        {
            _productRepository.AddProduct(product);  //Add using 1st instance
            var products = _productRepository1.GetAllProducts(); // get using second instance
            // Works because both share single repo instance

            return Ok(products);
        }
    }
}
```

Because the service is registered as Singleton:

```text
_productRepository
       │
       ├──── same object ────┐
       │                     │
_productRepository1          │
       │                     │
       └─────────────────────┘
```

So data added through the first reference can be retrieved through the second reference.

### 8.4 State Persistence

If a Singleton repository contains an in-memory list:

```text
Request 1
└── Add Product
       ↓
Singleton list updated

Request 2
└── Get Products
       ↓
Previously added product still exists
```

This happens because the same service instance remains alive.

### 8.5 Application Restart

Singleton state is **not permanent storage**.

```text
Application running
└── Singleton instance + state

Application stopped/restarted
└── Old instance lost
└── New instance created
└── Initial state restored
```

> [!Important]
> Singleton lifetime means one instance for the **application lifetime**. It does not mean the object's state survives an application restart.


<br>

---

<br>



## 9. Scoped — `AddScoped()`

Register a scoped service using:

```csharp
services.AddScoped<IProductRepository, ProductRepository>();
```

### Behavior

```text
HTTP Request 1
└── Repository instance A
    ├── Controller
    └── Other dependencies

HTTP Request 2
└── Repository instance B
    ├── Controller
    └── Other dependencies
```

The instance is shared **inside the request**, but not between separate requests.

### Example

```text
Request 1:
Controller → IProductRepository → Instance A
                       ↓
                  same Instance A

Request 2:
Controller → IProductRepository → Instance B
                       ↓
                  different instance
```

> [!Important]
> If a scoped service is requested multiple times during the same HTTP request, the same scoped instance is used.


<br>

---

<br>



## 10. Transient — `AddTransient()`

Register a transient service using:

```csharp
services.AddTransient<IProductRepository, ProductRepository>();
```

### Behavior

A new object is created whenever the service is requested.

```text
Request
│
├── Request #1 → Instance A
├── Request #2 → Instance B
└── Request #3 → Instance C
```

If a controller uses the service multiple times and each request resolves a new transient instance:

```text
Controller
├── Service request → Instance A
├── Service request → Instance B
└── Service request → Instance C
```

The instances do not share their in-memory state.

### Example Observation

Suppose:

```text
Instance A
└── Add Product
```

Then another transient instance is used:

```text
Instance B
└── Get Products
```

The product added to Instance A is not available in Instance B because they are separate objects.

> [!Important]
> **Transient = new instance every time the service is requested.**


<br>

---

<br>



## 11. `TryAddSingleton()`, `TryAddScoped()` and `TryAddTransient()`

These methods are useful when dealing with **multiple registrations for the same service interface**.

<br>

<div align="center">
  <img width="600" alt="TryAdd DI registration" src="https://github.com/user-attachments/assets/3a479dc4-37c6-4fbd-92f6-a4ed3a7653c9" />
</div>

<br>

### 11.1 The Problem: Multiple Registrations

Suppose the same interface is registered more than once:

```csharp
services.AddTransient<IProductRepository, ProductRepository>();
services.AddTransient<IProductRepository, ProductRepository>();
```

The source notes describe the later registration as overriding the earlier one when resolving a single service.

A similar situation can happen when multiple classes implement the same interface:

```text
IProductRepository
       ├── ProductRepository
       └── TestRepository
```

Registering implementations sequentially can lead to the later registration being selected for a normal single-service resolution.

### 11.2 The `TryAdd` Solution

ASP.NET Core provides:

```csharp
TryAddSingleton()
TryAddScoped()
TryAddTransient()
```

These methods perform the registration **only if the service has not already been registered**.

### 11.3 Behavior

```text
Service not registered
        ↓
TryAdd...
        ↓
Registration happens
```

But:

```text
Service already registered
        ↓
TryAdd...
        ↓
Registration skipped
```

### 11.4 Example

```csharp
services.AddTransient<IProductRepository, ProductRepository>();
services.AddTransient<IProductRepository, ProductRepository>(); //overrides the first

services.TryAddTransient<IProductRepository, ProductRepository>();
services.TryAddTransient<IProductRepository, ProductRepository>(); //skips this second
```

### 11.5 Applies to All Three Lifetimes

The same principle is available for:

```csharp
TryAddTransient()
TryAddScoped()
TryAddSingleton()
```

> [!Important]
> `Add...` registers the service normally, while `TryAdd...` registers it **only when a registration does not already exist**.


<br>

---

<br>



## 12. Resolve a Service Directly in an Action Method

Sometimes a service is needed by **only one action method** in a controller.

Instead of injecting the service into the controller constructor, it can be resolved directly in the action using:

```csharp
[FromServices]
```

### 12.1 Why Use `[FromServices]`?

Consider:

```text
Controller
├── Action A
├── Action B
├── Action C
└── Action D
```

Suppose only Action D needs:

```text
IProductRepository
```

Constructor injection makes the dependency available to the whole controller.

For a dependency needed only in one action, action-level injection can keep the constructor smaller.

### 12.2 Syntax

```csharp
public IActionResult Get(
    [FromServices] IMyService myService)
{
    // use service
}
```

### 12.3 Example

```csharp
public IActionResult GetName([FromServices] IProductRepository _productRepository)
{
   var name = _productRepository.GetName();
   return Ok(name);
}
```

### 12.4 Flow

```text
HTTP Request
     ↓
Specific Action Called
     ↓
[FromServices] IProductRepository
     ↓
DI Container resolves service
     ↓
Action executes
```

The framework resolves the service when that action is invoked.

> [!Tip]
> Use constructor injection for dependencies that are broadly required by a class. Use `[FromServices]` when a dependency is specifically needed by an individual action.


<br>

---

<br>



## 13. DI Mental Model

The entire concept can be remembered as:

```text
                 DI CONTAINER
                IServiceProvider
                      │
                      │ resolves
                      ↓
            IProductRepository
                      │
                      ↓
             ProductRepository
                      │
                      ↓
                  Controller
```

### Without DI

```text
Controller
    │
    └── new ProductRepository()
             │
             └── tightly coupled
```

### With DI

```text
Controller
    │
    └── IProductRepository
             ↑
             │ injected by
             │
       DI Container
             │
             ↓
     ProductRepository
```

### Service Lifetime Cheat Sheet

| Registration | Instance lifetime | Sharing |
|---|---|---|
| `AddSingleton()` | Entire application | Shared across requests |
| `AddScoped()` | One HTTP request | Shared within that request |
| `AddTransient()` | Every service request | Not shared |
| `TryAddSingleton()` | Singleton, only if not already registered | Conditional registration |
| `TryAddScoped()` | Scoped, only if not already registered | Conditional registration |
| `TryAddTransient()` | Transient, only if not already registered | Conditional registration |

### Final Mental Model

```text
Interface
   ↓
Implementation
   ↓
Register in ConfigureServices()
   ↓
DI Container / IServiceProvider
   ↓
Resolve dependency
   ↓
Inject into Controller / Action
   ↓
Use dependency
```

> [!Important]
> The main purpose of DI is not merely to avoid writing `new`. Its real value is **decoupling the consumer from the concrete implementation**, making the application easier to maintain, replace, and test.


<br>

---
---

<br>

