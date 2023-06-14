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
	/// Port of class TwoCards
	/// </summary>
	public class TwoCards
    {
        private Card[] cards = new Card[2]; // the 2 Cards, that represents the pair
        private Card card1st = null;
        private Card card2nd = null;
        internal int cardSumValue = 0;

        #region properties
        
        /// <summary>
        /// Pair of cards
        /// </summary>
        public Card[] Cards { get => cards; }
        /// <summary>
        /// 1st card of pair
        /// </summary>
        public Card Card1st { get => card1st; protected set => card1st = value; }
        /// <summary>
        /// 2nd card of pair
        /// </summary>
        public Card Card2nd { get => card2nd; protected set => card2nd = value; }

        #endregion properties

        #region ctor
        /// <summary>
        /// Constructor TwoCards()
        /// </summary>
        public TwoCards()
        {
            card1st = null;
            card2nd = null;
            cards = new Card[2];
            cards[0] = card1st;
            cards[1] = card2nd;
        }   
        
        /// <summary>
        /// Constructor with two Cards as agrument
        /// </summary>
        /// <param name="first">first represents 1st card</param>
        /// <param name="second">represents 2nd card</param>
        public TwoCards(Card first, Card second) : this()
        {
            card1st = new Card(first);
            card2nd = new Card(second);

            cards = new Card[2];
            cards[0] = card1st;
            cards[1] = card2nd;

            this.cardSumValue = card1st.CardValue.GetValue() + card2nd.CardValue.GetValue();
        }
        #endregion ctor

    }
}