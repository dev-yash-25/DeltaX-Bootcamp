# REST API Endpoint Examples

<br>

> [!Important]
> #### When its Actual entity, that can exists seperately, its `ChildController`
> ```
> GET /cart/{cartId}/items/{itemId}
> ```
> Its **ItemsController**
>
> <br>
>
> #### When its Property/Attribute, its `ParentController`
> ```
> GET /products/{productId}/discounts
> ```
> Its **ProductsController** not **DiscountsController**
> ```
> PATCH /task/{taskId}/status
> ```
> Its **TasksController** not **StatusController**
> ```
> PATCH /orders/{orderId}/status
> ```
> Its **OrdersController** not **StatusController**
>
> As status, discounts are property, status can exist as seperate entity, then we use enum in Resource jsons


<br>

## Index

**Prerequisites:**
1. [How to Think About an Endpoint](#1-how-to-think-about-an-endpoint)
2. [HTTP Method Cheat Sheet](#2-http-method-cheat-sheet)
3. [Controller Selection — Main Rule](#3-controller-selection--main-rule)

**Examples:**

4. [Users & Authentication](#4-users--authentication)
5. [Tasks](#5-tasks)
6. [Nested Resources](#6-nested-resources)
7. [E-Commerce Platform](#7-e-commerce-platform)
8. [Social Media Platform](#8-social-media-platform)
9. [Order Management — Controller Questions](#9-order-management--controller-questions)
10. [Important Controller Corrections We Resolved](#10-important-controller-corrections-we-resolved)

**Concepts and Observations:**

11. [The JWT Rule — Very Important](#11-the-jwt-rule--very-important)
12. [Route Parameter vs Query Parameter](#12-route-parameter-vs-query-parameter)
13. [Nested Resource Mental Model](#13-nested-resource-mental-model)
14. [Complete Mental Model](#14-complete-mental-model)
15. [Final Rules to Memorize](#15-final-rules-to-memorize)
16. [One Question to Ask Yourself in an Interview](#16-one-question-to-ask-yourself-in-an-interview)

<br>

# 1. How to Think About an Endpoint

When designing an endpoint, ask these questions in order:

## 1.1 What resource am I operating on?

Examples:

```text
/users
/tasks
/orders
/products
/posts
```

This usually helps determine the **controller**.

## 1.2 What operation am I performing?

```text
POST    → Create
GET     → Retrieve
PUT     → Update/replace complete resource
PATCH   → Update part of a resource
DELETE  → Delete/remove
```

## 1.3 Am I identifying a specific resource?

If yes, use a **route parameter**:

```http
GET /products/{productId}
```

## 1.4 Am I filtering, sorting, paginating?

If yes, use **query parameters**:

```http
GET /tasks?page=2&size=10&sortBy=priority
```

## 1.5 Is there a parent-child relationship?

> [!Tip]
> Look around the concept of [Ownership](https://github.com/Yash-Bandal/DeltaX-Bootcamp/blob/main/05_REST/02_Intermediate/ASP_Core_Web_API/Ownership_%26_Lifecycle.md)

If yes, a nested route may be appropriate:

```http
GET /projects/{projectId}/tasks
GET /tasks/{taskId}/comments
GET /products/{productId}/reviews
```

## 1.6 Is the logged-in user already known from JWT?

If the backend gets the user ID from the access token, **do not unnecessarily put `{userId}` in the route**.

For example:

```http
POST /orders
```

instead of:

```http
POST /users/{userId}/orders
```

because the backend can get the user ID from the JWT.

<br>

# 2. HTTP Method Cheat Sheet

| Method | Use |
|---|---|
| `GET` | Retrieve |
| `POST` | Create |
| `PUT` | Update/replace the complete resource |
| `PATCH` | Update specific fields/part of resource |
| `DELETE` | Delete/remove |

## 2.1 PUT vs PATCH

Your mental model:

> **PUT** - Method that is used when we want to update the entire column/all attributes of a resource.

> **PATCH** - Method that is used when we want to update a specific cell/field of a column/attribute without replacing a complete resource.

More simply:

```text
PUT
→ Complete resource update

PATCH
→ Partial resource update
```

<br>

# 3. Controller Selection — Main Rule

The controller is generally based on the **resource being operated on**, especially the resource that the endpoint is actually managing.

But for nested resources, ask:

> **Which resource is actually being managed?**

The parent ID may simply provide context.

Example:

```http
GET /projects/{projectId}/tasks
```

The requirement is:

> Get the Tasks belonging to this particular Project.

The resource being retrieved is **Tasks**.

Therefore:

```text
TasksController
```

not necessarily:

```text
ProjectsController
```

<br>

# 4. Users & Authentication

## 4.1 Create User

### Registration

```http
POST /api/auth
```

or, depending on the API's distinction between authentication and user administration:

```http
POST /api/users
```

Request:

```json
{
  "name": "Yash",
  "email": "yb@gdeltax.com",
  "password": "strongP"
}
```

Response:

```text
201 Created
```

### Important distinction from our discussion

```text
POST /api/auth
```

> Used when user is added directly at Register time.

```text
POST /api/users
```

> Used when, example, an admin wants to add a complete new user, with all details.

So the route depends on **what the operation represents**:

```text
Registration
    ↓
Auth-related operation

Admin/user management
    ↓
UsersController
```

## 4.2 Update Profile

```http
PATCH /api/users/{userId}
```

Request:

```json
{
  "name": "Yash Bandal",
  "email": "yb@gdeltax.com"
}
```

Response:

```text
200 OK
```

### Why PATCH?

> Here, we are just updating the name and email of the Profile, not the entire Profile resource, thus PATCH is used for updation.

> PUT will be appropriate when the complete resource is being replaced/updated.

## 4.3 Login

Authentication has its own controller because it deals with authentication logic.

```http
POST /api/auth/login
```

Request:

```json
{
  "email": "yb@gdeltax.com",
  "password": "strongP"
}
```

Response:

```json
{
  "token": "JWT_Token"
}
```

Response:

```text
200 OK
```

Controller:

```text
AuthController
```

Reason:

> This endpoint handles authentication and login functionality. Since it is associated with authentication logic, it should be placed in AuthController.

## 4.4 Logout 🏷️

```http
POST /api/auth/logout
```

No request body.

Response:

```text
204 No Content
```
> [!Tip]
> **Question** - Give me a case, where we perform **POST**, not **DELETE**, and still return ***204 No Content***

<br>

# 5. Tasks

Controller:

```text
TasksController
```

## 5.1 Task CRUD

### Create Task

```http
POST /api/tasks
```

Request:

```json
{
  "title": "REST-Assignment-1",
  "description": "Perform API Design Task",
  "status": "pending",
  "priority": "high",
  "dueDate": "2026-09-25"
}
```

Response:

```text
201 Created
```

### Retrieve Tasks

Get all:

```http
GET /api/tasks
```

Get one:

```http
GET /api/tasks/{taskId}
```

### Think 🏷️
> [!Important]
> ```
> GET /api/projects/{projectId}/tasks
> ```
> or we shall have
> ```
> GET /tasks?projectId=x
> ```
>
> Both syntaxes valid, but ✅
> ```
> GET /api/projects/{projectId}/tasks
> ```
> See, here we are not filtering a task, we are retriving a specific task
>
> Also here, task is dependent on project,  There is a strong `parent-child relationship`
> 
> Like: a project has a collection of tasks, and you're asking specifically for that project's tasks.
> 
> When independent Task as a resource, then second can be done, but thats rare 


Here,\
No request body.

Response:

```text
200 OK
```

<br>

---
### Note
#### **Q. Why do we need project Id in Tasks, if api tells us directly `/project/{id}/tasks/`?**

**Project JSON**
```json
{
  "id": 10,
  "name": "IMDB API",
  "description": "Build the movie API"
}
```

**Task JSON**
```json
{
  "id": 101,
  "title": "Implement JWT Authentication",
  "projectId": 10,
  "status": "Completed"
}
```
**Answer:**

Because `ProjectId` in `Task` is the **foreign key that tells us which Project the Task belongs to**.

Don't just think in API perspective, we need to think in DB and overall perspective


```text
Project
Id = 10
   ↑
   │ ProjectId = 10
Task
Id = 101
```

Without `ProjectId`:

```json
{
  "id": 101,
  "title": "Implement JWT"
}
```

we know the Task exists, but **we don't know which Project it belongs to**.

With it:

```json
{
  "id": 101,
  "title": "Implement JWT",
  "projectId": 10
}
```

we know:

> Task 101 belongs to Project 10.

That's exactly what allows:

```http
GET /api/projects/10/tasks
```

to find the Tasks where:

```sql
WHERE ProjectId = 10
```


```text
Project.Id
     ↑
     │ FK relationship
Task.ProjectId
```

---

<br>

### Update Task

```http
PATCH /api/tasks/{taskId}
```

Request:

```json
{
  "status": "completed",
  "priority": "high"
}
```

Response:

```text
200 OK
```

#### Why PATCH?

Only selected fields are being changed.

```text
Task
 ├── title
 ├── description
 ├── status       ← changing
 ├── priority     ← changing
 └── dueDate
```

We are not replacing the complete Task.

### Delete Task

```http
DELETE /api/tasks/{taskId}
```

No request body.

Response:

```text
204 No Content
```

## 5.2 Filtering

Controller:

```text
TasksController
```

Use query parameters for filtering.

```http
GET /api/tasks?status=pending&priority=high&dueDate=2026-09-25
```

No request body.

Response:

```text
200 OK
```

### Why query parameters?

Because we are still retrieving:

```text
/tasks
```

We are only specifying **which tasks we want**.

## 5.3 Pagination + Sorting + Filtering

All of these are retrieval options, so they go into query parameters.

### By priority

```http
GET /api/tasks?page=2&size=10&sortBy=priority&sortOrder=asc
```

### By due date

```http
GET /api/tasks?page=2&size=10&sortBy=dueDate&sortOrder=asc
```

### By status

```http
GET /api/tasks?page=2&size=10&sortBy=status&sortOrder=asc
```

### Combined

```http
GET /api/tasks?page=2&size=10&sortBy=dueDate&sortOrder=asc&status=pending&priority=high
```

No request body.

Response:

```text
200 OK
```

Mental model:

```text
/tasks
   ↓
Which tasks?

?page=2
&size=10
&sortBy=dueDate
&sortOrder=asc
&status=pending
&priority=high
```

## 5.4 Task Status

Controller:

```text
TasksController
```

```http
PATCH /api/tasks/{taskId}/status
```

Request:

```json
{
  "status": "completed"
}
```

Status type:

```csharp
enum TaskStatus
{
    completed,
    InProgress,
    pending
}
```

Response:

```text
200 OK
```

### Why `/status`?

We are specifically modifying the **status property of a Task**.

We are not treating Status as a separate entity.

Therefore:

```text
TasksController
```

not:

```text
StatusController
```

We would use something like a `StatusController` only if **Status itself were an independent entity/table/resource** that we were managing separately.

<br>

# 6. Nested Resources

## 6.1 Projects → Tasks

### Create Project

Controller:

```text
ProjectsController
```

```http
POST /api/projects
```

Request:

```json
{
  "title": "REST-Project-1",
  "description": "API Designs"
}
```

Response:

```text
201 Created
```

### Get Tasks of a Project

Controller:

```text
TasksController
```

```http
GET /api/projects/{projectId}/tasks
```

No request body.

Response:

```text
200 OK
```

#### Why TasksController?

> Tasks are the resources that are being retrieved under a specific project, projectId gives the context of the Parent resource - Project.

Mental model:

```text
Project
   │
   └── Tasks
          ↑
       resource
       being retrieved
```

Therefore:

```text
TasksController
```

## 6.2 Tasks → Subtasks

Controller:

```text
SubtasksController
```

### Create Subtask

```http
POST /api/tasks/{taskId}/subtasks
```

Request:

```json
{
  "title": "Subtask-1",
  "description": "Handle user auth requests"
}
```

Response:

```text
201 Created
```

### Get Specific Subtask

```http
GET /api/tasks/{taskId}/subtasks/{subtaskId}
```

No request body.

Response:

```text
200 OK
```

### Why SubtasksController? 🔖🏷️

Because the resource being operated on is:

```text
Subtask
```

while:

```text
taskId
```

Task
```json
{
  "id": 101,
  "title": "Build Movie API",
  "status": "In Progress"
}
```
Subtask
```json
{
  "id": 201,
  "title": "Implement JWT",
  "status": "Completed",
  "taskId": 101
}
```
provides the parent Task context.

## 6.3 Tasks → Comments

Controller:

```text
CommentsController
```

### Create Comment

```http
POST /api/tasks/{taskId}/comments
```

Request:

```json
{
  "comment": "Use JWT Auth"
}
```

Response:

```text
201 Created
```

### Why CommentsController?

> Comments is a child resource of a Task.

So:

```text
Task
 └── Comment
```

The resource being created is the **Comment**.

Therefore:

```text
CommentsController
```

### Important distinction

If instead Comment was simply treated as an attribute/property inside the Task request:

```json
{
  "taskId": 1,
  "name": "Update Auth",
  "comment": "Update the current task"
}
```

then it would be handled as part of the Task operation:

```text
TasksController
```

So:

```text
Separate Comment resource
        ↓
CommentsController

Comment just an attribute of Task
        ↓
TasksController
```

## 6.4 User → Tasks

Retrieve tasks assigned to a specific user:

```http
GET /api/users/{userId}/tasks
```

Controller:

```text
TasksController
```

No request body.

Response:

```text
200 OK
```

Again:

> Tasks are the resources being retrieved; `userId` gives the context of which user's tasks we want.

<br>

# 7. E-Commerce Platform

## 7.1 Retrieve Products

```http
GET /products
```

Response:

```json
[
  {
    "Product1": {}
  },
  {
    "Product2": {}
  },
  {
    "Productn": {}
  }
]
```

Response:

```text
200 OK
```

Controller:

```text
ProductsController
```

## 7.2 Retrieve Product Details

```http
GET /products/{productId}
```

Example:

```http
GET /products/1
```

Response:

```json
{
  "id": 1,
  "name": "Book1",
  "description": "Books of 2025",
  "price": 400
}
```

Response:

```text
200 OK
```

Controller:

```text
ProductsController
```

Reason:

> The main resource is Product and the endpoint is associated with retrieving a specific product.

## 7.3 Cart → Items

### Add Item to Cart

```http
POST /carts/{cartId}/items
```

Request:

```json
{
  "productId": 1,
  "quantity": 2
}
```

Response:

```json
{
  "message": "Item added to cart"
}
```

Response:

```text
200 OK
```

Controller:

```text
ItemsController
```

#### Important correction

We use:

```text
ItemsController
```

instead of:

```text
CartItemsController
```

because `CartItem` is a **junction/relationship table** that connects a Cart with an Item; it is not treated as an independent resource in our API.

The:

```text
cartId
```

provides the context of the parent Cart, while:

```text
items
```

represents the resource being managed within that Cart.

Think of:

```text
GetCartItems()
```

inside:

```text
ItemController
```

### Retrieve Cart Items

```http
GET /carts/{cartId}/items
```

Response:

```json
[
  {
    "productId": 1,
    "name": "Book1",
    "quantity": 2,
    "price": 400
  },
  {
    "productId": 2,
    "name": "Book2",
    "quantity": 1,
    "price": 300
  }
]
```

Response:

```text
200 OK
```

Controller:

```text
ItemsController
```

### Delete Cart Item

```http
DELETE /carts/{cartId}/items/{itemId}
```

Response:

```json
{
  "message": "Item removed from cart"
}
```

Response:

```text
200 OK
```

Controller:

```text
ItemsController
```

## 7.4 Placing an Order — JWT User

### Correct

```http
POST /orders
```

Request:

```json
{
  "cartId": 10,
  "shippingAddress": "Pune, Maharashtra"
}
```

Response:

```json
{
  "orderId": 5001,
  "message": "Order placed successfully"
}
```

Response:

```text
200 OK
```

### Wrong

```http
POST /users/{userId}/orders
```

### Why?

The backend will retrieve the `userId` from the **access token**.

Mental model:

```text
Client
  │
  │ Authorization: Bearer JWT
  ↓
API
  │
  ├── JWT contains user identity
  │
  └── Backend gets userId
           ↓
        Create Order
```

So there is no need to send:

```text
/user/{userId}
```

when the authenticated user is already known.

Controller:

```text
OrdersController
```

## 7.5 Cancel Order

```http
PATCH /orders/{orderId}
```

Request:

```json
{
  "status": "cancelled"
}
```

Response:

```json
{
  "message": "Order cancelled successfully"
}
```

Response:

```text
200 OK
```

Controller:

```text
OrdersController
```

Why PATCH?

Because we are changing only:

```text
status
```

of the Order.

## 7.6 Get Previous Orders

### Correct

```http
GET /orders?startDate=YYYY-MM-DD&endDate=YYYY-MM-DD
```

Example:

```http
GET /orders?startDate=2026-02-01&endDate=2026-03-31
```

Response:

```json
[
  {
    "orderId": 5001,
    "orderDate": "2026-02-15",
    "totalAmount": 1200,
    "status": "Delivered"
  },
  {
    "orderId": 5002,
    "orderDate": "2026-03-10",
    "totalAmount": 800,
    "status": "Cancelled"
  }
]
```

Response:

```text
200 OK
```

### Why no userId?

Same JWT principle:

```text
GET /orders
```

The backend knows the authenticated user from the access token.

Then:

```text
?startDate=...
&endDate=...
```

only controls the date filtering.

Controller:

```text
OrdersController
```

<br>

# 8. Social Media Platform

## 8.1 Create Post

```http
POST /posts
```

Request:

```json
{
  "content": "Having a great day!",
  "imageUrl": "https://example.com/image.jpg"
}
```

Response:

```json
{
  "postId": 101,
  "message": "Post created successfully"
}
```

Response:

```text
201 Created
```

Controller:

```text
PostsController
```

## 8.2 Delete Post

```http
DELETE /posts/{postId}
```

Response:

```json
{
  "message": "Post deleted successfully"
}
```

Response:

```text
200 OK
```

Controller:

```text
PostsController
```

## 8.3 Comment on Post

```http
POST /posts/{postId}/comments
```

Request:

```json
{
  "content": "Great post!"
}
```

Response:

```json
{
  "commentId": 501,
  "message": "Comment added successfully"
}
```

Response:

```text
201 Created
```

Controller:

```text
CommentsController
```

Reason:

> Comment is a child resource of Post, and the Comment itself is the resource being created.

## 8.4 Like / Unlike a Post

### Like

```http
POST /posts/{postId}/likes
```

### Unlike

```http
DELETE /posts/{postId}/likes
```

Response:

```json
{
  "message": "Post liked/disliked successfully"
}
```

Response:

```text
200 OK
```

Mental model:

```text
POST
 ↓
Create like relationship

DELETE
 ↓
Remove like relationship
```

The like itself is a relationship rather than an independent business resource.

## 8.5 Get Posts of the Logged-in User

### Correct

```http
GET /posts
```

Authorization:

```text
Authorization: Bearer <token>
```

### Wrong

```http
GET /users/{userId}/posts
```

Response:

```json
[
  {
    "postId": 101,
    "content": "Having a great day!",
    "likes": 25,
    "comments": 5
  },
  {
    "postId": 102,
    "content": "Learning REST APIs",
    "likes": 40,
    "comments": 8
  }
]
```

Response:

```text
200 OK
```

### Why?

The backend gets the user identity from:

```text
Bearer <token>
```

So:

```text
GET /posts
```

means:

> Give me the posts belonging to the currently authenticated user.

We don't need:

```text
/users/{userId}
```

because the user ID is already available from the JWT.

## 8.6 Follow / Unfollow User

Here we corrected the original endpoint.

### Wrong

```http
POST /users/{userId}/following/{targetUserId}
DELETE /users/{userId}/following/{targetUserId}
```

### Correct

#### Follow

```http
POST /users/{targetUserId}/following
```

#### Unfollow

```http
DELETE /users/{targetUserId}/following
```

### Why?

The currently logged-in user is already identified from the JWT.

We only need to tell the API:

> Which user do I want to follow?

Therefore:

```text
JWT
 ↓
Current user

Route
 ↓
targetUserId
```

Mental model:

```text
Current User ──────→ Target User
   JWT                  route
```

No request body is required.

Response:

```json
{
  "message": "User followed/Unfollowed successfully"
}
```

Response:

```text
200 OK
```

## 8.7 Get Followers of a User

```http
GET /users/{userId}/followers
```

Response:

```json
[
  {
    "userId": 10,
    "name": "Yash"
  },
  {
    "userId": 20,
    "name": "Sourabh"
  }
]
```

Response:

```text
200 OK
```

### Why is userId correct here?

> It is correct to take userId here as user is the subject here.

Meaning:

```text
/users/10/followers
```

asks:

> Who follows User 10?

Here we are explicitly asking for the followers **of a particular user**.

So `{userId}` is necessary.

## 8.8 Get Following of a User

```http
GET /users/{userId}/following
```

Response:

```json
[
  {
    "userId": 30,
    "name": "U1"
  },
  {
    "userId": 40,
    "name": "U2"
  }
]
```

Response:

```text
200 OK
```

Same reasoning:

```text
/users/10/following
```

means:

> Which users does User 10 follow?

Here User 10 is the **subject of the query**, so `{userId}` is appropriate.

<br>

# 9. Order Management — Controller Questions 

These are the controller decisions we resolved. 🏷️

| Endpoint | Controller | Core reasoning |
|---|---|---|
| `POST /v1/orders` | `OrdersController` | Order is the primary resource |
| `GET /v1/products/{productId}` | `ProductsController` | Product is being retrieved |
| `POST /v1/carts/{cartId}/items` | `ItemsController` | Items are the resource being managed; Cart gives parent context |
| `POST /v1/users` | `UsersController` | User is the resource |
| `POST /v1/payments` | `PaymentsController` | Payment is the resource |
| `GET /v1/categories` | `CategoriesController` | Categories are being retrieved |
| `POST /v1/products/{productId}/reviews` | `ReviewsController` | Review is the resource being created |
| `PUT /v1/orders/{orderId}` | `OrdersController` | Complete Order is being updated |
| `POST /v1/users/{userId}/addresses` | `AddressesController` | Address is the resource being managed |
| `POST /v1/auth/login` | `AuthController` | Authentication logic |
| `PATCH /v1/orders/{orderId}/status` | `OrdersController` | Order's status is being changed |
| `PUT /v1/users/{userId}` | `UsersController` | User is being updated |
| `GET /v1/products/{productId}/discounts` | `ProductsController` 🏷️ | Product provides context; endpoint retrieves its discounts |
| `GET /v1/products/{productId}/availability` | `ProductsController` | Product is the primary resource |
| `GET /v1/users/{userId}/orders` | `OrdersController` | Orders are the resource being retrieved |

<br>

# 10. Important Controller Corrections We Resolved

## 10.1 Cart Items

Earlier:

```text
CartController
```

Corrected to:

```text
ItemsController
```

Because:

> `CartItem` is a junction/relationship table that connects a Cart with an Item; it is not treated as an independent resource in our API. The `cartId` provides the context of the parent Cart, while `items` represents the resource being managed within that Cart.

## 10.2 Product Reviews

Earlier:

```text
ProductsController
```

Corrected to:

```text
ReviewsController
```

Because:

> Reviews is a nested child resource of Products, and the endpoint is associated with creating a new review on a specific product. Thus the operation belongs to `ReviewsController`.

## 10.3 User Addresses

Earlier:

```text
UsersController
```

Corrected to:

```text
AddressesController
```

For a simple 1:1 relationship, if Address is treated as part of User:

```http
PUT /api/users/10
```

```json
{
  "name": "Yash",
  "address": {
    "city": "Pune",
    "pincode": "411001"
  }
}
```

Here:

```text
UsersController
```

makes sense because we are primarily updating the User.

But for a separate address collection/resource:

```http
POST /v1/users/{userId}/addresses
```

we use:

```text
AddressesController
```

## 10.4 Project → Tasks

```http
GET /api/projects/{projectId}/tasks
```

Correct controller:

```text
TasksController
```

because:

> Tasks are the resources that are being retrieved under a specific project, projectId gives the context of the Parent resource - Project.

## 10.5 Task → Comments

```http
POST /api/tasks/{taskId}/comments
```

Correct controller:

```text
CommentsController
```

because Comment is a separate child resource.

If Comment were merely an attribute inside Task, then:

```text
TasksController
```

would make sense.

## 10.6 Order → Status

```http
PATCH /v1/orders/{orderId}/status
```

Correct controller:

```text
OrdersController
```

because:

> Status is the property being operated as a resource, but we are not operating on Status as an Entity.

We do **not** use:

```text
StatusController
```

because we are not doing:

```http
PATCH /v1/status
```

on a separate Status entity/table.

<br>

# 11. The JWT Rule — Very Important

This correction appeared in multiple endpoint questions.

When the API already has:

```http
Authorization: Bearer <token>
```

the backend can identify the currently authenticated user.

Therefore, don't unnecessarily write:

```http
/users/{userId}
```

for operations concerning the **current logged-in user**.

## 11.1 Example

Wrong:

```http
POST /users/{userId}/orders
```

Correct:

```http
POST /orders
```

because:

```text
JWT
 ↓
Current user ID
 ↓
Create order for that user
```

## 11.2 Another example

Wrong:

```http
GET /users/{userId}/posts
```

for "get my posts".

Correct:

```http
GET /posts
```

with:

```text
Authorization: Bearer <token>
```

## 11.3 But this does NOT mean `{userId}` is always wrong

If we are asking about a **specific user as the subject**, then it is correct.

For example:

```http
GET /users/{userId}/followers
```

means:

> Give me the followers of this particular user.

And:

```http
GET /users/{userId}/following
```

means:

> Give me the users this particular user follows.

So the distinction is:

```text
CURRENT LOGGED-IN USER
        ↓
JWT already identifies them
        ↓
Usually don't need userId


SPECIFIC USER AS SUBJECT
        ↓
Need userId
```

<br>

# 12. Route Parameter vs Query Parameter

## 12.1 Route Parameter

Used to identify a resource or subject.

```http
GET /products/{productId}
```

Example:

```http
GET /products/10
```

Means:

> Give me Product 10.

## 12.2 Query Parameter

Used for filtering, sorting, pagination, date ranges, etc.

```http
GET /tasks?status=pending&priority=high
```

or:

```http
GET /orders?startDate=2026-02-01&endDate=2026-03-31
```

or:

```http
GET /tasks?page=2&size=10&sortBy=priority
```

Mental model:

```text
/products/10
        ↑
     WHICH ONE?


/products?category=books
        ↑
   HOW TO FILTER?


/orders?startDate=...
        ↑
   HOW TO FILTER?
```

<br>

# 13. Nested Resource Mental Model

Think:

```text
Parent
  │
  └── Child
```

Examples:

```text
Project
  └── Tasks

Task
  ├── Subtasks
  └── Comments

Product
  ├── Reviews
  ├── Discounts
  └── Availability

Cart
  └── Items

User
  ├── Followers
  ├── Following
  ├── Orders
  └── Addresses

Post
  ├── Comments
  └── Likes
```

But remember:

> **Nested URL does not automatically mean ParentController.**

Ask:

> **Which resource am I actually operating on?**

For example:

```http
POST /products/{productId}/reviews
```

Resource being created:

```text
Review
```

Therefore:

```text
ReviewsController
```

Whereas:

```http
GET /products/{productId}
```

resource being retrieved:

```text
Product
```

Therefore:

```text
ProductsController
```

<br>

# 14. Complete Mental Model

```text
                    ENDPOINT
                       │
                       ↓
              What am I managing?
                       │
          ┌────────────┴────────────┐
          ↓                         ↓
      Main resource           Child resource
          │                         │
          ↓                         ↓
    UsersController          ChildController
    TasksController           etc.
    OrdersController
    ProductsController
          │
          ↓
      What operation?
          │
 ┌────────┼─────────┬──────────┐
 ↓        ↓         ↓          ↓
GET      POST      PUT       PATCH
 ↓        ↓         ↓          ↓
Read    Create   Complete    Partial
                              update
          │
          ↓
      Need specific
         resource?
          │
       ┌──┴──┐
       ↓     ↓
      YES    NO
       ↓     ↓
   /{id}    collection
             │
             ↓
       Need filtering/
       sorting/paging?
             │
             ↓
        ?key=value
```

<br>

# 15. Final Rules to Memorize

## 15.1 CRUD

```text
POST   /resources
       → Create

GET    /resources
       → Get all

GET    /resources/{id}
       → Get one

PUT    /resources/{id}
       → Complete update

PATCH  /resources/{id}
       → Partial update

DELETE /resources/{id}
       → Delete
```

## 15.2 Filtering

```text
GET /resources?filter=value
```

## 15.3 Pagination

```text
GET /resources?page=2&size=10
```

## 15.4 Sorting

```text
GET /resources?sortBy=price&sortOrder=asc
```

## 15.5 Date range

```text
GET /orders?startDate=...&endDate=...
```

## 15.6 Nested resource

```text
GET /parent/{parentId}/children
```

## 15.7 Separate child controller

```text
/products/{productId}/reviews
             ↓
       ReviewsController
```

## 15.8 Parent context + child resource

```text
/projects/{projectId}/tasks
              ↓
        TasksController
```

## 15.9 JWT current-user rule

```text
JWT
 ↓
Current User
 ↓
Don't unnecessarily put {userId} in route
```

Example:

```text
POST /orders
GET  /posts
```

## 15.10 Specific user as subject

```text
GET /users/{userId}/followers
GET /users/{userId}/following
```

because we are explicitly asking about **that particular user**.

## 15.11 Controller rule

> **The URL tells you the structure, but the actual resource being operated on helps determine the controller.**

<br>

# 16. One Question to Ask Yourself in an Interview

If you get stuck, ask:

> **"What exactly am I operating on?"**

For example:

```text
POST /carts/10/items
```

What am I operating on?

```text
Items
```

→ `ItemsController`

```text
POST /products/10/reviews
```

What am I operating on?

```text
Reviews
```

→ `ReviewsController`

```text
GET /projects/10/tasks
```

What am I retrieving?

```text
Tasks
```

→ `TasksController`

```text
PATCH /orders/10/status
```

What am I changing?

```text
Order's status
```

→ `OrdersController`

```text
POST /orders
```

Who is the order for?

```text
Authenticated user from JWT
```

→ no `{userId}` required.

That is the core reasoning behind all the endpoint examples above.
