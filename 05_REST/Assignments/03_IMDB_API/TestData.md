# IMDB API — Postman Test Data Referenc

Use this document as the reference dataset while testing the API in Postman.

<br>

# 1. Producers

## Producer 1

**POST**

```text
/api/producers
```

```json
{
  "name": "Christopher Nolan",
  "dob": "1970-07-30",
  "bio": "British-American filmmaker known for complex narratives and innovative filmmaking.",
  "gender": "Male"
}
```

Expected:

```text
201 Created
ID → 1
```

<br>

# 2. Actors

## Actor 1

**POST**

```text
/api/actors
```

```json
{
  "name": "Leonardo DiCaprio",
  "dob": "1974-11-11",
  "bio": "American actor known for a wide range of dramatic and film roles.",
  "gender": "Male"
}
```

Expected:

```text
ID → 1
```

<br>

## Actor 2

```json
{
  "name": "Joseph Gordon-Levitt",
  "dob": "1981-02-17",
  "bio": "American actor and filmmaker.",
  "gender": "Male"
}
```

Expected:

```text
ID → 2
```

<br>

## Actor 3

```json
{
  "name": "Tom Hardy",
  "dob": "1977-09-15",
  "bio": "English actor known for roles in action and dramatic films.",
  "gender": "Male"
}
```

Expected:

```text
ID → 3
```

<br>

# 3. Genres

Create these genres before creating movies.

## Genre 1

```text
POST /api/genres
```

```json
{
  "name": "Science Fiction"
}
```

Expected:

```text
ID → 1
```

<br>

## Genre 2

```json
{
  "name": "Thriller"
}
```

Expected:

```text
ID → 2
```

<br>

## Genre 3

```json
{
  "name": "Drama"
}
```

Expected:

```text
ID → 3
```

<br>

# 4. Movies

The following dataset assumes:

```text
Producer 1 → Christopher Nolan

Actors
1 → Leonardo DiCaprio
2 → Joseph Gordon-Levitt
3 → Tom Hardy

Genres
1 → Science Fiction
2 → Thriller
3 → Drama
```

<br>

## Movie 1 — Inception

**POST**

```text
/api/movies
```

Body → **raw → JSON**

```json
{
  "name": "Inception",
  "yearOfRelease": 2010,
  "plot": "A thief who steals secrets through dream-sharing technology is given an impossible task.",
  "coverImage": "https://example.com/inception.jpg",
  "producerId": 1,
  "actorIds": [1, 2, 3],
  "genreIds": [1, 2]
}
```

Expected:

```text
Movie ID → 1
```

Relationships:

```text
Producer → Christopher Nolan

Actors
→ Leonardo DiCaprio
→ Joseph Gordon-Levitt
→ Tom Hardy

Genres
→ Science Fiction
→ Thriller
```

<br>

## Movie 2 — The Dark Knight

```json
{
  "name": "The Dark Knight",
  "yearOfRelease": 2008,
  "plot": "Batman faces a criminal mastermind who plunges Gotham into chaos.",
  "coverImage": "https://example.com/dark-knight.jpg",
  "producerId": 1,
  "actorIds": [3],
  "genreIds": [2, 3]
}
```

Expected:

```text
Movie ID → 2
```

<br>

## Movie 3 — Interstellar

```json
{
  "name": "Interstellar",
  "yearOfRelease": 2014,
  "plot": "Explorers travel through a wormhole in search of a new home for humanity.",
  "coverImage": "https://example.com/interstellar.jpg",
  "producerId": 1,
  "actorIds": [1, 3],
  "genreIds": [1, 3]
}
```

Expected:

```text
Movie ID → 3
```

<br>

## Movie 4 — Dream Within

This deliberately uses **2010 again** so that year filtering can be tested properly.

```json
{
  "name": "Dream Within",
  "yearOfRelease": 2010,
  "plot": "A mysterious dream experiment blurs the boundary between reality and imagination.",
  "coverImage": "https://example.com/dream-within.jpg",
  "producerId": 1,
  "actorIds": [2],
  "genreIds": [1, 2]
}
```

Expected:

```text
Movie ID → 4
```

<br>

# 5. Final Dataset

After inserting everything successfully:

```text
PRODUCER
────────────────────────
1 → Christopher Nolan


ACTORS
────────────────────────
1 → Leonardo DiCaprio
2 → Joseph Gordon-Levitt
3 → Tom Hardy


GENRES
────────────────────────
1 → Science Fiction
2 → Thriller
3 → Drama


MOVIES
────────────────────────────────────────────
1 → Inception        → 2010
2 → The Dark Knight  → 2008
3 → Interstellar     → 2014
4 → Dream Within     → 2010
```

<br>

# 6. Movie Relationships

