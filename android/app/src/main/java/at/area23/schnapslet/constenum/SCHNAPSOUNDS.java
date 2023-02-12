/*
 *
 * @author           Heinrich Elsigan root@darkstar.work
 * @version          V 1.6.9
 * @since            API 26 Android Oreo 8.1
 *
 */
/*
   Copyright (C) 2000 - 2023 Heinrich Elsigan root@darkstar.work

   Schnapslet java applet is free software; you can redistribute it and/or
   modify it under the terms of the GNU Library General Public License as
   published by the Free Software Foundation; either version 2 of the
   License, or (at your option) any later version.
   See the GNU Library General Public License for more details.

*/

package at.area23.schnapslet.constenum;

import android.content.Context;

import at.area23.schnapslet.R;

/**
 * enum SCHNAPSOUNDS represents the enumerator for columns of the board
 */
public enum SCHNAPSOUNDS {
    MERGE_CARDS(1),
    GAME_START(2),
    CHANGE_ATOU(3),
    SAY20(4),
    SAY40(5),
    GAME_CLOSE(6),
    COLOR_HIT_MODE(7),

    PLAYER_BEATS(11),
    COMPUTER_BEATS(12),
    PLAYER_WIN(13),
    COMPUTER_WIM(14),
    GAME_END(15),
    NONE((int)Byte.MAX_VALUE);

    /**
     * Enum SCHNAPSOUNDS constructor
     * must have private or package scope. You can not use the public access modifier.
     */
    SCHNAPSOUNDS(int value) {
        this.value = value;
    }

    private final int value;

    /**
     * getValue
     * @return (@link int) value
     */
    public int getValue() { return value; }


    /**
     * saySpeach
     * @return upper letter {@link String} or NONE
     */
    public String saySpeach(Context c) {
        switch (this.getValue()) {
            case 1:     return c.getString(R.string.merging_cards);
            case 2:     return c.getString(R.string.newgame_starts);
            case 3:     return c.getString(R.string.bChange_text);
            case 4:     return c.getString(R.string.twenty_in_color);
            case 5:     return c.getString(R.string.fourty_in_color);
            case 6:     return c.getString(R.string.close_game);
            case 7:     return c.getString(R.string.color_hit_force_mode);
            case 8:     break;
            case 9:     break;
            case 10:    break;
            case 11:    return c.getString(R.string.your_hit_points);
            case 12:    return c.getString(R.string.computer_hit_points);
            case 13:    return c.getString(R.string.you_have_won_points);
            case 14:    return c.getString(R.string.computer_has_won_points);
            case 15:    return c.getString(R.string.nogame_started);
            default:    break;
        }
        return null;
    }

    /**
     * getName
     * @return upper letter {@link String} or NONE
     */
    public String getName() {
        switch (this.getValue()) {
            case 1: return SCHNAPSOUNDS.MERGE_CARDS.toString();
            case 2: return SCHNAPSOUNDS.GAME_START.toString();
            case 3: return SCHNAPSOUNDS.CHANGE_ATOU.toString();
            case 4: return SCHNAPSOUNDS.SAY20.toString();
            case 5: return SCHNAPSOUNDS.SAY40.toString();
            case 6: return SCHNAPSOUNDS.GAME_CLOSE.toString();
            case 7: return SCHNAPSOUNDS.COLOR_HIT_MODE.toString();
            case 8: break;
            case 9: break;
            case 10: break;
            case 11: return SCHNAPSOUNDS.PLAYER_BEATS.toString();
            case 12: return SCHNAPSOUNDS.COMPUTER_BEATS.toString();
            case 13: return SCHNAPSOUNDS.PLAYER_WIN.toString();
            case 14: return SCHNAPSOUNDS.COMPUTER_WIM.toString();
            case 15: return SCHNAPSOUNDS.GAME_END.toString();
            default:  break;
        }
        return SCHNAPSOUNDS.NONE.toString();
    }


    /**
     * getEnum
     * @param idx int
     * @return the enum {@link SCHNAPSOUNDS}
     */
    public static SCHNAPSOUNDS getEnum(int idx) {
        switch (idx) {
            case 1: return SCHNAPSOUNDS.MERGE_CARDS;
            case 2: return SCHNAPSOUNDS.GAME_START;
            case 3: return SCHNAPSOUNDS.CHANGE_ATOU;
            case 4: return SCHNAPSOUNDS.SAY20;
            case 5: return SCHNAPSOUNDS.SAY40;
            case 6: return SCHNAPSOUNDS.GAME_CLOSE;
            case 7: return SCHNAPSOUNDS.COLOR_HIT_MODE;
            case 8: break;
            case 9: break;
            case 10: break;
            case 11: return SCHNAPSOUNDS.PLAYER_BEATS;
            case 12: return SCHNAPSOUNDS.COMPUTER_BEATS;
            case 13: return SCHNAPSOUNDS.PLAYER_WIN;
            case 14: return SCHNAPSOUNDS.COMPUTER_WIM;
            case 15: return SCHNAPSOUNDS.GAME_END;
            default:  break;
        }
        return SCHNAPSOUNDS.NONE;

    }

    /**
     * getEnum
     * @param ch column character
     * @return the enum {@link SCHNAPSOUNDS}
     */
    public static SCHNAPSOUNDS getEnum(char ch) {
        char uch = String.valueOf(ch).toUpperCase().charAt(0);
        switch (uch) {
            case '1': return SCHNAPSOUNDS.MERGE_CARDS;
            case '2': return SCHNAPSOUNDS.GAME_START;
            case '3': return SCHNAPSOUNDS.CHANGE_ATOU;
            case '4': return SCHNAPSOUNDS.SAY20;
            case '5': return SCHNAPSOUNDS.SAY40;
            case '6': return SCHNAPSOUNDS.GAME_CLOSE;
            case '7': return SCHNAPSOUNDS.COLOR_HIT_MODE;
            case '8': break;
            case '9': break;
            case 'A': break;
            case 'B': return SCHNAPSOUNDS.PLAYER_BEATS;
            case 'C': return SCHNAPSOUNDS.COMPUTER_BEATS;
            case 'D': return SCHNAPSOUNDS.PLAYER_WIN;
            case 'E': return SCHNAPSOUNDS.COMPUTER_WIM;
            case 'F': return SCHNAPSOUNDS.GAME_END;
            default:  break;
        }
        return SCHNAPSOUNDS.NONE;

    }

}

