# SQL Assignment 3 - Queries Summary

Database Used: `IMDB_Dummy`

<br>

# 1. Get Age of Actors in Days

### Query

```sql
SELECT
    Name,
    DATEDIFF(DAY, DateOfBirth, GETDATE()) AS AgeInDays
FROM Foundation.Actors;
```

### Logic

- Read actors from `Actors`.
- Calculate the difference between `DateOfBirth` and today's date using `DATEDIFF`.
- Display actor name and age in days.

### Sample Output

| ActorName | AgeInDays |
|------------|----------:|
| Vicky Kaushal | 13970 |
| Yami Gautam | 13773 |
| Paresh Rawal | 25900 |

<br>

# 2. Get Actors who worked with Producer X

### Query

```sql
SELECT DISTINCT
    A.Name AS ActorName
FROM Foundation.Producers P
INNER JOIN Foundation.Movies M
    ON P.Id = M.ProducerId
INNER JOIN Foundation.Actor_Movies AM
    ON M.Id = AM.MovieId
INNER JOIN Foundation.Actors A
    ON AM.ActorId = A.Id
WHERE P.Name = 'Aditya Dhar';
```

### Logic

- Find the producer.
- Retrieve movies produced by that producer.
- Find actors associated with those movies.
- Use `DISTINCT` to remove duplicate actor names.

### Join Path

```
Producers
    ↓
Movies
    ↓
Actor_Movies
    ↓
Actors
```

### Sample Output

| ActorName |
|------------|
| Vicky Kaushal |
| Yami Gautam |
| Paresh Rawal |

<br>

### Alternative Using GROUP BY

```sql
SELECT
    A.Id,
    A.Name AS ActorName
FROM Foundation.Producers P
INNER JOIN Foundation.Movies M
    ON P.Id = M.ProducerId
INNER JOIN Foundation.Actor_Movies AM
    ON M.Id = AM.MovieId
INNER JOIN Foundation.Actors A
    ON AM.ActorId = A.Id
WHERE P.Name = 'Aditya Dhar'
GROUP BY
    A.Id,
    A.Name;
```

### DISTINCT vs GROUP BY

```text
DISTINCT
→ Remove duplicate result rows.

GROUP BY
→ Create groups, usually for aggregation/group-level filtering.
```

For simply finding unique actors, `DISTINCT` expresses the intention more directly.


<br>


# 3. Actors who acted together in Two or More Movies

### Query

```sql
SELECT
    A1.Name AS Actor1,
    A2.Name AS Actor2,
    COUNT(*) AS MoviesTogether
FROM Foundation.Actor_Movies AM1

INNER JOIN Foundation.Actor_Movies AM2
    ON AM1.MovieId = AM2.MovieId
    AND AM1.ActorId < AM2.ActorId

INNER JOIN Foundation.Actors A1
    ON AM1.ActorId = A1.Id

INNER JOIN Foundation.Actors A2
    ON AM2.ActorId = A2.Id

GROUP BY
    A1.Name,
    A2.Name

HAVING COUNT(*) >= 2;
```

### Logic

```
Actor_Movies
     ↓
Self Join on MovieId
     ↓
Find actors in the same movie
     ↓
ActorId < ActorId
     ↓
Remove self-pairs and duplicate A-B / B-A pairs
     ↓
GROUP BY actor pair
     ↓
COUNT movies together
     ↓
HAVING COUNT(*) >= 2
```

### Why `ActorId < ActorId`?

Without it:

```text
A + B
B + A
A + A
B + B
```

With it:

```text
A + B
```


Only one direction remains, and an actor cannot pair with themselves.


### Important GROUP BY + COUNT concept

`COUNT(*)` counts rows **inside each group**, not the entire table.

```text
GROUP BY Actor1, Actor2
        ↓
Creates one group per actor pair
        ↓
COUNT(*)
        ↓
Counts rows inside that pair's group
```


### Join Path

```
Actor_Movies
      ↓
Actor_Movies (Self Join)
      ↓
Actors
      ↓
Actors
```

### Sample Output

| Actor1 | Actor2 | MoviesTogether |
|---------|---------|---------------:|
| Vicky Kaushal | Yami Gautam | 2 |
| Christian Bale | Cillian Murphy | 2 |

