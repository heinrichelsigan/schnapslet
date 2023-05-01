using SchnapsNet.ConstEnum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Web;
using System.Web.UI.WebControls;

namespace SchnapsNet.Models
{
    /// <summary>
    /// Port of class Player
    /// </summary>
    public class Player
    {
        volatile bool begins;    // does this Player begin ?
        public Card[] hand = new Card[5];  // Cards in Players Hand
        // not implemented yet !
        // Card hits[] = new Card[10]; // hits made by this player

        public bool hasClosed = false;
        public int points = 0;             // points made by this player
                                           // char pairs[] = {'n', 'n', 'n', 'n'};
        internal PairCard[] pairs = new PairCard[2];
        public char[] handpairs = { 'n', 'n' };
        int[] colorHitArray = { 0, 0, 0, 0, 0 };
        public int playerOptions = 0;

        public int stitchCount = 0;
        public Dictionary<int, TwoCards> cardStitches = new Dictionary<int, TwoCards>();
        // Resources r;
        HttpContext context;

        #region properties

        /// <summary>
        /// HasPair - checks if a player (the player or computer) has a Queen & King pair        
        /// returns number of pairs, that player has (1 or 2)
        /// </summary>
        public int HasPair
        {
            get
            {
                int i, hpidx = 0;
                handpairs[0] = 'n';
                handpairs[1] = 'n';
                pairs[0] = null;
                pairs[1] = null;   

                for (i = 0; i < 4; i++)
                {
                    if (hand[i].CardColor.GetChar() == hand[i + 1].CardColor.GetChar())
                    {
                        if ((hand[i].CardValue.GetValue() + hand[i + 1].CardValue.GetValue()) == 7)
                        {
                            if (hpidx == 0)
                            {
                                pairs[0] = new PairCard(hand[i], hand[i + 1]);
                            }
                            if (hpidx == 1)
                            {
                                pairs[1] = new PairCard(hand[i], hand[i + 1]);
                            }

                            handpairs[hpidx++] = hand[i].CardColor.GetChar();
                        }
                    }
                }
                return hpidx;
            }
        }

        /// <summary>
        /// CanChangeAtou
        /// returns if can change atou, return index of card, that is Jack to change, otherwise -1
        /// </summary>
        public int CanChangeAtou
        {
            get
            {
                for (int i = 0; i < 5; i++)
                {
                    if ((hand[i].isAtou) && (hand[i].CardValue == CARDVALUE.JACK))
                    {
                        return i;
                    }
                }
                return (-1);
            }
        }
        
        #endregion properties

        /// <summary>
        /// Constructor of Player
        /// </summary>
        /// <param name="c">HttpContext c</param>
        public Player(HttpContext c)
        {            
            this.context = c;
            // this.r = c.getResources();
            hasClosed = false;
            for (int i = 0; i < 5; i++)
            {
                hand[i] = new Card(c);
                colorHitArray[i] = (-1);
            }
            cardStitches = new Dictionary<int, TwoCards>();
        }

        /// <summary>
        /// Constructor of Player
        /// </summary>
        /// <param name="starts">true if this player starts the game, otherwise false</param>
        /// <param name="c">HttpContext of app</param>
        public Player(bool starts, HttpContext c) : this(c)
        {
            this.begins = starts;
        }

        /// <summary>
        /// Stop - Destructor for hand cards of Player
        /// </summary>
        public void Stop()
        {
            int i;
            for (i = 0; i < 5; i++)
                hand[i] = null;

            stitchCount = 0;
            cardStitches.Clear();
        }
        
        /// <summary>
        /// Show players hand cards
        /// </summary>
        /// <returns>concatenate String of all players hand cards</returns>
        public String ShowHand()
        {
            String retVal = "";
            for (int j = 0; j < 5; j++)
                retVal = retVal + hand[j].getName() + " ";
            return retVal;
        }

