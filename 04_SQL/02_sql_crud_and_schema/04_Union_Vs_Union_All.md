# UNION vs UNION ALL

> [!Important]
>  For `UNION` and `UNION ALL` to work, the Number, Data types, and the order of the Columns in the select statements should be same


<br>

| Feature | UNION | UNION ALL (Understand as UNION Keep ALL)|
|--|--|--|
| Removes duplicate rows | ✅ Yes | ❌ No  |
| Keeps duplicate rows | ❌ No | ✅ Yes |
| Performance | Slower (checks duplicates) | Faster (Since no overhead to remove duplicates) |
| Use when | Need unique results | Need all results, including duplicates |

## Example Tables

### Table A

| Name |
|------|
| Alice |
| Bob |
| Charlie |

### Table B

| Name |
|------|
| Bob |
| David |
| Emma |

## UNION

```sql
SELECT Name FROM TableA
UNION
SELECT Name FROM TableB;
```

### Result

| Name |
|------|
| Alice |
| Bob |
| Charlie |
| David |
| Emma |

> **Note:** Duplicate values (e.g., **Bob**) are removed.

<br>

## UNION ALL

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
| David |
| Emma |

> **Note:** Duplicate values are retained.

<br>

## Key Difference

- **UNION** → Returns **unique** rows (duplicates removed).
- **UNION ALL** → Returns **all** rows, including duplicates (**faster** than `UNION`).
