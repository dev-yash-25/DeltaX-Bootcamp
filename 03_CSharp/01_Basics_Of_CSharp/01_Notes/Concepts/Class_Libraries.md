

# What is a Class Library?

A **Class Library is a project** whose purpose is to contain reusable C# code.


# Without a Class Library

Suppose you have one ASP.NET Core API project:

```text
TaskManagementAPI
│
├── Controllers
│   └── TaskController.cs
│
├── Services
│   └── TaskService.cs
│
├── Models
│   └── Task.cs
│
└── Program.cs
```

Everything is inside one project.

```text
TaskManagementAPI
       │
       ├── Controllers
       ├── Services
       ├── Models
       └── Repositories
```

This is completely valid, especially for a small application.


<br>


# With Class Libraries

You can split the application into separate projects:

```text
TaskManagementAPI
       │
       └── Controllers

TaskManagement.Services
       │
       └── TaskService

TaskManagement.Repositories
       │
       └── TaskRepository

TaskManagement.Models
       │
       └── Task
```

For example:

```text
TaskManagementAPI
       |
       ↓
TaskManagement.Services
       |
       ↓
TaskManagement.Repositories
       |
       ↓
Database
```

Each of these can be a **Class Library project**.


<br>



# Example

Suppose you create:

```text
TaskManagement.Services
```

as a Class Library.

Inside it:

```csharp
public class TaskService
{
    public string GetTask()
    {
        return "REST Assignment";
    }
}
```

Then your API project references the class library:

```text
TaskManagementAPI
       ↓
TaskManagement.Services
```

And your controller can use it:

```csharp
public class TasksController : ControllerBase
{
    private readonly TaskService _service;

    public TasksController(TaskService service)
    {
        _service = service;
    }
}
```

<br>


# Why use Class Libraries?

Main reason:

> **Separation and reusability.**

For a larger application, you might have:

```text
MyAPI
MyAPI.Business
MyAPI.Data
MyAPI.Models
MyAPI.Common
```

This makes responsibilities clearer.

For example:

```text
API
 ↓
Business Logic
 ↓
Data Access
```


