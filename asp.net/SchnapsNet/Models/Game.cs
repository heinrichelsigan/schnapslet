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
	[Serializable]
	public class Game : IDisposable
	{
		public volatile bool isGame = false;		// a Game is running
		public bool atouChanged = false;			// Atou allready changed
		public bool playersTurn = true;				// Who's playing
		public bool colorHitRule = false;			// Farb und Stichzwang
		public bool isClosed = false;				// game is closed
		public bool isReady = false;				// is ready to play out
		public bool shouldContinue = false;			// should continue the game
		public bool a20 = false;					// can player announce 1st pair
		public bool b20 = false;					// can player announce 2nd pair
		public bool bChange = false;				// can player change atou
		public bool pSaid = false;					// said pair in game

		public SCHNAPSTATE schnapState = SCHNAPSTATE.NONE;
		public Stack<SCHNAPSTATE> schnapsStack = new Stack<SCHNAPSTATE>();
		public CARDCOLOR atouColor = CARDCOLOR.NONE;    // CARDCOLOR that is atou in this game
														// public char atouInGame = 'n';
		public char said = 'n';							// player said pair char
		public char csaid = 'n';						// computer said pair char

		public int index = 9;
		public int movs = 0;
		public int phoneDirection = -1;
		public int[] inGame = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
						10,11,12,13,14,15,16,17,18,19 };

		public String sayMarriage20, sayMarriage40, statusMessage;
		public PLAYERDEF whoStarts = PLAYERDEF.UNKNOWN;

        internal GlobalAppSettings globalAppSettings;
		public Card[] set = new Card[20];
		public Card playedOut, playedOut0, playedOut1;

		public Player gambler, computer;
		// java.applet.Applet masterApplet = null;
		Random random;
        // Resources r;
        HttpContext context;

		Queue<string> mqueue = new Queue<string>();

        #region properties

        /// <summary>
        /// atouInGame => char for CARDCOLOR of atou in game
        /// </summary>       
        public char AtouInGame { get => (char)atouColor.GetChar(); }

        /// <summary>
        /// gets a random number modulo 20
        /// </summary>
        public int Random { get => GetRandom(20, false); }

        #endregion properties

        /// <summary>
        /// Constructor of Card
        /// </summary>
        /// <param name="c">current context</param>
        public Game(HttpContext c)
		{
			// super();
			globalAppSettings = (GlobalAppSettings)c.Session[Constants.APPNAME];
            pSaid = false;
            isGame = true;
			atouChanged = false;
			playersTurn = true;
			whoStarts = PLAYERDEF.HUMAN;
            colorHitRule = false;
			isClosed = false;
			shouldContinue = false;

			this.context = c;
			// this.r = c.getResources();
			this.schnapState = SCHNAPSTATE.GAME_START;
			ClearMsg();			
            
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
			movs = 0;

			schnapsStack = new Stack<SCHNAPSTATE>();
			statusMessage = "";
			sayMarriage20 = JavaResReader.GetValueFromKey("b20a_text", "");
			sayMarriage40 = JavaResReader.GetValueFromKey("b20b_text", "");

			InsertMsg(JavaResReader.GetValueFromKey("newgame_starts", globalAppSettings.TwoLetterISOLanguageName)); // TODO: giver msg            
        }

        /// <summary>
        /// Constructor of Game with HttpContext and who starts as 2 parameters
        /// </summary>
        /// <param name="c"><see cref="HttpContext">HttpContext</see></param>
        /// <param name="starts"><see cref="PLAYERDEF"/> for 
		/// <seealso cref="PLAYERDEF.HUMAN"/> or <seealso cref="PLAYERDEF.COMPUTER"/></param>
        public Game(HttpContext c, PLAYERDEF starts = PLAYERDEF.HUMAN) : this(c)
		{
			whoStarts = starts;			
			playersTurn = true;
			if (starts == PLAYERDEF.COMPUTER)
				playersTurn = false;

            gambler = new Player(playersTurn, context);
            gambler.points = 0;
            computer = new Player(!playersTurn, context);
            computer.points = 0;
			isClosed = false;

            schnapsStack = new Stack<SCHNAPSTATE>();
            MergeCards();
        }



		/// <summary>
		/// Gets a positive random number
        /// </summary>
        /// <param name="modulo">modulo modulo %</param>
        /// <param name="generate">true for generate a new Random, false for use existing one</param>
        /// <returns>random as int</returns>
        int GetRandom(int modulo, bool generate)
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
        /// MergeCards - function for merging cards
        /// </summary>
        public void MergeCards()
		{
			int i, k, j, l, tmp;
            
            InsertMsg(JavaResReader.GetValueFromKey("merging_cards", globalAppSettings.TwoLetterISOLanguageName));
            k = GetRandom(32, true);

			for (i = 0; i < k + 20; i++)
			{
				j = Random;
				l = Random;

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
			set[19].SetAtou();						
			this.atouColor = set[19].CardColor;  // set[19].getCharColor();

			for (i = 0; i < 19; i++)
			{
				set[i] = new Card(inGame[i], this.AtouInGame, context);
				if (i < 5)
				{
					gambler.AssignCard(set[i]);
				}
				else if (i < 10)
				{
					computer.AssignCard(set[i]);
				}
			}

			gambler.SortHand();
			computer.SortHand();
			schnapState = SCHNAPSTATE.GAME_STARTED;
		}

        /// <summary>
        /// destructor Dispose, it really destroys a game
		/// originally destroyGame in Java => realized by implementing IDisposible
        /// </summary>
        public void Dispose()
		{
			isGame = false;
			computer.Stop();
			gambler.Stop();
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
        /// StopGame - stops softley a game
        /// </summary>
        public void StopGame()
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
		/// Change Atou Card in game
        /// </summary>
        /// <param name="aPlayer">player, that changes Atou Card</param>
        public void ChangeAtou(Player aPlayer)
		{
			int cardidx;
			Card tmpCard;
			if ((cardidx = aPlayer.CanChangeAtou) < 0) return;

			tmpCard = aPlayer.hand[cardidx];
			aPlayer.hand[cardidx] = set[19];
			set[19] = tmpCard;

			computer.playerOptions += PLAYEROPTIONS.CHANGEATOU.GetValue();
			aPlayer.SortHand();
			atouChanged = true;
        }

		/// <summary>
		/// Checks, if a player can change Atou Card
        /// </summary>
        /// <param name="aPlayer">player, that should change Atou</param>
        /// <returns>true, if player can change, otherwise false</returns>
        public bool AtouIsChangable(Player aPlayer)
		{
			if (atouChanged ||
				schnapState == SCHNAPSTATE.GAME_CLOSED ||
				schnapState == SCHNAPSTATE.TALON_ONE_REMAINS ||
				schnapState == SCHNAPSTATE.TALON_CONSUMED)
				return false;
				if (aPlayer.CanChangeAtou >= 0) return true;
			return false;
        }

        /// <summary>
        /// CheckPoints
        /// </summary>
        /// <param name="ccard">index of computer card</param>
        /// <returns>points from the current hit, players points a positive vvalue, computer pinnts as negative value</returns>
        public int CheckPoints(int ccard)
		{
			int tmppoints;
			if (playersTurn)
			{
				if (playedOut.HitsCard(computer.hand[ccard], true))
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
				if (computer.hand[ccard].HitsCard(playedOut, true))
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
        /// AssignNewCard [Obsolete] assigns new cards to both player & computer		
        /// </summary>
        /// <returns>default 0, 1 if game entered colorGitRule; Players now must follow the suit led</returns>
        [Obsolete("AssignNewCard is obsolete", false)]
		public int AssignNewCard()
		{
			int retval = 0;
			if (!colorHitRule) // (colorHitRule == false)
            { 
				if (playersTurn)
				{
					gambler.AssignCard(set[++index]);
					computer.AssignCard(set[++index]);
				}
				else
				{
					computer.AssignCard(set[++index]);
					gambler.AssignCard(set[++index]);
				}
				if (index == 17)
				{
                    schnapState = SCHNAPSTATE.TALON_ONE_REMAINS;
                    atouChanged = true;
				}
				if (index == 19)
				{
					retval = 1;
                    atouChanged = true;
                    schnapState = SCHNAPSTATE.TALON_CONSUMED;                    
                    colorHitRule = true;
				}
			}
			else
			{
				if (++movs >= gambler.hand.Length)
				{
					schnapState = SCHNAPSTATE.ZERO_CARD_REMAINS;
				}					
			}
			computer.SortHand();
			gambler.SortHand();

			return retval;
        }

        /// <summary>
        /// AssignNextCard assigns the next card
        /// </summary>
        /// <param name="assignedCard">Card to be assigned (from game stack)</param>
        /// <returns>true, if the last card in talon has been assigned, otherwise false</returns>
        public bool AssignNextCard(Card assignedCard)
		{
			bool lastCard = false;
			if (!colorHitRule)
			{
				if (playersTurn)
				{
					assignedCard = set[++index];
					gambler.AssignCard(assignedCard);
					computer.AssignCard(set[++index]);
				}
				else
				{
					computer.AssignCard(set[++index]);
					assignedCard = set[++index];
					gambler.AssignCard(assignedCard);
				}
				if (index == 17)
				{
					schnapState = SCHNAPSTATE.TALON_ONE_REMAINS;
					atouChanged = true;
				}
				if (index == 19)
				{
					schnapState = SCHNAPSTATE.TALON_CONSUMED;
                    atouChanged = true;
                    lastCard = true;
					colorHitRule = true;
				}
			}
			else
			{
				assignedCard = null;
                if (++movs >= gambler.hand.Length)
                {
                    schnapState = SCHNAPSTATE.ZERO_CARD_REMAINS;
                }
            }
			computer.SortHand();
			gambler.SortHand();

			return lastCard;
        }


		/// <summary>
		/// Get points for currently finished game for tournament
		/// </summary>
		/// <param name="whoWon"><see cref="PLAYERDEF">PLAYERDEF</see> 
		/// could be either <see cref="PLAYERDEF.HUMAN"/> or <see cref="PLAYERDEF.COMPUTER"/></param>
		/// <returns>1, 2 or 3 points</returns>
        public int GetTournamentPoints(PLAYERDEF whoWon)
		{
			if (whoWon == PLAYERDEF.HUMAN)
			{
				if (computer.points == 0)
					return 3;
				if (computer.points < 33)
					return 2;
				return 1;
			}
			if (whoWon == PLAYERDEF.COMPUTER)
			{
				if (gambler.points == 0)
					return 3;
				if (gambler.points < 33)
					return 2;
				return 1;
			}
			return 0; // TODO Assertion
		}

        /// <summary>
        /// Closes a running game
        /// </summary>
        /// <param name="whoCloses"><see cref="PLAYERDEF"/> who closes are valid for 
        /// <seealso cref="PLAYERDEF.HUMAN"/> or <seealso cref="PLAYERDEF.COMPUTER"/></param>
        public void CloseGame(PLAYERDEF whoCloses)
		{
			if (this.isGame == false || this.gambler == null || this.isClosed || this.colorHitRule)
			{
				throw new InvalidSchnapsStateException(JavaResReader.GetValueFromKey("exception_cannot_close_game"));
			}

            schnapState = SCHNAPSTATE.GAME_CLOSED;
            isClosed = true;
            colorHitRule = true;
            if (atouChanged)
            {
                atouChanged = true;
            }

            if (whoCloses == PLAYERDEF.HUMAN)
            {
                this.gambler.hasClosed = true;
                statusMessage = JavaResReader.GetValueFromKey("player_closed_game");
            }
            if (whoCloses == PLAYERDEF.COMPUTER)
            {
                this.computer.hasClosed = true;
                statusMessage = JavaResReader.GetValueFromKey("computer_closed_game");
            }
			
			InsertMsg(statusMessage);
        }

        /// <summary>
        /// ComputerStarts() implementation of a move, where computer sraers (that move)
        /// </summary>
        /// <returns>card index of computer hand, tgar opebs the current move</returns>
        public int ComputerStarts()
		{
			computer.playerOptions = 0;
			int i; int j = 0; int mark = 0;

			#region changeAtou
			if (AtouIsChangable(computer))
			{
				ChangeAtou(computer);
				InsertMsg(JavaResReader.GetValueFromKey("computer_changes_atou", globalAppSettings.TwoLetterISOLanguageName));
            }
            #endregion changeAtou

            int cBestCloseCard = -1;
			#region has20_has40
            if ((i = computer.HasPair) > 0)
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
						if (!computer.hand[i].IsValidCard)
							continue;
						else if (computer.hand[i].CardValue.GetValue() >= CARDVALUE.TEN.GetValue())
						{
							int bestIdx = gambler.PreferedInColorHitsContext(computer.hand[i]);
							if (gambler.hand[bestIdx].IsValidCard &&
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

                        if (computer.hand[j].IsAtou)
							computer.points += 40;
						else
							computer.points += 20;

						if (computer.points > 65)
						{
							String andEnough = JavaResReader.GetValueFromKey("twenty_and_enough", globalAppSettings.TwoLetterISOLanguageName);
							if (computer.hand[j].IsAtou)
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
					if (!computer.hand[i].IsValidCard)
						continue;
					else if (computer.hand[i].CardValue.GetValue() >= CARDVALUE.TEN.GetValue())
					{
						int bestIdx = gambler.PreferedInColorHitsContext(computer.hand[i]);
						if (gambler.hand[bestIdx].IsValidCard &&
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
					if (computer.hand[i].IsValidCard)
					{
						if (!computer.hand[i].IsAtou && computer.hand[i].CardValue.GetValue() >= CARDVALUE.TEN.GetValue())
						{
							int bestIdx = gambler.PreferedInColorHitsContext(computer.hand[i]);

							if (gambler.hand[bestIdx].IsValidCard && !gambler.hand[bestIdx].IsAtou &&
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
					if (computer.hand[i].IsValidCard)
					{
						int bestIdx = gambler.PreferedInColorHitsContext(computer.hand[i]);

						if (gambler.hand[bestIdx].IsValidCard &&
							gambler.hand[bestIdx].CardColor.GetChar() == computer.hand[i].CardColor.GetChar() &&
								gambler.hand[bestIdx].CardValue.GetValue() < computer.hand[i].CardValue.GetValue())
						{
							return i;
						}
					}
				}

				for (i = 0; i < 5; i++)
				{
					if (computer.hand[i].IsValidCard)
					{
						return i;
					}
				}
			}
            #endregion colorHitRule

            int min = 10, max = 1, minIdx = -1, maxIdx = -1;
			Card maxCard = null;
			for (i = 0; i < 5; i++)
			{
				if (computer.hand[i].IsValidCard)
				{
					if (!computer.hand[i].IsAtou)
					{
						if (computer.hand[i].CardValue.GetValue() < min)
						{
                            minIdx = i;
                            min = computer.hand[i].CardValue.GetValue();
						}
						if (computer.hand[i].CardValue.GetValue() > max)
						{
                            maxIdx = i;
                            maxCard = computer.hand[i];
                            max = computer.hand[i].CardValue.GetValue();
                        }
                    }
				}
			}

			if (minIdx >= 0)
				return minIdx;
			if (maxIdx >= 0 && maxCard.CardValue == CARDVALUE.ACE)
				return maxIdx;

			minIdx = -1; maxIdx = -1; min = 12; max = 1;
            for (i = 0; i < 5; i++)
            {
                if (computer.hand[i].IsValidCard)
                {                    
                    if (computer.hand[i].CardValue.GetValue() < min)
                    {
                        minIdx = i;
                        min = computer.hand[i].CardValue.GetValue();
                    }
                    if (computer.hand[i].CardValue.GetValue() > max && computer.hand[i].IsAtou)
                    {
                        maxIdx = i;
                        maxCard = computer.hand[i];
                        max = computer.hand[i].CardValue.GetValue();
                    }
                }
            }

			if (maxIdx >= 0)
				return maxIdx;

            return minIdx;
        }

        /// <summary>
        /// ComputersAnswer implementation of computer answr  move)
        /// </summary>
        /// <returns>card index of computer hand, that was played for answering</returns>
        public int ComputersAnswer()
		{
			int i = 0;
			
			if (colorHitRule)
			{
				i = computer.PreferedInColorHitsContext(this.playedOut);
				return (i);
			}

            int tmpPair = computer.HasPair;
            for (i = 0; i < 5; i++)
			{
				if (computer.hand[i].HitsCard(playedOut, false))
				{
					if (computer.hand[i].IsAtou && playedOut.CardValue.GetValue() > CARDVALUE.KING.GetValue())
					{
						if (computer.hand[i].CardValue == CARDVALUE.JACK ||
							computer.hand[i].CardValue == CARDVALUE.TEN)
							return i;
					}                      
                    if (!computer.hand[i].IsAtou && 
						computer.hand[i].CardValue.GetValue() > CARDVALUE.KING.GetValue())
					{
						return i;
					}                    
                }
			}

			int min = 12, minIdx = -1;
			for (i = 0; i < 5; i++)
			{
				if (computer.hand[i].IsValidCard)
				{
					if (computer.hand[i].CardValue.GetValue() < min)
					{
                        minIdx = i;
						min = computer.hand[i].CardValue.GetValue();
						if (tmpPair > 0 && computer.hand[i].CardValue == CARDVALUE.JACK)
							return minIdx;
                    }
				}
			}

			return minIdx;
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