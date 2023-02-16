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
        PairCard[] pairs = new PairCard[2];
        public char[] handpairs = { 'n', 'n' };
        int[] colorHitArray = { 0, 0, 0, 0, 0 };
        public int playerOptions = 0;
        // Resources r;
        HttpContext context;

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
        /// stop - Destructor for hand cards of Player
        /// </summary>
        public void stop()
        {
            int i;
            for (i = 0; i < 5; i++)
                hand[i] = null;
        }
        
        /// <summary>
        /// show players hand cards
        /// </summary>
        /// <returns>concatenate String of all players hand cards</returns>
        public String showHand()
        {
            String retVal = "";
            for (int j = 0; j < 5; j++)
                retVal = retVal + hand[j].getName() + " ";
            return retVal;
        }

        /// <summary>
        /// sortHand - (bubble)sorts player hand
        /// </summary>
        public void sortHand()
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
        /// canChangeAtou
        /// </summary>
        /// <returns>if can change atou, return index of card, that is Jack to change, otherwiese -1</returns>
        public int canChangeAtou()
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

        /// <summary>
        /// has20 - checks if a player (the player or computer) has a Queen & King pair        
        /// </summary>
        /// <returns>number of pairs, that player has(1 or 2)</returns>
        public int has20()
        {
            int i, hpidx = 0;
            handpairs[0] = 'n';
            handpairs[1] = 'n';

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

        /// <summary>
        /// assignCard assigns a new card to players hand
        /// </summary>
        /// <param name="gotCard">the new card to assign</param>
        public void assignCard(Card gotCard)
        {
            int i = 0;
            while ((i < 5) && (hand[i] != null) && (hand[i].isValidCard)) i++;
            if (i < 5)
            {
                hand[i] = new Card(gotCard, context);
            }
        }

        /// <summary>
        /// isInColorHitsContextValid checks it a card is valid in color hit rule context
        /// </summary>
        /// <param name="nynum">number of the played card</param>
        /// <param name="aCard">another card</param>
        /// <returns>true if played card is valid in color hit rule context, otherwise false</returns>
        public bool isInColorHitsContextValid(int nynum, Card aCard)
        {
            int i = 0;
            int j = 0;
            int max = -1;
            for (i = 0; i < 5; i++)
            {
                // ist gueltige Karte -> PRI 0
                if (hand[i] == null || !hand[i].isValidCard)
                {
                    colorHitArray[i] = (-1);
                }
                else if (hand[i].isValidCard)
                {

                    colorHitArray[i] = 0;
                    if (max < 0) max = 0;

                    // ich hab gleich Farbe -> PRI 2
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

                        // ich kann aber auch noch stechen -> PRI 3
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
        /// bestInColorHitsContext calculate best card number, that is valid s valid in color hit rule context
        /// </summary>
        /// <param name="aCard">another card</param>
        /// <returns>number of best card to be played, that is valid in color hit rule context</returns>
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
        [Obsolete("isColorHitValid(int cidx, Card otherCard) is marked obsolete.", false)]
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