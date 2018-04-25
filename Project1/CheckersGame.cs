using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    // THIS IS THE USER INTERFACE //
    class CheckersGame
    {
        private string m_FirstUserName;
        private string m_SecondUserName;
        private int m_NumOfPlayers;
        private int m_TableSize;
        private CheckersTable m_Table;

        public void StartGame()
        {
            // gets all the required input from the user
            getUserName();
            getTableSize();
            getNumOfPlayers();
            m_Table = new CheckersTable(m_TableSize, m_NumOfPlayers);

            // play the game till its over
            while (!m_Table.m_GameOver)
            {
                string s = getLegalMoveMessage();
                if (isLegalMove(s))
                {
                    m_Table.move(s);
                }
            }
        }

        private void getUserName()
        {

        }

        private void getTableSize()
        {

        }

        /* if num of players is 2, ask for the second user's name */
        private void getNumOfPlayers()
        {
            Console.WriteLine("Please choose the type of your oponnent: .. (and then press enter)");
            Console.WriteLine("1 - Computer");
            Console.WriteLine("2 - Second player");
            Console.ReadLine();
            // TODO : Finish this method
        }

        /* gets a move from the user, check if it's of the format Colrow>Colrow */
        private string getLegalMoveMessage()
        {
            return null;
        }

        /* given move, check if its legal accoring the table's logic, if so, perform the move */
        private bool isLegalMove(string i_MoveMessage)
        {
            
            return m_Table.isLegalMove(i_MoveMessage);
        }

    }
}
