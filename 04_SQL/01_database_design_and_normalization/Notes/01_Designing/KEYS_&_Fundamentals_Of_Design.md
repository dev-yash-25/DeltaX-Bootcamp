
###  **Database design** 
Tutorial for creating a car listing application. The process focuses on moving from a conceptual idea to a structured SQL database.


> [!Tip]
> 1. Dont waste to much time in making design perfect, use time in development, ou can refine design later as per requirements
> 2. Prefer to use proper names for table primary keys, ids , instead of `id`, name it `usersId`, `carsId`, `makeId`, `BodyTypesId`
> 3. Design **Linking** `Relationship tables`, like eg. `movieActors` table, `carPictures` tables
> 4. For cols you want to deactivate, you can flag them , like `isActive` set **false/true**
> 5. Whenever you see
>      ```
>       Many ↔ Many
>      ```
>     Your brain should instantly think -> **`Linking Table`**
>
> MovieActors
> ```
> MovieActors
> -------------
> MovieId (FK)
>
> ActorId (FK)
> ```
> Usually both columns together become a Composite Primary Key.
>```
> (MovieId, ActorId)
> ```

### 1. The Initial Design Process
The first step in database design is to **identify all the objects (entities)** that will be part of the system. For a car listing application, the primary entities include:
*   **Users:** People who post and view listings.
*   **Cars:** The actual vehicle listings.
*   **Makes, Models, and Body Types:** Categorical data for the vehicles.
*   **Pictures:** Media associated with the listings.

**Key Insight:** Don’t aim for perfection in the first phase. Database designs are iterative; you can add tables or columns as the application’s features evolve.

<div align = "center">
<img width="700" alt="image" src="https://github.com/user-attachments/assets/8ab101db-8b1c-4dbf-9f1b-3d07d0604fbf" />
</div>

### 2. Normalization: Why Separate Tables?
Instead of putting all information (like the "Make" of a car) into a single "Cars" table as text, it is best practice to **break them into separate tables**.
*   **Data Integrity:** It prevents misspellings. By using a separate `Makes` table, you can force users to select from a preset list (like a dropdown) rather than typing it manually.
*   **Storage Efficiency:** Storing an integer (ID) that references another table takes up much less storage space than repeating long strings (like "Chevrolet") a million times.
*   **Better Reporting:** Consistent spelling ensures that queries and reports return accurate data.

### 3. Defining Relationships and Keys
*   **Primary Keys:** Every table should have a unique identifier, such as `userID` or `carID`. In SQL Server, these are often set as **Identity Columns**, which automatically increment by one for every new record.
*   **Foreign Keys:** These are used to link tables together. For example, the `Cars` table includes a `userID` to identify which user posted the car and a `makeID` to identify the manufacturer.
*   **Hierarchical Links:** Sometimes entities are dependent on each other. For instance, a **Model should be linked to a Make** (e.g., "Impreza" is linked to "Subaru") so that the UI can narrow down choices for the user.

### 4. Handling Many-to-Many Relationships
When one entity can be linked to many others, a **linking table** (also called a join table) is often required.
*   **Example:** If you want to use the same `Pictures` table for both cars and user profiles, adding `carID` and `userID` columns directly to the `Pictures` table is considered poor practice.
*   **Solution:** A linking table like `car_pictures` (containing only `carID` and `pictureID`) allows for more flexibility without cluttering the main tables.

### 5. Best Practices for Table Columns
*   **Timestamps:** It is good practice to include columns for when a record was **created** and when it was **last modified**.
*   **Soft Deletes:** Instead of deleting data, use an **`is_active` flag** (boolean/bit). This allows you to deactivate a user or a listing without losing the historical data.
*   **Data Types:** 
    *   Use **`int`** for IDs and years.
    *   Use **`varchar`** for text strings, adjusting the length (e.g., 50 for names, 300+ for image paths or descriptions) based on the expected input.
    *   **Nullability:** Decide which fields are mandatory. For a user, you might require a `first_name` and `email` but make the `last_name` optional.

### 6. Implementation in SQL Server (SSMS)
When building the database:
1.  Create the **smaller, independent tables first** (Makes, Models, Body Types) before the tables that rely on them (Cars).
2.  Use the **UI (Edit Top 200 Rows)** or **SQL scripts** to populate initial data.
3.  If you realize a mistake (like a missing column), you can use an `ALTER` script or, if the database is still in development without data, drop and recreate the table.

<br>

# Keys

<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/0862ba43-4f8b-4a74-9d2c-0ddaddb364d8" />
   <br>
   <img width="500"  alt="image" src="https://github.com/user-attachments/assets/f912f5ec-0576-4e3a-9c23-bae3b6429d69" />
</div>


<br>

> [!TIP]
> ### Candidate Key vs Superkey
>
> **Candidate Key**
> - A candidate key is a **column or combination of columns** that **guarantees uniqueness** of every row.
> - It is **minimal** — no unnecessary column is included.
> - Multiple candidate keys can exist, and we can **nominate/select one as the Primary Key**.
>
> ```text
> MovieId                  → Candidate Key
> (MovieId, ActorId)       → Composite Candidate Key
> ```
>
> **Superkey**
> - A superkey is also a **column or combination of columns that guarantees uniqueness**.
> - But unlike a candidate key, it **can contain extra/non-key attributes**.
>
> ```text
> MovieId                         → Superkey
> MovieId + MovieName             → Superkey
> MovieId + MovieName + Year      → Superkey
> ```
>
> ### Simple difference
>
> **Candidate Key = Minimal combination guaranteeing uniqueness**
>
> **Superkey = Any combination guaranteeing uniqueness, even with extra columns**
>
> ```text
> Superkey
>    ↓
> Candidate Key
>    ↓
> Primary Key (one candidate we nominate/select)
> ```
>

<br>

## Candidate and Super Key

**Super Key:**

> [!Note]
> A super key is different from composite key, composite key can have only 2 or 3 max cols, that too group of 2 seperate `primary keys`
>
> Super key can have extra redundant non key attrubutes

<div align = "center">
<img width="500"  alt="image" src="https://github.com/user-attachments/assets/5e64e272-b02f-4460-a869-274ce8ba3b52" />
</div>


**Candidate Key:**

<div align = "center">
<img width="500"  alt="image" src="https://github.com/user-attachments/assets/a8d920d0-f87c-4588-9596-f192e50b89bf" />
</div>


 A **Candidate Key can have 1 or more columns**.

Example:

| Student_ID | Course_ID | Name  | Marks |
| ---------- | --------- | ----- | ----- |
| 101        | C1        | Rahul | 80    |
| 101        | C2        | Rahul | 75    |
| 102        | C1        | Priya | 90    |

Suppose **Student_ID alone is NOT unique**, and **Course_ID alone is NOT unique**.

But together:

**{Student_ID, Course_ID}** → uniquely identifies each row.

So:

* `{Student_ID, Course_ID}` = **Candidate Key** ✅ (minimal)
* `{Student_ID, Course_ID, Name}` = **Super Key** ✅ (uniquely identifies, but has an extra column)
* `{Student_ID, Course_ID, Marks}` = **Super Key** ✅


**Candidate Key = Super Key with NO unnecessary column.**


