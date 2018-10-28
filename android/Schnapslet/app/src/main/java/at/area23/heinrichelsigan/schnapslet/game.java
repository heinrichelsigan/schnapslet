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
import java.net.*;
import java.util.*;
import android.content.res.Resources;
import android.content.Context;

/**
 * Game class represents a single game.
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class Game {
    volatile boolean isGame = false;   // a Game is running
    char atouInGame = 'n';             // color that is atou in this game
    boolean atouChanged = false;      // Atou allready changed
    boolean playersTurn = true;      // Who's playing
    char said = 'n';
    char csaid = 'n';
    boolean colorHitRule = false;
    boolean isClosed = false;
    boolean shouldContinue = false;
    Card playedOut, emptyTmpCard;
    int index = 9;
    int movs = 0;
    int[] inGame = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                    10,11,12,13,14,15,16,17,18,19 };
    Card[] set = new Card[20];
    MessageQueue mqueue = new MessageQueue();
    Player gambler;
    Player computer;
	// java.applet.Applet masterApplet = null;
    Resources r;
    Context context;

    /**
     *  onstructor
     * @param c current context
     */
    public Game(Context c) {
        super();
        isGame = true;
        atouChanged = false;
        playersTurn = true;
        colorHitRule = false;
        isClosed = false;
        shouldContinue = false;

        this.context = c;
        this.r = c.getResources();

        mqueue.clear();
        mqueue.insert(r.getString(R.string.newgame_starts));

        emptyTmpCard = new Card(-2, c.getApplicationContext());

        set = new Card[20];
        for (int i = 0; i < 20; i++) {
            set[i] = new Card(context); //new Card(applet, -1);
            inGame[i] = i;
        }

        index = 9;
        said = 'n';
        csaid = 'n';
        // mqueue.insert(r.getString(R.string.creating_players));
        // gambler = new Player(true);
        gambler = new Player(true, context);
        gambler.points = 0;
        // computer = new Player(false);
        computer = new Player(false, context);
        computer.points = 0;

        mergeCards();
    }

    /**
     * mergeCards - function for merging cards
     */
    public void mergeCards() {
    	int i, k, j, l, tmp;
        
        // mqueue.insert(r.getString(R.string.merging_cards));

        Random rand = new Random();
	    if ((k = rand.nextInt()) < 0)
	        k = 0 - k;
	    k = k%32;

        for (i = 0; i < k + 20; i++) {

            // j = ((j = rand.nextInt()) < 0) ? (0 - j)%20 : j%20;
            if ((j = rand.nextInt()) < 0)
                j = 0 - j;
            j %= 20;

            if ((l = rand.nextInt()) < 0)
                l = 0 - l;
            l = l%20;

            if (l == j) {
                // l = (l != 0) ? 0 : (i + 1)%20;
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
        this.atouInGame = set[19].getColor();

        for (i = 0; i < 19; i++) {
            set[i] = new Card(inGame[i], this.atouInGame, context);
            if (i < 5) {
                gambler.assignCard(set[i]);
            } else if (i < 10) {
                computer.assignCard(set[i]);
            }
        }

        gambler.sortHand();
        computer.sortHand();
	}

    /**
     * destructor . ut reakky destrpys a gam1e
     */
	public void destroyGame() {
        isGame = false;
        computer.stop();
        gambler.stop();
        gambler = null;
        computer = null;
        playedOut = null;        
        for (int i = 0; i < 20; i++) {
	        set[i] = null;
	        inGame[i] = i;
	    }
	}

    /**
     * stopGamr - stops softleey a game
     */
    public void stopGame() {
        isGame = false;
        atouInGame = 'n';
       
        colorHitRule = false;
        isClosed = false;
        playedOut = new Card(context); // new Card(masterApplet, -1);
        for (int i = 0; i < 5; i++) {
	        gambler.hand[i] = new Card(context); // new card(masterApplet, -1);
	        computer.hand[i] = playedOut;
        }                     
        // mqueue.insert(context.getResources().getString(R.string.ending_game));
        
	}

    /**
     * change Atou Card in game
     * @param aPlayer player, that changes Atou Card
     */
    public void changeAtou(Player aPlayer) {
        int cardidx;
        Card tmpCard;
        if ((cardidx = aPlayer.canChangeAtou()) < 0) return ;

        tmpCard = aPlayer.hand[cardidx];
        aPlayer.hand[cardidx] = set[19];
        set[19] = tmpCard;

        computer.playerOptions += PLAYEROPTIONS.CHANGEATOU.getValue();
        aPlayer.sortHand();
        atouChanged = true;
    }

    /**
     * Checks, if a player can change Atou Card
     * @param aPlayer player, that should change Atou
     * @return true, if player can change, otherwise false
     */
    public boolean atouIsChangable(Player aPlayer) {
        if (atouChanged) return false;
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
            if (playedOut.hitsCard(computer.hand[ccard],true) == true) {
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
	public int assignNewCard() {
	    int retval = 0;
	    if (colorHitRule == false) {
            if (playersTurn) { 
                gambler.assignCard(set[++index]);                
                computer.assignCard(set[++index]);                 
            } else {
                computer.assignCard(set[++index]);               
                gambler.assignCard(set[++index]);                                
            }
            if (index == 17)
                atouChanged = true;
            if (index == 19) {
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
     * @return the assigned Card for player
     */
	public int assignNextCard(Card assignedCard) {
        int retval = 0;
        if (colorHitRule == false) {
            if (playersTurn) {
                assignedCard = set[++index];
                gambler.assignCard(assignedCard);
                computer.assignCard(set[++index]);
            } else {
                computer.assignCard(set[++index]);
                assignedCard = set[++index];
                gambler.assignCard(assignedCard);
            }
            if (index == 17)
                atouChanged = true;
            if (index == 19) {
                retval = 1;
                colorHitRule = true;
            }
        } else {
            assignedCard = null;
            movs++;
        }
        computer.sortHand();
        gambler.sortHand();

        return retval;
    }

    /**
     * computerStarts() implementation of a move, where computer sraers (that move)
     * @return card index of computer hand, tgar opebs the current move
     */
    public int computerStarts() {

        computer.playerOptions = 0;
        int i; int j = 0; int mark = 0;

        //region  changeAtoi
        if (atouIsChangable(computer)) {
            changeAtou(computer);
            mqueue.insert(r.getString(R.string.computer_changes_atou));
        }
        //endregion

        //region  has20_has40
        if ((i = computer.has20()) > 0) {
            // if ((i > 1) && (computer.pairs[1] != null && computer.pairs[1].atou))
            if ((i > 1) && (computer.handpairs[1] == this.atouInGame))
                mark = 1;

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
                        if (computer.hand[j].isAtou()) {
                            andEnough = r.getString(R.string.fourty_and_enough);
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

        //region colorHitRule
        if (colorHitRule) {
            mark = 0;

            for (i = 0; i < 5; i++) {
                if (computer.hand[i].isValidCard()) {

                    if (!computer.hand[i].isAtou() && computer.hand[i].cardValue.getValue() >= CARDVALUE.TEN.getValue()) {

                        int bestIdx = gambler.bestInColorHitsContext(computer.hand[i]);

                        if (gambler.hand[bestIdx].isValidCard() && !gambler.hand[bestIdx].isAtou() &&
                                gambler.hand[bestIdx].getColor() == computer.hand[i].getColor() &&
                                gambler.hand[bestIdx].getValue() < computer.hand[i].getValue()) {
                            return i;
                        }
                    }
                }
            }

            for (i = 0; i < 5; i++) {
                if (computer.hand[i].isValidCard()) {

                    int bestIdx = gambler.bestInColorHitsContext(computer.hand[i]);

                    if (gambler.hand[bestIdx].isValidCard() &&
                            gambler.hand[bestIdx].getColor() == computer.hand[i].getColor() &&
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

        int min = 12; int c_idx = 0;
        for (i = 0; i < 5; i++) {
            if (computer.hand[i].isValidCard()) {
                if (!computer.hand[i].isAtou()) {
                    if (computer.hand[i].getValue() < min) {
                        c_idx = i;
                        min = computer.hand[i].getValue();
                    }
                }
            }
        }

        return c_idx;
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
            i = computer.bestInColorHitsContext(this.playedOut);
            // for (j = 0; j < 5; j++) {
            //     c_array = c_array + computer.colorHitArray[j] + " ";
            // }

            // mqueue.insert(c_array);
            // mqueue.insert("i = " + i +  " Computer,hand[" + i + "] = " + computer.hand[i].getName() + " !");
            // mqueue.insert("Computer Hand: " + computer.showHand());

            return(i);
        }
        //endregion

        for (i = 0; i < 5; i++) {
            if (computer.hand[i].hitsCard(playedOut,false)) {
                if (!computer.hand[i].isAtou()) {
                    return i;
                }
                if (playedOut.getValue() > CARDVALUE.KING.getValue()) {
                    return i;                
                }
            }
        }

        int min = 12; int c_idx = 0;
        for (i = 0; i < 5; i++) {
            if (computer.hand[i].isValidCard()) {
                if (computer.hand[i].getValue() < min) {
                    c_idx = i;
                    min = computer.hand[i].getValue();
                }
            }
        }

        return c_idx;
    }

    /**
     * printColor prinrs tbe full bame og color
     * @param ch cgar color
     * @return printed name
     */
    public String printColor(char ch) {
        switch(ch) {
            case 'k': return "Karo";
            case 'h': return "Herz";
            case 't': return "Treff";
            case 'p': return "Pik";
            default: break;
        }
        return "NoColor";
    }

    /**
     * inner class MessageQueue
     */
    class MessageQueue {
	    StringBuffer qbuffer;
	    int qindex;
	    int qcount;
	    
	    public MessageQueue() {
            super();
            clear();
	    }
	    
	    public void clear() {
	        qbuffer = new StringBuffer();
	        qindex = 0;
	        qcount = 0;
	    }
	    
	    public void insert (String mes) {
	        qcount++;
	        qbuffer.append(">" + mes + "\n");
	        // qbuffer.append(qcount + "->" + mes + "\n");
	    }
	    
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
