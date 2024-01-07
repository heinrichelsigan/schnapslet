using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchnapsNet.ConstEnum
{
    public static class Constants
    {
        public const string APPNAME     = "Schnaps.Net";
        public const string APPDIR      = "SchnapsNet";
        public const string VERSION     = "v2.11.33";
        public const string APPPATH     = "https://area23.at/mono/SchnapsNet";

        public const string GITURL      = "https://github.com/heinrichelsigan/schnapslet";
        public const string WIKIURL     = "https://github.com/heinrichelsigan/schnapslet/wiki";
        public const string URLPREFIX   = "https://area23.at/mono/SchnapsNet/cardpics/";
        public const string URLPIC      = "https://area23.at/mono/SchnapsNet/cardpics/";
        public const string URLXML      = "https://area23.at/mono/SchnapsNet/Properties/strings";

        public const string LOGDIR      = "log";
        public const string CARDPICSDIR = "cardpics";

        public const string MUTEX       = "SchnapsMutex";

        public const int PLAY_DOWN_FROM     = 7;    // play down from 7 points
        public const int PLAY_DOWN_FARMER   = 24;   // play down from 24 points in farmer game
        public const int PLAY_DOWN_MOCK     = 1;    // play down mock 1 points            

        public const string COLOR_K     = "🔶";  //  "&#9830;"; "&#128310;" //  "&diams;"🔶
        public const string COLOR_H     = "♥";  //  "&#9829;";  "&#129505;"  //  "&hearts;"
        public const string COLOR_P     = "♠";  //  "&#9824;";    //  "&spades;"
        public const string COLOR_T     = "♣";  //  "&#9827;";    //  "&clubs;"
        public const string COLOR_N     = "NOCOLOR";
        public const string COLOR_E     = "EMPTYCOLOR";
        public const string TAYLOR_SYM0 = "&#x2702;";       // ✂
        public const string TAYLOR_SYM1 = "&#9986;";
        public const string TAYLOR_SYM2 = "&#x2704;";       // ✄

        public const string CCARD       = "ccard";
        public const int ENOUGH         = 66;
        public const string OFDELIM     = " of ";


        // "http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml


    }
}