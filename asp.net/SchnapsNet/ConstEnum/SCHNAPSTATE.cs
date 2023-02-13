using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchnapsNet.ConstEnum
{
    public enum SCHNAPSTATE 
	{

		GAME_START          = 0,
		GAME_STARTED        = 1,
		COLOR_HIT_RULE      = 2,
		GAME_CLOSED         = 3,
		TALON_ONE_REMAINS   = 5,
		TALON_CONSUMED      = 7,
		GAME_STOP           = 8,
		MERGING_CARDS       = 16,
		MERGE_PLAYER        = 32,
		MERGE_COMPUTER      = 64,
		STARTS_SPLIT_TALON  = 256,
		STARTS_FIST_TALON   = 512,
		NONE                = 1023
		
    }
    
    public static class SCHNAPSTATE_Extensions
    {
        public static int GetValue(this SCHNAPSTATE schnapState)
        {
            switch(schnapState)
            {
                case SCHNAPSTATE.GAME_START: return 0;
                case SCHNAPSTATE.GAME_STARTED: return 1;
                case SCHNAPSTATE.COLOR_HIT_RULE: return 2;
                case SCHNAPSTATE.GAME_CLOSED: return 3;
                case SCHNAPSTATE.TALON_ONE_REMAINS: return 5;
                case SCHNAPSTATE.TALON_CONSUMED: return 7;
				case SCHNAPSTATE.GAME_STOP: return 8;
                case SCHNAPSTATE.MERGING_CARDS: return 16;
                case SCHNAPSTATE.MERGE_PLAYER: return 32;
                case SCHNAPSTATE.MERGE_COMPUTER: return 64;
                case SCHNAPSTATE.STARTS_SPLIT_TALON: return 256;
                case SCHNAPSTATE.STARTS_FIST_TALON: return 512;
				case SCHNAPSTATE.NONE: return 1023;
            }
            return 0;
        }
    }
}