using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class CheckersTable
    {
        internal int m_Size;
        private int m_NumOfPlayers;
        private int m_NumOfMenFirstPlayer;
        private int m_NumOfMenSecondPlayer;
        internal CheckerSquare[,] m_Table;
        internal bool m_GameOver;
        private string m_Turn;
        internal int m_NumO;
        internal int m_NumX;

        /* constructor, initializing a checkers table */
        public CheckersTable(int i_TableSize, int i_NumOfPlayers)
        {
            m_Size = i_TableSize;
            m_NumOfPlayers = i_NumOfPlayers;
            m_NumO = calcNumOfMen();
            m_NumX = m_NumO;
            initTable();
        }

        /* given a string of the format COLrow>Colrow, make a move in the table */
        /* returns 0 if couldnt move, returns 1 if moved */
        internal int move(string i_MoveMessage, Player i_PlayerTurn)
        {
            int success = 0;
            bool punish = false;
            CheckersLogic cLogic = new CheckersLogic(this, i_PlayerTurn);
            if (cLogic.isLegalMove(i_MoveMessage, ref i_PlayerTurn))
            {

                // before performing a move, check if player could perform eat and didnt do it.
                string check = cLogic.checkIfCanEat(i_MoveMessage);
                if (!check.Equals("") && !check.Equals(i_MoveMessage))
                {
                    Console.WriteLine("player could eat and didnt do it, now get punished");
                    Console.WriteLine("the eat move was : " + check);
                    punish = true;
                }
                cLogic.move(i_MoveMessage, i_PlayerTurn);
                if (punish)
                {
                    cLogic.punish(i_MoveMessage, check);
                }

                success = 1;
            }
            Console.WriteLine("its not legal eat");

            return success;
        }


        internal CheckerSquare[] GetCheckerSquares(int i_PlayerID)
        {
            int k = 0;
            int size = i_PlayerID == 0 ? m_NumX : m_NumO;
            CheckerSquare[] cSquare = new CheckerSquare[size];
            for (int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if (i_PlayerID == 1)
                    {
                        if (m_Table[i, j].m_CheckerMan.m_Type == CheckersMan.eType.O || m_Table[i, j].m_CheckerMan.m_Type == CheckersMan.eType.U)
                        {
                            cSquare[k++] = m_Table[i, j];
                        }
                    }
                    else
                    {
                        if (m_Table[i, j].m_CheckerMan.m_Type == CheckersMan.eType.K || m_Table[i, j].m_CheckerMan.m_Type == CheckersMan.eType.X)
                        {
                            cSquare[k++] = m_Table[i, j];
                        }
                    }

                }
            }

            return cSquare;
        }


        private void initTable()
        {
            int numOfMen = calcNumOfMen();
            m_Table = new CheckerSquare[m_Size,m_Size];

            initUpperSide();
            // space of 2 lines between the oponnents
            CheckersMan cMan = new CheckersMan(CheckersMan.eType.None);
            for (int i = ((m_Size - 1) / 2); i < ((m_Size / 2) + 1); i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    m_Table[i, j] = new CheckerSquare(i, j, cMan);
                }
            }
                
            initBottomSide();


        }

        private void initUpperSide()
        {
            CheckersMan cMan;

            // occupy the squares on the upper side of the table
            for (int i = 0; i < ((m_Size - 1) / 2); i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        cMan = new CheckersMan(CheckersMan.eType.O);
                        m_Table[i, j] = new CheckerSquare(i, j, cMan);
                    }
                    else
                    {
                        cMan = new CheckersMan(CheckersMan.eType.None);
                        m_Table[i, j] = new CheckerSquare(i, j, cMan);
                    }
                }
            }
        }

        private void initBottomSide()
        {
            CheckersMan cMan;
            int bottomSide = (m_Size / 2) + 1;

            // put men on the bottom side of the table
            for (int i = bottomSide; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        cMan = new CheckersMan(CheckersMan.eType.X);
                        m_Table[i, j] = new CheckerSquare(i, j, cMan);
                    }
                    else
                    {
                        cMan = new CheckersMan(CheckersMan.eType.None);
                        m_Table[i, j] = new CheckerSquare(i, j, cMan);
                    }
                }
            }
        }

        internal void printTable()
        {
            string lineSeparator = " =================================";
            string colSeparator = "|";
            string rowHeadline = getRowHeadLine();
            string colHeadline = getColHeadLine();
            char colChar = (char)(Convert.ToInt32('a') - 1);

            Console.WriteLine(rowHeadline);
            Console.WriteLine(lineSeparator);

            for (int i = 0; i < m_Size; i++)
            {
                colChar = (char)(Convert.ToInt32(colChar) + 1);
                Console.Write(colChar + "|");
                for (int j = 0; j < m_Size; j++)
                {
                    Console.Write(m_Table[i, j].ToString() + "|");
                }
                Console.WriteLine();
                Console.WriteLine(lineSeparator);
            }
        }

        private string getRowHeadLine()
        {
            StringBuilder headLine = new StringBuilder("   ");
            char lineChar = 'A';
            headLine.Append(lineChar);
            headLine.Append("   ");

            for (int i = 1; i < m_Size; i++)
            {
                lineChar = (char)(Convert.ToInt32(lineChar) + 1);
                headLine.Append(lineChar);
                headLine.Append("   ");
            }


            return headLine.ToString();
        }

        private string getColHeadLine()
        {
            StringBuilder headLine = new StringBuilder("");
            char lineChar = 'a';
            headLine.Append(lineChar);
            headLine.Append(Environment.NewLine + Environment.NewLine);

            for (int i = 1; i < m_Size; i++)
            {
                lineChar = (char)(Convert.ToInt32(lineChar) + 1);
                headLine.Append(lineChar);
                headLine.Append(Environment.NewLine + Environment.NewLine);
            }

            return headLine.ToString();
        }

        internal int calcNumOfMen()
        {
            int numOfMen = 0;

            switch (m_Size)
            {
                case 6:
                    numOfMen = 6;
                    break;
                case 8:
                    numOfMen = 12;
                    break;
                case 10:
                    numOfMen = 20;
                    break;
            }

            return numOfMen;
        }
    }
}
