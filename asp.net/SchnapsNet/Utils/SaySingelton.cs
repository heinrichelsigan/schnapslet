using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Speech.AudioFormat;
using static System.Net.Mime.MediaTypeNames;
using System.Web;

namespace SchnapsNet.Utils
{
    /// <summary>
    /// SaýSpeach is an audio wav generating singelton <see cref="Lazy{T}"/>.
    /// </summary>
    public class SaySingelton
    {       
        private static string[] TMPS = { "temp", "TEMP", "tmp", "TMP" };
        protected internal static readonly Lazy<SaySingelton> lazySaySingleton = new Lazy<SaySingelton>(() => new SaySingelton());
        protected internal static SaySingelton saySingle;
        private static object outerLock, innerLock;
        internal SpeechSynthesizer synthesizer;
        private static string saveDir;
        protected internal string[] voices, femaleVoices;

        private static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        public static SaySingelton Instance { get => lazySaySingleton.Value; }

        public static SaySingelton Singleton
        {
            get
            {
                outerLock = new object();
                lock (outerLock)
                {
                    if (saySingle == null)
                    {
                        innerLock = new object();
                        lock (innerLock)
                        {
                            saySingle = new SaySingelton();
                            saySingle.synthesizer = new SpeechSynthesizer();
                            // saySingle.femaleVoices = saySingle.AddVoices(saySingle.voices);
                            // if (saySingle.femaleVoices != null && saySingle.femaleVoices.Length > 0)
                            //    saySingle.synthesizer.SelectVoice(saySingle.femaleVoices[0]);
                        }
                    }
                }
                return saySingle;
            }
        }

        internal static string AppPathUrl { get => HttpContext.Current.Request.ApplicationPath; }

        internal static string AudioDir 
        {
            get
            {
                string audioPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
                if (!audioPath.Contains("SchnapsNet"))
                    audioPath += "SchnapsNet" + SepChar;
                if (!audioPath.Contains("res"))
                    audioPath += "res" + SepChar;
                // if (!Directory.Exists(audioPath))
                return audioPath;
            }
        }

        /// <summary>
        /// SaveOutPaths returns a <see cref="string[]" /> containg possible save output paths.
        /// </summary>
        protected static string[] SaveOutPaths
        {
            get
            {
                List<string> savePathList = new System.Collections.Generic.List<string>();
                string tmpDir = Paths.AudioDir;
                if (tmpDir != null && Directory.Exists(tmpDir))
                    savePathList.Add(tmpDir);

                tmpDir = AppDomain.CurrentDomain.BaseDirectory;
                if (tmpDir != null && Directory.Exists(tmpDir))
                    savePathList.Add(tmpDir);

                foreach (string tmpVar in TMPS)
                {
                    tmpDir = Environment.GetEnvironmentVariable(tmpVar);
                    if (tmpDir != null && tmpDir.Length > 1 && Directory.Exists(tmpDir))
                        savePathList.Add(tmpDir);
                }

                return savePathList.ToArray();
            }
        }

        /// <summary>
        /// SavePath return path to save output directory.
        /// </summary>
        protected static internal String SavePath
        {
            get
            {
                if (saveDir != null && saveDir.Length > 1 && Directory.Exists(saveDir))
                    return saveDir;

                saveDir = AudioDir;
                if (Directory.Exists(saveDir))
                    return saveDir;

                foreach (string appPath in SaveOutPaths)
                {
                    saveDir = appPath;
                    try
                    {
                        if (!Directory.Exists(saveDir))
                        {
                            DirectoryInfo dirInfo = Directory.CreateDirectory(saveDir);
                            if (dirInfo == null || !dirInfo.Exists)
                                throw new IOException(saveDir + " doesn't exist!");
                        }
                    }
                    catch (Exception) { saveDir = appPath; }
                    try
                    {
                        string tmpFile = Path.Combine(saveDir, (DateTime.Now.Ticks + ".txt"));
                        File.WriteAllText(tmpFile, DateTime.Now.ToString());
                        if (File.Exists(tmpFile))
                        {
                            File.Delete(tmpFile);
                            return saveDir;
                        }
                    }
                    catch (Exception) { continue; }
                }

                saveDir = "." + SepChar;
                return saveDir;
            }
        }

        /// <summary>
        /// ctor protected internal of <see cref="SaySingelton"/>.
        /// </summary>
        protected internal SaySingelton()
        {
            synthesizer = new SpeechSynthesizer();
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
                    (((int)sayChar) >= ((int)'a') && ((int)sayChar) <= ((int)'z')))
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
            // if (!wavOutPath.Contains("SchnapsNet"))
            //     wavOutPath += "SchnapsNet" + "/";
            if (!wavOutPath.Contains("res"))
                wavOutPath += "res" + "/";
            wavOutPath += fileNameSay;

            return wavOutPath;
        }

        /// <summary>
        /// AddVoices 
        /// adds all containing synthesizer voices to <see cref="string[]"/>
        /// </summary>
        /// <param name="voiceArray">string array <see cref="string[]" />, where voice names are stored as string.</param>
        /// <returns>string array <see cref="string[]"/>, where voice names are stored as string.</returns>
        protected internal string[] AddVoices(string[] voiceArray)
        {
            if (synthesizer == null)
                synthesizer = new SpeechSynthesizer();

            List<string> myVoices = (voiceArray != null && voiceArray.Length > 0) ?
                new List<string>(voiceArray) : new List<string>();
            List<string> femaleVoices = new List<string>();

            foreach (var voice in synthesizer.GetInstalledVoices())
            {
                var info = voice.VoiceInfo;
                if (info != null)
                {
                    if (info.Gender == VoiceGender.Female)
                    {
                        if (myVoices.Contains(info.Name))
                            myVoices.Remove(info.Name);
                        femaleVoices.Add(info.Name);
                    }
                    else if (!myVoices.Contains(info.Name))
                        myVoices.Add(info.Name);
                }
            }
            femaleVoices.AddRange(myVoices.ToArray());
            voiceArray = myVoices.ToArray();

            return femaleVoices.ToArray(); 
        }

        /// <summary>
        /// Say speaks out a word to say.
        /// In that case synthesizer will be set with <see cref="SpeechSynthesizer.SetOutputToWaveFile(string)"/>.
        /// </summary>
        /// <param name="say">word to say as <see cref="string"/></param>
        /// <returns><see cref="Path"/> to recorded audio wav file</returns>
        public string Say(string say) 
        {
            // femaleVoices = AddVoices(voices);

            if (!string.IsNullOrWhiteSpace(say))
            {
                string fileNameSay = WaveFileName(say);

                if (synthesizer == null)
                {
                    synthesizer = new SpeechSynthesizer();
                }
                string wavOutPath = WaveFileUrl(say, AppPathUrl);
                using (synthesizer)
                {
                    // voices = AddVoices(voices);
                    // SpeechAudioFormatInfo sAudioFormatInfo = new SpeechAudioFormatInfo(1, 1, AudioChannel.Stereo);

                    string wavFile = SavePath + SepChar + fileNameSay;
                    synthesizer.SetOutputToWaveFile(wavFile);
                    synthesizer.Speak(say);                    
                }
                return wavOutPath;
            }
            return null;
        }

    }

}
