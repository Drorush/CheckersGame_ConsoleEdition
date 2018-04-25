using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    // this class represents man object
    class CheckersMan
    {
        internal enum eType
        {
            O,
            X,
            U,
            K,
        };
        internal eType m_Type;
        internal int m_Posx;
        internal int m_Posy;

        public CheckersMan(int i_Posx, int i_Posy, eType i_Type)
        {
            m_Posx = i_Posx;
            m_Posy = i_Posy;
            m_Type = i_Type;

        }

        internal string returnType()
        {
            string type = "   ";

            switch(m_Type)
            {
                case eType.O:
                    type = " O ";
                    break;
                case eType.X:
                    type = " X ";
                    break;
                case eType.U:
                    type = " U ";
                    break;
                case eType.K:
                    type = " K ";
                    break;
            }

            return type;
        }
    }
}
