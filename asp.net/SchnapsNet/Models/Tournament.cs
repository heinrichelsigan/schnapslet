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

namespace SchnapsNet.Models
{
    /// <summary>
    /// Port of class Tournament
    /// <see cref="https://github.com/heinrichelsigan/schnapslet/wiki"/>
    /// </summary>
    public class Tournament
    {
        public int GamblerTPoints { get; set; } = Constants.PLAY_DOWN_FROM;
        public int ComputerTPoints { get; set; } = Constants.PLAY_DOWN_FROM;

        public List<Point> tHistory = new List<Point>();

        public PLAYERDEF NextGameGiver { get; set; } = PLAYERDEF.COMPUTER;

        public PLAYERDEF NextGameStarter 
        {
            get
            {
                if (NextGameGiver == PLAYERDEF.HUMAN)
                    return PLAYERDEF.COMPUTER;
                if (NextGameGiver == PLAYERDEF.COMPUTER)
                    return PLAYERDEF.HUMAN;
                return PLAYERDEF.UNKNOWN; // TODO: ReThink Unknown never occurred state
            }
        }

        public PLAYERDEF WonTournament
        {
            get
            {
                if (ComputerTPoints <= 0 && GamblerTPoints > 0)
                    return PLAYERDEF.COMPUTER;
                if (GamblerTPoints <= 0 && ComputerTPoints > 0)
                    return PLAYERDEF.HUMAN;
                return PLAYERDEF.UNKNOWN;
            }
        }

        public bool Taylor
        {
            get
            {
                if ((WonTournament == PLAYERDEF.COMPUTER && GamblerTPoints == 7) ||
                    (WonTournament == PLAYERDEF.HUMAN && ComputerTPoints == 7))
                    return true;
                return false;
            }
        } 
        

        /// <summary>
        /// default constructor for Tournament
        /// </summary>
        public Tournament()
        {
            ComputerTPoints = Constants.PLAY_DOWN_FROM; // Constants.PLAY_DOWN_MOCK;
#if MOCK
            GamblerTPoints = Constants.PLAY_DOWN_MOCK;
#else
            GamblerTPoints = Constants.PLAY_DOWN_FROM; // Constants.PLAY_DOWN_MOCK;
#endif
            tHistory = new List<Point>();
            Point ptStart = new Point(GamblerTPoints, ComputerTPoints);
            tHistory.Add(ptStart);
            Random random = new Random();
            int rand = random.Next();
            NextGameGiver = (rand % 2 == 0) ? PLAYERDEF.HUMAN : PLAYERDEF.COMPUTER;
        }

        /// <summary>
        /// ctor of Tournament with <see cref="PLAYERDEF"/> for next giver
        /// </summary>
        /// <param name="nextGiver"><see cref="PLAYERDEF">PLAYERDEF.HUMAN or PLAYERDEF.COMPUTER</see></param>
        public Tournament(PLAYERDEF nextGiver) : this()
        {
            NextGameGiver = nextGiver;
        }

        /// <summary>
        /// Add points
        /// </summary>
        /// <param name="tournementPts"></param>
        /// <param name="whoWon"></param>
        /// <exception cref="InvalidProgramException"></exception>
        public void AddPointsRotateGiver(int tournementPts, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN)
        {
            if (whoWon == PLAYERDEF.HUMAN)
            {
                GamblerTPoints -= tournementPts;
            }
            else if (whoWon == PLAYERDEF.COMPUTER)
            {
                ComputerTPoints -= tournementPts;
            }
            Point ptStart = new Point(GamblerTPoints, ComputerTPoints);
            tHistory.Add(ptStart);
            if (NextGameGiver == PLAYERDEF.COMPUTER)
                NextGameGiver = PLAYERDEF.HUMAN;
            else if (NextGameGiver == PLAYERDEF.HUMAN)
                NextGameGiver = PLAYERDEF.COMPUTER;
            else if (NextGameGiver == PLAYERDEF.UNKNOWN)
                throw new InvalidProgramException("Unknown game state to determine next giver");
        }

    }
}