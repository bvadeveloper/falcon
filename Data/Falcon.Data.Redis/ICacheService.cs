using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Falcon.Data.Redis
{
    public interface ICacheService
    {

        /// <summary>
        /// Delete all the keys from the DB.
        /// </summary>
        /// <returns></returns>
        ICacheService Flush();

        /// <summary>
        /// Sets expiration time to key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ttl"></param>
        void SetTTL(string key, TimeSpan ttl);

        /// <summary>
        /// Set object value to cache
        /// </summary>
        void SetValue(string key, object value);

        /// <summary>
        /// Set string value to cache
        /// </summary>
        void SetValue(string key, string value);

        /// <summary>
        /// Set string value to cache with TTL
        /// </summary>
        void SetValue(string key, string value, TimeSpan ttl);

        /// <summary>
        /// Set object value to cache async
        /// </summary>
        Task SetValueAsync(string key, object value);

        /// <summary>
        /// Set string value to cache async
        /// </summary>
        Task SetValueAsync(string key, string value);

        /// <summary>
        /// Set object value to cache with lifetime
        /// </summary>
        void SetValue(string key, object value, TimeSpan ttl);

        /// <summary>
        /// Set object value to cache with lifetime async
        /// </summary>
        Task SetValueAsync(string key, object value, TimeSpan ttl);

        /// <summary>
        /// Set object value to cache if the specified key does not exist.
        /// </summary>
        /// <returns>True if the key was set.</returns>
        Task<bool> SetValueIfNotExistsAsync(string key, object value);

        /// <summary>
        /// Get value by key
        /// </summary>
        T GetValue<T>(string key);

        /// <summary>
        /// Get string value by key
        /// </summary>
       string GetValue(string key);

        /// <summary>
        /// Get value by key async
        /// </summary>
        Task<T> GetValueAsync<T>(string key);

        /// <summary>
        /// Get string value by key async
        /// </summary>
        Task<string> GetValueAsync(string key);

        /// <summary>
        /// Check if a particular key exists in the cache
        /// </summary>
        bool KeyExists(string key);

        /// <summary>
        /// Check if a particular key exists in the cache async
        /// </summary>
        Task<bool> KeyExistsAsync(string key);

        /// <summary>
        /// Delete value in cache by key
        /// </summary>
        void Delete(string key);

        /// <summary>
        /// Delete value in cache by key async
        /// </summary>
        Task DeleteAsync(string key);

        /// <summary>
        /// Delete values in cache by keys async
        /// </summary>
        Task<long> DeleteAsync(params string[] keys);

        /// <summary>
        /// Delete value in cache by RegEx expression
        /// </summary>
        void DeleteByPattern(string pattern);

        /// <summary>
        /// Delete value in cache by RegEx expression async
        /// </summary>
        Task DeleteByPatternAsync(string pattern);

        /// <summary>
        /// Set collection of values to cache
        /// </summary>
        void SetValues<T>(string key, IEnumerable<T> values) where T : class;

        /// <summary>
        /// Set collection of strings to cache
        /// </summary>
        void SetValues(string key, IEnumerable<string> values);

        /// <summary>
        /// Add the value to the set stored at key
        /// </summary>
        void AppendValue(string key, object value);

        /// <summary>
        /// Get collection of values by key
        /// </summary>
        ICollection<T> GetValues<T>(string key);

        /// <summary>
        /// Get collection of values by key
        /// </summary>
        Task<ICollection<T>> GetValuesAsync<T>(string key);

        /// <summary>
        /// Get collection of strings by key
        /// </summary>
        ICollection<string> GetValues(string key);

        /// <summary>
        /// Returns whether  cache contains hash value
        /// </summary>
        bool HashHasValue(string key, string hashKey);

        /// <summary>
        /// Returns whether  cache contains hash value
        /// </summary>
        Task<bool> HashHasValueAsync(string key, string hashKey);

        /// <summary>
        /// Get hash value
        /// </summary>
        string GetHashValue(string key, string hashKey);

        /// <summary>
        /// Get hash value
        /// </summary>
        Task<string> GetHashValueAsync(string key, string hashKey);

        /// <summary>
        /// Get hash value. Generic version
        /// </summary>
        T GetHashValue<T>(string key, string hashKey);

        /// <summary>
        /// Get hash value. Generic version
        /// </summary>
        Task<T> GetHashValueAsync<T>(string key, string hashKey);

        /// <summary>
        /// Get all hash
        /// </summary>
        Dictionary<string, T> GetAllHash<T>(string key);
        
        /// <summary>
        /// Get all hash
        /// </summary>
        Task<Dictionary<string, T>> GetAllHashAsync<T>(string key);

        /// <summary>
        /// Delete keys from hash
        /// </summary>
        Task DeleteHashValuesAsync(string key, IEnumerable<string> hashKeys);

        /// <summary>
        /// Set hash value
        /// </summary>
        bool SetHashValue(string key, string hashKey, string value);

        /// <summary>
        /// Set hash value
        /// </summary>
        Task<bool> SetHashValueAsync(string key, string hashKey, string value);

        /// <summary>
        /// Set hash value. Generic version
        /// </summary>
        bool SetHashValue(string key, string hashKey, object value);

        /// <summary>
        /// Set hash value. Generic version
        /// </summary>
        Task<bool> SetHashValueAsync(string key, string hashKey, object value);
    }
}
