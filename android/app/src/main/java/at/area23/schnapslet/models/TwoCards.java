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
package at.area23.schnapslet.models;

import at.area23.schnapslet.constenum.CARDCOLOR;

/**
 * TwoCards class represents a pair (marriage) of cards.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class TwoCards {
    
	// boolean atou = false;
	// CARDCOLOR cardColor = CARDCOLOR.NONE; // Color of Pair
	// char color = 'n';   // 4 colors and 'n' for unitialized
    // int pairValue = 20;      // 5 values and 0 for unitialized
	public Card[] cards = new Card[2]; // the 2 Cards, that represents the pair
	public Card card1st = null;
	public Card card2nd = null;
	public int cardSumValue = 0;

    /**
     * Constructor TwoCards()
     */
    public TwoCards() {
        super();
		card1st = null;
		card2nd = null;
		cards = new Card[2];
		cards[0] = card1st;
		cards[1] = card2nd;
    }

	/**
	 * Constructor PairCard
	 * @param first represents 1st card
	 * @param second represents 2nd card
	 */
	public TwoCards(Card first, Card second) {
		this();
		
		card1st = new Card(first);
		card2nd = new Card(second);

		cards = new Card[2];
		cards[0] = card1st;
		cards[1] = card2nd;

		this.cardSumValue = card1st.getValue() + card2nd.getValue();
	}

}