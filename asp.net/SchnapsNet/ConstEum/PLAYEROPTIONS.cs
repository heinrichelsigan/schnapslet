using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchnapsNet.ConstEnum
{
    public enum PLAYEROPTIONS
    {
        NONE        = 0,
        CHANGEATOU  = 1,
        CLOSESGAME  = 2,
        SAYPAIR     = 4,
        PLAYSCARD   = 8,
        HITSCARD    = 16,
        ANDENOUGH   = 32
    }
    
    public static class PLAYEROPTIONS_Extensions
    {
        public static int GetValue(this PLAYEROPTIONS playOpt)
        {
            switch(playOpt)
            {
                case PLAYEROPTIONS.CHANGEATOU: return 1;
                case PLAYEROPTIONS.CLOSESGAME: return 1;
                case PLAYEROPTIONS.SAYPAIR: return 4;
                case PLAYEROPTIONS.PLAYSCARD: return 8;
                case PLAYEROPTIONS.HITSCARD: return 16;
                case PLAYEROPTIONS.ANDENOUGH: return 32;

            }
            return 0;
        }
    }
}