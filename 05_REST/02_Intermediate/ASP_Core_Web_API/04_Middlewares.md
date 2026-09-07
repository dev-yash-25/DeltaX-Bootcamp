# Action Methods & Middleware

## Index

- [1. Action Methods in `Startup.cs`](#1-action-methods-in-startupcs)
- [2. Action Methods in Controllers](#2-action-methods-in-controllers)
- [3. HTTP Request Pipeline](#3-http-request-pipeline)
- [4. Middleware](#4-middleware)
- [5. `Use()`, `Next()`, `Run()` and `Map()`](#5-use-next-run-and-map)  🏷️
- [6. Custom Middleware](#6-custom-middleware)


<br>

---

<br>


## 1. Action Methods in `Startup.cs`

Endpoints can be mapped directly inside `Startup.cs` using `MapGet()`.

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleAppone
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }   

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello from web API app.");
                });


                endpoints.MapGet("/test", async context =>
                {
                    await context.Response.WriteAsync("Hello from web API app - Test");
                });
            });
        }
    }
}
```

### Output

`GET /`

```text
Hello from web API app.
```

`GET /test`

```text
Hello from web API app - Test
```


<br>



## 2. Action Methods in Controllers

Instead of defining endpoint logic directly in `Startup.cs`, it can be placed inside a controller.

### `Startup.cs`

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleAppone
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }   

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
```

### `TestController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;

namespace ConsoleAppone.Controllers
{
    [ApiController]
    [Route("test/[action]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello From Get";
        }
        public string Get1()
        {
            return "Hello From Get1";
        }
    }
}
```

### How `[action]` works

```text
[Route("test/[action]")]
```

`[action]` is replaced by the action method's name.

```text
Get()  → /test/get
Get1() → /test/get1
```

### Access

```text
endpoint ->
1. https://localhost:64428/test/get  
2. https://localhost:64428/test/get1
```

### Output

For `/test/get`:

```text
Hello From Get
```

For `/test/get1`:

```text
Hello From Get1
```


<br>

---

<br>


## 3. HTTP Request Pipeline

A request does not go directly from the client to a controller.

It first passes through the **HTTP request pipeline**, which consists of middleware components.

> [!Important]
> **Order matters in Middleware.**

<br>
<div align = "center">
  <img
    width="500"
    alt="image"
    src="https://github.com/user-attachments/assets/69a0b7d3-7500-40cb-a5a0-eb8fe9c867c9"
  />
</div>
<br>

### Request flow

The request travels through middleware in insertion order. The response travels back through them in reverse order.

If middleware calls `next()`, execution continues.

If it does not call `next()`, the pipeline stops there.

### Middleware diagrams

<br>
<table>
<tr>
  <td>
    <img width="559" height="357" alt="image" src="https://github.com/user-attachments/assets/cd210a7e-4396-4ea1-8890-bc82d271d84b" />
  </td>
  <td>
    <img width="738" height="342" alt="image" src="https://github.com/user-attachments/assets/a936d461-6d9a-4181-9326-135c506baeb0" />
  </td>
</tr>
</table>


<br>

---

<br>


## 4. Middleware

### Definition

**Middleware is a piece of code inserted into the HTTP request pipeline to perform a specific function.**

A middleware can:
- Handle a request.
- Call the next middleware.
- Modify the request.
- Modify the response.
- Stop the pipeline.

An application can contain any number of middleware components.

### Common examples

1. **Routing**

    Handles path-based requests.
    
    ```text
    Request → Routing → Appropriate endpoint/controller
    ```

2. **Authentication**

    Validates requests before they reach protected application logic.
    
    ```text
    Request
      ↓
    Authentication
      ↓
    Valid → Continue
    Invalid → Stop / return response
    ```

3. **Exception Handling**

    Useful for global exception handling because requests and responses pass through the middleware pipeline.

<br>

### Where is the pipeline configured?

The `Configure()` method in `Startup.cs` defines the request pipeline.

It is mandatory for the application in this ASP.NET Core 5.0 `Startup` model.

<br>


Middleware can therefore have code both before and after `next()`:

```csharp
app.Use(async (context, next) =>
{
    // Executes before next middleware
    await next();

    // Executes after next middleware returns
});
```

### Mental model

```text
Middleware 1 Before
        ↓
