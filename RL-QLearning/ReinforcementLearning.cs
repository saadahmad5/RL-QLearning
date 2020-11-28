using System;

namespace RL_QLearning
{
    public class ReinforcementLearning
    {
        private readonly Matrix AccessFrequency;                        // N(s,a)
        private readonly Matrix QLearning;                              // Q(s,a)

        public ReinforcementLearning()
        {
            // Assign Default map to both matrices to create the matrix map given by the professor
            AccessFrequency = new Matrix(DefaultMap: true);
            QLearning = new Matrix(DefaultMap: true);
        }

        //This function will start generating the 10000 trials so we can form a Q matrix and N matrix 
        public void StartLearning()
        {
            for (int trial = 0; trial < Constants.TRAILS; trial++)
            {
                // Shows the progress bar
                ConsoleUtility.WriteProgressBar((trial + 1) * 100 / Constants.TRAILS);

                // Generate random x, y until they map the obstacles because we need non-obstacle point
                int x, y;
                do
                {
                    Random random = new Random();
                    x = random.Next(0, Constants.ROWS);
                    y = random.Next(0, Constants.COLS);
                }
                while (AccessFrequency.IsObstacleAt(x, y));

                // Continue drawing trajectory until either reached to terminal state
                // or operation aborts if more than (100) steps are elapsed
                int steps = 0;
                bool reachedTerminalState = false;
                while (!reachedTerminalState && steps < Constants.STEPS)
                {
                    //if we have reached the terminal state, set flag to true so the learning loop will stop iterating
                    if (QLearning.IsTerminalAt(x, y))
                    {
                        reachedTerminalState = true;
                    }
                    //otherwise keep going on
                    else
                    {
                        // Determine next direction based on Exploration/ Exploitation algorithm
                        Direction direction = DetermineNextCell(x, y);

                        // Update the Access Frequency N(s,a) matrix
                        UpdateAccessFrequencyMatrix(x, y, direction);

                        // Update the coordinates of the current position
                        // Save x and y in variables because x and y will possibly change when assigning values to next state cell
                        int currentX = x;
                        int currentY = y;

                        // Next state cell Q(s', a'), will be set in the next function
                        Cell nextStateCell = new Cell();

                        // Here x, y are passed by reference so trajectory continues with updated x, y
                        AssignValuesToNextStateCell(ref x, ref y, direction, nextStateCell);

                        // Update the Q-Learning matrix Q(s,a) values
                        UpdateQLearningMatrix(currentX, currentY, direction, nextStateCell);
                    }
                    //increment steps to check loop does not exceed 100 elapsed steps
                    ++steps;
                }
            }
        }

        // Assigns value to a new cell which is used as the next state cell
        private void AssignValuesToNextStateCell(ref int x, ref int y, Direction direction, Cell cell)
        {
            //create values struct to hold values of next state cell
            Values values = new Values();

            //if the next direction from the current cell is WEST, check if next cell is not an obstacle
            //if next is not obstacle, update y to go LEFT, and get Q values at those coordinates, save them in values struct
            //if the next cell is an obstacle, the coordinates will not change, because the machine will bounce back to same current cell
            if (direction == Direction.WEST)
            {
                if (!QLearning.IsObstacleAt(x, y - 1))
                {
                    y = y - 1;
                }
                values.West = QLearning.GetWestValueAt(x, y);
                values.North = QLearning.GetNorthValueAt(x, y);
                values.East = QLearning.GetEastValueAt(x, y);
                values.South = QLearning.GetSouthValueAt(x, y);
            }
            //if the next direction from the current cell is NORTH, check if next cell is not an obstacle
            //if next is not obstacle, update x to go UP, and get Q values at those coordinates, save them in values struct
            //if the next cell is an obstacle, the coordinates will not change, because the machine will bounce back to same current cell
            else if (direction == Direction.NORTH)
            {
                if (!QLearning.IsObstacleAt(x - 1, y))
                {
                    x = x - 1;
                }
                values.West = QLearning.GetWestValueAt(x, y);
                values.North = QLearning.GetNorthValueAt(x, y);
                values.East = QLearning.GetEastValueAt(x, y);
                values.South = QLearning.GetSouthValueAt(x, y);
            }
            //if the next direction from the current cell is EAST, check if next cell is not an obstacle
            //if next is not obstacle, update y to go RIGHT, and get Q values at those coordinates, save them in values struct
            //if the next cell is an obstacle, the coordinates will not change, because the machine will bounce back to same current cell
            else if (direction == Direction.EAST)
            {
                if (!QLearning.IsObstacleAt(x, y + 1))
                {
                    y = y + 1;
                }
                values.West = QLearning.GetWestValueAt(x, y);
                values.North = QLearning.GetNorthValueAt(x, y);
                values.East = QLearning.GetEastValueAt(x, y);
                values.South = QLearning.GetSouthValueAt(x, y);
            }
            //if the next direction from the current cell is SOUTH, check if next cell is not an obstacle
            //if next is not obstacle, update x to go DOWN, and get Q values at those coordinates, save them in values struct
            //if the next cell is an obstacle, the coordinates will not change, because the machine will bounce back to same current cell
            else if (direction == Direction.SOUTH)
            {
                if (!QLearning.IsObstacleAt(x + 1, y))
                {
                    x = x + 1;
                }
                values.West = QLearning.GetWestValueAt(x, y);
                values.North = QLearning.GetNorthValueAt(x, y);
                values.East = QLearning.GetEastValueAt(x, y);
                values.South = QLearning.GetSouthValueAt(x, y);
            }
            //if the direction in anything other than a valid direction of WNES, throw exception. Program should never get here
            else
            {
                throw new Exception("Error in next Cell computation");
            }

            //set the cell, which is the next state cell, to the values that we found after moving in the next direction
            cell.AssignValueAtWest(values.West);
            cell.AssignValueAtNorth(values.North);
            cell.AssignValueAtEast(values.East);
            cell.AssignValueAtSouth(values.South);
        }

