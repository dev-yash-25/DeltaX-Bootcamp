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




