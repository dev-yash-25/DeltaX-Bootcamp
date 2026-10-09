# Tight Coupling, Loose Coupling and Dependency Injection

## Part 1 — What is Dependency Injection?

So, if you ask me what Dependency Injection is, I would say:

**Dependency Injection is a mechanism that allows a class to get the objects it needs from outside, instead of creating those objects by itself.**

<br>

## Part 2 — Why do we need DI?

Now, you might ask, why do we actually need DI?

The main reason  is that DI helps us **achieve** **`loose coupling`**, and also **makes** our code more **`testable`**.

If our app doesnt implement DI, then we would have tightly coupled classes insteead  

<br>

## Part 3 — What is Tight Coupling?

So then, what exactly is tight coupling?

In simple terms, tight coupling is when a class directly depends on a **concrete implementation** of the dependency.

Whereas in loose coupling, the class depends on an **abstraction, usually an interface**, instead of directly depending on a particular implementation.

###  Why is it a problem?
Tight coupling **increases the responsibility** of the class to decide which dependency to use and create that dependency itself.

This makes the class harder to change, replace, and test.

<br>

## Part 4 — Tight Coupling in Our Application

For example, in our application, let's take `ActorService`.

`ActorService` needs a Repository because after doing its business logic and validation, it needs to store or retrieve Actor data.

The main responsibility of `ActorService` is to perform the required business logic and use the operations it needs from the Repository, like Get, Add, Update and Delete.

It is not really the responsibility of `ActorService` to
- Decide which Repository implementation to use or
- Create that Repository itself.

<br>

But suppose we make it tightly coupled like this:

```csharp
public ActorService()
{
    _actorRepository = new ActorRepository();
}
```

Now the Service itself has to decide what Repository it wants.

For example, should it be `ActorRepositorySql`? Should it be `ActorRepositoryMongo`?

Then, after deciding that, it also has to create that Repository instance using `new`.

And finally, it uses the Repository methods it actually needs.

So now the Service has taken on extra responsibility.

It is not only doing its own business logic anymore. It is also deciding **which dependency to use and creating that dependency**.

That's what creates the tight coupling.

<br>

## Part 5 — Loose Coupling with DI

Now with loose coupling, we do this:

```csharp
public ActorService(IActorRepository actorRepository)
{
    _actorRepository = actorRepository;
}
```

Here, the Service is basically saying:

**"I need an `IActorRepository`. I don't care which concrete Repository implementation you give me, as long as it provides the operations that I need."**

So the Service doesn't have to decide whether it is SQL, MongoDB, or some other implementation.

It also doesn't have to create the Repository itself.

It just receives it and uses the operations that the interface guarantees, like Get, Add, Update and Delete.

<br>

## Part 6 — Why Does Loose Coupling Matter as the Application Grows?

Now, this becomes more useful when the application grows.

Initially, you might think:

**"Okay, if I have tight coupling and tomorrow I want to change SQL to MongoDB, I'll just change this one line."**

But imagine the application has hundreds of classes, and many of those classes are directly creating their dependencies.

Now if we decide to change:

```text
ActorRepositorySql → ActorRepositoryMongo
ProducerRepositorySql → ProducerRepositoryMongo
GenreRepositorySql → GenreRepositoryMongo
```

we would have to go through all those classes and change those hardcoded constructions.

<br>

But with loose coupling and DI, the classes depend on the interfaces, and the actual implementation is configured centrally.

For example:

```csharp
services.AddScoped<IActorRepository, ActorRepositorySql>();
```

If tomorrow we want MongoDB, we can change it to:

```csharp
services.AddScoped<IActorRepository, ActorRepositoryMongo>();
```

The `ActorService` doesn't need to change at all.

<br>

## Part 7 — What About Testing?

The same thing applies to testing.

If the Repository is hardcoded inside the Service, then during testing we'd have to go and replace things like:

```text
new ActorRepositorySql()
```

with some test Repository in all the places where they are hardcoded.

<br>

But with loose coupling, the Service only asks for:

```text
IActorRepository
```

So in the test, I can provide a mock implementation of that interface:

```text
Mock<IActorRepository>
```

The Service itself doesn't have to change.

<br>

## Part 8 — Final Understanding

So basically, the whole idea is:

**With tight coupling, the class decides what dependency to use and creates it itself.**

<br>

**With loose coupling, the class only defines what kind of dependency it needs through an interface, and the actual implementation is provided from outside.**

<br>

And **Dependency Injection is the mechanism that actually provides that dependency from outside.**