Middleware 2 Before
        ↓
Endpoint
        ↓
Middleware 2 After
        ↓
Middleware 1 After
```


<br>

---

<br>


## 5. `Use()`, `Next()`, `Run()` and `Map()`

| Method | Purpose |
|---|---|
| `Use()` | Adds middleware that can call the next middleware |
| `Next()` | Passes execution to the next middleware |
| `Run()` | Terminal middleware; does not call next |
| `Map()` | Branches the pipeline based on a path |


<br>


### 1. `Use()`

`Use()` is used to add middleware.

It takes:

```text
HttpContext context
RequestDelegate next
```

Example:

```csharp
app.Use(async (context, next) =>
{
    // context + next
    await next();
});
```

#### `HttpContext`

`HttpContext` represents the current HTTP request and response.

```text
HttpContext
│
├── Request
│   ├── Path
│   ├── Method
│   ├── Headers
│   ├── Query
│   └── Body
│
├── Response
│   ├── StatusCode
│   ├── Headers
│   └── Body
│
└── User
```


<br>




### 2. `Next()`

`next()` passes execution to the next middleware.

```csharp
app.Use(async (context, next) =>
{
    await next();
});
```

Without:

```csharp
await next();
```

execution does not continue to the next middleware.

#### Flow

```text
Current Middleware
       ↓
    next()
       ↓
Next Middleware
       ↓
returns
       ↓
Current Middleware continues
```


### `Use()` + `Run()` Example

```csharp
namespace ConsoleAppone
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }   

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Middleware 1
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello From Use 1.1 Middleware");
                await next();
                await context.Response.WriteAsync("Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2 ");
            });

            // Middleware 2
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello From Use 2.1 Middleware ");
                await next();
                await context.Response.WriteAsync("Hello From Use 2.2 Middleware - I am executed at the last after Run");
            });

            // Middleware 3
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Request Complete.. No next() after this. Thus Pipeline stops here");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // End of Middleware Pipeline - Middleware 4
            app.Run(async context =>
            {
                await context.Response.WriteAsync("End of Run Middleware Pipeline");
            });
        }
    }
}
```

#### Output

```text
Hello From Use 1.1 Middleware 
Hello From Use 2.1 Middleware 
Request Complete.. No next() after this. Thus Pipeline stops here 
Hello From Use 2.2 Middleware - I am executed at the last after Run 
Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2
```

#### Why?

Execution enters:

```text
Use 1.1
   ↓
Use 2.1
   ↓
Middleware 3
```

Middleware 3 does not call `next()`:

```csharp
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Request Complete.. No next() after this. Thus Pipeline stops here 
");
});
```

Therefore, later middleware such as `Run()` is not reached.

Then execution returns through the previous middleware:

```text
Middleware 3
   ↓
Use 2.2
   ↓
Use 1.2
```

This is why code after `await next()` runs on the way back.



<br>




### 3. `Run()`

`Run()` adds **terminal middleware**.

```csharp
app.Run(async context =>
{
    await context.Response.WriteAsync("End of Middleware Pipeline");
});
```

It takes one parameter:

```text
HttpContext context
```

Through `HttpContext`, the middleware can access:

```text
context.Request
context.Response
context.User
context.Session
context.Items
```

> [!Note]
> `Run()` marks the end of the middleware pipeline. It does not call `next()`, so later middleware is not executed.


<br>




### 4. `Map()`

`Map()` branches the request pipeline based on a path.

Example:

```csharp
app.Map("/yash", CustomCode);
```

A request matching:

```text
/yash
```

is sent into the branch defined by `CustomCode`.

#### Mental model

```text
Request
   ↓
Main Pipeline
   ↓
Does path match /yash?
   ↙             ↘
 Yes              No
  ↓                ↓
Branch          Main Pipeline
```

<br>


### `Map()` Example

```csharp
using Microsoft.AspNetCore.Builder;

