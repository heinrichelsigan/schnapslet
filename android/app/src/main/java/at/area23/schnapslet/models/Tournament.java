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

import android.content.Context;
import android.content.res.Resources;
import android.graphics.Point;

import java.net.URL;
import java.util.HashSet;
import java.util.Locale;
import java.util.Random;
import java.util.Set;

import at.area23.schnapslet.*;
import at.area23.schnapslet.models.*;
import at.area23.schnapslet.constenum.*;

/**
 * Tournament class represents a set of games that ends with a fat point (Bummerl) or a taylor (Schneider)
 *
 * @see <a href="https://github.com/heinrichelsigan/schnapslet/wiki</a>
 */
public class Tournament extends SchnapsAtom {

	public int gamblerTPoints = 7;
	public int computerTPoints = 7;
	
	public Set<Point> tHistory = new HashSet<>();
   
    public PLAYERDEF nextGameGiver = PLAYERDEF.COMPUTER;


	/**
	 * Constructor of Tournament
	 * @param c Context
	 */
    public Tournament(Context c) {
        super(c);
        this.name = "aTournament_" + Constants.getDateString();
		gamblerTPoints = 7;
		computerTPoints = 7;

		tHistory = new HashSet<>();
		Point ptStart = new Point(gamblerTPoints, computerTPoints);
		tHistory.add(ptStart);

		Random random = new Random();
		int rand = random.nextInt();
		rand = (rand >= 0) ? rand : -rand;
		nextGameGiver = (rand % 2 == 0) ? PLAYERDEF.HUMAN : PLAYERDEF.COMPUTER;
    }

	/**
	 * Constructor of Tournament
	 * @param c Context
	 * @param nextGiver PLAYERDEF who's next giver
	 */
    public Tournament(Context c, PLAYERDEF nextGiver) {
        this(c);
        this.nextGameGiver = nextGiver;
    }
	
	/**
     * getTournamentWinner
     * @return PLAYERDEF.HUMAN, 	if gambler won tournement, 
	 *         PLAYERDEF.COMPUTER, 	if computer won tournement, 
	 *         PLAYERDEF.UNKNOWN, 	if tournement is still running
     */
	public PLAYERDEF getTournamentWinner() {
		if (computerTPoints <= 0 && gamblerTPoints > 0)
			return PLAYERDEF.COMPUTER;
		if (gamblerTPoints <= 0 && computerTPoints > 0)
			return PLAYERDEF.HUMAN;
		return PLAYERDEF.UNKNOWN;
	}
	
		
	/**
     * hasTaylor
     * @return true, if opposite of winner made no game in entire Tournament
	 */
	public boolean hasTaylor() {
		return ((getTournamentWinner() == PLAYERDEF.COMPUTER && gamblerTPoints == 7) ||
				(getTournamentWinner() == PLAYERDEF.HUMAN && computerTPoints == 7));
	}
	
	/**
     * getNextGameStarter
     * @return PLAYERDEF.HUMAN, 	if nextGameGiver is computer
	 *         PLAYERDEF.COMPUTER, 	if nextGameGiver is gambler
	 *         PLAYERDEF.UNKNOWN, 	in case of assertion
     */
	public PLAYERDEF getNextGameStarter() {
		if (nextGameGiver == PLAYERDEF.HUMAN)
			return PLAYERDEF.COMPUTER;
		if (nextGameGiver == PLAYERDEF.COMPUTER)
			return PLAYERDEF.HUMAN;
		return PLAYERDEF.UNKNOWN; // TODO: ReThink Unknown never occurred state
	}
	
	/**
     * writerPointsRotateGiver
     * 	write down Tournament points of last game, rotate giver and starter
     */
	public void writerPointsRotateGiver()
	{
		Point ptStart = new Point(gamblerTPoints, computerTPoints);
		tHistory.add(ptStart);
		if (nextGameGiver == PLAYERDEF.COMPUTER)
			nextGameGiver = PLAYERDEF.HUMAN;
		else if (nextGameGiver == PLAYERDEF.HUMAN)
			nextGameGiver = PLAYERDEF.COMPUTER;
		else if (nextGameGiver == PLAYERDEF.UNKNOWN)
			throw new IllegalStateException("Unknown game state to determine next giver");
	}

	/**
	 * addPointsRotateGiver
	 * @param tournamentPoints substracting last game points from tounnaments table
	 * @param whoWon PLAYERDEF enum for PLAYERDEF.HUMAN or PLAYERDEF.COMPUTER
	 */
	public void addPointsRotateGiver(int tournamentPoints, PLAYERDEF whoWon) {
		// substructs points from down playing tournament
		if (whoWon == PLAYERDEF.HUMAN)
			gamblerTPoints -= tournamentPoints;
		else if (whoWon == PLAYERDEF.COMPUTER)
			computerTPoints -= tournamentPoints;
		// write down tournament table history
		Point ptStart = new Point(gamblerTPoints, computerTPoints);
		tHistory.add(ptStart);
		// rotate next game giver
		nextGameGiver = getNextGameStarter();
	}

	/**
	 * getTournamentsTable
	 * @return full tournaments table as ascii string with new lines
	 */
	public String getTournamentsTable() {
		String tourTable = "P | C\n";
		for (Point tPt : tHistory) {
			tourTable += tPt.x + " | " + tPt.y + "\n";
		}
		PLAYERDEF whoWon = getTournamentWinner();
		if (whoWon != PLAYERDEF.UNKNOWN) {
			if (hasTaylor()) {
				tourTable += (whoWon == PLAYERDEF.HUMAN) ?
						"  | " + Constants.TAYLOR_SYM0 :
						Constants.TAYLOR_SYM2 + " |  ";
			}
		}
		return tourTable;
	}

}
