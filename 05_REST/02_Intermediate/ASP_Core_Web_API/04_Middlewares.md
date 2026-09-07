Video 1

## Action Methods inside `Startup.cs`

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


## Action Methods inside Controllers
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
Access
```
endpoint ->
1. https://localhost:64428/test/get  
2. https://localhost:64428/test/get1
```


Video 2
# What is Middleware and HTTP Request Pipeline | Asp.Net Core Web API tutorial



This tutorial explains the fundamental concept of **Middleware** and the **HTTP Request Pipeline** in ASP.NET Core web applications.

> [!Important]
> Order matters in Middleware


### The HTTP Request Pipeline
Contrary to the assumption that a request goes directly from the browser to a controller's action method, it must first pass through a **pipeline** (0:52-1:04). 

<br>

<div align = "center">
  <p>Request First Passes through the Pipeline of Midlewares before reaching controller</p>
  <img width="500" alt="image" src="https://github.com/user-attachments/assets/69a0b7d3-7500-40cb-a5a0-eb8fe9c867c9" />
</div>

<br>

*   **How it works:** The pipeline consists of multiple middleware components (1:11-1:14). When a request enters, it passes through the first middleware, then the second, and so on (1:30-1:58).
*   **The 'Next' Method:** Each middleware has the option to call a `next` method to pass the execution to the subsequent middleware. If a middleware does not call `next`, the request stops there and returns (1:39-2:03).
*   **Response Path:** Once the request hits the end of the pipeline and generates a response, it travels back through the middleware components in reverse order (2:03-2:24).


<br>
<p>Middleware Diagram</p>
<table align = "center">
<tr>
  <td>
    <img width="450" alt="image" src="https://github.com/user-attachments/assets/cd210a7e-4396-4ea1-8890-bc82d271d84b" />
  </td>
  <td>
    <img width="450" alt="image" src="https://github.com/user-attachments/assets/a936d461-6d9a-4181-9326-135c506baeb0" />
  </td>
</tr>
</table>
<br>


### Understanding Middleware
*   **Definition:** Middleware is a piece of code (a "bundle of lines of code") inserted into the request pipeline to provide specific functionality to an application.    A function that handles the request or calls the given next function.
*   **Flexibility:** Developers can use pre-built middleware or create custom ones. An application can have any number of middleware components (2:46-3:04).
*   **Order Matters:** The order in which middleware is added to the pipeline is critical. The request will travel through them in exactly the same sequence they were inserted (3:04-3:13).

### Real-World Examples 
*   **Routing:** Required to handle path-based requests (3:17-3:26).
*   **Authentication:** Validates requests before they reach the controller. If the request is invalid, the middleware can return the request immediately, securing the controller (3:35-4:18).
*   **Exception Handling:** Ideal for global exception handling because every request and response passes through the middleware (4:23-4:41).

### Implementation in ASP.NET Core
*   **Startup Class:** The `Configure` method in the `Startup` class is where the request pipeline is defined (4:53-5:01).
*   **Must-Have Method:** The `Configure` method is mandatory; the application will not run without it (5:01-5:12).
*   **Code Examples:** The instructor demonstrates adding middleware like `app.useDeveloperExceptionPage`, `app.useRouting`, and `app.useEndpoints` (5:18-5:32).
*   **Instructor Observation:** The instructor emphasizes that the **order is vital**. For example, changing the order of `useRouting` and `useEndpoints` can cause exceptions, highlighting that developers must be very careful when configuring the pipeline (5:32-5:56).


 Video 3

 # Working with Run(), Map(), Use() and Next() method | ASP.NET Core 5.0 Web API Tutorial

This tutorial provides an introduction to the fundamental middleware components in *ASP.NET Core* used to handle HTTP requests. Below are the key concepts and methods discussed:

