# Points to Rememmber

> “What mistakes do developers commonly make when they move from training into actual development? Things that I should start observing and avoiding from now itself

> [!note]
> There is a Design Tradeoff in Database Design
>
> You Can either have 'Good Speed', or 'Less Space`

<br>

## Index
1. [Primary Key vs Unique](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#1-primary-key-vs-unique)
2. [Where vs Having - No Aggregates with WHERE](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#2-where-vs-having)
3. [UNION vs UNION ALL](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#3-union-vs-union-all)
4. [UNION vs Join](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#4-union-vs-join)
5. [With and Without Group BY](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#5-with-and-without-group-by)
6. [Group by Primary Key + Display Columns](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#6-group-by-primary-key--display-columns)
7. [Group By and Aggregate](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#7-dont-group-by-aggregating-column)
8. [Functions vs Stored Procedures](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#8-stored-procedure-vs-function)
9. [Anomalies](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#9-database-anomalies)
10. [SQL Injection](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#10-sql-injection)
11. [Transactions](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#11-transactions)
12. [3NF and No junction table](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#12-3nf)
13. [Candidate Vs Composite key](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#13-candidate-key-vs-composite-key)
14. [Delete vs Truncate vs Drop](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#14-delete-vs-truncate-vs-drop)
15. [`COUNT(column)` and `NULL`](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#15-countcolumn-and-null)
16. [GROUP BY and SELECT](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#16-group-by-and-select)
17. [DISTINCT vs GROUP BY - Think of 2 methods of Finding Unique](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/Points_To_Remember.md#17-distinct-vs-group-by)

<br>


## Terms
**1. Referential Integrity** - Referential integrity is a database rule that keeps data correct and linked

**2. Cascading referential integrity constraint** - A cascading referential integrity constraint is a database rule 
that automatically updates or deletes dependent child table rows when a parent table row is updated or deleted.

<br>

## Tips
1. You forget sometimes to use `Group By` before `Having`
2. Also many times, you forget to use `DISTINCT` inside COUNT/Aggregate
   ```sql
   SELECT
     P.Name as ProducerName
   FROM Foundation.Producers P 
   INNER JOIN Foundation.Movies M
     ON P.Id = M.Producerid
   Group by P.Id, P.Name   -- you forgot here
   HAVING COUNT(Distinct M.Languages) = 
   (
    SELECT Count(Distinct Languages ) From Movies;
   )
   ```
3. You sometimes use Aggregate along with `WHERE`,
    - Remember that Aggregate works on Group by, so you can use it with having after groups are formed
      
   ❌ Wrong Syntax
   ```sql
   SELECT M.Name as MovieName
   FROM Foundation.Movies M
   INNER JOIN Foundation.ActorMovies AM
   ON M.Id = AM.ActorId
   WHERE COUNT(AM.ActorId) > 2
   GROUP BY M.Id, M.Name
   HAVING M.Profit >
   (
   SELECT AVG(Profit) From Foundation.Movies
   GROUP BY id,name  -- Also wrong
   )
   ```
4. Like what you need in `SELECT` must be in `GROUP BY`, similarly, dont forget,\
   **What you need in `HAVING` (Non aggregate - Like here M.Profit) should also be in `GROUP BY`**
   ```sql
      SELECT
       M.Name AS MovieName,
       M.Profit,
       COUNT(AM.ActorId) AS ActorCount
   FROM Foundation.Movies M
   INNER JOIN Foundation.Actor_Movies AM
       ON M.Id = AM.MovieId
   GROUP BY
       M.Id,
       M.Name,
               -- Need here
   HAVING COUNT(AM.ActorId) > 2
      AND M.Profit >  --Need 
          (
              SELECT AVG(Profit)
              FROM Foundation.Movies
          );
       ```
5. WHen I need to compare a row to another row from the same table → consider a self join
   ```
   Producers who produced movies released in consecutive years 
   ```
   Example Hint Keywords
   ```
   "consecutive", "previous/next", "before/after", or "compare one row with another row"
   ```

6. It is not always necessary to use `GROUP BY` for using **Aggregates** like `COUNT`, `SUM` and all..you shall use group by only when condition on 1+ groups needed
   not for single Group

      You **need `GROUP BY E.name`** if you want to select `E.name` along with `SUM(salary)`.
      
      ### ❌ Wrong
      
      ```sql
      SELECT E.name, SUM(E.salary)
      FROM Employees E;
      ```
      
      SQL Server will give an error because:
      
      * `SUM(E.salary)` → aggregate
      * `E.name` → normal column
      * `E.name` is neither aggregated nor grouped.

      ### ✅ Correct
      
      ```sql
      SELECT
          E.name,
          SUM(E.salary) AS TotalSalary
      FROM Employees E
      GROUP BY E.name;
      ```
      
      
      Here you're saying:
      
      > **"Give me the total salary for each name."**
      
      If you want **one overall salary total**, don't select `name`:
      
      ```sql
      SELECT
          SUM(E.salary) AS TotalSalary
      FROM Employees E;
      ```

7. Avoid Over Normalization, Creating Too many tables, Create tables / Normalize only to reduce redundancy and anomalies 

<br>

---

<br>

## Notes
1. Self Join is used in Hierarchy cases, eg. Employee table, find manager and associated employee
2. Use Self Referencing, in cases like friendships, subscribers, followers..
3. Avoid Duplicates in Self join - Using condition
   ```sql
   ON T1.Id < T2.Id 
   ```
   or
   ```sql
   ON T1.OtherId = T2.OtherId
   AND T1.Id < T2.Id
   ```

<br>

---

<br>

## Things worth Noting

### 1. Primary key vs Unique 
| Feature | Primary Key | Unique Key |
| :--- | :--- | :--- |
| **Quantity** | Only **one** allowed per table. | **Multiple** allowed per table. |
| **NULL Values** | Strictly **no NULL** values allowed. | Allows **one NULL** value (varies by database). |
| **Default Index** | Automatically creates a **Clustered Index**. | Automatically creates a **Non-Clustered Index**. |


<br>



### 2. WHERE vs HAVING

> [!caution]
> You cannot use where with aggregate

<img width="610" height="328" alt="image" src="https://github.com/user-attachments/assets/d4b25306-93c6-48ec-9c0d-d4632334a205" />


❌ Incorrect
```sql
SELECT *
FROM tblEmployee
WHERE SUM(Salary) > 4000;
```
Error:
```
An aggregate may not appear in the WHERE clause...
```

✅ Correct: Use HAVING
```sql
SELECT Department,
       SUM(Salary) AS TotalSalary
FROM tblEmployee
GROUP BY Department
HAVING SUM(Salary) > 4000;
```

<br>

## 3. UNION vs UNION ALL

| Feature | UNION | UNION ALL |
|---------|-------|-----------|
| Removes duplicate rows | ✅ Yes | ❌ No |
| Keeps duplicate rows | ❌ No | ✅ Yes |
| Performance | Slower (checks duplicates) | Faster |
| Use when | Need unique results | Need all results, including duplicates |

### Example Tables

#### Table A

| Name |
|------|
| Alice |
| Bob |
| Charlie |

#### Table B

| Name |
|------|
| Bob |
| David |
| Emma |

### UNION

```sql
SELECT Name FROM TableA
UNION
SELECT Name FROM TableB;
```

#### Result

| Name |
|------|
| Alice |
| Bob |
| Charlie |


> **Note:** Duplicate values (e.g., **Bob**) are removed.

<br>

### UNION ALL

```sql
SELECT Name FROM TableA
UNION ALL
SELECT Name FROM TableB;
```

### Result

| Name |
|------|
| Alice |
| Bob |
| Charlie |
| Bob |


duplicate Bob not removed


<br>

## 4. UNION vs JOIN

| Feature | UNION | JOIN |
|---------|-------|------|
|Direction | Horizontal (makes table wider) | Vertical (makes table taller)|
| Purpose | Combines rows from two or more `SELECT` queries | Combines columns from two or more tables |
| Combines 🏷️ | Rows | Columns |
| Number of Columns | Must be the same in both queries | Can be different |
| Matching Condition | Not required | Required (`ON` clause, except `CROSS JOIN`) |
| Duplicate Handling | `UNION` removes duplicates, `UNION ALL` keeps them | No duplicate removal by default |
| Use Case | Merge similar data from multiple tables | Retrieve related data from multiple tables |

<br>

## 5. With and Without Group BY
Think of it like this:

Without GROUP BY:

```sql
SELECT COUNT(*)
FROM Table;
```

➡ Counts every row in the table.

With GROUP BY:
```sql
SELECT Actor1, Actor2, COUNT(*)
FROM ...
GROUP BY Actor1, Actor2;
```

It's almost as if SQL does this internally:
```
For each unique (Actor1, Actor2):

    Count how many rows belong to THIS pair

    Output one row
```
So COUNT(*) is per group, not for the entire table.

A nice way to think about it is:

GROUP BY splits the table into many mini-tables, and COUNT(*) counts rows inside each mini-table.

This mental model works for SUM(), AVG(), MIN(), MAX(), etc. as well.


<br>

## 6. Group by Primary Key + Display Columns

Whenever you use an aggregate function (`COUNT`, `SUM`, `AVG`, `MIN`, `MAX`):

> **Every non-aggregated column in the `SELECT` must appear in the `GROUP BY` clause.**

### ❌ Wrong 🏷️

```sql
SELECT
    C.Name,
    COUNT(S.subscribed_to)
FROM Channel_Profile C
INNER JOIN Subscription S
    ON C.Id = S.subscribed_to
GROUP BY
    S.subscribed_to;
```

**Reason:** `C.Name` is selected but not included in `GROUP BY`.



### ✅ Right

```sql
SELECT
    C.Name,
    COUNT(S.subscribed_to)
FROM Channel_Profile C
INNER JOIN Subscription S
    ON C.Id = S.subscribed_to
GROUP BY
    C.Id,
    C.Name;
```



### Easy Memory Trick

✅ **Ask yourself:**

> **"Am I printing this column directly?"**

If **Yes**, then:
- Either put it in `GROUP BY`
- Or wrap it inside an aggregate function.

Otherwise, SQL Server throws an error.


<br>




## 7 Don't Group By Aggregating Column
Do not GROUP BY the column that you're aggregating.

WRONG
```sql
SELECT Department, AVG(Salary)
FROM Employees
GROUP BY Department, Salary;
```


<br>

## 8. Stored Procedure vs Function 

> [!Important]
> **Core difference**
>
> Function is mainly used to calculate/return a value.\
> Stored Procedure is used to perform an operation/business process.

|                               | Function                                           | Stored Procedure                               |
| ----------------------------- | -------------------------------------------------- | ---------------------------------------------- |
| Purpose                       | Return a value/table                               | Perform a task                                 |
| Return                        | Must return a value/table                          | Can return nothing, result sets, output params |
| Call from `SELECT`            | Yes                                                | No                                             |
| Can modify data               | Generally no side-effecting `INSERT/UPDATE/DELETE` | Yes                                            |
| Input parameters              | Yes                                                | Yes                                            |
| Output parameters             | No                                                 | Yes                                            |
| Multiple result sets          | No                                                 | Yes                                            |
| Transactions                  | Restricted                                         | Yes                                            |
| Dynamic SQL                   | No                                                 | Yes                                            |
| Called from procedure         | Yes                                                | —                                              |


Function example
```sql
CREATE FUNCTION GetEmployeeSalary
(
    @EmployeeId INT
)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @Salary DECIMAL(10,2);


    SELECT @Salary = Salary
    FROM Employees
    WHERE Id = @EmployeeId;


    RETURN @Salary;
END;
```
Use it:
```sql
SELECT dbo.GetEmployeeSalary(101);
```

Stored Procedure example
```sql
CREATE PROCEDURE GetEmployeeDetails
    @EmployeeId INT
AS
BEGIN
    SELECT *
    FROM Employees
    WHERE Id = @EmployeeId;
END;
```
Execute:
```sql
EXEC GetEmployeeDetails 101;
```

<br>

##  9. Database Anomalies
In database systems, anomalies are unexpected or undesired behaviors that can occur when manipulating data,

They are problems caused by **redundant / poorly designed data**.

**eg: of bad db**

| StudentID | StudentName | CourseID | CourseName | Instructor |
| --------- | ----------- | -------- | ---------- | ---------- |
| 1         | Yash        | C101     | SQL        | Rahul      |
| 2         | Amit        | C101     | SQL        | Rahul      |
| 3         | John        | C102     | C#         | Priya      |


### 1. Update Anomaly

Same data exists in multiple rows → changing it requires multiple updates.

**Example:** Course name `SQL` stored for 10 students. Changing it requires updating all 10 rows.

### 2. Insertion Anomaly

Cannot insert new information without unrelated information.

**Example:** Cannot add a new course unless at least one student is enrolled.

### 3. Deletion Anomaly

Deleting one piece of data accidentally deletes other useful information.

**Example:** Deleting the only student enrolled in `C#` also removes the C# course information.

> **Remember:**
> **Update → Multiple changes**\
> **Insert → Can't add independently**\
> **Delete → Unwanted data loss**


<br>

## 10. SQL Injection
SQL injection (SQLi) is a **Security flaw** / **Vulnerability** / **Attack** where bad actors type malicious database commands into website entry boxes, like a login or search form. 

If the website is not built safely, it runs this hidden code
### How to avoid it?
1. Parameterized queries / Prepared statements — most important.
       Unsafe:
       ```sql
       SELECT * FROM Users
       WHERE Name = ' + userInput + ';
       ```
       Safe:
       ```sql
       SELECT * FROM Users
       WHERE Name = @Name;
       ```
   
2. Validate and constrain input — datatype, length, allowed values, etc.
3. Avoid dynamic SQL where possible; if necessary, parameterize it.
   
   So even if the user enters malicious SQL, it is treated as plain data, preventing SQL Injection.


<br>

## 11. Transactions
A Transaction is a Sequence of operation performed as a single unit of Task/Work.

It works on the Idea, either `Commit all` or `Nothing`.


### 1. What is `@@TRANCOUNT`?

`@@TRANCOUN` is a SQL Server system variable that tells you how many **Active Transactions** are currently open in your session.
```
BEGIN TRANSACTION;
-- @@TRANCOUNT = 1

BEGIN TRANSACTION;
-- @@TRANCOUNT = 2

       .
       .       
       .
       
COMMIT;
-- @@TRANCOUNT = 1

COMMIT;
-- @@TRANCOUNT = 0
```
```sql
IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
```

It means:\
"If a transaction is currently open or Active, roll it back."

### When would @@TRANCOUNT be 0?
Observe the code, we write validation checks `IF..THROW` before we `BEGIN TRANSACTION`

So, if validation fails, and throws something, Begin transaction is never reached, and `@TRANCOUNT` remains 0



### Why not simply write ROLLBACK?

You could write:
```sql
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;

END CATCH
```
But if there is no active transaction, ROLLBACK TRANSACTION itself can produce an error:

> "The ROLLBACK TRANSACTION request has no corresponding BEGIN TRANSACTION."

so its safe to write
```sql
IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
```

<br>

## 12. 3NF 
3NF ensures table is in 2nf, and no transitive dependency exists

```text
Employee
--------------------------------
EmployeeId | EmployeeName | DeptId | DeptName
1          | Yash         | D10    | IT
2          | Rahul        | D10    | IT
3          | Amit         | D20    | HR
```

* **PK:** `EmployeeId`
* `DeptId` is **NOT part of the candidate key**.
* Still violates **3NF** because of a **transitive dependency**:

```text
EmployeeId → DeptId
DeptId     → DeptName

Therefore:
EmployeeId → DeptName   (transitively)
```

### Fix

Split into:

```text
Employee
-------------------------
EmployeeId | EmployeeName | DeptId

Department
----------------
DeptId | DeptName
```

* `DeptId` in `Employee` becomes an **FK**.
* Relationship is **1:N**:
  * **1 Department → Many Employees**
  * **1 Employee → 1 Department** (assuming each employee belongs to one department)
* Therefore, **NO junction table**.

### Remember

> **M:N → Junction table**

> **1:N → FK on the N-side**

Here, Employee is the **N-side**, so `DeptId` goes into `Employee`.


<br>

##  13. Candidate Key vs. Composite Key

| Feature | Candidate Key | Composite Key |
|---|---|---|
| Primary Definition | A minimal super key that can uniquely identify a row. | A database key made up of two or more columns. |
| Column Count | Can be a single column or multiple columns. | Must always have multiple columns. |
| Core Purpose | To act as a "candidate" to become the Primary Key. | To enforce uniqueness when single columns fail to do so. |
| Minimality | Strictly minimal. Removing one column destroys uniqueness. | Not necessarily minimal. It can sometimes include extra columns. |

& Superkey vs Composite key

| Feature | Superkey | Composite Key |
|---|---|---|
| Definition | Any combination of fields that uniquely identifies a row. | A key composed of two or more columns to ensure uniqueness. |
| Minimality | Not required to be minimal; often contains extra columns. | Must be minimal; removing any column breaks uniqueness. |
| Number of Columns | Can be a single column or multiple columns. | Must always consist of 2 or more columns. |
| Redundancy | Contains redundant attributes. | Contains zero redundant attributes. |



<br>

## 14. Delete vs Truncate vs Drop

| Command    | Removes             | `WHERE` allowed? | Table and its Structure remains? | Typical use             |
| ---------- | ------------------- | ---------------: | -------------: | ----------------------- |
| `DELETE`   | Can delete some Selected rows or all rows        |            ✅ Yes |          ✅ Yes | Remove specific recordsm SLow because of where filter functionility |
| `TRUNCATE` | **All rows** , `Resets` the table       |             ❌ No |          ✅ Yes | Empty a table quickly   |
| `DROP`     | Delete he Entire table/object |             ❌ No |           ❌ No | Remove the table itself |


<br>


## 15. `COUNT(column)` and `NULL`

* `COUNT(column)` **does not count `NULL` values**.
* Therefore:

```sql
HAVING COUNT(AM2.MovieId) = 0
```

means:

> This group has **zero non-NULL `AM2.MovieId` values**.

Useful for detecting groups with **no matching rows** after a `LEFT JOIN`.

### Important distinction

```sql
AM2.MovieId IS NULL
```

→ **row-level** check.

```sql
COUNT(AM2.MovieId) = 0
```

→ **group-level** check.

<br>


## 16. `GROUP BY` and `SELECT`

After using `GROUP BY`, every column in `SELECT` must be either:

1. A column present in `GROUP BY`
2. An aggregate expression such as `COUNT()`, `SUM()`, `MAX()`, `MIN()`, `AVG()`

Example:

```sql
SELECT
    Language,
    SUM(Profit)
FROM Foundation.Movies
GROUP BY Language;
```

Here:

```text
Language  → GROUP BY column
SUM(...)  → Aggregate
```

### Mental model

```text
FROM / JOIN
    ↓
GROUP BY
    ↓
Creates groups
    ↓
Aggregate functions operate inside groups
    ↓
SELECT projects grouped/aggregated result
```



###  Can an Aggregated Column Also Be in `GROUP BY`?

**Yes, syntactically.**

```sql
SELECT
    Language,
    Profit,
    SUM(Profit)
FROM Foundation.Movies
GROUP BY
    Language,
    Profit;
```

But this changes the grouping.

Instead of:

```text
Hindi
 ├── 500
 ├── 250
 └── 350
     ↓
SUM = 1100
```

you get:

```text
Hindi + 500 → SUM = 500
Hindi + 250 → SUM = 250
Hindi + 350 → SUM = 350
```

### Rule

> `GROUP BY` determines **what defines a group**. Aggregation operates on the rows **inside that group**.

So grouping by the column you're trying to aggregate is usually inappropriate when you want the aggregate across that entire category.

<br>


## 17. `DISTINCT` vs `GROUP BY`

Both can remove duplicate result rows.

### `DISTINCT`

Use when your intention is simply:

> **Remove duplicate rows.**

```sql
SELECT DISTINCT
    A.Id,
    A.Name
FROM ...
```

### `GROUP BY`

Use when your intention is:

> **Create groups for aggregation or group-level filtering.**

```sql
SELECT
    A.Id,
    A.Name,
    COUNT(*)
FROM ...
GROUP BY
    A.Id,
    A.Name;
```


### Mental shortcut

```text
DISTINCT
→ "Give me unique rows."

GROUP BY
→ "Create groups so I can calculate something for each unique/distinct group."
```

<br>


