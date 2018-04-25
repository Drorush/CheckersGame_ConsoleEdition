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
        private CheckersMan[,] m_Table;
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
            int i;
            m_Table = new CheckersMan[m_Size,m_Size];

            // put men on the upper side of the table
            for (i = 0; i < m_Size/2; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if (i + j % 2 == 1)
                    {
                        m_Table[i, j] = new CheckersMan(i, j, CheckersMan.Type.O);
                    }
                }
            }

            // space of 2 lines between the oponnents
            // put men on the bottom side of the table
            for (i+=2; i < m_Size/2; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if (i + j % 2 == 1)
                    {
                        m_Table[i, j] = new CheckersMan(i, j, CheckersMan.Type.X);
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

            Console.WriteLine(rowHeadline);
            Console.WriteLine(lineSeparator);
            Console.WriteLine(colHeadline);

            for (int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {

                }
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
