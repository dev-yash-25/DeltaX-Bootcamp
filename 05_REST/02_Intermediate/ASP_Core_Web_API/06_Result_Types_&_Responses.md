# Specific Type

## Index

- [1. Returning Data](#1-returning-data)
- [2. Complex Types & Models](#2-complex-types--models)
- [3. `IActionResult`](#3-iactionresult)
- [4. Why Use `IActionResult`?](#4-why-use-iactionresult)
- [5. Example](#5-example)

<br>

---

<br>

## 1. Returning Data

Action methods can return:

- **Primitive types** — `string`, `int`, etc.
- **Complex types** — custom objects/classes.

<br>

---

<br>

## 2. Complex Types & Models

For more advanced scenarios, define custom data structures as **Models**.

A common approach is to create a dedicated `Models` folder to organize these classes.

```text
Models/
└── Employee.cs
```

<br>

---

<br>

## 3. `IActionResult`

`IActionResult` is an interface used when an action method may need to return **different types of HTTP responses**.

For example, the same action can return:

- `Ok()` → **200 OK**
- `NotFound()` → **404 Not Found**

This is useful when the result depends on conditions.

<br>

---

<br>

## 4. Why Use `IActionResult`?

With a fixed return type such as:

```csharp
public List<Employee> GetEmployees()
```

the method is expected to return that type.

But with:

```csharp
public IActionResult GetEmployee(int id)
```

the method can return different results:

```csharp
if (id == 0)
{
    return NotFound();
}

return Ok(employee);
```

### Key Benefit

`IActionResult` gives flexibility to explicitly communicate different **HTTP status codes** from the same action.

> [!Important]
> Use `IActionResult` when an action can have multiple response outcomes, such as success or not-found.

<br>

---

<br>

## 5. Example

```csharp
public IActionResult GetEmployee(int id)
{
    if (id == 0)
    {
        return NotFound();
    }

    return Ok(employee);
}
```

Here:

- `id == 0` → `NotFound()` → **404**
- Otherwise → `Ok(employee)` → **200**

### Observation

`IActionResult` is flexible, but the return type itself does not immediately tell you what data type will be returned.

> **`IActionResult` allows a controller action to return multiple response types/status codes based on the logic.**

<br>

---

<br>
