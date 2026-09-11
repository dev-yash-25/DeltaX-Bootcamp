# Model Binding

> [!Important]
> `[BindProperty]` works **only with Form Data**.
> By default it does **not** work with `HTTP GET` requests — to use it with GET, pass `SupportsGet = true`.

<br>

## Index

1. [What is a Model Binder?](#1-what-is-a-model-binder)
2. [Data Source Cheat Sheet](#2-data-source-cheat-sheet)
3. [How Data is Received](#3-how-data-is-received)
4. [How the Model Binder Works](#4-how-the-model-binder-works)
5. [`[BindProperty]` — Bind Form Data to a Single Property](#5-bindproperty--bind-form-data-to-a-single-property)
6. [`[BindProperties]` — Bind Form Data at the Controller Level](#6-bindproperties--bind-form-data-at-the-controller-level)
7. [Default Model Binder Behavior (No Attributes)](#7-default-model-binder-behavior-no-attributes)
8. [`[FromQuery]` — Bind Query String Data](#8-fromquery--bind-query-string-data)
9. [`[FromRoute]` — Bind Route Data](#9-fromroute--bind-route-data)
10. [`[FromBody]` — Bind Request Body Data](#10-frombody--bind-request-body-data)
11. [`[FromForm]` — Bind Form Data](#11-fromform--bind-form-data)
12. [`[FromHeader]` — Bind Header Data](#12-fromheader--bind-header-data)
13. [Custom Model Binder — Example 1 (Transform a Query Value)](#13-custom-model-binder--example-1-transform-a-query-value)
14. [Custom Model Binder — Example 2 (Fetch an Object from an ID)](#14-custom-model-binder--example-2-fetch-an-object-from-an-id)
15. [Final Attribute Cheat Sheet](#15-final-attribute-cheat-sheet)
16. [Final Memory Trick](#16-final-memory-trick)

<br>

---

<br>

## 1. What is a Model Binder?

### The Problem

When an HTTP request sends data from a client to the server, that data needs to be mapped to server-side .NET types — like parameters in action methods or properties in controllers.

### The Solution

The **Model Binder** is the mechanism responsible for this mapping.

### Definition

> Model binding is the process of binding HTTP request data to the parameters of application controllers, or to their properties.

<br>

## 2. Data Source Cheat Sheet

<table>
  <thead>
    <tr>
      <th>Data source</th>
      <th>Typical parameter type</th>
      <th>Example</th>
      <th>Query</th>
      <th>Passing method</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><strong>Route</strong></td>
      <td>Primitive/simple type</td>
      <td><code>int taskId</code></td>
      <td>
        <pre><code>GET /api/tasks/10</code></pre>
      </td>
      <td>
        <pre><code>public IActionResult GetTask(int taskId)
{
    return Ok(taskId);
}</code></pre>
      </td>
    </tr>
    <tr>
      <td><strong>Query</strong></td>
      <td>Primitive/simple type</td>
      <td><code>string status</code></td>
      <td>
        <pre><code>GET /api/tasks?status=pending</code></pre>
      </td>
      <td>
        <pre><code>public IActionResult GetTasks(string status)
{
    return Ok(status);
}</code></pre>
      </td>
    </tr>
    <tr>
      <td><strong>Query</strong></td>
      <td>Complex type/model</td>
      <td><code>Country country</code></td>
      <td>
        <pre><code>GET /api/countries/USA?Name=India&amp;Area=PCMC&amp;Population=150000</code></pre>
      </td>
      <td>
        <pre><code>public IActionResult GetCountry(
    string code,
    Country country)
{
    return Ok(country);
}</code></pre>
      </td>
    </tr>
    <tr>
      <td><strong>Body (JSON)</strong></td>
      <td>Complex type/model</td>
      <td><code>Task task</code></td>
      <td>
        <pre><code>POST /api/tasks

{
    "title": "Learn ASP.NET",
    "status": "pending"
}</code></pre>
      </td>
      <td>
        <pre><code>public IActionResult AddTask(Task task)
{
    return Ok(task);
}</code></pre>
      </td>
    </tr>
  </tbody>
</table>

<br>

## 3. How Data is Received

In an ASP.NET Core Web API application, data can be sent via various parts of the HTTP request:

* **URL** — Data can be sent directly within the route (e.g., IDs).
* **Query String** — Appended to the URL.
* **Headers** — Metadata attached to the request.
* **Body** — The primary payload of the request.
* **Form Data** — Standard HTML form submissions.

<br>

## 4. How the Model Binder Works

* The Model Binder acts as an **intermediary** between the incoming HTTP data and the controller parameters/properties.
* It takes the HTTP data as input, identifies the exact names of the parameters or properties defined in the ASP.NET Core application, and executes the binding.

<br>
<div align = "center">
<img width="600" alt="image" src="https://github.com/user-attachments/assets/15dc0385-b593-4e28-a64e-11f0dcd14d72" />
</div>
<br>

> [!Note]
> First the data passes through the Model Binder, **before** reaching the controller — visible when stepping through with the debugger.

### Flexibility & Customization

* There are numerous **built-in methods and attributes** available in ASP.NET Core for managing model binding.
* You are not limited to built-in options — you can create a **custom model binder** when specialized logic is required to map incoming data to parameters (see [§13](#13-custom-model-binder--example-1-transform-a-query-value) and [§14](#14-custom-model-binder--example-2-fetch-an-object-from-an-id)).

<br>
<div align = "center">
  <img width="500" alt="image" src="https://github.com/user-attachments/assets/b18abee3-4d97-4de1-b202-abdeb20284af" />
</div>
<br>

<br>

---

<br>

## 5. `[BindProperty]` — Bind Form Data to a Single Property

### Core Concepts

* **Purpose:** Enables model binding for public properties, allowing incoming data to be mapped to these properties automatically.
* **Usage:** Applied individually to each target property within the controller. You can have one or multiple properties bound this way.
* **Data Format:** Works specifically with **form data** in HTTP requests.

### Implementation Steps

1. **Setup** — Identify the controller where you want to bind data (e.g., a `CountriesController` with an `HTTP POST` method to add a new country).
2. **Applying the Attribute** — Decorate the public property in the controller with `[BindProperty]`.
3. **Testing** — Use a tool like *Postman* to send a request. Ensure the request body is set to **form-data**, matching the key defined in your controller property.

### Key Observations & Best Practices

* **Multiple Properties:** If you use multiple properties in your controller, you must apply `[BindProperty]` to **each one** individually — the binding will not work for properties missing the attribute.
* **Complex Types:** You can also use `[BindProperty]` with complex model classes — create a model (e.g. `CountryModel`) and use the model object as a single bound property.
* **HTTP GET Limitations:** By default, `[BindProperty]` does not work with `HTTP GET` requests, because the underlying binding configuration for the attribute only supports `HTTP POST` by default.
* **Enabling GET Support:** To allow a property to work with `HTTP GET` requests, set `SupportsGet` to `true` — right-click `[BindProperty]` → **Go To Definition** to verify.
    * Example: `[BindProperty(SupportsGet = true)]`

### Troubleshooting

* **204 No Content:** Usually means the binding is not working correctly — the attribute was omitted, or the data format in the request (e.g. Postman) does not match the expected form-data keys.
* **NullReferenceException:** Happens if you try to access a property that wasn't bound (like during a GET request without `SupportsGet`).

### Post

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

### Get

```csharp
[Route("api/[controller]")]
[ApiController]

public class CountriesController : ControllerBase
{
    [BindProperty(SupportsGet = true)]
    public Country country { get; set; }

    [HttpGet("")]
    public IActionResult AddCountry()
    {
        ..
    }
}
```

### Mental model

```text
[BindProperty]
      ↓
Applied per-property
      ↓
Works with Form Data (POST by default)
      ↓
Add SupportsGet = true → also works with GET
```

<br>

---

<br>

## 6. `[BindProperties]` — Bind Form Data at the Controller Level

### Core Concepts

* **Purpose:** Maps incoming form data directly to the public properties of a controller.
* **Controller Level Application:** Unlike `[BindProperty]` (applied per-property), `[BindProperties]` is applied at the **controller level** — no need to add attributes to each property individually.
* **Versatility:** Works for both simple types and complex objects.

### Key Observations & Constraints

* **HTTP GET Limitation:** By default, `[BindProperties]` does **not** work with HTTP GET requests — it results in null values.
* **Supporting GET Requests:** Set `SupportsGet` to `true` to enable binding for HTTP GET requests. Right-click `[BindProperties]` → "Go to definition" to see available options.

### Procedural Summary

1. **Setup:** Remove individual `[BindProperty]` attributes from properties and apply `[BindProperties]` to the controller class itself.
2. **HTTP POST:** The attribute automatically binds data sent in the request body for POST methods.
3. **Complex Objects:** Same approach for complex model types — all properties are mapped correctly.
4. **Enabling GET:** Modify to `[BindProperties(SupportsGet = true)]` to allow binding via URL parameters or query strings in GET requests.

### Post

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

### Get

```csharp
[Route("api/[controller]")]
[ApiController]
[BindProperties(SupportsGet = true)]

public class CountriesController : ControllerBase
{
    public Country country { get; set; }

    [HttpGet("")]
    public IActionResult AddCountry()
    {
    }
}
```

### Mental model

```text
[BindProperties]
      ↓
Applied once, at controller level
      ↓
Every public property gets bound
      ↓
Add SupportsGet = true → also works with GET
```

> [!Tip]
> **`[BindProperty]` vs `[BindProperties]`** — same underlying idea, different scope: one property at a time vs. the whole controller at once.

<br>

---

<br>

## 7. Default Model Binder Behavior (No Attributes)

What ASP.NET Core does **by default** when no binding attribute is used on action method parameters.

### Core Binding Rules

* **Primitive Types:** If action parameters use simple/primitive data types (`int`, `float`, `string`, `char`), the model binder automatically looks for that data in the **URL**.

```csharp
[HttpGet("{name}/{area}/{population}")]
public IActionResult AddCountry(string name, string area, int population)
{
    return Ok($"Name = {name}, Area = {area}, Population = {population}");
}
```

```text
https://localhost:64428/api/countries/india/pune/10000
```

<img width="470" alt="image" src="https://github.com/user-attachments/assets/76ba80ed-412f-4bac-8cac-445ff90e9053" />

* **Complex Types:** If action parameters are complex types (a custom class/model), the model binder defaults to looking inside the **request body**.

```csharp
[HttpPost("")]
public IActionResult AddCountry(Country country)
{
    return Ok($"Name = {country.Name}, Population = {country.Population}, Area = {country.Area}");
}
```

```text
https://localhost:64428/api/countries
```

...or even query params — passing them doesn't break the code:

```text
https://localhost:64428/api/countries?Name=China&Population=19999&Area=xin
```

```json
{
    "Name" : "Ind",
    "Population" : "122",
    "Area" : "test"
}
```

<img width="450" alt="image" src="https://github.com/user-attachments/assets/6914b04c-7686-44be-9dd9-2dccfad3af00" />

<br>

### Data Binding from the URL

There are two primary ways to pass data via the URL, and the model binder handles both automatically:

1. **Query String:** Data is passed as `?key=value`. The model binder matches by name — order does not matter.
2. **Route:** Data is passed directly in the URL path. The binder matches by name and does not require a specific order.

### Key Observations

* **Automatic Conversion:** The model binder handles type conversion — even if data arrives as a string in the URL, it converts it to the target parameter type (e.g. `int`) automatically.
* **Error Handling:** If there's a mismatch (e.g. a string where an integer is expected), the application returns `400 Bad Request`.
* **Exact Name Matching:** The model binder is strict about matching the parameter names defined in the code with the keys provided in the request.

### Complex Type Binding

When working with complex objects (e.g. a `Country` class), the model binder expects the data in the **request body**, typically as JSON.

* Even if you provide matching keys in the URL for a complex object, the binder will continue to look for the data in the body.

> [!Tip]
> To customize this default behavior — such as forcing a complex object to be read from the URL, or specific parameters to be read only from the query string — you need **attributes** (`[FromQuery]`, `[FromRoute]`, `[FromBody]`, etc. — covered next).

### Mental model

```text
No attribute used
        │
        ├── Primitive parameter → looks in the URL (route/query)
        │
        └── Complex type parameter → looks in the request Body
```

<br>

---

<br>

## 8. `[FromQuery]` — Bind Query String Data

### Core Concept

* **Purpose:** Forces the application to extract parameter values **exclusively** from the query string of the URL.
* **Default Behavior:** By default, ASP.NET Core attempts to bind data from multiple locations (route, body, query string). `[FromQuery]` tells the framework to ignore the other sources and look only at the query string — useful for resolving conflicts when data might exist in multiple places.

### Practical Implementation

* **Simple Data Binding:** A simple string parameter (e.g. `string name`) decorated with `[FromQuery]` will be read from the query string even if data with the same name exists in the route.
* **Complex Object Binding:** Also supports binding complex objects — pass values via the query string that map to model properties (e.g. `model.Name`).
* **Handling Multiple Sources:** If data is sent in both the request body and the query string, `[FromQuery]` ensures the application specifically reads from the query string and ignores any conflicting body data.

### Key Observations & Workflow

* **Selective Binding:** You can apply `[FromQuery]` to specific properties or parameters within a method, for granular control over where the API fetches its data.
* **Default Values:** If a parameter is not provided in the query string, it takes its default value. With multiple parameters (e.g. `ID` and `Name`), the application binds values provided in the URL query string while keeping default states for missing ones.
* **Efficiency:** Prevents accidental binding from unintended sources, making the API's behavior more predictable and secure.

```csharp
[HttpPost("")]
public IActionResult AddCountry([FromQuery] string name)
{
    return Ok($"Name = {name}");
}
```

<br>

---

<br>

## 9. `[FromRoute]` — Bind Route Data

### Core Concepts

* **Purpose:** Forces the application to bind action method parameters specifically to data available in the URL **route**, rather than the query string or the request body.
* **Difference from `[FromQuery]`:** `[FromQuery]` targets the query string; `[FromRoute]` is strictly used for route data.

### Practical Implementation

* **Simple Data Binding:**
    * If you pass the same data in the query string or request body, the application **ignores** those external sources and prioritizes the route value.
    * A type mismatch (e.g. passing an integer where a string is expected) returns a `400` error.
* **Complex Data Binding:**
    * You can bind complex objects (like a `Country` model) using `[FromRoute]` — the framework maps route parameters to the model's properties.
    * Even with complex objects, redundant data in the query string or body is disregarded in favor of the route data.

### Using Multiple Attributes

* No restriction against using multiple binding attributes simultaneously — e.g. capture some data from the route and other data (like an `id`) from the query string in a single action method.

> [!Tip]
> `[FromRoute]` is essential when you want to enforce that specific inputs **must** originate from the URL structure, for cleaner API design and predictable data binding.

```csharp
[HttpPost("{name}/{area}/{population}")]
public IActionResult AddCountry([FromRoute] Country country, [FromQuery] int id)
{
    return Ok($"Name = {country.Name}");
}
```

<br>

---

<br>

## 10. `[FromBody]` — Bind Request Body Data

### Core Concept

* **Purpose:** Forces the application to read incoming data specifically from the **request body**.
* **Functionality:** Ensures parameters are not sourced from other locations like the query string — crucial for handling complex data objects.

### Practical Implementation

* **Testing with Postman:** Passing an `ID` via the query string is ineffective when the requirement is to bind from the body — send it in the body instead.
* **Handling Complex Data:** Highly effective for complex models — with `[FromBody]`, the application ignores query string parameters and correctly binds object properties present in the request body.
* **Mixing Data Sources:** You can simultaneously use other attributes like `[FromQuery]` or `[FromRoute]` to pull data from different parts of the same request.

### Advanced Usage: PUT Requests

For a `PUT` request, you often need the `ID` from the route and the data from the body.

**Solution:** Use `[FromRoute]` to capture the `ID` from the URL, and `[FromBody]` to capture the actual data model — both parts of the request get processed accurately.

<br>

---

<br>

## 11. `[FromForm]` — Bind Form Data

### Core Concept

* Forces the application to explicitly read and bind data originating from **form-data** in an HTTP request.

### Practical Implementation

* **Binding Workflow:** Apply the attribute to the action method parameter in your controller.
* **Testing with Postman:** Select the **form-data** tab to send your key-value pairs. If your controller also uses `[FromRoute]`, you can simultaneously pass values through the route and the body.
* **Data Binding Observations:**
    * The binder maps keys from the form-data to properties in your model (e.g. a `Country` model).
    * **Important:** Binding only works for properties where corresponding keys are provided in the request — if you only send one key (e.g. `"name"`), only that property gets populated.
    * If you send multiple keys that match your model's properties, all corresponding properties are bound successfully.

### Summary

`[FromForm]` is a crucial tool for ensuring your API correctly consumes and maps data submitted via forms, giving precise control over how request bodies are processed.

<br>

---

<br>

## 12. `[FromHeader]` — Bind Header Data

### Key Concepts and Implementation

* **Purpose:** Part of the model binder — used to extract and read custom data transmitted in the **header** of an HTTP request, forcing the application to look in the header rather than other locations.
* **Basic Usage:**
    * Define a key-value pair in the request header (e.g. via Postman).
    * In the controller action method, apply `[FromHeader]` to the corresponding parameter. You must specify the type (e.g. `string`) and provide the name of the header key (e.g. `developer`) so the binder knows which header value to map.
* **Handling Multiple Headers:**
    * You can use multiple `[FromHeader]` attributes within the same action method to capture several different values from the request header.
    * Simply add the required parameters to your action method, each decorated with `[FromHeader]` and mapped to its specific header key name.

### Important Considerations

* **Flexibility:** You are not restricted to only header-bound data — combine `[FromHeader]` with other model-binding attributes (`[FromQuery]`, `[FromRoute]`, `[FromBody]`, `[FromForm]`) to map data from various locations within a single action method.

<br>

---

<br>

## 13. Custom Model Binder — Example 1 (Transform a Query Value)

### The Problem

Standard model binding works for simple types, but if you need to perform **custom logic** — e.g. splitting a string from a query parameter into an array or list — before the data reaches your action method, a standard binder won't suffice.

### Step-by-Step Implementation

1. **Understand the Problem** — Custom logic is needed before the data reaches your action method.
2. **Create the Binder Class** — Create a new class (e.g. `CustomModelBinder`) at the root level of your project. It must implement the `IModelBinder` interface (`Microsoft.AspNetCore.Mvc.ModelBinding` namespace).
3. **Implement the Binding Logic:**
    * The interface requires implementing `BindModelAsync`.
    * Use `bindingContext` to access `HttpContext` and the incoming `HttpRequest`.
    * Extract the raw data from the request (headers, query parameters, or body).
    * Perform the necessary manipulation (e.g. retrieve a value, split a string, parse it).
    * Set the result using `bindingContext.Result = ModelBindingResult.Success(yourProcessedData)`.
4. **Register the Binder in the Controller** — Apply `[ModelBinder]` to the specific parameter, specifying the binder type: `[ModelBinder(BinderType = typeof(CustomModelBinder))]`.
5. **Test the Implementation** — Trigger the API endpoint; use a breakpoint inside `BindModelAsync` to verify the incoming data is intercepted, manipulated, and correctly mapped.

### Key Instructor Observations & Notes

* **Flexibility:** Custom model binders cleanly handle complex data transformation requirements that built-in binders can't handle automatically.
* **Accessing Data:** `BindingContext` is your gateway to the entire request lifecycle — access headers, route data, and query strings directly from `bindingContext.HttpContext.Request`.
* **Debugging:** Breakpoints in `BindModelAsync` are the most efficient way to inspect `bindingContext` and confirm your data extraction logic is working.

> [!Note]
> The data passes through the Model Binder **first**, before reaching the controller — confirmed via the debugger.

### Example

**Binder**

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

**Controller**

```csharp
using ConsoleAppone.Models;

namespace ConsoleAppone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        [HttpGet("search")]
        public IActionResult SearchCountries([ModelBinder(typeof(CustomBinder))] string[] countries)
        {
            return Ok(countries);
        }
    }
}
```

### Mental model

```text
Request query string: ?countries=India|USA|China
              ↓
CustomBinder.BindModelAsync()
              ↓
Split on '|'
              ↓
string[] countries = ["India", "USA", "China"]
              ↓
Delivered to the action method, already parsed
```

<br>

---

<br>

## 14. Custom Model Binder — Example 2 (Fetch an Object from an ID)

### Core Concept

Accept a simple request (sending only an `id`) and have the model binder automatically **retrieve** the corresponding data (from a database or service) and bind it to a full `Country` object parameter in the action method.

### Step-by-Step Implementation

1. **Action Method Setup** — Create an action method (e.g. `CountryDetails`) that accepts a `Country` model. Ensure the `Country` model includes an `id` property to receive the request data.
2. **Creating the Custom Binder:**
    * Implement `IModelBinder`.
    * **Retrieving the value:** Use `bindingContext.ValueProvider.GetValue("id")` to get the result from the request.
    * **Data Conversion:** The incoming value is a string, so convert it to an integer using `int.TryParse`.
    * **Data Retrieval:** Once `id` is parsed, inject services or call a database to fetch the full object (hardcoded data can stand in as a placeholder for a database call while learning).
    * **Binding the Result:** Use `bindingContext.Result = ModelBindingResult.Success(model)` to bind the populated object and complete the task.
3. **Binding the Model to the Binder** — Apply `[ModelBinder(BinderType = typeof(CustomBinderName))]` to the `Country` model class to associate it with the custom binder.
4. **Controller Integration** — Use `[ModelBinder]` on the controller action parameter if needed, or let the model-level attribute handle the binding.

### Debugging and Observation

* **Debugging the Binder:** Place breakpoints directly inside `BindModelAsync` to inspect the `valueProvider` and the conversion process.
* **Verification:** Via Postman, send the `id` in the request — the application hits the custom binder, fetches the full object based on the ID, and delivers the complete `Country` object to the action method.

> [!Important]
> In a real-world production application, replace the hardcoded lookup logic with **dependency injection** to fetch data from your actual database context.

### Mental model

```text
Request: ?id=4
      ↓
CustomBinder.BindModelAsync()
      ↓
bindingContext.ValueProvider.GetValue("id") → "4"
      ↓
int.TryParse("4", out int id)
      ↓
Fetch full Country object (DB / service call)
      ↓
bindingContext.Result = ModelBindingResult.Success(country)
      ↓
Action method receives the fully populated Country object
```

<br>

---

<br>

## 15. Final Attribute Cheat Sheet

| Attribute            | Applied to           | Scope                          | Reads from                 | GET support by default? |
| ---------------------- | ----------------------- | ---------------------------------- | ----------------------------- | -------------------------- |
| `[BindProperty]`       | Individual property        | One property at a time                | Form data                        | ❌ (`SupportsGet = true` to enable) |
| `[BindProperties]`     | Controller class             | Every public property on the controller | Form data                        | ❌ (`SupportsGet = true` to enable) |
| `[FromQuery]`          | Parameter / property           | Exclusively query string                    | Query string                        | ✅ (GET is the natural fit) |
| `[FromRoute]`          | Parameter / property             | Exclusively route data                       | URL route                              | ✅ |
| `[FromBody]`           | Parameter                          | Exclusively request body                       | Request body (JSON)                       | ❌ (body-based, meant for POST/PUT) |
| `[FromForm]`           | Parameter / property                 | Exclusively form-data                             | Form-data                                    | ❌ (form-based, meant for POST) |
| `[FromHeader]`         | Parameter                              | Exclusively a named header key                       | Request header                                  | ✅ |
| `[ModelBinder(...)]`   | Parameter or model class                   | Whatever the custom `IModelBinder` implements          | Anywhere you choose to read from in code            | Depends on implementation |

<br>

---

<br>

## 16. Final Memory Trick

```text
No attribute
      ↓
      Primitive → URL (route/query)
      Complex   → Body


[BindProperty]        → one property,  form-data only,  POST by default
[BindProperties]      → whole controller,  form-data only,  POST by default

[FromQuery]   → force: query string ONLY
[FromRoute]   → force: route ONLY
[FromBody]    → force: request body ONLY
[FromForm]    → force: form-data ONLY
[FromHeader]  → force: a specific header key ONLY

[ModelBinder(typeof(CustomBinder))]
      ↓
      "I'll decide myself where the data comes from,
       and what shape it arrives in."
```

<br>

---

<br>
