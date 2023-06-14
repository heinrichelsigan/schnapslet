using SchnapsNet.ConstEnum;
using System;

namespace SchnapsNet.Models
{
	/// <summary>
	/// Port of class PairCard
	/// </summary>
	public class PairCard : TwoCards
	{
        #region fields
        CARDCOLOR cardColor = CARDCOLOR.NONE;					// Color of Pair		
		Card[] pairs = new Card[2];                             // the 2 Cards, that represents the pair
        #endregion fields

        #region properties
        public char ColorChar { get; protected set; } = 'n';	// 4 colors and 'n' for unitialized
        public bool Atou { get; protected set; }				// normal marriage 20 or atou marriage 40
        public int PairValue { get; protected set; } = 20;      // value of 20 or 40
        #endregion properties

        #region ctor
        /// <summary>
        /// Constructor PairCard
        /// </summary>
        public PairCard()
		{
			// super();
			this.ColorChar = 'n';
			this.Atou = false;
			this.PairValue = 20;
		}

		/// <summary>
		/// Constructor PairCard		
		/// </summary>
		/// <param name="cardColor">represents the color of the pair</param>
		/// <param name="atouColor">represents the color of Atou card</param>
		public PairCard(CARDCOLOR cardColor, CARDCOLOR atouColor) : this()
		{
			this.cardColor = cardColor;
			this.ColorChar = (char)cardColor.GetChar();

			int queenNumValue = 1;
			if (cardColor == CARDCOLOR.SPADES) queenNumValue += 5;
			if (cardColor == CARDCOLOR.DIAMONDS) queenNumValue += 10;
			if (cardColor == CARDCOLOR.CLUBS) queenNumValue += 15;
			int kingNumValue = queenNumValue + 1;

			pairs[0] = new Card(queenNumValue, atouColor.GetChar());
			pairs[1] = new Card(kingNumValue, atouColor.GetChar());

			this.PairValue = 20;
			if (cardColor.GetChar() == atouColor.GetChar())
			{
				this.Atou = true;
				this.PairValue = 40;
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
            this.ColorChar = (char)queenCard.CardColor.GetChar();
            char kingColor = (char)kingCard.CardColor.GetChar();
            if (kingColor != this.ColorChar)
            {
                throw new InvalidOperationException("Queen " + ColorChar + " & King " + kingColor + " must have same colors!");
            }
            if (cardSumValue != 7)
            {
                throw new InvalidOperationException("Sum of queen + king in pair must be 7!");
            }
            
            pairs[0] = new Card(queenCard);
			pairs[1] = new Card(kingCard);
			if (queenCard.IsAtou)
			{
				this.Atou = true;
				this.PairValue = 40;
			}
		}
        #endregion ctor
    
	}
}