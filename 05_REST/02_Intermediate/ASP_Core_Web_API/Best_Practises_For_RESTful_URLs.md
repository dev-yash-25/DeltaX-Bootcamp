# Best Practices for RESTful URLs for CRUD operations 


<br>


## Index

- [1. Core Principles of RESTful URLs](#1-core-principles-of-restful-urls)
- [2. Request & Response Body](#2-request--response-body)
- [3. CRUD Operations Best Practices](#3-crud-operations-best-practices)
  - [3.1 GET — Read](#31-get--read)
  - [3.2 POST — Create](#32-post--create)
  - [3.3 PUT — Update](#33-put--update)
  - [3.4 DELETE — Remove](#34-delete--remove)
- [4. Instructor Observations](#4-instructor-observations)


<br>

---

> [!Tip]
> ### [Revise - Best Practises of Design](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/01_Fundamentals/01_RESTful_API.md#best-practises-of-design)

---


<br>




## 1. Core Principles of RESTful URLs

### 1.1 Resource-Centric URLs

RESTful URLs should represent **resources**, while the **HTTP verb** represents the operation being performed.

```text
Resource → employees
HTTP Verb → GET / POST / PUT / DELETE
```

For example:

```text
GET    /employees
POST   /employees
PUT    /employees/{id}
DELETE /employees/{id}
```

> [!Important]
> Avoid action-oriented URLs such as:
>
> ```text
> ❌ /addEmployee
> ❌ /getEmployeeDetails
> ❌ /deleteEmployee
> ```
>
> Prefer resource-oriented URLs:
>
> ```text
> ✅ POST   /employees
> ✅ GET    /employees/{id}
> ✅ DELETE /employees/{id}
> ```

<br>

### 1.2 Versioning

It is recommended to have a **versioning strategy** for APIs.

```text
https://example.com/api/v1/employees
```

Another approach is to specify the version using a query string:

```text
https://example.com/api/employees?api-version=1
```

<br>

### 1.3 Resource Naming

Use **plural nouns** for resource collections.

```text
✅ /employees
❌ /employee
```

The resource name identifies the collection, while the HTTP verb defines the intent.

<br>

---

## 2. Request & Response Body

### Request Body

```text
GET     → ❌ Usually no body
POST    → ✅ Body
PUT     → ✅ Body
PATCH   → ✅ Body
DELETE  → ⚠️ Usually no body
```

### Response Body

```text
GET     → ✅ Usually has body (requested data)
POST    → ✅ Usually has body (created resource/result)
PUT     → ✅ Usually has body (updated resource/result)
PATCH   → ✅ Usually has body (updated resource/result)
DELETE  → ⚠️ May have body, but often empty (204)
```

### Rule

```text
REQUEST BODY
----------------------------------------------
POST / PUT / PATCH → usually send data
GET                 → usually don't send data

RESPONSE BODY
----------------------------------------------
GET / POST / PUT / PATCH → usually return data
DELETE                   → often no body
```

> [!Note]
> These are **usual conventions**, not absolute restrictions.

<br>

---

## 3. CRUD Operations Best Practices

CRUD stands for:

```text
C → Create → POST
R → Read   → GET
U → Update → PUT
D → Delete → DELETE
```

### 3.1 GET — Read

#### Get All

```http
GET /employees
```

#### Get Single Item

```http
GET /employees/{id}
```

Example:

```http
GET /employees/101
```

#### Nested Resources

Get all accounts of an employee:

```http
GET /employees/{id}/accounts
```

Get a specific account:

```http
GET /employees/{id}/accounts/{accountid}
```

Example:

```http
GET /employees/101/accounts/5001
```

<br>

<div align="center">
  <img width="550" alt="GET operation" src="https://github.com/user-attachments/assets/17b9ad1e-b62c-4443-b060-187142ef8344" />
</div>

<br>

---

### 3.2 POST — Create

#### Add Resource

```http
POST /employees
```

The actual data is sent in the **request body**, not in the URL.

Example:

```json
{
    "name": "Yash",
    "department": "IT"
}
```

#### Add Nested Resource

```http
POST /employees/{id}/accounts
```

Example:

```http
POST /employees/101/accounts
```

The URL identifies the **parent**, while the body provides the details of the new nested resource.

<br>

<div align="center">
  <img width="550" alt="POST operation" src="https://github.com/user-attachments/assets/5a71b321-0660-43c9-a43f-a00476385661" />
</div>

<br>

---

### 3.3 PUT — Update

#### Update Resource

```http
PUT /employees/{id}
```

Example:

```http
PUT /employees/101
```

Include the updated properties in the request body.

#### Update Nested Resource

```http
PUT /employees/{id}/accounts/{accountid}
```

Example:

```http
PUT /employees/101/accounts/5001
```

<br>

<div align="center">
  <img width="550" alt="PUT operation" src="https://github.com/user-attachments/assets/3e8a41a7-cfb0-4d51-9263-79062a9252d6" />
</div>

<br>

---

### 3.4 DELETE — Remove

#### Delete Resource

```http
DELETE /employees/{id}
```

Example:

```http
DELETE /employees/101
```

#### Delete Nested Resource

```http
DELETE /employees/{id}/accounts/{accountid}
```

Example:

```http
DELETE /employees/101/accounts/5001
```

The URL pattern remains consistent across CRUD operations.

<br>

<div align="center">
  <img width="550" alt="DELETE operation" src="https://github.com/user-attachments/assets/561821ae-72ca-4dbe-b267-71c7ce93527a" />
</div>

<br>

---

## 4. Instructor Observations

### Resource-Centric, Not Action-Centric

URLs should identify **resources**, not actions.

```text
❌ /addEmployee
❌ /getEmployeeDetails
❌ /deleteEmployee
```

Instead:

```text
✅ POST   /employees
✅ GET    /employees
✅ DELETE /employees/{id}
```

The **HTTP method** communicates the action.

<br>

### Consistency

Reuse the same URL structure and change the HTTP verb according to the operation.

```text
GET    /employees
POST   /employees

GET    /employees/{id}
PUT    /employees/{id}
DELETE /employees/{id}
```

For nested resources:

```text
GET    /employees/{id}/accounts
POST   /employees/{id}/accounts

GET    /employees/{id}/accounts/{accountid}
PUT    /employees/{id}/accounts/{accountid}
DELETE /employees/{id}/accounts/{accountid}
```

> [!Tip]
> Think:
>
> ```text
> URL  → What resource?
> Verb → What operation?
> Body → What data?
> ```

<br>

---

## CRUD URL Pattern — At a Glance

| Operation | HTTP Method | URL |
|---|---|---|
| Get all | `GET` | `/employees` |
| Get one | `GET` | `/employees/{id}` |
| Create | `POST` | `/employees` |
| Update | `PUT` | `/employees/{id}` |
| Delete | `DELETE` | `/employees/{id}` |
| Get nested collection | `GET` | `/employees/{id}/accounts` |
| Create nested resource | `POST` | `/employees/{id}/accounts` |
| Get nested resource | `GET` | `/employees/{id}/accounts/{accountid}` |
| Update nested resource | `PUT` | `/employees/{id}/accounts/{accountid}` |
| Delete nested resource | `DELETE` | `/employees/{id}/accounts/{accountid}` |

<br>

---

> [!Tip]
> ### REST URL Design Formula
>
> ```text
> Collection:
> /employees
>
> Specific resource:
> /employees/{id}
>
> Nested collection:
> /employees/{id}/accounts
>
> Specific nested resource:
> /employees/{id}/accounts/{accountid}
> ```
>
> Then use the appropriate HTTP verb to express the operation.
