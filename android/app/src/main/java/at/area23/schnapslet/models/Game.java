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
import java.util.Random;
import android.content.res.Resources;
import android.content.Context;

import at.area23.schnapslet.*;
import at.area23.schnapslet.constenum.CARDCOLOR;
import at.area23.schnapslet.constenum.CARDVALUE;
import at.area23.schnapslet.constenum.PLAYERDEF;
import at.area23.schnapslet.constenum.PLAYEROPTIONS;
import at.area23.schnapslet.constenum.SCHNAPSTATE;

/**
 * Game class represents a single game.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class Game {
    public volatile boolean isGame = false;     // a Game is running
    public boolean atouChanged = false;         // Atou allready changed
    public boolean playersTurn = true;          // Who's playing
    public boolean colorHitRule = false;        // Farb und Stichzwang
    public boolean isClosed = false;            // game is closed
    public boolean isReady = false;              // is ready to play out
    public boolean shouldContinue = false;      // should continue the game
    public boolean a20 = false;                 // can player announce 1st pair
    public boolean b20 = false;                 // can player announce 2nd pair
    public boolean bChange = false;             // can player change atou

    public SCHNAPSTATE schnapState = SCHNAPSTATE.NONE;
    public CARDCOLOR atouColor = CARDCOLOR.NONE;    // CARDCOLOR that is atou in this game
    // public char atouInGame = 'n';
    public char said = 'n';                   // player said pair char
    public char csaid = 'n';                  // computer said pair char

    public int index = 9;
    public int movs = 0;
    public int phoneDirection = -1;
    public int[] inGame = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                    10,11,12,13,14,15,16,17,18,19 };
    public int tournamentPoints = 1;

    public String sayMarriage20, sayMarriage40, statusMessage = "";

    public GlobalAppSettings globalAppSettings;
    public Card[] set = new Card[20];
    public Card playedOut, playedOut0, playedOut1;

    public Player gambler, computer;
    public PLAYERDEF whoStarts = PLAYERDEF.HUMAN, whoWon = PLAYERDEF.UNKNOWN;

    Random random;
    Resources r;
    Context context;

    MessageQueue mqueue = new MessageQueue();

    /**
     * constructor
     * @param c current context
     */
    public Game(Context c) {
        super();
        globalAppSettings = (GlobalAppSettings) c;
        this.context = c;
        this.r = c.getResources();
        isGame = true;
        atouChanged = false;
        playersTurn = true;
        colorHitRule = false;
        isClosed = false;
        shouldContinue = false;

        this.schnapState = SCHNAPSTATE.GAME_START;
        whoWon = PLAYERDEF.UNKNOWN;
        tournamentPoints = 0;
        mqueue.clear();
        mqueue.insert(r.getString(R.string.newgame_starts));

        playedOut0 = globalAppSettings.cardEmpty();
        playedOut1 = globalAppSettings.cardEmpty();;

        set = new Card[20];
        for (int i = 0; i < 20; i++) {
            set[i] = new Card(context); //new Card(applet, -1);
            inGame[i] = i;
        }

        index = 9;
        said = 'n';
        csaid = 'n';

        globalAppSettings.initApplication();
        statusMessage = "";
        sayMarriage20 = r.getString(R.string.b20a_text);
        sayMarriage20 = globalAppSettings.getLocaleStringRes(R.string.b20a_text, context);
        sayMarriage40 = r.getString(R.string.b20b_text);
        sayMarriage40 = globalAppSettings.getLocaleStringRes(R.string.b20b_text, context);
    }

    /**
     * constructor of game
     * @param c Context
     * @param whichPlayerStarts PLAYERDEF who starts
     */
    public Game(Context c, PLAYERDEF whichPlayerStarts) {
        this(c);
        whoStarts = whichPlayerStarts;
        playersTurn = (whoStarts == PLAYERDEF.COMPUTER) ? false : true;

        mqueue.insert(r.getString(R.string.creating_players));
        gambler = new Player( context, PLAYERDEF.HUMAN, playersTurn);
        gambler.points = 0;
        computer = new Player(context,  PLAYERDEF.COMPUTER, !playersTurn);
        computer.points = 0;

        mergeCards();
    }

    /**
     * Game constructor
     * @param c Context
     * @param bGame blue print to clone / copy
     */
    public Game(Context c, Game bGame) {
        this(c);
        if (bGame != null) {
            globalAppSettings = (GlobalAppSettings) c;
            context = c;
            r = c.getResources();
            isGame = bGame.isGame;

            atouChanged = bGame.atouChanged;
            playersTurn = bGame.playersTurn;
            colorHitRule = bGame.colorHitRule;
            isClosed = bGame.isClosed;
            isReady = bGame.isReady;
            shouldContinue = bGame.shouldContinue;
            a20 = bGame.a20;
            b20 = bGame.b20;
            bChange = bGame.bChange;

            schnapState = bGame.schnapState;
            atouColor = bGame.atouColor;
            said = bGame.said;
            csaid = bGame.csaid;

            index = bGame.index;
            movs = bGame.movs;
            phoneDirection = bGame.phoneDirection;
            inGame = bGame.inGame;
            tournamentPoints = bGame.tournamentPoints;

            sayMarriage20 = bGame.sayMarriage20;
            sayMarriage40 = bGame.sayMarriage40;
            statusMessage = bGame.statusMessage;

            playedOut = bGame.playedOut;
            playedOut0 = bGame.playedOut0;
            playedOut1 = bGame.playedOut1;
            gambler = bGame.gambler;
            computer = bGame.computer;
            whoStarts = bGame.whoStarts;

            set = new Card[20];
            for (int ij = 0; ij < 19; ij++) {
                set[ij] = new Card(bGame.set[ij], c);
            }
        }
    }

    /**
     * gets a positive random number
     * @param modulo modulo %
     * @param generate true for generate a new Random, false for use existing one
     * @return random as int
     */
    int getRandom(int modulo, boolean generate) {
        int rand = 0;
        if (random == null || generate)
            random = new Random();
        if ((rand = random.nextInt()) < 0)
            rand = -rand;

        modulo = (modulo < 2) ? 20 : modulo;
        rand %= modulo;

        return rand;
    }

    /**
     * gets a random number modulo 20
     * @return random as int
     */
    int getRandom() {
        return getRandom(20, false);
    }

    /**
     * mergeCards - function for merging cards
     */
    public void mergeCards() {
    	int i, k, j, l, tmp;
        
        mqueue.insert(r.getString(R.string.merging_cards));
        k = getRandom(32, true);

        for (i = 0; i < k + 20; i++) {
            j = getRandom();
            l = getRandom();

            if (l == j) {
                if (l != 0)
                    j = 0;
                else // if (l == 0)
                    l = (i + 1)%20;
            }

            tmp = inGame[l];
            inGame[l] = inGame[j];
            inGame[j]= tmp;
        }

        set[19] = new Card(inGame[19], context);
        set[19].setAtou();
        mqueue.insert(r.getString(R.string.atou_is, set[19].getName()));
        this.atouColor = set[19].getCardColor();  // set[19].getCharColor();

        for (i = 0; i < 19; i++) {
            set[i] = new Card(inGame[i], this.atouInGame(), context);
            if (i < 5) {
                gambler.assignCard(set[i]);
            } else if (i < 10) {
                computer.assignCard(set[i]);
            }
        }

        gambler.sortHand();
        computer.sortHand();
        schnapState = SCHNAPSTATE.GAME_STARTED;
	}

    /**
     * destructor  it really destroys a game
     */
	public void destroyGame() {
        isGame = false;
        computer.stop();
        gambler.stop();
        gambler = null;
        computer = null;
        playedOut = null;
        playedOut0 = globalAppSettings.cardEmpty();
        playedOut1 = globalAppSettings.cardEmpty();;
        schnapState = SCHNAPSTATE.MERGING_CARDS;
        for (int i = 0; i < 20; i++) {
	        set[i] = null;
	        inGame[i] = i;
	    }
	}

    /**
     * stopGame - stops softley a game
     * @param winner PLAYERDEF who won game
     */
    public void stopGame(PLAYERDEF winner) {
        isGame = false;
        atouColor = CARDCOLOR.NONE;
       
        colorHitRule = false;
        isClosed = false;
        playedOut = new Card(context); // new Card(masterApplet, -1);
        for (int i = 0; i < 5; i++) {
            if (gambler != null && gambler.hand != null)
	            gambler.hand[i] = new Card(context); // new card(masterApplet, -1);
            if (computer != null && computer.hand != null)
                computer.hand[i] = playedOut;
        }
        whoWon = winner;
        schnapState = SCHNAPSTATE.NONE;
        mqueue.insert(context.getResources().getString(R.string.ending_game));
	}

    public int getTournamentPoints(PLAYERDEF winner) {
        tournamentPoints = 1;
        whoWon = winner;
        if (whoWon == PLAYERDEF.HUMAN) {
            if (computer.points == 0)
                tournamentPoints = 3;
            else if (computer.points < 33)
                tournamentPoints = 2;
            else
                tournamentPoints = 1;
        }
        if (whoWon == PLAYERDEF.COMPUTER) {
            if (gambler.points == 0)
                tournamentPoints = 3;
            else if (gambler.points < 33)
                tournamentPoints = 2;
            else
                tournamentPoints = 1;
        }
        return tournamentPoints;
    }

    /**
     * change Atou Card in game
     * @param aPlayer player, that changes Atou Card
     */
    public void changeAtou(Player aPlayer) {
        int cardidx;
        Card tmpCard;
        if (atouChanged || ((cardidx = aPlayer.canChangeAtou()) < 0))
            return ;

        tmpCard = aPlayer.hand[cardidx];
        aPlayer.hand[cardidx] = set[19];
        set[19] = tmpCard;

        aPlayer.sortHand();
        atouChanged = true;
    }

    /**
     * isAtouChangable - checks, if a player can change Atou Card
     * @param aPlayer player, that should change Atou
     * @return true, if player can change, otherwise false
     */
    public boolean isAtouChangable(Player aPlayer) {
        if (atouChanged ||
            schnapState == SCHNAPSTATE.GAME_CLOSED ||
            schnapState == SCHNAPSTATE.TALON_ONE_REMAINS ||
            schnapState == SCHNAPSTATE.TALON_CONSUMED)
            return false;
        if (aPlayer.canChangeAtou() >= 0) return true;
        return false;
    }

    /**
     * checkPoints
     * @param ccard undex of computer habd
     * @return points from the current hit, players points a positive vvalue, computer pinnts as negative value
     */
	public int checkPoints(int ccard) {
	    int tmppoints;
	    if (playersTurn) {
            if (playedOut.hitsCard(computer.hand[ccard],true)) {
                playersTurn = true;
                tmppoints = playedOut.value + computer.hand[ccard].value;
                gambler.points += tmppoints;
                mqueue.insert(r.getString(R.string.your_hit_points, String.valueOf(tmppoints)));

                return tmppoints;
            } else {
                playersTurn = false;
                tmppoints = playedOut.value + computer.hand[ccard].value;
                computer.points += tmppoints;
                mqueue.insert(r.getString(R.string.computer_hit_points, String.valueOf(tmppoints)));

                return (-tmppoints);
            }
        } else {
            if (computer.hand[ccard].hitsCard(playedOut,true)) {
                playersTurn = false;
                tmppoints = playedOut.value + computer.hand[ccard].value;
                computer.points += tmppoints;
                mqueue.insert(r.getString(R.string.computer_hit_points,String.valueOf(tmppoints)));

                return (-tmppoints);
            } else {
                playersTurn = true;
                tmppoints = playedOut.value + computer.hand[ccard].value;
                gambler.points += tmppoints;
                mqueue.insert(r.getString(R.string.your_hit_points,String.valueOf(tmppoints)));

                return tmppoints;
            }            
        }
	}

    /**
     * assignNewCard assigns new cards to both player & computer
     * @return default 0, 1 if game entered colorGitRule; Players now must follow the suit led
     */
    @Deprecated
	public int assignNewCard() {
	    int retval = 0;
	    if (!colorHitRule) { // (colorHitRule == false)
            if (playersTurn) { 
                gambler.assignCard(set[++index]);                
                computer.assignCard(set[++index]);                 
            } else {
                computer.assignCard(set[++index]);               
                gambler.assignCard(set[++index]);                                
            }
            if (index == 17) {
                schnapState = SCHNAPSTATE.TALON_ONE_REMAINS;
                atouChanged = true;
            }
            if (index == 19) {
                schnapState = SCHNAPSTATE.TALON_CONSUMED;
                atouChanged = true;
                retval = 1;
                colorHitRule = true;
            }
        } else {
            movs++;
        }
        computer.sortHand();  
        gambler.sortHand();
       
        return retval;
	}

    /**
     * assignNextCard assigns the next card
     * @return true, if the last card in talon has been assigned, otherwise false
     */
	public boolean assignNextCard(Card assignedCard) {
        boolean lastCard = false;
        if (!colorHitRule) {
            if (playersTurn) {
                assignedCard = set[++index];
                gambler.assignCard(assignedCard);
                computer.assignCard(set[++index]);
            } else {
                computer.assignCard(set[++index]);
                assignedCard = set[++index];
                gambler.assignCard(assignedCard);
            }
            if (index == 17) {
                schnapState = SCHNAPSTATE.TALON_ONE_REMAINS;
                atouChanged = true;
            }
            if (index == 19) {
                schnapState = SCHNAPSTATE.TALON_CONSUMED;
                atouChanged = true;
                lastCard = true;
                colorHitRule = true;
            }
        } else {
            assignedCard = null;
            movs++;
        }
        computer.sortHand();
        gambler.sortHand();

        return lastCard;
    }

    /**
     * closeGame - sets SCHNAPSTATE.GAME_CLOSED for that game
     */
    public void closeGame(PLAYERDEF whoCloses) {

        schnapState = SCHNAPSTATE.GAME_CLOSED;
        isClosed = true;
        colorHitRule = true;

        if (whoCloses  == PLAYERDEF.HUMAN) {
            this.gambler.hasClosed = true;
            statusMessage = context.getString(R.string.player_closed_game);
            statusMessage = globalAppSettings.getLocaleStringRes(R.string.player_closed_game, context);
        }
        if (whoCloses  == PLAYERDEF.COMPUTER) {
            this.computer.hasClosed = true;
            statusMessage = context.getString(R.string.computer_closed_game);
            statusMessage = globalAppSettings.getLocaleStringRes(R.string.computer_closed_game, context);
        }

        if (atouChanged) {
            atouChanged = true;
        }
    }

    /**
     * computerStarts() implementation of a move, where computer sraers (that move)
     * @return card index of computer hand, tgar opebs the current move
     */
    public int computerStarts() {

        int i; int j = 0; int mark = 0;
        computer.playerOptions = 0; // reset computer player options

        //region changeAtoi
        if (isAtouChangable(computer)) {
            computer.playerOptions += PLAYEROPTIONS.CHANGEATOU.getValue();
            changeAtou(computer);
            mqueue.insert(r.getString(R.string.computer_changes_atou));
        }
        //endregion

        int cBestCloseCard = -1;
        //region has20_has40
        if ((i = computer.hasPair()) > 0) {
            // if ((i > 1) && (computer.pairs[1] != null && computer.pairs[1].atou))
            if ((i > 1) && (computer.handpairs[1] == this.atouInGame()))
                mark = 1;

            // TODO: Computer closes game
            int addPoints = (computer.handpairs[mark] == this.atouInGame()) ? 52 : 33;
            if (!this.isClosed && !this.colorHitRule && schnapState == SCHNAPSTATE.GAME_STARTED &&
                (computer.points + addPoints >= 66)) {
                for (i = 0; i < 5; i++) {
                    if (!computer.hand[i].isValidCard())
                        continue;
                    else if (computer.hand[i].cardValue.getValue() >= CARDVALUE.TEN.getValue()) {
                        int bestIdx = gambler.preferedInColorHitsContext(computer.hand[i]);
                        if (gambler.hand[bestIdx].isValidCard() &&
                                gambler.hand[bestIdx].getCardColor() == computer.hand[i].getCardColor() &&
                                gambler.hand[bestIdx].getValue() < computer.hand[i].getValue()) {
                            cBestCloseCard = i;
                            break;
                        }
                    }
                }
                if (cBestCloseCard > -1) {
                    computer.playerOptions += PLAYEROPTIONS.PLAYSCARD.getValue();
                    computer.playerOptions += PLAYEROPTIONS.CLOSESGAME.getValue();
                    return cBestCloseCard;
                }
            }

            for (j = 0; j < 5; j++) {
                if ((computer.hand[j].color == computer.handpairs[mark]) &&
                    (computer.hand[j].getValue() > 2) && (computer.hand[j].getValue() < 5)) {

                    computer.playerOptions += PLAYEROPTIONS.SAYPAIR.getValue();
                    csaid = computer.handpairs[mark];
                    mqueue.insert(r.getString(R.string.computer_says_pair, printColor(csaid)));

                    if (computer.hand[j].isAtou())
                        computer.points += 40;
                    else
                        computer.points += 20;

                    if (computer.points > 65) {
                        String andEnough = r.getString(R.string.twenty_and_enough);
                        andEnough = globalAppSettings.getLocaleStringRes(R.string.twenty_and_enough, context);
                        if (computer.hand[j].isAtou()) {
                            andEnough = r.getString(R.string.fourty_and_enough);
                            andEnough = globalAppSettings.getLocaleStringRes(R.string.fourty_and_enough, context);
                        }

                        computer.playerOptions += PLAYEROPTIONS.ANDENOUGH.getValue();
                        mqueue.insert(andEnough +  " " + r.getString(R.string.computer_has_won_points, String.valueOf(computer.points)));
                    } else {
                        computer.playerOptions += PLAYEROPTIONS.PLAYSCARD.getValue();
                    }

                    return j;
                }
            }
        }
        //endregion

        computer.playerOptions += PLAYEROPTIONS.PLAYSCARD.getValue();
        // TODO: Computer closes game
        if (!this.isClosed && !this.colorHitRule && (computer.points + 12 >= 66) &&
                schnapState == SCHNAPSTATE.GAME_STARTED) {
            for (i = 0; i < 5; i++) {
                if (!computer.hand[i].isValidCard())
                    continue;
                else if (computer.hand[i].cardValue.getValue() >= CARDVALUE.TEN.getValue()) {
                    int bestIdx = gambler.preferedInColorHitsContext(computer.hand[i]);
                    if (gambler.hand[bestIdx].isValidCard() &&
                            gambler.hand[bestIdx].getCardColor() == computer.hand[i].getCardColor() &&
                            gambler.hand[bestIdx].getValue() < computer.hand[i].getValue()) {
                        cBestCloseCard = i;
                        break;
                    }
                }
            }
            if (cBestCloseCard > -1) {
                computer.playerOptions += PLAYEROPTIONS.CLOSESGAME.getValue();
                return cBestCloseCard;
            }
        }

        //region colorHitRule
        if (colorHitRule) {
            mark = 0;

            for (i = 0; i < 5; i++) {
                if (computer.hand[i].isValidCard()) {

                    if (!computer.hand[i].isAtou() && computer.hand[i].cardValue.getValue() >= CARDVALUE.TEN.getValue()) {

                        int bestIdx = gambler.preferedInColorHitsContext(computer.hand[i]);

                        if (gambler.hand[bestIdx].isValidCard() && !gambler.hand[bestIdx].isAtou() &&
                                gambler.hand[bestIdx].getCardColor() == computer.hand[i].getCardColor() &&
                                gambler.hand[bestIdx].getValue() < computer.hand[i].getValue()) {
                            return i;
                        }
                    }
                }
            }

            for (i = 0; i < 5; i++) {
                if (computer.hand[i].isValidCard()) {

                    int bestIdx = gambler.preferedInColorHitsContext(computer.hand[i]);

                    if (gambler.hand[bestIdx].isValidCard() &&
                            gambler.hand[bestIdx].getCharColor() == computer.hand[i].getCharColor() &&
                            gambler.hand[bestIdx].getValue() < computer.hand[i].getValue()) {
                        return i;
                    }
                }
            }

            for (i = 0; i < 5; i++) {
                if (computer.hand[i].isValidCard()) {
                    return i;
                }
            }
        }
        //endregion

        int min = 10, max = 1, minIdx = -1, maxIdx = -1;
        Card maxCard = null;
        for (i = 0; i < 5; i++)
        {
            if (computer.hand[i].isValidCard())
            {
                if (!computer.hand[i].isAtou())
                {
                    if (computer.hand[i].cardValue.getValue() < min)
                    {
                        minIdx = i;
                        min = computer.hand[i].cardValue.getValue();
                    }
                    if (computer.hand[i].cardValue.getValue() > max)
                    {
                        maxIdx = i;
                        max = computer.hand[i].cardValue.getValue();
                        if (computer.hand[i].cardValue == CARDVALUE.ACE)
                            return maxIdx;
                    }
                }
            }
        }

        if (minIdx >= 0)
            return minIdx;
        if (maxIdx >= 0)
            return maxIdx;

        minIdx = -1; maxIdx = -1; min = 12; max = 1;
        for (i = 0; i < 5; i++)
        {
            if (computer.hand[i].isValidCard())
            {
                if (computer.hand[i].cardValue.getValue() < min)
                {
                    minIdx = i;
                    min = computer.hand[i].cardValue.getValue();
                }
                if (computer.hand[i].cardValue.getValue() > max && computer.hand[i].isAtou())
                {
                    maxIdx = i;
                    maxCard = computer.hand[i];
                    max = computer.hand[i].cardValue.getValue();
                }
            }
        }

        if (maxIdx >= 0)
            return maxIdx;

        return minIdx;
    }

    /**
     * computersAnswer implementation of computer answr  move)
     * @return card index of computer hand, that was played for answering
     */
    public int computersAnswer() {

        int i = 0, j = 0;
        // String c_array = "Computer ARRAY: ";

        //region colorHitRule
        if (colorHitRule) {
            i = computer.preferedInColorHitsContext(this.playedOut);
            return(i);
        }
        //endregion

        int tmpPair = computer.hasPair();
        for (i = 0; i < 5; i++) {
            if (computer.hand[i].hitsCard(playedOut,false)) {
                if (!computer.hand[i].isAtou() &&
                    computer.hand[i].cardValue.getValue() > CARDVALUE.KING.getValue()) {
                    return i;
                }
                if (playedOut.getValue() > CARDVALUE.KING.getValue() &&
                    computer.hand[i].isAtou() &&
                    computer.hand[i].cardValue.getValue() > CARDVALUE.KING.getValue()) {
                    return i;                
                }
            }
        }

        int min = 12; int minIdx = -1;
        for (i = 0; i < 5; i++) {
            if (computer.hand[i].isValidCard()) {
                if (computer.hand[i].getValue() < min) {
                    minIdx = i;
                    min = computer.hand[i].getValue();
                }
            }
        }

        return minIdx;
    }

    /**
     * printColor prinrs tbe full bame og color
     * @param ch cgar color
     * @return printed name
     */
    public String printColor(char ch) {
        switch(ch) {
            case 'k': // return context.getString(R.string.color_k);
                return globalAppSettings.getLocaleStringRes(R.string.color_k, context);
            case 'h': // return context.getString(R.string.color_h);
                return globalAppSettings.getLocaleStringRes(R.string.color_h, context);
            case 't': // return context.getString(R.string.color_t);
                return globalAppSettings.getLocaleStringRes(R.string.color_t, context);
            case 'p': // return context.getString(R.string.color_p);
                return globalAppSettings.getLocaleStringRes(R.string.color_p, context);
            default: break;
        }
        return "NoColor";
    }

    /**
     * atouInGame
     * @return char for CARDCOLOR of atou in game
     */
    public char atouInGame() {
        return (char)atouColor.getChar();
    }


    /**
     * insertMsg - inserts msg into internal message queue
     * @param msg String to insert
     */
    public void  insertMsg(String msg) {
        mqueue.insert(msg);
    }

    /**
     * fetchMsg - fetches msg from internal message queue,
     * @return String fetched message
     */
    public String fetchMsg() {
        return mqueue.fetch();
    }

    /**
     * inner class MessageQueue
     */
    static class MessageQueue {
	    StringBuffer qbuffer;
	    int qindex;
	    int qcount;

        /**
         * constructor of inner MessageQueue
         */
	    public MessageQueue() {
            super();
            clear();
	    }

        /**
         * clear messages in inner MessageQueue
         */
	    public void clear() {
	        qbuffer = new StringBuffer();
	        qindex = 0;
	        qcount = 0;
	    }

        /**
         * insert message into inner MessageQueue
         */
	    public void insert (String mes) {
	        qcount++;
            // qbuffer.append(">" + mes + "\n");
	        qbuffer.append(qcount + "->" + mes + "\n");
	    }

        /**
         * fetch messages from inner MessageQueue
         */
	    public String fetch() {
	        String retVal = qbuffer.toString().substring(qindex);
	        qindex = qbuffer.length();
	        return retVal;
	    }
	    
	    public String history() {
	        return (qbuffer.toString());
	    }
	}
	    
}