namespace ConsoleAppone
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }   

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {   

            // Middleware 1
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello From Use 1.1 Middleware ");
                await next();
                await context.Response.WriteAsync("Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2");
            });

            // Middleware Map 2 - Branched to seperate route
            app.Map("/yash", CustomCode);

            // Middleware 3
            app.Use(async (context, next) =>
            {
            });

            // Middleware 4
            app.Use(async (context, next) =>
            {
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // End of Middleware Pipeline - Middleware 5
            app.Run(async context =>
            {
            });
        }

        private void CustomCode(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello from Yash ");
            });
        }
    }
}
```

#### Request

```text
/yash
```

#### Output

```text
Hello From Use 1.1 Middleware 
Hello from Yash 
Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2
```

#### Flow

```text
Request /yash
      ↓
Use 1.1
      ↓
Map("/yash")
      ↓
CustomCode
      ↓
Return to Use 1.2
```

`Map()` creates a separate branch for matching requests.


<br>

---

<br>






# 6. Custom Middleware

Instead of putting middleware directly in `Startup.cs`, create a separate middleware class.

## Step 1 — Create `CustomMiddleware.cs`

```csharp
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ConsoleAppone
{
    public class CustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("Hello From new file 2 Custom Middleware 1");
            await next(context);
            await context.Response.WriteAsync("Bye From new file 2 Custom Middleware ");
        }
    }
}
```

### Important parts

```csharp
public class CustomMiddleware : IMiddleware
```

The middleware implements `IMiddleware`.

Its logic is placed in:

```csharp
InvokeAsync()
```

It receives:

```text
HttpContext context
RequestDelegate next
```

<br>

## Step 2 — Register the Middleware

Inside `ConfigureServices()`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddTransient<CustomMiddleware>();
}
```

`AddTransient` registers `CustomMiddleware` with the dependency injection container.


<br>



## Step 3 — Use the Middleware

Use:

```csharp
app.UseMiddleware<CustomMiddleware>();
```

Complete example:

```csharp
namespace ConsoleAppone
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<CustomMiddleware>();
        }   

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {   

            // Middleware 1
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello From Use 1.1 Middleware ");
                await next();
                await context.Response.WriteAsync("Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2 ");
            });

            // Custom Middleware 2
            app.UseMiddleware<CustomMiddleware>();

            // Middleware 3
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello From Use 3.1 Middleware ");
                await next();
                await context.Response.WriteAsync("Hello From Use 3.2 Middleware - I am executed at the last after Run ");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
```

### Output

```text
Hello From Use 1.1 Middleware 
Hello From new file 2 Custom Middleware 1
Hello From Use 3.1 Middleware 
Hello From Use 3.2 Middleware - I am executed at the last after Run 
Bye From new file 2 Custom Middleware 
Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2  
```

### Execution flow

```text
Use 1.1
   ↓
Custom Middleware
   ↓
Use 3.1
   ↓
Use 3.2
   ↓
Custom Middleware "Bye"
   ↓
Use 1.2
```

The statements after `next()` execute while the pipeline is unwinding.


<br>




###  Custom Middleware Execution Flow

Consider:

```csharp
app.Use(async (context, next) =>
{
    Console.WriteLine("Before");
    await next();
    Console.WriteLine("After");
});
```

Flow:

```text
Before
  ↓
next()
  ↓
Next Middleware
  ↓
returns
  ↓
After
```

For multiple middleware:

```text
Middleware 1 Before
    ↓
Middleware 2 Before
    ↓
Endpoint
    ↓
Middleware 2 After
    ↓
Middleware 1 After
```


<br>

---

<br>


> [!Tip]
> ## `Next()` method in built-in middleware 🏷️,
> - We inspect open-source implementation on [GitHub](https://github.com/dotnet/aspnetcore), [class1](https://github.com/dotnet/aspnetcore/blob/main/src/Http/Routing/src/Builder/EndpointRoutingApplicationBuilderExtensions.cs) and [class2](https://github.com/dotnet/aspnetcore/blob/main/src/Http/Routing/src/EndpointRoutingMiddleware.cs)
> - By navigating the repository, we can confirm how the framework handles request flow through the middleware pipeline via dependency injection.

<br>

> [!Important]
>  Middleware executes in the order it is added. Code before `next()` runs on the forward path; code after `next()` runs on the return path.


<br>

---

<br>
