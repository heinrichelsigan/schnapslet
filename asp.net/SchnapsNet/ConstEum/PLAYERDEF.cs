using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchnapsNet.ConstEnum
{
    public enum PLAYERDEF
    {
        UNKNOWN     = 0,
        HUMAN       = 1,
        COMPUTER    = 2
    }
    
    public static class PLAYERDEF_Extensions
    {
        public static int GetValue(this PLAYERDEF playOpt)
        {
            switch(playOpt)
            {                
                case PLAYERDEF.HUMAN: return 1;
                case PLAYERDEF.COMPUTER: return 2;
                case PLAYERDEF.UNKNOWN:
                default: return 0;
            }
        }
    }
}