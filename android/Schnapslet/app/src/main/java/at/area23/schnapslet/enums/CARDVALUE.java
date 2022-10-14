/*
*
* @author           Heinrich Elsigan
* @version          V 1.3.4
* @since            JDK 1.2.1
*
*/
/*
   Copyright (C) 2000 - 20018 Heinrich Elsigan

   Schnapslet java applet is free software; you can redistribute it and/or
   modify it under the terms of the GNU Library General Public License as
   published by the Free Software Foundation; either version 2 of the
   License, or (at your option) any later version.
   See the GNU Library General Public License for more details.

*/
package at.area23.schnapslet.enums;

import java.lang.String;

/**
 * CARDVALUE enum represents the value of a card.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public enum CARDVALUE {

	EMPTY(-2),
	NONE(-1),
	JACK(2),
	QUEEN(3),
	KING(4),
	TEN(10),
	ACE(11);

	private final int value;

	/**
	 * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier.
	 */
	CARDVALUE(int value) {
		this.value = value;
	}

	public int getValue() { return value; }

	public String getName() {
		return this.name();
	}
}


	


