using SchnapsNet.Utils;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SchnapsNet.Cache
{


    /// <summary>
    /// RedIstatic static AWS elastic valkey cache singelton connector
    /// </summary>
    public static class RediStatic
    {


        static ConnectionMultiplexer connMux;
        static ConfigurationOptions options;
        static string endpoint = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        static StackExchange.Redis.IDatabase db;


        private static HashSet<string> _allKeys = new HashSet<string>();
        public static string[] AllKeys { get => _allKeys.ToArray(); }

        public static StackExchange.Redis.IDatabase Db
        {
            get
            {
                if (db == null)
                    db = ConnMux.GetDatabase();
                return db;
            }
        }

        public static StackExchange.Redis.ConnectionMultiplexer ConnMux
        {
            get
            {
                if (connMux == null)
                {
                    if (options == null)
                        options = new ConfigurationOptions
                        {
                            EndPoints = { endpoint },
                            Ssl = true
                        };
                    connMux = ConnectionMultiplexer.Connect(options);
                }
                return connMux;
            }
        }


        /// <summary>
        /// static ctor for static class RedIstatic 
        /// </summary>
        static RediStatic()
        {
            endpoint = Constants.VALKEY_CACHE_HOST_PORT; // "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
            if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY] != null)
                endpoint = (string)ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY];
            options = new ConfigurationOptions
            {
                EndPoints = { endpoint },
                Ssl = true
            };
            if (connMux == null)
                connMux = ConnectionMultiplexer.Connect(options);
            if (db == null)
                db = connMux.GetDatabase();
        }



        /// <summary>
        /// GetString gets a string value by redis key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"><see cref="CommandFlags"/></param>
        /// <returns>(<see cref="string"/>) value for key redIsKey</returns>
        public static string GetString(string redIsKey, CommandFlags flags = CommandFlags.None)
        {
            string redIsString = Db.StringGet(redIsKey, flags);
            return redIsString;
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
        public static void SetString(string redIsKey, string redIsString, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            Db.StringSet(redIsKey, redIsString, expiry, keepTtl, when, flags);
            if (_allKeys == null || _allKeys.Count == 0)
            {
                var keys = GetKey<string[]>("AllKeys");
                if (keys != null && keys.Length > 0)
                    _allKeys = new HashSet<string>(keys);
            }            
            if (!_allKeys.Contains(redIsKey))
            {
                _allKeys.Add(redIsKey);
                string jsonVal = JsonConvert.SerializeObject(AllKeys);
                Db.StringSet("AllKeys", jsonVal, null, false, When.Always, CommandFlags.None);
            }
        }


        /// <summary>
        /// SetKey<typeparamref name="T"/> sets a genric type T with a referenced key
        /// </summary>
        /// <typeparam name="T">generic type or class</typeparam>
        /// <param name="redIsKey">key for cache</param>
        /// <param name="tValue">Generic value to set</param>
        /// <param name="expiry"></param>
        /// <param name="keepTtl"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        public static void SetKey<T>(string redIsKey, T tValue, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            string jsonVal = JsonConvert.SerializeObject(tValue);
            SetString(redIsKey, jsonVal, expiry, keepTtl, when, flags);
        }

        /// <summary>
        /// GetKey<typeparamref name="T"/> gets a generic class type T from redis cache with key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static T GetKey<T>(string redIsKey, CommandFlags flags = CommandFlags.None)
        {
            string jsonVal = Db.StringGet(redIsKey, flags);
            var tValue = JsonConvert.DeserializeObject<T>(jsonVal);

            return tValue;
        }

        /// <summary>
        /// DeleteKey delete entry referenced at key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"><see cref="CommandFlags.FireAndForget"/> as default</param>
        public static void DeleteKey(string redIsKey, CommandFlags flags = CommandFlags.FireAndForget)
        {
            Db.StringGetDelete(redIsKey, flags);
            if (_allKeys == null || _allKeys.Count == 0)
            {
                var keys = GetKey<string[]>("AllKeys");
                if (keys != null && keys.Length > 0)
                    _allKeys = new HashSet<string>(keys);
            }
            if (!_allKeys.Contains(redIsKey))
            {
                _allKeys.Add(redIsKey);
                string jsonVal = JsonConvert.SerializeObject(AllKeys);
                Db.StringSet("AllKeys", jsonVal, null, false, When.Always, CommandFlags.None);
            }
        }

    }

}