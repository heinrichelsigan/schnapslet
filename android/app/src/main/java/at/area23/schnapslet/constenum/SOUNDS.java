/*
 * @author           <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 * @version          V 1.0.1
 * @since            API 27 Oreo 8.1
 *
 * based on austrian card game Schnapsen 66.</p>
 *
 * <P>Coded 2021 by <a href="mailto:heinrich.elsigan@area23.at">Heinrich Elsigan</a>
 */

package at.area23.schnapslet.constenum;

/**
 * BOARDCOL represents the enumerator for columns of the board
 */
public enum SOUNDS {
    DRAG_CARD(1),
    DROP_CARD(2),
    CAMERA_CLICK(3),
	
    CHANGE_ATOU(4),
    SAY_20(5),
    SAY_40(6),
	
    CLOSE_GAME(7),
    COLOR_HIT_RULE(8),
    AND_ENOUGH(9),
	
    MERGE_CARDS(10),
    CHANGE_CARDDECK(11),
	
	PLAYER_HITS (12),
    COMPUTER_HITS(13),
    PLAYER_WIN(14),
    COMPUTER_WIN(15),

    NONE((int)Byte.MAX_VALUE);

    /**
     * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier.
     */
    SOUNDS(int value) {
        this.value = value;
    }

    private final int value;

    /**
     * getValue
     * @return (@link int) value
     */
    public int getValue() { return value; }


    /**
     * getName
     * @return upper letter {@link String} or NONE
     */
    public String getName() {
        switch (this.getValue()) {
            case 1:     return "windows_restore.wav";
            case 2:     return "windows_recycle.wav";
            case 3:     return "windows_camera_shutter.wav";
            case 4:     return "windows_hardware_remove.wav";
            case 5:     return "windows_hardware_insert.wav";
            case 6:     return "windows_logon.wav";
            case 7:
            case 8:
                return "windows_ding.wav";
            case 9:     return "kbd_keytap.wav";
            case 10:    return "windows_notify_messaging.wav";
            case 11:    return "windows_notify_email.wav";
			case 12: 	return "windows_background.wav";
            case 13:    return "windows_error.wav";
            case 14:    return "windows_logoff.wav";
            case 15:    return "windows_critical_stop.wav";
            default:
                return null;
        }
    }


    /**
     * getEnum
     * @param idx integer index for enum SOUNDS
     * @return the enum {@link SOUNDS}
     */
    public static SOUNDS getEnum(int idx) {
        switch (idx) {
            case 1: return SOUNDS.DRAG_CARD;
            case 2: return SOUNDS.DROP_CARD;
            case 3: return SOUNDS.CAMERA_CLICK;
            case 4: return SOUNDS.CHANGE_ATOU;
            case 5: return SOUNDS.SAY_20;
            case 6: return SOUNDS.SAY_40;
            case 7: return SOUNDS.CLOSE_GAME;    
            case 8: return SOUNDS.COLOR_HIT_RULE;
            case 9: return SOUNDS.AND_ENOUGH;
            case 10: return SOUNDS.MERGE_CARDS;
            case 11: return SOUNDS.CHANGE_CARDDECK;
            case 12: return SOUNDS.PLAYER_HITS;
            case 13: return SOUNDS.COMPUTER_HITS;
            case 14: return SOUNDS.PLAYER_WIN;
            case 15: return SOUNDS.COMPUTER_WIN;
            default:  break;
        }
        return SOUNDS.NONE;

    }

    /**
     * getEnum
     * @param ch column character
     * @return the enum {@link SOUNDS}
     */
    public static SOUNDS getEnum(char ch) {
        char uch = String.valueOf(ch).toUpperCase().charAt(0);
        switch (uch) {
            case '1': return SOUNDS.DRAG_CARD;
            case '2': return SOUNDS.DROP_CARD;
            case '3': return SOUNDS.CAMERA_CLICK;
            case '4': return SOUNDS.CHANGE_ATOU;
            case '5': return SOUNDS.SAY_20;
            case '6': return SOUNDS.SAY_40;
            case '7': return SOUNDS.CLOSE_GAME;
            case '8': return SOUNDS.COLOR_HIT_RULE;
            case '9': return SOUNDS.AND_ENOUGH;
            case 'A': return SOUNDS.MERGE_CARDS;
            case 'B': return SOUNDS.CHANGE_CARDDECK;
            case 'C': return SOUNDS.PLAYER_HITS;
            case 'D': return SOUNDS.COMPUTER_HITS;
            case 'E': return SOUNDS.PLAYER_WIN;
            case 'F': return SOUNDS.COMPUTER_WIN;
            default:  break;
        }
        return SOUNDS.NONE;

    }

}

