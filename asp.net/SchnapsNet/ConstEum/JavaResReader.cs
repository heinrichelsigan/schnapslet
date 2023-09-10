using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Xml;
using SchnapsNet;
using SchnapsNet.ConstEnum;
using SchnapsNet.Properties;

namespace SchnapsNet.ConstEnum
{
    public static class JavaResReader
    {
        public static string GetValueFromKey(string key, string langCode = "")
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