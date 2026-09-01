# Doctor Management System - Database Design

<br>
<div align = "center">
<img width="700" alt="image" src="https://github.com/user-attachments/assets/88c2b243-2fab-466a-a0e5-e9f3336fef70" />
</div>
<br>

### Junction Table — Points to Remember

1. **Ask: "What does this attribute describe?"**

   * Entity → put in entity table.
   * Relationship → put in junction table.

2. **Relationship-specific data → junction table**

   * `Prescription ↔ Medicine` → dosage, frequency, duration.
   * `Patient ↔ Room` → arrival, discharge.

3. **Don't put changing/transaction data in the entity**

   * `Medicine.quantity` ❌ → `Inventory.quantity` ✅
   * `Patient.age` ❌ → `Patient.date_of_birth` ✅

4. **Junction table is not only for M:N**

   * Use it for 1:1 / 1:N too **if the relationship itself needs data or history**.

5. **Think of the junction table as a sentence:**

   > "Patient X stayed in Room Y **from A to B**."

6. **Normalization shortcut:**

   > If the value depends on **both entities together**, it belongs in the junction/association table.

**Your main mistake:** you were putting relationship attributes (`dosage`, `arrival/discharge`) inside the entity tables\
instead of asking **who/what the fact belongs to**.


<br>

## Queries


Doctors - Name
Patients 

Rooms - Id, RoomNumber
RoomsOccupancy - Id, RoomId FK -> Rooms.Id, Status, PatientId FK -> Patients.Id, StartTime, EndTime




LIST of all patients along with the number of doctors they will have visited
```sql
SELECT P.Name,A.DrId COUNT(*) as TOTAL_Doctors
FROM Patients P 
LEFT JOIn Appointment A
ON P.Id = A.PatientId
GROUP BY P.Id, P.Name, A.DrId;
```
```sql
SELECT C.Name , COUNT(*) as TOtal Reviews
FROM Customers C 
INNER JOIN Reviews R 
ON C.Id = R.CustomerId
GROUP BY C.Id, C.Name;
```

Find most fav dish of users who  joined after 1st aug


```sql
SELECT  TOP 1 MI.Name , COUNT(*) as TotalOrders
FROM MenuItem MI INNER JOIN RestaurantMenuItem RMI
ON MI.Id = RMI.ItemId
INNER JOIN OrderItems OI
ON OI.itemId = RMI.itemId
INNER JOIN Orders O 
ON O.Id = OI.orderId
INNER JOIN Customer C
ON O.customer_id = C.Id
WHERE  CAST(joineddate as date) > '01-08-2026'
GROUP BY MI.Id, MI.Name
ORDER BY TotalOrders DESC;
```

Find the p of customers who never ordered  a dish on the same day

```sql
SELECT C1.Name, C2.Name
FROM Customers C1 
INNER JOIN Customers C2
ON C1.Id < C2.Id
LEFT JOIN Orders O1
ON C1.ID = O1.cusotomerId
LEFT JOIN Orders O2
ON C2.Id =O2.customerId
AND O1.order_date = O2.order_date
GROUP BY
C1.Id, C2.Id, C1.Name, C2.Name
HAVING COUNT(O2.order_date) = 0; 
```

Find the name of dishes which
 were never reviewed the day they were ordered

```sql
SELECT MI.Name 
FROM Menuitem MI 
INNER JOIN Restaurant_MenuItem RMI
ON RMI.ItemId = MI.Id
INNER JOIN OrderItems OI
ON OI.itemId = RMI.itemId
LEFT JOIN Orders O
ON OI.orderId = O.Id
LEFT JOIN Reviews RW
ON RW.restaurantmenuitem = RMI.Id
AND CAST(O.order_Date as Date) = CAST(R.Reviewdate as date)
WHERE R.ReviewDate IS NULL
GROUP BY MI.Id, MI.Name
```

```sql
SELECT
C.Name,
MI.ItemId,
MI.Name,
COUnT(O.Id) as OrderCount
FROM Customers C
INNER JOIN Orders
 ON C.Id = O.customer_id
INNER JOIN OrderItems OI
ON O.Id = OI.orderId
INNER JOIN RestoMenuItems RMI
ON OI.ItemId = RMI.itemid
INNer joiN MenuItems MI
ON Mi.id = RMI.itemId
GROUP BY C.Id, C.name, mi.id, mi.name
HAVING ordercount > 1;
```