        // Prints N matrix by calling function that prints frequency matirx
        public void PrintNMatrix()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Matrix N(s,a)\n\n");
            AccessFrequency.PrintNMatrix();
        }

        // Prints Q matrix by calling function that prints Q values matirx
        public void PrintQMatrix()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Matrix Q(s,a)\n\n");
            QLearning.PrintQMatrix();
        }

        // Prints the Optimal Action Matrix by calling function that prints optimal actions based on max Q values
        public void PrintOptimalAction()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Optimal Action:\n\n");
            QLearning.printOptimalPolicyMatrix();
        }

        // Updates the N-Matrix's cell (x, y) at a given direction
        private void UpdateAccessFrequencyMatrix(int x, int y, Direction direction)
        {
            // Based on the given direction, it grabs the N value,
            // It increments the N(s,a) and updates the Access Frequency matrix

            //if the next direction that the machine will take is WEST, get value at west of cell from N matrix
            //increment that value and set it in the N matirx at the same cell at WEST direction
            if (direction == Direction.WEST)
            {
                double nValue = AccessFrequency.GetWestValueAt(x, y);
                AccessFrequency.AssignWestValueAt(x, y, ++nValue);
            }

            //if the next direction that the machine will take is NORTH, get value at west of cell from N matrix
            //increment that value and set it in the N matirx at the same cell at NORTH direction
            else if (direction == Direction.NORTH)
            {
                double nValue = AccessFrequency.GetNorthValueAt(x, y);
                AccessFrequency.AssignNorthValueAt(x, y, ++nValue);
            }
            //if the next direction that the machine will take is EAST, get value at west of cell from N matrix
            //increment that value and set it in the N matirx at the same cell at EAST direction
            else if (direction == Direction.EAST)
            {
                double nValue = AccessFrequency.GetEastValueAt(x, y);
                AccessFrequency.AssignEastValueAt(x, y, ++nValue);
            }
            //if the next direction that the machine will take is SOUTH, get value at west of cell from N matrix
            //increment that value and set it in the N matirx at the same cell at SOUTH direction
            else if (direction == Direction.SOUTH)
            {
                double nValue = AccessFrequency.GetSouthValueAt(x, y);
                AccessFrequency.AssignSouthValueAt(x, y, ++nValue);
            }
            //otherwise if direction is not set to a valid value throw Exception
            else
            {
                // Control should never reach here
                throw new Exception("Something is wrong with directions in N(s,a)");
            }
        }

        // This function updates the Q-Matrix's cell (x, y) at given direction
        private void UpdateQLearningMatrix(int x, int y, Direction direction, Cell nextCell)
        {
            // Based on the given direction that the cell will go next, it grabs the N value and Q value,
            // Then it computes the Q(s,a) and updates the Q-Learning matrix
            if (direction == Direction.WEST)
            {
                double qValue = QLearning.GetWestValueAt(x, y);
                double nValue = AccessFrequency.GetWestValueAt(x, y);
                qValue = computeQValue(qValue, nValue, direction, nextCell);
                QLearning.AssignWestValueAt(x, y, qValue);
            }
            else if (direction == Direction.NORTH)
            {
                double qValue = QLearning.GetNorthValueAt(x, y);
                double nValue = AccessFrequency.GetNorthValueAt(x, y);
                qValue = computeQValue(qValue, nValue, direction, nextCell);
                QLearning.AssignNorthValueAt(x, y, qValue);
            }
            else if (direction == Direction.EAST)
            {
                double qValue = QLearning.GetEastValueAt(x, y);
                double nValue = AccessFrequency.GetEastValueAt(x, y);
                qValue = computeQValue(qValue, nValue, direction, nextCell);
                QLearning.AssignEastValueAt(x, y, qValue);
            }
            else if (direction == Direction.SOUTH)
            {
                double qValue = QLearning.GetSouthValueAt(x, y);
                double nValue = AccessFrequency.GetSouthValueAt(x, y);
                qValue = computeQValue(qValue, nValue, direction, nextCell);
                QLearning.AssignSouthValueAt(x, y, qValue);
            }
            else
            {
                // Control should never reach here
                throw new Exception("Something is wrong with directions in Q(s,a).");
            }
        }

        // Returns the max a Q(s', a') in a given cell, used for Q-Learning computation
        private double GetMaxQValue(Cell cell)
        {
            // Get the Q-Values
            Values values = new Values
            {
                West = cell.GetValues().West,
                North = cell.GetValues().North,
                East = cell.GetValues().East,
                South = cell.GetValues().South
            };
            // Determine & return the maximum Q-value 
            return Math.Max(Math.Max(values.West, values.North), Math.Max(values.East, values.South));
        }

        //This function will determine and return the direction that the machine will take as it is learning and moving in the maze
        private Direction DetermineNextCell(int x, int y)
        {
            //generate a random number from 0 to 99, 5% for random action, 95% for optimal policy
            Random random = new Random();
            int randomNumber = random.Next(0, 100);

            // Random numbers: 0 to Epsilon (0 - 4) represents random action since epsilon = 5%
            if (randomNumber < Constants.EPSILON * 100)
            {
                // Returns random direction by generating a random number from 0 to 3 to represent directions from W to S
                // And checks if there will be a drift by calling getDirectionWithDrift function
                Direction randomDirection = (Direction)random.Next(0, Constants.NUMOFDIRECTIONS);
                return getDirectionWithDrift(randomDirection);
            }
            // Random numbers: Epsilon to 100 (5 - 99) represents optimal action
            else
            {
                // Get the Q-Values at the current cell and save them in this values struct
                Values values = new Values
                {
                    West = QLearning.GetWestValueAt(x, y),
                    North = QLearning.GetNorthValueAt(x, y),
                    East = QLearning.GetEastValueAt(x, y),
                    South = QLearning.GetSouthValueAt(x, y)
                };

                // Determine & return the direction based on the maximum Q-value 
                double maxValue = Math.Max(Math.Max(values.West, values.North), Math.Max(values.East, values.South));
                
                // Check what the optimal action and see if it will drift based on the drift Random Number generated in the getDirectionWithDrift function
                if (values.West == maxValue)
                {
                    return getDirectionWithDrift(Direction.WEST);
                }
                else if (values.North == maxValue)
                {
                    return getDirectionWithDrift(Direction.NORTH);
                }
                else if (values.East == maxValue)
                {
                    return getDirectionWithDrift(Direction.EAST);
                }
                else if (values.South == maxValue)
                {
                    return getDirectionWithDrift(Direction.SOUTH);
                }
            }

            // Returns before this point so should never reach to this exception
            throw new Exception("Something is wrong in finding next cell.");
        }

        // This function will update the passed qValue by using the Q-Learning Formula
        // The formula depends on the frequence nValue, the original qValue, direction the machine will go next for getting reward value, and the max value of the next cell to get Q(s',a')
        private double computeQValue(double qValue, double nValue, Direction nextDirection, Cell nextCell)
        {
            //find reward value based on direction
            double rewardValue = 0;
            switch (nextDirection)
            {
                //if next direction is west, the reward is negative of the cost of going west
                case Direction.WEST:
                    rewardValue = Constants.WEST;
                    break;

                //if next direction is north, the reward is negative of the cost of going north
                case Direction.NORTH:
                    rewardValue = Constants.NORTH;
                    break;

                //if next direction is east, the reward is negative of the cost of going east
                case Direction.EAST:
                    rewardValue = Constants.EAST;
                    break;

                //if next direction is south, the reward is negative of the cost of going south
                case Direction.SOUTH:
                    rewardValue = Constants.SOUTH;
                    break;

                //if next direction is not valid throw exception
                default:
                    throw new Exception("Something is wrong with directions in Q(s,a).");
            }

            //substitute values in Q learning formula to return updated Q value
            return (qValue + ((1.0 / nValue) * (rewardValue + (Constants.GAMMA * GetMaxQValue(nextCell)) - qValue)));
        }


        // This function will get a direction as a parameter that is the straight and initial action that was supposed to be taken
        // Then it will generate a random number to account for drifting and returns the actual action that will be taken based on whether drifting will happen or not
        private Direction getDirectionWithDrift(Direction straightDirection)
        {
            // To determine if we will drift relative left or right due to windy condition, generate a random number from 0 to 99.
            // Assume 15% -> [0 to SlipProbability) is rel. left [0, 15) = [0, 14]
            // Assume 70% -> [SlipProbability to SlipProbability + MoveProbability) is rel. straight [15,85) = [15, 84]
            // Assume 15% -> [SlipProbability + MoveProbability to 100) is rel. right [85, 100) = [85, 99]
            Random random = new Random();
            int driftRandomNumber = random.Next(0, 100);

            //initial direction is straight but it may change based on random number and drift possibility
            Direction direction = straightDirection;

            switch (straightDirection)
            {
                case Direction.WEST:
                    if (driftRandomNumber >= Constants.MINLIMIT && driftRandomNumber < Constants.LEFTLIMIT)
                    {
                        direction = Direction.SOUTH;     //relative left
                    }
                    else if (driftRandomNumber >= Constants.LEFTLIMIT && driftRandomNumber < Constants.STRAIGHTLIMIT)
                    {
                        direction = Direction.WEST;      //straight
                    }
                    else if (driftRandomNumber >= Constants.STRAIGHTLIMIT && driftRandomNumber < Constants.RIGHTLIMIT)
                    {
                        direction = Direction.NORTH;     //relative right
                    }
                    break;

                case Direction.NORTH:
                    if (driftRandomNumber >= Constants.MINLIMIT && driftRandomNumber < Constants.LEFTLIMIT)
                    {
                        direction = Direction.WEST;       //relative left
                    }
                    else if (driftRandomNumber >= Constants.LEFTLIMIT && driftRandomNumber < Constants.STRAIGHTLIMIT)
                    {
                        direction = Direction.NORTH;      //straight
                    }
                    else if (driftRandomNumber >= Constants.STRAIGHTLIMIT && driftRandomNumber < Constants.RIGHTLIMIT)
                    {
                        direction = Direction.EAST;       //relative right
                    }
                    break;

                case Direction.EAST:
                    if (driftRandomNumber >= Constants.MINLIMIT && driftRandomNumber < Constants.LEFTLIMIT)
                    {
                        direction = Direction.NORTH;       //relative left
                    }
                    else if (driftRandomNumber >= Constants.LEFTLIMIT && driftRandomNumber < Constants.STRAIGHTLIMIT)
                    {
                        direction = Direction.EAST;       //straight
                    }
                    else if (driftRandomNumber >= Constants.STRAIGHTLIMIT && driftRandomNumber < Constants.RIGHTLIMIT)
                    {
                        direction = Direction.SOUTH;       //relative right
                    }
                    break;

                case Direction.SOUTH:
                    if (driftRandomNumber >= Constants.MINLIMIT && driftRandomNumber < Constants.LEFTLIMIT)
                    {
                        direction = Direction.EAST;       //relative left
                    }
                    else if (driftRandomNumber >= Constants.LEFTLIMIT && driftRandomNumber < Constants.STRAIGHTLIMIT)
                    {
                        direction = Direction.SOUTH;      //straight
                    }
                    else if (driftRandomNumber >= Constants.STRAIGHTLIMIT && driftRandomNumber < Constants.RIGHTLIMIT)
                    {
                        direction = Direction.WEST;       //relative right
                    }
                    break;

                default:
                    throw new Exception("Something is wrong with directions in Q(s,a).");
                    break;
            }
            return direction;
        }
    }
}
