# RESTful API 

## Index

* [1. HTTP (HyperText Transfer Protocol)](#http-hypertext-transfer-protocol)
* [2. What is a Web Service?](#what-is-a-web-service)
* [3. What is an API?](#what-is-an-api)
* [4. What is REST?](#what-is-rest)
* [5. Motivation Behind REST](#motivation-behind-rest)
   * [6 Design Principles](#6-rest-principles) 
   * [Best Practises of Design](#best-practises-of-design)
* [6. REST Uses HTTP Methods](#rest-uses-http-methods)
* [7. REST Uses Existing Standards](#rest-uses-existing-standards)
* [8. Core Concepts of REST](#core-concepts-of-rest)
  * [8.1 Resource](#1-resource)
  * [8.2 URI (Uniform Resource Identifier)](#2-uri-uniform-resource-identifier)
  * [8.3 Verbs (HTTP Methods)](#3-verbs-http-methods)
  * [8.4 Representation](#4-representation)
* [9. REST API Examples](#rest-api-examples)
  * [9.1 Get All Users](#get-all-users)
  * [9.2 Get Single User](#get-single-user)
  * [9.3 Create User](#create-user)
  * [9.4 Update Entire User](#update-entire-user)
  * [9.5 Update Only Last Name](#update-only-last-name)
  * [9.6 Delete User](#delete-user)
* [10. ASP.NET Core](#aspnet-core)
* [11. MVC (Model View Controller)](#mvc-model-view-controller)
  * [11.1 Model](#model)
  * [11.2 View](#view)
  * [11.3 Controller](#controller)
* [12. Repository Pattern](#repository-pattern)
* [13. Service Layer](#service-layer)
* [14. Repository vs Service](#repository-vs-service)
* [15. Dependency Injection (DI)](#dependency-injection-di)
* [16. Inversion of Control (IoC)](#inversion-of-control-ioc)
* [17. Mocking](#mocking)
* [18. Uploading Images to Firebase](#uploading-images-to-firebase)
* [19. Important Topics to Read](#important-topics-to-read)
  * [19.1 Cross-Origin Requests (CORS)](#cross-origin-requests-cors)
  * [19.2 Dapper](#dapper)
  * [19.3 Parameterized Query](#parameterized-query)
  * [19.4 Options Pattern](#options-pattern)
  * [19.5 Anonymous Type](#anonymous-type)
  * [19.6 Integration Testing](#integration-testing)
  * [19.7 JWT (JSON Web Token)](#jwt-json-web-token)
  * [19.8 API Versioning](#api-versioning)
* [20. Additional Reading (Optional)](#additional-reading-optional)
  * [20.1 GraphQL](#graphql)
  * [20.2 API Testing](#api-testing)
  * [20.3 BDD (Behavior Driven Development)](#bdd-behavior-driven-development)
* [21. Quick Interview Points](#quick-interview-points)



<br>

<div align = "center">
       <img width="500" alt="image" src="https://github.com/user-attachments/assets/e8365d73-cc19-4e1e-9237-620b4b3cac90" />
</div>
<br>

# HTTP (HyperText Transfer Protocol)

HTTP is the communication protocol used by the Web. It defines how a client (browser/app) sends requests to a server and how the server sends responses.

### Flow

```
Client (Browser/App)
       |
   HTTP Request
       |
     Server
       |
   HTTP Response
       |
Client receives data
```

### Example

```
GET /users/1 HTTP/1.1
```

Server Response

```json
{
    "id": 1,
    "name": "Yash"
}
```

<br>

# What is a Web Service?

A **Web Service** is a server-side application (piece of code) that exposes functionality over the internet so other applications can use it.

It listens for HTTP requests, executes code, and returns a response.

### Example

Suppose you have a C# method

```csharp
public User GetUser(int id)
{
    // Fetch from database
}
```

If this method is exposed over HTTP,

```
GET /users/5
```

internally calls

```csharp
GetUser(5);
```

and returns

```json
{
    "id":5,
    "name":"Yash"
}
```

This is a **Web Service**.

<br>

---

<br>

# What is an API?

API (Application Programming Interface) is a set of rules that allows two software applications to communicate.

An API defines

- What requests can be made
- What data should be sent
- What response will be returned

### Example

```
Weather App
        |
        | API Request
        |
Weather Server
        |
Returns Temperature
```

<br>

# What is REST?

REST (Representational State Transfer) is an **architectural style** for designing web services.

**Important**

- REST is **NOT a protocol**
- REST is **NOT an official standard**
- REST is a collection of architectural principles
- REST generally uses HTTP

Interview Definition

> REST is an architectural style that defines a set of constraints for designing scalable web APIs over HTTP.

<br>

# Motivation Behind REST

REST was created to capture the characteristics that made the Web successful.

Main ideas

- URI Addressable Resources
- HTTP Protocol
- Request → Response communication

Example

```
Client
   |
GET /users/1
   |
Server
   |
Returns JSON
```

<br>

## 6 Rest Principles

<br>
<div align = "center">
 <img width="550" alt="image" src="https://github.com/user-attachments/assets/49678e14-d997-47df-99a5-03f160aba4b9" />
</div>
<br>

| | Principle                       | Meaning                                                                                                                                                | Example                                      |
| - | ------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ | -------------------------------------------- |
| 1 | **Client–Server**               | Client and server have separate responsibilities. The client handles the UI, while the server handles data and business logic.                         | App → API → Database                         |
| 2 | **Stateless**                   | The server does not store client session state between requests. Each request must contain all the information needed to process it.                   | `GET /users/10` + authentication token       |
| 3 | **Cacheable**                   | A response should indicate whether it can be cached. Cached data can be reused instead of requesting it from the server again.                         | `GET /products` → response cached for 10 min |
| 4 | **Uniform Interface**           | There should be a consistent way to identify and interact with resources, using standard methods and representations.                                  | `GET /users/10`, `DELETE /users/10`          |
| 5 | **Layered System**              | The client may not know whether it is communicating directly with the actual server. Requests can pass through gateways, load balancers, proxies, etc. | Client → API Gateway → Server                |
| 6 | **Code-on-Demand** *(Optional)* | The server can optionally send executable code to the client, allowing the client's functionality to be extended.                                      | Server → JavaScript → Browser                |

<br>

## Best Practises of Design

| | Best Practice                  | Meaning                                                                        | Example                                                                |
| -- | ------------------------------ | ------------------------------------------------------------------------------ | ---------------------------------------------------------------------- |
| 1  | **Use Nouns for Resources**    | URLs should represent resources, while HTTP methods describe the action.       | ✅ `GET /users` ❌ `GET /getUsers`                                       |
| 2  | **Use Plural Resource Names**  | Prefer plural names for collections to keep API naming consistent.             | ✅ `/users` ❌ `/user`                                                   |
| 3  | **Use HTTP Methods Correctly** | Use standard HTTP verbs according to the operation being performed.            | `GET` → read, `POST` → create, `PUT/PATCH` → update, `DELETE` → delete |
| 4  | **Use Hierarchical URLs**      | Show relationships between related resources through the URL structure.        | `/users/10/orders`                                                     |
| 5  | **Use Resource IDs**           | Use unique identifiers to access a specific resource.                          | `GET /users/10`                                                        |
| 6  | **Avoid Verbs in URLs**        | Don't put actions in URLs; let the HTTP method represent the action.           | ❌ `/createUser` → ✅ `POST /users`                                      |
| 7  | **Use HTTP Status Codes**      | Return appropriate status codes so the client knows the result of the request. | `200` → success, `201` → created, `404` → not found                    |
| 8  | **Use JSON Representations**   | Use a standard, consistent format for exchanging resource data.                | `{ "id": 10, "name": "John" }`                                         |
| 9  | **Use Query Parameters**       | Use query parameters for filtering, sorting, searching, and pagination.        | `/users?role=admin&page=2`                                             |
| 10 | **Consistent Naming**          | Follow one naming convention throughout the API.                               | `/first-name` or `/firstName` — don't mix both                         |
| 11 | **API Versioning**             | Use versions when making changes that could break existing clients.            | `/api/v1/users`                                                        |
| 12 | **Consistent Error Responses** | Return errors in a predictable structure so clients can handle them easily.    | `{ "error": "User not found" }`                                        |
| 13 | **Use Pagination**             | Don't return huge collections at once; divide large results into pages.        | `/users?page=2&limit=20`                                               |
| 14 | **Use Filtering & Sorting**    | Allow clients to request only the data they need.                              | `/products?category=mobile&sort=price`                                 |
| 15 | **Use Proper Authentication**  | Secure APIs using appropriate authentication and authorization mechanisms.     | `Authorization: Bearer <token>`                                        |

<br>

# REST Uses HTTP Methods

REST makes proper use of HTTP methods.

| Method | Purpose |
|------|---|
| GET | Read Data |
| POST | Create Data |
| PUT | Replace Entire Resource |
| PATCH | Update Part of Resource |
| DELETE | Delete Resource |

<br>

# REST Uses Existing Standards

REST itself is not a standard.

It uses existing standards like

- HTTP
- URI / URL
- JSON
- XML
- HTML
- Images
- MIME Types

<br>

---

<br>


# Core Concepts of REST

## 1. Resource

A Resource is any object or data exposed by the server.

Examples

- User
- Movie
- Product
- Order

Every resource has a unique URI.

Example

```
/users
/users/1
/movies/25
/orders/15
```

<br>

## 2. URI (Uniform Resource Identifier)

A URI uniquely identifies a resource.

Example

```
https://api.company.com/users/1
```

Here,

Resource = User

Id = 1

<br>

## 3. Verbs (HTTP Methods)

HTTP methods tell the server what action should be performed.

| Verb | Meaning |
|------|---|
| GET | Read |
| POST | Create |
| PUT | Replace |
| PATCH | Partial Update |
| DELETE | Remove |

<br>

## 4. Representation

Representation is the format in which data is returned.

Most common formats

- JSON ⭐
- XML

Example JSON

```json
{
    "id":1,
    "name":"Yash"
}
```

<br>

# REST API Examples

## Get All Users

```
GET /v1/users
```

Returns

```json
[
    {
        "id":1,
        "name":"Yash"
    },
    {
        "id":2,
        "name":"Kshitij"
    }
]
```

<br>

## Get Single User

```
GET /v1/users/1
```

Returns

```json
{
    "id":1,
    "name":"Yash"
}
```

<br>

## Create User

```
POST /v1/users
```

Body

```json
{
    "firstName":"Kshitj",
    "lastName":"Nangare"
}
```

<br>

## Update Entire User

```
PUT /v1/users/2
```

Body

```json
{
    "firstName":"Kshitij",
    "lastName":"Nangare"
}
```

PUT replaces the complete resource.

<br>

## Update Only Last Name

```
PATCH /v1/users/2
```

Body

```json
{
    "lastName":"Nangare"
}
```

PATCH updates only specified fields.

<br>

## Delete User

```
DELETE /v1/users/2
```

Deletes user with Id = 2.

<br>

---

<br>

# ASP.NET Core

ASP.NET Core is Microsoft's

- Cross-platform
- Open-source
- High-performance

framework for building

- Web APIs
- Web Applications
- Cloud Applications
- Mobile Backends
- Microservices

Supports

- Windows
- Linux
- macOS

<br>

---

<br>


# MVC (Model View Controller)


<br>
<div align = "center">
<img width="550" alt="image" src="https://github.com/user-attachments/assets/70418005-1f4e-4192-9617-602bfc2f2a93" />
</div>
<br>

MVC separates an application into three parts.

```
Request
   |
Controller
   |
Business Logic
   |
Model
   |
Database
   |
Controller
   |
View / JSON Response
```

## Model

Represents data.

Examples

- Movie
- Actor
- Producer

<br>

## View

Represents the User Interface.

Examples

- HTML Page
- Razor View

For Web APIs, usually JSON is returned instead of a View.

<br>

## Controller

Receives requests, calls business logic, returns responses.

Example

```
GET /movies
        |
MovieController
        |
MovieService
        |
MovieRepository
        |
Database
```

<br>

---

<br>


# Repository Pattern

These classes handle getting data into and out of our data store, with the important caveat that each Repository only works against a single Model class.  So, if your models are Dogs, Cats, and Rats, you would have a Repository for each, the DogRepository would not call anything in the CatRepository, and so on.



Repository handles only database operations.

Each Repository manages only one entity.

Example

```
MovieRepository
ActorRepository
ProducerRepository
```

MovieRepository should not access ActorRepository directly.

Example of operations

```csharp
MovieRepository.GetMovieById()
MovieRepository.AddMovie()
MovieRepository.DeleteMovie()
```

<br>

# Service Layer
These classes can query multiple Repository classes and combine their data to form new, more complex business objects.

Service contains business logic.

A Service can call multiple repositories.

Example

```
MovieService

        |
        |---- MovieRepository
        |
        |----  ActorRepository
        |
        |----  ProducerRepository
```

Example

While creating a movie,

MovieService

- Validates Producer
- Validates Actors
- Saves Movie

<br>

# Repository vs Service

Repository

- Database operations
- CRUD
- One entity only

Service

- Business logic
- Validation
- Uses multiple repositories

<br>

---

<br>



# Dependency Injection (DI)

Dependency Injection is a design pattern that provides required objects from outside instead of creating them manually.

Without DI

```csharp
var repo = new MovieRepository();
```

With DI

```csharp
public MovieService(IMovieRepository repo)
{
    _repo = repo;
}
```

Benefits

- Loose Coupling
- Easy Testing
- Easy Maintenance
- Better Code Reuse

<br>

> [!Note]
> - DI is a software design pattern which enables the development of loosely coupled code.
> - Through DI, you can decrease tight coupling between software components.
> - It is an implementation of Inversion-of-Control, which makes unit testing convenient.

# Inversion of Control (IoC)

<br>
<div align = "center">
<img width="400" alt="image" src="https://github.com/user-attachments/assets/144489b2-1095-4c09-95a3-d7bd3f2f1e57" />
</div>
<br>

DI is an implementation of IoC.

- IoC (Inversion of Control) is a **big-picture design** principle where you stop your code from creating and managing the objects it needs.
- Instead, an outside framework or container creates them and hands them to your code. 
- **Dependency Injection (DI)** is just the specific method used to do this.

Instead of your class creating dependencies,

the framework provides them.


<br>
<div align = "center">
       <img width="500" alt="image" src="https://github.com/user-attachments/assets/557c73de-0c20-4af7-ad35-d12d7524c67e" />
</div>
<br>




# Mocking

Mocking replaces real dependencies with fake ones during testing.

Purpose

- Test only your code
- Avoid database calls
- Faster tests

Example

Instead of

```
MovieService
      |
SQL Database
```

Use

```
MovieService
      |
Fake Repository (Mock)
```

Now testing becomes easier.

<br>

---

<br>



# Uploading Images to Firebase

Package

```
FirebaseStorage.net
Version 1.0.3
```

Basic Flow

```
Client
   |
Upload Image
   |
API
   |
Firebase Storage
   |
Returns Image URL
```

<br>

---

<br>



# Important Topics to Read

## [Cross-Origin Requests (CORS)](https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1)

Allows or blocks requests coming from another domain.

Example

```
Frontend
http://localhost:3000

Backend
http://localhost:5000
```

Without CORS

❌ Request Blocked

With CORS Enabled

✅ Request Allowed

<br>

## Dapper

A lightweight Micro ORM for .NET.

Converts SQL query results directly into C# objects.

<br>

## Parameterized Query

Used to prevent SQL Injection.

Bad

```sql
SELECT * FROM Users
WHERE Name = '" + name + "'";
```

Good

```sql
SELECT * FROM Users
WHERE Name = @Name;
```

<br>

## Options Pattern

Used to read configuration values from appsettings.json.

Example

```json
{
    "ConnectionStrings": {
        "Default": "..."
    }
}
```

<br>

## Anonymous Type

Create objects without defining a class.

Example

```csharp
var user = new
{
    Name = "Yash",
    Age = 22
};
```

<br>

## Integration Testing

Tests multiple components working together.

Example

```
Controller
    |
Service
    |
Repository
    |
Database
```

All tested together.

<br>

## JWT (JSON Web Token)

Used for Authentication.

Flow

```
Login

↓

Server Creates Token

↓

Client Stores Token

↓

Client Sends Token

↓

Server Verifies Token
```

<br>

## API Versioning

Allows multiple versions of an API.

Example

```
GET /api/v1/users

GET /api/v2/users
```

Older applications continue working even after new versions are released.

<br>

# Additional Reading (Optional)

- GraphQL
- API Testing
- BDD (Behavior Driven Development)

These are useful but not required for the assignment.

<br>

---

<br>



# Quick Interview Points

- HTTP is a protocol.
- REST is an architectural style (not a standard).
- Web Service is server-side code exposed over HTTP.
- API is a contract for communication between software.
- Resource = Data/Object exposed by API.
- URI uniquely identifies a resource.
- JSON is the most common REST response format.
- Repository handles data access.
- Service handles business logic.
- DI provides dependencies instead of creating them manually.
- Mocking replaces real dependencies during testing.
- JWT is used for authentication.
- CORS controls cross-origin requests.
  
<br>

---

<br>

