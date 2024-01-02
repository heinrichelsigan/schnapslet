using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace SchnapsNet.Utils
{
    /// <summary>
    /// SaýSpeach is an audio wav generating singelton <see cref="Lazy{T}"/>.
    /// </summary>
    public class SayBase
    {       
        internal static string saveDir;
        protected internal string[] voices, femaleVoices;

        internal static string SpeechSets
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["AppSpeech"] != null)
                {
                    switch (System.Configuration.ConfigurationManager.AppSettings["AppSpeech"].ToLower())
                    {
                        case "false":
                        case "0":
                            return "false";
                        case "true":
                        case "1":
                            return "true";
                        case "cache":
                        case "usecache":
                            return "usecache";
                        default:
                            return "cacheonly";
                    }
                }
                return "false";
            }
        }

        internal static Boolean SpeechOut { get => (SpeechSets != "false"); }

        internal static Boolean SpeechNew { get => (SpeechSets == "true" || SpeechSets == "usecache"); }

        internal static Boolean SpeechCache { get => (SpeechSets == "cacheonly" || SpeechSets == "usecache"); }

        protected internal static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        internal static string AppPathUrl { get => HttpContext.Current.Request.ApplicationPath; }

        internal string AudioDir
        {
            get
            {
                string audioPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
                if (!audioPath.Contains(Paths.AppFolder))
                    audioPath += Paths.AppFolder + SepChar;
                if (!audioPath.Contains("res"))
                    audioPath += "res" + SepChar;
                // if (!Directory.Exists(audioPath))
                return audioPath;
            }
        }

        internal String SavePath
        {
            get
            {
                if (saveDir != null && saveDir.Length > 1 && Directory.Exists(saveDir))
                    return saveDir;

                saveDir = System.AppDomain.CurrentDomain.BaseDirectory + SepChar + "res" + SepChar + "wav" + SepChar;
                if (Directory.Exists(saveDir))
                    return saveDir;

                saveDir = AudioDir;
                if (Directory.Exists(saveDir))
                    return saveDir;


                saveDir = "." + SepChar;
                return saveDir;
            }
        }

        /// <summary>
        /// ctor protected internal of <see cref="SayBase"/>.
        /// </summary>
        protected internal SayBase()
        {
            // synthesizer = new SpeechSynthesizer();
            saveDir = SavePath;
            voices = (voices == null || voices.Length == 0) ? new string[0] : voices;            
        }


        /// <summary>
        /// WaveFileName
        /// </summary>
        /// <param name="sayText">text to say</param>
        /// <returns>wave file name with .wav extension</returns>
        protected internal string WaveFileName(string sayText)
        {
            string sayWaveFile = string.Empty;
            foreach (char sayChar in sayText.ToCharArray())
            {
                if ((((int)sayChar) >= ((int)'A') && ((int)sayChar) <= ((int)'Z')) ||
                    (((int)sayChar) >= ((int)'a') && ((int)sayChar) <= ((int)'z')) ||
                    (((int)sayChar) >= ((int)'0') && ((int)sayChar) <= ((int)'9')))
                    sayWaveFile += sayChar;
            }
            if (string.IsNullOrEmpty(sayWaveFile))
                sayWaveFile = DateTime.Now.Ticks.ToString();
            return (sayWaveFile + ".wav");
        }

        protected internal string WaveFileUrl(string sayText, string rawUrl)
        {
            string fileNameSay = WaveFileName(sayText);
            // TODO: change in production
            string wavOutPath = rawUrl.Replace("SchnapsenNet.aspx", "").Replace("SchnapsNet.aspx", "");
            // TODO: change in production
            // if (!wavOutPath.Contains(Paths.AppFolder))
            //     wavOutPath += Paths.AppFolder + "/";
            if (!wavOutPath.Contains("res"))
                wavOutPath += "res" + "/";
            if (!wavOutPath.Contains("wav"))
                wavOutPath += "wav" + "/";
            wavOutPath += fileNameSay;

            return wavOutPath;
        } 

    }

}
