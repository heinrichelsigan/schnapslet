using SchnapsNet.ConstEnum;
using SchnapsNet.Utils;
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
using System.Xml.Linq;

namespace SchnapsNet.Models
{
	/// <summary>
	/// Game Schnapsen with 2 Players
	/// <see cref="https://github.com/heinrichelsigan/schnapslet/wiki"/>
	/// </summary>
	[Serializable]
	public class Game : GameBase, IDisposable
	{		
		public bool atouChanged = false;			// Atou allready changed	
		public bool isClosed = false;				// game is closed
		public bool bChange = false;                // can player change atou		

		public bool CanCloseOrChange
		{
			get => (!colorHitRule && !isClosed &&
				schnapState != SCHNAPSTATE.GAME_CLOSED &&
                schnapState != SCHNAPSTATE.TALON_ONE_REMAINS &&
				schnapState != SCHNAPSTATE.TALON_CONSUMED &&
				schnapState != SCHNAPSTATE.GAME_CLOSED &&
				schnapState != SCHNAPSTATE.ZERO_CARD_REMAINS);
		}

		public bool ZeroRemain
		{
			get
			{
				for (int gi = 0; gi < gambler.hand.Length; gi++)
				{
					try
					{
						if (gambler.hand[gi] != null && gambler.hand[gi].IsValidCard)
							return false;
					}
					catch (Exception e) 
					{ 
						if (globalAppSettings != null)
							globalAppSettings.LastException = e;
					}
                }
                for (int ci = 0; ci < computer.hand.Length; ci++)
                {
					try
					{
						if (computer.hand[ci] != null && computer.hand[ci].IsValidCard)
							return false;
					}
					catch (Exception e)
					{
						if (globalAppSettings != null)
							globalAppSettings.LastException = e;
					}
                }
				return true;
            }
		}


        /// <summary>
        /// Constructor of Game
        /// </summary>
        /// <param name="c">current context</param>
        public Game(HttpContext c) : base(c)
		{
			// super();
			globalAppSettings = (GlobalAppSettings)c.Session[Constants.APPNAME];
			atouChanged = false;			
			isClosed = false;			
        }

        /// <summary>
        /// Constructor of Game with HttpContext and who starts as 2 parameters
        /// </summary>
        /// <param name="c"><see cref="HttpContext">HttpContext</see></param>
        /// <param name="starts"><see cref="PLAYERDEF"/> for 
		/// <seealso cref="PLAYERDEF.HUMAN"/> or <seealso cref="PLAYERDEF.COMPUTER"/></param>
        public Game(HttpContext c, PLAYERDEF starts = PLAYERDEF.HUMAN) : base(c, starts)
		{			
        }

