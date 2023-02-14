using SchnapsNet.ConstEnum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Web;
using System.Web.UI.WebControls;

namespace SchnapsNet.Models
{
	/// <summary>
	/// Port of class Game
	/// <see cref="https://github.com/heinrichelsigan/schnapslet/wiki"/>
	/// </summary>
	public class Game
	{
		public volatile bool isGame = false;     // a Game is running
		public bool atouChanged = false;         // Atou allready changed
		public bool playersTurn = true;          // Who's playing
		public bool colorHitRule = false;        // Farb und Stichzwang
		public bool isClosed = false;            // game is closed
		public bool isReady = false;              // is ready to play out
		public bool shouldContinue = false;      // should continue the game
		public bool a20 = false;                 // can player announce 1st pair
		public bool b20 = false;                 // can player announce 2nd pair
		public bool bChange = false;             // can player change atou

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

		public String sayMarriage20, sayMarriage40, textMsg;

		// public GlobalAppSettings globalAppSettings;
		public Card[] set = new Card[20];
		public Card playedOut, playedOut0, playedOut1;

		public Player gambler, computer;
		// java.applet.Applet masterApplet = null;
		Random random;
		// Resources r;
		Context context;

		// MessageQueue mqueue = new MessageQueue();

		/**
		 * constructor
		 * @param c current context
		 */
		public Game(Context c)
		{
			// super();
			// globalAppSettings = (GlobalAppSettings) c;
			isGame = true;
			atouChanged = false;
			playersTurn = true;
			colorHitRule = false;
			isClosed = false;
			shouldContinue = false;

			this.context = c;
			// this.r = c.getResources();
			this.schnapState = SCHNAPSTATE.GAME_START;
			// mqueue.clear();
			// mqueue.insert(r.getString(R.string.newgame_starts));

			// playedOut0 = globalAppSettings.cardEmpty();
			// playedOut1 = globalAppSettings.cardEmpty();;

			set = new Card[20];
			for (int i = 0; i < 20; i++)
			{
				set[i] = new Card(context); //new Card(applet, -1);
				inGame[i] = i;
			}

			index = 9;
			said = 'n';
			csaid = 'n';

			textMsg = "";
			sayMarriage20 = JavaResReader.GetValueFromKey("b20a_text", "");
			sayMarriage40 = JavaResReader.GetValueFromKey("b20b_text", "");

			// mqueue.insert(r.getString(R.string.creating_players));
			gambler = new Player(true, context);
			gambler.points = 0;
			computer = new Player(false, context);
			computer.points = 0;

			mergeCards();
		}

		/**
		 * gets a positive random number
		 * @param modulo modulo %
		 * @param generate true for generate a new Random, false for use existing one
		 * @return random as int
		 */
		int getRandom(int modulo, bool generate)
		{
			int rand = 0;
			if (random == null || generate)
				random = new Random();
			if ((rand = random.Next()) < 0)
				rand = -rand;

			modulo = (modulo < 2) ? 20 : modulo;
			rand %= modulo;

			return rand;
		}

		/**
		 * gets a random number modulo 20
		 * @return random as int
		 */
		int getRandom()
		{
			return getRandom(20, false);
		}

