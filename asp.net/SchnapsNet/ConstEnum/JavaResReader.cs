using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace SchnapsNet.ConstEnum
{
    public static class JavaResReader
    {
        public static string GetValueFromKey(string key, string langCode = "")
        {
            string retVal = null, retAttr = null;
            XmlReader xmlReader = XmlReader.Create(Constants.URLXML + langCode + ".xml");
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == key))
                {
                    if (xmlReader.HasAttributes)
                        retAttr = xmlReader.GetAttribute(key);
                    if (!string.IsNullOrEmpty(retAttr))
                        return retAttr;
                    retVal = xmlReader.GetValueAsync().ConfigureAwait(true).ToString();                    
                    if (!string.IsNullOrEmpty(retVal)) 
                        return retVal;
                }
            }
            
            return (!string.IsNullOrEmpty(retAttr)) ? retAttr : retVal;
        }

    }
}