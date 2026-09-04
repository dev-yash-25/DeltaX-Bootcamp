# Points to Remember

1. How to Choose which controller >
  Don't ask:
  > "What is the first resource in the URL?"
  
  Ask:
  > "What resource is the endpoint actually operating on?"

 Example 1
  ```
  GET /users/10/orders
  What are we getting? ->  Orders.
  → OrdersController
  ```
