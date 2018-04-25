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
            CheckersTable ct = new CheckersTable(8, 2);
            ct.printTable();
            Console.ReadLine();
        }
    }
}
