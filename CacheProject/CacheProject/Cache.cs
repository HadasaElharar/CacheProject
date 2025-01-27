using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Timers;

namespace CacheProject
{
    internal class Cache
    {
        private readonly ConcurrentDictionary<string, CacheItem> _cache = new ConcurrentDictionary<string, CacheItem>();

        private readonly string _filePath;

        private readonly Timer _cleanupTimer;

        public Cache(string filePath, double cleanupIntervalInSeconds = 60)
        {
            _filePath = filePath;

            // טוען נתונים קיימים מהקובץ (אם קיים)
            LoadFromFile();

            _cleanupTimer = new Timer(cleanupIntervalInSeconds * 1000); //ממיר שניות למילישניות
            _cleanupTimer.Elapsed += (sender, args) => RemoveExpiredItems();
            _cleanupTimer.Start();
        }
        public void Add(string key, object value, int ttlInMinutes = 5)
        {

            
            if (_cache.ContainsKey(key))
            {
                Console.WriteLine($"Key '{key}' already exists.");
                return;
            }
           

            var expiration = DateTime.Now.AddMinutes(ttlInMinutes);
            var item = new CacheItem
            {
                Key = key,
                Value = value,
                Expiration = expiration
            };

            if (_cache.TryAdd(key, item))
            {
                SaveToFile();
                Console.WriteLine($"Key '{key}' added successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to add key '{key}'.");
            }
        }
        public CacheItem Get(string key, object defaultValue = null)
        {
            if (_cache.TryGetValue(key, out var item))
            {
                if (item.IsExpired())
                {
                    _cache.TryRemove(key, out _);
                    SaveToFile();
                    Console.WriteLine($"Key '{key}' is expired and has been removed.");
                    return null;
                }

                return item;
            }
            else
            {
                // אם אין ערך בברירת המחדל, יש להוסיף אותו
                if (defaultValue != null)
                {
                    Add(key, defaultValue);
                    return _cache[key];
                }

                Console.WriteLine($"Key '{key}' not found.");
                return null;
            }
        }
        public void Update(string key, object newValue, int ttlInMinutes = 5)
        {
            // בודקים אם ה-key קיים במילון
            if (_cache.TryGetValue(key, out var item))
            {
                // מעדכנים את הערך ואת זמן התפוגה
                item.Value = newValue;
                item.Expiration = DateTime.Now.AddMinutes(ttlInMinutes);

                // מעדכנים את המילון
                _cache[key] = item;

                // שומרים את השינויים בקובץ
                SaveToFile();

                Console.WriteLine($"Key '{key}' has been updated successfully.");
            }
            else
            {
                Console.WriteLine($"Key '{key}' does not exist. No updates were made.");
            }
        }
        public void Remove(string key)
        {
            if (_cache.TryRemove(key, out _))
            {
                SaveToFile();
                Console.WriteLine($"Key '{key}' has been removed successfully.");
            }
            else
            {
                Console.WriteLine($"Key '{key}' was not found or could not be removed.");
            }
        }

        private void RemoveExpiredItems()
        {
            // מציאת כל המפתחות של פריטים שפג תוקפם
            var expiredKeys = _cache
            .Where(pair => pair.Value.IsExpired())
            .Select(pair => pair.Key)
            .ToList();

            foreach (var key in expiredKeys)
            {
                _cache.TryRemove(key, out _);
                Console.WriteLine($"Removed expired item: {key}");
            }

            if (expiredKeys.Any())
            {
                SaveToFile();
            }
        }
        public void SaveToFile()
        {
            var data = JsonSerializer.Serialize(_cache.Values, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, data);
        }
        public void Dispose()
        {
            _cleanupTimer?.Dispose();
        }

        public void LoadFromFile()
        {
            if (!File.Exists(_filePath))
                return;

            try
            {
                var data = File.ReadAllText(_filePath);

                // אם הקובץ ריק, נשתמש ברשימה ריקה
                if (string.IsNullOrWhiteSpace(data))
                {
                    data = "[]";
                }

                var items = JsonSerializer.Deserialize<List<CacheItem>>(data);

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        if (!item.IsExpired())
                        {
                            _cache[item.Key] = item;
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
            }
        }


    }
}
    
