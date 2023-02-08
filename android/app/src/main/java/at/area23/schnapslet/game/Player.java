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
package at.area23.schnapslet.game;

import android.content.Context;

import java.util.Locale;

/**
 * Player class represents the player or computer in a game.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class Player extends SchnapsAtom {
    volatile boolean begins;    // does this Player begin ?
    public final Card[] hand = new Card[5];  // Cards in Players Hand
    // not implemented yet !
    // Card hits[] = new Card[10]; // hits made by this player

    public boolean hasClosed = false;
    public int points = 0;             // points made by this player
    // char pairs[] = {'n', 'n', 'n', 'n'};
    final PairCard[] pairs = new PairCard[2];
    public final char[] handpairs = {'n', 'n'};
    final int[] colorHitArray = {0, 0, 0, 0, 0};
    public int playerOptions = 0;
    // final Resources r;
    // final Context context;

    /**
     * Constructor of Player
     * @param c context of android app
     */
    public Player(Context c) {
        super();
        this.context = c;
        this.r = c.getResources();
        initLocale();
        hasClosed = false;
        for (int i = 0; i < 5; i++) {
            hand[i] = new Card(c);
            colorHitArray[i] = (-1);
        }
    }

    /**
     * Constructor of Player
     * @param starts true if this player starts the game, otherwise false
     * @param c context of android app
     */
    public Player(boolean starts, Context c) {
        this(c);
        this.begins = starts;
    }

    /**
     * Destructor of Player
     */
    public void stop() {
        int i;
        for (i = 0; i < 5; i++)
            hand[i] = null;
    }

    /**
     * setLocale set current locale
     * @param loc Locale
     */
    @Override
    public void setLocale(Locale loc) {
        if (loc.getLanguage() != locale.getLanguage()) {
            locale = loc;

            if (hand != null) {
                for (int ihc = 0; ihc < hand.length; ihc++) {
                    if (hand[ihc] != null) {
                        hand[ihc].setLocale(loc);
                    }
                }
            }
            if (pairs != null) {
                for (int ihp = 0; ihp < pairs.length; ihp++) {
                    if (pairs[ihp] != null && pairs[ihp].pairs != null) {
                        for (int ihpc = 0; ihpc < pairs[ihp].pairs.length; ihpc++) {
                            pairs[ihp].pairs[ihpc].setLocale(loc);
                        }
                    }
                }
            }

            localeChanged = true;
        }
    }

    /**
     * showHand show players hand cards
     * @return concatenate String of all players hand cards
     */
    public String showHand() {
        String retVal = "";
        for (int j = 0; j < 5; j++) {
            retVal = String.valueOf(retVal) +  hand[j].getName() + " ";
            // retVal = retVal + hand[j].getName() + " ";
        }
        return retVal;
    }

    /**
     * sortHand sorts player hand
     */
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

    /**
     * isInColorHitsContextValid checks it a card is valid in color hit rule context
     * @param nynum - number of the played card
     * @param aCard - another card
     * @return true if played card is valid in color hit rule context, otherwise false
     */
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
                    if ((!aCard.isAtou()) && (hand[i].isAtou())) { // aCard.isAtou() == false
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

        return (colorHitArray[nynum] == max);
    }

    /**
     * bestInColorHitsContext calculate best card number, that is valid s valid in color hit rule context
     * @param aCard - another card
     * @return number of best card to be played, that is valid in color hit rule context
     */
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

    /**
     * @param cidx - number of the played card
     * @param otherCard - another card
     * @return true if played card is valid in color hit rule context, otherwise false
     *
     * @deprecated  isColorHitValid is marked deprecated<br/>
     *              {will be removed in next version} <br/>
     *              use {@link #isInColorHitsContextValid(int nynum, Card aCard)} instead like this:
     *
     * <blockquote><pre>
     * bool isPlayable = isInColorHitsContextValid(mynum, aCard);
     * </pre></blockquote>
     */
    @Deprecated
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
