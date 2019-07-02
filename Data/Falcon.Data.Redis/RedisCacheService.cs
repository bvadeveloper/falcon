using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcon.Logging;
using StackExchange.Redis;
using Falcon.Utils.Serialization;

namespace Falcon.Data.Redis
{
    public class RedisCacheService : ICacheService
    {
        private const int SaddMaxValuesChunkSize = 32;
        private readonly IJsonLogger _logger;
        private readonly IDatabase _database;
        private readonly IServer _server;
        private readonly ISerializationService _serializationService;


        // Is increased comparing to the default value (of 10) to reduce Client/Server communication during keys scan
        // Scan returns the page regardles are there values or not.
        // Increasing this param would force redis to scan more entities before respond to client that current page is empty
        private const int ScanKeysPageSize = 10000;

        public RedisCacheService(
            IDatabase database,
            IServer server,
            IJsonLogger<RedisCacheService> logger,
            ISerializationService serializationService)
        {
            _database = database;
            _server = server;
            _logger = logger;
            _serializationService = serializationService;
        }

        public ICacheService Flush()
        {
            _database.Execute("FLUSHDB");
            return this;
        }

        public void SetTTL(string key, TimeSpan ttl)
        {
            _database.KeyExpire(key, ttl, CommandFlags.FireAndForget);
        }

        #region Set

        public void SetValue(string key, object value)
        {
            var stringValue = Serialize(value);
            _database.StringSet(key, stringValue, null, When.Always, CommandFlags.FireAndForget);
        }

        public void SetValue(string key, string value)
        {
            _database.StringSet(key, value, null, When.Always, CommandFlags.FireAndForget);
        }

        public void SetValue(string key, string value, TimeSpan ttl)
        {
            _database.StringSet(key, value, ttl, When.Always, CommandFlags.FireAndForget);
        }

        public void SetValues<T>(string key, IEnumerable<T> values) where T : class
        {
            values
                .Select((x, i) => new { x, i })
                .GroupBy(tup => tup.i / SaddMaxValuesChunkSize)
                .Select(gs => gs.Select(x => x.x).ToList())
                .ToList()
                .ForEach(xs =>
                {
                    var index = 0;
                    var redisValues = new RedisValue[xs.Count()];
                    foreach (var value in xs)
                    {
                        redisValues[index] = Serialize(value);
                        index += 1;
                    }

                    _database.SetAdd(key, redisValues, CommandFlags.FireAndForget);
                });
        }

        public void SetValues(string key, IEnumerable<string> values)
        {
            values
                .Select((x, i) => new { x, i })
                .GroupBy(tup => tup.i / SaddMaxValuesChunkSize)
                .Select(gs => gs.Select(x => x.x).ToList())
                .ToList()
                .ForEach(xs =>
                {
                    var index = 0;
                    var redisValues = new RedisValue[xs.Count()];
                    foreach (var value in xs)
                    {
                        redisValues[index] = value;
                        index += 1;
                    }

                    _database.SetAdd(key, redisValues, CommandFlags.FireAndForget);
                });
        }

        public Task SetValueAsync(string key, object value)
        {
            var stringValue = Serialize(value);
            return _database.StringSetAsync(key, stringValue, null, When.Always, CommandFlags.FireAndForget);
        }

        public Task SetValueAsync(string key, string value)
        {
            return _database.StringSetAsync(key, value, null, When.Always, CommandFlags.FireAndForget);
        }

        public void SetValue(string key, object value, TimeSpan ttl)
        {
            var stringValue = Serialize(value);
            _database.StringSet(key, stringValue, ttl, When.Always, CommandFlags.FireAndForget);
        }

        public Task SetValueAsync(string key, object value, TimeSpan ttl)
        {
            var stringValue = Serialize(value);
            return _database.StringSetAsync(key, stringValue, ttl, When.Always, CommandFlags.FireAndForget);
        }

        public async Task<bool> SetValueIfNotExistsAsync(string key, object value)
        {
            return 0 != (int) await _database.ExecuteAsync($"SETNX", key, Serialize(value));
        }

        #endregion

        #region Get

        public T GetValue<T>(string key)
        {
            var currentValue = _database.StringGet(key);
            if (string.IsNullOrWhiteSpace(currentValue))
            {
                return default(T);
            }

            return Parse<T>(currentValue);
        }