        /// <summary>
        /// SortHand - (bubble)sorts player hand
        /// </summary>
        public void SortHand()
        { 
            int j, k, min, mark;
            Card tmpCard;
            for (k = 0; k < 4; k++) // Bubble
            { 
                min = 20;
                mark = -1;
                for (j = k; j < 5; j++) // Bubble
                {
                    if (hand[j].intern < min)
                    {
                        min = hand[j].intern;
                        mark = j;
                    }
                }
                if (mark > 0)
                {
                    tmpCard = hand[mark];
                    hand[mark] = hand[k];
                    hand[k] = tmpCard;
                }
            }
        }
        
        /// <summary>
        /// AssignCard assigns a new card to players hand
        /// </summary>
        /// <param name="gotCard">the new card to assign</param>
        public void AssignCard(Card gotCard)
        {
            int i = 0;
            while ((i < 5) && (hand[i] != null) && (hand[i].isValidCard)) i++;
            if (i < 5)
            {
                hand[i] = new Card(gotCard, context);
            }
        }

        /// <summary>
        /// IsValidInColorHitsContext checks it a card is valid in color hit rule context
        /// </summary>
        /// <param name="nynum">number of the played card</param>
        /// <param name="aCard">another card</param>
        /// <returns>true if played card is valid in color hit rule context, otherwise false</returns>
        public bool IsValidInColorHitsContext(int nynum, Card aCard)
        {
            int i = 0;
            int j = 0;
            int max = -1;
            for (i = 0; i < 5; i++)
            {
                // valid Card -> PRI 0
                if (hand[i] == null || !hand[i].isValidCard)
                {
                    colorHitArray[i] = (-1);
                }
                else if (hand[i].isValidCard)
                {
                    colorHitArray[i] = 0;
                    if (max < 0) max = 0;

                    // has same color -> PRI 2
                    if ((hand[i].CardColor.GetChar()) != aCard.CardColor.GetChar())
                    {
                        // ich kann mit Atou stechen -> PRI 1
                         if ((!aCard.isAtou) && (hand[i].isAtou))
                        { 
                            colorHitArray[i] = 1;
                            if (max < 1) max = 1;
                        }
                    }
                    else if ((hand[i].CardColor.GetChar()) == aCard.CardColor.GetChar())
                    {

                        colorHitArray[i] = 2;
                        if (max < 2) max = 2;

                        // can hit -> PRI 3
                        if (hand[i].CardValue.GetValue() > aCard.CardValue.GetValue())
                        {
                            colorHitArray[i] = 3;
                            if (max < 3) max = 3;
                        }
                    }
                }
            }

            return (colorHitArray[nynum] == max); // return true;
                                                  // return false;
        }


