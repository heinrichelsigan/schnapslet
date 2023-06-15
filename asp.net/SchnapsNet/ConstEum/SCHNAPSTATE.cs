using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchnapsNet.ConstEnum
{
    public enum SCHNAPSTATE 
	{

		GAME_START          = 0,
        MERGING_CARDS       = 1,
        MERGE_PLAYER        = 2,
        MERGE_COMPUTER      = 3,
        PLAYER_TAKES        = 4, 
        PLAYER_1ST_3        = 5,
        COMPUTER_1ST_3      = 6,
        PLAYER_FIST         = 8,
        PLAYER_1ST_5        = 9,
        GIVE_ATOU           = 10,
        PLAYER_2ND_2        = 12,
        COMPUTER_2ND_2      = 13,
        COMPUTER_1ST_5      = 14,
        GIVE_TALON          = 15,
		GAME_STARTED        = 16,
		COLOR_HIT_RULE      = 17,
		GAME_CLOSED         = 18,
		TALON_ONE_REMAINS   = 19,
		TALON_CONSUMED      = 20,
		ZERO_CARD_REMAINS   = 21,		
		STARTS_SPLIT_TALON  = 22,
		STARTS_FIST_TALON   = 23,
		NONE                = 1023
		
    }
    
    public static class SCHNAPSTATE_Extensions
    {
        public static int GetValue(this SCHNAPSTATE schnapState)
        {
            switch(schnapState)
            {
                case SCHNAPSTATE.GAME_START: return 0;
                case SCHNAPSTATE.MERGING_CARDS: return 1;
                case SCHNAPSTATE.MERGE_PLAYER: return 2;
                case SCHNAPSTATE.MERGE_COMPUTER: return 3;

                case SCHNAPSTATE.PLAYER_TAKES: return 4;
                case SCHNAPSTATE.PLAYER_1ST_3: return 5;
                case SCHNAPSTATE.COMPUTER_1ST_3: return 6;
                                
                case SCHNAPSTATE.PLAYER_FIST: return 8;
                case SCHNAPSTATE.PLAYER_1ST_5: return 9;
                case SCHNAPSTATE.GIVE_ATOU: return 10;

                case SCHNAPSTATE.PLAYER_2ND_2: return 12;
                case SCHNAPSTATE.COMPUTER_1ST_5: return 13;
                case SCHNAPSTATE.COMPUTER_2ND_2: return 14;

                case SCHNAPSTATE.GIVE_TALON: return 15;

                case SCHNAPSTATE.GAME_STARTED: return 16;
                case SCHNAPSTATE.COLOR_HIT_RULE: return 17;
                case SCHNAPSTATE.GAME_CLOSED: return 18;
                case SCHNAPSTATE.TALON_ONE_REMAINS: return 19;
                case SCHNAPSTATE.TALON_CONSUMED: return 20;
				case SCHNAPSTATE.ZERO_CARD_REMAINS: return 21;

                case SCHNAPSTATE.STARTS_SPLIT_TALON: return 22;
                case SCHNAPSTATE.STARTS_FIST_TALON: return 23;
				case SCHNAPSTATE.NONE: return 1023;
            }
            return 0;
        }

        public static int StateValue(SCHNAPSTATE schnapState)
        {
            switch (schnapState)
            {
                case SCHNAPSTATE.GAME_START: return 0;
                case SCHNAPSTATE.MERGING_CARDS: return 1;
                case SCHNAPSTATE.MERGE_PLAYER: return 2;
                case SCHNAPSTATE.MERGE_COMPUTER: return 3;

                case SCHNAPSTATE.PLAYER_TAKES: return 4;
                case SCHNAPSTATE.PLAYER_1ST_3: return 5;
                case SCHNAPSTATE.COMPUTER_1ST_3: return 6;

                case SCHNAPSTATE.PLAYER_FIST: return 8;
                case SCHNAPSTATE.PLAYER_1ST_5: return 9;

                case SCHNAPSTATE.GIVE_ATOU: return 10;

                case SCHNAPSTATE.PLAYER_2ND_2: return 12;
                case SCHNAPSTATE.COMPUTER_1ST_5: return 13;
                case SCHNAPSTATE.COMPUTER_2ND_2: return 14;

                case SCHNAPSTATE.GIVE_TALON: return 15;

                case SCHNAPSTATE.GAME_STARTED: return 16;
                case SCHNAPSTATE.COLOR_HIT_RULE: return 17;
                case SCHNAPSTATE.GAME_CLOSED: return 18;
                case SCHNAPSTATE.TALON_ONE_REMAINS: return 19;
                case SCHNAPSTATE.TALON_CONSUMED: return 20;
                case SCHNAPSTATE.ZERO_CARD_REMAINS: return 21;

                case SCHNAPSTATE.STARTS_SPLIT_TALON: return 22;
                case SCHNAPSTATE.STARTS_FIST_TALON: return 23;
                case SCHNAPSTATE.NONE: return 1023;
            }
            return 0;
        }
    }
}