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
    /// GameDreier implementation of Schnapsen with 3 Players
    /// <see cref="https://github.com/heinrichelsigan/schnapslet/wiki"/>
    /// </summary>
    [Serializable]
	public class GameDreier : GameBase, IDisposable
	{		
		public Player computer1, computer2;         // two computer player
		public Card playedOut2;
		public Card[] talon = new Card[2];

        /// <summary>
        /// Constructor of GameDreier
        /// </summary>
        /// <param name="c">current context</param>
        public GameDreier(HttpContext c) : base(c)
		{
			// super();
			globalAppSettings = (GlobalAppSettings)c.Session[Constants.APPNAME];					
        }

        /// <summary>
        /// Constructor of GameDreier with HttpContext and who starts as 2 parameters
        /// </summary>
        /// <param name="c"><see cref="HttpContext">HttpContext</see></param>
        /// <param name="starts"><see cref="PLAYERDEF"/> for 
		/// <seealso cref="PLAYERDEF.HUMAN"/> or <seealso cref="PLAYERDEF.COMPUTER"/></param>
        public GameDreier(HttpContext c, PLAYERDEF starts = PLAYERDEF.HUMAN) : base(c)
		{
            gambler = new Player(playersTurn, context, 6);
            computer1 = new Player(!playersTurn, context, 6);
            computer2 = new Player(!playersTurn, context, 6);
        }

		/// <summary>
		/// MergeCards - function for merging cards
		/// </summary>
		public override void MergeCards()
		{
			base.MergeCards();

            set[4] = new Card(inGame[4], context);
			set[4].SetAtou();						
			this.atouColor = set[4].CardColor;  // implement atou call
			string atouMsg = String.Format(ResReader.GetValue("atou_is", globalAppSettings.ISO2Lang),
                PrintColor(CARDCOLOR_Extensions.ColorChar(set[19].CardColor)));
			InsertMsg(atouMsg);

            for (int ti = 0; ti < 20; ti++)
			{
                set[ti] = new Card(inGame[ti], this.AtouInGame, context);
                if (ti < gambler.HandCount)
				{
					gambler.AssignCard(set[ti]);
				}
				else if (ti < gambler.HandCount + computer1.HandCount)
				{
					computer1.AssignCard(set[ti]);
				}
                else if (ti < gambler.HandCount + computer1.HandCount + computer2.HandCount)
				{
                    computer2.AssignCard(set[ti]);
                }
				else
				{
					talon[ti - 18] = set[ti];
				}

            }

			gambler.SortHand();
			computer1.SortHand();
            computer2.SortHand();
            schnapState = SCHNAPSTATE.GAME_STARTED;
		}

        /// <summary>
        /// destructor Dispose, it really destroys a game
		/// originally destroyGame in Java => realized by implementing IDisposible
        /// </summary>
        public override void Dispose()
		{            
            computer1 = null;
            computer2 = null;
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
					InsertFormated(ResReader.GetValue("your_hit_points", globalAppSettings.ISO2Lang),
						tmppoints.ToString());

                    return tmppoints;
				}
				else
				{
					playersTurn = false;
					tmppoints = playedOut.CardValue.GetValue() + computer.hand[ccard].CardValue.GetValue();
					computer.points += tmppoints;
                    InsertFormated(ResReader.GetValue("computer_hit_points", globalAppSettings.ISO2Lang),
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
                    InsertFormated(ResReader.GetValue("computer_hit_points", globalAppSettings.ISO2Lang),
                        tmppoints.ToString());

                    return (-tmppoints);
				}
				else
				{
					playersTurn = true;
					tmppoints = playedOut.CardValue.GetValue() + computer.hand[ccard].CardValue.GetValue();
					gambler.points += tmppoints;
                    InsertFormated(ResReader.GetValue("your_hit_points", globalAppSettings.ISO2Lang),
                        tmppoints.ToString());

                    return tmppoints;
				}
			}
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
        /// ComputerStarts() implementation of a move, where computer sraers (that move)
        /// </summary>
        /// <returns>card index of computer hand, tgar opebs the current move</returns>
        public int ComputerStarts()
		{
			computer.playerOptions = 0;
			int i; int j = 0; int mark = 0;			

            int cBestCloseCard = -1;
			#region has20_has40
            if ((i = computer.HasPair) > 0)
			{
				// if ((i > 1) && (computer.pairs[1] != null && computer.pairs[1].atou))
				if ((i > 1) && (computer.handpairs[1] == this.AtouInGame))
					mark = 1;

				// TODO: Computer closes game
				int addPoints = (computer.handpairs[mark] == this.AtouInGame) ? 52 : 33;
				if (!this.colorHitRule && schnapState == SCHNAPSTATE.GAME_STARTED &&
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
						InsertFormated(ResReader.GetValue("computer_says_pair", globalAppSettings.ISO2Lang),
							 PrintColor(csaid));

                        if (computer.hand[j].IsAtou)
							computer.points += 40;
						else
							computer.points += 20;

						if (computer.points >= Constants.ENOUGH)
						{
							String andEnough = ResReader.GetValue("twenty_and_enough", globalAppSettings.ISO2Lang);
							if (computer.hand[j].IsAtou)
							{
								andEnough = ResReader.GetValue("fourty_and_enough", globalAppSettings.ISO2Lang);
							}

							computer.playerOptions += PLAYEROPTIONS.ANDENOUGH.GetValue();
							InsertMsg(andEnough + " " + string.Format(ResReader.GetValue("computer_has_won_points", globalAppSettings.ISO2Lang),
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
			if (!this.colorHitRule && (computer.points + 12 >= Constants.ENOUGH) &&
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
            for (i = 0; i < computer.hand.Length; i++)
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