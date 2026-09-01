<img width="474" height="331" alt="image" src="https://github.com/user-attachments/assets/d9cef2db-96dd-4caa-a4e6-30921c26b3a3" /># Rapido Database Design & SQL Practice

## Database Design
<br>
<div align = "center">
<img width="650" alt="image" src="https://github.com/user-attachments/assets/f724503a-837a-4362-bc2f-ecd734da3baf" />
    <br>
<img width="650" alt="image" src="https://github.com/user-attachments/assets/43d3e4bc-785f-429e-82fc-399b86c00ef0" />
</div>
<br>



# SQL Practice Queries

<br>

## 1. Total Rides Per User

### Query

```sql
SELECT
    u.ID,
    u.Name,
    COUNT(r.ID) AS Total_Rides
FROM Users u
LEFT JOIN Rides r
    ON u.ID = r.User_ID
GROUP BY
    u.ID,
    u.Name;
```

### Logic

- LEFT JOIN keeps users even if they never booked.
- COUNT(r.ID) counts rides.
- GROUP BY creates one row per user.

<br>

## 2. Top 5 Riders by Completed Rides

### Query

```sql
SELECT TOP 5
    Rider_ID,
    COUNT(*) AS Total_Rides
FROM Rides
WHERE Status = 'Completed'
GROUP BY Rider_ID
ORDER BY Total_Rides DESC;
```

### Logic

- Consider only completed rides.
- Count rides for every rider.
- Sort descending.
- Return top five.

<br>

## 3. Average Rating Received by Each Rider

### Query

```sql
SELECT
    U.Id,
    U.Name,
    AVG(RT.Rating_Value) AS Avg_Rating
FROM Rider R
INNER JOIN User U
    ON R.User_ID = U.Id
INNER JOIN Ratings RT
    ON RT.To_User_ID = U.Id
GROUP BY
    U.Id,
    U.Name;
```

> [!Caution]
> Do not GROUP BY the column that you're aggregating. eg `GROUP BY RT.Rating_Value

### Logic

- Join riders with received ratings.
- Average all ratings.
- One row per rider.

<br>

## 4. Total Earnings Per Rider

### Query

```sql
SELECT
    Rider_ID,
    SUM(Fare) AS Total_Earnings
FROM Rides
WHERE Status = 'Completed'
GROUP BY Rider_ID;
```

### Logic

- Completed rides generate earnings.
- SUM adds fares.
- GROUP BY rider.

<br>

## 5. Users Who Never Booked a Ride

### Query

```sql
SELECT
    u.ID,
    u.Name
FROM Users u
LEFT JOIN Rides r
    ON u.ID = r.User_ID
WHERE r.ID IS NULL;
```

### Logic

- LEFT JOIN keeps every user.
- Users without matching rides have NULL.
- Filter NULL values.

<br>

## 6. Most Frequent Pickup Location

### Query

```sql
SELECT TOP 1
    Pickup_Location,
    COUNT(*) AS Ride_Count
FROM Rides
GROUP BY Pickup_Location
ORDER BY Ride_Count DESC;
```

### Logic

- Count rides for every pickup location.
- Highest count is the most popular.

<br>

## 7. Ride Duration

### Query

```sql
SELECT
    ID,
    DATEDIFF(MINUTE, Started_At, Completed_At) AS Duration_Minutes
FROM Rides
WHERE Status = 'Completed';
```

### Logic

- DATEDIFF calculates minutes between start and end.
- Only completed rides have both timestamps.

<br>

## 8. Riders Having Average Rating Below 3

### Query

```sql
SELECT
    r.ID,
    r.Name,
    AVG(rt.Rating_Value) AS Avg_Rating
FROM Riders r
JOIN Ratings rt
    ON r.ID = rt.To_User_ID
GROUP BY
    r.ID,
    r.Name
HAVING AVG(rt.Rating_Value) < 3;
```

### Logic

- GROUP BY rider.
- HAVING filters grouped data.
- Return riders with average rating less than 3.

<br>

## 9. Peak Booking Hour

### Query

```sql
SELECT TOP 1
    DATEPART(HOUR, Requested_At) AS Booking_Hour,
    COUNT(*) AS Total_Rides
FROM Rides
GROUP BY
    DATEPART(HOUR, Requested_At)
ORDER BY Total_Rides DESC;
```

### Logic

- Extract hour from booking time.
- Count bookings in each hour.
- Highest count = peak booking hour.

> [!Note]
> ```sql
> DATEPART(HOUR, Requested_At)
> ```
>
> | Requested_At | DATEPART(HOUR, Requested_At) |
> | ------------ | ---------------------------- |
> | 08:10        | 8                            |
> | 08:45        | 8                            |
> | 09:05        | 9                            |
> | 09:15        | 9                            |
> | 09:30        | 9                            |
> | 10:12        | 10                           |


<br>

## 10. User-Rider Pairs Having 3 or More Completed Rides Together

### Query

```sql
SELECT
    User_ID,
    Rider_ID,
    COUNT(*) AS Ride_Count
FROM Rides
WHERE Status = 'Completed'
GROUP BY
    User_ID,
    Rider_ID
HAVING COUNT(*) >= 3;
```

### Logic

- Group by both user and rider.
- Count rides between the pair.
- Keep only pairs having at least three completed rides.

<br>

# SQL Concepts Covered

- INNER JOIN
- LEFT JOIN
- GROUP BY
- HAVING
- Aggregate Functions
  - COUNT()
  - AVG()
  - SUM()
- DATEPART()
- DATEDIFF()
- ORDER BY
- TOP
- NULL Handling
- Foreign Keys
- One-to-Many Relationships
- Basic Database Normalization
