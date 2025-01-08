using System;

namespace CacheProject
{
    public class CacheItem
    {
        // המפתח המשמש לזיהוי פריט ב-Cache
        public string Key { get; set; }

        // הערך הנשמר ב-Cache
        public object Value { get; set; }

        // זמן התפוגה של הפריט
        public DateTime Expiration { get; set; }

        // פונקציה שבודקת האם פריט פג תוקף
        public bool IsExpired()
        {
            return DateTime.Now > Expiration;
        }
    }
}
