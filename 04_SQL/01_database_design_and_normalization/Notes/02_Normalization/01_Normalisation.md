# Database Normalization

<br>

## Index

1. [What is Database Normalization?](#what-is-database-normalization)
2. [Why Do We Need Normalization?](#why-do-we-need-normalization)
3. [First Normal Form (1NF)](#first-normal-form-1nf)
4. [Second Normal Form (2NF)](#second-normal-form-2nf)
5. [Third Normal Form (3NF)](#third-normal-form-3nf)
6. [Boyce-Codd Normal Form (BCNF)](#boyce-codd-normal-form-bcnf)
7. [Overview](#summary)


<br>

## What is Database Normalization?
Database Normalization is the process of organizing data in a database to:

### &nbsp; &nbsp; C &uarr; &nbsp; &nbsp; R &darr; &nbsp; &nbsp; A &darr; &nbsp; &nbsp; M &uarr;
- Improve **data consistency**
- Minimize **data redundancy** (duplicate data)
- Reduce **data anomalies**
- Improve **database maintainability**

There are **six normal forms (1NF–6NF)**, but in real-world applications, most databases are normalized up to **Third Normal Form (3NF)**.


<br>


> [!Tip]
> ### `2NF` vs `3NF` major difference
>  - In 2NF, the Primary key is a **Composite Primary Key**
>  - In 3NF, the Primary key is perfectly fine, **Normal primary key**
>
> **Example:** (Cols may be same, just different constraints)\
> In `2Nf` -> Conposite primary key (MovieId, ProducerId) -
>
> Note we have only 1 pkey in table,\
> so we cant consider only 1 of both is pkey,\
> or both are pkeys, both are together pkeys
> 
> ```
> MovieId | ProducerId | MovieName | ProducerName
> ```
>
> <br>
> 
> In `3Nf`, MovieId is only single Primary key,\
> ProducerId is a FK only if it references another table.\
>  (not in unnormalized bad table, there its just a nonkey attribute
>
> ```
> MovieId | ProducerId | MovieName | ProducerName
> ```
> #### This is not something random! 🏷️
> 

<br>

# Why Do We Need Normalization?

Without normalization, the same information is stored repeatedly, causing several problems.

## Problems Caused by Data Redundancy

### 1. Data Redundancy
- Same data is stored multiple times.
- Increases storage requirements.

**Example**

| EmployeeID | EmployeeName | Department | Location |
|------------|--------------|------------|----------|
| 1 | Alice | IT | London |
| 2 | Bob | IT | London |
| 3 | Charlie | IT | London |

Here, **IT** and **London** are repeated for every employee.

using `3NF`

**Departments**
| DepartmentID | Department | Location |
| ------------ | ---------- | -------- |
| 1            | IT         | London   |

**Employees**
| EmployeeID | EmployeeName | DepartmentID |
| ---------- | ------------ | ------------ |
| 1          | Alice        | 1            |
| 2          | Bob          | 1            |
| 3          | Charlie      | 1            |


<br>

### 2. Data Inconsistency

If department information changes, every row must be updated.

Example:

Old:

```
Department = IT
Location = London
```

New:

```
Department = IT
Location = Manchester
```

If one row is missed during update:

| Employee | Department | Location |
|----------|------------|----------|
| Alice | IT | Manchester |
| Bob | IT | London |

Now the database contains conflicting information.

<br>

### 3. Disk Space Wastage

Repeated values consume unnecessary storage.

Instead of storing:

```
IT
IT
IT
IT
IT
```

Store it once in a separate table and reference it using an ID.

<br>

### 4. Poor Performance

Operations like:

- INSERT
- UPDATE
- DELETE

become slower because duplicate data exists in many rows.


<br>

---

<br>



# First Normal Form (1NF)

## Definition

A table is in **First Normal Form (1NF)** if:

- Every column contains **atomic (single) values**
- No repeating groups
- Every row is uniquely identified by a Primary Key

<br>
<div align = "center">
      <img width="650" alt="image" src="https://github.com/user-attachments/assets/6509c45b-f416-4c70-b89c-2e8350622c6b" />
</div>
<br>

## Rules of 1NF

### Rule 1: Atomic Values

Each cell should contain only one value.

### Wrong

| Student | Subjects |
|----------|----------|
| John | Math, Science |

The Subjects column stores multiple values.

> [!caution]
> ### Why we need to ensure 1NF? Why Multiple subjects in 1 cell a problem?
> Now, if non-atomic, its not possible to apply **SELECT**, **DELETE**, **INSERT** on just one subject

<br>

### Correct

| Student | Subject |
|----------|----------|
| John | Math |
| John | Science |

Each cell now contains only one value.

<br>

### Rule 2: No Repeating Columns

Avoid columns like:

| Student | Subject1 | Subject2 | Subject3 |
|----------|-----------|-----------|-----------|

Problems:

- Many NULL values
- Difficult to add new subjects
- Requires ALTER TABLE frequently

Instead create:

| Student | Subject |
|----------|----------|
| John | Math |
| John | Science |

<br>

### Rule 3: Primary Key

Each row should be uniquely identifiable.

Example:

```
StudentID
```

or

```
EmployeeID
```

<br>

## 1NF Solution

Split data into multiple tables and connect them using a **Foreign Key**.

<br>

---

<br>



# Second Normal Form (2NF)


## Definition

A table is in **Second Normal Form (2NF)** if:

- It satisfies **1NF**
- Redundant data is moved into separate tables
- Relationships are maintained using Foreign Keys
- Ensure no `Partial Dependency`

> [!Note]
>### Partial Dependency
> **Definition**:
> A non-key attribute depends on only a part of a composite primary key instead of the whole composite key.
>```
> Primary Key = (StudentID, CourseID)
>```
> This is called a Composite Key.
>
>  The primary key has 2 columns
>
> `(StudentID, CourseID)`
>
> But StudentName depends only on
>
> `StudentID`
>
> not on the complete key.
>
> Likewise,
>
> CourseName depends only on
>
> `CourseID`
>
> not the whole key.
> Only part of the composite key determines the value.
>
> That is why it is called **`Partial Dependency`**
>
> Thus we use **Listing / Junction Tables**

<br>
<div align = "center">
      <img width="650" alt="image" src="https://github.com/user-attachments/assets/3dde22ae-36ed-4743-b6d5-6c62978a62f0" />
</div>
<br>

## Why 2NF?

Instead of storing department information repeatedly:

### Employees

| EmployeeID | Name | DepartmentID |
|------------|------|--------------|
| 1 | Alice | 1 |
| 2 | Bob | 1 |

<br>

### Departments

| DepartmentID | Department | Location |
|--------------|------------|----------|
| 1 | IT | London |

Department information is stored only once.

<br>

## Benefits of 2NF

- Less duplication
- Easier updates
- Smaller tables
- Better consistency

<br>


> [!Tip]
> ### Why Need to  remove Partial Dependency to establish 2NF?, Why 2NF?
>
> Suppose:
>
> ```text
> MovieId | ProducerId | MovieName | ProducerName
> ```
>
> Composite key: `(MovieId, ProducerId)`
>
> If one producer has multiple movies:
>
> ```text
> 1 | P1 | Movie A | Producer X
> 2 | P1 | Movie B | Producer X
> 3 | P1 | Movie C | Producer X
> ```
>
> `ProducerName` depends only on `ProducerId`, not on the **whole composite key** → **partial dependency**.
>
> This causes anomalies:
> - **Update:** Change Producer X's name → update multiple rows.
> - **Insert:** Can't store a producer easily without a movie.
> - **Delete:** Deleting the producer's last movie may also delete producer information.
>
> ### Better design
>
> ```text
> Movie
> -------------------------
> MovieId | MovieName | ProducerId
>
> Producer
> -----------------
> ProducerId | ProducerName
> ```
>
> Now `ProducerName` depends only on `ProducerId`, where it belongs.
> `Movie` stores only the `ProducerId` as the foreign key.
>
> **2NF = Remove partial dependency → every non-key attribute must depend on the whole key.**

<br>

---

<br>

# Third Normal Form (3NF)

## Definition

A table is in **Third Normal Form (3NF)** if:

- It satisfies **1NF**
- It satisfies **2NF**
- Every non-key attribute depends **only on the Primary Key**
- No transitive dependency

<br>
<div align = "center">
      <img width="650" alt="image" src="https://github.com/user-attachments/assets/4133b799-7c17-4d72-a1ab-6068dcd09831" />
</div>
<br>



## Functional Dependency

Every column should depend only on the Primary Key.

Example:

```
EmployeeID
      ↓
EmployeeName
DepartmentID
Salary
```

Correct because all attributes depend directly on EmployeeID.

<br>

## Transitive Dependency

A non-key column depends on another non-key column.

### Wrong

| EmployeeID | DepartmentID | DepartmentName | DepartmentHead |
|------------|--------------|----------------|----------------|

Here,

```
EmployeeID
      ↓
DepartmentID
      ↓
DepartmentName
      ↓
DepartmentHead
```

DepartmentHead depends on DepartmentName instead of EmployeeID.

This is called **Transitive Dependency**.

<br>

## Solution

Move department information into another table.

### Employees

| EmployeeID | Name | DepartmentID |
|------------|------|--------------|

<br>

### Departments

| DepartmentID | DepartmentName | DepartmentHead |
|--------------|----------------|----------------|

<br>

## Avoid Computed Columns

Do not store values that can be calculated.

### Wrong

| MonthlySalary | AnnualSalary |
|---------------|--------------|

Since

```
AnnualSalary = MonthlySalary × 12
```

Store only:

```
MonthlySalary
```

Calculate AnnualSalary when needed.

**Before 3NF**
```
Employees

+-----------+-------------+--------------+----------------+
|EmployeeID |EmployeeName |DepartmentID  |DepartmentName  |
+-----------+-------------+--------------+----------------+
|101        |Yash         |D1            |IT              |
|102        |Rahul        |D2            |HR              |
|103        |Aman         |D1            |IT              |
+-----------+-------------+--------------+----------------+

EmployeeID → DepartmentID

DepartmentID → DepartmentName

Transitive Dependency exists
```

**After 3NF**
```
Employees

+-----------+-------------+--------------+
|EmployeeID |EmployeeName |DepartmentID  |
+-----------+-------------+--------------+
|101        |Yash         |D1            |
|102        |Rahul        |D2            |
|103        |Aman         |D1            |
+-----------+-------------+--------------+


Departments

+--------------+----------------+
|DepartmentID  |DepartmentName  |
+--------------+----------------+
|D1            |IT              |
|D2            |HR              |
+--------------+----------------+
```

<br>

> [!tip]
> ### Why we need 3NF? Why Transitive Dependency can be a problem?
>
> Suppose we have:
>
> ```text
> MovieId | MovieName | GenreId | GenreName | GenreType
> ```
>
> **PK:** `MovieId`  
> `GenreId` is **not a PK** here; it's a **FK**.
>
> Example:
>
> ```text
> 1 | Inception | 10 | Sci-Fi | Fictional
> 2 | Interstellar | 10 | Sci-Fi | Fictional
> 3 | Titanic | 20 | Romance | Love Story
> ```
>
> Dependency:
>
> ```text
> MovieId → GenreId → GenreName, GenreType
> ```
>
> `GenreName` and `GenreType` depend on `GenreId`, which is a **non-key attribute** → **transitive dependency**.
>
> This causes anomalies:
> - **Update:** Change `Sci-Fi` → update multiple movie rows.
> - **Insert:** Can't properly store a new genre without a movie.
> - **Delete:** Deleting the last movie of a genre can delete the genre information.
>
> ### Better design
>
> ```text
> Movie
> --------------------------------
> MovieId (PK) | MovieName | GenreId (FK)
>
> Genre
> -------------------------------
> GenreId (PK) | GenreName | GenreType
> ```
>
> Now genre information belongs to `Genre`, not `Movie`.
>
> **3NF = Remove transitive dependency → non-key attributes should not depend on other non-key attributes.**
>

<br>

---

<br>



# Boyce-Codd Normal Form (BCNF)

## What is BCNF?

**Boyce-Codd Normal Form (BCNF)** is a stricter version of **Third Normal Form (3NF)**.

A table is in **BCNF** if:

- It is already in **3NF**
- **Every determinant must be a [Candidate Key](https://github.com/dev-yash-25/DeltaX-Bootcamp/blob/main/04_SQL/01_database_design_and_normalization/Notes/01_Designing/KEYS_&_Fundamentals_Of_Design.md#candidate-and-super-key)** (More precisely a Super key)

> **Determinant:** An attribute (or set of attributes) that determines another attribute.

If:

```
X → Y
```

Then **X** must be a **Super Key**.

<br>
<div align = "center">
      <img width="550" alt="image" src="https://github.com/user-attachments/assets/fd2a367c-0e1c-46e5-b0f5-1b33bf71b5f6" />
      <br>
      <img width="550" alt="image" src="https://github.com/user-attachments/assets/d6494102-2748-4c62-882f-ce17a53d12fb" />
</div>
<br>



# Why Do We Need BCNF?

Sometimes a table satisfies **3NF** but still contains redundancy because a **non-candidate key** determines another attribute.

BCNF removes these remaining anomalies.


<br>



# Example

### Student_Course

| Student | Course | Instructor |
|---------|--------|------------|
| Alice | DBMS | John |
| Bob | DBMS | John |
| Charlie | OS | David |

Functional Dependencies:

```
(Student, Course) → Instructor
Instructor → Course
```

Candidate Key:

```
(Student, Course)
```

Here,

```
Instructor → Course
```

is a problem because **Instructor** is **not** a Candidate Key.

Therefore, the table is in **3NF** but **not in BCNF**.


<br>



# BCNF Solution

Split the table into two tables.

### Student_Instructor

| Student | Instructor |
|---------|------------|
| Alice | John |
| Bob | John |
| Charlie | David |

### Instructor_Course

| Instructor | Course |
|------------|--------|
| John | DBMS |
| David | OS |

Now every determinant is a Candidate Key.


<br>


> [!TIP]
> ### Why we need BCNF? Why 3NF may not be Complete fully?
>
> Suppose:
>
> ```text
> MovieId | MovieName | GenreId | GenreName | GenreType | Language
> ```
>
> **PK:** `MovieId`
>
> Suppose:
>
> ```text
> GenreId → GenreName, GenreType
> ```
>
> `GenreId` is **not a Superkey** because multiple movies can have the same `GenreId`.
>
> ```text
> GenreId → GenreName
>     ↓
> GenreId is NOT a Superkey
>     ↓
> ❌ BCNF violation
> ```
>
> ### Solution
>
> Separate the dependency:
>
> ```text
> Movie
> --------------------------------
> MovieId (PK) | MovieName | GenreId (FK) | Language
> ```
>
> ```text
> Genre
> -------------------------------
> GenreId (PK) | GenreName | GenreType
> ```
>
> Now:
>
> ```text
> GenreId → GenreName, GenreType
> ```
>
> `GenreId` **is the PK** of `Genre`, therefore it is a **Superkey**.
>
> ```text
> GenreId → GenreName
>     ↓
> GenreId is Superkey
>     ↓
> ✅ BCNF
> ```
>
>  >  Every determinant must be a `candidate key` (more precisely, a `superkey`).
>

<br>




# Difference Between 3NF and BCNF

| 3NF | BCNF |
|------|------|
| Every non-key attribute depends only on the Primary Key. | Every determinant must be a Candidate Key, Non-key attribute should not determine another attibute |
| May still allow some redundancy. | Removes more redundancy than 3NF. |
| Less strict. | More strict than 3NF. |



<br>


---

<br>

# Summary

| Normal Form | Rule |
|-------------|------|
| **1NF** | Atomic values, no repeating groups, Primary Key |
| **2NF** | Remove redundant data into separate tables and use Foreign Keys |
| **3NF** | Remove transitive dependencies and ensure every non-key attribute depends only on the Primary Key |

<br>

**[YB Studios](https://github.com/Yash-Bandal)**
