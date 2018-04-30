using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class CheckersLogic
    {
        const int asciiValOfa = 97;
        const int asciiValOfA = 65;
        private CheckersTable m_CheckersTable;
        private int m_TableSize;
        internal Player m_Player;

        public CheckersLogic(CheckersTable i_Table, Player i_Player)
        {
            m_CheckersTable = i_Table;
            m_TableSize = m_CheckersTable.m_Size;
            m_Player = i_Player;
        }

        internal void move(string i_MoveCommand, Player i_Player)
        {
            int[] startPoint = getStartPoint(i_MoveCommand);
            int[] endPoint = getEndPoint(i_MoveCommand);
            CheckerSquare currentSquare = m_CheckersTable.m_Table[startPoint[0], startPoint[1]];
            CheckerSquare targetSquare = m_CheckersTable.m_Table[endPoint[0], endPoint[1]];
            CheckerSquare SquareToFree = null;
            // make the move !
            if (isLegalEat(currentSquare, targetSquare, ref SquareToFree))
            {
                Console.WriteLine("islegaleat = " + isLegalEat(currentSquare, targetSquare, ref SquareToFree) + "with square : " + currentSquare.m_Posx + "," + currentSquare.m_Posy + "->" + targetSquare.m_Posx + "," + targetSquare.m_Posy);
                // perform the eat
                if (SquareToFree.m_CheckerMan.returnType().Equals(" K ") || SquareToFree.m_CheckerMan.returnType().Equals(" X "))
                {
                    m_CheckersTable.m_NumX--;
                    m_Player.m_Points--;
                }
                else
                {
                    m_CheckersTable.m_NumO--;
                    m_Player.m_Points--;
                }

                Console.WriteLine("square to free : " + SquareToFree.m_Posx + " , " + SquareToFree.m_Posy);
                SquareToFree.free();
            }

            if (targetSquare.m_Posx == 0 || targetSquare.m_Posx == (m_TableSize - 1) && currentSquare.m_CheckerMan.m_Type != CheckersMan.eType.K && currentSquare.m_CheckerMan.m_Type != CheckersMan.eType.U) // MAKE KING !!
            {
                if (currentSquare.m_CheckerMan.m_Type == CheckersMan.eType.X)
                {
                    currentSquare.m_CheckerMan = new CheckersMan(CheckersMan.eType.K);
                }
                else if (currentSquare.m_CheckerMan.m_Type == CheckersMan.eType.O)
                {
                    currentSquare.m_CheckerMan = new CheckersMan(CheckersMan.eType.U);
                }
            }

            targetSquare.m_CheckerMan = new CheckersMan(currentSquare.m_CheckerMan.m_Type);
            currentSquare.m_CheckerMan.m_Type = CheckersMan.eType.None;
        }

        internal bool isLegalMove(string i_MoveCommand, ref Player i_MovePlayer)
        {
            int[] startPoint = getStartPoint(i_MoveCommand);
            int[] endPoint = getEndPoint(i_MoveCommand);
            CheckerSquare currentSquare = m_CheckersTable.m_Table[startPoint[0], startPoint[1]];
            CheckerSquare targetSquare = m_CheckersTable.m_Table[endPoint[0], endPoint[1]];

            // checks if the squares are inside the border of the table
            bool firstCheck = isSquaresInBorder(currentSquare, targetSquare);
            firstCheck = firstCheck && (currentSquare.m_CheckerMan.m_Type == i_MovePlayer.m_FirstType || currentSquare.m_CheckerMan.m_Type == i_MovePlayer.m_SecondType);
            // checks if currentsquare is occupied and the target square isnt 
            bool secondCheck = currentSquare.isOccupied() && !targetSquare.isOccupied();
            // check if the move is diagonal, otherwise if its eating move
            bool thirdCheck = false;
            bool fourthCheck = false;
            if (firstCheck && secondCheck)
            {
                thirdCheck = isDiagonalMove(currentSquare, targetSquare);
                if (isEatMove(currentSquare, targetSquare))
                {
                    CheckerSquare SquareToFree = null;
                    fourthCheck = isLegalEat(currentSquare, targetSquare, ref SquareToFree);
                }

            }

            return (firstCheck && secondCheck && (thirdCheck || fourthCheck));
        }

        private bool isEatMove(CheckerSquare i_CurrentSquare, CheckerSquare i_TargetSquare)
        {
            bool isEatMove = false;

            if (i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.X) // if curr square holds X
            {
                isEatMove = i_CurrentSquare.m_Posx == (i_TargetSquare.m_Posx + 2);
            }
            else if (!m_Player.m_JustAte && i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.O) // if curr square holds O
            {
                isEatMove = i_CurrentSquare.m_Posx == (i_TargetSquare.m_Posx - 2); // this case can only go down
            }
            else // if curr square holds U or K (kings)
            {
                isEatMove = Math.Abs(i_CurrentSquare.m_Posx - i_TargetSquare.m_Posx) == 2;
            }

            isEatMove = isEatMove && Math.Abs(i_CurrentSquare.m_Posy - i_TargetSquare.m_Posy) == 2;

            return isEatMove;
        }

        private bool isLegalEatForKings(CheckerSquare i_CurrentSquare, CheckerSquare i_TargetSquare, ref CheckerSquare i_SquareToFree)
        {
            bool isLegalEatForKing = false;
            if (i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.K)
            {
                if (i_CurrentSquare.m_Posx < m_TableSize - 2) // downside eating with K
                {
                    if (i_CurrentSquare.m_Posy < i_TargetSquare.m_Posy) // eat to the right side
                    {
                        isLegalEatForKing = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.O
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                         || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.U
                         && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1];
                    }
                    else // down left eating
                    {
                        isLegalEatForKing = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.O
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                            || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.U
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1];
                    }
                }

                if (i_CurrentSquare.m_Posx > 1)
                {
                    if (i_CurrentSquare.m_Posx > 1) // upside eating with K
                    {
                        if (i_CurrentSquare.m_Posy < i_TargetSquare.m_Posy) // if its eat to the right side
                        {
                            isLegalEatForKing = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.O
                                && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                                || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.U
                                && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                            i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1];
                        }
                        else // if its eat to the left side
                        {
                            isLegalEatForKing = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.O
                                && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                              || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.U
                              && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                            i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1];
                        }
                    }
                }
            }
            else // its U
            {
                if (i_CurrentSquare.m_Posx > 1) // upside eating with U
                {
                    if (i_CurrentSquare.m_Posy < i_TargetSquare.m_Posy) // eat right
                    {
                        isLegalEatForKing = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.X
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                                || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.K
                                && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1];
                    }
                    else // eat left
                    {
                        isLegalEatForKing = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.X
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                            || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.K
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1];
                    }
                }
                if (i_CurrentSquare.m_Posx < m_TableSize - 2) // downside eating with U
                {
                    if (i_CurrentSquare.m_Posy < i_TargetSquare.m_Posy) // eat right
                    {
                        isLegalEatForKing = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.X
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                                || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.K
                                && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1];
                    }
                    else // eat left
                    {
                        isLegalEatForKing = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.X
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                            || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.K
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1];
                    }
                }
            }

            return isLegalEatForKing;
        }

        private bool isLegalEatForMen(CheckerSquare i_CurrentSquare, CheckerSquare i_TargetSquare, ref CheckerSquare i_SquareToFree)
        {
            bool isLegalEat = false;
            if (i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.X) // X
            {
                if (i_CurrentSquare.m_Posx > 1) // upside eating
                {
                    if (i_CurrentSquare.m_Posy < i_TargetSquare.m_Posy) // if its eat to the right side
                    {
                        isLegalEat = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.O
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                            || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.U
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1];
                    }
                    else // if its eat to the left side
                    {
                        isLegalEat = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.O
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                          || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.U
                          && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1];
                    }
                }
            }
            else // O
            {
                if (i_CurrentSquare.m_Posx < m_TableSize - 2) // we are eating down
                {
                    if (i_CurrentSquare.m_Posy < i_TargetSquare.m_Posy) // if its eat right
                    {
                        isLegalEat = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.X
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                                    || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.K
                                    && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy + 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1];
                    }
                    else // left eat
                    {
                        isLegalEat = (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.X
                            && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None)
                                || (m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.K
                                && m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy - 2].m_CheckerMan.m_Type == CheckersMan.eType.None);
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1];
                    }
                }
            }

            return isLegalEat;
        }

        private bool isLegalEat(CheckerSquare i_CurrentSquare, CheckerSquare i_TargetSquare, ref CheckerSquare i_SquareToFree)
        {
            bool isLegalEat = false;

            if (i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.K || i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.U)
            {
                isLegalEat = isLegalEatForKings(i_CurrentSquare, i_TargetSquare, ref i_SquareToFree);
            }
            else
            {
                isLegalEat = isLegalEatForMen(i_CurrentSquare, i_TargetSquare, ref i_SquareToFree);
            }

            return isLegalEat;
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
                isDiagonal = i_CurrentSquare.m_Posx == (i_TargetSquare.m_Posx + 1);
            }
            else if (i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.O)
            {
                isDiagonal = i_CurrentSquare.m_Posx == (i_TargetSquare.m_Posx - 1); // this case can only go down
            }
            else // KINGS CASE
            {
                isDiagonal = Math.Abs(i_CurrentSquare.m_Posx - i_TargetSquare.m_Posx) == 1;
            }

            isDiagonal = isDiagonal && Math.Abs(i_CurrentSquare.m_Posy - i_TargetSquare.m_Posy) == 1;

            return isDiagonal;
        }


        private int[] getStartPoint(string i_MoveCommand)
        {
            Console.WriteLine("startpoint - " + i_MoveCommand);
            int[] startPoint = new int[2];
            startPoint[1] = i_MoveCommand[0] - 65;
            startPoint[0] = i_MoveCommand[1] - 97;

            return startPoint;
        }

        private int[] getEndPoint(string i_MoveCommand)
        {
            Console.WriteLine("endpoint - " + i_MoveCommand);
            int[] endPoint = new int[2];

            endPoint[1] = i_MoveCommand[3] - 65;
            endPoint[0] = i_MoveCommand[4] - 97;

            return endPoint;
        }

        internal string[] getPossibleMovesForPlayer(ref CheckerSquare[] i_SquareList)
        {
            string[] possibleMoves;
            string[] possibleMovesForSingleSquare;
            int countKings = 0;
            int countNormals = 0;
            for (int c = 0; c < i_SquareList.Length; c++)
            {
                if (i_SquareList[c].m_CheckerMan.m_Type == CheckersMan.eType.O)
                {
                    countNormals++;
                }
                else
                {
                    countKings++;
                }
            }

            possibleMoves = new string[6 * countNormals + 8 * countKings];

            int i = 0;
            for (int m = 0; m < i_SquareList.Length; m++)
            {
                if (i_SquareList[m].m_CheckerMan.m_Type == CheckersMan.eType.U)
                {
                    possibleMovesForSingleSquare = new string[8];
                }
                else
                {
                    possibleMovesForSingleSquare = new string[6];
                }

                possibleMovesForSingleSquare = getPossibleMoves(i_SquareList[m]);
                for (int j = 0; j < possibleMovesForSingleSquare.Length; j++)
                {
                    possibleMoves[i++] = possibleMovesForSingleSquare[j];
                }
            }

            return possibleMoves;
        }
        internal string[] getPossibleMoves(CheckerSquare i_CurrentSquare)
        {
            string[] possibleMoves;

            Console.WriteLine("getting possible moves for : " + i_CurrentSquare.m_Posx + " , " + i_CurrentSquare.m_Posy);
            if (i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.K || i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.U)
            {
                possibleMoves = new string[8];
                possibleMoves[0] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1);
                possibleMoves[1] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1);
                possibleMoves[2] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1);
                possibleMoves[3] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1);
                possibleMoves[4] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy + 2);
                possibleMoves[5] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy - 2);
                possibleMoves[6] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy + 2);
                possibleMoves[7] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy - 2);

            }
            else if (i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.X)
            {
                possibleMoves = new string[6];
                possibleMoves[0] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1);
                possibleMoves[1] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1);
                possibleMoves[2] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy + 2);
                possibleMoves[3] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy - 2);
                possibleMoves[4] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy + 2);
                possibleMoves[5] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy - 2);
            }
            else
            {
                possibleMoves = new string[6];
                possibleMoves[0] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1);
                possibleMoves[1] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1);
                possibleMoves[2] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy + 2);
                possibleMoves[3] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy - 2);
                possibleMoves[4] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy + 2);
                possibleMoves[5] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy - 2);
            }

            return possibleMoves;
        }

        private string createMoveFromIndices(int startX, int startY, int Endx, int Endy)
        {
            char currentRowChar = Convert.ToChar((startX + 97));
            char currentColChar = Convert.ToChar((startY + 65));
            char targetRowChar = Convert.ToChar((Endx + 97));
            char targetColChar = Convert.ToChar((Endy + 65));
            string move = currentColChar + "" + currentRowChar + ">" + targetColChar + targetRowChar;

            if (!checkRange(move) || !isLegalMove(move, ref m_Player))
            {
                move = "Aa>Aa";
            }

            return move;
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

        internal string canEatAgain(string i_move)
        {
            int playerPosx = Convert.ToChar(i_move[4] - 97);
            int playerPosy = Convert.ToChar(i_move[3] - 65);

            return canEatAfterEat(m_CheckersTable.m_Table[playerPosx, playerPosy]);
        }

        private string canEat(CheckerSquare i_Square)
        {
            bool canEat = false;
            Console.WriteLine("check caneat for : " + i_Square.m_Posx + "," + i_Square.m_Posy);
            string[] possibleMoves = getPossibleMoves(i_Square);
            string eatMove = "";
            for (int i = 0; i < possibleMoves.Length; i++)
            {
                Console.WriteLine(possibleMoves[i]);
                int[] startPoint = getStartPoint(possibleMoves[i]);
                int[] endPoint = getEndPoint(possibleMoves[i]);
                CheckerSquare currentSquare = m_CheckersTable.m_Table[startPoint[0], startPoint[1]];
                CheckerSquare targetSquare = m_CheckersTable.m_Table[endPoint[0], endPoint[1]];
                if (isEatMove(currentSquare, targetSquare))
                {
                    CheckerSquare SquareToFree = null;
                    if (isLegalEat(currentSquare, targetSquare, ref SquareToFree))
                    {
                        eatMove = possibleMoves[i];
                        canEat = true;
                        break;
                    }
                }
            }

            return eatMove;
        }

        internal string canEatAfterEat(CheckerSquare i_Square)
        {
            bool canEat = false;
            string[] possibleMoves = getPossibleMoves(i_Square);
            string eatMove = "";
            for (int i = 0; i < possibleMoves.Length; i++)
            {
                int[] startPoint = getStartPoint(possibleMoves[i]);
                int[] endPoint = getEndPoint(possibleMoves[i]);
                CheckerSquare currentSquare = m_CheckersTable.m_Table[startPoint[0], startPoint[1]];
                CheckerSquare targetSquare = m_CheckersTable.m_Table[endPoint[0], endPoint[1]];
                Console.WriteLine("possible move: " + possibleMoves[i]);
                if (isEatMove(currentSquare, targetSquare))
                {
                    Console.WriteLine("iseatmove -> true");
                    CheckerSquare SquareToFree = null;
                    bool isLegEat = isLegalEat(currentSquare, targetSquare, ref SquareToFree);
                    bool isLegEatAfterEat = isLegalEatAfterEat(currentSquare, targetSquare, ref SquareToFree);
                    if (isLegEat || isLegEatAfterEat)
                    {
                        Console.WriteLine("islegal eat = " + isLegEat);
                        Console.WriteLine("islegal eat after eat = " + isLegEatAfterEat);
                        eatMove = possibleMoves[i];
                        canEat = true;
                        break;
                    }
                }
            }

            return eatMove;
        }

        private bool isLegalEatAfterEat(CheckerSquare i_CurrentSquare, CheckerSquare i_TargetSquare, ref CheckerSquare i_SquareToFree)
        {
            bool isLegalBackWardEat = false;
            if (i_CurrentSquare.m_CheckerMan.m_Type == CheckersMan.eType.X)
            {
                if (i_CurrentSquare.m_Posx < m_TableSize - 2) // downside eating with X
                {
                    if (i_CurrentSquare.m_Posy < i_TargetSquare.m_Posy) // eat to the right side
                    {
                        isLegalBackWardEat = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.O
                         || m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.U;
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1];
                    }
                    else // down left eating
                    {
                        isLegalBackWardEat = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.O
                            || m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.U;
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1];
                    }
                }
            }
            else // can eat up with O
            {
                if (i_CurrentSquare.m_Posx > 1) // upside eating with O
                {
                    if (i_CurrentSquare.m_Posy < i_TargetSquare.m_Posy) // eat right
                    {
                        isLegalBackWardEat = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.X
                                || m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1].m_CheckerMan.m_Type == CheckersMan.eType.K;
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1];
                    }
                    else // eat left
                    {
                        isLegalBackWardEat = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.X
                            || m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1].m_CheckerMan.m_Type == CheckersMan.eType.K;
                        i_SquareToFree = m_CheckersTable.m_Table[i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1];
                    }
                }
            }
                          
                return isLegalBackWardEat;
        }

        private string[] getPossibleMovesAfterEat(CheckerSquare i_CurrentSquare)
        {
            string[] possibleMoves;

            possibleMoves = new string[8];
            possibleMoves[0] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy + 1);
            possibleMoves[1] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 1, i_CurrentSquare.m_Posy - 1);
            possibleMoves[2] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy + 1);
            possibleMoves[3] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 1, i_CurrentSquare.m_Posy - 1);
            possibleMoves[4] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy + 2);
            possibleMoves[5] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx + 2, i_CurrentSquare.m_Posy - 2);
            possibleMoves[6] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy + 2);
            possibleMoves[7] = createMoveFromIndices(i_CurrentSquare.m_Posx, i_CurrentSquare.m_Posy, i_CurrentSquare.m_Posx - 2, i_CurrentSquare.m_Posy - 2);

            return possibleMoves;
        }

        internal bool isEatMove(string i_MoveMessage)
        {
            int[] startPoint = getStartPoint(i_MoveMessage);
            int[] endPoint = getEndPoint(i_MoveMessage);
            CheckerSquare currentSquare = m_CheckersTable.m_Table[startPoint[0], startPoint[1]];
            CheckerSquare targetSquare = m_CheckersTable.m_Table[endPoint[0], endPoint[1]];

            return isSquaresInBorder(currentSquare, targetSquare) && isEatMove(currentSquare, targetSquare);
        }

        internal string checkIfCanEat(string i_MoveMessage)
        {
            Console.WriteLine("checks if can eat " + i_MoveMessage);
            int id = 0;
            CheckerSquare[] cSquare;
            string eatMove = "";

            if (m_Player.m_FirstType == CheckersMan.eType.K || m_Player.m_FirstType == CheckersMan.eType.X)
            {
                id = 0;
                cSquare = m_CheckersTable.GetCheckerSquares(id);
            }
            else
            {
                id = 1;
                cSquare = m_CheckersTable.GetCheckerSquares(id);
            }

            for(int i = 0; i < cSquare.Length; i++)
            {
                eatMove = canEat(cSquare[i]);
                if (!eatMove.Equals(""))
                {
                    break;
                }
            }

            return eatMove;
        }

        internal void punish(string i_MoveMessage, string i_EatMessage)
        {
            int X;
            int Y;
            // in this case the square that should be punished moved
            if (i_MoveMessage.Substring(0,2).Equals(i_EatMessage.Substring(0,2)))
            {
                X = Convert.ToInt32(i_MoveMessage[4] - 97);
                Y = Convert.ToInt32(i_MoveMessage[3] - 65);
            }
            else
            {
                X = Convert.ToInt32(i_EatMessage[1] - 97);
                Y = Convert.ToInt32(i_EatMessage[0] - 65);
            }

            if (m_CheckersTable.m_Table[X, Y].m_CheckerMan.m_Type == CheckersMan.eType.K || m_CheckersTable.m_Table[X, Y].m_CheckerMan.m_Type == CheckersMan.eType.X)
            {
                m_CheckersTable.m_NumX--;
            }
            else
            {
                m_CheckersTable.m_NumO--;
            }

            Console.WriteLine("i am freeing square : " + X + " , " + Y);
            m_CheckersTable.m_Table[X, Y].free();
        }
    }
}
