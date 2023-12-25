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
    public class SaySpeach
    {             
        internal SpeechSynthesizer synthesizer;
        private static string saveDir;
        protected internal string[] voices, femaleVoices;

        private static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        internal static string AppPathUrl { get => HttpContext.Current.Request.ApplicationPath; }

        internal string AudioDir 
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
        /// SavePath return path to save output directory.
        /// </summary>
        protected internal String SavePath
        {
            get
            {
                if (saveDir != null && saveDir.Length > 1 && Directory.Exists(saveDir))
                    return saveDir;

                saveDir = System.AppDomain.CurrentDomain.BaseDirectory + SepChar + "res" + SepChar;
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
        /// ctor protected internal of <see cref="SaySpeach"/>.
        /// </summary>
        public SaySpeach()
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
        public async Task Say(string say) 
        {
            // femaleVoices = AddVoices(voices);

            if (!string.IsNullOrWhiteSpace(say))
            {
                if (synthesizer == null)
                {
                    synthesizer = new SpeechSynthesizer();
                }
                
                using (synthesizer)
                {
                    // voices = AddVoices(voices);
                    // SpeechAudioFormatInfo sAudioFormatInfo = new SpeechAudioFormatInfo(1, 1, AudioChannel.Stereo);

                    string wavFile = SavePath + SepChar + WaveFileName(say); 
                    //try
                    //{
                    //    if (File.Exists(wavFile))
                    //        File.Delete(wavFile);
                    //} catch (Exception ex) { }
                    synthesizer.SetOutputToWaveFile(wavFile);
                    synthesizer.Speak(say);                    
                }                
            }            
        }

    }

}