		/// <summary>
		/// MergeCards - function for merging cards
		/// </summary>
		public override void MergeCards()
		{
			base.MergeCards();

            set[19] = new Card(inGame[19], context);
			set[19].SetAtou();						
			this.atouColor = set[19].CardColor;  // set[19].getCharColor();
			string atouMsg = ResReader.GetStringFormated("atou_is", globalAppSettings.Locale, 
                PrintColor(CARDCOLOR_Extensions.ColorChar(set[19].CardColor)));
			InsertMsg(atouMsg);

            for (int j = 0; j < 19; j++)
			{
                set[j] = new Card(inGame[j], this.AtouInGame, context);
                if (j < gambler.HandCount)
				{
					gambler.AssignCard(set[j]);
				}
				else if (j < gambler.HandCount + computer.HandCount)
				{
					computer.AssignCard(set[j]);
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
        public override void Dispose()
		{
			base.Dispose();
		}

        /// <summary>
        /// StopGame - stops softley a game
        /// </summary>
        public override void StopGame()
		{
			base.StopGame();
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
                    InsertMsg(ResReader.GetStringFormated("your_hit_points", globalAppSettings.Locale, tmppoints.ToString()));

                    return tmppoints;
				}
				else
				{
					playersTurn = false;
					tmppoints = playedOut.CardValue.GetValue() + computer.hand[ccard].CardValue.GetValue();
					computer.points += tmppoints;
                    InsertMsg(ResReader.GetStringFormated("computer_hit_points", globalAppSettings.Locale, tmppoints.ToString()));

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
					InsertMsg(ResReader.GetStringFormated("computer_hit_points", globalAppSettings.Locale, tmppoints.ToString()));

                    return (-tmppoints);
				}
				else
				{
					playersTurn = true;
					tmppoints = playedOut.CardValue.GetValue() + computer.hand[ccard].CardValue.GetValue();
					gambler.points += tmppoints;
					InsertMsg(ResReader.GetStringFormated("your_hit_points", globalAppSettings.Locale, tmppoints.ToString()));

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
				if (++movs >= gambler.hand.Length || movs >= computer.HandCount)
				{
					schnapState = SCHNAPSTATE.ZERO_CARD_REMAINS;
					retval = movs;
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
        public override int GetTournamentPoints(PLAYERDEF whoWon)
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
			if (this.isGame == false || this.gambler == null || this.colorHitRule)
			{
				throw new InvalidSchnapsStateException(ResReader.GetRes("exception_cannot_close_game", globalAppSettings.Locale));
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
                statusMessage = ResReader.GetRes("player_closed_game", globalAppSettings.Locale);
            }
            if (whoCloses == PLAYERDEF.COMPUTER)
            {
                this.computer.hasClosed = true;
                statusMessage = ResReader.GetRes("computer_closed_game", globalAppSettings.Locale);
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
				InsertMsg(ResReader.GetRes("computer_changes_atou", globalAppSettings.Locale));
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
					(computer.points + addPoints >= Constants.ENOUGH))
				{
					for (i = 0; i < computer.hand.Length; i++)
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

				for (j = 0; j < computer.hand.Length; j++)
				{
					if ((computer.hand[j].CardColor.GetChar() == computer.handpairs[mark]) &&
						(computer.hand[j].CardValue.GetValue() > 2) &&
						(computer.hand[j].CardValue.GetValue() < 5))
					{
						computer.playerOptions += PLAYEROPTIONS.SAYPAIR.GetValue();
						csaid = computer.handpairs[mark];
						InsertFormated(ResReader.GetRes("computer_says_pair", globalAppSettings.Locale),
							 PrintColor(csaid));

                        if (computer.hand[j].IsAtou)
							computer.points += 40;
						else
							computer.points += 20;

						if (computer.points >= Constants.ENOUGH)
						{
							String andEnough = ResReader.GetRes("twenty_and_enough", globalAppSettings.Locale);
							if (computer.hand[j].IsAtou)
							{
								andEnough = ResReader.GetRes("fourty_and_enough", globalAppSettings.Locale);
							}

							computer.playerOptions += PLAYEROPTIONS.ANDENOUGH.GetValue();
							InsertMsg(andEnough + " " + 
								ResReader.GetStringFormated("computer_has_won_points", globalAppSettings.Locale,
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
			if (!this.isClosed && !this.colorHitRule && (computer.points + 12 >= Constants.ENOUGH) &&
					schnapState == SCHNAPSTATE.GAME_STARTED)
			{
				for (i = 0; i < computer.hand.Length; i++)
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
				for (i = 0; i < computer.hand.Length; i++)
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

				for (i = 0; i < computer.hand.Length; i++)
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

				for (i = 0; i < computer.hand.Length; i++)
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
			for (i = 0; i < computer.hand.Length; i++)
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
            for (i = 0; i < computer.HandCount; i++)
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
            for (i = 0; i < computer.hand.Length; i++)
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
			for (i = 0; i < computer.hand.Length; i++)
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
        public override String PrintColor(char ch)
		{
			return base.PrintColor(ch);
        }

    }

}