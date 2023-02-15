using SchnapsNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchnapsNet.ConstEnum
{
    public enum CARDVALUE
    {
        EMPTY   = -2,
        NONE    = -1,
        JACK    = 2,
        QUEEN   = 3,
        KING    = 4,
        TEN     = 10,
        ACE     = 11
    }
    
    public static class CARDVALUE_Extensions
    {
        public static int GetValue(this CARDVALUE cardVal)
        {
            switch(cardVal)
            {
                case CARDVALUE.EMPTY: return -2;
                case CARDVALUE.NONE: return -1;
                case CARDVALUE.JACK: return 2;
                case CARDVALUE.QUEEN: return 3;
                case CARDVALUE.KING: return 4;
                case CARDVALUE.TEN: return 10;
                case CARDVALUE.ACE: return 11;
            }
            return -2;
        }

        public static int CardValue(CARDVALUE cardVal)
        {
            switch (cardVal)
            {
                case CARDVALUE.EMPTY: return -2;
                case CARDVALUE.NONE: return -1;
                case CARDVALUE.JACK: return 2;
                case CARDVALUE.QUEEN: return 3;
                case CARDVALUE.KING: return 4;
                case CARDVALUE.TEN: return 10;
                case CARDVALUE.ACE: return 11;
            }
            return -2;
        }

    }
}