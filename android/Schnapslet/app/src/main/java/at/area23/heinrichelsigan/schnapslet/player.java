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


import at.area23.heinrichelsigan.schnapslet.Card;
import java.lang.*;
import android.content.res.Resources;
import android.content.Context;
import java.io.*;
import java.net.*;
import java.util.*;

public class Player {
    volatile boolean begins;    // does this Player begin ?
    Card[] hand = new Card[5];  // Cards in Players Hand
    // not implemented yet !
    // Card hits[] = new Card[10]; // hits made by this player

    boolean hasClosed = false;
    int points = 0;             // points made by this player
    // char pairs[] = {'n', 'n', 'n', 'n'};
    PairCard[] pairs = new PairCard[2];
    char[] handpairs = {'n', 'n'};
    volatile int[] colorHitArray = {0, 0, 0, 0, 0};
    int playerOptions = 0;
    Resources r;
    Context context;

    public Player(Context c) {
        super();
        this.context = c;
        this.r = c.getResources();
        hasClosed = false;
        for (int i = 0; i < 5; i++) {
            hand[i] = new Card(c);
            colorHitArray[i] = (-1);
        }
    }

    public Player(boolean starts, Context c) {
        this(c);
        this.begins = starts;
    }

    public void stop() {
        int i;
        for (i = 0; i < 5; i++)
            hand[i] = null;
    }

    public String showHand() {
        String retVal = "";
        for (int j = 0; j < 5; j++)
            retVal = retVal + hand[j].getName() + " ";
        return retVal;
    }

    public void sortHand() { // BubbleSort
        int j, k, min, mark;
        Card tmpCard;
        for (k = 0; k < 4; k++) { // Bubble
            min = 20;
            mark = -1;
            for (j = k; j < 5; j++) {
                if (hand[j].intern < min) {
                    min = hand[j].intern;
                    mark = j;
                }
            }
            if (mark > 0) {
                tmpCard = hand[mark];
                hand[mark] = hand[k];
                hand[k] = tmpCard;
            }
        }
    }

    /**
     * canChangeAtou
     * @return if can change atou, return index of card, that is Jack to change, otherwiese -1
     */
    public int canChangeAtou() {
        for (int i = 0; i < 5; i++)
            if ((hand[i].isAtou()) && (hand[i].getValue() == 2)) {
                return i;
            }
        return (-1);
    }

    /**
     * has20 - checks if a player (the player or computer) has a Queen & King pair
     * @return number of pairs, that player has (1 or 2)
     */
    public int has20() {
        int i, hpidx = 0;
        handpairs[0] = 'n';
        handpairs[1] = 'n';

        for (i = 0; i < 4; i++) {
            if (hand[i].color == hand[i + 1].color) {
                if ((hand[i].getValue() + hand[i + 1].getValue()) == 7) {

                    if (hpidx == 0) {
                        pairs[0] = new PairCard(hand[i], hand[i + 1]);
                    }
                    if (hpidx == 1) {
                        pairs[1] = new PairCard(hand[i], hand[i + 1]);
                    }

                    handpairs[hpidx++] = hand[i].color;
                }
            }
        }
        return hpidx;
    }

    /**
     * assignCard assigns a new card to players hand
     * @param gotCard - the new card to assign
     */
    public void assignCard(Card gotCard) {
        int i = 0;
        while ((i < 5) && (hand[i].isValidCard()))
            i++;
        if (i < 5)
            hand[i] = new Card(gotCard, context);
    }

    public boolean isInColorHitsContextValid(int nynum, Card aCard) {
        int i = 0;
        int j = 0;
        int max = -1;
        for (i = 0; i < 5; i++) {
            // ist gueltige Karte -> PRI 0
            if (!hand[i].isValidCard()) {
                colorHitArray[i] = (-1);
            } else if (hand[i].isValidCard()) {

                colorHitArray[i] = 0;
                if (max < 0) max = 0;

                // ich hab gleich Farbe -> PRI 2
                if ((hand[i].color) != aCard.color) {
                    // ich kann mit Atou stechen -> PRI 1
                    if ((aCard.isAtou() == false) && (hand[i].isAtou())) {
                        colorHitArray[i] = 1;
                        if (max < 1) max = 1;
                    }
                } else if ((hand[i].color) == aCard.color) {

                    colorHitArray[i] = 2;
                    if (max < 2) max = 2;

                    // ich kann aber auch noch stechen -> PRI 3
                    if ((hand[i].getValue()) > (aCard.getValue())) {
                        colorHitArray[i] = 3;
                        if (max < 3) max = 3;
                    }
                }
            }
        }

        if (colorHitArray[nynum] == max) return true;
        return false;
    }

    public int bestInColorHitsContext(Card aCard) {
        int i = 0, j = 0, mark = -1, max = -1;

        for (i = 0; i < 5; i++) {
            if (!hand[i].isValidCard()) {
                colorHitArray[i] = (-1);
            } else if (hand[i].isValidCard()) {

                colorHitArray[i] = 0;
                if (max < 0) max = 0;

                if ((hand[i].color) != (aCard.color)) {
                    // not same colors
                    if ((hand[i].isAtou()) && (!aCard.isAtou())) {
                        // ich kann mit Atou stechen -> PRI 1
                        colorHitArray[i] = 1;
                        if (max < 1) max = 1;
                    }
                } else if ((hand[i].color) == (aCard.color)) {
                    // same colors
                    colorHitArray[i] = 2;
                    if (max < 2) max = 2;

                    if ((hand[i].getValue()) > (aCard.getValue())) {
                        colorHitArray[i] = 3;
                        if (max < 3) max = 3;
                    }
                }
            }
        }

        for (j = 0; j < 5; j++) {
            if (colorHitArray[j] == max) {
                if (mark < 0) mark = j;
                else switch (max) {
                    case 0:
                    case 2:
                        if ((hand[mark].getValue()) <  (hand[j].getValue()))
                            mark = j;
                        break;
                    case 1:
                    case 3:
                        if ((hand[mark].getValue()) <  (hand[j].getValue()))
                            mark = j;
                        break;
                    default:
                        break;
                }
            }
        }

        return mark;
    }

    public boolean isColorHitValid(int cidx, Card otherCard) {
        int i;
        // gleiche Farbe und groesser -> OK
        if (hand[cidx].hitsValue(otherCard))
            return true;

        // Kann ich mit Farbe stechen ->
        for (i = 0; i < 5; i++) {
            if (hand[i].isValidCard())
                if (hand[i].hitsValue(otherCard))
                    return false;
        }

        // Ist die Karte gleicher Farbe -> OK
        if (hand[cidx].color == otherCard.color)
            return true;

        for (i = 0; i < 5; i++) {
            if (hand[i].isValidCard()) {
                if (hand[i].getColor() == otherCard.getColor())
                    return false;
            }
        }

        if (hand[cidx].isAtou())
            return true;

        for (i = 0; i < 5; i++) {
            if (hand[i].isAtou())
                return false;
        }

        return true;
    }

}
