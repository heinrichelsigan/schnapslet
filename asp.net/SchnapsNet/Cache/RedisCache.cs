using SchnapsNet.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace SchnapsNet.Cache
{

    /// <summary>
    /// RedisCache AWS elastic valkey cache singelton connector
    /// </summary>
    public class RedisCache : MemoryCache
    {

        const string VALKEY_CACHE_HOST_PORT = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        const string VALKEY_CACHE_APP_KEY = "RedisValkeyCache";
        const string ALL_KEYS = "AllKeys";
        protected internal static object _redIsLock = new object();

        ConnectionMultiplexer connMux;
        ConfigurationOptions options;
        string endpoint = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        StackExchange.Redis.IDatabase db;

        public static MemoryCache ValKey => _instance.Value;

        private static HashSet<string> _allKeys = new HashSet<string>();
        public override string[] AllKeys { get => GetAllKeys().ToArray(); }

        private static string _endPoint = VALKEY_CACHE_HOST_PORT;
        public static string EndPoint
        {
            get
            {
                if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings[VALKEY_CACHE_APP_KEY] != null)
                    _endPoint = (string)ConfigurationManager.AppSettings[VALKEY_CACHE_APP_KEY];
                if (string.IsNullOrEmpty(_endPoint))
                    _endPoint = VALKEY_CACHE_HOST_PORT; // back to default
                return _endPoint;
            }
        }

        public static StackExchange.Redis.IDatabase Db
        {
            get
            {
                if (((RedisCache)(_instance.Value)).db == null)
                    ((RedisCache)(_instance.Value)).db = ConnMux.GetDatabase();

                return ((RedisCache)(_instance.Value)).db;
            }
        }

        public static StackExchange.Redis.ConnectionMultiplexer ConnMux
        {
            get
            {
                if (((RedisCache)(_instance.Value)).connMux == null)
                {
                    if (((RedisCache)(_instance.Value)).options == null)
                        ((RedisCache)(_instance.Value)).options = new ConfigurationOptions
                        {
                            EndPoints = { EndPoint },
                            Ssl = true
                        };
                    ((RedisCache)(_instance.Value)).connMux = ConnectionMultiplexer.Connect(((RedisCache)(_instance.Value)).options);
                }
                return ((RedisCache)(_instance.Value)).connMux;
            }
        }


        /// <summary>
        /// default parameterless constructor for RedisCacheValKey cache singleton
        /// </summary>
        public RedisCache(PersistType cacheType = PersistType.Redis) 
        {
            endpoint = VALKEY_CACHE_HOST_PORT; // "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
            if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings[VALKEY_CACHE_APP_KEY] != null)
                endpoint = (string)ConfigurationManager.AppSettings[VALKEY_CACHE_APP_KEY];
            options = new ConfigurationOptions
            {
                EndPoints = { endpoint },
                AbortOnConnectFail = false,
                Ssl = true,
                ConnectTimeout = 6000,
                AsyncTimeout = 6000,
                SyncTimeout = 9000
            };
            // if (connMux == null)
                connMux = ConnectionMultiplexer.Connect(options);
            if (db == null)
                db = connMux.GetDatabase();
        }


        /// <summary>
        /// GetString gets a string value by RedisCache key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"><see cref="CommandFlags"/></param>
        /// <returns>(<see cref="string"/>) value for key redIsKey</returns>
        public string GetString(string redIsKey, CommandFlags flags = CommandFlags.None)
        {
            return Db.StringGet(redIsKey, flags);
        }

        /// <summary>
        /// SetString set key with string value
        /// </summary>
        /// <param name="redIsKey">key for string/param>
        /// <param name="redIsString"></param>
        /// <param name="expiry"></param>
        /// <param name="keepTtl"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        public bool SetString(string redIsKey, string redIsString, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            bool success = false;
            lock (_redIsLock)
            {
                var allRedIsKeys = GetAllKeys();
                success = Db.StringSet(redIsKey, redIsString, expiry, when, flags);

                if (success && !allRedIsKeys.Contains(redIsKey))
                {
                    allRedIsKeys.Add(redIsKey);
                    string jsonVal = JsonConvert.SerializeObject(AllKeys);
                    success = Db.StringSet(ALL_KEYS, jsonVal, null, keepTtl, When.Always, CommandFlags.None);
                    _allKeys = allRedIsKeys;
                }
            }

            return success;
        }

        /// <summary>
        /// SetValue sets value to cache
        /// </summary>
        /// <typeparam name="T">typeparameter</typeparam>
        /// <param name="ckey">key to set</param>
        /// <param name="tvalue">generic value</param>
        /// <returns>success on true</returns>
        public override bool SetValue<T>(string ckey, T tvalue)
        {
            TimeSpan? expiry = new TimeSpan(1, 1, 1, 1);
            bool keepTtl = false;
            When when = When.Always;
            CommandFlags flags = CommandFlags.None;
            string jsonVal = JsonConvert.SerializeObject(tvalue);
            bool success = SetString(ckey, jsonVal, expiry, keepTtl, when, flags);

            return success;
        }

        /// <summary>
        /// gets a generic class type T from redis cache with key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ckey">rediskey</param>
        /// <returns>T value/returns>
        public override T GetValue<T>(string ckey)
        {
            CommandFlags flags = CommandFlags.None;
            string jsonVal = Db.StringGet(ckey, flags);
            T tval = default(T);
            if (jsonVal != null)
            {
                tval = JsonConvert.DeserializeObject<T>(jsonVal);
            }

            return tval;
        }

        /// <summary>
        /// DeleteKey delete entry referenced at key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"><see cref="CommandFlags.FireAndForget"/> as default</param>
        public override bool RemoveKey(string redIsKey)
        {
            CommandFlags flags = CommandFlags.FireAndForget;
            lock (_redIsLock)
            {
                var allRedIsKeys = GetAllKeys();
                if (allRedIsKeys.Contains(redIsKey))
                {
                    allRedIsKeys.Remove(redIsKey);
                    string jsonVal = JsonConvert.SerializeObject(allRedIsKeys.ToArray());
                    Db.StringSet("AllKeys", jsonVal, null, false, When.Always, flags);
                    _allKeys = allRedIsKeys;
                }
                try
                {
                    TimeSpan span = new TimeSpan(0, 0, 1);
                    Db.StringGetDelete(redIsKey, flags);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exception {ex.GetType()}: {ex.Message}\r\n\t{ex}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// ContainsKey check if <see cref="Constants.ALL_KEYS">AllKeys</see> key contains element redIsKey
        /// </summary>
        /// <param name="ckey">redIsKey to search</param>
        /// <returns>true, if cache contains key, otherwise false</returns>
        public override bool ContainsKey(string ckey)
        {
            if (GetAllKeys().Contains(ckey))
            {
                string redIsString = Db.StringGet(ckey, CommandFlags.None);
                if (!string.IsNullOrEmpty(redIsString))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// GetAllKeys returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/>
        /// </summary>
        /// <returns>returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/></returns>
        public static HashSet<string> GetAllKeys()
        {
            if (_allKeys == null || _allKeys.Count == 0)
            {
                string jsonVal = Db.StringGet(ALL_KEYS, CommandFlags.None);
                string[] keys = (jsonVal != null) ? JsonConvert.DeserializeObject<string[]>(jsonVal) : new string[0];
                if (keys != null && keys.Length > 0)
                    _allKeys = new HashSet<string>(keys);
            }

            return _allKeys;
        }

    }

}