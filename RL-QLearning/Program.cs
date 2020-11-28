/*
 * Author(s):           Kawthar Badran & Saad Ahmad
 * Course:              Intro to Artificial Intelligence
 * Organization:        University of Michigan- Dearborn
 * Description:         Implementation of Reinforcement Learning using Q-Learning algorithm approach
 * Submitted to:        Dr. Shengquan Wang
 * Date:                Dec. 3rd, 2020
 */

using System;

namespace RL_QLearning
{
    class Program
    {
        //Main Program
        static void Main(string[] args)
        {
            //Print welcome message
            Console.WriteLine("Welcome to Reinforcement Learning Program!");
            Console.WriteLine("Reinforcement Learning is implemented using Q-Learning approach");

            // Driver code: create new instance of Reinforcement Learning for 3 matrices and their functions
            ReinforcementLearning reinforcementLearning = new ReinforcementLearning();

            // Starts reinforcement learning to generate trials and building matrices
            reinforcementLearning.StartLearning();

            // Prints N(s,a) a.k.a. Access frequency matrix
            reinforcementLearning.PrintNMatrix();

            // Prints Q(s,a) a.k.a Q-Value Q-Learning
            reinforcementLearning.PrintQMatrix();

            // Prints optimal action
            reinforcementLearning.PrintOptimalAction();

            // Get key before ending Program
            Console.ReadKey();
        }
    }
}
