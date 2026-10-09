
# Event Manager and his Pizza Delivery

Imagine you're an **event manager**.

The event manager's job is:

> "Make sure the guests get pizzas at the right time."

There are two designs.

## Tight coupling

The event manager does everything:

```text
Event Manager
     ↓
"I need pizzas"
     ↓
I find the ingredients
     ↓
I make the pizza
     ↓
I decide when it's ready
     ↓
I give it to guests
```

So the manager is responsible for:

1. Knowing that pizza is needed.
2. Creating/making the pizza.
3. Managing everything required to create it.
4. Deciding when to give it.
5. Giving it to guests.

The problem isn't that the manager **decides things**.

The problem is:

> **The manager has taken responsibility for creating the thing it depends on.**

<br>

# Loose coupling / DI

Now hire a pizza supplier.

```text
                Pizza Supplier
                     ↑
                     |
              "Give me pizza"
                     |
Event Manager ───────┘
     ↓
Decides when to give pizza
     ↓
Guests
```

The event manager now says:

> "I need pizzas. Someone else will provide them. I'll decide when to distribute them."

So:

```text
Event Manager
     │
     ├── decides WHEN to give pizza
     │
     └── uses pizza
          ↓
       Pizza Supplier
          ↓
       makes pizza
```

The manager **still has decisions**.

For example:

> "Guests arrive at 7 PM, so distribute pizzas at 7:30."

That's still the manager's responsibility.

What disappeared is:

> "I personally have to make the pizza."

<br>

---

<br>


# Now map this directly to your IMDB API

Your `ActorService` is the event manager.

Its job is:

> "Perform actor-related business operations."

It needs a repository to get/store actor data.

### Tight coupling

```csharp id="fydqpr"
public ActorService()
{
    _actorRepository = new ActorRepository();
}
```

Think:

```text
ActorService
     ↓
"I need actor data"
     ↓
"I'll create ActorRepository myself"
     ↓
ActorRepository
     ↓
Database
```

So `ActorService` has taken responsibility for **creating its dependency**.

<br>


### Loose coupling

```csharp id="b4kz5g"
public ActorService(IActorRepository actorRepository)
{
    _actorRepository = actorRepository;
}
```

Now:

```text
Outside / DI
     ↓
creates ActorRepository
     ↓
gives it to ActorService
     ↓
ActorService
     ↓
"I need actor 10"
     ↓
_actorRepository.Get(10)
```

`ActorService` doesn't have to think:

> "How do I construct an ActorRepository?"

It only thinks:

> "I need actor 10. I'll ask my repository."

<br>


# And this addresses your last point

You said:

> "There is no question about implementation. The goal is clear: make pizzas."

**Exactly — if you're using the analogy to explain responsibility.**

The event manager's goal is:

> **Get pizzas to guests.**

The repository's goal is:

> **Perform actor data access.**

The manager doesn't need to know:

```text
how the pizza is made
where flour comes from
how the oven works
```

Likewise:

```text
ActorService
doesn't need to know
how ActorRepository is constructed.
```

It just knows the **contract**:

> "I have something that can perform the actor repository operations."

<br>

> **DI is not about stopping the class from making decisions.**
>
> **DI is about removing the responsibility of creating/managing the things that the class depends on.**

<br>

> "Tight coupling = `new`; loose coupling = constructor injection."

`new` is just the **visible way the problem appears in your code**. The underlying issue is **who is responsible for creating the dependency**.