```text
Inception
├── Producer: Christopher Nolan
├── Actors:
│   ├── Leonardo DiCaprio
│   ├── Joseph Gordon-Levitt
│   └── Tom Hardy
└── Genres:
    ├── Science Fiction
    └── Thriller


The Dark Knight
├── Producer: Christopher Nolan
├── Actors:
│   └── Tom Hardy
└── Genres:
    ├── Thriller
    └── Drama


Interstellar
├── Producer: Christopher Nolan
├── Actors:
│   ├── Leonardo DiCaprio
│   └── Tom Hardy
└── Genres:
    ├── Science Fiction
    └── Drama


Dream Within
├── Producer: Christopher Nolan
├── Actors:
│   └── Joseph Gordon-Levitt
└── Genres:
    ├── Science Fiction
    └── Thriller
```

<br>

# 7. GET Tests

## Get all movies

```http
GET /api/movies
```

Expected:

```text
200 OK
```

Should return **4 movies**.

<br>

## Get movie by ID

```http
GET /api/movies/1
```

Expected:

```text
200 OK
```

Should return:

```text
Inception
Producer
Actors
Genres
```

<br>

## Get another movie

```http
GET /api/movies/3
```

Expected:

```text
200 OK
```

Should return:

```text
Interstellar
```

<br>

# 8. Year Filter Tests

## Movies from 2010

```http
GET /api/movies?year=2010
```

Expected:

```text
200 OK
```

Expected movies:

```text
1 → Inception
4 → Dream Within
```

<br>

## Movies from 2008

```http
GET /api/movies?year=2008
```

Expected:

```text
200 OK

2 → The Dark Knight
```

<br>

## Movies from 2014

```http
GET /api/movies?year=2014
```

Expected:

```text
200 OK

3 → Interstellar
```

<br>

## Year with no movies

```http
GET /api/movies?year=2020
```

Expected:

```text
200 OK

[]
```

<br>

# 9. Negative GET Tests

## Non-existing Movie

```http
GET /api/movies/999
```

Expected:

```text
404 Not Found
```

or whatever status your assignment's validation/exception handling specifies.

<br>

# 10. Relationship Validation Tests

These are particularly important for testing your service architecture.

## Invalid Producer

Try creating a movie with:

```json
{
  "name": "Test Movie",
  "yearOfRelease": 2020,
  "plot": "Test plot",
  "coverImage": "https://example.com/test.jpg",
  "producerId": 999,
  "actorIds": [1],
  "genreIds": [1]
}
```

Expected:

```text
Producer 999 does not exist
```

<br>

## Invalid Actor

```json
{
  "name": "Test Movie",
  "yearOfRelease": 2020,
  "plot": "Test plot",
  "coverImage": "https://example.com/test.jpg",
  "producerId": 1,
  "actorIds": [1, 999],
  "genreIds": [1]
}
```

Expected:

```text
Invalid Actor Ids : 999
```

<br>

## Invalid Genre

```json
{
  "name": "Test Movie",
  "yearOfRelease": 2020,
  "plot": "Test plot",
  "coverImage": "https://example.com/test.jpg",
  "producerId": 1,
  "actorIds": [1],
  "genreIds": [1, 999]
}
```

Expected:

```text
Invalid Genre Ids : 999
```

<br>

# 11. Validation Tests

## Empty Movie Name

```json
{
  "name": "",
  "yearOfRelease": 2010,
  "plot": "Test plot",
  "coverImage": "https://example.com/test.jpg",
  "producerId": 1,
  "actorIds": [1],
  "genreIds": [1]
}
```

Expected:

```text
400 Bad Request
Movie Name is Empty.
```

<br>

## Invalid Year

```json
{
  "name": "Test Movie",
  "yearOfRelease": 0,
  "plot": "Test plot",
  "coverImage": "https://example.com/test.jpg",
  "producerId": 1,
  "actorIds": [1],
  "genreIds": [1]
}
```

Expected:

```text
400 Bad Request
```

<br>

# 12. Recommended Test Order

Run the tests in this order:

```text
1. Create Producer
       ↓
2. Create Actors
       ↓
3. Create Genres
       ↓
4. Create Movie 1
       ↓
5. Create Movie 2
       ↓
6. Create Movie 3
       ↓
7. Create Movie 4
       ↓
8. GET all Movies
       ↓
9. GET Movie by ID
       ↓
10. GET by Year
       ↓
11. Test invalid IDs
       ↓
12. Test invalid request data
       ↓
13. Test Update
       ↓
14. Test Delete
```

This gives you both **positive tests** and **negative/validation tests** while exercising the relationships:

```text
Movie
 ├── Producer → ProducerService
 ├── Actors   → ActorService
 └── Genres   → GenreService
```
