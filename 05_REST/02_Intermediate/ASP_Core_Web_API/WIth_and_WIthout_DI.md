# WIth and Without DI

## Implementation  (Lets try for implementing Tight coupling for `Actor`)
```text
Controller → IActorService
               ↓
          new ActorService()
               ↓
        IActorRepository
               ↓
       new ActorRepository()
```

The key point: **interfaces remain, but objects are manually created with `new` instead of being injected by ASP.NET Core.**

### 1. `ActorRepository`

```csharp
public class ActorRepository : IActorRepository
{
    // existing implementation
}
```

No change to the methods.

---

### 2. `ActorService`

Keep the interface dependency:

```csharp
private readonly IActorRepository _actorRepository;
```

But instead of constructor injection:

```csharp
public ActorService(IActorRepository actorRepository)
{
    _actorRepository = actorRepository;
}
```

use:

```csharp
public ActorService()
{
    _actorRepository = new ActorRepository();
}
```

So:

```csharp
public class ActorService : IActorService
{
    private readonly IActorRepository _actorRepository;

    public ActorService()
    {
        _actorRepository = new ActorRepository();
    }

    // existing methods
}
```

---

### 3. `ActorsController`

Keep:

```csharp
private readonly IActorService _actorService;
```

But remove constructor injection:

```csharp
public ActorsController()
{
    _actorService = new ActorService();
}
```

So:

```csharp
public class ActorsController : ControllerBase
{
    private readonly IActorService _actorService;

    public ActorsController()
    {
        _actorService = new ActorService();
    }

    // existing actions
}
```

---

### 4. `Startup.cs`

Remove only the Actor registrations:

```csharp
// REMOVE
services.AddSingleton<IActorRepository, ActorRepository>();
services.AddScoped<IActorService, ActorService>();
```

Everything else stays.

---

### Result

**With DI:**

```csharp
public ActorsController(IActorService actorService)
```

ASP.NET creates and supplies the service.

**Without DI:**

```csharp
public ActorsController()
{
    _actorService = new ActorService();
}
```

The controller creates the service itself.

And inside:

```csharp
public ActorService()
{
    _actorRepository = new ActorRepository();
}
```

The service creates the repository itself.

### Important

You **keep**:

```csharp
IActorService
IActorRepository
```


<br>

---

<br>

## Q1. Will the application run and perform HTTP operations?

**Not necessarily with your current fake `List<Actor>` repository.**

Your tight-coupled code can compile and the application can start, because you no longer need DI registrations for:

```csharp
IActorRepository → ActorRepository
IActorService → ActorService
```

because you're doing:

```csharp
new ActorService()
new ActorRepository()
```

But there's a **runtime behavior issue** with your fake repository.

If your controller does:

```csharp
public ActorsController()
{
    _actorService = new ActorService();
}
```

and your service does:

```csharp
public ActorService()
{
    _actorRepository = new ActorRepository();
}
```

then every HTTP request creates a new chain:

```text
GET /actors
    ↓
new ActorsController
    ↓
new ActorService
    ↓
new ActorRepository
    ↓
new List<Actor>()
```

So if you do:

```text
POST /actors
```

the actor is stored in that particular repository instance.

Then:

```text
GET /actors
```

creates another repository:

```text
new ActorRepository()
```

with another empty:

```csharp
new List<Actor>()
```

Therefore, your CRUD won't behave like your previous application.

### Why did DI previously solve this?

You had:

```csharp
services.AddSingleton<IActorRepository, ActorRepository>();
```

So ASP.NET created **one ActorRepository instance** and reused it across requests.

Therefore:

```text
POST
 ↓
Repository instance A
 ↓
List contains Actor 1

GET
 ↓
same Repository instance A
 ↓
List still contains Actor 1
```

With your current `new` approach:

```text
POST
 ↓
Repository A
 ↓
List A → Actor 1

GET
 ↓
Repository B
 ↓
List B → EMPTY
```

So **the tight-coupled structure can run, but your current fake in-memory DB won't persist between HTTP requests.**

For a demonstration of coupling, that's actually a useful thing to show your mentor.

---

# Q2. What is the difference before and after?

### BEFORE — DI + loose coupling

Your architecture was:

```text
ActorsController
       ↓
   IActorService
       ↓
   IActorRepository
       ↓
   ActorRepository
```

