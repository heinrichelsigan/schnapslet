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
        public Card[] cards = new Card[2]; // the 2 Cards, that represents the pair
        public Card card1st = null;
        public Card card2nd = null;
        internal int cardSumValue = 0;


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

    }
}