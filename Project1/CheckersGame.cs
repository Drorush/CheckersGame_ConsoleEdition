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
            initGame();
        }

        private void initGame()
        {
            m_Table = new CheckersTable(m_TableSize, m_NumOfPlayers);
            m_Table.printTable();
            m_PlayerOne = new Player(m_FirstUserName, 0, 0);
            m_PlayerTwo = new Player(m_SecondUserName, 0, 1);
            m_turn = m_FirstUserName + "\'s";
            playGame();
        }

        // starts a new game if the users say so
        private void startNewGame()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine("-- We are starting a new game -- GOOD LUCK !");
            m_Table = new CheckersTable(m_TableSize, m_NumOfPlayers);
            m_Table.printTable();
            playGame();
        }

        // play a game until someone wins / draw
        private void playGame()
        {
            bool hasNoLegalMoves = false;
            bool surrender = false;
            // we start with X player
            int playerIdTurn = 0;
            string moveMessage = "";
            CheckerSquare[] cSquare;
            string[] moveMessages;

            // play the game till its over
            while (!m_Table.m_GameOver)
            {
                CheckersLogic logic = new CheckersLogic(m_Table, getPlayerById(playerIdTurn));
                Console.WriteLine(m_turn + " turn:");
                // if first player can eat again, we give him another turn
                if (m_CanEatAgain && playerIdTurn == 0)
                {
                    m_CanEatAgain = false;
                }
                else
                {
                    // if we are playing against the computer
                    if (m_turn.Equals("Computer\'s"))
                    {
                        cSquare = m_Table.GetCheckerSquares(1);
                        moveMessages = logic.getPossibleMovesForPlayer(ref cSquare);
                        hasNoLegalMoves = logic.hasNoLegalMoves(moveMessages);
                        playerIdTurn = 1;
                        if (hasNoLegalMoves)
                        {
                            break;
                        }

                        Random random = new Random();
                        moveMessage = moveMessages[random.Next(moveMessages.Length)];
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
                            moveMessage = eat.Equals("") ? moveMessage : eat; // prefer the eating move
                        }
                    }
                    // player's (non-computer) turn
                    else
                    {
                        cSquare = m_Table.GetCheckerSquares(playerIdTurn);
                        moveMessages = logic.getPossibleMovesForPlayer(ref cSquare);
                        hasNoLegalMoves = logic.hasNoLegalMoves(moveMessages);
                        if (hasNoLegalMoves)
                        {
                            break;
                        }

                        moveMessage = getLegalMoveMessage(playerIdTurn);
                        if (moveMessage.Equals("Q")) // if players wants and can quit
                        {
                            surrender = true;
                            break;
                        }
                    }

                    int moveIndicator = m_Table.move(moveMessage, getPlayerById(playerIdTurn));
                    if (moveIndicator == 0)
                    {
                        Console.WriteLine("Please enter legal move");
                    }
                    else
                    {
                        // made a move , clear the screen
                        Ex02.ConsoleUtils.Screen.Clear();
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
                                m_CanEatAgain = true;
                                m_EatMove = canEat;
                            }
                        }
                        // if it wasnt eatmove, checks if he could eat someone and didnt do it, if so he lose man
                        else
                        {
                            switchTurn(ref playerIdTurn);
                        }

                    }
                }
                if (m_Table.m_NumO == 0 || m_Table.m_NumX == 0 || surrender || hasNoLegalMoves)
                {
                    Console.WriteLine("GAME OVER !");
                    break;
                }
            }

            // something got us out of the game, check what happend
            calculatePointsAfterGame();
            Console.WriteLine("Player one has now " + m_PlayerOne.m_TotalPoints + " points");
            Console.WriteLine("Player two has now " + m_PlayerTwo.m_TotalPoints + " points");
            checkIfPlayAgain();
        }

        private void checkIfPlayAgain()
        {
            Console.WriteLine("Do you want to play another game?");
            Console.WriteLine("If yes, please type yes (and then press enter). If no, type anything and then press enter");
            string yesno = Console.ReadLine();
            
            if (yesno.Equals("yes") || yesno.Equals("Yes") || yesno.Equals("YES"))
            {
                startNewGame();
            }
            else
            {
                Console.WriteLine("Good bye !!");
            }
        }

        private void calculatePointsAfterGame()
        {
            int playerOnePoints = m_Table.calculatePoints(0);
            int playerTwoPoints = m_Table.calculatePoints(1);

            if (playerOnePoints >= playerTwoPoints)
            {
                m_PlayerOne.m_TotalPoints += playerOnePoints - playerTwoPoints;
            }
            else
            {
                m_PlayerTwo.m_TotalPoints += playerTwoPoints - playerOnePoints;
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
        private string getLegalMoveMessage(int i_id)
        {
            string move = Console.ReadLine();
            while (!isLegalMove(move, i_id))
            {
                Console.WriteLine("Incorrect move, please enter a new move (Format: COLRow>COLRow)");
                move = Console.ReadLine();
            }

            return move;
        }


        /* given move, check if its legal accoring the table's logic, if so, perform the move */
        internal bool isLegalMove(string i_MoveMessage, int i_id)
        {
            bool isQuit = i_MoveMessage.Equals("Q") && checkIfCanQuit(i_id);
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

            return isQuit || (isLegal && isLegalCur && isLegalNext && isInRange);
        }

        private bool checkIfCanQuit(int i_id)
        {
            bool canQuit = false;
            if (i_id == 0)
            {
                Console.WriteLine(m_Table.m_NumX <= m_Table.m_NumO);
                Console.WriteLine(m_Table.m_NumX + ", " + m_Table.m_NumO);
                canQuit = m_Table.m_NumX <= m_Table.m_NumO;
            }
            else
            {
                canQuit = m_Table.m_NumO <= m_Table.m_NumX;
            }

            return canQuit;
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
