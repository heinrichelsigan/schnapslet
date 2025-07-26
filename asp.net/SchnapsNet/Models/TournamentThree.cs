using SchnapsNet.ConstEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SchnapsNet.Models;
using System.Runtime.Serialization.Formatters;
using System.Drawing;
using System.EnterpriseServices;

namespace SchnapsNet.Models
{
    /// <summary>
    /// Port of class TournamentThree
    /// <see cref="https://github.com/heinrichelsigan/schnapslet/wiki"/>
    /// </summary>
    [Serializable]
    public class TournamentThree : Tournament
    {
        public new int GamblerTPoints { get; set; } = Constants.PLAY_DOWN_FARMER;
        public int Computer1TPoints { get; set; } = Constants.PLAY_DOWN_FARMER;
        public int Computer2TPoints { get; set; } = Constants.PLAY_DOWN_FARMER;

        public List<Triplet> threeHistory = new List<Triplet>();

        public new PLAYERDEF NextGameGiver { get; set; } = PLAYERDEF.COMPUTER1;

        public new PLAYERDEF NextGameStarter 
        {
            get
            {
                if (NextGameGiver == PLAYERDEF.HUMAN)
                    return PLAYERDEF.COMPUTER1;
                if (NextGameGiver == PLAYERDEF.COMPUTER1)
                    return PLAYERDEF.COMPUTER2;
                if (NextGameGiver == PLAYERDEF.COMPUTER2)
                    return PLAYERDEF.HUMAN;
                return PLAYERDEF.UNKNOWN; // TODO: ReThink Unknown never occurred state
            }
        }

        public new PLAYERDEF WonTournament
        {
            get
            {
                if (Computer1TPoints <= 0 && GamblerTPoints > 0 && Computer2TPoints > 0)
                    return PLAYERDEF.COMPUTER1;
                if (Computer2TPoints <= 0 && GamblerTPoints > 0 && Computer1TPoints > 0)
                    return PLAYERDEF.COMPUTER2;
                if (GamblerTPoints <= 0 && Computer1TPoints > 0 && Computer2TPoints > 0)
                    return PLAYERDEF.HUMAN;
                return PLAYERDEF.UNKNOWN;
            }
        }

        public new bool Taylor
        {
            get
            {
                if ((WonTournament == PLAYERDEF.COMPUTER1 && GamblerTPoints == PlayDownFrom && Computer2TPoints == PlayDownFrom) ||
                    (WonTournament == PLAYERDEF.COMPUTER2 && GamblerTPoints == PlayDownFrom && Computer1TPoints == PlayDownFrom) ||
                    (WonTournament == PLAYERDEF.HUMAN && Computer1TPoints == PlayDownFrom && Computer2TPoints == PlayDownFrom))
                    return true;
                return false;
            }
        }


        /// <summary>
        /// default constructor for TournamentThree
        /// </summary>
        public TournamentThree(int playDownFrom = Constants.PLAY_DOWN_FROM) : base(playDownFrom)
        {
            GamblerTPoints = playDownFrom; // Constants.PLAY_DOWN_MOCK;
            Computer1TPoints = playDownFrom; // Constants.PLAY_DOWN_MOCK;            
            Computer2TPoints = playDownFrom; // Constants.PLAY_DOWN_MOCK;  

            threeHistory = new List<Triplet>();
            Triplet triStart = new Triplet(GamblerTPoints, Computer1TPoints, Computer2TPoints);
            threeHistory.Add(triStart);
            Random random = new Random();
            int rand = random.Next();
            NextGameGiver = (rand % 3 == 0) ? PLAYERDEF.HUMAN :
                ((rand % 3 == 1) ? PLAYERDEF.COMPUTER1 : PLAYERDEF.COMPUTER2);
//#if MOCK
//            NextGameGiver = PLAYERDEF.HUMAN;
//            GamblerTPoints = Constants.PLAY_DOWN_MOCK;
//            ComputerTPoints = Constants.PLAY_DOWN_MOCK;
//#endif
        }

        /// <summary>
        /// ctor of Tournament with <see cref="PLAYERDEF"/> for next giver
        /// </summary>
        /// <param name="nextGiver"><see cref="PLAYERDEF">PLAYERDEF.HUMAN or PLAYERDEF.COMPUTER</see></param>
        public TournamentThree(PLAYERDEF nextGiver, int playDownFrom = Constants.PLAY_DOWN_FROM) : this(playDownFrom)
        {
            NextGameGiver = nextGiver;
        }

        /// <summary>
        /// Add points
        /// </summary>
        /// <param name="tournementPts"></param>
        /// <param name="whoWon"></param>
        /// <exception cref="InvalidProgramException"></exception>
        public new void AddPointsRotateGiver(int tournementPts, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN)
        {
            if (whoWon == PLAYERDEF.HUMAN)
            {
                GamblerTPoints -= tournementPts;
            }
            else if (whoWon == PLAYERDEF.COMPUTER1)
            {
                Computer1TPoints -= tournementPts;
            }
            else if (whoWon == PLAYERDEF.COMPUTER2)
            {
                Computer2TPoints -= tournementPts;
            }
            Triplet triStart = new Triplet(GamblerTPoints, Computer1TPoints, Computer2TPoints);
            threeHistory.Add(triStart);
            NextGameGiver = this.NextGameStarter;
            if (NextGameGiver == PLAYERDEF.UNKNOWN)
                throw new InvalidProgramException("Unknown game state to determine next giver");
        }

    }
}