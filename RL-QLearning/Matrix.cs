using System;

namespace RL_QLearning
{
    public partial class Matrix
    {
        //create matrix to hold values
        private readonly Cell[,] _Matrix;

        //constructor to create and initialize cells of the matrix
        public Matrix()
        {
            _Matrix = new Cell[Constants.ROWS, Constants.COLS];
            for (int i = 0; i < Constants.ROWS; ++i)
            {
                for (int j = 0; j < Constants.COLS; ++j)
                {
                    _Matrix[i, j] = new Cell();
                }
            }
        }

        #region Value Set functions
        //set value of matrix cell at west to the value passed
        public void AssignWestValueAt(int x, int y, double value)
        {
            _Matrix[x, y].AssignValueAtWest(value);
        }

        //set value of matrix cell at north to the value passed
        public void AssignNorthValueAt(int x, int y, double value)
        {
            _Matrix[x, y].AssignValueAtNorth(value);
        }

        //set value of matrix cell at east to the value passed
        public void AssignEastValueAt(int x, int y, double value)
        {
            _Matrix[x, y].AssignValueAtEast(value);
        }

        //set value of matrix cell at south to the value passed
        public void AssignSouthValueAt(int x, int y, double value)
        {
            _Matrix[x, y].AssignValueAtSouth(value);
        }
        #endregion

        #region Value Get functions
        //get value of matrix cell at west
        public double GetWestValueAt(int x, int y)
        {
            return _Matrix[x, y].GetValues().West;
        }

        //set value of matrix cell at north to the value passed
        public double GetNorthValueAt(int x, int y)
        {
            return _Matrix[x, y].GetValues().North;
        }

        //set value of matrix cell at east to the value passed
        public double GetEastValueAt(int x, int y)
        {
            return _Matrix[x, y].GetValues().East;
        }

        //set value of matrix cell at south to the value passed
        public double GetSouthValueAt(int x, int y)
        {
            return _Matrix[x, y].GetValues().South;
        }
        #endregion

        //sets the cell at the passed coordinates to an obstacle
        public void AssignObstacleAt(int row, int col)
        {
            _Matrix[row, col].AssignObstacle();
        }

        //returns whether cell is an obstacle or not
        public bool IsObstacleAt(int row, int col)
        {
            // We assume everything outside (Out of bound) of the grid is obstacle
            if (row < 0 || col < 0 || row >= Constants.ROWS || col >= Constants.COLS)
            {
                return true;
            }
            //if not out of bounds, return bool of cell struct to see if obstacle or not
            return _Matrix[row, col].IsObstacle();
        }

        //sets the cell at the passed coordinates to a terminal
        public void AssignTerminalAt(int row, int col)
        {
            _Matrix[row, col].AssignTerminal();
        }

        //returns whether cell is a terminal or not
        public bool IsTerminalAt(int row, int col)
        {
            // We assume everything outside (Out of bound) of the grid cannot be reward 
            if (row < 0 || col < 0 || row >= Constants.ROWS || col >= Constants.COLS)
            {
                return false;
            }
            //if not out of bounds, return bool of cell struct to see if terminal or not
            return _Matrix[row, col].IsTerminal();
        }

