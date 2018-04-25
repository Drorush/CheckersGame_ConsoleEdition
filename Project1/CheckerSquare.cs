using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class CheckerSquare
    {
        internal int m_Posx;
        internal int m_Posy;
        internal CheckersMan m_CheckerMan;

        public CheckerSquare(int i_Posx, int i_Posy, CheckersMan i_CheckerMan)
        {
            m_Posx = i_Posx;
            m_Posy = i_Posy;
            m_CheckerMan = i_CheckerMan;
        }

        // free occupied square
        private void free(int i_Posx, int i_Posy)
        {
            m_CheckerMan = null;
        }

        // checks if a given square is occupied
        internal bool isOccupied()
        {
            return m_CheckerMan != null;
        }

        public override string ToString()
        {
            string squareOccupier;
            
            if (m_CheckerMan == null)
            {
                squareOccupier = "   ";
            }
            else
            {
                squareOccupier = m_CheckerMan.returnType();
            }

            return squareOccupier;
        }
    }
}