<br>

# 4. Get the Youngest Actor

### Query

```sql
SELECT TOP 1
    Name AS ActorName,
    DateOfBirth
FROM Foundation.Actors
ORDER BY DateOfBirth DESC;
```

### Logic

- Latest date of birth means youngest actor.
- Sort DOB in descending order.
- Return first row.

### Sample Output

| ActorName | DateOfBirth |
|------------|-------------|
| Alia Bhatt | 1993-03-15 |

<br>

# 5. Actors who have Never Worked Together

## Method 1 — `EXCEPT`


```sql
SELECT
    A1.Name,
    A2.Name
FROM Foundation.Actors A1

INNER JOIN Foundation.Actors A2
    ON A1.Id < A2.Id

EXCEPT

SELECT
    A1.Name,
    A2.Name
FROM Foundation.Actor_Movies AM1

INNER JOIN Foundation.Actor_Movies AM2
    ON AM1.MovieId = AM2.MovieId

INNER JOIN Foundation.Actors A1
    ON AM1.ActorId = A1.Id

INNER JOIN Foundation.Actors A2
    ON AM2.ActorId = A2.Id

WHERE AM1.ActorId < AM2.ActorId;
```

### Logic

```
All Possible Actor Pairs

        -

Pairs who worked together

        =

Pairs who never worked together
```

## Method 2 — LEFT JOIN

- Using LEFT JOIN (Optimal)
- Think what it looks of actor with no common movie
- it looks A1.Name/AM1.Name has val, A1.Name/AM1.Name has NULL,
```
 A1 + AM1 → has a value (Actor 1's movie)
 A2 + AM2 → NULL (no matching movie for Actor 2)
```
```sql
SELECT
    A1.Name AS Actor1,
    A2.Name AS Actor2
FROM Foundation.Actors A1
INNER JOIN Foundation.Actors A2
    ON A1.Id < A2.Id

LEFT JOIN Foundation.Actor_Movies AM1
    ON A1.Id = AM1.ActorId

LEFT JOIN Foundation.Actor_Movies AM2
    ON A2.Id = AM2.ActorId
    AND AM1.MovieId = AM2.MovieId

GROUP BY
    A1.Id,
    A2.Id,
    A1.Name,
    A2.Name

HAVING COUNT(AM2.MovieId) = 0;
```
| A1.Name | AM1.MovieId | A2.Name | AM2.MovieId |
| ------- | ----------: | ------- | ----------: |
| A       |           1 | B       |           1 |
| A       |           1 | C       |        NULL |
| B       |           1 | C       |        NULL |


<br>
<div align = "center">
<img width="500" alt="image" src="https://github.com/user-attachments/assets/54910b28-1ac3-4c81-a357-96a00ff5b872" />
    <br>
    <img width="350" alt="image" src="https://github.com/user-attachments/assets/c007083e-7eee-4d6b-a9a3-e6d1edd48526" />
</div>
<br>

### How to consciously think about it

```text
1. Get all unique actor pairs
   A1.Id < A2.Id

        ↓

2. LEFT JOIN Actor_Movies as AM1
   Get movies of Actor1

        ↓

3. LEFT JOIN Actor_Movies as AM2
   Get movies of Actor2

   AND require:
   AM1.MovieId = AM2.MovieId

        ↓

4. If Actor2 has the same movie
   → AM2.MovieId gets a value

   If Actor2 doesn't have that movie
   → AM2.MovieId = NULL

        ↓

5. GROUP BY the actor pair

        ↓

6. Count AM2.MovieId

   Matching movie → counted
   NULL           → NOT counted

        ↓

7. HAVING COUNT(AM2.MovieId) = 0

        ↓

   Only actor pairs with ZERO
   common movies remain
```

### Why not simply use `WHERE AM2.MovieId IS NULL`?

Because the same actor pair can have both matching and non-matching rows.

Example:

```text
A's movies: 1, 2
B's movies: 1, 3
```

Joined result can contain:

```text
A + B + Movie 1
A + B + NULL
```

`WHERE AM2.MovieId IS NULL` would incorrectly keep A-B.

Instead:

```sql
HAVING COUNT(AM2.MovieId) = 0
```

asks:

