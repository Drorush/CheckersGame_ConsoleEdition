using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class CheckersLogic
    {
        private CheckersTable m_CheckersTable;

        public CheckersLogic(CheckersTable i_Table)
        {
            m_CheckersTable = i_Table;
        }

        internal bool isLegalMove(CheckerSquare i_CheckerSquare, string i_MoveCommand)
        {
            int[] startPoint = getStartPoint(i_MoveCommand);
            int[] endPoint = getEndPoint(i_MoveCommand);
            CheckerSquare currentSquare = m_CheckersTable.m_Table[startPoint[0], startPoint[1]];
            CheckerSquare targetSquare = m_CheckersTable.m_Table[endPoint[0], endPoint[1]];
            // checks if the squares are inside the border of the table
            bool firstCheck = isSquaresInBorder(currentSquare, targetSquare);
            // checks if currentsquare is occupied and the target square isnt 
            bool secondCheck = currentSquare.isOccupied() && !targetSquare.isOccupied();
            // check if the move is diagonal, otherwise if its eating move
            bool thirdCheck = isDiagonalMove(currentSquare, targetSquare) || isEatMove(currentSquare, targetSquare);

            return false;
        }

        private bool isEatMove(CheckerSquare i_CurrentSquare, CheckerSquare i_TargetSquare)
        {
            return false;
        }

        private bool isSquaresInBorder(CheckerSquare i_CurrentSquare, CheckerSquare i_TargetSquare)
        {
            return (i_CurrentSquare != null && i_TargetSquare != null);
        }

        private bool isDiagonalMove(CheckerSquare i_CurrentSquare, CheckerSquare i_TargetSquare)
        {
            bool isDiagonal = false;

            if (i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.X) // this case can only go up
            {
                isDiagonal = i_CurrentSquare.m_Posx == (i_TargetSquare.m_Posx - 1);
            }
            else
            {
                isDiagonal = i_CurrentSquare.m_Posx == (i_TargetSquare.m_Posx + 1); // this case can only go down
            }

            isDiagonal = isDiagonal && Math.Abs(i_CurrentSquare.m_Posy - i_TargetSquare.m_Posy) == 1;

            return isDiagonal;
        }


        private int[] getStartPoint(string i_MoveCommand)
        {
            int[] startPoint = new int[2];
            startPoint[0] = i_MoveCommand[0] - 65;
            startPoint[1] = i_MoveCommand[1] - 97;

            return startPoint;
        }

        private int[] getEndPoint(string i_MoveCommand)
        {
            int[] endPoint = new int[2];

            endPoint[0] = i_MoveCommand[3] - 65;
            endPoint[1] = i_MoveCommand[4] - 97;

            return endPoint;
        }
    }
}
