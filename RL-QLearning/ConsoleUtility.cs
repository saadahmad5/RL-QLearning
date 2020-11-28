using System;

namespace RL_QLearning
{
    // Since 10000 trials will be generated to generate the Q values and N values, the code will take time to generate the matirx
    // So instead of displaying a screen that does nothing, the program will show a progress bar in the console
    // Code adopted from https://www.codeproject.com/Tips/5255878/A-Console-Progress-Bar-in-Csharp

    public static class ConsoleUtility
    {
        const char _block = '■';
        const string _back = "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b";

        public static void WriteProgressBar(int percent)
        {
            Console.Write(_back);
            Console.Write("[");
            var p = (int)((percent / 10f) + .5f);
            for (var i = 0; i < 10; ++i)
            {
                if (i >= p)
                    Console.Write(' ');
                else
                    Console.Write(_block);
            }
            Console.Write("] {0,3:##0}%", percent);
        }
    }
}
