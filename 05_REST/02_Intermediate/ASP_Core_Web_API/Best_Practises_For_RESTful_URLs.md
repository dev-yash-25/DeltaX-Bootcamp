# Best Practices for RESTful URLs for CRUD operations 

<br>

---

> [!Tip]
> ### [Revise - Best Practises of Design](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/05_REST/01_Fundamentals/01_RESTful_API.md#best-practises-of-design)

---

<br>



This tutorial focuses on designing clean, professional, and standard-compliant RESTful URLs for Web APIs using *ASP.NET Core 5.0*.

### Core Principles of RESTful URLs
* **Structure:** RESTful URLs are defined by combining a **resource** (e.g., `employees`) with an **HTTP verb** (e.g., `GET`, `POST`, `PUT`, `DELETE`). The URL should represent the resource, not the action (0:13-0:36).
* **Versioning:** It is highly recommended to include a versioning strategy in your URLs. Examples include `example.com/api/v1/employees` or using a query string like `?api-version=1` (1:02-1:57).
* **Resource Naming:** Always use plural nouns for resource collections (e.g., `employees` instead of `employee`). Use the resource name in the URL and the HTTP verb to define the intent (2:01-2:13).

<br>

**Request**
```
GET     → ❌ Usually no body
POST    → ✅ Body
PUT     → ✅ Body
PATCH   → ✅ Body
DELETE  → ⚠️ Usually no body
```
**Response**
```
GET     → ✅ Usually has body (requested data)
POST    → ✅ Usually has body (created resource/result)
PUT     → ✅ Usually has body (updated resource/result)
PATCH   → ✅ Usually has body (updated resource/result)
DELETE  → ⚠️ May have body, but often empty (204)
```
**Rule**
```
REQUEST BODY
----------------------------------------------
POST / PUT / PATCH → usually send data
GET                 → usually don't send data

RESPONSE BODY
----------------------------------------------
GET / POST / PUT / PATCH → usually return data
DELETE                   → often no body
```

<br>

### CRUD Operations Best Practices

#### 1. GET (Read)
* **Get All:** Use the base resource endpoint: `GET /employees` (0:45-0:55).
* **Get Single Item:** Append the unique identifier: `GET /employees/{id}` (2:16-2:41).
* **Nested Resources:** To retrieve data associated with a specific resource, chain them: `GET /employees/{id}/accounts` for all accounts of an employee, or `GET /employees/{id}/accounts/{accountid}` for a specific account (3:19-3:52).

<br>
<div align = "center">
  <img width="550" alt="image" src="https://github.com/user-attachments/assets/17b9ad1e-b62c-4443-b060-187142ef8344" />
</div>
<br>

#### 2. POST (Create)
* **Add Resource:** Use the base URL `POST /employees`. The actual data object is sent in the request body, not in the URL (4:03-4:25).
* **Add Nested Resource:** Follow the same logic: `POST /employees/{id}/accounts`. The URL identifies the parent, while the body provides the details for the new nested resource (4:29-5:08).

<br>
<div align = "center">
  <img width="550" alt="image" src="https://github.com/user-attachments/assets/5a71b321-0660-43c9-a43f-a00476385661" />
</div>
<br>


#### 3. PUT (Update)
* **Update Resource:** Use the specific resource URL: `PUT /employees/{id}`. Include the updated properties in the request body (5:15-5:25).
* **Update Nested Resource:** Use the full path: `PUT /employees/{id}/accounts/{accountid}`. The specific resource being updated must be clearly defined in the URL (5:28-5:47).




<br>
<div align = "center">
<img width="550" alt="image" src="https://github.com/user-attachments/assets/3e8a41a7-cfb0-4d51-9263-79062a9252d6" />
</div>
<br>


#### 4. DELETE (Remove)
* **Delete Resource:** Similar to other operations, target the specific resource URL: `DELETE /employees/{id}` (5:51-6:02).
* **Delete Nested Resource:** Use the specific path: `DELETE /employees/{id}/accounts/{accountid}`. The URL pattern remains consistent across all operations, making the API intuitive and easy to navigate (6:15-6:34).


<br>
<div align = "center">
<img width="550" alt="image" src="https://github.com/user-attachments/assets/561821ae-72ca-4dbe-b267-71c7ce93527a" />
</div>
<br>


### Instructor Observations
* The instructor emphasizes that URLs should be resource-centric rather than action-centric (avoid names like `/addEmployee` or `/getEmployeeDetails` in the URL).
* Consistency is key; by reusing the same URL structures and applying different HTTP verbs, you create a standard, predictable API interface.
