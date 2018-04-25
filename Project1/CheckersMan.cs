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
        internal enum Type { O, X, U, K };
        internal int m_Posx;
        internal int m_Posy;

        public CheckersMan(int i_Posx, int i_Posy, Type i_type)
        {

            m_Posx = i_Posx;
            m_Posy = i_Posy;
        }
    }
}
