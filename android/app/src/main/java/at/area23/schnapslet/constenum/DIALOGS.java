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


/**
 * DIALOGS enum for defined dialog fragments
 */
public enum DIALOGS {
	About('a'),
	Help('h'),
	GameOver('g'),
	FinishedGame('f'),
    None('b');

    /**
     * enum DIALOGS constructor must have private or package scope. You can not use the public access modifier.
     */
    DIALOGS(char dialogCh) {
        this.dialogChar = dialogCh;
    }

    private final char dialogChar;

    /**
     * getChar
     * @return (@link char) dialogChar
     */
    public int getChar() { return dialogChar; }
	
}

