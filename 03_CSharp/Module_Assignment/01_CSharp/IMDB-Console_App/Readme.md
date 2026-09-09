
# IMDB Console App

A C# console application for managing movies, actors, and producers. It demonstrates object-oriented programming, layered architecture, interfaces, LINQ, validation, custom exceptions, and in-memory data storage.

## Features

- Add actors with name and date of birth.
- Add producers with name and date of birth.
- Add a movie with a title, release year, plot, one or more actors, and one producer.
- List movies with their resolved actor and producer details.
- Delete a movie by ID.
- Validate user input and business rules with friendly console error messages.

## Technology

- C#
- .NET Framework 4.7.2
- Console application
- In-memory `List<T>` repositories

## Application Structure

```text
Assignment4/
    └── IMDBConsoleApp
        ├── Program.cs                         # Console menu, input, and output
        |
        ├── Models
        │   ├── Person.cs                      # Abstract base class
        |   |
        │   ├── Entities
        │   │   ├── Actor.cs
        │   │   ├── Producer.cs
        │   │   └── Movie.cs
        |   |
        │   ├── Request
        │   │   └── MovieRequest.cs            # Input data for adding a movie
        |   |
        │   └── Response
        │       └── MovieResponse.cs           # Display-ready movie data
        |   
        ├── Repository
        │   ├── ActorRepository.cs
        │   ├── ProducerRepository.cs
        │   ├── MovieRepository.cs
        |   |
        │   └── Interface
        │       ├── IActorRepository.cs
        │       ├── IProducerRepository.cs
        │       └── IMovieRepository.cs
        |   
        ├── Services
        │   ├── ActorService.cs
        │   ├── ProducerService.cs
        │   ├── MovieService.cs
        |   |
        │   └── Interface
        │       ├── IActorService.cs
        │       ├── IProducerService.cs
        │       └── IMovieService.cs
        |   
        ├── Helpers
        │   ├── ValidationHelper.cs
        │   └── Validators
        │       └── PersonValidator.cs
        |   
        └── Exceptions
            ├── InvalidInputException.cs
            └── ValidationException.cs
```

## Architecture and Flow

The application follows a simple layered approach:

```text
User
  ↓
Program.cs (menu, input, output)
  ↓
Service layer (business rules, validation, mapping)
  ↓
Repository layer (add, retrieve, delete)
  ↓
In-memory List<T> data
```

- **Program** reads console input, calls services, and displays results.
- **Services** apply business rules. For example, `MovieService` verifies movie fields, checks duplicates, and validates referenced actor/producer IDs.
- **Repositories** store data in private lists and assign incremental IDs.
- **Models** define the application data.

## Domain Model

```text
Person (abstract)
├── Actor
└── Producer

Movie
├── ActorIds : List<int>
└── ProducerId : int
```

`Movie` stores actor and producer IDs. When movies are listed, `MovieService` resolves those IDs into full `Actor` and `Producer` objects and returns a `MovieResponse`.

## Example: Add Movie Flow

1. The user enters the movie name, year, plot, actor IDs, and producer ID.
2. `Program.cs` creates a `MovieRequest`.
3. `MovieService.Add` validates required fields and duplicate movies.
4. `ActorService` and `ProducerService` confirm that selected IDs exist.
5. `MovieRepository` assigns an ID and stores the movie in its list.

## Validation and Exceptions

The app uses two custom exceptions:

| Exception | Used for | Example |
|---|---|---|
| `InvalidInputException` | Input cannot be parsed | Entering `abc` where a number is expected |
| `ValidationException` | A business/data rule is not satisfied | Empty name, future DOB, duplicate movie, missing entity |

Exceptions propagate up to the `try`/`catch` in `Program.Main`, where the app prints an error and continues showing the menu.

```text
Service/helper throws exception
  → calling service methods stop
  → Program.Main catches it
  → error is displayed
  → menu loop continues
```

## Running the App

1. Open `IMDBConsoleApp/IMDBConsoleApp.sln` in Visual Studio.
2. Restore/build the solution.
3. Run the project with `F5` or `Ctrl+F5`.
4. Use the menu to manage actors, producers, and movies.

> Data is in memory only. All movies, actors, and producers are lost when the application exits.

## Possible Enhancements

- Persist data in SQL Server or another database.
- Add update operations for movies, actors, and producers.
- Use dependency injection for repositories.
- Add unit tests for services and validators.
- Add stricter validation for movie release year and duplicate actor selections.
- Display movie IDs in the movie-list output to simplify deletion.
