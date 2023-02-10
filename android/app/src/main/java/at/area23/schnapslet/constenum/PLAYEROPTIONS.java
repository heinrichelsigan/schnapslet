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
 * PLAYEROPTIONS enum represents possible options of playing.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public enum PLAYEROPTIONS {

	CHANGEATOU(1),
	CLOSESGAME(2),
	SAYPAIR(4),
	PLAYSCARD(8),
	HITSCARD(16),
	ANDENOUGH(32);

	/**
	 * Enum PLAYEROPTIONS constructor
	 *  must have private or package scope. You can not use the public access modifier.
	 */
	PLAYEROPTIONS(int value) {
		this.value = value;
	}

	private final int value;

	/**
	 * getValue()
	 * @return int value
	 */
	public int getValue() {
		return value;
	}
}



	


