using IMDBConsoleApp.Exceptions;
using IMDBConsoleApp.Helpers;
using IMDBConsoleApp.Models;
using IMDBConsoleApp.Models.Request;
using IMDBConsoleApp.Models.Response;
using IMDBConsoleApp.Services;
using IMDBConsoleApp.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IMDBConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IMDB Console App");

            IActorService actorService = new ActorService();
            IProducerService producerService = new ProducerService();
            IMovieService movieService = new MovieService(actorService, producerService);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. List Movies");
                Console.WriteLine("2. Add Actor");
                Console.WriteLine("3. Add Producer");
                Console.WriteLine("4. Add Movie");
                Console.WriteLine("5. Delete Movie");
                Console.WriteLine("0. Exit ");

                try
                {
                    int choice = ReadInt("Choose Option : ");
                    
                    switch (choice)
                    {
                        case 1:
                            ListMovies(movieService);
                            break;

                        case 2:
                            AddActor(actorService);
                            break;

                        case 3:
                            AddProducer(producerService);
                            break;

                        case 4:
                            AddMovie(movieService, actorService, producerService);
                            break;

                        case 5:
                            DeleteMovie(movieService);
                            break;

                        case 0:
                            return;

                        default:
                            Console.WriteLine("Invalid Option");
                            break;
                    }
                }
                catch (InvalidInputException E)
                {
                    Console.WriteLine($"Invalid Input Exception Error! : {E.Message}");
                }
                catch (ValidationException e)
                {
                    Console.WriteLine($"Invalid Data Exception Error! : {e.Message}");
                }
            }
        }

        // Services 
        public static void AddActor(IActorService actorService)
        {
            string actorName = ReadString("Enter actor name: ");
            var actorDob = ReadDate("Enter DOB (dd/mm/yyyy): ");

            actorService.Add(actorName, actorDob);

            Console.WriteLine("Actor Added Successfully.");
            Console.WriteLine("---------------------------------------");
        }

        public static void AddProducer(IProducerService producerService)
        {
            string producerName = ReadString("Enter Producer name: ");
            DateTime producerDob = ReadDate("Enter DOB (dd/mm/yyyy): ");

            producerService.Add(producerName, producerDob);

            Console.WriteLine("Producer Added Successfully.");
            Console.WriteLine("---------------------------------------");
        }

        public static void AddMovie(IMovieService movieService, IActorService actorService, IProducerService producerService)
        {
            // 1. Take Input
            string movieName = ReadString("Enter Movie Name: ");
            int yearOfRelease = ReadInt("Enter Year of Release: ");
            string plot = ReadString("Enter Movie Plot: ");

            var actors = actorService.Get();
            var producers = producerService.Get();

            if (!actors.Any() || !producers.Any())
            {
                Console.WriteLine("Please add atleast one actor and a producer.");
                return;
            }

            List<int> actorIds = ReadActorIds(actors);
            int producerId = ReadProducerId(producers);

            // 2. Add
            MovieRequest movieRequest = new MovieRequest
            {
                Name = movieName,
                YearOfRelease = yearOfRelease,
                Plot = plot,
                ActorIds = actorIds,
                ProducerId = producerId
            };

            movieService.Add(movieRequest);

            Console.WriteLine("Movie Added Successfully");
            Console.WriteLine("---------------------------------------");
        }

        public static void ListMovies(IMovieService movieService)
        {
            var movies = movieService.Get(); 

            if (!movies.Any())
            {
                Console.WriteLine("No Movies Available Currently!. \n");
                return;
            }

            var movieDetails = movies.Select(movieItem =>
                $"Movie Name : {movieItem.Name}\n" +
                $"Year : {movieItem.YearOfRelease}\n" +
                $"Plot : {movieItem.Plot}\n" +
                $"Actors : {string.Join(", ", movieItem.Actors.Select(a => a.Name))}\n" +
                $"Producer : {movieItem.Producer.Name}\n"+
                "---------------------------------------"
            );

            Console.WriteLine(string.Join("\n", movieDetails));
        }

        public static void DeleteMovie(IMovieService movieService)
        {
            // Menu
            ListMovies(movieService);
 
            int movieId = ReadInt("Enter movie id to Delete: ");
            Console.WriteLine($" Deleting Movie - {movieService.Get(movieId).Name}.");
            
            movieService.Delete(movieId);

            Console.WriteLine("Movie deleted successfully.");
        }

        // Helper 
        private static int ReadInt(string message)
        {
            Console.Write(message);

            if (!int.TryParse(Console.ReadLine()?.Trim(), out int value))
            {
                throw new InvalidInputException("Please enter a valid number.");
            }

            ValidationHelper.ValidatePositiveInt(value, "Value");

            return value;
        }

        private static string ReadString(string message)
        {
            Console.Write(message);
            string value = Console.ReadLine().Trim();

            ValidationHelper.ValidateString(value, "Name");

            return value;
        }

        private static DateTime ReadDate(string message)
        {
            Console.Write(message);

            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                throw new InvalidInputException("Invalid date format.");
            }

            ValidationHelper.ValidateDate(date, "Date");

            return date;
        }

        private static List<int> ReadActorIds(IEnumerable<Actor> actors)
        {
            // Show
            Console.WriteLine("Select Actors (Comma Separated IDs):");

            Console.WriteLine(
                string.Join("\n", actors.Select(a => $"{a.Id}. {a.Name}"))
            );

            // Read
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                throw new InvalidInputException("Please select at least one actor.");

            // Parse
            var actorIds = input
                .Split(',')
                .Select(id =>
                {
                    if (!int.TryParse(id.Trim(), out int actorId))
                        throw new InvalidInputException("Invalid Actor ID.");

                    return actorId;
                })
                .ToList();

            return actorIds;
        }

        private static int ReadProducerId(IEnumerable<Producer> producers)
        {
            Console.WriteLine("Select Producer:");

            Console.WriteLine(
                string.Join("\n", producers.Select(p => $"{p.Id}. {p.Name}"))
            );

            int producerId = ReadInt("Producer ID: ");

            return producerId;
        }
    }
}

