using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class CheckersLogic
    {
        private CheckersTable m_Table;

        public CheckersLogic(CheckersTable i_Table)
        {
            m_Table = i_Table;
        }

        internal bool isLegalMove(CheckersMan i_CheckersMan, string i_MoveCommand)
        {
            return false;
        }

        internal void translateMoveCommandToPosx(string i_MoveCommand)
        {

        }

        internal void translateMoveCommandToPosy(string i_MoveCommand)
        {

        }
    }
}