        /// <summary>
        /// preferedInColorHitsContext calculate best card number, that is valid s valid in color hit rule context
        /// </summary>
        /// <param name="aCard">another card</param>
        /// <returns>number of best card to be played, that is valid in color hit rule context</returns>
        public int preferedInColorHitsContext(Card aCard)
        {
            int i = 0, j = 0, tmp = -1, min = -1, max = -1, mark = -1, markMin = -1, markMax = -1;

            for (i = 0; i < 5; i++)
            {
                if (!hand[i].isValidCard)
                {
                    tmp = -1;
                }
                else if (hand[i].isValidCard)
                {
                    // valid card => max = CardValue
                    tmp = hand[i].CardValue.GetValue();

                    if (hand[i].CardColor.GetChar() != aCard.CardColor.GetChar()) 
                    {
                        // not same colors && atou => max is atou card value
                        if ((hand[i].isAtou) && (!aCard.isAtou)) // can hit with atou -> PRI 1                        
                            tmp = 11 + hand[i].CardValue.GetValue();
                    }
                    else if (hand[i].CardColor.GetChar() == aCard.CardColor.GetChar())
                    {
                        // same colors
                        tmp = 22 + hand[i].CardValue.GetValue();

                        if (hand[i].CardValue.GetValue() > aCard.CardValue.GetValue())
                            tmp = 33 + hand[i].CardValue.GetValue();
                    }
                }
                colorHitArray[i] = tmp;
                if (tmp > max) max = tmp;
                if (tmp >= 0 && min < tmp) min = tmp;
            }

            if (max > 0 && max <= 11)
            {
                min = max; markMin = -1; markMax = -1;
                for (j = 0; j < 5; j++)
                {
                    if (colorHitArray[j] >= 0 && colorHitArray[j] <= 11 && colorHitArray[j] == max)
                        markMax = j;
                    if (colorHitArray[j] >= 0 && colorHitArray[j] < 11 && colorHitArray[j] < min)
                    {
                        min = colorHitArray[j];
                        markMin = j;
                    }
                }
                return (markMin >= 0) ? markMin : markMax;
            }
            if (max > 11 && max <= 22) // can only hit with atou
            {
                min = max; markMin = -1; markMax = -1;
                Card maxBestCard = null, minBestCard = null, tmpBestCard = null;
                for (j = 0; j < 5; j++)
                {
                    if (colorHitArray[j] > 11 && colorHitArray[j] <= 22 && colorHitArray[j] == max)
                    {
                        markMax = j;
                        maxBestCard = hand[j];
                        tmpBestCard = hand[j];
                    }
                    if (colorHitArray[j] > 11 && colorHitArray[j] < 22 && colorHitArray[j] < min)
                    {
                        markMin = j;
                        min = colorHitArray[j];
                        minBestCard = hand[j];
                        tmpBestCard = hand[j];
                    }
                }
                mark = (markMin >= 0) ? markMin : markMax;
                int tmpHasPair = HasPair;
                if ((tmpHasPair == 1 && pairs[0].card1st.CardColor == tmpBestCard.CardColor) ||
                    (tmpHasPair == 2 && pairs[1].card1st.CardColor == tmpBestCard.CardColor))
                {
                    markMax = -1; markMin = -1;
                    for (j = 0; j < 5; j++)
                    {
                        if (colorHitArray[j] > 11 && colorHitArray[j] <= 22 && colorHitArray[j] > 20)
                            markMax = j;
                        if (colorHitArray[j] > 11 && colorHitArray[j] <= 22 && colorHitArray[j] < 14)
                            markMin = j;
                    }
                    if (markMax >= 0)
                        return markMax;
                    if (markMin >= 0)
                        return markMin;
                    return mark;
                }
                if (markMin >= 0)
                {
                    if (maxBestCard.CardValue == CARDVALUE.ACE)
                        return markMin;
                    if (maxBestCard.CardValue == CARDVALUE.TEN)
                        return markMax;
                    return markMin;
                }
                return markMax;
            }
            if (max > 22 && max <= 33)
            {
                min = max; markMin = -1; markMax = -1;
                Card maxBestCard = null, minBestCard = null, tmpBestCard = null;
                for (j = 0; j < 5; j++)
                {
                    if (colorHitArray[j] > 22 && colorHitArray[j] <= 33 && colorHitArray[j] == max)
                    {
                        markMax = j;
                        maxBestCard = hand[j];
                        tmpBestCard = hand[j];
                    }
                    if (colorHitArray[j] > 22 && colorHitArray[j] < 33 && colorHitArray[j] < min)
                    {
                        markMin = j;
                        min = colorHitArray[j];
                        minBestCard = hand[j];
                        tmpBestCard = hand[j];
                    }
                }
                mark = (markMin >= 0) ? markMin : markMax;
                int tmpHasPair = HasPair;
                if ((tmpHasPair == 1 && pairs[0].card1st.CardColor == tmpBestCard.CardColor) ||
                    (tmpHasPair == 2 && pairs[1].card1st.CardColor == tmpBestCard.CardColor))
                {
                    if (minBestCard.CardValue == CARDVALUE.JACK)
                        return markMin;
                }
                return (markMin >= 0) ? markMin : markMax;
            }
            if (max > 33 && max <= 44)
            {
                min = max; markMin = -1; markMax = -1;
                Card maxBestCard = null, minBestCard = null, tmpBestCard = null;
                for (j = 0; j < 5; j++)
                {
                    if (colorHitArray[j] > 33 && colorHitArray[j] <= 44 && colorHitArray[j] == max)
                    {
                        markMax = j;
                        maxBestCard = hand[j];
                        tmpBestCard = hand[j];
                    }
                    if (colorHitArray[j] > 33 && colorHitArray[j] < 44 && colorHitArray[j] < min)
                    {
                        min = colorHitArray[j];
                        markMin = j;
                        minBestCard = hand[j];
                        tmpBestCard = hand[j];
                    }
                }
                return markMax;
            }

            return mark;
        }


