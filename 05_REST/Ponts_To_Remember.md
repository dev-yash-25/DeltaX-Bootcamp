# Points to Remember

<br>

## Index

1. [How to Choose which Controller?](#1-how-to-choose-which-controller)
2. [Choosing Route vs Query Parameter](#2-choosing-route-vs-query-parameter)
3. [Parent, Child & Junction Table](#3-parent-child--junction-table)
4. [Property vs Entity](#4-property-vs-entity)
5. [Using `_async`](#using-_async)


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
You **can** do:

```http
GET /api/tasks?id=15
```

There is nothing technically wrong with it.

The distinction is mainly about **API semantics and conventions**.\
and using the about syntax does not follow `REST Convention`

### Query parameter is meant for → `filtering a collection`

```http
GET /api/tasks?status=pending
```



### Path parameter is meant for → `identifying one resource`

```http
GET /api/tasks/15
```


### Why prefer `/tasks/15` for ID?

Because an ID usually **identifies a resource**, rather than merely filtering a collection.





So:

```text
/api/tasks/{taskId}              → mandatory identity
/api/tasks?status=pending        → optional filter
```

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

- You don't use async on every method in an ASP.NET Core application.

- You generally use async when the method is performing an asynchronous operation, especially I/O such as `database calls`, `HTTP calls`, or `file operations`.

> CPU-only/simple calculation → synchronous is often fine.

> Waiting for DB/API/file/network → async is usually preferred.
