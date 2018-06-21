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
import at.area23.heinrichelsigan.schnapslet.card;

import java.lang.*;
import java.io.*;
import java.net.*;
import java.util.*;

public class game {
    volatile boolean isGame = false;   // a Game is running
    char atouInGame = 'n';             // color that is atou in this game
    boolean atouChanged = false;      // Atou allready changed
    boolean playersTurn = true;      // Who's playing
    char said = 'n';
    char csaid = 'n';
    boolean colorHitRule = false;
    boolean isClosed = false;
    card playedOut;
    int index = 9;    
    int movs = 0;
    int inGame[] = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                    10,11,12,13,14,15,16,17,18,19 };
    card set[] = new card[20]; 
    messageQueue mqueue = new messageQueue();
    player gambler;
    player computer;
	// java.applet.Applet masterApplet = null;
    
                    
    public game() {
        super();
        isGame = true;
        atouChanged = false;   
        playersTurn = true;
        colorHitRule = false;    
        isClosed = false;
	// }
	
	// public game(java.applet.Applet applet) {
	//	this();
	//	masterApplet = applet;
        mqueue.clear();
        mqueue.insert("Starting new game ...");
        
        card set[] = new card[20];
        for (int i = 0; i < 20; i++) {
	        set[i] = new card(); //new card(applet, -1);
	        inGame[i] = i;
        }      
        
        index = 9;
        said = 'n';
        csaid = 'n';
        mqueue.insert("Creating new players ...");
        gambler = new player(true);
        gambler.points = 0;
        computer = new player(false);
        computer.points = 0;
        mergeCards();
    }
    
    public void mergeCards() {
    	int i, k, j, l, tmp;
        
        mqueue.insert("merging Cards ...");
        Random rand = new Random();
	    if ((k = rand.nextInt()) < 0) k = 0 - k; k = k%32;
        for (i = 0; i < k+20; i++) {
            if ((j = rand.nextInt())< 0) j = 0 - j; j = j%20;
            if ((l = rand.nextInt())< 0) l = 0 - l; l = l%20;
            if (l == j) j =0;
            tmp = inGame[l];
            inGame[l] = inGame[j];
            inGame[j]= tmp;
        }

        // et[19] = new card(masterApplet, inGame[19]);
        set[19] = new card(inGame[19]);
        set[19].setAtou();
        mqueue.insert("Atou ist " + set[19].getName() + " !");
        this.atouInGame = set[19].getColor();
        for (i = 0; i < 19; i++) {
            // set[i] = new card(masterApplet, inGame[i], this.atouInGame);
            set[i] = new card(inGame[i], this.atouInGame);
            if (i < 5) {
                gambler.assignCard(set[i]);
            } else if (i < 10) {
                computer.assignCard(set[i]);
            }
        }
        gambler.sortHand();
        computer.sortHand();
	}
	
    public void destroyGame() {
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

	public void stopGame() {
        isGame = false;
        atouInGame = 'n';
       
        colorHitRule = false;
        isClosed = false;
        playedOut = new card(); // new card(masterApplet, -1);
        for (int i = 0; i < 5; i++) {
	        gambler.hand[i] = new card(); // new card(masterApplet, -1);
	        computer.hand[i] = playedOut;
        }                     
        mqueue.insert("ending game ...");
        
	}

    public void changeAtou(player aPlayer) {
        int cardidx;
        card tmpCard;
        if ((cardidx = aPlayer.canChangeAtou()) < 0) return ;
        tmpCard = aPlayer.hand[cardidx];
        aPlayer.hand[cardidx] = set[19];
        set[19] = tmpCard;
        gambler.sortHand();
        atouChanged = true;
    }
    
    public boolean atouIsChangable(player aPlayer) {
        if (atouChanged) return false;
        if (aPlayer.canChangeAtou()>=0) return true;
        return false;
    }
	
	public int checkPoints(int ccard) {
	    int tmppoints;
	    if (playersTurn) {
            if (playedOut.hitsCard(computer.hand[ccard],true)==true) {
                playersTurn = true;
                tmppoints = playedOut.value+computer.hand[ccard].value;
                gambler.points += tmppoints;
                mqueue.insert("Sie stechen und machen "+tmppoints+" Punkte !");
                return tmppoints;
            } else {
                playersTurn = false;
                tmppoints = playedOut.value+computer.hand[ccard].value;
                computer.points += tmppoints;
                mqueue.insert("Computer sticht und macht: "+tmppoints+" Punkte ");
                return (-tmppoints);
            }
        } else {
            if (computer.hand[ccard].hitsCard(playedOut,true)) {
                playersTurn = false;
                tmppoints=playedOut.value+computer.hand[ccard].value;
                computer.points += tmppoints;
                mqueue.insert("Computer sticht und macht: "+tmppoints+" Punkte ");
                return (-tmppoints);
            } else {
                tmppoints=playedOut.value+computer.hand[ccard].value;
                playersTurn = true;
                gambler.points += tmppoints;
                mqueue.insert("Sie stechen und machen "+tmppoints+" Punkte !");                
                return tmppoints;
            }            
        }
	}

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
            if (index == 17) atouChanged = true;
            if (index == 19) { retval = 1; colorHitRule = true; }
        } else {
            movs++;
        }
        computer.sortHand();  
        gambler.sortHand();
       
        return retval;
	}
    
    public int computerStarts() {
        int min = 12; int c_idx = 0; int i; int j = 0; int mark = 0;

        if ((i = computer.has20()) > 0) {
            if ((i > 1) && (computer.handpairs[1] == this.atouInGame))
                mark = 1;
            for (j = 0; j < 5; j++) {
                if ((computer.hand[j].color == computer.handpairs[mark]) &&
                    (computer.hand[j].getValue() > 2) && (computer.hand[j].getValue() < 5)) {
                    csaid = computer.handpairs[0];
                    if (computer.hand[j].isAtou() == true) computer.points += 40;
                    else computer.points += 20;
                    return j;
                }
            }
        }
        
        if (colorHitRule) {
            mark = 0;
            for (i = 0; i < 5; i++) {
                if (computer.hand[i].isValidCard()) {
                    return i;
                }
            }
        }

        for (i = 0; i < 5; i++) {
            if (computer.hand[i].isValidCard()) {
                if (computer.hand[i].isAtou() == false) {
                    if (computer.hand[i].getValue() < min) {
                        c_idx = i;
                        min = computer.hand[i].getValue();
                    }
                }
            }
        }

        return c_idx;
    }
    
    
    public int computersAnswer() {
        int min = 12; int c_idx = 0; int i, j;
        String c_array = "Computer ARRAY: ";
        if (colorHitRule) {
            i=computer.bestInColorHitsContext(this.playedOut);
            for (j = 0; j < 5; j++) {
                c_array = c_array + computer.colorHitArray[j] + " ";
            }
            mqueue.insert(c_array);
            mqueue.insert("Computer MAX: "+i+" !");
            mqueue.insert("Computer Hand: "+computer.showHand());
            return(i);
        }

        for (i = 0; i < 5; i++) {
            if (computer.hand[i].hitsCard(playedOut,false)) {
                if (computer.hand[i].isAtou() == false) {
                    return i;
                }
                if (playedOut.getValue() > 5) {
                    return i;                
                }
            }
        }

        for (i = 0; i < 5; i++) {
            if (computer.hand[i].isValidCard()) {
                if (computer.hand[i].getValue()<min) {
                    c_idx = i;
                    min=computer.hand[i].getValue();
                }
            }
        }

        return c_idx;
    }
    
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
    
	class messageQueue {
	    StringBuffer qbuffer;
	    int qindex;
	    int qcount;
	    
	    public messageQueue() {
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
	        qbuffer.append(">"+mes+"\n");
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
