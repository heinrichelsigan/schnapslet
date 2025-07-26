using SchnapsNet.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SchnapsNet.Cache
{

    /// <summary>
    /// CacheHashDict an application cache implemented with a <see cref="ConcurrentDictionary{string, CacheValue}"/> saved in <see cref="HttpApplicationState"/>
    /// </summary>
    public class ApplicationStateCache : MemoryCache
    {

        protected internal static readonly object _smartLock = new object();
        

        /// <summary>
        /// public property get accessor for <see cref="_appDict"/> stored in <see cref="HttpApplicationState"/>
        /// </summary>
        protected override ConcurrentDictionary<string, CacheValue> AppDict
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Application != null && 
                    HttpContext.Current.Application[Constants.APP_CONCURRENT_DICT] != null)
                {
                    lock (_smartLock)
                    {
                        try
                        {
                            _appDict = (ConcurrentDictionary<string, CacheValue>)HttpContext.Current.Application[Constants.APP_CONCURRENT_DICT];
                        }
                        catch (Exception ex)
                        {
                            Area23.At.Framework.Library.Util.Area23Log.LogStatic(ex);
                        }
                    }
                }
                        
                
                if (_appDict == null)
                {
                    lock (_smartLock)
                    {
                        _appDict = new ConcurrentDictionary<string, CacheValue>();
                        HttpContext.Current.Application[Constants.APP_CONCURRENT_DICT] = _appDict;
                    }
                }

                return _appDict;
            }
            set
            {
                lock (_smartLock)
                {
                    if (value != null && value.Count > 0)
                    {
                        _appDict = value;
                        if (HttpContext.Current != null && HttpContext.Current.Application != null)
                            HttpContext.Current.Application[Constants.APP_CONCURRENT_DICT] = _appDict;
                    }
                }
            }
        }


        public ApplicationStateCache(PersistType cacheType = PersistType.ApplicationState) 
        {
                
        }

    }

}
