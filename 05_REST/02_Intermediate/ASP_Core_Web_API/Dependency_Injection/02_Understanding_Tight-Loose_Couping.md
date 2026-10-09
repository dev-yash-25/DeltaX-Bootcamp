# Tight Coupling, Loose Coupling and Dependency Injection

We are considering an example of a **Service using a Repository to store its data**.

Let’s consider `ActorService`, which needs a Repository to store its final, validated data.

<br>

# Part 1 — Prerequisites / Understanding

First of all, we need to be clear about the responsibilities of the Service and Repository.

### Service's job

The Service's job is to **perform business logic on the data and make it ready to be passed to the Repository for storing**.

It doesn't care about:

- which database is being used
- how the data is actually stored
- how the database query is written

Its job is simply:

> **Perform the required business logic and give the Repository the proper data in the format it requires.**

### Repository's job

The Repository's job is to **receive clean, validated, transformed, logic-applied data from the Service and store it in the database**.

Its job is simply:

> **Take the data from the Service and perform the required database operation.**

The Repository could implement queries for:

- SQL Server
- MongoDB
- PostgreSQL
- or any other database

The Service should not need to care about that.

<br>

### Service has a dependency on the Repository

So, in our example:

> **ActorService depends on a Repository to store and retrieve Actor data.**

The responsibilities are clear.

The important thing we will now look at is **who makes the decisions about that dependency and who creates it**.

### Dependency 

In our example, the dependency is:

> **The Repository that ActorService uses to store/retrieve Actor data.**

The Service needs the Repository because it needs operations such as:

```text
Get
Add
Update
Delete
```

The Repository provides those operations and is responsible for actually performing the data-access work.

<br>

### Interface

The Interface acts as a **contract / promise** that the required operations are available.

For example:

```csharp
public interface IActorRepository
{
    Task<IEnumerable<Actor>> Get();
    Task<Actor> Get(int id);
    Task<int> Add(Actor actor);
    Task Update(int id, Actor actor);
    Task Delete(int id);
}
```

This means:

> "If you give me an implementation of `IActorRepository`, I guarantee that these required operations are available."

So `ActorService` doesn't need to know exactly which Repository implementation it has received. It only needs to know that the implementation follows the `IActorRepository` contract.

<br>

---

<br>

# Part 2 — Tight Coupling

### Definition

> **Tight coupling means a class depends directly on a concrete implementation and is responsible for deciding which dependency to use and creating it itself.**

Now consider:

```csharp
public ActorService()
{
    _actorRepository = new ActorRepository();
}
```

Here, `ActorService` itself is deciding:

> "I need a Repository, so I will create one."

There is no abstraction being used to provide the dependency from outside.

The `ActorService` now has to take care of multiple things.

### First — Decide which dependency to choose

It has to decide which concrete Repository it wants:

```text
ActorRepositorySql
ActorRepositoryMongo
ActorRepositoryPostgreSQL
...
```

For example:

```csharp
_actorRepository = new ActorRepositorySql();
```

### Second — Create the dependency

It then has to create the actual Repository instance:

```csharp
new ActorRepositorySql();
```

### Third — Use the Repository

Only after that does it perform its actual Service work by using the Repository's operations:

```text
Get
Add
Update
Delete
```

So `ActorService` has control over:

> **Dependency → Choice → Creation → Usage**

That is why this is **tight coupling**.

The Service is strongly dependent on the specific concrete Repository implementation that it has chosen and created.

<br>

---

<br>

# Part 3 — Loose Coupling and Dependency Injection

### Definition

> **Loose coupling means the class depends on an abstraction/interface and receives its dependency from outside instead of creating the concrete dependency itself.**

Now we change our Service to:

```csharp
public ActorService(IActorRepository actorRepository)
{
    _actorRepository = actorRepository;
}
```

Now something important has changed.

`ActorService` is **receiving the dependency from outside**.

The dependency is still the Repository.

The difference is that `ActorService` is no longer responsible for deciding which concrete Repository to create.

<br>

### The Interface sits in between

Here:

```text
ActorService
     ↓
IActorRepository
     ↓
Concrete Repository
```

The interface is sitting between the Service and the actual implementation.

It guarantees:

> "Whatever implementation is provided to you will have the operations you require."

For example:

```text
Add
Get
Update
Delete
```

So `ActorService` doesn't need to create an instance itself.

It is essentially saying:

> "I need an `IActorRepository`. I don't need to know how you create it or which implementation you use. Just provide me something that follows this contract."

