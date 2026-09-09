# Status Code 2XX - 200, 201, 202 & 204 


> [!Tip]
> **When asked: What is the difference between Created/Accepted — `Plain`, `By Action`, and `By Route`?**
>
> The difference is **how the `Location` URL is provided/generated**.
>
> * **Plain** → We provide the **entire location URL ourselves**.
>
>   ```csharp
>   return Created(
>       location: $"/api/Animals/{animal.Id}",
>       value: animal
>   );
>   ```
>
> * **By Action** → We tell ASP.NET to **identify/generate the location using an action**.
>
>   ```csharp
>   return CreatedAtAction(
>       actionName: nameof(GetAnimal),
>       routeValues: new { id = animal.Id },
>       value: animal
>   );
>   ```
>
>   Meaning:
>
>   > "Find the URL belonging to the `GetAnimal` action."
>
> * **By Route** → We tell ASP.NET to **identify/generate the location using a named route**.
>
>   ```csharp
>   return CreatedAtRoute(
>       routeName: "GetAnimalById",
>       routeValues: new { id = animal.Id },
>       value: animal
>   );
>   ```

> [!Note]
> Location = the URL of the resource/endpoint that the client should go to next.
>
> To access the Posted- Created,accepted resource

<br>

## Index

1. [HTTP 200 — OK](#1-http-200--ok)
2. [HTTP 201 — Created](#2-http-201--created)\
   2.1. [Created() — Plain](#21-created--plain)\
   2.2. [CreatedAtAction() — By Action](#22-createdataction--by-action)\
   2.3. [CreatedAtRoute() — By Route Name](#23-createdatroute--by-route-name)
3. [HTTP 202 — Accepted](#3-http-202--accepted)\
   3.1. [Accepted() — Plain](#31-accepted--plain)\
   3.2. [AcceptedAtAction() — By Action](#32-acceptedataction--by-action)\
   3.3. [AcceptedAtRoute() — By Route Name](#33-acceptedatroute--by-route-name)
4. [AtAction vs AtRoute](#ataction-vs-atroute)
5. [HTTP 204 — No Content](#4-http-204--no-content)
6. [Final Parameter Cheat Sheet](#final-parameter-cheat-sheet)
7. [Final Memory Trick](#final-memory-trick)
8. [Your Named Route](#your-named-route)

<br>

---

<br>

## 1. HTTP 200 — OK

### Purpose

`200 OK` means:

> The request was successfully processed and the server is returning a response.

### Return examples

```csharp
return Ok();
```

```csharp
return Ok(value: animal);
```

```csharp
return Ok(value: animals);
```

### Response

```http
200 OK
```

with a response body when a value is provided.

### Parameters

| Parameter | Required?  | Meaning       |
| --------- | ---------- | ------------- |
| `value`   | ❌ Optional | Response body |

### Mental model

```text
Ok()
 ↓
200 OK
 ↓
Optionally return a value
```

<br>

---

<br>

# 2. HTTP 201 — Created

`201 Created` means:

> A new resource was successfully created.

For example:

```http
POST /api/Animals
```

creates:

```json
{
  "id": 4,
  "name": "Elephant"
}
```

<br>



## 2.1 `Created()` — Plain

### Meaning

You provide the URL yourself.

### Return

```csharp
return Created(
    location: $"/api/Animals/{animal.Id}",
    value: animal
);
```

Here:

```text
location = "/api/Animals/4"
value    = animal
```

### Response

```http
201 Created
Location: /api/Animals/4
```

Body:

```json
{
  "id": 4,
  "name": "Elephant"
}
```

### Parameters

| Parameter  | Required?  | Meaning                     |
| ---------- | ---------- | --------------------------- |
| `location` | ✅ Required | URL of the created resource |
| `value`    | ✅ Required | Response body               |

### Mental model

```text
Created()
   ↓
I provide the URL myself
   ↓
location: "/api/Animals/4"
```

<br>



# 2.2 `CreatedAtAction()` — By Action

### Meaning

Instead of manually providing the URL:

> Find the URL using an action method.

### Return

```csharp
return CreatedAtAction(
    actionName: nameof(GetAnimal),
    routeValues: new { id = animal.Id },
    value: animal
);
```

ASP.NET finds:

```text
actionName = GetAnimal
routeValues = id = 4
        ↓
/api/Animals/4
```

### You can also specify the controller

```csharp
return CreatedAtAction(
    actionName: nameof(GetAnimal),
    controllerName: "Animals",
    routeValues: new { id = animal.Id },
    value: animal
);
```

### Parameters

| Parameter        | Required?   | Meaning                          |
| ---------------- | ----------- | -------------------------------- |
| `actionName`     | ✅ Required  | Action to use for URL generation |
| `controllerName` | ❌ Optional  | Controller containing the action |
| `routeValues`    | ❌ Optional* | Values required by the route     |
| `value`          | ❌ Optional  | Response body                    |

> **Important:** `routeValues` is technically optional, but your route contains `{id}`. Therefore you need to provide `id` to generate `/api/Animals/4`.

### Mental model

```text
CreatedAtAction()
       ↓
actionName: GetAnimal
       ↓
routeValues: id = 4
       ↓
ASP.NET generates:
       ↓
/api/Animals/4
```

<br>


# 2.3 `CreatedAtRoute()` — By Route Name

### Meaning

Instead of finding the action:

> Find the URL using the route's name.

Your route is:

```csharp
[HttpGet("{id}", Name = "GetAnimalById")]
public IActionResult GetAnimal(int id)
{
    ...
}
```

The route name is:

```text
GetAnimalById
```

### Return

```csharp
return CreatedAtRoute(
    routeName: "GetAnimalById",
    routeValues: new { id = animal.Id },
    value: animal
);
```

ASP.NET does:

```text
routeName: GetAnimalById
       ↓
route: /api/Animals/{id}
       ↓
routeValues: id = 4
       ↓
/api/Animals/4
```

### Parameters

| Parameter     | Required?   | Meaning                      |
| ------------- | ----------- | ---------------------------- |
| `routeName`   | ✅ Required  | Name of the route            |
| `routeValues` | ❌ Optional* | Values required by the route |
| `value`       | ❌ Optional  | Response body                |

> **Important:** `routeValues` is technically optional, but your route contains `{id}`, so you need `new { id = animal.Id }` to generate the correct URL.

### Mental model

```text
CreatedAtRoute()
       ↓
routeName: GetAnimalById
       ↓
routeValues: id = 4
       ↓
ASP.NET generates:
       ↓
/api/Animals/4
```

<br>

---

<br>


# 3. HTTP 202 — Accepted

`202 Accepted` means:

> The server accepted the request, but processing may not be completed yet.

<br>




## 3.1 `Accepted()` — Plain

### Return

```csharp
return Accepted();
```

### With a location

```csharp
return Accepted(
    location: "/api/jobs/123"
);
```

### With location and response body

```csharp
return Accepted(
    location: "/api/jobs/123",
    value: new { message = "Processing started" }
);
```

### Parameters

| Parameter  | Required?  | Meaning                             |
| ---------- | ---------- | ----------------------------------- |
| `location` | ❌ Optional | URL related to the accepted request |
| `value`    | ❌ Optional | Response body                       |

### Mental model

```text
Accepted()
     ↓
202 Accepted
```

No parameters are required.

<br>



# 3.2 `AcceptedAtAction()` — By Action

### Return

```csharp
return AcceptedAtAction(
    actionName: nameof(GetAnimal),
    routeValues: new { id = 1 },
    value: null
);
```

Or, if you want to specify the controller:

```csharp
return AcceptedAtAction(
    actionName: nameof(GetAnimal),
    controllerName: "Animals",
    routeValues: new { id = 1 },
    value: null
);
```

ASP.NET finds:

```text
actionName: GetAnimal
routeValues: id = 1
       ↓
/api/Animals/1
```

### Response

```http
202 Accepted
Location: /api/Animals/1
```

### Parameters

| Parameter        | Required?   | Meaning                          |
| ---------------- | ----------- | -------------------------------- |
| `actionName`     | ✅ Required  | Action to use for URL generation |
| `controllerName` | ❌ Optional  | Controller containing the action |
| `routeValues`    | ❌ Optional* | Values required by the route     |
| `value`          | ❌ Optional  | Response body                    |

<br>


# 3.3 `AcceptedAtRoute()` — By Route Name

### Return

```csharp
return AcceptedAtRoute(
    routeName: "GetAnimalById",
    routeValues: new { id = 1 }
);
```

Or with a response body:

```csharp
return AcceptedAtRoute(
    routeName: "GetAnimalById",
    routeValues: new { id = 1 },
    value: new { message = "Animal processing started" }
);
```

ASP.NET does:

```text
routeName: GetAnimalById
       ↓
routeValues: id = 1
       ↓
/api/Animals/1
```

### Response

```http
202 Accepted
Location: /api/Animals/1
```

### Parameters

| Parameter     | Required?   | Meaning                      |
| ------------- | ----------- | ---------------------------- |
| `routeName`   | ✅ Required  | Name of the route            |
| `routeValues` | ❌ Optional* | Values required by the route |
| `value`       | ❌ Optional  | Response body                |

<br>

---

<br>


#  `AtAction` vs `AtRoute`

Your endpoint:

```csharp
[HttpGet("{id}", Name = "GetAnimalById")]
public IActionResult GetAnimal(int id)
{
    ...
}
```

has two different identities:

```text
C# ACTION NAME
      ↓
GetAnimal


ROUTE NAME
      ↓
GetAnimalById
```

Therefore:

### `AtAction`

```csharp
return CreatedAtAction(
    actionName: nameof(GetAnimal),
    routeValues: new { id = animal.Id },
    value: animal
);
```

Means:

> Find the URL by **action name**.

<br>


### `AtRoute`

```csharp
return CreatedAtRoute(
    routeName: "GetAnimalById",
    routeValues: new { id = animal.Id },
    value: animal
);
```

Means:

> Find the URL by **route name**.

<br>

---

<br>


# 4. HTTP 204 — No Content

`204 No Content` means:

> The request was successfully processed, but there is no response body to return.

### Return

```csharp
return NoContent();
```

### Your DELETE example

```csharp
[HttpDelete("{id}")]
public IActionResult DeleteAnimal(int id)
{
    var animal = animals.FirstOrDefault(x => x.Id == id);

    if (animal == null)
        return NotFound();

    animals.Remove(animal);

    return NoContent();
}
```

### Response

```http
204 No Content
```

There is no response body.

### Parameters

| Method        | Required | Optional |
| ------------- | -------- | -------- |
| `NoContent()` | None     | None     |

### Mental model

```text
NoContent()
     ↓
204 No Content
     ↓
No response body
```

<br>

---

<br>


# Final Parameter Cheat Sheet

| Method               | Required parameters | Optional parameters                      |
| -------------------- | ------------------- | ---------------------------------------- |
| `Ok()`               | None                | `value`                                  |
| `Created()`          | `location`, `value` | —                                        |
| `CreatedAtAction()`  | `actionName`        | `controllerName`, `routeValues`, `value` |
| `CreatedAtRoute()`   | `routeName`         | `routeValues`, `value`                   |
| `Accepted()`         | None                | `location`, `value`                      |
| `AcceptedAtAction()` | `actionName`        | `controllerName`, `routeValues`, `value` |
| `AcceptedAtRoute()`  | `routeName`         | `routeValues`, `value`                   |
| `NoContent()`        | None                | None                                     |

### Important note about `routeValues`

```text
Technically optional
        +
Route contains {id}
        ↓
You need to provide id
```

For example:

```csharp
routeValues: new { id = animal.Id }
```

<br>

---

<br>


# Final Memory Trick

```text
200 → OK
      ↓
      Success + result


201 → Created
      ↓
      Created()
          → YOU provide URL

      CreatedAtAction()
          → Find URL by ACTION

      CreatedAtRoute()
          → Find URL by ROUTE NAME


202 → Accepted
      ↓
      Request accepted

      Accepted()
          → Simple 202

      AcceptedAtAction()
          → Find Location by ACTION

      AcceptedAtRoute()
          → Find Location by ROUTE NAME


204 → NoContent
      ↓
      Success + no response body
```

## Your named route

```csharp
[HttpGet("{id}", Name = "GetAnimalById")]
public IActionResult GetAnimal(int id)
```

means:

```text
Action name = GetAnimal
Route name  = GetAnimalById
URL         = /api/Animals/{id}
```

So:

```csharp
CreatedAtAction(
    actionName: nameof(GetAnimal),
    routeValues: new { id = animal.Id },
    value: animal
);
```

→ **Find by action**

while:

```csharp
CreatedAtRoute(
    routeName: "GetAnimalById",
    routeValues: new { id = animal.Id },
    value: animal
);
```

→ **Find by route name**

<br>



# Example Controller Code
```csharp
/*
 | Thing                 | Name                |
| --------------------- | ------------------- |
| C# action/method name | `GetAnimal`         |
| Route name            | `GetAnimalById`     |
| URL                   | `/api/Animals/{id}` |

 */

using ConsoleAppone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAppone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        // Temporary data — no database
        private static List<Animal> animals = new()
        {
            new Animal { Id = 1, Name = "Dog" },
            new Animal { Id = 2, Name = "Cat" },
            new Animal { Id = 3, Name = "Lion" }
        };


        // =====================================================
        // 1. 200 OK
        // =====================================================

        [HttpGet("")]
        public IActionResult GetAnimals()
        {
            return Ok(animals);
        }

        /*
         URL -  https://localhost:64428/api/animals

         Response -
        [
          {
            "id": 1,
            "name": "Dog"
          },
          {
            "id": 2,
            "name": "Cat"
          },
          {
            "id": 3,
            "name": "Lion"
          }
        ]
         */


        // =====================================================
        // 2. 202 Accepted
        // =====================================================

        [HttpGet("accepted")]
        public IActionResult TestAccepted()
        {
            return Accepted();
        }

        /*
         You will have - Request URL in Inspect, 
          No Location Header
         */


        // =====================================================
        // 3. 202 AcceptedAtAction
        /*
         * AcceptedAction and AcceptedRoute return the same response Header Location
         The difference is only how ASP.NET finds that URL.

            AcceptedAtAction - You point to the action method

            AcceptedAtRoute - You point to the route name
         */
        // =====================================================

        // TEST 202 AcceptedAtAction
        [HttpGet("accepted-action")]
        public IActionResult TestAcceptedAtAction()
        {
            return AcceptedAtAction(
                actionName: nameof(GetAnimal),
                controllerName: "Animals",
                routeValues: new { id = 1 },
                value: null
            );
        }


        /*
         You will have - Request URL in Inspect, 
          And also Location Header- https://localhost:64428/api/Animals/1

        "Find the URL belonging to GetAnimal
         with id = 1"

         */


        // =====================================================
        // 4. 202 AcceptedAtRoute
        // =====================================================

        [HttpGet("accepted-route")]
        public IActionResult TestAcceptedAtRoute()
        {
            return AcceptedAtRoute(
                "GetAnimalById",
                new { id = 1 }
            );
        }

        /*
         You will have - Request URL in Inspect, 
          And also Location Header- https://localhost:64428/api/Animals/1

        "Find the URL belonging to the route
         named GetAnimalById with id = 1"

         */


        // =====================================================
        // This endpoint will be used by
        // AcceptedAtAction / AcceptedAtRoute
        // =====================================================

        [HttpGet("{id}", Name = "GetAnimalById")]
        public IActionResult GetAnimal(int id)
        {
            var animal = animals.FirstOrDefault(x => x.Id == id);

            if (animal == null)
                return NotFound();

            return Ok(animal);
        }


        // =====================================================
        // 7. 201 Created
        // =====================================================

        [HttpPost("")]
        public IActionResult CreateAnimal(Animal animal)
        {
            animal.Id = animals.Count + 1;

            animals.Add(animal);

            return Created(
                $"https://localhost:64428/api/Animals/{animal.Id}", // you provide location yourself
                animal
            );
        }

        /*
         URL - https://localhost:64428/api/animals

         Request Body -
         {
           "name": "Elephant"
         }

         Response -
         201 Created

         Location Header -
         https://localhost:64428/api/Animals/4

         Body -
         {
           "id": 4,
           "name": "Elephant"
         }

         "I created the resource.
          Here is the URL of the new resource."
         */


        // =====================================================
        // 8. 201 CreatedAtAction
        // =====================================================

        [HttpPost("created-action")]
        public IActionResult CreateAnimalAtAction(Animal animal)
        {
            animal.Id = animals.Count + 1;

            animals.Add(animal);

            return CreatedAtAction(
                nameof(GetAnimal),
                new { id = animal.Id },
                animal
            );
        }

        /*
         URL - https://localhost:64428/api/animals/created-action

         Request Body -
         {
           "name": "Elephant"
         }

         Response -
         201 Created

         Location Header -
         https://localhost:64428/api/Animals/4

         Body -
         {
           "id": 4,
           "name": "Elephant"
         }

         "I created the resource.
          Find the URL using the GetAnimal action."
         */


        // =====================================================
        // 9. 201 CreatedAtRoute
        // =====================================================

        [HttpPost("created-route")]
        public IActionResult CreateAnimalAtRoute(Animal animal)
        {
            animal.Id = animals.Count + 1;

            animals.Add(animal);

            return CreatedAtRoute(
                "GetAnimalById",
                new { id = animal.Id },
                animal
            );
        }

        /*
         URL - https://localhost:64428/api/animals/created-route

         Request Body -
         {
           "name": "Elephant"
         }

         Response -
         201 Created

         Location Header -
         https://localhost:64428/api/Animals/4

         Body -
         {
           "id": 4,
           "name": "Elephant"
         }

         "I created the resource.
          Find the URL using the route named GetAnimalById."
         */
      

        // =====================================================
        // 8. 204 No Content
        // =====================================================

        [HttpDelete("{id}")]
        public IActionResult DeleteAnimal(int id)
        {
            var animal = animals.FirstOrDefault(x => x.Id == id);

            if (animal == null)
                return NotFound();

            animals.Remove(animal);

            return NoContent();
        }

        /*
         URL - https://localhost:64428/api/animals/1

         Response -
         204 No Content

         No response body
         */
    }
}
```
Model
```csharp
namespace ConsoleAppone.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
```
