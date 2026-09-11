# Model Binding

## Index

- [1. What is Model Binding?](#1-what-is-model-binding)
- [2. Data Sources for Model Binding](#2-data-sources-for-model-binding)
- [3. Core Concept: Model Binder](#3-core-concept-model-binder)
- [4. `[BindProperty]` Attribute](#4-bindproperty-attribute)
   - [Why we Need BindProperty?](#why-we-need-bindproperty)
- [5. `[BindProperties]` Attribute](#5-bindproperties-attribute)
- [6. Default Model Binding Rules](#6-default-model-binding-rules)
- [7. `[FromQuery]` Attribute](#7-fromquery-attribute)
- [8. `[FromRoute]` Attribute](#8-fromroute-attribute)
- [9. `[FromBody]` Attribute](#9-frombody-attribute)
- [10. `[FromForm]` Attribute](#10-fromform-attribute)
- [11. `[FromHeader]` Attribute](#11-fromheader-attribute)
- [12. Custom Model Binder](#12-custom-model-binder)
  - [12.1 Example 1 — Transform Query Data](#121-example-1--transform-query-data)
  - [12.2 Example 2 — Bind a Complex Object](#122-example-2--bind-a-complex-object)
- [13. Model Binding Mental Model](#13-model-binding-mental-model)

<br>

---

## 1. What is Model Binding?

When an **HTTP request** sends data from a client to the server, that data needs to be mapped to **server-side .NET types**, such as action parameters or controller properties.

**Model Binding** is the process of binding **HTTP request data** to the parameters of application controllers or their properties.

### Model Binder

The **Model Binder** is the mechanism responsible for this mapping.

```text
HTTP Request Data
       ↓
  Model Binder
       ↓
Controller Parameters / Properties
```

The binder:

1. Receives data from the HTTP request.
2. Identifies the names of parameters/properties.
3. Converts incoming values to the required .NET types.
4. Supplies the resulting values to the controller/action.

<br>

<div align="center">
<img width="600" alt="Model Binding" src="https://github.com/user-attachments/assets/15dc0385-b593-4e28-a64e-11f0dcd14d72" />
</div>

<br>

### Important Observations

- ASP.NET Core provides many built-in methods and attributes for model binding.
- If built-in behavior is insufficient, a **Custom Model Binder** can be created.
- A custom binder allows specialized logic before data reaches the action.

<br>

<div align="center">
<img width="500" alt="Model Binder" src="https://github.com/user-attachments/assets/b18abee3-4d97-4de1-b202-abdeb20284af" />
</div>

<br>

> [!Note]
> The data passes through the **Model Binder before reaching the controller/action method**. This can be observed using the debugger.


<br>

---

<br>


## 2. Data Sources for Model Binding

Data can be sent through different parts of an HTTP request:

| Data source | Typical parameter type | Example | Query | Passing method |
|---|---|---|---|---|
| **Route** | Primitive/simple type | `int taskId` | `GET /api/tasks/10` | `public IActionResult GetTask(int taskId)` |
| **Query** | Primitive/simple type | `string status` | `GET /api/tasks?status=pending` | `public IActionResult GetTasks(string status)` |
| **Query** | Complex type/model | `Country country` | `GET /api/countries/USA?Name=India&Area=PCMC&Population=150000` | `public IActionResult GetCountry(string code, Country country)` |
| **Body (JSON)** | Complex type/model | `Task task` | `POST /api/tasks` + JSON body | `public IActionResult AddTask(Task task)` |

### Examples

#### Route

```csharp
public IActionResult GetTask(int taskId)
{
    return Ok(taskId);
}
```

```text
GET /api/tasks/10
```

#### Query

```csharp
public IActionResult GetTasks(string status)
{
    return Ok(status);
}
```

```text
GET /api/tasks?status=pending
```

#### Query + Complex Type

```csharp
public IActionResult GetCountry(
    string code,
    Country country)
{
    return Ok(country);
}
```

```text
GET /api/countries/USA?Name=India&Area=PCMC&Population=150000
```

#### Body (JSON)

```csharp
public IActionResult AddTask(Task task)
{
    return Ok(task);
}
```

```text
POST /api/tasks
```

```json
{
    "title": "Learn ASP.NET",
    "status": "pending"
}
```

<br>

> [!Important]
> `[BindProperty]` / `[BindProperties]` are used for binding **form-data to controller properties**. By default, they do not support HTTP GET. Use `SupportsGet = true` when GET support is required.

<br>

---

<br>


## 3. Core Concept: Model Binder

### The Problem

The client sends data through an HTTP request, but the server works with .NET types.

```text
HTTP:
population=10000
```

The application may require:

```csharp
int population
```

The Model Binder handles this mapping and conversion.

### How Data is Received

ASP.NET Core can receive data from:

1. **URL / Route**
2. **Query String**
3. **Headers**
4. **Body**
5. **Form Data**

### How Model Binding Works

```text
Incoming HTTP Request
        │
        ├── Route
        ├── Query String
        ├── Headers
        ├── Body
        └── Form Data
                │
                ▼
          Model Binder
                │
                ▼
     .NET Parameters / Properties
                │
                ▼
        Controller Action
```

<br>

---

<br>


## 4. `[BindProperty]` Attribute

`[BindProperty]` enables model binding for a **public property inside a controller**.

<br>

## Why We need `BindProperty`?

Model binding is automatic. [BindProperty] is not required just because you're using model binding. 
It is used when you specifically want binding to a controller/page property
Instead of `Action Paramter` which is default.

### 1. Without `[BindProperty]` — Action parameter

```csharp
public class CountriesController : ControllerBase
{
    [HttpPost]
    public IActionResult AddCountry(Country country)
    {
        return Ok(country.Name);
    }
}
```

Request:

```json
{
    "name": "India"
}
```

Binding:

```text
JSON → country action parameter
```

<br>


### 2. With `[BindProperty]` — Controller property

```csharp
public class CountriesController : ControllerBase
{
    [BindProperty]
    public Country Country { get; set; }

    [HttpPost]
    public IActionResult AddCountry()
    {
        return Ok(Country.Name);
    }
}
```

Same request:

```json
{
    "name": "India"
}
```

Binding:

```text
JSON → Country controller property
```

### Key difference

```text
Action parameter:
AddCountry(Country country)
             ↑
       data goes here


Controller property:
[BindProperty]
Country Country
       ↑
 data goes here
```

> [!tip]
> For **Web API controllers**, prefer the **action parameter** approach in most cases. `[BindProperty]` is much more commonly useful in **Razor Pages**.

<br>


### Purpose

Incoming form-data can be mapped directly to a controller property instead of being received as an action parameter.

```csharp
[BindProperty]
public Country country { get; set; }
```


> [!Important]
> `[BindProperty]` works with **form-data**.
>
> By default:
> - POST → supported
> - GET → not supported
>
> For GET:
>
> ```csharp
> [BindProperty(SupportsGet = true)]
> ```

### POST Example

```csharp
[Route("api/[controller]")]
[ApiController]

public class CountriesController : ControllerBase
{
    [BindProperty]
    public Country country { get; set; }

    [HttpPost("")]
    public IActionResult AddCountry()
    {
        return Ok($"Name : {this.country.Name}," +
            $" Population : {this.country.Population}," +
            $"   Area : {this.country.Area}");
    }
}
```

Postman should use:

```text
Body → form-data
```

Example:

```text
Name        India
Population  150000
Area        PCMC
```

### GET Example

```csharp
[Route("api/[controller]")]
[ApiController]

public class CountriesController : ControllerBase
{
    [BindProperty(SupportsGet =true)]
    public Country country { get; set; }

    [HttpGet("")]
    public IActionResult AddCountry()
    {
      ..
    }
}
```

### Multiple Properties

Each property that should use this binding mechanism needs `[BindProperty]`.

```csharp
[BindProperty]
public string Name { get; set; }

[BindProperty]
public int Population { get; set; }
```

### Complex Types

```csharp
[BindProperty]
public CountryModel country { get; set; }
```

### Troubleshooting

#### 204 No Content

Possible reasons:

- `[BindProperty]` was omitted.
- Postman was not configured for `form-data`.
- Form-data keys do not match the expected property names.

#### `NullReferenceException`

An unbound property can be `null`, for example when using `[BindProperty]` during GET without `SupportsGet = true`.

<br>

---

<br>


## 5. `[BindProperties]` Attribute

`[BindProperties]` is the controller-level version of `[BindProperty]`.

| Attribute | Applied to |
|---|---|
| `[BindProperty]` | Individual property |
| `[BindProperties]` | Controller |

### Purpose

It eliminates the need to put `[BindProperty]` on every individual property.

### POST Example

```csharp
[Route("api/[controller]")]
[ApiController]
[BindProperties]

public class CountriesController : ControllerBase
{
    public Country country { get; set; }

    [HttpPost("")]
    public IActionResult AddCountry()
    {
    }
}
```

### GET Example

By default, `[BindProperties]` does not support GET.

```csharp
[Route("api/[controller]")]
[ApiController]
[BindProperties(SupportsGet =true)]

public class CountriesController : ControllerBase
{
    public Country country { get; set; }

    [HttpGet("")]
    public IActionResult AddCountry()
    {
    }
}
```

### Key Points

- Applied at **controller level**.
- Works with simple and complex objects.
- Applies binding to controller public properties.
- GET requires `SupportsGet = true`.

> [!Tip]
> Use `[BindProperties]` when several controller properties need the same binding behavior.

<br>

---

<br>


## 6. Default Model Binding Rules

When no explicit binding attribute is used, ASP.NET Core follows default model-binding behavior.

### 6.1 Primitive / Simple Types

For simple types such as `int`, `float`, `string`, and `char`, the model binder automatically looks for data in the **URL**.

```csharp
[HttpGet("{name}/{area}/{population}")]
//                                     Simple/Primitive Types
//                                             |
//                                             V
public IActionResult AddCountry(string name, string area, int population)
{
    return Ok($"Name = {name}, Area = {area}, Population = {population}");
}
```

Request:

```text
https://localhost:64428/api/countries/india/pune/10000
```

Output:

```text
Name = india, Area = pune, Population = 10000
```

<img width="470" alt="Model binding primitive types" src="https://github.com/user-attachments/assets/76ba80ed-412f-4bac-8cac-445ff90e9053" />

### 6.2 Complex Types

For complex types, such as custom classes/models, the model binder defaults to looking inside the **request body**.

```csharp
[HttpPost("")]
//                                     Complex Type - Object Model
//                                             |
//                                             V
public IActionResult AddCountry(Country country)
{
    return Ok($"Name = {country.Name}, Population = {country.Population}, Area = {country.Area}");
}
```

Request:

```text
https://localhost:64428/api/countries
```

JSON:

```json
{
    "Name" : "Ind",
    "Population" : "122",
    "Area" : "test"
}
```

The same endpoint can also receive matching query parameters:

```text
https://localhost:64428/api/countries?Name=China&Population=19999&Area=xin
```

<img width="450" alt="Model binding complex types" src="https://github.com/user-attachments/assets/6914b04c-7686-44be-9dd9-2dccfad3af00" />

### 6.3 Query String Binding

Query string:

```text
/api/countries?Name=China&Population=19999&Area=xin
```

The binder matches query-string keys with parameter/property names.

**Order does not matter.**

### 6.4 Route Binding

Route:

```text
/api/countries/india/pune/10000
```

The binder matches route values with parameter names.

### Automatic Type Conversion

URL values arrive as strings, but the model binder converts them to the target .NET type.

```text
"10000" → int 10000
```

If conversion fails, the application can return:

```text
400 Bad Request
```

### Exact Name Matching

The binder matches incoming names with parameter/property names.

```csharp
public IActionResult AddCountry(string name)
```

Example:

```text
?name=India
```

> [!Important]
> If you need to explicitly control the source of a value, use binding attributes such as `[FromQuery]`, `[FromRoute]`, `[FromBody]`, `[FromForm]`, or `[FromHeader]`.
>
> Means, when you decide, "The source should come from either Query only, or Body only etc, at a time, even if data is fed from different sources at a single time (Query + Route + Property)


<br>

---

<br>


## 7. `[FromQuery]` Attribute

`[FromQuery]` explicitly tells ASP.NET Core to bind a parameter from the **query string**.

### Basic Example

```csharp
[HttpPost("")]
public IActionResult AddCountry([FromQuery]string name)
{
    return Ok($"Name = {name}");
}
```

Request:

```text
POST /api/countries?name=India
```

### Complex Types

```csharp
public IActionResult GetCountry([FromQuery]Country country)
{
    return Ok(country);
}
```

Query:

```text
/api/countries?Name=India&Population=150000&Area=PCMC
```

### Why Use `[FromQuery]`?

It is useful when:

- You explicitly want query-string binding.
- Data may exist in multiple request locations.
- You want predictable binding behavior.
- You want to prevent accidental binding from another source.

### Multiple Parameters

```csharp
public IActionResult AddCountry(
    [FromQuery]string name,
    [FromQuery]int population)
{
    return Ok();
}
```

### Missing Values

If a query parameter is not supplied, the parameter receives its default value according to its type.

<br>

---

<br>


## 8. `[FromRoute]` Attribute

`[FromRoute]` explicitly binds data from the **route**.

### Purpose

It tells ASP.NET Core that a parameter must come from route data.

### Difference from `[FromQuery]`

| Attribute | Source |
|---|---|
| `[FromQuery]` | Query string |
| `[FromRoute]` | Route |

### Complex Type

```csharp
[HttpPost("{name}/{area}/{population}")]
public IActionResult AddCountry([FromRoute]Country country, [FromQuery] int id)
{
    return Ok($"Name = {country.Name}");
}
```

Here:

```text
Route → Country
Query → id
```

### Multiple Binding Sources

```csharp
public IActionResult AddCountry(
    [FromRoute]string name,
    [FromQuery]int id)
{
    return Ok();
}
```

> [!Important]
> `[FromRoute]` is useful when you want to enforce that a particular input must originate from the URL route.

<br>

---

<br>


## 9. `[FromBody]` Attribute

`[FromBody]` explicitly tells ASP.NET Core to bind data from the **request body**.

### Basic Example

```csharp
public IActionResult AddCountry([FromBody]Country country)
{
    return Ok(country);
}
```

Request body:

```json
{
    "Name": "India",
    "Population": 150000,
    "Area": "PCMC"
}
```

### Complex Models

```csharp
public IActionResult AddCountry([FromBody]Country country)
{
    return Ok($"Name = {country.Name}");
}
```

### Mixing Sources

```csharp
public IActionResult UpdateCountry(
    [FromRoute]int id,
    [FromBody]Country country)
{
    return Ok();
}
```

Request:

```text
PUT /api/countries/10
```

```json
{
    "Name": "India",
    "Population": 150000,
    "Area": "PCMC"
}
```

Common pattern:

```text
Route → ID
Body  → Resource Data
```

> [!Tip]
> Think of `[FromRoute]` as identifying **which resource** to operate on and `[FromBody]` as carrying **the resource data**.

<br>

---

<br>


## 10. `[FromForm]` Attribute

`[FromForm]` explicitly binds data from **form-data**.

### Basic Usage

```csharp
public IActionResult AddCountry([FromForm]Country country)
{
    return Ok(country);
}
```

### Postman

```text
Body → form-data
```

Example:

```text
Key          Value
---------------------------
Name         India
Population   150000
Area         PCMC
```

The binder maps the keys to the corresponding model properties.

### Partial Binding

Only properties whose matching keys are provided will be populated.

If only:

```text
Name = India
```

is sent, then `country.Name` is populated while other properties may remain at their default values.

### Combining Sources

```csharp
public IActionResult AddCountry(
    [FromRoute]int id,
    [FromForm]Country country)
{
    return Ok();
}
```

Here:

```text
Route → id
Form  → country
```

<br>

---

<br>


## 11. `[FromHeader]` Attribute

`[FromHeader]` binds data from the **HTTP request headers**.

### Basic Usage

```csharp
public IActionResult GetDeveloper([FromHeader]string developer)
{
    return Ok(developer);
}
```

Request header:

```text
developer: Yash
```

### Explicit Header Name

```csharp
public IActionResult GetDeveloper(
    [FromHeader(Name = "developer")] string developer)
{
    return Ok(developer);
}
```

### Multiple Headers

```csharp
public IActionResult GetData(
    [FromHeader]string developer,
    [FromHeader]string version)
{
    return Ok();
}
```

### Combining Binding Sources

```csharp
public IActionResult GetData(
    [FromRoute]int id,
    [FromQuery]string name,
    [FromHeader]string developer)
{
    return Ok();
}
```

> [!Note]
> `[FromHeader]` is useful for custom request metadata or values intentionally carried in HTTP headers.

<br>

---

<br>


## 12. Custom Model Binder

Built-in model binding is sufficient for most standard scenarios.

Sometimes incoming data needs **custom manipulation or transformation** before it can be used by an action method.

In such cases, create a **Custom Model Binder**.

### Why Custom Model Binding?

Example:

```text
countries=India|China|USA
```

But the action expects:

```csharp
string[] countries
```

The custom binder can:

```text
"India|China|USA"
        ↓
Split by "|"
        ↓
["India", "China", "USA"]
        ↓
Action parameter
```

### Steps

1. Create a binder class.
2. Implement `IModelBinder`.
3. Implement `BindModelAsync`.
4. Read incoming request data.
5. Perform custom transformation/logic.
6. Set the binding result.
7. Apply `[ModelBinder]` to the parameter or model.

### Important Interface

```csharp
using Microsoft.AspNetCore.Mvc.ModelBinding;
```

The main method is:

```csharp
BindModelAsync(ModelBindingContext bindingContext)
```

### `BindingContext`

`bindingContext` provides access to request/binding information.

For example:

```csharp
bindingContext.HttpContext.Request
```

can access query strings, headers, body, and other request information.

The result is supplied using:

```csharp
bindingContext.Result =
    ModelBindingResult.Success(yourProcessedData);
```

> [!Important]
> The custom binder runs **before the controller action receives the parameter**.

<br>

---

<br>


### 12.1 Example 1 — Transform Query Data

#### Custom Binder

```csharp
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ConsoleAppone
{
    public class CustomBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var data = bindingContext.HttpContext.Request.Query;

            //                           name of query string
            //                                |
            //                                V
            var result = data.TryGetValue("countries", out var country);

            if (result)
            {
                var array = country.ToString().Split('|');

                bindingContext.Result = ModelBindingResult.Success(array);
            }

            return Task.CompletedTask;
        }
    }
}
```

#### Controller

```csharp
using ConsoleAppone.Models;

namespace ConsoleAppone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        [HttpGet("search")]
        public IActionResult SearchCountries([ModelBinder(typeof(CustomBinder))]string[] countries)
        {
            return Ok(countries);
        }
    }
}
```

#### Request

```text
GET /api/countries/search?countries=India|China|USA
```

### Binding Flow

```text
countries=India|China|USA
            ↓
       CustomBinder
            ↓
      Split by "|"
            ↓
["India", "China", "USA"]
            ↓
string[] countries
            ↓
SearchCountries()
```

<br>

---

<br>


### 12.2 Example 2 — Bind a Complex Object

The goal is to accept a simple request containing an `id` and have the model binder retrieve and bind a complete `Country` object.

### Flow

```text
Request
  ↓
id
  ↓
Custom Binder
  ↓
Convert string → int
  ↓
Retrieve Country
  ↓
ModelBindingResult.Success(model)
  ↓
Controller Action
```

### Steps

#### 1. Action Method

The action accepts a `Country` model:

```csharp
public IActionResult CountryDetails(Country country)
{
    return Ok(country);
}
```

#### 2. Retrieve the Value

Use:

```csharp
bindingContext.ValueProvider.GetValue("id")
```

#### 3. Convert the ID

The incoming value is a string, so convert it using:

```csharp
int.TryParse(...)
```

#### 4. Retrieve the Object

The parsed ID can be used to retrieve the full object.

The tutorial uses hardcoded data as a placeholder for a database call.

#### 5. Set the Binding Result

```csharp
bindingContext.Result =
    ModelBindingResult.Success(model);
```

### Associate Binder with Model

A custom binder can be associated with the model:

```csharp
[ModelBinder(BinderType = typeof(CustomBinderName))]
```

Or applied to the action parameter:

```csharp
public IActionResult CountryDetails(
    [ModelBinder(typeof(CustomBinderName))] Country country)
{
    return Ok(country);
}
```

### Debugging

Place a breakpoint inside:

```csharp
BindModelAsync()
```

Inspect:

```csharp
bindingContext
```

and its `ValueProvider`.

> [!Important]
> In a production application, replace hardcoded retrieval with **dependency injection + service/database access**.

<br>

---

<br>


## 13. Model Binding Mental Model

```text
CLIENT
  │
  │ HTTP Request
  ▼
┌─────────────────────────────┐
│       Request Data          │
│                             │
│ Route   → /countries/10     │
│ Query   → ?name=India       │
│ Header  → developer:Yash    │
│ Body    → { ... }           │
│ Form    → key/value         │
└──────────────┬──────────────┘
               │
               ▼
        ┌──────────────┐
        │ Model Binder │
        └──────┬───────┘
               │
               ▼
      .NET Parameters /
         Properties
               │
               ▼
       Controller Action
```

### Binding Attribute Cheat Sheet

| Attribute | Source |
|---|---|
| `[FromQuery]` | Query string |
| `[FromRoute]` | Route |
| `[FromBody]` | Request body |
| `[FromForm]` | Form-data |
| `[FromHeader]` | HTTP headers |
| `[BindProperty]` | Form-data → controller property |
| `[BindProperties]` | Form-data → controller properties |
| `[ModelBinder]` | Custom binding logic |

### Key Mental Model

> **Model Binding = HTTP Request Data → .NET Object / Parameter**

```text
Route / Query / Header / Body / Form
                  ↓
            Model Binding
                  ↓
        .NET Model / Parameter
                  ↓
             Controller
```

> [!Important]
> Use the default model binder when ASP.NET Core can naturally map the request data. Use `[From...]` attributes when you need explicit source control, and use a **custom model binder** when incoming data requires custom transformation or lookup logic.

<br>

---
---

<br>
