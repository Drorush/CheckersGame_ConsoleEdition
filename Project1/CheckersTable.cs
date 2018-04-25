using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class CheckersTable
    {
        private int m_Size;
        private int m_NumOfPlayers;
        private int m_NumOfMenFirstPlayer;
        private int m_NumOfMenSecondPlayer;
        internal CheckerSquare[,] m_Table;
        internal bool m_GameOver;
        private string m_Turn;

        /* constructor, initializing a checkers table */
        public CheckersTable(int i_TableSize, int i_NumOfPlayers)
        {
            m_Size = i_TableSize;
            m_NumOfPlayers = i_NumOfPlayers;
            initTable();
        }

        /* given a string of the format COLrow>Colrow, make a move in the table */
        internal void move(string i_MoveMessage)
        {

        }

        internal bool isLegalMove(string i_MoveMessage)
        {
            return false;
        }

        private void initTable()
        {
            int numOfMen = calcNumOfMen();
            m_Table = new CheckerSquare[m_Size,m_Size];

            initUpperSide();
            // space of 2 lines between the oponnents
            for (int i = ((m_Size - 1) / 2); i < ((m_Size / 2) + 1); i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    m_Table[i, j] = new CheckerSquare(i, j, null);
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
                        cMan = new CheckersMan(i, j, CheckersMan.eType.O);
                        m_Table[i, j] = new CheckerSquare(i, j, cMan);
                    }
                    else
                    {
                        m_Table[i, j] = new CheckerSquare(i, j, null);
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
                        cMan = new CheckersMan(i, j, CheckersMan.eType.X);
                        m_Table[i, j] = new CheckerSquare(i, j, cMan);
                    }
                    else
                    {
                        m_Table[i, j] = new CheckerSquare(i, j, null);
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

        private int calcNumOfMen()
        {
            int numOfMen = 0;

            switch (m_Size)
            {
                case 6:
                    numOfMen = 12;
                    break;
                case 8:
                    numOfMen = 24;
                    break;
                case 10:
                    numOfMen = 40;
                    break;
            }

            return numOfMen;
        }
    }
}