> Did this entire actor pair have zero matching movies?

### `COUNT(column)` and NULL

`COUNT(column)` does not count NULL.

```text
AM2.MovieId
-----------
1
NULL
NULL

COUNT(AM2.MovieId) = 1
```

Therefore:

```text
COUNT(AM2.MovieId) = 0
```

means there were **no matching movie rows**.

Important distinction:

```text
IS NULL
→ Row-level check

COUNT(column) = 0
→ Group-level check
```



### Sample Output

| Actor1 | Actor2 |
|---------|---------|
| Yami Gautam | Christian Bale |
| Paresh Rawal | Tom Hardy |

<br>

# 6. Number of Movies in Each Language

### Query

```sql
SELECT
    Language,
    COUNT(*) AS MovieCount
FROM Foundation.Movies
GROUP BY Language;
```

### Logic

- Group movies by language.
- Count movies in each language.

### Sample Output

| Language | MovieCount |
|-----------|-----------:|
| Hindi | 5 |
| English | 3 |
| Tamil | 2 |
| Marathi | 1 |

<br>

# 7. Total Profit of Movies in Each Language

### Query

```sql
SELECT
    Language,
    ISNULL(SUM(Profit),0) AS TotalProfit
FROM Foundation.Movies
GROUP BY Language;
```

### Logic

- Group movies by language.
- Add profits using `SUM`.
- Replace NULL with 0 using `ISNULL`.

### Sample Output

| Language | TotalProfit |
|-----------|------------:|
| Hindi | 2550 |
| English | 3100 |
| Tamil | 1100 |
| Marathi | 300 |

<br>

# 8. Total Profit of Movies having Actor X in Each Language

### Query

```sql
SELECT
    M.Language,
    ISNULL(SUM(M.Profit),0) AS TotalProfit
FROM Foundation.Movies M

INNER JOIN Foundation.Actor_Movies AM
    ON M.Id = AM.MovieId

INNER JOIN Foundation.Actors A
    ON AM.ActorId = A.Id

WHERE A.Name = 'Yami Gautam'

GROUP BY M.Language;
```

### Logic

- Find Actor X.
- Get movies acted by that actor.
- Retrieve movie language and profit.
- Group by language.
- Sum profits.

### Join Path

```
Actors
    ↓
Actor_Movies
    ↓
Movies
```

### Sample Output

| Language | TotalProfit |
|-----------|------------:|
| Hindi | 750 |
| Marathi | 300 |

<br>

# 9. Total Profit by Year of Release and Language

### Query

```sql
SELECT
    YearOfRelease,
    Language,
    ISNULL(SUM(Profit),0) AS TotalProfit
FROM Foundation.Movies
GROUP BY
    YearOfRelease,
    Language;
```

### Logic

- Group movies by release year and language.
- Sum profits for each combination.

### Sample Output

| Year | Language | TotalProfit |
|------:|-----------|------------:|
| 2008 | English | 1000 |
| 2019 | Hindi | 500 |
| 2022 | Tamil | 700 |
| 2023 | English | 1200 |

<br>

# 10. Number of Movies in Each Language Produced by Each Producer

### Query

```sql
SELECT
    P.Name AS ProducerName,
    M.Language,
    COUNT(M.Id) AS MovieCount
FROM Foundation.Movies M

INNER JOIN Foundation.Producers P
    ON M.ProducerId = P.Id

GROUP BY
    P.Name,
    M.Language;
```

### Logic

- Join Movies with Producers.
- Group by Producer and Language.
- Count movies for each producer-language combination.

### Join Path

```
Movies
    ↓
Producers
```

### Sample Output

| Producer | Language | MovieCount |
|-----------|-----------|-----------:|
| Aditya Dhar | Hindi | 3 |
| Christopher Nolan | English | 3 |
| Rajkumar Hirani | Hindi | 2 |
| Rajkumar Hirani | Marathi | 1 |
| Mani Ratnam | Tamil | 2 |


<br>

---

<br>



---

# Core SQL Concepts From These Queries

## 1. GROUP BY

`GROUP BY` creates groups based on specified columns.

```sql
GROUP BY Language
```

means:

```text
Hindi group
English group
Tamil group
...
```

Then aggregate functions operate inside those groups.

