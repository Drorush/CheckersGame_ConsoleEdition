namespace Project1
{
    internal class Player
    {
        internal int m_Points;
        internal string m_Name;

        // 0 if this player is X or K , 1 if this player is U or O
        internal int m_ID; 
        internal CheckersMan.eType m_FirstType;
        internal CheckersMan.eType m_SecondType;
        internal bool m_JustAte;
        internal int m_TotalPoints = 0;

        public Player(string i_name, int i_points, int i_id)
        {
            m_Name = i_name;
            m_Points = i_points;
            m_ID = i_id;
            if (i_id == 0)
            {
                m_FirstType = CheckersMan.eType.X;
                m_SecondType = CheckersMan.eType.K;
            }
            else
            {
                m_FirstType = CheckersMan.eType.O;
                m_SecondType = CheckersMan.eType.U;
            }
        }
    }
}
