using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchnapsNet.ConstEnum
{
    public enum CARDCOLOR
    {
        EMPTY       = 'e',
        NONE        = 'n',
        HEARTS      = 'h',
        SPADES      = 'p',
        DIAMONDS    = 'k',
        CLUBS       = 't'
    }
    
    public static class CARDCOLOR_Extensions
    {
        public static char GetChar(this CARDCOLOR cardColor)
        {
            switch(cardColor)
            {
                case CARDCOLOR.EMPTY: return 'e'; 
                case CARDCOLOR.NONE: return 'n';
                case CARDCOLOR.HEARTS: return 'h';
                case CARDCOLOR.SPADES: return 't';
                case CARDCOLOR.DIAMONDS: return 'k';
                case CARDCOLOR.CLUBS: return 'p';
            }
            return 'e';
        }

        public static char ColorChar(CARDCOLOR cardColor)
        {
            switch (cardColor)
            {
                case CARDCOLOR.EMPTY: return 'e';
                case CARDCOLOR.NONE: return 'n';
                case CARDCOLOR.HEARTS: return 'h';
                case CARDCOLOR.SPADES: return 't';
                case CARDCOLOR.DIAMONDS: return 'k';
                case CARDCOLOR.CLUBS: return 'p';
            }
            return 'e';
        }
    }
}