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
    /// Port of class Tournement
    /// <see cref="https://github.com/heinrichelsigan/schnapslet/wiki"/>
    /// </summary>
    public class Tournement
    {
        public int GamblerTPoints { get; set; } = 7;
        public int ComputerTPoints { get; set; } = 7;

        public List<Point> tHistory = new List<Point>();

        public PLAYERDEF NextGameGiver { get; set; } = PLAYERDEF.COMPUTER;

        public PLAYERDEF NextGameStarter {
            get
            {
                if (NextGameGiver == PLAYERDEF.HUMAN)
                    return PLAYERDEF.COMPUTER;
                if (NextGameGiver == PLAYERDEF.COMPUTER)
                    return PLAYERDEF.HUMAN;
                return PLAYERDEF.UNKNOWN; // TODO: ReThink Unknown never occurred state
            }
        }

        public PLAYERDEF WonTournement
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
                if ((WonTournement == PLAYERDEF.COMPUTER && GamblerTPoints == 7) ||
                    (WonTournement == PLAYERDEF.HUMAN && ComputerTPoints == 7))
                    return true;
                return false;
            }
        } 
        

        public Tournement()
        {
            GamblerTPoints = 7;
            ComputerTPoints = 7;
            tHistory = new List<Point>();
            Point ptStart = new Point(GamblerTPoints, ComputerTPoints);
            tHistory.Add(ptStart);
            Random random = new Random();
            int rand = random.Next();
            NextGameGiver = (rand % 2 == 0) ? PLAYERDEF.HUMAN : PLAYERDEF.COMPUTER;
        }
        
        public Tournement(PLAYERDEF nextGiver) : this()
        {
            NextGameGiver = nextGiver;
        }

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