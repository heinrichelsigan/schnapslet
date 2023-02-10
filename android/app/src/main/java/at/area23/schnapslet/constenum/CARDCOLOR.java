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
 * CARDCOLOR enum represents the color of a card.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public enum CARDCOLOR {

	EMPTY('e'),
	NONE('n'),
	HEARTS('h'),
	SPADES('p'),
	DIAMONDS('k'),
	CLUBS('t');

	/**
	 * Enum CARDCOLOR constructor
	 * must have private or package scope. You can not use the public access modifier.
	 */
	CARDCOLOR(char colorCh) {
		this.colorChar = colorCh;
	}

	private final char colorChar;

	/**
	 * getChar()
	 * @return char colorChar
	 */
	public char getChar() { return colorChar; }

	/**
	 * getName()
	 * @return String this.name
	 */
	public String getName() { return this.name(); }

}

