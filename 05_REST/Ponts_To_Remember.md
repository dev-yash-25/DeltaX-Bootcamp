# Points to Remember

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
GET /api/tasks
```

```http
GET /api/tasks?status=pending
```

```http
GET /api/tasks?priority=high
```

Think
```text
Route parameter
    ↓
"WHICH specific resource?"

Query parameter
    ↓
"WHAT resource FILTER / OPTIONS do I want?"
```

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


---

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
