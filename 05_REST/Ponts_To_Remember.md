# Points to Remember

<br>

## Index

1. [How to Choose which Controller?](#1-how-to-choose-which-controller)
2. [Choosing Route vs Query Parameter](#2-choosing-route-vs-query-parameter)
3. [Parent, Child & Junction Table](#3-parent-child--junction-table)
4. [Property vs Entity](#4-property-vs-entity)
5. [Using `_async`](#using-_async)
6. [`Task` in async](#task-in-async-programming)
7. [Captive Dependency](#di-lifetime-mismatch---captive-dependency)
8. [Order of exceotion Catch block](#exception-catch-block-order)
9. [IEnumerable vs List](#ienumerable-vs-list)
10. [Idempotency](#idempotency)

<br>

## Insights
1. Why we use `Scoped` instead of `Singleton` for db backed requests? Like we used singleton for repos where List was used

```
Since we want our application to be stateless, we don't want the server/repository to keep information 
from one HTTP request and use it in another request. Therefore, for a database-backed repository, we prefer Scoped.

Singleton can also give the same result when using SQL Server, but it keeps the same repository object
 alive for the entire application. We don't need that because the database already stores our data.

If we add something like List<Actor> _cache, then we intentionally want to keep some data between requests.
In that case, the cache can be a Singleton because the cache itself needs to share data. But we don't necessarily
 need to make the whole repository Singleton.

Stateless REST does not mean "the server cannot store any data."
It means the server should not depend on previous HTTP request state to understand the current request.
```


2. **A long-lived service should not hold a reference to a short-lived service.**
   
Eg:-  ❌ Problem
```js
services.AddSingleton<IMovieService, MovieService>();
services.AddScoped<IActorRepository, ActorRepository>(); 
```
✅ Works
```js
services.AddScoped<IMovieService, MovieService>();
services.AddSingleton<IActorRepository, ActorRepository>(); 
```
but, this is not a problem
```js
services.AddSingleton<IMovieService, MovieService>();
services.AddTransient<IActorRepository, ActorRepository>();
```
The transient dependency effectively lives as long as the Singleton in this situation.

> [!Important]
> Transient has **no fixed** lifetime; a new instance is created each time the DI container is asked for that service.
>
> it lives **as long as** the object using it.
> 
> So:
> - **Scoped** → normally one instance per request
> - **Transient** → new instance each time the container is requested to provide it
> - **Singleton** → one instance for the application's lifetime

```
Singleton → Scoped       ❌
Singleton → Transient    ✅
Singleton → Singleton    ✅

Scoped    → Singleton    ✅
Scoped    → Scoped       ✅
Scoped    → Transient    ✅

Transient → Singleton    ✅
Transient → Scoped       ✅
Transient → Transient    ✅
```

3. **Normal C# objects don't stay until app ends — they remain as long as they're referenced; once no references remain, they're eligible for Garbage Collection.**

<br>

## 1. How to Choose which controller?
Don't ask:
> "What is the first resource in the URL?"

Ask:
> "What resource is the endpoint actually operating on?"\
The Parent resorce just gives the context of the Child Resource

Example 1
```
GET /users/10/orders
What are we getting? ->  Orders.
→ OrdersController
```

<br>

---

<br>


## 2. Choosing Route vs Query Paramter
> Q. Why we use query params
> ```
> GET /api/tasks?status=pending&priority=high&dueDate..
> ```
> Instead of route paparam
> ```
> GET /api/tasks/status/priority/dueDate
> ```

* **Route parameter → identifies a specific resource / mandatory part of the resource**
* **Query parameter → filters, searches, sorts, or optionally modifies the result**

### Example: Tasks

**Specific task — route parameter:**

```http
GET /api/tasks/15
```
`15` is mandatory because it identifies **which task**.


**Filtering tasks — query parameters:**

```http
GET /api/tasks?status=pending&priority=high
```

The filters are optional.. not mandaatory


```http
GET /api/tasks?status=pending
```

```http
GET /api/tasks?priority=high
```
Even if we dont apply filters and simply do ->
```http
GET /api/tasks
```

It will return all tasks, filter just adds on convinience / limit

Now, why not we do
```http
GET /api/tasks/?id=15
```
We **can** do it

There is nothing technically wrong with it.

The difference is mainly about **API semantics and conventions**.\
and using the about syntax does not follow `REST Convention`

### Query parameter is meant for → `filtering a collection`

```http
GET /api/tasks?status=pending
```



### Route parameter is meant for → `identifying one resource`

```http
GET /api/tasks/15
```


### Why prefer `/tasks/15` for ID?

Because an ID usually **identifies a resource**, rather than merely filtering a collection.



<br>

---

<br>



## 3. Parent, Child & Junction Table

```http
POST /v1/carts/{cartId}/items
```

→ **ItemsController**

* We don't create `CartItemsController` just because `CartItems` is a junction table.
* `cartId` gives **parent context**; `items` is the resource being operated on.
* Junction tables are usually **relationship/DB implementation details**.

```text
Cart → CartItems → Item
 ↑                  ↑
Parent           Resource
```
Inside ItemsController, we could have operations such as:\
GetCartItems()\
AddItemToCart()\
RemoveItemFromCart()

> [!Important]

Don't decide the controller based on:

> "Which tables exist in the database?"

Instead ask:

> "What resource is the API operating on?"



<br>

---

<br>

### 4. Property vs Entity

```http
PATCH /v1/orders/{orderId}/status
```
**Controller → `OrdersController`**

Here, we are changing the **status of a specific Order**.

`Status` is a property/part of the Order being operated on.

```text
Order
 ├── Id
 ├── Status        ← being updated
 └── ...
```

Therefore:

```text
PATCH /v1/orders/{orderId}/ status
                    ↓
              OrdersController
```

We don't use:

```http
PATCH /v1/status
```

or create a `StatusController` simply because `Status` exists as a property/enum.

### When would we use `StatusController`?

When **Status itself is a separate entity/resource** that we are managing.

For example, if the database has:

```text
Status
----------------
Id
Name
Description
```

and we need to manage those Status records:

<br>

---

<br>
    

### Using `_async()`

#### Think of a waiter 🍽️
**Without async:**
> Waiter takes your order → stands beside the kitchen for 10 minutes → does nothing → brings food.

**With async:**
> Waiter takes your order → sends it to kitchen → serves another table → kitchen signals when food is ready → waiter comes back.

The food still takes 10 minutes.

`async` just means the waiter isn't standing uselessly beside the kitchen.

That's what "the thread is free" means.

#### Usage
- You don't use async on every method in an ASP.NET Core application.

- You generally use async when the method is performing an asynchronous operation, especially I/O such as `database calls`, `HTTP calls`, or `file operations`.

> CPU-only/simple calculation → synchronous is often fine.

> Waiting for DB/API/file/network → async is usually preferred.


**Q. But isnt await introducing synchronousness, and here we are using combination of async and await, so at the end the thread waits, so isn't it synchronous like fully**
> No. This is the key misconception to clear up:
>
> `await` does not make the thread wait. It makes the method's logical execution wait for the result, while the thread is released.


<br>

---

<br>

## Task in async programming

| Method | Meaning |
|---|---|
| `Task` | async operation returns no value |
| `Task<T>` | async operation eventually returns a `T` |
| `void` | synchronous method returns nothing |
| `T` | synchronous method returns a `T` |


```
Synchronous              Asynchronous

void                      Task
int                       Task<int>
Actor                     Task<Actor>
IEnumerable<Actor>        Task<IEnumerable<Actor>>
```

> [!Note]
> `async void` does exist, but it is generally avoided for normal methods. It's mainly appropriate for event handlers.

<br>

---

<br>


## DI Lifetime Mismatch - Captive Dependency

### Core Rule

> **A longer-lived object should not hold a shorter-lived dependency.**

Why? Because the longer-lived object can outlive the dependency it is holding.

### Example 1 — ❌ Fails

```csharp
services.AddScoped<IUserRepository, UserRepository>();
services.AddSingleton<IUserService, UserService>();
```

Dependency:

```text
UserService     →     UserRepository
Singleton             Scoped
```

```text
Application lifetime
└── UserService (Singleton)
        ↓
    UserRepository (Scoped)
        ↑
   Request lifetime
```

`UserService` can live from application start to application shutdown, while `UserRepository` is supposed to live only for one request.

So a **Singleton cannot depend directly on a Scoped service**.

The application can build successfully because the C# code itself is valid, but DI validation can fail when the application starts or when the dependency is resolved.

<br>

### Example 2 — ✅ Successful

```csharp
services.AddSingleton<IUserRepository, UserRepository>();
services.AddScoped<IUserService, UserService>();
```

Dependency:

```text
UserService     →     UserRepository
Scoped                Singleton
```

```text
Request lifetime
└── UserService (Scoped)
        ↓
Application lifetime
└── UserRepository (Singleton)
```

This is safe from a lifetime perspective because the `UserService` disappears at the end of the request, while the `UserRepository` continues to exist.

### Easy way to remember

```text
❌ Singleton → Scoped
   LONGER       SHORTER

✅ Scoped → Singleton
   SHORTER      LONGER
```


<br>

---

<br>


## Exception Catch Block Order 

### 1. Inheritance structure

A custom exception is itself an `Exception` because it inherits from the base `Exception` class:

```csharp
class CustomException : Exception
{
}
```

So:

```text
Exception              ← Base / Parent
    ↑
CustomException        ← Derived / Child
```

Therefore:

> **A `CustomException` IS an `Exception`.**

<br>

### 2. Why order matters

C# checks `catch` blocks **from top to bottom** and stops at the first matching catch.

### ✅ Correct

```csharp
try
{
    // code
}
catch (CustomException ex)
{
    // specifically handle custom exception
}
catch (Exception ex)
{
    // handle all other exceptions
}
```

### ❌ Wrong

```csharp
try
{
    // code
}
catch (Exception ex)
{
    // catches CustomException too
}
catch (CustomException ex)
{
    // Never reached
}
```

If:

```csharp
throw new CustomException();
```

then:

```text
CustomException thrown
       ↓
catch (Exception)
       ↓
MATCH ✅  <- program reads custom exception as a exception
       ↓
Handled
       ↓
CustomException catch is never reached
```

Because:

```text
CustomException IS-A Exception
```

<br>

### 3. Catch priority

> **Put the most specific/derived exception first, and the most general/base exception last.**

```text
Most specific
     ↓
CustomException
     ↓
More general exceptions
     ↓
Exception
     ↓
Most general
```

Example:

```csharp
catch (EntityNotFoundException)
{
}
catch (ValidationException)
{
}
catch (Exception)
{
}
```

The general `Exception` catch should be **last** because it can catch almost every exception derived from `Exception`.


<br>

---

<br>


## IEnumerable vs List

<br>
<div align  = "center">
 <img width="500" alt="image" src="https://github.com/user-attachments/assets/066af8db-859a-4621-b2cd-24b7102a1081" />
</div>
<br>

### 1. List implements multiple interfaces

```text
List<T>
 ├── IEnumerable<T>
 ├── IEnumerable
 ├── ICollection<T>
 ├── IList<T>
 └── ...
```

So:

> **`List<T>` is an `IEnumerable<T>`**, but `IEnumerable<T>` is not necessarily a `List<T>`.

<br>

### 2. List → IEnumerable ✅

```csharp
List<int> list = new List<int>();

IEnumerable<int> items = list;
```

This works because `List<T>` implements `IEnumerable<T>`.

```text
List
 ↓
IEnumerable
```

A `List` can therefore be treated as the more general `IEnumerable`.

<br>

### 3. IEnumerable → List ❌ Direct assignment

```csharp
IEnumerable<int> items = ...;

List<int> list = items; // ❌
```

Why?

Because the actual object behind `IEnumerable` could be:

```text
IEnumerable
 ├── List
 ├── Array
 ├── HashSet
 └── Other collection
```

So C# cannot assume that it is a `List`.

<br>

### 4. IEnumerable → List using `.ToList()` ✅

```csharp
List<int> list = items.ToList();
```

`.ToList()` creates a **new `List<T>`** from the enumerable.

```text
IEnumerable
     ↓
  .ToList()
     ↓
 New List
```

### Core rule

> **Specific → General: direct assignment works.**

```text
List → IEnumerable ✅
```

> **General → Specific: direct assignment does not work; conversion is needed.**

```text
IEnumerable → List ❌
IEnumerable → ToList() → List ✅

IEnumerable<int>  IEnum = new List<int>(); ✅
List<int>  List = new IEnumerable<int>(); ❌
```

<br>

---

<br>

### Idempotency

A request is **idempotent** if sending the **same request multiple times has the same final effect as sending it once**.

Think:

> **1 request or 10 identical requests → same final state.**

### HTTP methods

| Method | Idempotent? | Example |
|---|---|---|
| **GET** | ✅ | `GET /movies/5` → just reads movie |
| **PUT** | ⚠️ depends | `PUT /movies/5` with same data → movie ends up with same data |
| **DELETE** | ✅ | `DELETE /movies/5` → first deletes it; repeating it doesn't delete another movie |
| **POST** | ❌ generally | `POST /movies` → each request can create another movie |
| **PATCH** | ⚠️ depends | `PATCH /movies/5` → depends on what the patch operation does |

### Easy example

```http
PUT /users/10
{
    "name": "Yash"
}
```

Send it once:

```text
User 10 → Yash
```

Send it 5 times:

```text
User 10 → Yash
```

Final state is the same → **idempotent**.

But:

```http
POST /movies
{
    "name": "Inception"
}
```

Send it 5 times:

```text
Movie 1 → Inception
Movie 2 → Inception
Movie 3 → Inception
...
```

Different final state → **not idempotent**.


<br>

### PATCH can be either Idempotent or non Idempotent

It depends on **what operation the PATCH performs**.

#### ❌ Non-idempotent PATCH

Suppose:

```http
PATCH /users/10
{
    "operation": "incrementAge"
}
```

Initial:

```text
Age = 20
```

Send once:

```text
20 → 21
```

Send again:

```text
21 → 22
```

So repeated requests keep changing the state.

 **Not idempotent**

<br>

#### ✅ Idempotent PATCH

Suppose:

```http
PATCH /users/10
{
    "name": "Yash"
}
```

Initial:

```text
Name = Rahul
```

Send once or multiple times:

```text
Rahul → Yash
```

The final state is the same.

**Idempotent**

### Easy way to remember

```text
PATCH "set this value"       → usually idempotent ✅
PATCH "do this operation"   → can be non-idempotent ❌
```
