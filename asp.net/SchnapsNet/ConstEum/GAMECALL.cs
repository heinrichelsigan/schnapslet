using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchnapsNet.ConstEnum
{
    public enum GAMECALL
    {
        PLAYOUT             = 0,
        BEGAR               = 2,
        ACEBEGAR            = 4,
        SCHNAPSER           = 6,
        MARCH               = 8,
        TENHOLE             = 10,
        FARMERSCHNAPSER     = 12,
        SIRSCHNAPSER        = 24           
    }
    
    public static class GAMECALL_Extensions
    {
        public static int GetValue(this GAMECALL gameCall)
        {
            return ((int)gameCall);            
        }
    }
}