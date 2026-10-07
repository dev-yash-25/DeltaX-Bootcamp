# 1. IIS and Kestrel

## Kestrel

**Kestrel** is the **built-in, cross-platform web server for ASP.NET Core**.

It receives HTTP requests and passes them into the ASP.NET Core application.

```text
Client
   ↓ HTTP Request
Kestrel
   ↓
ASP.NET Core API
   ↓
Database
```

When you run:

```bash
dotnet run
```

your ASP.NET Core application normally starts **Kestrel**.

Example:

```text
Now listening on: http://localhost:5000
```

This means Kestrel is listening for HTTP requests on port `5000`.

### Key Points

* Built into ASP.NET Core.
* Cross-platform: Windows, Linux, macOS.
* Lightweight and high-performance.
* Can directly serve an ASP.NET Core application.
* **Primarily a web server, not a reverse proxy.**



## IIS

**IIS (Internet Information Services)** is Microsoft's **web server for Windows**.

For ASP.NET Core, IIS can commonly work as a **reverse proxy** in front of Kestrel.

```text
Client
   ↓
IIS
   ↓
Kestrel
   ↓
ASP.NET Core API
   ↓
Database
```

The client communicates with IIS, and IIS forwards the request to the ASP.NET Core application running through Kestrel.

### IIS can provide

* HTTPS/TLS handling
* Request filtering
* Authentication
* Process management
* Reverse proxy functionality
* Hosting/management features for Windows applications



### IIS vs Kestrel

|                                 | Kestrel                           | IIS                                              |
| ------------------------------- | --------------------------------- | ------------------------------------------------ |
| Type                            | Web server                        | Web server                                       |
| Platform                        | Cross-platform - Linux, Windows, Mac                   | Windows                                          |
| Built into ASP.NET Core         | ✅                                 | ❌                                                |
| Can directly serve ASP.NET Core | ✅                                 | Usually through ASP.NET Core hosting integration |
| Reverse proxy                   | Not its primary role              | ✅ Commonly used                                  |
| Common usage                    | Direct hosting, containers, cloud | Windows/IIS hosting                              |


<br>

---

<br>


## Nested route 

A route where one resource is inside another resource, showing their relationship.
```
Parent → provides context
Child → resource being accessed/modified
```
Example:
```
/users/{userId}/orders
```
User = parent/context
Orders = child/resource being accessed


<br>

## Pagination
- Pagination in ASP.NET Core REST APIs is a technique used to divide a large dataset into **smaller, manageable chunks** (pages) before sending it to the client.
- Instead of returning thousands or millions of database rows in a single HTTP request—which can degrade network performance and crash applications—the API returns only a small subset at a time


<br>

---

<br>

## Q/A

### 1. Why separate Controller, Service, Repository layers? Why not one file?

Your idea is correct. The key reason is **Separation of Concerns + Single Responsibility**.

A better answer:

> We separate the application into layers so each layer has a clear responsibility and doesn't need to know the internal implementation of the other layers.
>
> **Controller** handles HTTP/API concerns, **Service** handles business logic, and **Repository** handles data access.
>
> This follows **Separation of Concerns and SRP**, making the code easier to maintain, test, and change.

Example:

```text
Controller
   ↓
"Get movie"
   ↓
Service
   ↓
"Apply business rules"
   ↓
Repository
   ↓
"Get it from database"
```

If everything were in one file:

```text
Controller
 ├── HTTP handling
 ├── validation
 ├── business logic
 ├── SQL queries
 ├── database handling
 └── mapping
```

Changing the database could then require changing the controller too.

**Small correction:** Don't say *"REST and SOLID principles define that we must have these layers."* REST doesn't require Controller/Service/Repository layers. These are architectural/design choices that help us achieve separation and maintainability.

<br>

### 2. Why DI? Why not just use `new`?

Your answer is also correct. The strongest points are **loose coupling and testability**.

I'd say:

> DI allows a class to receive its dependencies instead of creating them itself with `new`. This reduces tight coupling and makes the implementation easier to replace and mock during testing.

Your example is good:

```csharp
IStorageService _storageService;
```

The service can depend on the **abstraction**:

```text
IStorageService
      ↑
 ┌────┴─────┐
Supabase   Amazon S3
```

So you can change:

```text
SupabaseStorageService
        ↓
AmazonS3StorageService
```

without changing the class that uses `IStorageService`.

With `new`:

```csharp
_storageService = new SupabaseStorageService();
```

the class is directly coupled to Supabase.

### Interview-ready answer

> We use DI to achieve loose coupling and dependency inversion. Instead of a class creating its dependencies with `new`, the dependency is provided to it. This makes implementations easier to replace and makes unit testing and mocking easier.

**One important distinction:**  
**DIP** is the SOLID principle; **DI** is a technique commonly used to implement that principle. **IoC** is the broader concept of giving control of dependency creation/wiring to the framework/container.


<br>
