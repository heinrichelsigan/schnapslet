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

import java.lang.*;
import java.util.HashMap;

import android.content.res.Resources;
import android.content.Context;

import at.area23.schnapslet.*;
import at.area23.schnapslet.models.*;
import at.area23.schnapslet.constenum.*;

/**
 * Player class represents the player or computer in a game.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class Player {
    volatile boolean begins;    // does this Player begin ?
    public final Card[] hand = new Card[5];  // Cards in Players Hand
    // not implemented yet !
    // Card hits[] = new Card[10]; // hits made by this player

    public PLAYERDEF playerDefinition = PLAYERDEF.UNKNOWN;
    public boolean hasClosed = false;
    public int points = 0;             // points made by this player
    // char pairs[] = {'n', 'n', 'n', 'n'};
    final PairCard[] pairs = new PairCard[2];
    public final char[] handpairs = {'n', 'n'};
    final int[] colorHitArray = {0, 0, 0, 0, 0};
    public int playerOptions = 0;

    public int stitchCount = 0;
    public HashMap<Integer, TwoCards> cardStitchs = new HashMap<>();

    final Resources r;
    final Context context;

    /**
     * Constructor of Player
     * @param c context of android app
     */
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

    /**
     * Constructor of Player
     * @param c context of android app
     * @param playerDef definition if human or computer
     * @param starts true if this player starts the game, otherwise false
     */
    public Player(Context c, PLAYERDEF playerDef, boolean starts) {
        this(c);
        this.playerDefinition = playerDef;
        this.begins = starts;
    }

    /**
     * Destructor of Player
     */
    public void stop() {
        int i;
        for (i = 0; i < 5; i++)
            hand[i] = null;

        stitchCount = 0;
        cardStitchs.clear();
    }

    /**
     * showHand show players hand cards
     * @return concatenate String of all players hand cards
     */
    public String showHand() {
        StringBuilder retVal = new StringBuilder();
        for (int j = 0; j < 5; j++)
            retVal.append(hand[j].getName()).append(" ");
        return retVal.toString();
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
            if ((hand[i].isAtou()) && (hand[i].cardValue == CARDVALUE.JACK)) {
                return i;
            }
        return (-1);
    }

    /**
     * hasPair - checks if a player (the player or computer) has a Queen & King pair
     * @return number of pairs, that player has (1 or 2)
     */
    public int hasPair() {
        int i, hpidx = 0;
        pairs[0] = null;
        pairs[1] = null;
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
     * isValidInColorHitsContext checks it a card is valid in color hit rule context
     * @param nynum - number of the played card
     * @param aCard - another card
     * @return true if played card is valid in color hit rule context, otherwise false
     */
    public boolean isValidInColorHitsContext(int nynum, Card aCard) {
        int i = 0;
        int j = 0;
        int max = -1;
        for (i = 0; i < 5; i++) {
            // is valid card -> PRI 0
            if (!hand[i].isValidCard()) {
                colorHitArray[i] = -1;
            } else if (hand[i].isValidCard()) {
                // any valid card => PRIO 0
                colorHitArray[i] = 0;
                if (max < 0) max = 0;

                if ((hand[i].color) != aCard.color) {
                    // can hit with atou => PRIO 1
                    if ((!aCard.isAtou()) && (hand[i].isAtou())) { // aCard.isAtou() == false)
                        colorHitArray[i] = 1;
                        if (max < 1) max = 1;
                    }
                } else if ((hand[i].color) == aCard.color) {
                    // same color => PRIO 2
                    colorHitArray[i] = 2;
                    if (max < 2) max = 2;

                    // can hit with same color => PRIO 3
                    if ((hand[i].getValue()) > (aCard.getValue())) {
                        colorHitArray[i] = 3;
                        if (max < 3) max = 3;
                    }
                }
            }
        }

        return (colorHitArray[nynum] == max); // return true;
        // return false;
    }

    /**
     * preferedInColorHitsContext calculate best card number, that is valid s valid in color hit rule context
     * @param aCard - another card
     * @return number of best card to be played, that is valid in color hit rule context
     */
    public int preferedInColorHitsContext(Card aCard) {
        int i = 0, j = 0, tmp = -1, min = -1, max = -1, mark = -1, markMin = -1, markMax = -1;

        for (i = 0; i < 5; i++)
        {
            if (!hand[i].isValidCard())
            {
                tmp = -1;
            }
            else if (hand[i].isValidCard())
            {
                // valid card => max = CardValue
                tmp = hand[i].cardValue.getValue();

                if (hand[i].cardColor.getChar() != aCard.cardColor.getChar())
                {
                    // not same colors && atou => max is atou card value
                    if ((hand[i].isAtou()) && (!aCard.isAtou())) // can hit with atou -> PRI 1
                        tmp = 11 + hand[i].cardValue.getValue();
                }
                else if (hand[i].cardColor.getChar() == aCard.cardColor.getChar())
                {
                    // same colors
                    tmp = 22 + hand[i].cardValue.getValue();

                    if (hand[i].cardValue.getValue() > aCard.cardValue.getValue())
                        tmp = 33 + hand[i].cardValue.getValue();
                }
            }
            colorHitArray[i] = tmp;
            if (tmp >= max) max = tmp;
            if (tmp >= 0 && min < tmp) min = tmp;
        }

        if (max > 0 && max <= 11)
        {
            min = max; markMin = -1; markMax = -1;
            for (j = 0; j < 5; j++)
            {
                if (colorHitArray[j] >= 0 && colorHitArray[j] <= 11 && colorHitArray[j] == max)
                    markMax = j;
                if (colorHitArray[j] >= 0 && colorHitArray[j] < 11 && colorHitArray[j] < min)
                {
                    min = colorHitArray[j];
                    markMin = j;
                }
            }
            return (markMin >= 0) ? markMin : markMax;
        }
        if (max > 11 && max <= 22) // can only hit with atou
        {
            min = max; markMin = -1; markMax = -1;
            Card maxBestCard = null, minBestCard = null, tmpBestCard = null;
            for (j = 0; j < 5; j++)
            {
                if (colorHitArray[j] > 11 && colorHitArray[j] <= 22 && colorHitArray[j] == max)
                {
                    markMax = j;
                    maxBestCard = hand[j];
                    tmpBestCard = hand[j];
                }
                if (colorHitArray[j] > 11 && colorHitArray[j] < 22 && colorHitArray[j] < min)
                {
                    markMin = j;
                    min = colorHitArray[j];
                    minBestCard = hand[j];
                    tmpBestCard = hand[j];
                }
            }
            mark = (markMin >= 0) ? markMin : markMax;
            int tmpHasPair = hasPair();
            if ((tmpHasPair == 1 && pairs[0].card1st.cardColor == tmpBestCard.cardColor) ||
                    (tmpHasPair == 2 && pairs[1].card1st.cardColor == tmpBestCard.cardColor))
            {
                markMax = -1; markMin = -1;
                for (j = 0; j < 5; j++)
                {
                    if (colorHitArray[j] > 11 && colorHitArray[j] <= 22 && colorHitArray[j] > 20)
                        markMax = j;
                    if (colorHitArray[j] > 11 && colorHitArray[j] <= 22 && colorHitArray[j] < 14)
                        markMin = j;
                }
                if (markMax >= 0)
                    return markMax;
                if (markMin >= 0)
                    return markMin;
                return mark;
            }
            if (markMin >= 0)
            {
                if (maxBestCard.cardValue == CARDVALUE.ACE)
                    return markMin;
                if (maxBestCard.cardValue == CARDVALUE.TEN)
                    return markMax;
                return markMin;
            }
            return markMax;
        }
        if (max > 22 && max <= 33)
        {
            min = max; markMin = -1; markMax = -1;
            Card maxBestCard = null, minBestCard = null, tmpBestCard = null;
            for (j = 0; j < 5; j++)
            {
                if (colorHitArray[j] > 22 && colorHitArray[j] <= 33 && colorHitArray[j] == max)
                {
                    markMax = j;
                    maxBestCard = hand[j];
                    tmpBestCard = hand[j];
                }
                if (colorHitArray[j] > 22 && colorHitArray[j] < 33 && colorHitArray[j] < min)
                {
                    markMin = j;
                    min = colorHitArray[j];
                    minBestCard = hand[j];
                    tmpBestCard = hand[j];
                }
            }
            mark = (markMin >= 0) ? markMin : markMax;
            int tmpHasPair = hasPair();
            if ((tmpHasPair == 1 && pairs[0].card1st.cardColor == tmpBestCard.cardColor) ||
                    (tmpHasPair == 2 && pairs[1].card1st.cardColor == tmpBestCard.cardColor))
            {
                if (minBestCard.cardValue == CARDVALUE.JACK)
                    return markMin;
            }
            return (markMin >= 0) ? markMin : markMax;
        }
        if (max > 33 && max <= 44)
        {
            min = max; markMin = -1; markMax = -1;
            Card maxBestCard = null, minBestCard = null, tmpBestCard = null;
            for (j = 0; j < 5; j++)
            {
                if (colorHitArray[j] > 33 && colorHitArray[j] <= 44 && colorHitArray[j] == max)
                {
                    markMax = j;
                    maxBestCard = hand[j];
                    tmpBestCard = hand[j];
                }
                if (colorHitArray[j] > 33 && colorHitArray[j] < 44 && colorHitArray[j] < min)
                {
                    min = colorHitArray[j];
                    markMin = j;
                    minBestCard = hand[j];
                    tmpBestCard = hand[j];
                }
            }
            return markMax;
        }

        return mark;
    }

    /**
     * bestInColorHitsContext calculate best card number, that is valid s valid in color hit rule context
     * @param aCard - another card
     * @return number of best card to be played, that is valid in color hit rule context
     */
    @Deprecated
    public int bestInColorHitsContext(Card aCard) {
        int i = 0, j = 0, mark = -1, max = -1;

        for (i = 0; i < 5; i++) {
            if (!hand[i].isValidCard()) {
                colorHitArray[i] = (-1);
            }
            else if (hand[i].isValidCard()) {

                colorHitArray[i] = 0;
                if (max < 0) max = 0;

                if ((hand[i].color) != (aCard.color)) {
                    // not same colors
                    if ((hand[i].isAtou()) && (!aCard.isAtou())) {
                        // ich kann mit Atou stechen -> PRI 1
                        colorHitArray[i] = 1;
                        if (max < 1) max = 1;
                    }
                }
                else {
                    if (hand[i].cardColor.getChar() == aCard.cardColor.getChar()) {
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
        }

        for (j = 0; j < 5; j++) {
            if (colorHitArray[j] == max) {
                if (mark < 0) mark = j;
                else if ((hand[mark].getValue()) < (hand[j].getValue())) {
                    mark = j;
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
     *              use {@link #isValidInColorHitsContext(int nynum, Card aCard)} instead like this:
     *
     * <blockquote><pre>
     * bool isPlayable = isInColorHitsContextValid(mynum, aCard);
     * </pre></blockquote>
     */
    @Deprecated
    public boolean isColorHitValid(int cidx, Card otherCard) {
        int i;
        // same color and hit => OK
        if (hand[cidx].hitsValue(otherCard))
            return true;

        // can hit with color ->
        for (i = 0; i < 5; i++) {
            if (hand[i].isValidCard())
                if (hand[i].hitsValue(otherCard))
                    return false;
        }

        // is card same color -> OK
        if (hand[cidx].color == otherCard.color)
            return true;

        for (i = 0; i < 5; i++) {
            if (hand[i].isValidCard()) {
                if (hand[i].getCardColor() == otherCard.getCardColor())
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