        //Prints matrix of Q(s,a)
        public void PrintQMatrix()
        {
            // Uses Unicode charset (Box Drawing characters)

            // Top-line of the Table
            Console.Write("┌");
            for (int i = 0; i < Constants.COLS - 1; ++i)
            {
                Console.Write("─────────────────────┬");
            }
            Console.Write("─────────────────────┐");
            Console.WriteLine();

            // Loop through table values inside to print all North values first
            for (int i = 0; i < Constants.ROWS; ++i)
            {
                Console.Write("│");
                for (int j = 0; j < Constants.COLS; ++j)
                {
                    //print space if obstacle or terminal
                    if (_Matrix[i, j].IsObstacle() || _Matrix[i, j].IsTerminal())
                    {
                        Console.Write("                     │");
                    }
                    //otherwise print the value at north of the cell of the matirx
                    else
                    {
                        Console.Write($"       {_Matrix[i, j].GetValues().North,7:####0.0}       │");
                    }
                }
                Console.WriteLine();
                Console.Write("│");

                // Loop through table values inside to print West and East values
                for (int j = 0; j < Constants.COLS; ++j)
                {
                    //if cell is an obstacle, print dashes
                    if (_Matrix[i, j].IsObstacle())
                    {
                        Console.Write("        ─ ─ ─        │");
                    }
                    //if cell is terminal, print terminal value (100)
                    else if (_Matrix[i, j].IsTerminal())
                    {
                        Console.Write($"         +{Math.Round((double)Constants.TERMINALREWARD),-7}    │");
                    }
                    //otherwise print the value at west and east of the cell of the matirx
                    else
                    {
                        Console.Write($"  {_Matrix[i, j].GetValues().West,7:####0.0}   {_Matrix[i, j].GetValues().East,7:####0.0}  │");
                    }
                }
                Console.WriteLine();
                Console.Write("│");

                // Loop through table values inside again to print South values
                for (int j = 0; j < Constants.COLS; ++j)
                {
                    //print space if the cell is an obstacle or terminal
                    if (_Matrix[i, j].IsObstacle() || _Matrix[i, j].IsTerminal())
                    {
                        Console.Write("                     │");
                    }
                    //otherwise print the value at south of the cell of the matirx
                    else
                    {
                        Console.Write($"       {_Matrix[i, j].GetValues().South,7:####0.0}       │");
                    }
                }
                Console.WriteLine();

                // Middle-line of the Table
                if (i < Constants.ROWS - 1)
                {
                    Console.Write("├");
                    for (int k = 0; k < Constants.COLS - 1; ++k)
                    {
                        Console.Write("─────────────────────┼");
                    }
                    Console.Write("─────────────────────┤");
                    Console.WriteLine();
                }
            }

            // Bottom-line of the Table
            Console.Write("└");
            for (int i = 0; i < Constants.COLS - 1; ++i)
            {
                Console.Write("─────────────────────┴");
            }
            Console.Write("─────────────────────┘");
            Console.WriteLine();
        }
        
        //Prints matrix of N(s,a). will have the same format of Q(s,a) matrix but with integers instead of doubles
        public void PrintNMatrix()
        {
            // Uses Unicode charset (Box Drawing characters)

            // Top-line of the Table
            Console.Write("┌");
            for (int i = 0; i < Constants.COLS - 1; ++i)
            {
                Console.Write("─────────────────────┬");
            }
            Console.Write("─────────────────────┐");
            Console.WriteLine();

            // Loop through table values inside to print all North values first
            for (int i = 0; i < Constants.ROWS; ++i)
            {
                Console.Write("│");
                for (int j = 0; j < Constants.COLS; ++j)
                {
                    //print space if obstacle or terminal
                    if (_Matrix[i, j].IsObstacle() || _Matrix[i, j].IsTerminal())
                    {
                        Console.Write("                     │");
                    }
                    // Otherwise print the value at north of the cell of the matirx. Round the double value to an integer
                    // It will not need rounding since it will be incremented in whole numbers but just to be safe
                    else
                    {
                        Console.Write($"       {Math.Round(_Matrix[i, j].GetValues().North),7}       │");
                    }
                }
                Console.WriteLine();
                Console.Write("│");

                // Loop through table values inside to print West & East values
                for (int j = 0; j < Constants.COLS; ++j)
                {
                    //if cell is an obstacle, print dashes
                    if (_Matrix[i, j].IsObstacle())
                    {
                        Console.Write("        ─ ─ ─        │");
                    }
                    //if cell is terminal, print terminal value (100)
                    else if (_Matrix[i, j].IsTerminal())
                    {
                        Console.Write($"         +{Math.Round((double)Constants.TERMINALREWARD),-7}    │");
                    }
                    //otherwise print the value at west & east of the cell of the matirx
                    else
                    {
                        Console.Write($"  {Math.Round(_Matrix[i, j].GetValues().West),7}   {Math.Round(_Matrix[i, j].GetValues().East),7}  │");
                    }
                }
                Console.WriteLine();
                Console.Write("│");

                // Loop through table values inside again to print South values
                for (int j = 0; j < Constants.COLS; ++j)
                {
                    //print space if the cell is an obstacle or terminal
                    if (_Matrix[i, j].IsObstacle() || _Matrix[i, j].IsTerminal())
                    {
                        Console.Write("                     │");
                    }
                    //otherwise print the value at south of the cell of the matirx
                    else
                    {
                        Console.Write($"       {Math.Round(_Matrix[i, j].GetValues().South),7}       │");
                    }
                }
                Console.WriteLine();

                // Middle-line of the Table
                if (i < Constants.ROWS - 1)
                {
                    Console.Write("├");
                    for (int k = 0; k < Constants.COLS - 1; ++k)
                    {
                        Console.Write("─────────────────────┼");
                    }
                    Console.Write("─────────────────────┤");
                    Console.WriteLine();
                }
            }

            // Bottom-line of the Table
            Console.Write("└");
            for (int i = 0; i < Constants.COLS - 1; ++i)
            {
                Console.Write("─────────────────────┴");
            }
            Console.Write("─────────────────────┘");
            Console.WriteLine();
        }

