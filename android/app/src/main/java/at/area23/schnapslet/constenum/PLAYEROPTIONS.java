/*
*
* @author           Heinrich Elsigan
* @version          V 1.3.4
* @since            JDK 1.2.1
*
*/
/*
   Copyright (C) 2000 - 2018 Heinrich Elsigan

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
	 * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier.
	 */
	PLAYEROPTIONS(int value) {
		this.value = value;
	}

	private final int value;

	public int getValue() {
		return value;
	}
}



	


