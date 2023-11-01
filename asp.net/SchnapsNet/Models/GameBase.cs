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
using System.Xml.Linq;

namespace SchnapsNet.Models
{
	/// <summary>
	/// Basic game for all variants of Schnapsen
	/// <see cref="https://github.com/heinrichelsigan/schnapslet/wiki"/>
	/// </summary>
	[Serializable]
	public abstract class GameBase : IDisposable
	{
		public volatile bool isGame = false;		// a Game is running
		public bool playersTurn = true;				// Who's playing
		public bool colorHitRule = false;			// Farb und Stichzwang
		public bool isReady = false;				// is ready to play out
		public bool shouldContinue = false;			// should continue the game
		public bool a20 = false;					// can player announce 1st pair
		public bool b20 = false;					// can player announce 2nd pair
		public bool pSaid = false;					// said pair in game

		public SCHNAPSTATE schnapState = SCHNAPSTATE.NONE;
		public Stack<SCHNAPSTATE> schnapsStack = new Stack<SCHNAPSTATE>();
		public CARDCOLOR atouColor = CARDCOLOR.NONE;    // CARDCOLOR that is atou in this game
														// public char atouInGame = 'n';
		public char said = 'n';							// player said pair char
		public char csaid = 'n';						// computer said pair char

		public int index = 9;
		public int movs = 0;
		// public int phoneDirection = -1;
		public int fetchedMsgCount = 0;
		public int[] inGame = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
						10,11,12,13,14,15,16,17,18,19 };

		public String sayMarriage20, sayMarriage40, statusMessage;
		public PLAYERDEF whoStarts = PLAYERDEF.UNKNOWN;

        internal GlobalAppSettings globalAppSettings;
		public Card[] set = new Card[20];
		public Card playedOut, playedOut0, playedOut1;

		public Player gambler, computer;
		protected internal Random random;
        protected internal HttpContext context;

		internal Queue<string> mqueue = new Queue<string>();

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
        /// Constructor of GameBase
        /// </summary>
        /// <param name="c">current context</param>
        public GameBase(HttpContext c)
		{
			// super();
			globalAppSettings = (GlobalAppSettings)c.Session[Constants.APPNAME];
            pSaid = false;
            isGame = true;
			playersTurn = true;
			whoStarts = PLAYERDEF.HUMAN;
            colorHitRule = false;
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
            fetchedMsgCount = 0;

            schnapsStack = new Stack<SCHNAPSTATE>();
			statusMessage = "";
			sayMarriage20 = JavaResReader.GetValueFromKey("b20a_text", "");
			sayMarriage40 = JavaResReader.GetValueFromKey("b20b_text", "");

			InsertMsg(JavaResReader.GetValueFromKey("newgame_starts", globalAppSettings.ISO2Lang)); // TODO: giver msg            
        }

        /// <summary>
        /// Constructor of Game with HttpContext and who starts as 2 parameters
        /// </summary>
        /// <param name="c"><see cref="HttpContext">HttpContext</see></param>
        /// <param name="starts"><see cref="PLAYERDEF"/> for 
		/// <seealso cref="PLAYERDEF.HUMAN"/> or <seealso cref="PLAYERDEF.COMPUTER"/></param>
        public GameBase(HttpContext c, PLAYERDEF starts = PLAYERDEF.HUMAN) : this(c)
		{
			whoStarts = starts;			
			playersTurn = true;
			if (starts == PLAYERDEF.COMPUTER)
				playersTurn = false;

			gambler = new Player(playersTurn, context);
            computer = new Player(!playersTurn, context);

            schnapsStack = new Stack<SCHNAPSTATE>();
            MergeCards();
        }

		/// <summary>
		/// Gets a positive random number
        /// </summary>
        /// <param name="modulo">modulo modulo %</param>
        /// <param name="generate">true for generate a new Random, false for use existing one</param>
        /// <returns>random as int</returns>
        internal virtual int GetRandom(int modulo, bool generate)
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
        public virtual void MergeCards()
		{
			int i, k, j, l, tmp;
            
            InsertMsg(JavaResReader.GetValueFromKey("merging_cards", globalAppSettings.ISO2Lang));
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

            for (i = 0; i < 20; i++)
			{
				set[i] = new Card(inGame[i], this.AtouInGame, context);
			}

			schnapState = SCHNAPSTATE.GAME_STARTED;
		}

        /// <summary>
        /// destructor Dispose, it really destroys a game
		/// originally destroyGame in Java => realized by implementing IDisposible
        /// </summary>
        public virtual void Dispose()
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
        public virtual void StopGame()
		{
			isGame = false;
			atouColor = CARDCOLOR.NONE;

			colorHitRule = false;
			playedOut = new Card(context); // new Card(masterApplet, -1);
			for (int i = 0; i < gambler.hand.Length; i++)
			{
				if (gambler != null && gambler.hand != null)
					gambler.hand[i] = new Card(context); // new card(masterApplet, -1);
				if (computer != null && computer.hand != null)
					computer.hand[i] = playedOut;
			}
			schnapState = SCHNAPSTATE.NONE;            
            InsertMsg(JavaResReader.GetValueFromKey("ending_game", globalAppSettings.ISO2Lang));
        }

		/// <summary>
		/// Get points for currently finished game for tournament
		/// </summary>
		/// <param name="whoWon"><see cref="PLAYERDEF">PLAYERDEF</see> 
		/// could be either <see cref="PLAYERDEF.HUMAN"/> or <see cref="PLAYERDEF.COMPUTER"/></param>
		/// <returns>1, 2 or 3 points</returns>
		public abstract int GetTournamentPoints(PLAYERDEF whoWon);
        
		/// <summary>
		/// PrintColor prinrs tbe full name of color from xml file 
		///  at https://area23.at/mono/SchnapsNet/
        /// </summary>
        /// <param name="ch">char color</param>
        /// <returns>printed color name</returns>
        public virtual String PrintColor(char ch)
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
        public virtual void InsertMsg(String msg) 
		{
			mqueue.Enqueue(msg);
        }

        /// <summary>
        /// InsertFormated - inserts a multipart string in msg queue
        /// </summary>
        /// <param name="msg">formated string with one placeholder to insert</param>
        /// <param name="prmArg0">parameter argument for placeholder</param>
        public virtual void InsertFormated(string fMsg, string prmArg0)
		{
            mqueue.Enqueue(string.Format(fMsg, prmArg0));
        }

        /// <summary>
        /// InsertFormated - inserts a multipart string in msg queue
        /// </summary>
        /// <param name="msg">pre formated string with placeholders to insert</param>
        /// <param name="prmArgs">parameter arguments for placeholders</param>
        public virtual void InsertFormated(string fMsg, string prmArg0, string[] prmArgs)
        {
            mqueue.Enqueue(string.Format(fMsg, prmArgs));
        }

        /// <summary>
        /// FetchMsg - fetches msg from internal message queue
        /// </summary>
        /// <returns>String fetched messages</returns>
        public virtual String FetchMsg() 
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
		/// Fetches last message in message queue
		/// </summary>
		/// <returns>string last message</returns>
		public virtual String[] FetchMsgArray()
		{
			List<string> msgs = new List<string>();
            if (mqueue != null && mqueue.Count > 0)
            {
                foreach (string mesg in mqueue)
                {
					msgs.Add(mesg);

                }
            }
			return msgs.ToArray();
        }

		/// <summary>
		/// Clears internal message queue
		/// </summary>
		public virtual void ClearMsg()
		{
			mqueue.Clear();
        }

		#endregion internalMessageQueue
    }

}