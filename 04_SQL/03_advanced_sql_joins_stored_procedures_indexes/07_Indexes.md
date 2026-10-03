# SQL Server – Indexes 

<br>

## Index

1. [Quick Overview](#quick-overview)
2. [What is an Index?](#what-is-an-index)
3. [Why Use Indexes?](#why-use-indexes)
4. [Types of Indexes](#types-of-indexes)
5. [Database Pages](#database-pages)
6. [Heap](#heap)
7. [B-Tree (Balanced Tree)](#b-tree-balanced-tree)
8. [Clustered Index](#1-clustered-index)
   - [How is a B-Tree formed?](#think-how-is-a-b-tree-formed)
   - [Characteristics](#characteristics)
9. [Non-Clustered Index](#2-non-clustered-index)
   - [Comparison](#comparison)
10. [Composite Index](#composite-index)
11. [Leftmost Prefix Rule](#leftmost-prefix-rule)
12. [Create Index](#create-index)
   - [Clustered](#clustered)
   - [Non-Clustered](#non-clustered)
13. [Drop Index](#drop-index)
14. [View Indexes](#view-indexes)
15. [When to Create an Index](#when-to-create-an-index)
16. [Avoid Indexes On](#avoid-indexes-on)
17. [Clustered vs Non-Clustered](#clustered-vs-non-clustered)
18. [Interview Tips](#interview-tips)


<br>

---


## Quick Overview



| Topic                | Key Point                                         |
| ------------------- | ------------------------------- |
| Index                | Speeds up data retrieval                          |
| Heap                 | Table without a clustered index                   |
| Page Size            | 8 KB                                              |
| B-Tree               | Root → Intermediate → Leaf                        |
| Clustered Index      | Physically sorts table data                       |
| Non-Clustered Index  | Separate structure with row pointers              |
| Composite Index      | Index on multiple columns                         |
| Leftmost Prefix Rule | Query must start with the leftmost indexed column |
| `CREATE INDEX`       | Creates an index                                  |
| `DROP INDEX`         | Removes an index                                  |

---

<br>


## What is an Index?

An **Index** is a database object that helps SQL Server locate rows quickly without scanning the entire table.

 **Analogy:** Like an index in a book—you directly jump to the required page instead of reading every page.

Without Index

```text
Search Row
     ↓
Full Table Scan ❌
```

With Index

```text
Search Row
     ↓
Index
     ↓
Required Row ✅
```

<br>

# Why Use Indexes?

* Faster data retrieval (`SELECT`)
* Reduces Full Table Scans
* Improves `WHERE`, `JOIN`, `ORDER BY`, and `GROUP BY` performance

**Trade-off**

* Faster Reads ✅
* Slower INSERT, UPDATE, DELETE ❌ (indexes must also be updated)

# Types of Indexes

<br>
<div align = "center">
     <img width="500" alt="image" src="https://github.com/user-attachments/assets/a567b3dc-85f4-47c0-97e4-8f5eef9ab5c1" />
</div>
<br>


---

<br>

# Database Pages

 Data is stored in pages, user sees a **Table**, that is a list of `rows` and `cols`, but at backend they are `Pages`, like this below

 A page is a smallest unit of data storage in a Database (8kb). It stores anything (Data, Metadata, Indexes).
 

SQL Server stores data in **8 KB Pages**.

A page contains:

* Page Header
* Data Rows
* Offset Array

Multiple pages form a table.

<br>
<div align = "center">
     <p>Data is stored in pages, user sees a list of rows and cols, but at backend it is stored like this</p>
     <img width="500" alt="image" src="https://github.com/user-attachments/assets/4032bd05-4cc7-4fc1-a61f-76b3b8215d37" />
     <br>
     <p>This is how a page of 8kb looks combining multiple bytes</p>
    <img width="500" alt="image" src="https://github.com/user-attachments/assets/2ac17b28-f430-429d-b751-18eb4693c39f" />


</div>
<br>


<br>

# Heap

Think of heap storage as You have Index pages of book, but all are very randomly stored inside a box, such that it is hard to find a single row we want

A table **without a clustered index** is called a **Heap**.

Characteristics

* Data stored randomly
* Faster inserts
* Slower reads (often requires Full Table Scan)
  
<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/bac53c74-d75f-45dd-aec7-781dff48cd48" />
</div>
<br>

<br>

# B-Tree (Balanced Tree)

Indexes are organized as a **B-Tree**.

```text
Root Node
    │
Intermediate Nodes
    │
Leaf Nodes
```

This structure allows SQL Server to locate rows efficiently.

<br>
<div align = "center">
     <img width="500" alt="image" src="https://github.com/user-attachments/assets/ce72601b-5987-4a39-b7c6-e75c3f08922e" />
</div>
<br>




## 1. Clustered Index

Physically sorts the table data based on the indexed column.

<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/241d6b60-e5f6-4800-9dea-c5630ee68d01" />
</div>
<br>




### Think, How is a B-Tree formed?

> [!Note]
> The index `eg:1:100` is just a pointer, nothing much

The Intermediate nodes are Index pages, 

A Index page is a intermediate page, that stores key values (pointers) to another page. It does not store the actual rows, 

Leaf nodes contain the **actual data rows**.

<br>
<div align = "center">
     <img width="500" alt="image" src="https://github.com/user-attachments/assets/9b6e98f8-20a0-4adb-804d-1657e060ad0f" />
     <p>Now another index</p>
    <img width="500" alt="image" src="https://github.com/user-attachments/assets/520faae6-0c55-4af5-b866-09bb2f5ad3e2" />
</div>
<br>

> [!Important]
> #### Observe
> We dont have a pointer for each row above, we have pointer for groups
>  - 1 -> 5
>  - 6 -> 10
>  - 11 -> 15
>  - 16 -> 20
> Thats why we call it clustered index

If index required is between 1 -> 10, eg. 7, go to left of B Tree, eliminating right

<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/251d0e45-50ad-4d6a-b4af-7736c8ea29de" />
     <br>
     <img width="500" alt="image" src="https://github.com/user-attachments/assets/84abc920-b5a3-484b-9c98-233cf37770e1" />
</div>
<br>

```sql
CREATE CLUSTERED INDEX IX_Foundation_Movies_Id
ON Foundation.Movies (Id);
```

### Characteristics

* Only **1** per table
* Usually created on the Primary Key
* Best for range searches and sorting

Example

```sql
SELECT *
FROM Foundation.Movies
WHERE Id BETWEEN 2 AND 5;
```

<br>

## 2. Non-Clustered Index

A separate structure that stores:

* Indexed column(s)
* Pointer to the actual row

> [!Note]
> Using a non clustered index, no index is organized or changed , like we do in Clustered, it remains as it is unorganized
>
> We maintain a `Row Locator page`, that maintans **A pointer for each ID**.


<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/4676e93c-cfa0-4ac9-a006-f855ff3d9831" />
     <p>An offset is also mapped along with the directly generated intermediate index page</p>
     <br>
     <img width="500" alt="image" src="https://github.com/user-attachments/assets/4ea98e29-469d-439c-856e-3650b2b1c3fe" />
</div>
<br>

Intermediate Row locator Page

<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/5a620a58-c9d1-476a-afb6-e048e4f4ff53" />
</div>
<br>

The index pages are sorted, not the data pages , then bottom to top same BTree operation as clustered index

The sorted pointers are mapped to respective indexes, eg, pointer 1-> index 1, so at the end when user request index 13, BTree takes flow to pointer 13-> that is mapped to Index 13

<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/238c757a-5fb4-4d23-a926-413690ef0326" />
</div>
<br>


```sql
CREATE NONCLUSTERED INDEX IX_Foundation_Actors_Name
ON Foundation.Actors (Name);
```



Example

```sql
SELECT *
FROM Foundation.Actors
WHERE Name = 'Yami Gautam';
```

### Comparison

We just have `1` More extra Layer in **non clustered** as compared to **clustered** index BTree
<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/0e4f983f-f8fc-4228-ad3a-29c6196879c1" />
     <p>Think of clustered as First Index page of Book, and non clustered as Last page of Book </p>
<img width="500" alt="image" src="https://github.com/user-attachments/assets/2657bd0e-73ce-4313-b9e2-a0e48278d4d3" />
     <br>
     <img width="500" alt="image" src="https://github.com/user-attachments/assets/a889e2ca-6b7c-428d-ac36-09f07759f0ef" />
</div>
<br>

We can have only `1 Clustered Index` per table, as data can be Physically sorted only once , think Logically (Clustered is the main one)

But we can have `1+ Non Clustered Index` per table, as we can have multiple pointers 

You cannot have both fast reads and fast writes, there is always a `Trade Off`, heao is though the fastest writes, as no indexes there, but slow very slow reads

<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/4ba13b38-06d8-4124-9932-dcbc5ac0a08f" />
     <br>
     <img width="500" alt="image" src="https://github.com/user-attachments/assets/d2ddb04b-b8a6-42ab-9fe7-bcc752d267c0" />
</div>
<br>


<br>

### Syntax


<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/757f10fc-282b-45a9-8a06-100b097a0c53" />
</div>
<br>


<br>


# Composite Index

`

An index on multiple columns.

```sql
CREATE INDEX IX_Foundation_Movies_Producer_Year
ON Foundation.Movies (ProducerId, YearOfRelease);
```



Works best for

```sql
WHERE ProducerId = 1
```

or

```sql
WHERE ProducerId = 1
AND YearOfRelease = 2024
```

> [!Tip]
> While using multiple Columns as Index, ensure you use sequence in Queries
> eg
> 
> ```sql
> FROM Sales.DBCUstomers
> WHERE ProducerId = 1 AND YearOfRelease > 2020;
> ```
>Will use Index
> ```sql
> FROM Sales.DBCUstomers
> WHERE  YearOfRelease > 2020 AND ProducerId = 1
> ```
> Will not

<br>

# Leftmost Prefix Rule

For

```sql
CREATE INDEX IX_Foundation_Movies_Producer_Year
ON Foundation.Movies (ProducerId, YearOfRelease);
```

✅ Uses Index

```sql
WHERE ProducerId = 1
```

```sql
WHERE ProducerId = 1
AND YearOfRelease = 2024
```

❌ Doesn't efficiently use the index

```sql
WHERE YearOfRelease = 2024
```

Because the **leftmost column (`ProducerId`) is skipped.**

<br>

# Create Index

Clustered

```sql
CREATE CLUSTERED INDEX IX_Foundation_Movies_Id
ON Foundation.Movies (Id);
```

Non-Clustered

```sql
CREATE NONCLUSTERED INDEX IX_Foundation_Actors_Name
ON Foundation.Actors (Name);
```

<br>

# Drop Index

```sql
DROP INDEX IX_Foundation_Actors_Name
ON Foundation.Actors;
```

<br>

# View Indexes

In SQL Server Management Studio (SSMS):

```text
Database
 └── Tables
      └── Table
           └── Indexes
```

<br>

# When to Create an Index

Good candidates:

* Frequently searched columns (`WHERE`)
* Foreign Keys
* JOIN columns
* ORDER BY columns
* GROUP BY columns

Examples

```sql
CREATE INDEX IX_Foundation_Movies_ProducerId
ON Foundation.Movies (ProducerId);

CREATE INDEX IX_Foundation_ActorMovies_ActorId
ON Foundation.Actor_Movies (ActorId);

CREATE INDEX IX_Foundation_ActorMovies_MovieId
ON Foundation.Actor_Movies (MovieId);
```

<br>

# Avoid Indexes On

* Small tables
* Columns with frequent updates
* Columns with very few unique values (e.g., `Sex`)

<br>

# Clustered vs Non-Clustered

| Feature             | Clustered   | Non-Clustered         |
| ------------------- | ----------- | --------------------- |
| Physical data order | Yes         | No                    |
| Leaf nodes          | Actual data | Pointer to data       |
| Allowed per table   | 1           | Multiple              |
| Read performance    | Faster      | Fast                  |
| Write performance   | Slower      | Better than clustered |

<br>



### Interview Tips

* A **Primary Key** creates a **Clustered Index** by default (unless specified otherwise).
* Every index improves **reads** but adds overhead to **INSERT**, **UPDATE**, and **DELETE** operations.
* Use **Clustered Index** for columns used in range queries (e.g., `Id`, `CreatedAt`) and **Non-Clustered Index** for frequently searched or joined columns (e.g., `Name`, `ProducerId`).
