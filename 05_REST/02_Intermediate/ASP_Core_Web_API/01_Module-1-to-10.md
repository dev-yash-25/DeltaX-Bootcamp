# REST APIs — ASP.NET Core Web API

<br>
<table align = "center"> 
  <tr>
    <td align="center">
      <p><b>REST Architecture</b></p>
      <img width="400" alt="REST architecture" src="https://github.com/user-attachments/assets/d7d71f55-60fb-4dc5-96d3-2f83513df4ec" />
    </td>
    <td align="center">
      <p><b>API Gateway - Intermediate Server</b></p>
      <img width="400" alt="API Gateway - Intermediate server" src="https://github.com/user-attachments/assets/f0fc8661-4f74-44cc-a05e-d507042e3901" />
    </td>
  </tr>
</table>
<br>


<table align = "center">
  <tr>
    <td align="center">
      <p><b>Types of API for Different Frameworks</b></p>
      <img width="300" alt="Types of API for different frameworks" src="https://github.com/user-attachments/assets/81e62b30-9bad-445a-939c-71e2ac2419a7" />
    </td>
    <td align="center">
      <p><b>API Structure</b></p>
      <img width="450" alt="API Structure" src="https://github.com/user-attachments/assets/b8c5bf42-b678-4db0-973a-8af468f9aa61" />
    </td>
    <td align="center">
      <p><b>JWT</b></p>
      <img width="300" alt="JWT" src="https://github.com/user-attachments/assets/4466c28f-01a5-4874-a51c-0844d87373e1" />
    </td>
    <td></td>
  </tr>
</table>

<br>


## Index

