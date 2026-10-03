# Constraints

| Constraint      | Purpose                                                                                                   | Example                                               |
| --------------- | --------------------------------------------------------------------------------------------------------- | ----------------------------------------------------- |
| **PRIMARY KEY** | Uniquely identifies each row. Cannot contain `NULL`. Automatically creates a unique index.                | `EmployeeID INT PRIMARY KEY`                          |
| **FOREIGN KEY** | Maintains referential integrity by linking a column to another table's primary key.                       | `FOREIGN KEY (DeptID) REFERENCES Departments(DeptID)` |
| **UNIQUE**      | Ensures all values in a column (or column combination) are unique. Allows one `NULL` value in SQL Server. | `Email VARCHAR(100) UNIQUE`                           |
| **NOT NULL**    | Prevents `NULL` values from being inserted into a column.                                                 | `Name VARCHAR(50) NOT NULL`                           |
| **CHECK**       | Restricts values based on a condition.                                                                    | `CHECK (Salary >= 0)`                                 |
| **DEFAULT**     | Automatically assigns a default value if none is provided.                                                | `Status VARCHAR(20) DEFAULT 'Active'`                 |

<br>

---

<br>


### Default Constraint
<img width="700" alt="image" src="https://github.com/user-attachments/assets/0eefb1ce-0bcd-4e67-8967-be74baef3171" />

<br>

---

<br>


### Check Constraint
<img width="700" alt="image" src="https://github.com/user-attachments/assets/a0672aa0-302d-4286-9a4b-f49e7e6e9605" />
<img width="641" height="409" alt="image" src="https://github.com/user-attachments/assets/eaaa2210-1f0b-4423-ab90-2e55cad4e4e2" />


<br>

---

<br>


### Identity Constraint


- Like `Auto Increment` in MySQL
- But, you cannot insert in a table having identity constraint, so we use this below

### `SET IDENTITY_INSERT`


> [!Tip]
> In simple, Identity (Like Id) is auto added to a table on inserting data,\
> but if you want to add manually, you need to TURN **ON** `IDENTITY_INSERT`

**Purpose:** Allows inserting explicit values into an `IDENTITY` column, typically used when you need to manually restore or re-seed specific primary key values.


#### Demo Table

**Foundation.Actor**

| Id | Name               | Sex    |
| -- | ------------------ | ------ |
| 1  | Robert Downey Jr.  | Male   |
| 2  | Scarlett Johansson | Female |
| 3  | Chris Evans        | Male   |



#### Example Workflow (Delete and Reinsert ID)

| Step | Action                       | SQL                                                                                     |
| ---- | ---------------------------- | --------------------------------------------------------------------------------------- |
| 1    | Delete existing record       | `DELETE FROM Foundation.Actor WHERE Id = 1;`                                            |
| 2    | Enable identity insert       | `SET IDENTITY_INSERT Foundation.Actor ON;`                                              |
| 3    | Reinsert record with same ID | `INSERT INTO Foundation.Actor (Id, Name, Sex) VALUES (1, 'Robert Downey Jr.', 'Male');` |
| 4    | Disable identity insert      | `SET IDENTITY_INSERT Foundation.Actor OFF;`                                             |



#### Full Syntax Example

```sql
-- Step 1: Remove existing record
DELETE FROM Foundation.Actor
WHERE Id = 1;

-- Step 2: Allow manual identity insert
SET IDENTITY_INSERT Foundation.Actor ON;

-- Step 3: Reinsert record with explicit ID
INSERT INTO Foundation.Actor
(Id, Name, Sex)
VALUES
(1, 'Robert Downey Jr.', 'Male');

-- Step 4: Turn identity insert back off
SET IDENTITY_INSERT Foundation.Actor OFF;

-- Then If you delete all rows in table, the table id would not be reset, so
-- it may start from another value,
-- So use DBCC to reset index
```



#### Notes

* Only **one table per session** can have `IDENTITY_INSERT ON`.
* You must explicitly include the identity column in the `INSERT`.
* Always turn `IDENTITY_INSERT` **OFF** after completing the operation.
* Deleting and reinserting is useful for **data correction, migration, or reseeding** scenarios.
* If you delete 



#### When to Use

* Restoring deleted primary key records
* Migrating data between environments
* Seeding databases with fixed IDs
* Fixing inconsistent identity sequences

<br>

---

<br>


### Unique Constraint

| Feature | Primary Key | Unique Key |
| :--- | :--- | :--- |
| **Purpose** | Uniquely identifies each record/row. | Prevents duplicate data within a column. |
| **Quantity** | Only **one** allowed per table. | **Multiple** allowed per table. |
| **NULL Values** | Strictly **no NULL** values allowed. | Allows **one NULL** value (varies by database). |
| **Default Index** | Automatically creates a **Clustered Index**. | Automatically creates a **Non-Clustered Index**. |
| **Modifications** | Difficult to change or remove. | Easy to change or drop. |

<br>

---

<br>

