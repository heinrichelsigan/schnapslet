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
    [Serializable]
    public class Tournament
    {
        public virtual int PlayDownFrom { get; set; } = Constants.PLAY_DOWN_FROM;
        public virtual int GamblerTPoints { get; set; } = Constants.PLAY_DOWN_FROM;
        public virtual int ComputerTPoints { get; set; } = Constants.PLAY_DOWN_FROM;       

        public List<Point> tHistory = new List<Point>();

        public virtual PLAYERDEF NextGameGiver { get; set; } = PLAYERDEF.COMPUTER;

        public virtual PLAYERDEF NextGameStarter 
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

        public virtual PLAYERDEF WonTournament
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

        public virtual bool Taylor
        {
            get
            {
                if ((WonTournament == PLAYERDEF.COMPUTER && GamblerTPoints == PlayDownFrom) ||
                    (WonTournament == PLAYERDEF.HUMAN && ComputerTPoints == PlayDownFrom))
                    return true;
                return false;
            }
        } 
        

        /// <summary>
        /// default constructor for Tournament
        /// </summary>
        public Tournament(int playDownFrom = Constants.PLAY_DOWN_FROM)
        {
            PlayDownFrom = playDownFrom;
            GamblerTPoints = PlayDownFrom; // Constants.PLAY_DOWN_MOCK;
            ComputerTPoints = PlayDownFrom; // Constants.PLAY_DOWN_MOCK;            

            tHistory = new List<Point>();
            Point ptStart = new Point(GamblerTPoints, ComputerTPoints);
            tHistory.Add(ptStart);
            Random random = new Random();
            int rand = random.Next();
            NextGameGiver = (rand % 2 == 0) ? PLAYERDEF.HUMAN : PLAYERDEF.COMPUTER;
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
        public Tournament(PLAYERDEF nextGiver, int playDownFrom = Constants.PLAY_DOWN_FROM) : this(playDownFrom)
        {
            NextGameGiver = nextGiver;
        }

        /// <summary>
        /// Add points
        /// </summary>
        /// <param name="tournementPts"></param>
        /// <param name="whoWon"></param>
        /// <exception cref="InvalidProgramException"></exception>
        public virtual void AddPointsRotateGiver(int tournementPts, PLAYERDEF whoWon = PLAYERDEF.UNKNOWN)
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
            NextGameGiver = this.NextGameStarter;
            if (NextGameGiver == PLAYERDEF.UNKNOWN)
                throw new InvalidProgramException("Unknown game state to determine next giver");
        }

    }
}