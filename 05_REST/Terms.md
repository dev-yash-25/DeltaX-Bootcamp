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
