# ASP.NET Core Web API — Important 3xx Notes

## Index

1. [**301 — Moved Permanently**](#1-301--moved-permanently)
2. [**302 — Found / Temporary Redirect**](#2-302--found--temporary-redirect)
3. **Quick Revision**

<br>

---

<br>



## 1. 301 — Moved Permanently

Used when the resource has **permanently moved**.

### Main Methods

```csharp
RedirectPermanent(
    location: "/api/Animals/1"
);
```

```csharp
RedirectToActionPermanent(
    actionName: nameof(GetAnimal),
    routeValues: new { id = 1 }
);
```

```csharp
RedirectToRoutePermanent(
    routeName: "GetAnimalById",
    routeValues: new { id = 1 }
);
```

### Parameters

| Method                        | Required     | Optional                                    |
| ----------------------------- | ------------ | ------------------------------------------- |
| `RedirectPermanent()`         | `location`   | `preserveMethod`                            |
| `RedirectToActionPermanent()` | `actionName` | `controllerName`, `routeValues`, `fragment` |
| `RedirectToRoutePermanent()`  | `routeName*` | `routeValues`, `fragment`                   |

`*` `routeName` is optional at the method-signature level, but normally provided when using a named route.

<br>

---

<br>



## 2. 302 — Found / Temporary Redirect

Used for a **temporary redirect**.

### Main Methods

```csharp
Redirect(
    location: "/api/Animals/1"
);
```

```csharp
RedirectToAction(
    actionName: nameof(GetAnimal),
    routeValues: new { id = 1 }
);
```

```csharp
RedirectToRoute(
    routeName: "GetAnimalById",
    routeValues: new { id = 1 }
);
```

### Parameters

| Method               | Required     | Optional                                    |
| -------------------- | ------------ | ------------------------------------------- |
| `Redirect()`         | `location`   | `preserveMethod`                            |
| `RedirectToAction()` | `actionName` | `controllerName`, `routeValues`, `fragment` |
| `RedirectToRoute()`  | `routeName*` | `routeValues`, `fragment`                   |

<br>





#  Quick Revision Table

| Status  | Plain                 | By Action                     | By Route                     |
| ------- | --------------------- | ----------------------------- | ---------------------------- |
| **301** | `RedirectPermanent()` | `RedirectToActionPermanent()` | `RedirectToRoutePermanent()` |
| **302** | `Redirect()`          | `RedirectToAction()`          | `RedirectToRoute()`          |

<br>

---

<br>


## Parameter Cheat Sheet

### 301

#### `RedirectPermanent()`

```text
location       → Required
preserveMethod → Optional
```

#### `RedirectToActionPermanent()`

```text
actionName     → Required
controllerName → Optional
routeValues    → Optional*
fragment       → Optional
```

#### `RedirectToRoutePermanent()`

```text
routeName      → Optional at signature level
routeValues    → Optional*
fragment       → Optional
```

<br>


### 302

#### `Redirect()`

```text
location       → Required
preserveMethod → Optional
```

#### `RedirectToAction()`

```text
actionName     → Required
controllerName → Optional
routeValues    → Optional*
fragment       → Optional
```

#### `RedirectToRoute()`

```text
routeName      → Optional at signature level
routeValues    → Optional*
fragment       → Optional
```

`*` = If the route contains a placeholder such as `{id}`, you need `routeValues` to provide that value.

<br>

---

<br>

# 8. Memory Trick

Remember the **two dimensions**:

### First: Which status?

```text
Permanent → 301
Temporary → 302
```

### Second: How do I specify the destination?

```text
URL       → Plain
Action    → By Action
Route     → By Route
```

Therefore:

```text
301
├── RedirectPermanent()
├── RedirectToActionPermanent()
└── RedirectToRoutePermanent()

302
├── Redirect()
├── RedirectToAction()
└── RedirectToRoute()
```

### One-line memory

> **Permanent = `Permanent` suffix → 301**
> **No `Permanent` suffix → 302**
> **Action = action name, Route = route name, Plain = URL**