        public string GetValue(string key)
        {
            var currentValue = _database.StringGet(key);
            if (string.IsNullOrWhiteSpace(currentValue))
            {
                return null;
            }

            return currentValue.ToString();
        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            var currentValue = await _database.StringGetAsync(key);
            if (string.IsNullOrWhiteSpace(currentValue))
            {
                return default(T);
            }

            return Parse<T>(currentValue);
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public ICollection<T> GetValues<T>(string key)
        {
            var values = _database.SetMembers(key);

            return values == null ? null : Array.ConvertAll(values, x => Parse<T>(x));
        }

        public async Task<ICollection<T>> GetValuesAsync<T>(string key)
        {
            var values = await _database.SetMembersAsync(key);

            return values == null ? null : Array.ConvertAll(values, x => Parse<T>(x));
        }

        public ICollection<string> GetValues(string key)
        {
            var values = _database.SetMembers(key);

            return values == null ? null : Array.ConvertAll(values, x => x.ToString());
        }

        #endregion

        #region Key

        public bool KeyExists(string key)
        {
            return _database.KeyExists(key);
        }

        public Task<bool> KeyExistsAsync(string key)
        {
            return _database.KeyExistsAsync(key);
        }

        #endregion

        #region Delete

        public void Delete(string key)
        {
            _database.KeyDelete(key, CommandFlags.FireAndForget);
        }

        public Task DeleteAsync(string key)
        {
            return _database.KeyDeleteAsync(key, CommandFlags.FireAndForget);
        }

        public Task<long> DeleteAsync(params string[] keys)
        {
            return _database.KeyDeleteAsync(keys.Select(s => (RedisKey) s).ToArray(), CommandFlags.FireAndForget);
        }

        public void DeleteByPattern(string pattern)
        {
            var keys = _server.Keys(0, pattern, ScanKeysPageSize).ToArray();
            foreach (var redisKey in keys)
            {
                _database.KeyDelete(redisKey, CommandFlags.FireAndForget);
            }
        }

        public Task DeleteByPatternAsync(string pattern)
        {
            var keys = _server.Keys(0, pattern, ScanKeysPageSize).ToArray();
            foreach (var redisKey in keys)
            {
                _database.KeyDeleteAsync(redisKey, CommandFlags.FireAndForget);
            }

            return Task.FromResult(0);
        }

        #endregion

        #region Append

        public void AppendValue(string key, object value)
        {
            // TODO JsonConvert.DeserializeObject is not idempotent, ToString(x) == ToString(x) -> false, sometimes
            // TODO it looks like LPUSH is what we need here
            _database.SetAdd(key, Serialize(value), CommandFlags.FireAndForget);
        }

        #endregion

        #region Hash

        public bool HashHasValue(string key, string hashKey)
        {
            return _database.HashExists(key, hashKey);
        }

        public async Task<bool> HashHasValueAsync(string key, string hashKey)
        {
            return await _database.HashExistsAsync(key, hashKey);
        }

        public string GetHashValue(string key, string hashKey)
        {
            return _database.HashGet(key, hashKey);
        }

        public async Task<string> GetHashValueAsync(string key, string hashkey)
        {
            return await _database.HashGetAsync(key, hashkey);
        }

        public T GetHashValue<T>(string key, string hashKey)
        {
            var stringValue = _database.HashGet(key, hashKey);

            if (string.IsNullOrWhiteSpace(stringValue))
                return default(T);

            return Parse<T>(stringValue);
        }

        public async Task<T> GetHashValueAsync<T>(string key, string hashKey)
        {
            var stringValue = await _database.HashGetAsync(key, hashKey);

            if (string.IsNullOrWhiteSpace(stringValue))
                return default(T);

            return Parse<T>(stringValue);
        }

        public Dictionary<string, T> GetAllHash<T>(string key)
        {
            var hash = _database.HashGetAll(key);
            var entries = hash?.ToDictionary(e => e.Name.ToString(), e => Parse<T>(e.Value));
            return entries;
        }

        public async Task<Dictionary<string, T>> GetAllHashAsync<T>(string key)
        {
            var hash = await _database.HashGetAllAsync(key);
            var entries = hash?.ToDictionary(e => e.Name.ToString(), e => Parse<T>(e.Value));
            return entries;
        }

        public async Task DeleteHashValuesAsync(string key, IEnumerable<string> hashKeys)
        {
            await _database.HashDeleteAsync(key, hashKeys.Select(k => (RedisValue) k).ToArray());
        }

        public bool SetHashValue(string key, string hashKey, string value)
        {
            return _database.HashSet(key, hashKey, value, When.Always, CommandFlags.FireAndForget);
        }

        public async Task<bool> SetHashValueAsync(string key, string hashKey, string value)
        {
            return await _database.HashSetAsync(key, hashKey, value, When.Always, CommandFlags.FireAndForget);
        }

        public bool SetHashValue(string key, string hashKey, object value)
        {
            var stringValue = Serialize(value);
            return _database.HashSet(key, hashKey, stringValue, When.Always, CommandFlags.FireAndForget);
        }

        public async Task<bool> SetHashValueAsync(string key, string hashKey, object value)
        {
            var stringValue = Serialize(value);
            return await _database.HashSetAsync(key, hashKey, stringValue, When.Always, CommandFlags.FireAndForget);
        }

        #endregion

        #region Helper Methods

        private string Serialize(object value) => _serializationService.Serialize(value);

        private T Parse<T>(string value)
        {
            try
            {
                return _serializationService.Deserialize<T>(value);
            }
            catch (Exception e)
            {
                _logger.Warning("Can't deserialize object from cache.",
                    new Dictionary<string, object> { { nameof(value), value } }, e);
            }

            return default(T);
        }

        #endregion
    }
}