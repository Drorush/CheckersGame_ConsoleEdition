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
        const int MaxNameLength = 20;
        const int asciiValOfa = 65;
        const int asciiValOfA = 97;
        const int MoveLength = 5;
        private string m_FirstUserName;
        private string m_SecondUserName;
        private int m_NumOfPlayers;
        private int m_TableSize;
        private CheckersTable m_Table;

        public void StartGame()
        {
            // gets all the required input from the user
           m_FirstUserName = getUserName();
            getTableSize();
            getNumOfPlayers();
            getLegalMoveMessage();
            getLegalMoveMessage();
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

        private string getUserName()
        {
            Console.WriteLine("Please enter first player's name");
            string name = Console.ReadLine();
            while (!legalName(name))
            {
                name = Console.ReadLine();
            }
            return name;
        }
        private bool legalName(string i_input)
        {
            bool isLegal = true;
            if(i_input.Length > MaxNameLength)
            {
                isLegal = false;
                Console.WriteLine("Your name is invalid please enter a new name (less than 20 characters)");
            }
            return isLegal;
        }

        private void getTableSize()
        {
            Console.WriteLine("Hey "+m_FirstUserName +" please choose a table size :10, 8 , 6");
            int.TryParse(Console.ReadLine(),out int number);
            while (!legalSize(number))
            {
                int.TryParse(Console.ReadLine(),out number);
            }
            m_TableSize = number;
            
        }
        private bool legalSize(int i_size)
        {
            bool isLegal = true;
            if (!(i_size == 10|| i_size == 8 | i_size == 6))
            {
                isLegal = false;
                Console.WriteLine("Your size is invalid please enter a new size: 10 / 8 / 6");
            }
            return isLegal;
        }

        /* if num of players is 2, ask for the second user's name */
        private void getNumOfPlayers()
        {
            Console.WriteLine(m_FirstUserName + " please choose the type of your oponnent: .. (and then press enter)");
            Console.WriteLine("1 - Computer");
            Console.WriteLine("2 - Second player");
            string input = Console.ReadLine();
            gameType(input);
        }
        private void gameType(string i_input)
        {
            while(i_input != "1" && i_input != "2") 
            {
                Console.WriteLine("Invalid answer , please try again");
                Console.WriteLine("1 - Computer");
                Console.WriteLine("2 - Second player");
                i_input = Console.ReadLine();
            }
            if(i_input == "1")
            {
                m_SecondUserName = "Computer";
                m_NumOfPlayers = 1;
            }
            else
            {
                m_SecondUserName = getSecondName();
                m_NumOfPlayers = 2;
            }
        }
        private string getSecondName()
        {
            Console.WriteLine("Please enter second player's name");
            string name = Console.ReadLine();
            while (!legalName(name))
            {
                name = Console.ReadLine();
            }
            return name;
        }

        /* gets a move from the user, check if it's of the format Colrow>Colrow */
        private string getLegalMoveMessage()
        {
            Console.WriteLine("Lets start to play...");
            Console.WriteLine("Enter a move: ColRow>ColRow");
            string move = Console.ReadLine();
            while (!isLegalMove(move))
            {
                Console.WriteLine("Incorrect move , please enter a new move (Format : ColRow>ColRow)");
                move = Console.ReadLine();
            }
            return move;
        }


        /* given move, check if its legal accoring the table's logic, if so, perform the move */
        private bool isLegalMove(string i_MoveMessage)
        {
            bool isLegalCur = true;
            bool isLegalNext = true;
            bool isLegal = true;
            string curPosition = i_MoveMessage.Substring(0, 2);
            char sign = i_MoveMessage[2];
            string nextPosition = i_MoveMessage.Substring(3, 2);
            if (i_MoveMessage.Length != MoveLength || sign != '>')
            {
                isLegal = false;
            }
            if (isLegal)
            {
                isLegalCur = isLegalStep(curPosition);
                isLegalNext = isLegalStep(nextPosition);
            }
            return (isLegalCur && isLegalNext);
        }


        private bool isLegalStep(string i_move)
        {
            bool isLegal = true;
            char columnMove = i_move[0];
            char rowMove = i_move[1];

            if (!(columnMove - asciiValOfa >= 0 && columnMove - asciiValOfa < m_TableSize - 1))
            {
                isLegal = false;
            }
            if (isLegal && !(rowMove - asciiValOfA >= 0 && rowMove - asciiValOfA < m_TableSize - 1))
            {
                isLegal = false;
            }
            return isLegal;
        }
    }
}
