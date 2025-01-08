using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cache = new Cache("C:\\Users\\User\\Desktop\\CacheProject\\CacheProject\\CacheJson\\cache.json");

            Console.WriteLine("Welcome to the Cache Program!");

            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Add to Cache");
                Console.WriteLine("2. Get from Cache");
                Console.WriteLine("3. Update Cache");
                Console.WriteLine("4. Remove from Cache");
                Console.WriteLine("5. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Add to cache
                        Console.Write("Enter key: ");
                        string key = Console.ReadLine();

                        Console.Write("Enter value: ");
                        string value = Console.ReadLine();

                        cache.Add(key, value); // Add with default TTL 60 seconds
                        break;

                    case "2": // Get from cache
                        Console.Write("Enter key: ");
                        string getKey = Console.ReadLine();

                        var item = cache.Get(getKey);
                        if (item != null)
                            Console.WriteLine($"Value: {item.Value}, Expires at: {item.Expiration}");
                        else
                            Console.WriteLine("Key not found or expired.");
                        break;

                    case "3": // Update cache
                        Console.Write("Enter key to update: ");
                        string updateKey = Console.ReadLine();

                        Console.Write("Enter new value: ");
                        string newValue = Console.ReadLine();

                        cache.Update(updateKey, newValue);
                        break;

                    case "4": // Remove from cache
                        Console.Write("Enter key to remove: ");
                        string removeKey = Console.ReadLine();

                        cache.Remove(removeKey);
                        break;

                    case "5": // Exit
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }
    }
}
