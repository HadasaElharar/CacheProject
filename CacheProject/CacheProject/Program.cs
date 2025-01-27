using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CacheProject
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to the Cache Program!");

            string cacheFilePath = InitializeCache();

            Console.WriteLine($"Cache file is located at: {cacheFilePath}");


            var cache = new Cache(cacheFilePath);

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Add to Cache");
                Console.WriteLine("2. Get from Cache");
                Console.WriteLine("3. Update Cache");
                Console.WriteLine("4. Remove from Cache");
                Console.WriteLine("5. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // הוספה לcache
                        Console.Write("Enter key: ");
                        string key = Console.ReadLine();

                        Console.Write("Enter value: ");
                        string value = Console.ReadLine();

                        cache.Add(key, value);
                        break;

                    case "2": // קבלה 
                        Console.Write("Enter key: ");
                        string getKey = Console.ReadLine();

                        var item = cache.Get(getKey);
                        if (item != null)
                            Console.WriteLine($"Value: {item.Value}, Expires at: {item.Expiration}");
                        else
                            Console.WriteLine("Key not found or expired.");
                        break;

                    case "3": // עדכון 
                        Console.Write("Enter key to update: ");
                        string updateKey = Console.ReadLine();

                        Console.Write("Enter new value: ");
                        string newValue = Console.ReadLine();

                        cache.Update(updateKey, newValue);
                        break;

                    case "4": // מחיקה
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
            cache.Dispose();
        }
        static string InitializeCache()
        {
            // קבלת נתיב לשולחן העבודה של המשתמש
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // יצירת נתיב לתיקייה ול-JSON
            string projectPath = Path.Combine(desktopPath, "CacheProject", "CacheJson");
            string cacheFilePath = Path.Combine(projectPath, "cache.json");

            // יצירת תיקיות אם הן אינן קיימות
            EnsureDirectoryExists(projectPath);

            // יצירת קובץ JSON אם אינו קיים
            EnsureFileExists(cacheFilePath);

            return cacheFilePath;
        }

        // פונקציה שמוודאת שהתיקייה קיימת
        static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine($"Created directory at: {path}");
            }
        }

        // פונקציה שמוודאת שהקובץ קיים
        static void EnsureFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
                Console.WriteLine($"Created cache file at: {filePath}");
            }
        }

    }

}
