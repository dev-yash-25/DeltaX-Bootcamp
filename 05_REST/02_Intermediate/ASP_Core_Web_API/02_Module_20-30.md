# ASP.NET Core Web API — Project Structure & Hosting

## Index

1. [ASP.NET Core Web API Project Files](#1-aspnet-core-web-api-project-files)
2. [launchSettings.json](#2-launchsettingsjson)
3. [appsettings.json](#3-appsettingsjson)
4. [.csproj Project File](#4-csproj-project-file)
5. [Converting a Console App to ASP.NET Core](#5-converting-a-console-app-to-aspnet-core)
6. [Default Host Builder](#6-default-host-builder)
7. [Startup Class](#7-startup-class)
8. [ConfigureServices Method](#8-configureservices-method)
9. [Configure Method](#9-configure-method)
10. [Environment Handling](#10-environment-handling)
11. [Quick Revision](#11-quick-revision)

<br>

# 1. ASP.NET Core Web API Project Files

A typical ASP.NET Core Web API project contains:

```text
MyWebApi/
│
├── Controllers/
│   └── WeatherForecastController.cs
│
├── Models/
│   └── WeatherForecast.cs
│
├── Properties/
│   └── launchSettings.json
│
├── appsettings.json
├── Program.cs
├── Startup.cs
└── MyWebApi.csproj
```

### Dependencies

The **Dependencies** section in Visual Studio contains project dependencies such as:

- NuGet packages
- Framework references
- Other project dependencies
- Analyzers

### Controllers

Controllers handle incoming HTTP requests.

A Web API controller normally inherits from:

```csharp
ControllerBase
```

### Models

Models represent the application's data.

The default template contains a `WeatherForecast` model, but you can create your own `Models` folder and classes.

> The default project structure is only a starting point. You can create additional folders and classes according to your application's architecture.

<br>

# 2. launchSettings.json

Location:

```text
Properties/launchSettings.json
```

> [!NOTE]
> **`launchSettings.json` → "How do I launch?"**

`launchSettings.json` defines **launch profiles** used mainly during local development.

## Profiles

Common profiles include:

### IIS Express

Runs the application using **IIS Express**.

- Windows-specific
- Can automatically launch the browser
- Can define HTTP/HTTPS URLs
- Can define environment variables

Example:

```json
"environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Development"
}
```

### Project Profile

Runs the application using **Kestrel**, ASP.NET Core's cross-platform web server.

It can be started using:

```bash
dotnet run
```

Kestrel supports:

```text
Windows
Linux
macOS
```

## IIS Express vs Kestrel

| IIS Express | Project / Kestrel |
|---|---|
| Windows-specific | Cross-platform |
| Uses IIS Express | Uses Kestrel |
| Common with Visual Studio | Can run with `dotnet run` |
| Browser can launch automatically | Console output/logs are common |

You can switch between profiles in Visual Studio.

> **Remember:** `launchSettings.json` controls how the application is launched locally; it is not the main place for application configuration.

<br>

# 3. appsettings.json

`appsettings.json` stores **application configuration/settings**.

> [!NOTE]
> **`appsettings.json` → "What settings does my application use?"**

Example:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "..."
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information"
        }
    }
}
```

It can contain:

- Connection strings
- Logging configuration
- Custom application settings
- External service configuration

## Environment-Specific Configuration

You can have:

```text
appsettings.json
appsettings.Development.json
appsettings.Production.json
```

Environment-specific files can override values from the base configuration.

### Key Difference

```text
launchSettings.json
        ↓
How do I launch?

appsettings.json
        ↓
What settings does my application use?
```

<br>

# 4. .csproj Project File

The `.csproj` file contains important **project configuration**.

Example:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>

</Project>
```

## Important Parts

### SDK

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
```

Specifies the SDK used to build the project.

For an ASP.NET Core web project, the Web SDK is used.

### Target Framework

```xml
<TargetFramework>net5.0</TargetFramework>
```

Specifies the .NET version targeted by the project.

### Package References

Packages can be declared in an `ItemGroup`:

```xml
<ItemGroup>
    <PackageReference Include="Some.Package" Version="1.0.0" />
</ItemGroup>
```

## SDK-Style Project Files

Modern .NET project files are simplified.

You generally do not need to list every `.cs` file manually.

Files inside the project directory are normally included automatically according to SDK conventions.

<br>

# 5. Converting a Console App to ASP.NET Core

A basic console application can be converted into an ASP.NET Core application by changing the project SDK and adding web-hosting setup.

## Step 1 — Change `.csproj`

### Console Application

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>

</Project>
```

### ASP.NET Core Application

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>

</Project>
```

Main change:

```text
Microsoft.NET.Sdk
        ↓
Microsoft.NET.Sdk.Web
```

The Web SDK provides web-specific project functionality.

## Step 2 — Configure Program.cs

```csharp
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ConsoleAppone
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webHost =>
                {
                    webHost.UseStartup<Startup>();
                });
    }
}
```

Flow:

```text
Main()
  ↓
CreateHostBuilder()
  ↓
Build()
  ↓
Run()
```

## Step 3 — Create Startup.cs

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppone
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(
                        "Hello from web API app.");
                });

                endpoints.MapGet("/test", async context =>
                {
                    await context.Response.WriteAsync(
                        "Hello from web API app - Test");
                });
            });
        }
    }
}
```

The endpoints demonstrate that the application can receive HTTP requests and return responses.

<br>

# 6. Default Host Builder

The **Host** is responsible for managing the application's lifetime and providing common application infrastructure.

The standard hosting approach in ASP.NET Core 5 is:

```csharp
Host.CreateDefaultBuilder(args)
```

Example:

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webHost =>
        {
            webHost.UseStartup<Startup>();
        });
```

Then:

```csharp
CreateHostBuilder(args).Build().Run();
```

## What does `CreateDefaultBuilder()` provide?

It sets up common infrastructure such as:

- Dependency Injection
- Configuration
- Logging
- Environment configuration
- Content root
- Hosting defaults

It also supports configuration from sources such as `appsettings.json` and environment variables.

### Content Root

The content root is set to the application's current directory.

Conceptually:

```text
Application starts
       ↓
CreateDefaultBuilder()
       ↓
Configure common infrastructure
       ↓
Build host
       ↓
Run application
```

<br>

# 7. Startup Class

In the ASP.NET Core 5 hosting model, the `Startup` class contains application configuration.

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env)
    {
    }
}
```

The two important methods are:

```text
ConfigureServices()
        ↓
Register/configure services

Configure()
        ↓
Configure HTTP request pipeline
```

<br>

# 8. ConfigureServices Method

```csharp
public void ConfigureServices(IServiceCollection services)
{
}
```

`ConfigureServices` is used to **register and configure services** in ASP.NET Core's Dependency Injection container.

The parameter:

```csharp
IServiceCollection services
```

represents the collection of services being configured.

Example:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
}
```

Services commonly registered here include:

```text
Application services
Repositories
Database contexts
Authentication
Other dependencies
```

Conceptually:

```text
ConfigureServices()
        ↓
Register dependencies
        ↓
DI Container
        ↓
Controllers / Services receive dependencies
```

<br>

# 9. Configure Method

```csharp
public void Configure(
    IApplicationBuilder app,
    IWebHostEnvironment env)
{
}
```

`Configure` defines the **HTTP request pipeline**.

The pipeline consists of middleware components through which requests pass.

Example:

```csharp
public void Configure(
    IApplicationBuilder app,
    IWebHostEnvironment env)
{
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
```

Conceptually:

```text
HTTP Request
     ↓
 Middleware
     ↓
 Middleware
     ↓
 Middleware
     ↓
 Endpoint
     ↓
HTTP Response
```

### IApplicationBuilder

`IApplicationBuilder` is used to configure the request pipeline.

Common middleware methods include:

```csharp
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
```

The exact middleware depends on the application.

<br>

# 10. Environment Handling

Applications commonly run in different environments:

```text
Development
Staging
Production
```

The application may need different behavior in each environment.

`IWebHostEnvironment` provides information about the current hosting environment.

Example:

```csharp
public void Configure(
    IApplicationBuilder app,
    IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        // Development-specific behavior
    }

    app.UseRouting();
}
```

This allows middleware or application behavior to vary by environment.

Example:

```text
Development
    ↓
Development-specific behavior

Production
    ↓
Production-specific behavior
```

<br>

# 11. Quick Revision

## Project Files

```text
.csproj
    → Project configuration

Program.cs
    → Entry point + host setup

Startup.cs
    → Service registration + request pipeline

appsettings.json
    → Application configuration

launchSettings.json
    → Local launch profiles

Controllers/
    → HTTP request handling

Models/
    → Data models
```

## Most Important Distinction

```text
launchSettings.json
        ↓
"How do I launch?"

appsettings.json
        ↓
"What settings does my application use?"
```

## Startup

```text
Startup
 ├── ConfigureServices()
 │      ↓
 │   Register services / dependencies
 │
 └── Configure()
        ↓
     Configure HTTP request pipeline
```

## Hosting

```text
Program.cs
    ↓
Host.CreateDefaultBuilder()
    ↓
Web Host
    ↓
Startup
    ↓
Application
```

## Console → ASP.NET Core

```text
Microsoft.NET.Sdk
        ↓
Microsoft.NET.Sdk.Web
```

<br>

# Important .NET Terminology

> [!TIP]
> **The naming changed after .NET Core 3.1.**
>
> ```text
> .NET Core 1.x – 3.1
>        ↓
> Microsoft's modern, cross-platform .NET platform
>
> .NET 5+
>        ↓
> Unified .NET platform
> ```
>
> Starting with **.NET 5**, Microsoft dropped the **"Core"** name and used the unified **.NET** name.
>
> **ASP.NET Core** is the web framework for building web applications and Web APIs on .NET.
>
> Therefore, avoid saying:
>
> ```text
> ASP.NET Core is a component of .NET Core
> ```
>
> for modern .NET. A better mental model is:
>
> ```text
> .NET
> └── ASP.NET Core
>       ├── Web API
>       ├── MVC
>       └── Razor Pages
> ```
>
> The course uses **ASP.NET Core 5.0**, so older terminology such as `.NET Core` and the `Startup` hosting model appears throughout these notes.
