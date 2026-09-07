# What is Routing in ASP.NET Core Web API 


<br>
<div align = "center">
  <img width="500" alt="image" src="https://github.com/user-attachments/assets/553c84e3-a50a-495b-8718-f381645f3316" />
</div>
<br>

This tutorial explains the fundamental concept of **routing** within an *ASP.NET Core Web API* application. 

> [!Tip]
> There are `N` Controllers/
> Routing answers, "How the application will know, which request will go to whichcontroller?"

### Core Concept: Routing
* **Definition:** Routing is the process of mapping an incoming HTTP request to a specific resource (usually an action method within a controller) (1:09).
   ```
    https://orders.com/url/endpoint   ->  Resource-Order
   ```
* **Mechanism:** When a request is made from a client (like a browser or *Postman*), the application must identify which controller and which specific action method should handle that request to return the correct response (0:56).
* **Implementation:** The application maintains a table that acts as a map between URLs and their corresponding resources. When a request arrives, the application checks this table for an exact match to determine where to forward the request (1:17 - 1:36).

### Controller and Action Structure
* An *ASP.NET Core* application typically consists of multiple controllers, each containing various **action methods** designed to handle specific tasks (0:17 - 0:25).
* Routing ensures that the correct action method is triggered based on the incoming URL (0:44 - 0:53).

### Key Observations and Constraints
* **Unique URLs:** You can assign multiple unique URLs to a single action method or resource, which is fully supported in *ASP.NET Core* (1:49 - 2:00).
* **Ambiguity Warning:** The instructor notes that you **cannot** map multiple different resources or action methods to the *same* URL. Doing so will cause the application to become confused, leading to an **ambiguous match error** (2:04 - 2:14).
