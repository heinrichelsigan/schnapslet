/*
*
* @author           Heinrich Elsigan
* @version          V 0.2
* @since            JDK 1.2.1
*

*/
/*
   Copyright (C) 2000 - 2002 Heinrich Elsigan

   Schnapslet java applet is free software; you can redistribute it and/or
   modify it under the terms of the GNU Library General Public License as
   published by the Free Software Foundation; either version 2 of the
   License, or (at your option) any later version.
   See the GNU Library General Public License for more details.

*/
package at.area23.heinrichelsigan.schnapslet;

import java.lang.*;

public enum CARDCOLOR {

	EMPTY('e'),
	NONE('n'),
	HERZ('h'),
	PIK('p'),
	KARO('k'),
	TREFF('t');

	/**
	 * NOTE: Enum constructor must have private or package scope. You can not use the public access modifier.
	 */
	private CARDCOLOR(char value) {
		this.value = value;
	}

	private final char value;

	public char getValue() {
		return value;
	}

};