---

## 2. SELECT After GROUP BY

After grouping, a selected column must generally be:

```text
1. Present in GROUP BY

OR

2. Inside an aggregate function
```

Valid:

```sql
SELECT
    Language,
    SUM(Profit)
FROM Movies
GROUP BY Language;
```

Invalid:

```sql
SELECT
    Language,
    Name,
    SUM(Profit)
FROM Movies
GROUP BY Language;
```

`Name` is neither grouped nor aggregated.

---

## 3. Can an Aggregated Column Also Be in GROUP BY?

Technically yes:

```sql
SELECT
    Language,
    Profit,
    SUM(Profit)
FROM Movies
GROUP BY
    Language,
    Profit;
```

But this changes the groups.

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
Hindi + 500 → 500
Hindi + 250 → 250
Hindi + 350 → 350
```

### Rule

> `GROUP BY` defines what makes rows belong to the same group. Aggregation operates on the rows inside that group.

---

## 4. DISTINCT vs GROUP BY

### DISTINCT

Use when the intention is simply:

```text
Remove duplicate result rows
```

```sql
SELECT DISTINCT
    A.Id,
    A.Name
FROM ...;
```

### GROUP BY

Use when the intention is:

```text
Create groups for aggregation
```

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

### Quick Rule

```text
DISTINCT
→ "Give me unique rows."

GROUP BY
→ "Create groups so I can calculate something for each group."
```

Both can sometimes produce the same result, but they express different intentions.

---

## 5. WHERE vs HAVING

### WHERE

Filters individual rows **before grouping**.

```sql
WHERE Language = 'Hindi'
```

### HAVING

Filters groups **after grouping/aggregation**.

```sql
HAVING COUNT(*) >= 2
```

### Mental Execution Flow

```text
FROM / JOIN
     ↓
WHERE
     ↓
GROUP BY
     ↓
Aggregation
     ↓
HAVING
     ↓
SELECT
```

---

## 6. Self JOIN

A table can be joined with itself using different aliases.

```sql
Actor_Movies AM1
        JOIN
Actor_Movies AM2
```

Useful when comparing rows within the same table.

Example:

```text
AM1 → Actor 1
AM2 → Actor 2
```

---

## 7. `A1.Id < A2.Id`

Used when creating unique pairs.

Without it:

```text
A-B
B-A
A-A
B-B
```

With:

```sql
A1.Id < A2.Id
```

we get:

```text
A-B
A-C
B-C
```

It removes:

- Self-pairs
- Reverse duplicates

---

## 8. LEFT JOIN for "No Match"

General pattern:

```text
All rows
   ↓
LEFT JOIN matching rows
   ↓
Unmatched side becomes NULL
   ↓
GROUP BY / IS NULL
   ↓
Find missing relationships
```

For the actor problem:

```text
All actor pairs
      ↓
Try to find common movies
      ↓
No common movie
      ↓
COUNT(common movie) = 0
```

---

## 9. STRING_AGG

`STRING_AGG()` combines multiple values into one string.

```sql
STRING_AGG(A.Name, ', ')
```

Example:

```text
Vicky Kaushal
Yami Gautam
Paresh Rawal

        ↓

Vicky Kaushal, Yami Gautam, Paresh Rawal
```

With:

```sql
GROUP BY M.Id
```

it performs the aggregation **inside each movie's group**.

```text
JOIN
 ↓
Multiple actor rows per movie
 ↓
GROUP BY movie
 ↓
STRING_AGG(actor names)
 ↓
One row per movie
```

---

# High-Value Mental Models

```text
DISTINCT
→ Remove duplicate rows

GROUP BY
→ Create groups

COUNT()
→ Count rows/values inside each group

COUNT(column)
→ NULL values are not counted

SUM()
→ Add values inside each group

HAVING
→ Filter groups

WHERE
→ Filter rows

A1.Id < A2.Id
→ Unique pairs, no self-pairs

LEFT JOIN + NULL
→ Find unmatched rows

LEFT JOIN + GROUP BY + COUNT() = 0
→ Find groups with no matching rows

STRING_AGG()
→ Combine multiple values into one string

SELF JOIN
→ Compare rows within the same table

DATEDIFF()
→ Difference between two dates
```