        // Prints matrix for optimal action by looping through values of calling Q(s,a) matrix and 
        // deciding the optimal action is the same as the direction that has the largest Q value
        public void printOptimalPolicyMatrix(){

            // Uses Unicode charset (Box Drawing characters)
            // Top-line of the Table
            Console.Write("┌");
            for (int i = 0; i < Constants.COLS - 1; ++i)
            {
                Console.Write("─────────────────────┬");
            }
            Console.Write("─────────────────────┐");
            Console.WriteLine();

            // Loop through table values inside to print all North values first
            for (int i = 0; i < Constants.ROWS; ++i)
            {
                Console.Write("│");
                for (int j = 0; j < Constants.COLS; ++j)
                {
                    //just print space at the north of regardless if obstacle or terminal or normal cell
                    Console.Write("                     │");
                }
                Console.WriteLine();
                Console.Write("│");

                // Loop through table values inside to print policy string
                for (int j = 0; j < Constants.COLS; ++j)
                {
                    //if cell is an obstacle, print dashes
                    if (_Matrix[i, j].IsObstacle())
                    {
                        Console.Write("        ─ ─ ─        │");
                    }
                    //if cell is terminal, print terminal value (100)
                    else if (_Matrix[i, j].IsTerminal())
                    {
                        Console.Write($"         +{Math.Round((double)Constants.TERMINALREWARD),-7}    │");
                    }
                    //otherwise print the string of the optimal policy by calling function to 
                    //get the optimal action policy string based on the values of the current cell of the Q matrix
                    else
                    {
                        Console.Write($"        {getOptimalPolicyString(_Matrix[i, j].GetValues())}        │");
                    }
                }
                Console.WriteLine();
                Console.Write("│");

                // Loop through table values inside again to print South values
                for (int j = 0; j < Constants.COLS; ++j)
                {
                    //print space if the cell regardless of whether the cell is an obstacle or terminal or normal cell
                    Console.Write("                     │");
                   
                }
                Console.WriteLine();

                // Middle-line of the Table
                if (i < Constants.ROWS - 1)
                {
                    Console.Write("├");
                    for (int k = 0; k < Constants.COLS - 1; ++k)
                    {
                        Console.Write("─────────────────────┼");
                    }
                    Console.Write("─────────────────────┤");
                    Console.WriteLine();
                }
            }

            // Bottom-line of the Table
            Console.Write("└");
            for (int i = 0; i < Constants.COLS - 1; ++i)
            {
                Console.Write("─────────────────────┴");
            }
            Console.Write("─────────────────────┘");
            Console.WriteLine();
        }

        // This function will determine what the optimal policy string is and return that string
        // It gets the values of each direction of the current cell of the Q matrix and checks which is the largest
        // Then it returns the string from Constants.cs that points to the direction of the largest Q value
        private String getOptimalPolicyString(Values cellValues)
        {
            //loop through each of the four values to determine which is the largest
            String optimalAction = "";
            double maxQValue = Math.Max(Math.Max(cellValues.West, cellValues.North), Math.Max(cellValues.East, cellValues.South));

            if (maxQValue == cellValues.West)
            {
                optimalAction = Constants.WESTDIR;
            }
            else if(maxQValue == cellValues.North)
            {
                optimalAction = Constants.NORTHDIR;
            }
            else if (maxQValue == cellValues.East)
            {
                optimalAction = Constants.EASTDIR;
            }
            else if (maxQValue == cellValues.South)
            {
                optimalAction = Constants.SOUTHDIR;
            }
            else
            {
                throw new Exception("Something is wrong with th eoptimal action in Q(s,a).");
            }
            return optimalAction;
        }
    }
}
