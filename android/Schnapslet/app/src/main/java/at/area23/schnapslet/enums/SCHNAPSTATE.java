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
package at.area23.schnapslet.enums;

import java.lang.String;

/**
 * SCHNAPSTATE enum represents game state in schnapsen
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public enum SCHNAPSTATE {

	GAME_START(0),
	GAME_STARTED(1),
	COLOR_HIT_RULE(2),
	GAME_CLOSED(3),
	TALON_ONE_REMAINS(5),
	TALON_CONSUMED(7),
	GAME_STOP(8),
	MERGING_CARDS(16),
	MERGE_PLAYER(32),
	MERGE_COMPUTER(64),
	STARTS_SPLIT_TALON(256),
	STARTS_FIST_TALON(512),
	NONE(1023);

	/**
	 * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier.
	 */
	SCHNAPSTATE(int value) {
		this.value = value;
	}

	private final int value;

	public int getValue() {
		return value;
	}

	public String getName() { return this.name(); }
}