        /// <summary>
        /// bestInColorHitsContext calculate best card number, that is valid s valid in color hit rule context
        /// </summary>
        /// <param name="aCard">another card</param>
        /// <returns>number of best card to be played, that is valid in color hit rule context</returns>
        [Obsolete("bestInColorHitsContext(Card aCard) is obsolete, use preferedInColorHitContext(Card aCard", false)]
        public int bestInColorHitsContext(Card aCard)
        {
            int i = 0, j = 0, mark = -1, max = -1;

            for (i = 0; i < 5; i++)
            {
                if (!hand[i].isValidCard)
                {
                    colorHitArray[i] = (-1);
                }
                else if (hand[i].isValidCard)
                {
                    colorHitArray[i] = 0;
                    if (max < 0) max = 0;

                    if (hand[i].CardColor.GetChar() != aCard.CardColor.GetChar())
                    {
                        // not same colors
                        if ((hand[i].isAtou) && (!aCard.isAtou))
                        {
                            // ich kann mit Atou stechen -> PRI 1
                            colorHitArray[i] = 1;
                            if (max < 1) max = 1;
                        }
                    }
                    else if (hand[i].CardColor.GetChar() == aCard.CardColor.GetChar())
                    {
                        // same colors
                        colorHitArray[i] = 2;
                        if (max < 2) max = 2;

                        if (hand[i].CardValue.GetValue() > aCard.CardValue.GetValue())
                        {
                            colorHitArray[i] = 3;
                            if (max < 3) max = 3;
                        }
                    }
                }
            }

            for (j = 0; j < 5; j++)
            {
                if (colorHitArray[j] == max)
                {
                    if (mark < 0) mark = j;
                    else
                    {
                        switch (max)
                        {
                            case 0:
                            case 2:
                                if (hand[mark].CardValue.GetValue() < hand[j].CardValue.GetValue())
                                    mark = j;
                                break;
                            case 1:
                            case 3:
                                if (hand[mark].CardValue.GetValue() < hand[j].CardValue.GetValue())
                                    mark = j;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return mark;
        }


        /// <summary>
        /// isColorHitValid
        /// </summary>
        /// <param name="cidx">number of the played card</param>
        /// <param name="otherCard">otherCard - another card</param>
        /// <returns>true if played card is valid in color hit rule context, otherwise false</returns>
        [Obsolete("isColorHitValid(int cidx, Card otherCard) is marked obsolete.", true)]
        public bool isColorHitValid(int cidx, Card otherCard)
        {
            int i;
            // gleiche Farbe und groesser -> OK
            if (hand[cidx].hitsValue(otherCard))
                return true;

            // Kann ich mit Farbe stechen ->
            for (i = 0; i < 5; i++)
            {
                if (hand[i].isValidCard) 
                    if (hand[i].hitsValue(otherCard))
                        return false;
            }

            // Ist die Karte gleicher Farbe -> OK
            if (hand[cidx].CardColor.GetChar() == otherCard.CardColor.GetChar())
                return true;

            for (i = 0; i < 5; i++)
            {
                if (hand[i].isValidCard)
                {
                    if (hand[i].CardColor.GetChar() == otherCard.CardColor.GetChar())
                        return false;
                }
            }

            if (hand[cidx].isAtou)
                return true;

            for (i = 0; i < 5; i++)
            {
                if (hand[i].isAtou)
                    return false;
            }

            return true;
        }

    }
}