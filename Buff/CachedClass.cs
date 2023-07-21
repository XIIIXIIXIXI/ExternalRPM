using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalRPM.Buff
{
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
