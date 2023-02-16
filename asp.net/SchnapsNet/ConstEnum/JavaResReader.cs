using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Xml;

namespace SchnapsNet.ConstEnum
{
    public static class JavaResReader
    {
        public static string GetValueFromKey(string key, string langCode = "")
        {
            string retVal = SchnapsNet.Properties.Resource.ResourceManager.GetString(key);
            if (langCode.ToLower() == "de")
            {
                string retVal_de = SchnapsNet.Properties.Resource_de.ResourceManager.GetString(key);
                if (!string.IsNullOrEmpty(retVal_de))
                {
                    return retVal_de;
                }
            }
            return (!string.IsNullOrEmpty(retVal)) ? retVal : key;
            /*
            string retAttr = null;
            string myUri = Constants.URLXML + "-" + langCode + ".xml";
            XmlReader xmlReader = XmlReader.Create(myUri);
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "string"))
                {
                    if (xmlReader.HasAttributes)
                    {
                        retAttr = xmlReader.GetAttribute(0).ToString();
                        if (!string.IsNullOrEmpty(retAttr) && retAttr == key)
                        {
                            retVal = xmlReader.Value;
                            if (!string.IsNullOrEmpty(retVal))
                                return retVal;
                        }
                    }
                }
            }

            */
        }

    }
}