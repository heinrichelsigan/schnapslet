using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SchnapsNet.Utils
{
    public static class ResReader
    {
        public static string GetValue(string key, string langCode = "")
        {
            string retVal = Properties.Resource.ResourceManager.GetString(key);

            if (langCode.ToLower() == "de")
            {
                string retVal_de = Properties.Resource_de.ResourceManager.GetString(key);
                if (!string.IsNullOrEmpty(retVal_de))
                {
                    return retVal_de;
                }
            }
            if (langCode.ToLower() == "fr")
            {
                string retVal_fr = Properties.Resource_fr.ResourceManager.GetString(key);
                if (!string.IsNullOrEmpty(retVal_fr))
                {
                    return retVal_fr;
                }
            }
            return (!string.IsNullOrEmpty(retVal)) ? retVal : key;
        }
    }
}