using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SchnapsNet.Utils
{
    public static class ResReader
    {
        /// <summary>
        /// GetValue gets string resource form language specific resource file 
        /// </summary>
        /// <param name="key">unique key (culture independent) to address resource string</param>
        /// <param name="langCode">two letter long iso language code</param>
        /// <returns>string in local language fetched from resource file</returns>
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


        /// <summary>
        /// GetRes gets string resource form language specific resource file 
        /// </summary>
        /// <param name="key">unique key (culture independent) to address resource string</param>
        /// <param name="ci">CultureInfo for currently used language</param>
        /// <returns>string in local language fetched from resource file</returns>
        public static string GetRes(string key, CultureInfo ci)
        {
            string lang2IsoToLower = (ci != null) ? ci.TwoLetterISOLanguageName.ToLower() : string.Empty;
            string retVal = Properties.Resource.ResourceManager.GetString(key);

            switch (lang2IsoToLower)
            {
                case "de":
                    string retValDe = Properties.Resource_de.ResourceManager.GetString(key);
                    if (!string.IsNullOrEmpty(retValDe) && retValDe.Length > 0)
                        return retValDe;
                    break;
                case "fr":
                    string retValFr = Properties.Resource_de.ResourceManager.GetString(key);
                    if (!string.IsNullOrEmpty(retValFr) && retValFr.Length > 0)
                        return retValFr;
                    break;
                case "en":
                    string retValLang = Properties.Resource_en.ResourceManager.GetString(key);
                    if (!string.IsNullOrEmpty(retValLang) && retValLang.Length > 0)
                        retVal = retValLang;
                    break;
                default:
                    break;
            }

            return (!string.IsNullOrEmpty(retVal)) ? retVal : key.Replace("_", " ");
        }

        /// <summary>
        /// GetStringFormated gets a formated string from language specific resource file
        /// </summary>
        /// <param name="key">unique key (culture independent) to address resource string</param>
        /// <param name="ci">CultureInfo for currently used language</param>
        /// <param name="args">object[] arguments needed for <see cref="String.Format(string, object[])"/></param>
        /// <returns>string in local language fetched from resource file</returns>
        public static string GetStringFormated(string key, CultureInfo ci, params object[] args)
        {
            string lang2IsoToLower = (ci != null) ? ci.TwoLetterISOLanguageName.ToLower() : string.Empty;
            string retVal = Properties.Resource.ResourceManager.GetString(key);
            string retValLang = retVal;

            switch (lang2IsoToLower)
            {
                case "de":
                    if ((retValLang = Properties.Resource_de.ResourceManager.GetString(key)) != null && retValLang.Length > 0)
                        retVal = retValLang;
                    break;
                case "fr":
                    if ((retValLang = Properties.Resource_fr.ResourceManager.GetString(key)) != null && retValLang.Length > 0)
                        retVal = retValLang;
                    break;
                case "en":
                default:
                    if ((retValLang = Properties.Resource_en.ResourceManager.GetString(key)) != null && retValLang.Length > 0)
                        retVal = retValLang;
                    break;
            }

            if (!string.IsNullOrEmpty(retVal))
            {
                if (args != null && args.Length > 0 &&
                    retValLang.Contains("{") && retValLang.Contains("}") &&
                    (retValLang.Contains("{0}") || retValLang.Contains("{1}") || retValLang.Contains("{2}")))
                {
                    retVal = String.Format(retValLang, args);
                }
                return retVal;
            }

            return key.Replace("_", " ");
        }
    }
}