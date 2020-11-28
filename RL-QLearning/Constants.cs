
namespace RL_QLearning
{
    public static class Constants
    {
        // Matrix dimensions
        public const int ROWS = 6;
        public const int COLS = 5;

        // Windy situation scenario
        public const double SLIPPROBABILITY = 0.15;                         // Given (Rel. Left/ Right)
        public const double MOVEPROBABILITY = 1 - (2 * SLIPPROBABILITY);    // Complimentary (Rel. Straight)

        // Drifting Range, will be used to check against a random number to see if there will be slipping
        public const double MINLIMIT = 0;
        public const double LEFTLIMIT = SLIPPROBABILITY * 100;                                  //15%
        public const double STRAIGHTLIMIT = (SLIPPROBABILITY + MOVEPROBABILITY) * 100;          //85%
        public const double RIGHTLIMIT = ((SLIPPROBABILITY * 2) + MOVEPROBABILITY) * 100;       //100%

        // Num of Driections (W,N,E,S)
        public const int NUMOFDIRECTIONS = 4;

        // Reward/ -Utility Cost to move to directions
        public const double WEST = -2.0;
        public const double NORTH = -3.0;
        public const double EAST = -2.0;
        public const double SOUTH = -1.0;

        // Q-value computation variable
        public const double GAMMA = 0.9;

        // Exploration to Exploitation ratio; 95% optimal case, 5% new random case
        public const double EPSILON = 0.05;                                 // 0.05 or 5%

        // Reinforcement Learning Variables
        public const int TRAILS = 10000;                                    // Number of trails RL will run
        public const int STEPS = 100;                                       // Steps to abort if not reached terminal

        // Terminal Reward
        public const int TERMINALREWARD = 100;

        // For Printing Optimal Policy Matrix
        public const string WESTDIR = "<<<<<";
        public const string NORTHDIR = "^^^^^";
        public const string EASTDIR = ">>>>>";
        public const string SOUTHDIR = "vvvvv";
    }

    public partial class Matrix
    {
        // will initialize terminal square and obstacle squares according to maze given by professor
        public Matrix(bool DefaultMap) : this()
        {
            if (DefaultMap)
            {
                AssignTerminalAt(2, 2);
                
                AssignObstacleAt(1, 1);
                AssignObstacleAt(1, 2);
                AssignObstacleAt(2, 1);
                AssignObstacleAt(3, 1);
                AssignObstacleAt(3, 2);
                AssignObstacleAt(4, 1);
            }
        }
    }
}
