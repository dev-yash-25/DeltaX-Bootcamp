# SQL Server – Stored Procedures 

# 1. What is a Stored Procedure?

A **Stored Procedure (SP)** is a precompiled collection of one or more SQL statements stored in the database and executed as a single unit.

### Advantages

* Reusable code
* Better performance (execution plan is cached)
* Reduces network traffic
* Improves security
* Easier maintenance

<br>

## Creating Stored Procedures

## Naming Convention

Avoid `sp_` (reserved for system procedures).

Use:

```text
usp_GetActors
usp_AddMovie
usp_UpdateProducer
usp_DeleteActor
```

<br>

## Create a Procedure

```sql
CREATE PROCEDURE usp_GetActors
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Foundation.Actors;
END;
```

Execute

```sql
EXEC usp_GetActors;
```


<br>

---

<br>

## 2. Procedure with Parameters

```sql
CREATE PROCEDURE usp_GetMovieById
    @MovieId INT
AS
BEGIN
    SELECT *
    FROM Foundation.Movies
    WHERE Id = @MovieId;
END;
```

Execute

```sql
EXEC usp_GetMovieById 1;
```

<br>

### 2.1 Multiple Parameters

```sql
CREATE PROCEDURE usp_GetMovies
    @ProducerId INT,
    @Year INT
AS
BEGIN
    SELECT *
    FROM Foundation.Movies
    WHERE ProducerId = @ProducerId
      AND YearOfRelease = @Year;
END;
```

<br>

### 2.2 Default Parameter

```sql
CREATE PROCEDURE usp_GetMoviesByYear
    @Year INT = 2023
AS
BEGIN
    SELECT *
    FROM Foundation.Movies
    WHERE YearOfRelease = @Year;
END;
```

Execute

```sql
EXEC usp_GetMoviesByYear;
EXEC usp_GetMoviesByYear 2024;
```


<br>

---

<br>



###  `XACT_ABORT` , `XACT_STATE`

* **`XACT_ABORT`** → controls what happens to a transaction when a runtime error occurs.
* `SET XACT_ABORT ON;` → automatically rolls back the transaction when a qualifying runtime error occurs.
   - If a qualifying runtime error occurs while a transaction is running, automatically abort (roll back) the entire transaction.
* **`TRY...CATCH`** → catches the error so you can handle it.
* **`XACT_STATE()`** → tells the transaction's current state:
  * `1` → active & committable
  * `-1` → active but uncommittable
  * `0` → no transaction

### Typical stored procedure pattern

```sql
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- SQL operations

    COMMIT;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK;

    THROW;
END CATCH;
```

**Remember:**

> `TRY/CATCH` = handle error
> `XACT_ABORT` = rollback behavior
> `XACT_STATE()` = transaction status
> `THROW` = re-raise error


<br>

---

<br>




# 3. Otput Paramter and Return Value
## 3.1 OUTPUT Parameter

<div align = "center">
<img width="490" height="365" alt="image" src="https://github.com/user-attachments/assets/656f0dbd-60d1-4f25-9952-9c5a45ee39a5" />
</div>


### Step 1 : Define `Returning paramteer` procedure
```sql
CREATE PROCEDURE usp_GetMovieCount
    @Count INT OUTPUT
AS
BEGIN
    SELECT @Count = COUNT(*)
    FROM Foundation.Movies;
END;
```

### Step 2 : Declare output parameter variable

Execute

```sql
DECLARE @MovieCount INT;
```

### Step 3 : Pass variable & Call procedure
```sql
EXEC usp_GetMovieCount @MovieCount OUTPUT;

SELECT @MovieCount;
```

### Step 4 :  Print or select
```sql
PRINT @MovieCount
```
you can use `if..else` to verify if null f=or not
```sql
IF (@MovieCount is null)
    Print '@MovieCount is Null'
ELSE
    Print '@MovieCount is not null'
```


<br>

---

<br>

    
## 3.2  OUTPUT Parameter vs RETURN Value


<div align = "center">
    <img width="491" height="329" alt="image" src="https://github.com/user-attachments/assets/7f9e0c45-8f4d-4c05-9f65-8d7f431289e1" />
</div>

| OUTPUT Parameter                                              | RETURN Value                                     |
| ------------------------------------------------------------- | ------------------------------------------------ |
| Returns **data** from a stored procedure                      | Returns an **integer status/value**              |
| Can return multiple values (using multiple OUTPUT parameters) | Returns only one integer                         |
| Commonly used for counts, IDs, names, etc.                    | Commonly used for Success (0) / Error (non-zero) |
| Declared with `OUTPUT` keyword                                | Uses `RETURN` statement                          |

<br>

### OUTPUT Parameter

Used to return data through a parameter.

