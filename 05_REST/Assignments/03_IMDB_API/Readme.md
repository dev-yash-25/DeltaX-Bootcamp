1. We are not having `Update Id` feature, and also remember we dont pass Id in request object, but we require a Id for update, so we pass id as a parameter through update() method
2.  Registering a service in Startup.cs means adding its registration to ASP.NET Core's Dependency Injection (DI) container.\
    Example :-
    ```csharp
    services.AddAutoMapper(typeof(MappingProfile));
    ```
    **This tells ASP.NET Core:** "Add AutoMapper to the application's Dependency Injection container, and load the mappings from MappingProfile."



Skip very lasts, ask this to GPT
```
In order to ensure isolation (like of transaction) we create scoped instances 
transient for very small utiility changes like calcu, date

IMP

How many instance created per req, 

Give examples for each, 
what if this instance was singleton/scoped/transient

Lets go in depth!

What is scope here, is it a session..

frame all these questions first..give ..in sequence 
, before i ask u to explain each
```
<pre>

Yes. Let's structure **DI Lifetimes in ASP.NET Core** as a sequence of questions, from basic mental model → request lifecycle → each lifetime → real scenarios → mistakes.

## DI Lifetimes — Question Sequence

### 1. First understand the unit of lifetime

1. What exactly is an **instance**?
2. What does **lifetime** mean in Dependency Injection?
3. What is a **scope**?
4. Is a scope the same as a **session**?
5. What creates a scope?
6. In ASP.NET Core, is **one HTTP request = one scope**?
7. Can one request contain multiple scopes?
8. Can multiple requests share the same scoped instance?

---

### 2. The core question — instance count

For a service:

```csharp
services.AddSingleton<IService, Service>();
services.AddScoped<IService, Service>();
services.AddTransient<IService, Service>();
```

Ask:

9. **How many instances are created per application?**
10. **How many instances are created per request?**
11. What happens if the same service is injected into **two controllers/services in the same request**?
12. Will they receive the same instance or different instances?
13. What happens across **two different requests**?
14. When exactly is an instance created?
15. When exactly is it destroyed/disposed?

---

# 3. Singleton

16. What does Singleton actually mean?

17. How many instances exist?

```text
Application
    └── 1 Singleton instance
```

18. What happens if 1000 requests use it simultaneously?

19. What kind of data/state is safe inside a Singleton?

20. Why should Singleton generally be **stateless** or thread-safe?

21. When is Singleton preferred?

22. Examples:

* Cache
* Configuration
* Application-wide reference data
* Shared expensive resource

23. What happens if this service were **Scoped instead of Singleton**?

24. What happens if this service were **Transient instead of Singleton**?

25. What problems occur if a Singleton stores request/user-specific data?

26. Why can't a Singleton normally depend on a Scoped service?

---

# 4. Scoped

27. What does Scoped actually mean?

28. What exactly is the **scope boundary** in ASP.NET Core?

29. Is:

```text
1 HTTP Request = 1 Scope
```

always true?

30. How many instances are created?

For:

```csharp
services.AddScoped<IMovieService, MovieService>();
```

with:

```text
Request 1
Request 2
Request 3
```

31. What happens if MovieService is injected into **MovieController + another service in the same request**?

32. Do they get the same MovieService instance?

33. What happens on the next HTTP request?

34. Why is Scoped commonly used for:

```text
Controller
   ↓
Service
   ↓
Repository
   ↓
DbContext
```

35. How does Scoped help with **request/transaction isolation**?

36. Is a Scoped instance automatically a database transaction?

37. What is the difference between:

```text
Scope
Transaction
Session
```

38. What happens if this service were **Singleton instead of Scoped**?

39. What happens if this service were **Transient instead of Scoped**?

40. Why is `DbContext` normally Scoped?

---

# 5. Transient

41. What does Transient actually mean?

42. How many instances can be created during **one request**?

43. If three classes inject the same transient service:

```text
Controller ──┐
Service ─────┼──> IUtility
Repository ──┘
```

do they receive the same instance?

44. What happens on another request?

45. When is Transient preferred?

46. Why are small/stateless utilities good candidates?

Examples:

```text
Calculator
DateFormatter
StringFormatter
Mapper-like utility
Small validator
```

47. What happens if this service were **Scoped instead of Transient**?

48. What happens if it were **Singleton instead of Transient**?

49. Why can creating too many transient instances become inefficient?

---

# 6. The most important comparison

For this:

```csharp
public class MyService
{
    ...
}
```

compare:

### Singleton

```text
Application
     │
     └── MyService #1
          ↑
   Request 1, 2, 3, 4...
```

### Scoped

```text
Request 1 → MyService #1
Request 2 → MyService #2
Request 3 → MyService #3
```

### Transient

```text
Request 1
 ├── MyService #1
 ├── MyService #2
 └── MyService #3

Request 2
 ├── MyService #4
 └── MyService #5
```

Then ask:

50. **Why does Transient potentially create multiple instances within ONE request while Scoped doesn't?**

---

# 7. Real ASP.NET Core example

Take:

```text
MoviesController
      ↓
MovieService
      ↓
MovieRepository
      ↓
DbContext
```

Ask:

51. What lifetime should each have?

52. What happens if everything is Singleton?

53. What happens if everything is Scoped?

54. What happens if everything is Transient?

55. Why shouldn't we simply use one lifetime everywhere?

---

# 8. Your assignment — fake List<T> repository

For your current assignment:

```csharp
private readonly List<Actor> _actors;
```

Ask:

56. What happens if ActorRepository is Scoped?

57. What happens if ActorRepository is Singleton?

58. What happens if ActorRepository is Transient?

59. Why does Scoped cause your fake list to reset between requests?

60. Why can Singleton make sense **specifically for this fake repository**?

61. Would we use Singleton Repository if this were an actual database?

---

# 9. Dependency rules

62. Can Singleton depend on Scoped?

63. Can Scoped depend on Singleton?

64. Can Transient depend on Scoped?

65. Can Scoped depend on Transient?

66. Can Singleton depend on Transient?

67. What is the **captive dependency** problem?

---

# 10. Disposal

68. Who disposes a Singleton?

69. Who disposes a Scoped service?

70. Who disposes a Transient service?

71. What happens when a request ends?

72. Why does this matter for:

```text
DbContext
Database connections
Streams
HttpClient-related resources
```

---

# 11. Finally — decision-making

For any new class, ask:

```text
1. Does it contain application-wide shared state?
        ↓
      Singleton?

2. Does it represent request-specific state/context?
        ↓
      Scoped?

3. Is it small, stateless and cheap to create?
        ↓
      Transient?
```

Then we'll test this reasoning against **10–15 real classes** and deliberately change their lifetime to see **what breaks and why**.

That sequence will give you the actual mental model rather than just memorizing:

> Singleton = one
> Scoped = one per request
> Transient = every time.

</pre>
