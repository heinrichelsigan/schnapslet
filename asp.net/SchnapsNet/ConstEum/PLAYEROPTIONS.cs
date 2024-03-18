using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchnapsNet.ConstEnum
{
    [Serializable]
    public enum PLAYEROPTIONS
    {
        NONE        = 0,
        CHANGEATOU  = 1,
        SAYPAIR     = 2,        
        PLAYSCARD   = 4,
        HITSCARD    = 8,        
        CLOSESGAME  = 16,
        ANDENOUGH   = 32,
        LASTSTITCH  = 64
    }
    
    public static class PLAYEROPTIONS_Extensions
    {
        public static int GetValue(this PLAYEROPTIONS playOpt)
        {
            switch(playOpt)
            {
                case PLAYEROPTIONS.CHANGEATOU:  return 1;
                case PLAYEROPTIONS.SAYPAIR:     return 2;
                case PLAYEROPTIONS.PLAYSCARD:   return 4;
                case PLAYEROPTIONS.HITSCARD:    return 8;
                case PLAYEROPTIONS.CLOSESGAME:  return 16;
                case PLAYEROPTIONS.ANDENOUGH:   return 32;
                case PLAYEROPTIONS.LASTSTITCH:  return 64;
            }
            return 0;
        }
    }
}