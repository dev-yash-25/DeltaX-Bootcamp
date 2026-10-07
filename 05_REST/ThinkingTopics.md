| Concept                             | Basic theory                      | Hidden/actually confusing part                                                            |
| ----------------------------------- | --------------------------------- | ----------------------------------------------------------------------------------------- |
| **DI lifetimes**                    | Singleton/Scoped/Transient        | Lifetime capture, resolution vs creation, nested dependencies                             |
| **Middleware**                      | Request passes through middleware | Why exceptions travel *back upward*, `_next`, ordering                                    |
| **Authentication vs Authorization** | Both deal with security           | Authentication establishes identity; authorization decides access; middleware order       |
| **JWT**                             | Token contains claims             | Signing ≠ encryption, issuer/audience, who validates it, when validation happens          |
| **`async` / `await` / `Task`**      | Used for asynchronous code        | What actually happens to the thread, what `await` returns, why `Task<T>` isn't `T`        |
| **Dapper**                          | Micro-ORM                         | `Query` vs `Execute` vs `ExecuteScalar`, mapping, connection lifetime, deferred execution |
| **`IEnumerable` vs `List`**         | Both collections                  | Interface vs implementation, materialization, `.ToList()`, deferred execution             |
| **Repository/Service layers**       | Separate responsibilities         | Which layer should know what, and why cross-repository calls become problematic           |
| **REST routes**                     | Use URLs for resources            | Why `/movies/{id}` vs `?year=2020`, nested resources, controller selection                |
| **SQL joins / junction tables**     | Many-to-many needs junction table | Why bad designs allow duplicates, where constraints actually enforce rules                |
| **Exception handling**              | `try/catch` handles errors        | Exception propagation, catch ordering, middleware as global boundary                      |
| **Configuration / Options**         | Read settings from appsettings    | Binding vs registration vs resolution, `IOptions<T>`, configuration source precedence     |
| **AutoMapper**                      | Maps objects                      | Mapping is not validation/business logic; nested mappings and DTO boundaries              |