*   **Middleware Basics:** The instructor explains that understanding middleware methods is essential for building and customizing your network communication in *ASP.NET Core* (0:02 - 0:28).
*   **Key Methods:**
    *   **Use():** This method is used to implement custom middleware (0:20).
         - Takes 2 paramteres `HttpContext context`, `RequestDelegate next`
            - context represents the current HTTP request + response.
          ```
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
          ```csharp
          app.Use(async (context, next) =>
          {
              // context + next
              await next();
          });
          ```  
    *   **Next():** A crucial component for controlling the flow of the request pipeline. It allows the execution to pass from one piece of middleware to the next (0:47 - 0:52).
    *   **Run():** Acts as a terminal middleware. When used, it terminates the pipeline, meaning it will not call the next middleware .
         - Takes single Paramter `HttpContext context` that gives access to
           ```
            context.Request
            context.Response
            context.User
            context.Session
            context.Items
            ```
            ```csharp
            app.Run(async context =>
            {
                await context.Response.WriteAsync("End of Middleware Pipeline");
            });
            ```
    *   **Map():** Used for branching the request pipeline based on specific path segments, allowing for different logic for different URLs (1:18 - 1:34).
*   **Instructor's Observations:**
    *   The instructor emphasizes that `Next()` plays a very important role in communication (1:18).
    *   These methods should be used based on specific requirements for your application's network handling and routing logic (1:21 - 1:30).
    *   Proper use of these methods helps in creating a clean and efficient request processing pipeline (1:34 - 1:38).
 
> [!Note]
> `Run` marks the end of middleware pipeline, after thaat no middleware shall execute.

<br>

Startup.cs

Using Use, Run
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
                await context.Response.WriteAsync("Hello From Use 1.1 Middleware \n");
                await next();
                await context.Response.WriteAsync("Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2  \n");
            });

            // Middleware 2
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello From Use 2.1 Middleware \n");
                await next();
                await context.Response.WriteAsync("Hello From Use 2.2 Middleware - I am executed at the last after Run \n");
            });

            // Middleware 3
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Request Complete.. No next() after this. Thus Pipeline stops here \n");
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
                await context.Response.WriteAsync("End of Run Middleware Pipeline\n");
            });
        }
    }
}
```
```
Hello From Use 1.1 Middleware 
Hello From Use 2.1 Middleware 
Request Complete.. No next() after this. Thus Pipeline stops here 
Hello From Use 2.2 Middleware - I am executed at the last after Run 
Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2
```

Using Map
- Branch out to new endpoint, and run customcode
```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

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
                await context.Response.WriteAsync("Hello From Use 1.1 Middleware \n");
                await next();
                await context.Response.WriteAsync("Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2  \n");
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
                await context.Response.WriteAsync("Hello from Yash \n");
            });
        }
    }
}
```
```
Hello From Use 1.1 Middleware 
Hello from Yash 
Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2
```


## Custom Middlewares
1. Create `CustomMiddleware.cs` class
2. Define Middleware
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
                await context.Response.WriteAsync("Hello From new file 2 Custom Middleware 1\n");
                await next(context);
                await context.Response.WriteAsync("Bye From new file 2 Custom Middleware \n");
            }
        }
    }
    ```
3. Add CustomMiddleware Services inside `Startup.cs`, inside `ConfigureServices`
      ```csharp
      public void ConfigureServices(IServiceCollection services)
      {
          services.AddControllers();
          services.AddTransient<CustomMiddleware>();
      }
     ```
4. Use inside Configure, just as other Middlewares, using `UseMiddleware<Name>()` method
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
                      await context.Response.WriteAsync("Hello From Use 1.1 Middleware \n");
                      await next();
                      await context.Response.WriteAsync("Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2  \n");
                  });
      
                  // Custom Middleware 2
                  app.UseMiddleware<CustomMiddleware>();
      
                  // Middleware 3
                  app.Use(async (context, next) =>
                  {
                      await context.Response.WriteAsync("Hello From Use 3.1 Middleware \n");
                      await next();
                      await context.Response.WriteAsync("Hello From Use 3.2 Middleware - I am executed at the last after Run \n");
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
      ```
      Hello From Use 1.1 Middleware 
      Hello From new file 2 Custom Middleware 1
      Hello From Use 3.1 Middleware 
      Hello From Use 3.2 Middleware - I am executed at the last after Run 
      Bye From new file 2 Custom Middleware 
      Hello From Use 1.2 Middleware - I am executed at the last after Use 2.2  
      ```

<br>

> [!Tip]
> ## `Next()` method in built-in middleware 🏷️,
> - We inspect open-source implementation on [GitHub](https://github.com/dotnet/aspnetcore), [class1](https://github.com/dotnet/aspnetcore/blob/main/src/Http/Routing/src/Builder/EndpointRoutingApplicationBuilderExtensions.cs) and [class2](https://github.com/dotnet/aspnetcore/blob/main/src/Http/Routing/src/EndpointRoutingMiddleware.cs)
> - By navigating the repository, we can confirm how the framework handles request flow through the middleware pipeline via dependency injection.
