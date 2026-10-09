

### 1. DI + Loose Coupling ✅

```csharp
public ActorService(IActorRepository repo)
{
    _repo = repo;
}
```

- Dependency comes from outside → **DI**
- Depends on interface → **loose coupling**

**Responsibility:**
- Here Service has 1 Responsibility to Just Execute methods from repository

**This is our preferred design.**

---

### 2. DI but NOT Loose Coupling ⚠️

```csharp
public ActorService(ActorRepository repo)
{
    _repo = repo;
}
```

- Dependency comes from outside → **DI**
- Depends on concrete `ActorRepository` → **tight coupling**

**Responsibility:**
- Here Service has 1 Responsibility to  Execute methods from repository
- But has concrete dependency

So:

> **DI is being used, but loose coupling is not fully achieved.**

---

### 3. No DI + Tight Coupling ❌

```csharp
public ActorService()
{
    _repo = new ActorRepository();
}
```

- Class creates dependency itself → **no DI**
- Depends on concrete class → **tight coupling**

**Responsibility**
- First responsibility is to Choose which Dependency `ActorRepoSql`, `ActorRepoMongo`..
- Second is to create with `new`
- Third Responsibility to Just Execute methods from repository

This is the classic tight-coupling example.

---

### 4. No DI + Loose Coupling

Technically possible, but uncommon/not useful in this form.

For example, if the class internally creates an implementation through some factory:

```csharp
_repo = RepositoryFactory.Create<IActorRepository>();
```

The class may depend on the interface, but it is still **responsible for obtaining/creating its dependency**. That's why DI is generally preferred: the dependency creation responsibility is moved outside.

<br>

---

<br>

