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
	/// Port of class PairCard
	/// </summary>
	public class PairCard : TwoCards
	{
		bool atou = false;
		CARDCOLOR cardColor = CARDCOLOR.NONE;	// Color of Pair
		char color = 'n';			// 4 colors and 'n' for unitialized
		int pairValue = 20;			// 5 values and 0 for unitialized
		Card[] pairs = new Card[2]; // the 2 Cards, that represents the pair

		/// <summary>
		/// Constructor PairCard
		/// </summary>
		public PairCard()
		{
			// super();
			this.color = 'n';
			this.pairValue = 20;
		}

		/// <summary>
		/// Constructor PairCard		
		/// </summary>
		/// <param name="cardColor">represents the color of the pair</param>
		/// <param name="atouColor">represents the color of Atou card</param>
		public PairCard(CARDCOLOR cardColor, CARDCOLOR atouColor) : this()
		{
			this.cardColor = cardColor;
			this.color = (char)cardColor.GetChar();

			int queenNumValue = 1;
			if (cardColor == CARDCOLOR.SPADES) queenNumValue += 5;
			if (cardColor == CARDCOLOR.DIAMONDS) queenNumValue += 10;
			if (cardColor == CARDCOLOR.CLUBS) queenNumValue += 15;
			int kingNumValue = queenNumValue + 1;

			pairs[0] = new Card(queenNumValue, atouColor.GetChar());
			pairs[1] = new Card(kingNumValue, atouColor.GetChar());

			this.pairValue = 20;
			if (cardColor.GetChar() == atouColor.GetChar())
			{
				this.atou = true;
				this.pairValue = 40;
			}
		}

		/// <summary>
		/// Constructor PairCard
		/// </summary>
		/// <param name="queenCard">the Queen in that pair</param>
		/// <param name="kingCard">the King in that pair</param>
		public PairCard(Card queenCard, Card kingCard) : base(queenCard, kingCard)
		{
            this.cardColor = queenCard.CardColor;
            this.color = (char)queenCard.CardColor.GetChar();
            char kingColor = (char)kingCard.CardColor.GetChar();
            if (kingColor != this.color)
            {
                throw new InvalidOperationException("Queen " + color + " & King " + kingColor + " must have same colors!");
            }
            if (cardSumValue != 7)
            {
                throw new InvalidOperationException("Sum of queen + king in pair must be 7!");
            }
            
            pairs[0] = new Card(queenCard);
			pairs[1] = new Card(kingCard);
			if (queenCard.isAtou)
			{
				this.atou = true;
				this.pairValue = 40;
			}
		}
	}
}