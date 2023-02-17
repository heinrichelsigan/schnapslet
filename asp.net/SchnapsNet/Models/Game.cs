using SchnapsNet.ConstEnum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
	public class Game : IDisposable
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

		public GlobalAppSettings globalAppSettings;
		public Card[] set = new Card[20];
		public Card playedOut, playedOut0, playedOut1;

		public Player gambler, computer;
		// java.applet.Applet masterApplet = null;
		Random random;
        // Resources r;
        HttpContext context;

		Queue<string> mqueue = new Queue<string>();


        /// <summary>
        /// atouInGame => char for CARDCOLOR of atou in game
        /// </summary>       
        public char AtouInGame { get => (char)atouColor.GetChar(); }

        /// <summary>
        /// Constructor of Card
        /// </summary>
        /// <param name="c">current context</param>
        public Game(HttpContext c)
		{
			// super();
			globalAppSettings = (GlobalAppSettings)c.Session[Constants.APPNAME];
			isGame = true;
			atouChanged = false;
			playersTurn = true;
			colorHitRule = false;
			isClosed = false;
			shouldContinue = false;

			this.context = c;
			// this.r = c.getResources();
			this.schnapState = SCHNAPSTATE.GAME_START;
			ClearMsg();
			InsertMsg(JavaResReader.GetValueFromKey("newgame_starts", globalAppSettings.TwoLetterISOLanguageName));
            
			playedOut0 = globalAppSettings.CardEmpty;
			playedOut1 = globalAppSettings.CardEmpty;

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

			InsertMsg(JavaResReader.GetValueFromKey("newgame_starts", globalAppSettings.TwoLetterISOLanguageName));

            gambler = new Player(true, context);
			gambler.points = 0;
			computer = new Player(false, context);
			computer.points = 0;

			mergeCards();
        }

		/// <summary>
		/// gets a positive random number
        /// </summary>
        /// <param name="modulo">modulo modulo %</param>
        /// <param name="generate">true for generate a new Random, false for use existing one</param>
        /// <returns>random as int</returns>
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

		/// <summary>
		/// gets a random number modulo 20
        /// </summary>
        /// <returns>random as int</returns>
        int getRandom()
		{
			return getRandom(20, false);
		}

        /// <summary>
        /// mergeCards - function for merging cards
        /// </summary>
        public void mergeCards()
		{
			int i, k, j, l, tmp;
            
            InsertMsg(JavaResReader.GetValueFromKey("merging_cards", globalAppSettings.TwoLetterISOLanguageName));
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
			InsertFormated(JavaResReader.GetValueFromKey("merging_cards", globalAppSettings.TwoLetterISOLanguageName), set[19].FullName);			
			this.atouColor = set[19].CardColor;  // set[19].getCharColor();

			for (i = 0; i < 19; i++)
			{
				set[i] = new Card(inGame[i], this.AtouInGame, context);
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

        /// <summary>
        /// destructor Dispose, it really destroys a game
		/// originally destroyGame in Java => realized by implementing IDisposible
        /// </summary>
        public void Dispose()
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

        /// <summary>
        /// stopGame - stops softley a game
        /// </summary>
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
            InsertMsg(JavaResReader.GetValueFromKey("ending_game", globalAppSettings.TwoLetterISOLanguageName));
        }

		/// <summary>
		/// change Atou Card in game
        /// </summary>
        /// <param name="aPlayer">player, that changes Atou Card</param>
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

		/// <summary>
		/// Checks, if a player can change Atou Card
        /// </summary>
        /// <param name="aPlayer">player, that should change Atou</param>
        /// <returns>true, if player can change, otherwise false</returns>
        public bool atouIsChangable(Player aPlayer)
		{
			if (atouChanged) return false;
			if (aPlayer.canChangeAtou() >= 0) return true;
			return false;
        }

		/// <summary>
		/// checkPoints
        /// </summary>
        /// <param name="ccard">index of computer card</param>
        /// <returns>points from the current hit, players points a positive vvalue, computer pinnts as negative value</returns>
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
					InsertFormated(JavaResReader.GetValueFromKey("your_hit_points", globalAppSettings.TwoLetterISOLanguageName),
						tmppoints.ToString());

                    return tmppoints;
				}
				else
				{
					playersTurn = false;
					tmppoints = playedOut.CardValue.GetValue() + computer.hand[ccard].CardValue.GetValue();
					computer.points += tmppoints;
                    InsertFormated(JavaResReader.GetValueFromKey("computer_hit_points", globalAppSettings.TwoLetterISOLanguageName),
                        tmppoints.ToString());

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
                    InsertFormated(JavaResReader.GetValueFromKey("computer_hit_points", globalAppSettings.TwoLetterISOLanguageName),
                        tmppoints.ToString());

                    return (-tmppoints);
				}
				else
				{
					playersTurn = true;
					tmppoints = playedOut.CardValue.GetValue() + computer.hand[ccard].CardValue.GetValue();
					gambler.points += tmppoints;
                    InsertFormated(JavaResReader.GetValueFromKey("your_hit_points", globalAppSettings.TwoLetterISOLanguageName),
                        tmppoints.ToString());

                    return tmppoints;
				}
			}
        }

        /// <summary>
        /// assignNewCard [Obsolete] assigns new cards to both player & computer		
        /// </summary>
        /// <returns>default 0, 1 if game entered colorGitRule; Players now must follow the suit led</returns>
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
				{
                    schnapState = SCHNAPSTATE.TALON_ONE_REMAINS;
                    atouChanged = true;
				}
				if (index == 19)
				{
					retval = 1;
                    schnapState = SCHNAPSTATE.TALON_CONSUMED;                    
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

        /// <summary>
        /// assignNextCard assigns the next card
        /// </summary>
        /// <param name="assignedCard">Card to be assigned (from game stack)</param>
        /// <returns>true, if the last card in talon has been assigned, otherwise false</returns>
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

        /// <summary>
        /// computerStarts() implementation of a move, where computer sraers (that move)
        /// </summary>
        /// <returns>card index of computer hand, tgar opebs the current move</returns>
        public int computerStarts()
		{

			computer.playerOptions = 0;
			int i; int j = 0; int mark = 0;

			#region changeAtou
			if (atouIsChangable(computer))
			{
				changeAtou(computer);
				InsertMsg(JavaResReader.GetValueFromKey("computer_changes_atou", globalAppSettings.TwoLetterISOLanguageName));
            }
            #endregion changeAtou

            int cBestCloseCard = -1;
			#region has20_has40
            if ((i = computer.has20()) > 0)
			{
				// if ((i > 1) && (computer.pairs[1] != null && computer.pairs[1].atou))
				if ((i > 1) && (computer.handpairs[1] == this.AtouInGame))
					mark = 1;

				// TODO: Computer closes game
				int addPoints = (computer.handpairs[mark] == this.AtouInGame) ? 52 : 33;
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
						InsertFormated(JavaResReader.GetValueFromKey("computer_says_pair", globalAppSettings.TwoLetterISOLanguageName),
							 printColor(csaid));

                        if (computer.hand[j].isAtou)
							computer.points += 40;
						else
							computer.points += 20;

						if (computer.points > 65)
						{
							String andEnough = JavaResReader.GetValueFromKey("twenty_and_enough", globalAppSettings.TwoLetterISOLanguageName);
							if (computer.hand[j].isAtou)
							{
								andEnough = JavaResReader.GetValueFromKey("fourty_and_enough", globalAppSettings.TwoLetterISOLanguageName);
							}

							computer.playerOptions += PLAYEROPTIONS.ANDENOUGH.GetValue();
							InsertMsg(andEnough + " " + string.Format(JavaResReader.GetValueFromKey("computer_has_won_points", globalAppSettings.TwoLetterISOLanguageName),
								computer.points.ToString()));

                        }
                        else
						{
							computer.playerOptions += PLAYEROPTIONS.PLAYSCARD.GetValue();
						}

						return j;
					}
				}
			}
            #endregion has20_has40

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

			#region colorHitRule
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
            #endregion colorHitRule

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

		/// <summary>
		/// computersAnswer implementation of computer answr  move)
		/// </summary>
		/// <returns>card index of computer hand, that was played for answering</returns>
		public int computersAnswer()
		{
			int i = 0, j = 0;
			// String c_array = "Computer ARRAY: ";

			#region colorHitRule
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
            #endregion colorHitRule

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

		/// <summary>
		/// printColor prinrs tbe full name of color from xml file 
		///  at https://area23.at/mono/SchnapsNet/
        /// </summary>
        /// <param name="ch">char color</param>
        /// <returns>printed color name</returns>
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

        #region internalMessageQueue

        /// <summary>
        /// InsertMsg - inserts msg into internal message queue
        /// </summary>
        /// <param name="msg">string to insert</param>
        public void InsertMsg(String msg) 
		{
			mqueue.Enqueue(msg);
        }

        /// <summary>
        /// InsertFormated - inserts a multipart string in msg queue
        /// </summary>
        /// <param name="msg">formated string with one placeholder to insert</param>
        /// <param name="prmArg0">parameter argument for placeholder</param>
        public void InsertFormated(string fMsg, string prmArg0)
		{
            mqueue.Enqueue(string.Format(fMsg, prmArg0));
        }

        /// <summary>
        /// InsertFormated - inserts a multipart string in msg queue
        /// </summary>
        /// <param name="msg">pre formated string with placeholders to insert</param>
        /// <param name="prmArgs">parameter arguments for placeholders</param>
        public void InsertFormated(string fMsg, string prmArg0, string[] prmArgs)
        {
            mqueue.Enqueue(string.Format(fMsg, prmArgs));
        }

        /// <summary>
        /// FetchMsg - fetches msg from internal message queue
        /// </summary>
        /// <returns>String fetched messages</returns>
        public String FetchMsg() 
		{
			string retMsg = "";
			if (mqueue != null && mqueue.Count > 0) 
			{
				foreach (string mesg in mqueue)
				{
					retMsg += mesg + "\r\n";

                }
			}
			return retMsg;
        }

		/// <summary>
		/// Clears internal message queue
		/// </summary>
		public void ClearMsg()
		{
			mqueue.Clear();
        }

        #endregion internalMessageQueue
    }

}