#### Example

```sql
CREATE PROCEDURE usp_GetMovieCount
    @Count INT OUTPUT
AS
BEGIN
    SELECT @Count = COUNT(*)
    FROM Foundation.Movies;
END;
```

Execute

```sql
DECLARE @MovieCount INT;

EXEC usp_GetMovieCount @MovieCount OUTPUT;

SELECT @MovieCount;
```

Result

```text
6
```

<br>

### RETURN Value

Used to return an integer status or code.

#### Example

```sql
CREATE PROCEDURE usp_CheckMovie
AS
BEGIN
    RETURN 0;      -- Success
END;
```

Execute

```sql
DECLARE @Status INT;

EXEC @Status = usp_CheckMovie;

SELECT @Status;
```

Result

```text
0
```

<br>

### When to Use

* **OUTPUT Parameter** → Return actual data (Count, Id, Name, Total, etc.).
* **RETURN Value** → Return status or error codes (`0 = Success`, non-zero = Failure).

> **Best Practice:** Use **OUTPUT parameters** for returning data and **RETURN** only for status/error codes.


<br>

---

<br>

# 4.  Managing Stored Procedures
## Steps in easy
### 1. Step 1 : use system stored procedure `sp_helptext` to get code on window
   ```sql
   EXEC sp_helptext usp_GetActors
   ```
### 2.Step 2 : Change `CREATE` to `ALTER` and make modifications

**ALTER Procedure**

```sql
ALTER PROCEDURE usp_GetActors
AS
BEGIN
    SELECT Name, DateOfBirth
    FROM Foundation.Actors;
END;
```

<br>

## View Procedure Definition

```sql
EXEC sp_helptext 'usp_GetActors';
```
Gives the code of the procedure


> Doesn't work if created with `WITH ENCRYPTION`.

> [!Note]
> ### Encryption
> Add `WITH ENCRYTION` constraint to not allow ALTER , or modification of procedure (sealing)
> ```sql
> CREATE/ALTER PROCEDURE {usp_proc__name}
> @Parameter
> WITH ENCRYPTION
> AS
>   BEGIN
>    query
> END
> ```


<br>

---

<br>


## 5. DROP Procedure

```sql
DROP PROCEDURE usp_GetActors;
```

<br>

---

<br>

> [!Tip]
> An **Execution Plan** is a roadmap created by SQL Server that shows **how a query will be executed**.
> 
> It tells SQL Server:
> - Which indexes to use
> - Whether to perform a Table Scan or Index Seek
> - Join order
> - Estimated cost of each operation

<br>

## Difference between `AD-Hoc` queries (Direct) & `SPs`
Ad Hoc queries means, directly executing the query full code, without stored proc

The Difference is that, stored procedures always use the same execution plan, even if input paramter changes

Adhoc queries also use the same execution plan, just that with a single change in paramter/ space in query, a new execution plan is created

eg
```sql
SELECT * FROM TABLETest WHERE id = 1
```
re run, will use same exec plan

but, just by making some structure changes (extra spaces here), new plan created
```sql
SELECT * FROM TABLETest   WHERE  id = 1
```


<br>

# Useful Options

## WITH ENCRYPTION

Hides the procedure definition.

```sql
CREATE PROCEDURE usp_GetActors
WITH ENCRYPTION
AS
BEGIN
    SELECT *
    FROM Foundation.Actors;
END;
```

<br>

## SET NOCOUNT ON

Suppresses the "(n rows affected)" message.

```sql
SET NOCOUNT ON;
```


<br>

---

<br>

<div align = "center">
<img width="485" height="287" alt="image" src="https://github.com/user-attachments/assets/9bd38abc-df25-48dd-9e50-6960f3ea42a2" />
</div>


```sql
sp_help usp_procedure_name

sp_helptext usp_procedure_name

sp_depends usp_procedure_name
```


<br>

---

<br>

# Quick Revision

| Statement          | Purpose                          |
| ------------------ | -------------------------------- |
| `CREATE PROCEDURE` | Create a procedure               |
| `EXEC` / `EXECUTE` | Execute a procedure              |
| `ALTER PROCEDURE`  | Modify a procedure               |
| `sp_helptext`      | View procedure definition        |
| `DROP PROCEDURE`   | Delete a procedure               |
| `@Parameter`       | Input parameter                  |
| `OUTPUT`           | Return values through parameters |
| `RETURN`           | Return integer status/value      |
| `WITH ENCRYPTION`  | Hide procedure definition        |
| `SET NOCOUNT ON`   | Suppress row count messages      |

### Interview Tip

* **Stored Procedure** → Executes SQL statements and can return result sets, output parameters, or status codes.
* Prefer **parameterized procedures** over hardcoded queries for better reusability and security.
