
# SQL Server – Advanced (Intelligent) Joins

<br>

## Basic of Joins - [Link](https://github.com/Yash-Bandal/DeltaX-Bootcamp/blob/b663d4cef57eba0eb69b6b84ec05c2b3b5eb73d6/04_SQL/01_database_design_and_normalization/Notes/01_Designing/03_Joins.md)

<br>


<div  align= "center">
<img width="650" alt="image" src="https://github.com/user-attachments/assets/d5855eac-ef81-489f-a878-9dd5623c2fb8" />
</div>

These joins are mainly used to find **unmatched records** between two tables.

For our database, we'll use:

* `Foundation.Movies (M)`
* `Foundation.Producers (P)`

Join Condition:

```sql
M.ProducerId = P.Id
```

<br>

# 1. LEFT JOIN + WHERE Right IS NULL

Returns records from the **left table** that have **no matching record** in the right table.

```sql
SELECT M.Name
FROM Foundation.Movies M
LEFT JOIN Foundation.Producers P
ON M.ProducerId = P.Id
WHERE P.Id IS NULL;
```

### Use

Find movies **without a producer**.

> With current data → **No rows** (every movie has a producer).

<br>

# 2. RIGHT JOIN + WHERE Left IS NULL

Returns records from the **right table** that have **no matching record** in the left table.

```sql
SELECT P.Name
FROM Foundation.Movies M
RIGHT JOIN Foundation.Producers P
ON M.ProducerId = P.Id
WHERE M.Id IS NULL;
```

### Use

Find producers who **haven't produced any movie**.

> With current data → **No rows**.

Example (if a new producer is added without movies):

```text
Steven Spielberg
```

<br>

# 3. FULL JOIN + WHERE NULL

Returns **all unmatched records** from both tables.

```sql
SELECT
    M.Name AS Movie,
    P.Name AS Producer
FROM Foundation.Movies M
FULL JOIN Foundation.Producers P
ON M.ProducerId = P.Id
WHERE M.Id IS NULL
   OR P.Id IS NULL;
```

### Use

Find

* Movies without producers
* Producers without movies

in a **single query**.

> With current data → **No rows**.

<br>

# Actor Example

Find actors who are **not assigned to any movie**.

```sql
SELECT A.Name
FROM Foundation.Actors A
LEFT JOIN Foundation.Actor_Movies AM
ON A.Id = AM.ActorId
WHERE AM.ActorId IS NULL;
```

### Current Data

Every actor is assigned to at least one movie.

Result:

```text
No rows
```

If you add

```text
Shah Rukh Khan
```

without inserting into `Actor_Movies`, the query returns

```text
Shah Rukh Khan
```

<br>

# Movie Example

Find movies that have **no actors**.

```sql
SELECT M.Name
FROM Foundation.Movies M
LEFT JOIN Foundation.Actor_Movies AM
ON M.Id = AM.MovieId
WHERE AM.MovieId IS NULL;
```

Current data:

```text
No rows
```

If you insert a new movie without actor mappings, it will be returned.

<br>

# Quick Revision

| Join                                                      | Finds                              |
| --------------------------------------------------------- | ---------------------------------- |
| `LEFT JOIN ... WHERE Right.Id IS NULL`                    | Left table records with no match   |
| `RIGHT JOIN ... WHERE Left.Id IS NULL`                    | Right table records with no match  |
| `FULL JOIN ... WHERE Left.Id IS NULL OR Right.Id IS NULL` | Unmatched records from both tables |

### Interview Tip

These are called **anti-joins** (LEFT/RIGHT with `IS NULL`) and are commonly used to find **missing**, **orphan**, or **unassigned** records, such as:

* Customers with no orders
* Employees without departments
* Movies without producers
* Actors not assigned to any movie