[Mental Model](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#mental-model-for-designing-rest-apis)

1. [Course Overview](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#1-course-overview)
2. [Why Do We Need Web APIs?](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#2-why-do-we-need-web-apis)
3. [Role of a Web API](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#3-role-of-a-web-api)
4. [What is a Web API?](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#4-what-is-a-web-api)
5. [ASP.NET Web API vs ASP.NET Core](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#5-aspnet-web-api-vs-aspnet-core)
6. [REST](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#6-rest)
7. [HTTP](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#7-http)
8. [HTTP Request](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#8-http-request)
9. [HTTP Response](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#9-http-response)
10. [HTTP Methods / Verbs](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#10-http-methods--verbs)
11. [HTTP Status Code Categories](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#11-http-status-code-categories)
12. [Frequently Used Status Codes](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/01_Module-1-to-10.md#12-frequently-used-status-codes)


<br>

# Mental Model for Designing REST APIs

When designing an API, ask these questions in order:

### 1. What are my resources?

Example:

```text
Customer
Order
Product
Movie
Actor
```

### 2. What identifies each resource?

```text
Customer → customerId
Order    → orderId
Movie    → movieId
```

### 3. What operations are required?

```text
Create
Read
Update
Delete
Search
Filter
```

### 4. Which HTTP method represents the operation?

```text
Create → POST
Read   → GET
Update → PUT/PATCH
Delete → DELETE
```

### 5. Where should input go?

```text
Resource identity → Route
- GET /api/users/15

Filtering/search → Query
- GET /api/tasks?status=pending

Create/update data → Body
- POST /api/users
body.. {
       }
```

### 6. What status code should be returned?

```text
Success → 2xx
Invalid client request → 4xx
Server failure → 5xx
```

<br>

## 1. Course Overview

This course focuses on building **RESTful Web APIs using ASP.NET Core**.

Main topics:

- ASP.NET Core Web API
- REST principles
- HTTP
- Middleware
- Dependency Injection
- Routing
- Response formatting
- Model binding
- Entity Framework Core
- SQL Server
- JWT authentication
- Consuming APIs from client applications

### Prerequisites

- Basic C# knowledge: classes, interfaces, methods, data types
- Basic database knowledge: tables and columns

No prior ASP.NET Core or Entity Framework experience is required.



<br>

<div align="center">
  <p><b>v1 - All Concepts</b></p>
  <img width="600" alt="v1 - all concepts" src="https://github.com/user-attachments/assets/a587601f-decb-40c0-b1a5-6330091a86ef" />
</div>


<br>

# 2. Why Do We Need Web APIs?


<div align="center">
  <p><b>v3 - Need, Diagram</b></p>
  <img width="600" alt="v3 - Need, diagram" src="https://github.com/user-attachments/assets/b9a95adc-8933-4902-b297-2b6dd50f607d" />
</div>






Consider an application available on multiple platforms:

```text
             ┌── Website
             ├── Android App
Users ───────┤
             └── iOS App
                    │
                    ↓
                 Database
```

If every application communicates directly with the database:

### Duplicate Business Logic

The same business rules may need to be implemented separately in every application.

```text
Website  → Business Logic
Android  → Business Logic
iOS      → Business Logic
```

This causes:

- Code duplication
- More maintenance
- Higher chance of inconsistent behavior
- More testing effort

### Technology Constraints

Different clients may use different technologies:

```text
Website → Angular
Android → Android framework
iOS     → iOS framework
```

Clients should not need to understand database access and backend implementation details.

### Difficult Maintenance

If a business rule changes, every application may need to be updated and tested.

A Web API centralizes this logic:

```text
                 ┌── Website
                 ├── Android
                 └── iOS
                     ↓
                   Web API
                     ↓
                  Database
```

<br>

# 3. Role of a Web API

<br>
<div align="center">
  <p><b>General Core Workflow for All API Frameworks</b></p>
  <img width="600" alt="General core workflow for all API frameworks" src="https://github.com/user-attachments/assets/8d4062e5-a4db-4403-a84e-c284f65c558f" />
</div>
<br>


A Web API acts as an intermediary between clients and backend resources.

```text
Client
  ↓
Web API
  ↓
Business Logic
  ↓
Database / Storage / External Services
```

The API can handle:

- Data retrieval
- Data storage
- Business rules
- Database operations
- Authentication/authorization
- Cloud storage
- Third-party services

### Main Benefits

**Code Reusability**

One backend implementation can serve many clients.

**Uniformity**

All clients use the same business rules.

**Security**

The database and backend operations are not directly exposed to clients.

**Scalability**

New clients can consume the existing API.

> The API acts as a controlled entry point to backend resources.

<br>

# 4. What is a Web API?

**API = Application Programming Interface**

A Web API is a **concept**, not a specific technology.

It allows different applications/systems to communicate over the web, commonly using **HTTP**.

Example:

```text
Angular App
     ↓ HTTP
ASP.NET Core Web API
     ↓
Database
```

An API can be consumed by:

- Web applications
- Mobile applications
- Desktop applications
- Third-party applications
- Other backend services

Web APIs can be built using many technologies:

```text
Java
PHP
Node.js
Python
.NET
```

<br>

# 5. ASP.NET Web API vs ASP.NET Core

### ASP.NET Web API 2

- Based on the older .NET Framework
- Primarily associated with Windows/.NET Framework applications

### ASP.NET Core

- Modern .NET web framework
- Cross-platform
- Open-source
- Designed for modern web applications and APIs

For new development, **ASP.NET Core is generally the preferred choice**.

<br>

# 6. REST

**REST = Representational State Transfer**

REST is an **architectural style**, not a technology or framework.

A Web API is called **RESTful** when it follows REST principles/constraints.

REST focuses on representing and manipulating **resources** through standard web concepts.

Example resource:

```text
Movie
```

REST-style API:

```http
GET    /api/movies
GET    /api/movies/10
POST   /api/movies
PUT    /api/movies/10
PATCH  /api/movies/10
DELETE /api/movies/10
```

Important REST principles include:

- Resource-oriented URLs
- Standard HTTP methods
- Appropriate HTTP status codes
- Stateless communication

<br>

# 7. HTTP

**HTTP = HyperText Transfer Protocol**

HTTP is the protocol commonly used for communication between clients and web servers.

```text
Client
  │
  │ HTTP Request
  ↓
Server
  │
  │ HTTP Response
  ↓
Client
```

### Clients can include

- Web browser
- Mobile application
- Postman
- Swagger/OpenAPI UI
- Another backend service

HTTP enables communication between distributed systems, even when the client and server are physically located in different places.

<br>




<div align="center">
  <p><b>v7 - Request Response</b></p>
  <img width="700" alt="v7 - Request response" src="https://github.com/user-attachments/assets/b42d292c-9279-44c9-bf37-8df7f8a58385" />
</div>



# 8. HTTP Request

An HTTP request is sent by the client to the server.

```text
HTTP Request
├── URI / URL
├── HTTP Method
├── Headers
└── Body
```

Example:

```http
POST /api/movies
Content-Type: application/json

{
    "name": "Inception",
    "year": 2010
}
```

### URL / URI

Identifies the target resource.

```text
/api/movies
```

### HTTP Method

Specifies the intended operation.

```text
POST
```

### Headers

Metadata about the request.

Example:

```http
Content-Type: application/json
Authorization: Bearer <token>
```

### Body

Contains the data/payload sent to the server.

<br>

# 9. HTTP Response

After processing the request, the server sends an HTTP response.

```text
HTTP Response
├── Status Code
├── Response Headers
└── Response Body
```

Example:

```http
HTTP/1.1 200 OK
Content-Type: application/json

{
    "id": 10,
    "name": "Inception",
    "year": 2010
}
```

### Status Code

Indicates the result of the request.

### Response Headers

Metadata describing the response.

### Response Body

Contains returned data, if any.

<br>

# 10. HTTP Methods / Verbs


<br>
<table align = "center">
  <tr>
    <td align="center"><b>GET</b></td>
    <td align="center"><b>POST</b></td>
  </tr>
  <tr>
    <td align="center">
      <img width="300" alt="GET" src="https://github.com/user-attachments/assets/c7f0d3a2-4f26-4cef-b3dd-9b5f53e89f18" />
    </td>
    <td align="center">
      <img width="300" alt="POST" src="https://github.com/user-attachments/assets/2ca35d14-da1b-4901-a195-e44dc7b19f39" />
    </td>
  </tr>
  <tr>
    <td align="center"><b>PUT/PATCH</b></td>
    <td align="center"><b>DELETE</b></td>
  </tr>
  <tr>
    <td align="center">
      <img width="300" alt="PUT/PATCH" src="https://github.com/user-attachments/assets/441274a5-d174-4710-90bb-edd310908495" />
    </td>
    <td align="center">
      <img width="300" alt="DELETE" src="https://github.com/user-attachments/assets/b68a7dbc-1a46-43f9-a83f-facce340159b" />
    </td>
  </tr>
</table>
<br>


| Method | Purpose |
|---|---|
| GET | Retrieve data |
| POST | Create a new resource |
| PUT | Replace/update the entire resource |
| PATCH | Partially update a resource |
| DELETE | Remove a resource |

## GET

Retrieve data:

```http
GET /api/books
```

Filtering can use query parameters:

```http
GET /api/books?author=Robert
```

## POST

Create a new resource:

```http
POST /api/books
```

```json
{
    "title": "Clean Code",
    "author": "Robert Martin"
}
```

## PUT

Full replacement/update of an existing resource:

```http
PUT /api/books/10
```

Conceptually, the client provides the new representation of the resource.

## PATCH

Partial update:

```http
PATCH /api/books/10
```

```json
{
    "title": "Clean Code - Updated"
}
```

Only the specified property is changed.

## DELETE

Remove a resource:

```http
DELETE /api/books/10
```

Deletion can be:

**Hard delete**

```text
Record is permanently removed.
```

**Soft delete**

```text
Record remains but is marked inactive/deleted.
```

Example:

```text
IsDeleted = true
```

> [!NOTE]
> **PUT** is generally used to replace the resource representation, while **PATCH** is used for a partial update.

<br>

# 11. HTTP Status Code Categories

Status codes communicate the result of a request.

```text
1xx → Informational
2xx → Success
3xx → Redirection
4xx → Client Error
5xx → Server Error
```

### 1xx — Informational

The request has been received and processing is continuing.

### 2xx — Success

The request was successfully received, understood, and processed.

### 3xx — Redirection

The client needs to take further action, commonly involving a different URI.

### 4xx — Client Error

There is a problem with the request from the client.

Examples:

```text
Invalid request
Unauthenticated request
Resource not found
Unsupported HTTP method
```

### 5xx — Server Error

The server failed while processing an otherwise valid request.

Examples:

```text
Unhandled exception
Internal server failure
Unavailable service
```

<br>

# 12. Frequently Used Status Codes

## 200 OK

Request succeeded and the server returns the requested result.

```http
GET /api/movies/10
→ 200 OK
```

## 201 Created

A new resource was successfully created.

Commonly returned after `POST`.

```http
POST /api/movies
→ 201 Created
```

## 204 No Content

Request succeeded, but there is no response body.

```http
DELETE /api/movies/10
→ 204 No Content
```

## 301 Moved Permanently

The resource has permanently moved to another URI.

## 302 Found

The resource is temporarily available at another URI.

## 400 Bad Request

The server cannot process the request because the request/data is invalid or malformed.

## 401 Unauthorized

Authentication is required or the supplied authentication credentials are invalid.

Think:

```text
"You have not successfully authenticated."
```

## 404 Not Found

The requested resource does not exist.

```http
GET /api/movies/999
→ 404 Not Found
```

## 405 Method Not Allowed

The HTTP method is not supported for that endpoint.

```text
Endpoint supports GET
Client sends POST
→ 405 Method Not Allowed
```

## 500 Internal Server Error

An unexpected server-side error occurred.

Example:

```text
Unhandled exception
→ 500 Internal Server Error
```

## 503 Service Unavailable

The server is temporarily unable to handle the request.

Possible reasons:

- Maintenance
- Overload
- Temporary service failure

<br>

---

<br>

# 13. Typical CRUD API

For a `Movie` resource:

| Operation | Method | Endpoint |
|---|---|--|
| Get all | GET | `/api/movies` |
| Get one | GET | `/api/movies/{id}` |
| Create | POST | `/api/movies` |
| Replace/update | PUT | `/api/movies/{id}` |
| Partial update | PATCH | `/api/movies/{id}` |
| Delete | DELETE | `/api/movies/{id}` |

This is the basic pattern to remember:

```text
Collection:
GET    /movies
POST   /movies

Individual resource:
GET    /movies/{id}
PUT    /movies/{id}
PATCH  /movies/{id}
DELETE /movies/{id}
```
