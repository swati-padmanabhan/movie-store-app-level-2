using System.Configuration;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text.Json;
using MovieStoreLevel2.exceptions;
using MovieStoreLevel2.models;

namespace MovieStoreLevel2
{
    internal class Program
    {
        static List<Movie> movies = new List<Movie>();
        static string path = ConfigurationManager.AppSettings["filePath"]!.ToString();

        static void Main(string[] args)
        {
            {
                DisplayMenu();
            }
            static void DisplayMenu()
            {
                movies = DeserializeMovies();
                while (true)
                {

                    Console.WriteLine("\n==========Welcome to Movie Store Application : Swati Padmanabhan==========");
                    Console.WriteLine("What do you wish to do?\n" +
                        "1. Add New Movie\n" +
                        "2. Display All Movies\n" +
                        "3. Find Movie By Id\n" +
                        "4. Remove Movie by Id\n" +
                        "5. Clear All Movies\n" +
                        "6. Exit");

                    
                    try
                    {
                        int choice = Convert.ToInt32(Console.ReadLine());
                        DoTask(choice);
                    }
                    catch(FormatException fe)
                    {
                        Console.WriteLine($"Please enter number only! - { fe.Message}");
                    }
                    catch(InvalidChoiceException ice)
                    {
                        Console.WriteLine(ice.Message);
                    }
                    catch (MovieStoreIsEmptyException mee)
                    {
                        Console.WriteLine(mee.Message);
                    }
                    catch (MovieNotFoundException mfe)
                    {
                        Console.WriteLine(mfe.Message);
                    }
                    catch (CapacityIsFullException cfe)
                    {
                        Console.WriteLine(cfe.Message);
                    }
                }
            }
            static void DoTask(int choice)
            {
                switch (choice)
                {
                    case 1:
                        AddNewMovie();
                        break;
                    case 2:
                        if (movies.Count == 0)
                            Console.WriteLine("Movie List is Empty");
                        else
                            movies.ForEach(movie => Console.WriteLine(movie));
                        break;
                    case 3:
                        Movie findMovie = FindMovieById();
                        if (findMovie != null)
                            Console.WriteLine(findMovie);
                        else
                            throw new MovieNotFoundException("Movie Not Found");
                        break;
                    case 4:
                        RemoveMovie();
                        break;
                    case 5:
                        if (movies.Count == 0)
                            throw new MovieStoreIsEmptyException("Movie List is already Empty !");
                        else
                            movies.Clear();
                        Console.WriteLine("Movies Cleared Successfully !");
                        break;
                    case 6:
                        SerializeMovies();
                        Environment.Exit(0);
                        break;
                    default:
                        throw new InvalidChoiceException("Please Enter Valid Input !");
                        break;

                }
            }
            static void AddNewMovie()
            {
                if (movies.Count >= 5)
                {
                    throw new CapacityIsFullException("Insufficient space, only 5 movies allowed.");
                    return;
                }

                try
                {
                    Console.WriteLine("Enter Id: ");
                    int id = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Enter Name of the Movie: ");
                    string name = Console.ReadLine();

                    Console.WriteLine("Enter the Genre of the Movie: ");
                    string genre = Console.ReadLine();

                    Console.WriteLine("Enter Year of Release of the Movie in (DD/MM/YYYY): ");
                    DateTime yearOfRelease = Convert.ToDateTime(Console.ReadLine());

                    Movie newMovie = Movie.CreateMovie(id, name, genre, yearOfRelease);
                    movies.Add(newMovie);
                    Console.WriteLine("New Movie Added Successfully !");
                }
                catch(FormatException fe) {
                    Console.WriteLine(fe.Message);
                }
            }

            static Movie FindMovieById()
            {
                Movie findMovie = null;
                try
                {  
                    Console.WriteLine("Enter Id: ");
                    int id = Convert.ToInt32(Console.ReadLine());
                    findMovie = movies.Where(item => item.Id == id).FirstOrDefault();
                }
                catch(FormatException fe)
                {
                    Console.WriteLine($"Invalid Id Format: {fe.Message}");
                }
                return findMovie;
            }

            static void RemoveMovie()
            {
                Movie findMovie = FindMovieById();
                if (findMovie == null)
                    throw new MovieNotFoundException("Movie Not Found");
                else
                {
                    movies.Remove(findMovie);
                    Console.WriteLine("Movie Deleted Successfully !");
                }
            }
            static void SerializeMovies()
            {
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    sw.WriteLine(JsonSerializer.Serialize(movies));
                }
            }
            static List<Movie> DeserializeMovies()
            {
                if (!File.Exists(path))
                    return new List<Movie>();
                using (StreamReader sr = new StreamReader(path))
                {
                    List<Movie> accounts = JsonSerializer.Deserialize<List<Movie>>(sr.ReadToEnd());
                    
                }
                return movies;
            }


        }
    }
}
