
namespace RL_QLearning
{
    public class Cell
    {
        private Values Values;      //struct to hold values ateach direction of the cell
        private bool Obstacle;      //indicates if the cell is obstacle or not
        private bool Terminal;      //indicates if the cell is a terminal or not

        //initializes cell to all 0 values, not terminal, not obstacle
        //later cells will change when setting matrix to that given by the professor
        public Cell()
        {
            Obstacle = false;
            Terminal = false;
            Values.West = 0;
            Values.North = 0;
            Values.East = 0;
            Values.South = 0;
        }

        #region Value Set functions
        //sets value at west direction of the calling cell to the passed value
        public void AssignValueAtWest(double west)
        {
            Values.West = west;
        }

        //sets value at north direction of the calling cell to the passed value
        public void AssignValueAtNorth(double north)
        {
            Values.North = north;
        }

        //sets value at east direction of the calling cell to the passed value
        public void AssignValueAtEast(double east)
        {
            Values.East = east;
        }

        //sets value at south direction of the calling cell to the passed value
        public void AssignValueAtSouth(double south)
        {
            Values.South = south;
        }
        #endregion

        //Assignes the cell to a terminal by setting the values at each direction to the 100 terminal value defined in Constants
        //Will be used when setting a cell to a terminal cell when setting the matrix to that given by the professor
        public void AssignTerminal()
        {
            Terminal = true;
            Values.West = Constants.TERMINALREWARD;
            Values.North = Constants.TERMINALREWARD;
            Values.East = Constants.TERMINALREWARD;
            Values.South = Constants.TERMINALREWARD;
        }

        //returns if cell was terminal or not
        public bool IsTerminal()
        {
            return Terminal;
        }

        //sets cell to an obstacle
        public void AssignObstacle()
        {
            Obstacle = true;
        }

        //returns if cell was obstacle or not
        public bool IsObstacle()
        {
            return Obstacle;
        }

        //returns values at each direction of a cell
        public Values GetValues()
        {
            return Values;
        }
    }
}