<br>

### What responsibility does ActorService have now?

The Service only needs to focus on its actual job.

It can simply execute the operations it requires:

```text
Add
Get
Update
Delete
```

It does **not** need to decide:

```text
Should I use ActorRepositorySql?
Should I use ActorRepositoryMongo?
Should I use another implementation?
```

That responsibility is no longer inside `ActorService`.

It also doesn't need to create the Repository using:

```csharp
new ActorRepository();
```

So the responsibility of **creating the dependency** is also removed.

The Service simply receives the dependency and uses it.

<br>

---

<br>

# Part 4 — Why Do We Need Loose Coupling?

At first, you may think:

> "For now, I only have to make a small hardcoded change inside the class. Why is this such a big deal?"

The real benefit becomes clear when we think about a **large application**.

Imagine that the application has grown and now contains hundreds of classes.

One day, you have two requirements:

1. Change the Repository implementation from SQL to MongoDB.
2. Perform testing using test/mock Repositories.

Let's see what happens in both cases.

<br>

## Without DI — Tight Coupling

### Changing the implementation

If every class creates its own concrete Repository, you would have to visit all those classes and change the construction.

For example:

```text
new ActorRepositorySql()
        ↓
new ActorRepositoryMongo()

new ProducerRepositorySql()
        ↓
new ProducerRepositoryMongo()

new GenreRepositorySql()
        ↓
new GenreRepositoryMongo()

...
```

And this continues across all the classes that directly create their concrete dependencies.

Even if it is only **one line in each class**, you still have to find and change all those places.

<br>

---

<br>

### Testing

Now suppose you want to test the Services.

Again, the Services are directly creating the real Repositories:

```text
new ActorRepositorySql()
        ↓
new ActorRepositoryTest()

new ProducerRepositorySql()
        ↓
new ProducerRepositoryTest()

...
```

Again, you have to modify the construction inside all those classes.

So the burden exists in both situations:

```text
Changing implementation
        +
Testing
        ↓
Many classes need modification
```

The bigger the application becomes, the more painful this becomes.

<br>

## With Loose Coupling and DI

With DI, the concrete Repository implementations are registered in one central place — our Startup/DI configuration.

For example:

```csharp
services.AddScoped<IActorRepository, ActorRepositorySql>();
```

Now suppose tomorrow we want to use MongoDB instead.

We change the registration to:

```csharp
services.AddScoped<IActorRepository, ActorRepositoryMongo>();
```

That's it.

`ActorService` itself does not change.

It still remains:

```csharp
public ActorService(IActorRepository actorRepository)
{
    _actorRepository = actorRepository;
}
```

The Service doesn't care which concrete implementation is provided.

<br>

### Changing the implementation

Instead of changing the Repository construction in hundreds of classes:

```text
new ActorRepositorySql()
        ↓
new ActorRepositoryMongo()
```

we change the registration:

```csharp
services.AddScoped<IActorRepository, ActorRepositorySql>();
```

to:

```csharp
services.AddScoped<IActorRepository, ActorRepositoryMongo>();
```

The implementation decision is now centralized in the DI configuration instead of being spread across all the classes that use the dependency.

<br>

### Testing

The same idea helps during testing.

Inside a test, we can provide a mock/test implementation of the same interface:

```csharp
Mock<IActorRepository>
```

and inject it into:

```csharp
ActorService
```

The `ActorService` itself does not need to be changed.

We simply provide a different implementation of the same contract.

So we don't have to go through all the classes and replace:

```text
new ActorRepositorySql()
```

with:

```text
new ActorRepositoryTest()
```

<br>

---

<br>

# Final Understanding

The whole difference can be understood like this.

### Tight Coupling

```text
ActorService

    ↓
"I decide which Repository I need"

    ↓
"I create it"

    ↓
new ActorRepositorySql()

    ↓
"I use it"
```

The Service has control over the **dependency, its choice, and its creation**.

### Loose Coupling + DI

```text
             DI / Outside
                  |
                  ↓
        ActorRepositorySql
                  |
                  ↓
         IActorRepository
                  |
                  ↓
            ActorService
```

The Service simply says:

> "I need an `IActorRepository`."

It doesn't decide:

> "I will use SQL."

It doesn't create:

```csharp
new ActorRepositorySql();
```

It simply receives the dependency and uses the operations guaranteed by the interface.

So the responsibility of **choosing and creating the dependency is moved outside the class**.

That is the core idea behind **loose coupling and Dependency Injection**.
