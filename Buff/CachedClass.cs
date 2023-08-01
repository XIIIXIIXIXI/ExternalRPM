using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Buff
{
    /*
    Description:
    This class implements a caching mechanism to store and retrieve data efficiently by utilizing local and global caches. 
    It provides methods to set, get, and check the existence of cached data. The local cache is specific to each instance 
    of the class, while the global cache is shared among all instances of CachedClass.

    Local Cache Methods:
    - Clear(): Clears the local cache.
    - Get<T>(string key): Retrieves the cached value associated with the specified key from the local cache.
    - Has(string key): Checks if a value with the specified key exists in the local cache.
    - Set<T>(string key, T value): Sets the specified value in the local cache with the given key.
    - Use<T>(string key, System.Func<T> fn): Retrieves the cached value from the local cache for the specified key. 
      If the value is not found, it calculates the value using the provided function, caches it, and returns it.

    Global Cache Methods:
    - ClearGlobalCache(): Clears the global cache.
    - GetGlobal<T>(string key): Retrieves the cached value associated with the specified key from the global cache.
    - HasGlobal(string key): Checks if a value with the specified key exists in the global cache.
    - SetGlobal<T>(string key, T value): Sets the specified value in the global cache with the given key.
    - UseGlobal<T>(string key, System.Func<T> fn): Retrieves the cached value from the global cache for the specified key. 
      If the value is not found, it calculates the value using the provided function, caches it, and returns it.
*/
    public class CachedClass
    {
        private static readonly Dictionary<string, object> globalCache = new Dictionary<string, object>();
        private readonly Dictionary<string, object> localCache = new Dictionary<string, object>();

        public static bool Debug { get; set; } = false;

        protected void Clear()
        {
            localCache.Clear();
        }

        protected T Get<T>(string key)
        {
            return localCache.TryGetValue(key, out var value) ? (T)value : default;
        }

        protected bool Has(string key)
        {
            return localCache.ContainsKey(key);
        }

        protected void Set<T>(string key, T value)
        {
            localCache[key] = value;
        }

        protected T Use<T>(string key, System.Func<T> fn)
        {
            if (localCache.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            if (Debug)
            {
                System.Console.WriteLine("Calculating " + key);
            }

            var result = fn();
            localCache[key] = result;
            return result;
        }

        protected static void ClearGlobalCache()
        {
            globalCache.Clear();
        }

        protected static T GetGlobal<T>(string key)
        {
            return globalCache.TryGetValue(key, out var value) ? (T)value : default;
        }

        protected static bool HasGlobal(string key)
        {
            return globalCache.ContainsKey(key);
        }

        protected static void SetGlobal<T>(string key, T value)
        {
            globalCache[key] = value;
        }

        protected static T UseGlobal<T>(string key, System.Func<T> fn)
        {
            if (globalCache.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            var result = fn();
            globalCache[key] = result;
            return result;
        }
    }
}
