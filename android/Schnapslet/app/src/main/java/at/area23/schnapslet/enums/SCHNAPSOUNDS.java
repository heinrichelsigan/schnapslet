/*
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>, <a href="mailto:Eman@gmx.at">Georg Toth</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * <p>SUPU is the idea of  by <a href="mailto:Eman@gmx.at">Georg Toth</a>
 * based Sudoku with colors instead of numbers.</p>
 *
 * <P>Coded 2021 by <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 */

package at.area23.schnapslet.enums;

/**
 * BOARDCOL represents the enumerator for columns of the board
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
     * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier.
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
    public String saySpeach() {
        switch (this.getValue()) {
            case 1:     return "merging cards for new game";
            case 2:     return "Game started.";
            case 3:     return "changed Jack against Atou";
            case 4:     return "twenty";
            case 5:     return "fourty atou marriage";
            case 6:     return "game closed now";
            case 7:     return "color hit rule";
            case 8:     break;
            case 9:     break;
            case 10:    break;
            case 11:    return "Player beats!";
            case 12:    return "Computer beats!";
            case 13:    return "Player wins!";
            case 14:    return "Computer wins!";
            case 15:    return "Game ended";
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

