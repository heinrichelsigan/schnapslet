using SchnapsNet.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchnapsNet.Cache
{

    /// <summary>
    /// JsonFileCache an application cache implemented with <see cref="ConcurrentDictionary{string, CacheValue}"/> serialized with json    
    /// </summary>
    public class JsonFileCache : MemoryCache
    {

        //protected internal static readonly Lazy<MemCache> _instance = new Lazy<MemCache>(() => new JsonCache());
        //public static MemCache CacheDict => _instance.Value;

        const int INIT_SEM_COUNT = 1;
        const int MAX_SEM_COUNT = 1;
        const string JSON_APPCACHE_FILE = "AppCache.json";
        readonly static string JsonFullDirPath = LibPaths.SystemDirJsonPath; // Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "TEMP");
        readonly static string JsonFullFilePath = Path.Combine(JsonFullDirPath, JSON_APPCACHE_FILE);

        protected static SemaphoreSlim ReadWriteSemaphore = new SemaphoreSlim(INIT_SEM_COUNT, MAX_SEM_COUNT);

        protected static JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            MaxDepth = 16,
            NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTime,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        /// <summary>
        /// public property get accessor for <see cref="_appDict"/> stored in <see cref="AppDomain.CurrentDomain"/>
        /// </summary>
        protected override ConcurrentDictionary<string, CacheValue> AppDict
        {
            get
            {
                int semCnt = 0;
                try
                {
                    ReadWriteSemaphore.Wait(64);
                    // if (mutex.WaitOne(250, false)) 
                    if (_appDict == null || _appDict.Count == 0)
                    {
                        lock (_lock)
                        {
                            if (!Directory.Exists(JsonFullDirPath))
                                Directory.CreateDirectory(JsonFullDirPath);

                            string jsonSerializedAppDict = (System.IO.File.Exists(JsonFullFilePath)) ? System.IO.File.ReadAllText(JsonFullFilePath) : "";
                            if (!string.IsNullOrEmpty(jsonSerializedAppDict))
                                _appDict = (ConcurrentDictionary<string, CacheValue>)JsonConvert.DeserializeObject<ConcurrentDictionary<string, CacheValue>>(jsonSerializedAppDict);
                        }
                        if (_appDict == null || _appDict.Count == 0)
                            _appDict = new ConcurrentDictionary<string, CacheValue>();
                    }
                }
                catch (Exception exGetRead)
                {
                    CqrException.SetLastException(exGetRead);
                    // Console.WriteLine($"Exception {exGetRead.GetType()}: {exGetRead.Message} \r\n\t{exGetRead}");
                }
                finally
                {
                    if (ReadWriteSemaphore.CurrentCount > 0)
                        semCnt = ReadWriteSemaphore.Release();
                    // mutex.ReleaseMutex();
                }
                return _appDict;
            }
            set
            {
                int semCnt = 0;
                try
                {
                    semCnt = ReadWriteSemaphore.CurrentCount;
                    ReadWriteSemaphore.Wait(64);

                    string jsonDeserializedAppDict = "";
                    if (value != null && value.Count > 0)
                    {
                        // if (mutex.WaitOne(250, false)) 
                        lock (_lock)
                        {
                            _appDict = value;

                            // set it, where to set it _appDict
                            jsonDeserializedAppDict = JsonConvert.SerializeObject(_appDict, Formatting.Indented, JsonSettings);
                            System.IO.File.WriteAllText(JsonFullFilePath, jsonDeserializedAppDict, Encoding.UTF8);
                        }
                    }
                }
                catch (Exception exSetWrite)
                {
                    CqrException.SetLastException(exSetWrite);
                    // Console.WriteLine($"Exception {exSetWrite.GetType()}: {exSetWrite.Message} \r\n\t{exSetWrite}");
                }
                finally
                {
                    if (ReadWriteSemaphore.CurrentCount > 0)
                        semCnt = ReadWriteSemaphore.Release();
                }
            }
        }


        public JsonFileCache(PersistType cacheType = PersistType.JsonFile) 
        {
            
        }

    }


}
