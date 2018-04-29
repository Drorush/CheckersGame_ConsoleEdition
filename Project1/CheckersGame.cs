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
        string m_EatMove;
        bool m_CanEatAgain;
        const int MaxNameLength = 20;
        const int asciiValOfa = 97;
        const int asciiValOfA = 65;
        const int MoveLength = 5;
        internal Player m_PlayerOne;
        internal Player m_PlayerTwo;
        private string m_FirstUserName; // X player
        private string m_SecondUserName;// O player
        private int m_NumOfPlayers;
        private int m_TableSize;
        private CheckersTable m_Table;
        internal string m_turn;

        public void StartGame()
        {
            // gets all the required input from the user
            m_FirstUserName = getUserName();
            getTableSize();
            getNumOfPlayers();
            m_Table = new CheckersTable(m_TableSize, m_NumOfPlayers);
            m_Table.printTable();
            m_PlayerOne = new Player(m_FirstUserName, m_Table.calcNumOfMen(), 0);
            m_PlayerTwo = new Player(m_SecondUserName, m_Table.calcNumOfMen(), 1);
            m_turn = m_FirstUserName + "\'s";
            string moveMessage = "";
            int playerIdTurn = 0;

            // play the game till its over
            while (!m_Table.m_GameOver)
            {
                CheckersLogic logic = new CheckersLogic(m_Table, getPlayerById(playerIdTurn));
                Console.WriteLine(m_turn + " turn:");
                // if first player can eat again, we give him another turn
                if (m_CanEatAgain && playerIdTurn == 0)
                {
                    Console.WriteLine("You Can eat again!");
                    m_CanEatAgain = false;
                }
                else
                {
                    // if we are playing against the computer
                    if (m_turn.Equals("Computer\'s"))
                    {
                        CheckerSquare[] cSquare = m_Table.GetCheckerSquares(1);
                        string[] moveMessages = logic.getPossibleMovesForPlayer(ref cSquare);
                        Random random = new Random();
                        moveMessage = moveMessages[random.Next(moveMessages.Length)];
                        playerIdTurn = 1;
                        if (m_CanEatAgain)
                        {
                            moveMessage = m_EatMove;
                            m_CanEatAgain = false;
                        }
                        else
                        {
                            string eat = "";
                            while (moveMessage.Equals("Aa>Aa")) // filter the non-legal moves
                            {
                                moveMessage = moveMessages[random.Next(moveMessages.Length)];
                            }
                            eat = logic.checkIfCanEat(moveMessage);
                            moveMessage = eat.Equals("") ? moveMessage : eat;
                        }
                    }
                    // else its player's turn
                    else
                    {
                        moveMessage = getLegalMoveMessage();
                    }

                    int moveIndicator = m_Table.move(moveMessage, getPlayerById(playerIdTurn));
                    if (moveIndicator == 0)
                    {
                        Console.WriteLine("Please enter legal move");
                    }
                    else
                    {
                        // made a move
                        // Ex02.ConsoleUtils.Screen.Clear();
                        Console.WriteLine("\n");
                        m_Table.printTable(); // print state after the move
                        Console.WriteLine(m_turn + " move was: " + moveMessage);
                        //checks if move was eatmove
                        if (logic.isEatMove(moveMessage))
                        {
                            logic.m_Player.m_JustAte = true;
                            // checks if the player can eat again, if he can, camEat will hold the new move
                            string canEat = logic.canEatAgain(moveMessage);
                            if (canEat.Equals(""))
                            {
                                // if he cant eat anymore, just switch turns
                                logic.m_Player.m_JustAte = false;
                                switchTurn(ref playerIdTurn);
                                m_CanEatAgain = false;
                                m_EatMove = "";
                            }
                            else
                            {
                                Console.WriteLine("you can eat again, the move is : " + canEat);
                                m_CanEatAgain = true;
                                m_EatMove = canEat;
                            }
                        }
                        // if it wasnt eatmove, check if he could eat someone and didnt do it, if so he lose man
                        else
                        {
                            switchTurn(ref playerIdTurn);
                        }

                    }
                }
            }
        }

        private Player getPlayerById(int i_id)
        {
            Player ReturnPlayer;
            if (i_id == 0)
            {
                ReturnPlayer = m_PlayerOne;
            }
            else
            {
                ReturnPlayer = m_PlayerTwo;
            }

            return ReturnPlayer;
        }

        private void switchTurn(ref int i_id)
        {
            if (m_turn.Equals(m_FirstUserName + "\'s"))
            {
                m_turn = m_SecondUserName;
                    i_id = 1;
            }
            else
            {
                m_turn = m_FirstUserName;
                i_id = 0;
            }

            m_turn += "\'s";
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
            Console.WriteLine("Hey " + m_FirstUserName + " please choose a table size :10, 8 , 6");
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
            if (!(i_size == 10 || i_size == 8 || i_size == 6))
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

        /* gets a move from the user, check if it's of the Colrow>Colrow */
        private string getLegalMoveMessage()
        {
            string move = Console.ReadLine();
            while (!isLegalMove(move))
            {
                Console.WriteLine("Incorrect move, please enter a new move (Format: COLRow>COLRow)");
                move = Console.ReadLine();
            }

            return move;
        }


        /* given move, check if its legal accoring the table's logic, if so, perform the move */
        internal bool isLegalMove(string i_MoveMessage)
        {
            bool isInRange = false;
            bool isLegalCur = true;
            bool isLegalNext = true;
            bool isLegal = true;

            if (i_MoveMessage.Length != MoveLength || i_MoveMessage[2] != '>')
            {
                isLegal = false;
            }

            if (isLegal)
            {
                isInRange = checkRange(i_MoveMessage);
                isLegalCur = isLegalFormat(i_MoveMessage.Substring(0, 2));
                isLegalNext = isLegalFormat(i_MoveMessage.Substring(3, 2));
            }

            return (isLegal && isLegalCur && isLegalNext && isInRange);
        }

        private bool isLegalFormat(string i_move)
        {
            char columnMove = i_move[0];
            char rowMove = i_move[1];
            return (Char.IsUpper(columnMove) && Char.IsLower(rowMove));
        }



        private bool checkRange(string i_MoveMessage)
        {
            byte[] asciiInput = Encoding.ASCII.GetBytes(i_MoveMessage);
            byte startColChar = asciiInput[0];
            byte endColChar = asciiInput[3];
            byte startRowChar = asciiInput[1];
            byte endRowChar = asciiInput[4];

            bool firstCol = startColChar >= asciiValOfA && startColChar <= (asciiValOfA + m_TableSize - 1);
            bool secondCol = endColChar >= asciiValOfA && endColChar <= (asciiValOfA + m_TableSize - 1);
            bool firstRow = startRowChar >= asciiValOfa && startRowChar <= (asciiValOfa + m_TableSize - 1);
            bool secondRow = endRowChar >= asciiValOfa && endRowChar <= (asciiValOfa + m_TableSize - 1);
            return firstCol && secondCol && firstRow && secondRow;
        }
    }
}
