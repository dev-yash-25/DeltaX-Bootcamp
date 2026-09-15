# Dependency Injection

<br>
<div align = "center">
  <img width="600" alt="image" src="https://github.com/user-attachments/assets/595c23cb-8191-49b7-b92f-5db05a99c798" />
</div>
<br>

# What is Dependency Injection (DI) in ASP.NET Core Web API | ASP.NET Core 5.0 Web API Tutorial

This tutorial explains the architecture of web applications and the role of **Dependency Injection (DI)** in *ASP.NET Core*.

### Web Application Architecture
Most web applications follow a three-layer architecture (0:29):
1.  **Controllers:** Handle public requests and expose endpoints to the client.
2.  **Repository Layer:** Manages logic, coordination, and database interactions.
3.  **Services:** Global components like logging or email services.

### The Problem: Tight Coupling
In a traditional approach, components are manually instantiated using the `new` keyword (1:49, 2:27).
*   **Hard to maintain:** If you need to swap an implementation (e.g., changing from a local email sender to a third-party service), you must manually update every controller where the service was instantiated (2:41).
*   **Testing issues:** Using `new` makes unit testing difficult because you cannot easily inject mock objects (3:00).

### The Solution: Dependency Injection (DI)
DI promotes **Inversion of Control** and loose coupling (5:14). Instead of a class creating its dependencies, the framework provides them.

*   **How it works:**
    *   Create an **Interface** for your service (3:42).
    *   Implement the interface in your service class (3:52).
    *   In the controller, request the interface via the constructor instead of using `new` (4:04).
    *   The controller becomes unaware of the specific implementation (4:14).

### Configuring DI in ASP.NET Core
*   **Built-in Container:** *ASP.NET Core* has a built-in container (the *IServiceProvider*) that manages these dependencies (5:00, 5:53).
*   **Registration:** You register services in the `ConfigureServices` method of the `Startup` class (6:11).
*   **Service Lifetimes:** You can define how long a service instance lasts (6:24):
    1.  **Singleton:** One instance for the entire application life.
    2.  **Scoped:** One instance per client request.
    3.  **Transient:** A new instance every time the service is requested.
