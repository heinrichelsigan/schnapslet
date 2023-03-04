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
 * PLAYERDEF enum represents possible definition of players
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public enum PLAYERDEF {
	HUMAN(0),
	COMPUTER(1);

	PLAYERDEF(int value) { this.value = value; }

	private final int value;

	/**
	 * getValue()
	 * @return int value
	 */
	public int getValue() {
		return value;
	}
}




	


