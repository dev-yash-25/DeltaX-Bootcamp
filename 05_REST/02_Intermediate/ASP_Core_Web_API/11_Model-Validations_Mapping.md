# Model Validations & AutoMapper

## Index

- [1. Model Validation](#1-model-validation)
  - [1.1 Why Model Validation is Needed](#11-why-model-validation-is-needed)
  - [1.2 Data Annotations](#12-data-annotations)
  - [1.3 Common Validation Attributes](#13-common-validation-attributes)
  - [1.4 Automatic Validation with `[ApiController]`](#14-automatic-validation-with-apicontroller)
  - [1.5 Testing Validation in Postman](#15-testing-validation-in-postman)
  - [1.6 Multiple Validations](#16-multiple-validations)
  - [1.7 Custom Error Messages](#17-custom-error-messages)
- [2. AutoMapper](#2-automapper)
  - [2.1 Why AutoMapper](#21-why-automapper)
  - [2.2 Install AutoMapper](#22-install-automapper)
  - [2.3 Register AutoMapper](#23-register-automapper)
  - [2.4 Create a Mapping Profile](#24-create-a-mapping-profile)
  - [2.5 `CreateMap()` and `ReverseMap()`](#25-createmap-and-reversemap)
  - [2.6 Inject `IMapper`](#26-inject-imapper)
  - [2.7 Mapping Objects](#27-mapping-objects)
  - [2.8 Mapping Collections](#28-mapping-collections)
  - [2.9 Matching and Different Property Names](#29-matching-and-different-property-names)
  - [2.10 Database Entity vs API Model](#210-database-entity-vs-api-model)
- [3. Validation + Mapping Mental Model](#3-validation--mapping-mental-model)
- [4. Key Takeaways](#4-key-takeaways)


<br>

---

<br>

## 1. Model Validation

### 1.1 Why Model Validation is Needed

Model validation ensures that data received by the API is **valid and satisfies the required rules** before the application processes it.

Without validation, an API may accept requests containing:

- Missing required fields
- Empty values
- Invalid email addresses
- Values that are too long/short
- Values that do not match a required pattern

Example:

```text
Client
   ↓
HTTP Request
   ↓
Model Binding
   ↓
Model Validation
   ↓
Controller Action
```

The purpose is to prevent invalid data from entering the application's processing/database flow.


<br>

---

<br>

### 1.2 Data Annotations

ASP.NET Core provides validation attributes through:

```csharp
using System.ComponentModel.DataAnnotations;
```

Validation attributes are placed directly above model properties using square brackets.

Example:

```csharp
[Required]
public string Title { get; set; }
```

The attributes describe the rules that the property must satisfy.


<br>

---

<br>

### 1.3 Common Validation Attributes

#### `[Required]`

Ensures that a required field is not left empty.

```csharp
[Required]
public string Title { get; set; }
```

If `Title` is missing/invalid according to the validation rules, model validation fails.

<br>

#### `[StringLength]`

Specifies maximum/minimum character limits.

```csharp
[StringLength(100)]
public string Title { get; set; }
```

It can also specify both minimum and maximum lengths.

```csharp
[StringLength(100, MinimumLength = 3)]
public string Title { get; set; }
```

<br>

#### `[EmailAddress]`

Validates that the value follows an email-address format.

```csharp
[EmailAddress]
public string Email { get; set; }
```

<br>

#### `[RegularExpression]`

Allows custom validation rules using a regular expression.

```csharp
[RegularExpression("pattern")]
public string Value { get; set; }
```

Useful when the API requires a specific format.


<br>

---

<br>

### 1.4 Automatic Validation with `[ApiController]`

If the controller has:

```csharp
[ApiController]
```

ASP.NET Core automatically performs model validation.

Therefore, the controller normally does **not** need to manually check:

```csharp
if (!ModelState.IsValid)
{
    ...
}
```

The framework handles invalid model state automatically.

[!Important]

`[ApiController]` is important here because it enables automatic HTTP 400 responses when model validation fails.


<br>

---

<br>

### 1.5 Testing Validation in Postman

Suppose the model contains:

```csharp
[Required]
public string Title { get; set; }
```

If a request is sent without `Title`, the API returns:

```text
400 Bad Request
```

The response contains validation information explaining why the request was rejected.

### Request

```json
{
    "description": "A book description"
}
```

### Result

```text
HTTP 400 Bad Request
```

The request does not reach the normal successful controller processing because validation has failed.


<br>

---

<br>

### 1.6 Multiple Validations

Multiple validation attributes can be applied to the same property.

Example:

```csharp
[Required]
[StringLength(100, MinimumLength = 3)]
public string Title { get; set; }
```

Now `Title` must satisfy **both** rules:

1. It must be provided.
2. Its length must be within the specified range.

This is useful for combining different validation requirements.


<br>

---

<br>

### 1.7 Custom Error Messages

Validation attributes can contain custom error messages.

Example:

```csharp
[Required(ErrorMessage = "Please add a title")]
public string Title { get; set; }
```

Instead of a generic validation message, the API can return:

```text
Please add a title
```

Custom messages make validation responses more meaningful to API consumers.


<br>

---
---

<br>

# 2. AutoMapper

## 2.1 Why AutoMapper

In the CRUD APIs built previously, we manually mapped database entities to API models.

Example:

```csharp
var bookModel = new BookModel
{
    Id = book.Id,
    Title = book.Title,
    Description = book.Description
};
```

This becomes tedious when a model has many properties.

For example, if a table contains 20+ properties, manually assigning every property:

```text
Database Entity
     ↓
Id       → Id
Title    → Title
Author   → Author
Price    → Price
...
Property20 → Property20
```

has two major problems:

- **Time-consuming:** Lots of repetitive code.
- **Error-prone:** A property can accidentally be mapped to the wrong destination property.

AutoMapper automates this mapping.

### Mental Model

```text
Source Object
     ↓
   AutoMapper
     ↓
Destination Object
```


<br>

---

<br>

## 2.2 Install AutoMapper

For ASP.NET Core, install:

```text
AutoMapper.Extensions.Microsoft.DependencyInjection
```

This package allows AutoMapper to work with ASP.NET Core's Dependency Injection system.

> [!Note]
> AutoMapper itself can be used in .NET applications generally. The `AutoMapper.Extensions.Microsoft.DependencyInjection` package is specifically useful for ASP.NET Core DI integration.


<br>

---

<br>

## 2.3 Register AutoMapper

Register AutoMapper inside `Startup.cs`, in `ConfigureServices()`:

```csharp
services.AddAutoMapper(typeof(Startup));
```

Example:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    services.AddAutoMapper(typeof(Startup));
}
```

This makes AutoMapper available through Dependency Injection.


<br>

---

<br>

## 2.4 Create a Mapping Profile

Create a folder such as:

```text
Helpers
```

Then create a mapping profile class, for example:

```text
Helpers
└── ApplicationMapping.cs
```

The class inherits from AutoMapper's:

```csharp
Profile
```

Example:

```csharp
using AutoMapper;

public class ApplicationMapping : Profile
{
    public ApplicationMapping()
    {
        CreateMap<Book, BookModel>();
    }
}
```

The profile is where mapping rules are defined.

### Responsibility

```text
ApplicationMapping
       ↓
Defines:
Book → BookModel
```


<br>

---

<br>

## 2.5 `CreateMap()` and `ReverseMap()`

### Basic Mapping

```csharp
CreateMap<Book, BookModel>();
```

This defines:

```text
Book → BookModel
```

AutoMapper will map matching properties automatically.

### Bidirectional Mapping

The tutorial uses:

```csharp
CreateMap<Book, BookModel>().ReverseMap();
```

This enables both directions:

```text
Book ─────────→ BookModel
BookModel ────→ Book
```

So the same mapping configuration can be used for both entity-to-model and model-to-entity conversion.

> [!Important]
>
> `ReverseMap()` is useful when the application needs to map in both directions.


<br>

---

<br>

## 2.6 Inject `IMapper`

AutoMapper is registered with Dependency Injection, so `IMapper` can be injected into the repository.

Example:

```csharp
private readonly IMapper _mapper;

public BookRepository(IMapper mapper)
{
    _mapper = mapper;
}
```

The repository can now use:

```csharp
_mapper.Map<Destination>(source);
```


<br>

---

<br>

## 2.7 Mapping Objects

Suppose:

```text
Book
```

is the database entity and:

```text
BookModel
```

is the API/view model.

Instead of manually writing:

```csharp
var bookModel = new BookModel
{
    Id = book.Id,
    Title = book.Title,
    Description = book.Description
};
```

use AutoMapper:

```csharp
var bookModel = _mapper.Map<BookModel>(book);
```

The mapping becomes:

```text
Book
 ↓
_mapper.Map<BookModel>()
 ↓
BookModel
```

### Generic Syntax

```csharp
_mapper.Map<Destination>(source);
```

Example:

```csharp
_mapper.Map<BookModel>(book);
```


<br>

---

<br>

## 2.8 Mapping Collections

AutoMapper can also map collections.

Instead of manually doing:

```csharp
var bookModels = new List<BookModel>();

foreach (var book in books)
{
    var bookModel = new BookModel
    {
        Id = book.Id,
        Title = book.Title,
        Description = book.Description
    };

    bookModels.Add(bookModel);
}
```

AutoMapper can map the collection directly:

```csharp
var bookModels = _mapper.Map<List<BookModel>>(books);
```

So:

```text
List<Book>
    ↓
   AutoMapper
    ↓
List<BookModel>
```

This is especially useful for GET-all APIs.


<br>

---

<br>

## 2.9 Matching and Different Property Names

AutoMapper works best when source and destination properties have the **same names**.

Example:

```csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
}
```

```csharp
public class BookModel
{
    public int Id { get; set; }
    public string Title { get; set; }
}
```

AutoMapper can automatically map:

```text
Book.Id    → BookModel.Id
Book.Title → BookModel.Title
```

### Different Property Names

If property names are different, AutoMapper needs an explicit mapping rule in the Profile.

For example:

```text
Book.Name
    ↓
BookModel.Title
```

The profile must define how these differently named properties correspond.

> [!Note]
> The tutorial emphasizes that matching property names are the easiest case. When names differ, configure the mapping explicitly in the mapping profile.


<br>

---

<br>

## 2.10 Database Entity vs API Model

A major reason for using mapping is to separate the **database entity** from the **API model/view model**.

```text
Database
   ↓
Book Entity
   ↓
AutoMapper
   ↓
BookModel
   ↓
API Response
```

Instead of directly exposing the database entity through the API, the application can return a dedicated model.

### Why this structure helps

```text
Database Entity
      ≠
API Model
```

The database entity represents persistence/database structure, while the API model represents what the API exposes.

AutoMapper handles the conversion between them.


<br>

---

<br>

# 3. Validation + Mapping Mental Model

These two concepts solve different problems.

### Model Validation

Validation answers:

> **"Is the incoming data valid?"**

```text
HTTP Request
     ↓
Model Binding
     ↓
Model Validation
     ↓
Valid? ── No ──→ 400 Bad Request
     │
    Yes
     ↓
Controller
```

### AutoMapper

Mapping answers:

> **"How do I convert one object type into another?"**

```text
Database Entity
      ↓
   AutoMapper
      ↓
   API Model
      ↓
HTTP Response
```

### Together

```text
                 REQUEST
                    ↓
              Model Binding
                    ↓
             Model Validation
              ↙           ↘
          Invalid          Valid
             ↓               ↓
       400 Bad Request   Controller
                             ↓
                         Repository
                             ↓
                          Database
                             ↓
                       Entity / Model
                             ↓
                         AutoMapper
                             ↓
                         API Model
                             ↓
                        HTTP Response
```


<br>

---

<br>

# 4. Key Takeaways

## Model Validation

- Use:

```csharp
using System.ComponentModel.DataAnnotations;
```

- Validation rules are added using attributes.
- Common attributes:
  - `[Required]`
  - `[StringLength]`
  - `[EmailAddress]`
  - `[RegularExpression]`
- Multiple attributes can be applied to one property.
- Custom error messages can be supplied.
- `[ApiController]` automatically returns `400 Bad Request` when model validation fails.

## AutoMapper

- Eliminates repetitive manual object mapping.
- Reduces mapping mistakes.
- Install:

```text
AutoMapper.Extensions.Microsoft.DependencyInjection
```

- Register:

```csharp
services.AddAutoMapper(typeof(Startup));
```

- Create a class inheriting from `Profile`.
- Configure:

```csharp
CreateMap<Book, BookModel>();
```

- For bidirectional mapping:

```csharp
CreateMap<Book, BookModel>().ReverseMap();
```

- Inject:

```csharp
IMapper
```

- Map an object:

```csharp
_mapper.Map<BookModel>(book);
```

- Map a collection:

```csharp
_mapper.Map<List<BookModel>>(books);
```

- Matching property names are mapped automatically.
- Different property names require explicit configuration.


<br>

---

<br>

## Final Mental Model

```text
VALIDATION
Incoming Request
      ↓
Model Binding
      ↓
Data Annotations
      ↓
Valid?
  ├── No → 400 Bad Request
  └── Yes
       ↓

MAPPING
Entity / Model
      ↓
  AutoMapper
      ↓
Destination Model
      ↓
API Response
```


<br>

---
---

<br>