		/**
		 * mergeCards - function for merging cards
		 */
		public void mergeCards()
		{
			int i, k, j, l, tmp;

			// mqueue.insert(r.getString(R.string.merging_cards));
			k = getRandom(32, true);

			for (i = 0; i < k + 20; i++)
			{
				j = getRandom();
				l = getRandom();

				if (l == j)
				{
					if (l != 0)
						j = 0;
					else // if (l == 0)
						l = (i + 1) % 20;
				}

				tmp = inGame[l];
				inGame[l] = inGame[j];
				inGame[j] = tmp;
			}

			set[19] = new Card(inGame[19], context);
			set[19].setAtou();
			// mqueue.insert(r.getString(R.string.atou_is, set[19].getName()));
			this.atouColor = set[19].CardColor;  // set[19].getCharColor();

			for (i = 0; i < 19; i++)
			{
				set[i] = new Card(inGame[i], this.atouInGame(), context);
				if (i < 5)
				{
					gambler.assignCard(set[i]);
				}
				else if (i < 10)
				{
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
		public void destroyGame()
		{
			isGame = false;
			computer.stop();
			gambler.stop();
			gambler = null;
			computer = null;
			playedOut = null;
			playedOut0 = new Card(-2, context); // globalAppSettings.cardEmpty();
			playedOut1 = new Card(-2, context); // globalAppSettings.cardEmpty();;
			schnapState = SCHNAPSTATE.MERGING_CARDS;
			for (int i = 0; i < 20; i++)
			{
				set[i] = null;
				inGame[i] = i;
			}
		}

		/**
		 * stopGame - stops softley a game
		 */
		public void stopGame()
		{
			isGame = false;
			atouColor = CARDCOLOR.NONE;

			colorHitRule = false;
			isClosed = false;
			playedOut = new Card(context); // new Card(masterApplet, -1);
			for (int i = 0; i < 5; i++)
			{
				if (gambler != null && gambler.hand != null)
					gambler.hand[i] = new Card(context); // new card(masterApplet, -1);
				if (computer != null && computer.hand != null)
					computer.hand[i] = playedOut;
			}
			schnapState = SCHNAPSTATE.NONE;
			// mqueue.insert(context.getResources().getString(R.string.ending_game));
		}

		/**
		 * change Atou Card in game
		 * @param aPlayer player, that changes Atou Card
		 */
		public void changeAtou(Player aPlayer)
		{
			int cardidx;
			Card tmpCard;
			if ((cardidx = aPlayer.canChangeAtou()) < 0) return;

			tmpCard = aPlayer.hand[cardidx];
			aPlayer.hand[cardidx] = set[19];
			set[19] = tmpCard;

			computer.playerOptions += PLAYEROPTIONS.CHANGEATOU.GetValue();
			aPlayer.sortHand();
			atouChanged = true;
		}

		/**
		 * Checks, if a player can change Atou Card
		 * @param aPlayer player, that should change Atou
		 * @return true, if player can change, otherwise false
		 */
		public bool atouIsChangable(Player aPlayer)
		{
			if (atouChanged) return false;
			if (aPlayer.canChangeAtou() >= 0) return true;
			return false;
		}

		/**
		 * checkPoints
		 * @param ccard undex of computer habd
		 * @return points from the current hit, players points a positive vvalue, computer pinnts as negative value
		 */
		public int checkPoints(int ccard)
		{
			int tmppoints;
			if (playersTurn)
			{
				if (playedOut.hitsCard(computer.hand[ccard], true))
				{
					playersTurn = true;
					tmppoints = playedOut.CardValue.GetValue() + computer.hand[ccard].CardValue.GetValue();

					gambler.points += tmppoints;
					// mqueue.insert(r.getString(R.string.your_hit_points, String.valueOf(tmppoints)));

					return tmppoints;
				}
				else
				{
					playersTurn = false;
					tmppoints = playedOut.CardValue.GetValue() + computer.hand[ccard].CardValue.GetValue();
					computer.points += tmppoints;
					// mqueue.insert(r.getString(R.string.computer_hit_points, String.valueOf(tmppoints)));

					return (-tmppoints);
				}
			}
			else
			{
				if (computer.hand[ccard].hitsCard(playedOut, true))
				{
					playersTurn = false;
					tmppoints = playedOut.CardValue.GetValue() + computer.hand[ccard].CardValue.GetValue();
					computer.points += tmppoints;
					// mqueue.insert(r.getString(R.string.computer_hit_points,String.valueOf(tmppoints)));

					return (-tmppoints);
				}
				else
				{
					playersTurn = true;
					tmppoints = playedOut.CardValue.GetValue() + computer.hand[ccard].CardValue.GetValue();
					gambler.points += tmppoints;
					// mqueue.insert(r.getString(R.string.your_hit_points,String.valueOf(tmppoints)));

					return tmppoints;
				}
			}
		}

		/**
		 * assignNewCard assigns new cards to both player & computer
		 * @return default 0, 1 if game entered colorGitRule; Players now must follow the suit led
		 */
		[Obsolete("assignNewCard is obsolete", false)]
		public int assignNewCard()
		{
			int retval = 0;
			if (!colorHitRule)
			{ // (colorHitRule == false)
				if (playersTurn)
				{
					gambler.assignCard(set[++index]);
					computer.assignCard(set[++index]);
				}
				else
				{
					computer.assignCard(set[++index]);
					gambler.assignCard(set[++index]);
				}
				if (index == 17)
					atouChanged = true;
				if (index == 19)
				{
					retval = 1;
					colorHitRule = true;
				}
			}
			else
			{
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
		public bool assignNextCard(Card assignedCard)
		{
			bool lastCard = false;
			if (!colorHitRule)
			{
				if (playersTurn)
				{
					assignedCard = set[++index];
					gambler.assignCard(assignedCard);
					computer.assignCard(set[++index]);
				}
				else
				{
					computer.assignCard(set[++index]);
					assignedCard = set[++index];
					gambler.assignCard(assignedCard);
				}
				if (index == 17)
				{
					schnapState = SCHNAPSTATE.TALON_ONE_REMAINS;
					atouChanged = true;
				}
				if (index == 19)
				{
					schnapState = SCHNAPSTATE.TALON_CONSUMED;
					lastCard = true;
					colorHitRule = true;
				}
			}
			else
			{
				assignedCard = null;
				movs++;
			}
			computer.sortHand();
			gambler.sortHand();

			return lastCard;
		}

		/**
		 * computerStarts() implementation of a move, where computer sraers (that move)
		 * @return card index of computer hand, tgar opebs the current move
		 */
		public int computerStarts()
		{

			computer.playerOptions = 0;
			int i; int j = 0; int mark = 0;

			//region changeAtoi
			if (atouIsChangable(computer))
			{
				changeAtou(computer);
				// mqueue.insert(r.getString(R.string.computer_changes_atou));
			}
			//endregion

			int cBestCloseCard = -1;
			//region has20_has40
			if ((i = computer.has20()) > 0)
			{
				// if ((i > 1) && (computer.pairs[1] != null && computer.pairs[1].atou))
				if ((i > 1) && (computer.handpairs[1] == this.atouInGame()))
					mark = 1;

				// TODO: Computer closes game
				int addPoints = (computer.handpairs[mark] == this.atouInGame()) ? 52 : 33;
				if (!this.isClosed && !this.colorHitRule && schnapState == SCHNAPSTATE.GAME_STARTED &&
					(computer.points + addPoints >= 66))
				{
					for (i = 0; i < 5; i++)
					{
						if (!computer.hand[i].isValidCard)
							continue;
						else if (computer.hand[i].CardValue.GetValue() >= CARDVALUE.TEN.GetValue())
						{
							int bestIdx = gambler.bestInColorHitsContext(computer.hand[i]);
							if (gambler.hand[bestIdx].isValidCard &&
									gambler.hand[bestIdx].CardColor.GetChar() == computer.hand[i].CardColor.GetChar() &&
									gambler.hand[bestIdx].CardValue.GetValue() < computer.hand[i].CardValue.GetValue())
							{
								cBestCloseCard = i;
								break;
							}
						}
					}
					if (cBestCloseCard > -1)
					{
						computer.playerOptions += PLAYEROPTIONS.PLAYSCARD.GetValue();

						computer.playerOptions += PLAYEROPTIONS.CLOSESGAME.GetValue();
						return cBestCloseCard;
					}
				}

				for (j = 0; j < 5; j++)
				{
					if ((computer.hand[j].CardColor.GetChar() == computer.handpairs[mark]) &&
						(computer.hand[j].CardValue.GetValue() > 2) &&
						(computer.hand[j].CardValue.GetValue() < 5))
					{

						computer.playerOptions += PLAYEROPTIONS.SAYPAIR.GetValue();
						csaid = computer.handpairs[mark];
						// mqueue.insert(r.getString(R.string.computer_says_pair, printColor(csaid)));

						if (computer.hand[j].isAtou)
							computer.points += 40;
						else
							computer.points += 20;

						if (computer.points > 65)
						{
							String andEnough = JavaResReader.GetValueFromKey("twenty_and_enough", "");
							if (computer.hand[j].isAtou)
							{
								andEnough = JavaResReader.GetValueFromKey("fourty_and_enough");

							}

							computer.playerOptions += PLAYEROPTIONS.ANDENOUGH.GetValue();
							// mqueue.insert(andEnough + " " + JavaResReader.GetValueFromKey("computer_has_won_points", "") + computer.points);

						}
						else
						{
							computer.playerOptions += PLAYEROPTIONS.PLAYSCARD.GetValue();
						}

						return j;
					}
				}
			}
			//endregion

			computer.playerOptions += PLAYEROPTIONS.PLAYSCARD.GetValue();
			// TODO: Computer closes game
			if (!this.isClosed && !this.colorHitRule && (computer.points + 12 >= 66) &&
					schnapState == SCHNAPSTATE.GAME_STARTED)
			{
				for (i = 0; i < 5; i++)
				{
					if (!computer.hand[i].isValidCard)
						continue;
					else if (computer.hand[i].CardValue.GetValue() >= CARDVALUE.TEN.GetValue())
					{
						int bestIdx = gambler.bestInColorHitsContext(computer.hand[i]);
						if (gambler.hand[bestIdx].isValidCard &&
							gambler.hand[bestIdx].CardColor.GetChar() == computer.hand[i].CardColor.GetChar() &&
								gambler.hand[bestIdx].CardValue.GetValue() < computer.hand[i].CardValue.GetValue())
						{
							cBestCloseCard = i;
							break;
						}
					}
				}
				if (cBestCloseCard > -1)
				{
					computer.playerOptions += PLAYEROPTIONS.CLOSESGAME.GetValue();
					return cBestCloseCard;
				}
			}

			//region colorHitRule
			if (colorHitRule)
			{
				mark = 0;

				for (i = 0; i < 5; i++)
				{
					if (computer.hand[i].isValidCard)
					{

						if (!computer.hand[i].isAtou && computer.hand[i].CardValue.GetValue() >= CARDVALUE.TEN.GetValue())
						{

							int bestIdx = gambler.bestInColorHitsContext(computer.hand[i]);

							if (gambler.hand[bestIdx].isValidCard && !gambler.hand[bestIdx].isAtou &&
								gambler.hand[bestIdx].CardColor.GetChar() == computer.hand[i].CardColor.GetChar() &&
									gambler.hand[bestIdx].CardValue.GetValue() < computer.hand[i].CardValue.GetValue())
							{
								return i;
							}
						}
					}
				}

				for (i = 0; i < 5; i++)
				{
					if (computer.hand[i].isValidCard)
					{

						int bestIdx = gambler.bestInColorHitsContext(computer.hand[i]);

						if (gambler.hand[bestIdx].isValidCard &&
							gambler.hand[bestIdx].CardColor.GetChar() == computer.hand[i].CardColor.GetChar() &&
								gambler.hand[bestIdx].CardValue.GetValue() < computer.hand[i].CardValue.GetValue())
						{
							return i;
						}
					}
				}


				for (i = 0; i < 5; i++)
				{
					if (computer.hand[i].isValidCard)
					{
						return i;
					}
				}
			}
			//endregion

			int min = 12; int c_idx = 0;
			for (i = 0; i < 5; i++)
			{
				if (computer.hand[i].isValidCard)
				{
					if (!computer.hand[i].isAtou)
					{
						if (computer.hand[i].CardValue.GetValue() < min)
						{
							c_idx = i;
							min = computer.hand[i].CardValue.GetValue();
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
		public int computersAnswer()
		{

			int i = 0, j = 0;
			// String c_array = "Computer ARRAY: ";

			//region colorHitRule
			if (colorHitRule)
			{
				i = computer.bestInColorHitsContext(this.playedOut);
				// for (j = 0; j < 5; j++) {
				//     c_array = c_array + computer.colorHitArray[j] + " ";
				// }

				// mqueue.insert(c_array);
				// mqueue.insert("i = " + i +  " Computer,hand[" + i + "] = " + computer.hand[i].getName() + " !");
				// mqueue.insert("Computer Hand: " + computer.showHand());

				return (i);
			}
			//endregion

			for (i = 0; i < 5; i++)
			{
				if (computer.hand[i].hitsCard(playedOut, false))
				{
					if (!computer.hand[i].isAtou)
					{
						return i;
					}
					if (playedOut.CardValue.GetValue() > CARDVALUE.KING.GetValue())
					{
						return i;
					}
				}
			}

			int min = 12; int c_idx = 0;
			for (i = 0; i < 5; i++)
			{
				if (computer.hand[i].isValidCard)
				{
					if (computer.hand[i].CardValue.GetValue() < min)
					{
						c_idx = i;
						min = computer.hand[i].CardValue.GetValue();
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
		public String printColor(char ch)
		{
			switch (ch)
			{
				case 'k': return JavaResReader.GetValueFromKey("color_k", "");
				case 'h': return JavaResReader.GetValueFromKey("color_h", "");
				case 't': return JavaResReader.GetValueFromKey("color_t", "");
				case 'p': return JavaResReader.GetValueFromKey("color_p", "");
				default: break;
			}
			return "NoColor";
		}

		/**
		 * atouInGame
		 * @return char for CARDCOLOR of atou in game
		 */
		public char atouInGame()
		{
			return (char)atouColor.GetChar();
		}


		/**
		 * insertMsg - inserts msg into internal message queue
		 * @param msg String to insert
		 *
		public void  insertMsg(String msg) {
			mqueue.insert(msg);
		}

		/**
		 * fetchMsg - fetches msg from internal message queue,
		 * @return String fetched message
		 *
		public String fetchMsg() {
			return mqueue.fetch();
		}

		/**
		 * inner class MessageQueue
		 *
		static class MessageQueue {
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
				// qbuffer.append(">" + mes + "\n");
				qbuffer.append(qcount + "->" + mes + "\n");
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
	 */
	}

}