Controller:

```csharp
public ActorsController(IActorService actorService)
{
    _actorService = actorService;
}
```

Service:

```csharp
public ActorService(IActorRepository actorRepository)
{
    _actorRepository = actorRepository;
}
```

Startup tells ASP.NET:

```csharp
services.AddSingleton<IActorRepository, ActorRepository>();
services.AddScoped<IActorService, ActorService>();
```

ASP.NET creates and supplies the dependencies.

### Characteristics

```text
Controller doesn't create Service
Service doesn't create Repository
```

They **receive** their dependencies.

And:

```text
ActorService → IActorRepository
```

rather than:

```text
ActorService → ActorRepository
```

So the service depends on an abstraction.

---

# AFTER — no DI + tight coupling

You changed it to something like:

```csharp
public ActorsController()
{
    _actorService = new ActorService();
}
```

and:

```csharp
public ActorService()
{
    _actorRepository = new ActorRepository();
}
```

Now:

```text
Controller
   ↓
new ActorService()
   ↓
new ActorRepository()
```

The classes themselves are responsible for creating their dependencies.

### Characteristics

```text
Controller creates Service
Service creates Repository
```

So dependency creation is now **inside the classes**, rather than being handled by ASP.NET Core's DI container.

---

# Q3. Why can't only one layer be tight-coupled?

This is the interesting question.

**It absolutely can.**

You can mix the two.

For example:

```text
Controller
    ↓
new ActorService()       ← tight
    ↓
IActorRepository         ← abstraction
    ↓
DI → ActorRepository
```

But there's a catch.

You said you tried:

> tight coupling in Controller, but kept DI in Service and Repository

and the application threw an error.

That's because your controller probably did:

```csharp
public ActorsController()
{
    _actorService = new ActorService();
}
```

But your `ActorService` still has:

```csharp
public ActorService(IActorRepository actorRepository)
{
    _actorRepository = actorRepository;
}
```

Therefore this:

```csharp
new ActorService()
```

is invalid.

C# says, effectively:

> You told me to create `ActorService`, but its constructor requires an `IActorRepository`. You didn't give me one.

You would need:

```csharp
new ActorService(
    ??? // IActorRepository
)
```

---

## But here's the important part

You **could** do this:

```csharp
public ActorsController()
{
    _actorService =
        new ActorService(
            new ActorRepository()
        );
}
```

Now the structure is:

```text
Controller
    ↓
new ActorService(
    new ActorRepository()
)
```

The service itself still uses:

```csharp
IActorRepository
```

So:

```text
Controller → tight
Service → abstraction
Repository → concrete implementation
```

This can compile and run.

But **DI is no longer responsible for constructing that dependency chain**, because the controller manually constructed it.

That's why you saw the error: you created the service manually but didn't provide the dependency its constructor requires.

---

# The key rule

There is **no rule saying all layers must be either tight or loose.**

You can absolutely have:

```text
Tight → Loose → Loose
```

or:

```text
Loose → Tight → Loose
```

or:

```text
Tight → Tight → Loose
```

etc.

But whenever you manually instantiate a class with `new`, **you must satisfy its constructor dependencies yourself.**

For example:

```csharp
new ActorService()
```

requires a parameterless constructor.

But:

```csharp
new ActorService(actorRepository)
```

requires you to provide an `IActorRepository`.

---

## In your specific project

Your original design:

```text
┌─────────────────┐
│ ActorsController│
└────────┬────────┘
         │ DI
         ▼
┌─────────────────┐
│  IActorService  │
└────────┬────────┘
         │ DI
         ▼
┌────────────────────┐
│ IActorRepository   │
└────────┬───────────┘
         │
         ▼
  ActorRepository
```

Your current demonstration:

```text
┌─────────────────┐
│ ActorsController│
│                 │
│ new Service()    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  ActorService   │
│                 │
│ new Repository() │
└────────┬────────┘
         │
         ▼
  ActorRepository
```

The **big conceptual difference** is:

> **With DI, the class receives its dependency. With tight coupling, the class creates its dependency.**

And there's a second distinction:

> **Loose coupling is primarily about depending on abstractions rather than concrete implementations.**

That's why interfaces and DI are related, but **they are not the same thing**.

For your mentor, that's the clean explanation I'd give.
