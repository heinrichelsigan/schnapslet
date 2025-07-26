using SchnapsNet.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchnapsNet.Cache
{

    [Serializable]
    public class CacheData
    {
        static readonly byte[] buffer = new byte[4096];
        static Random random = new Random((DateTime.Now.Millisecond + 1) * (DateTime.Now.Second + 1));
        [JsonIgnore]
        protected internal int CIndex { get; set; }
        public string CKey { get; set; }
        public string CValue { get; set; }
        public int CThreadId { get; set; }
        public DateTime CTime { get; set; }

        static CacheData()
        {
            random.NextBytes(buffer);
        }

        public CacheData()
        {
            CIndex = 0;
            CValue = string.Empty;
            CKey = string.Empty;
            CTime = DateTime.MinValue;
            CThreadId = -1;
        }

        public CacheData(string ckey) : this()
        {
            CKey = ckey;
            CIndex = Int32.Parse(ckey.Replace("Key_", ""));
            CValue = GetRandomString(CIndex);
            CTime = DateTime.Now;
        }

        public CacheData(string ckey, int cThreadId) : this(ckey)
        {
            CThreadId = cThreadId;
        }

        public CacheData(int cThreadId) : this(string.Concat("Key_", cThreadId))
        {
            CThreadId = cThreadId;
        }


        internal string GetRandomString(int ix)
        {
            byte[] restBytes = new byte[64];
            Array.Copy(buffer, ix, restBytes, 0, 64);
            return Convert.ToBase64String(restBytes, 0, 64);
        }

    }


}
