using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Program
    {
        static void Main()
        {
            CheckersGame cGame = new CheckersGame();
            cGame.StartGame();
            Console.ReadLine();
        }
    }
}
