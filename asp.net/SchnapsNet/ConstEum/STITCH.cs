using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace SchnapsNet.ConstEnum
{
    /// <summary>
    /// emum STITCH for showing stitches
    /// </summary>
    public enum STITCH
    {
        NONE        = -3,
        HIDDEN      = -2,
        COMPUTER    = -1,
        PLAYER      = 0
    }
    
    public static class STITCH_Extensions
    {
        public static int Value(this STITCH cardStitch)
        {
            switch(cardStitch)
            {
                case STITCH.NONE: return -3;
                case STITCH.HIDDEN: return -2;
                case STITCH.COMPUTER: return -1;
                case STITCH.PLAYER: return 0;                
            }
            return -3;
        }

        public static int GetInt(STITCH cardStitch)
        {
            switch (cardStitch)
            {
                case STITCH.NONE: return -3;
                case STITCH.HIDDEN: return -2;
                case STITCH.COMPUTER: return -1;
                case STITCH.PLAYER: return 0;
            }
            return -3;
        }
    }
}