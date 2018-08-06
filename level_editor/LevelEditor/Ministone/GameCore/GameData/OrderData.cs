namespace Ministone.GameCore.GameData
{
    public class OrderData
    {
        int m_tip;                              // 小费
        float m_waitTime;                       // 等待时间
        float m_considerTime;                   // 考虑时间
        string m_customer;                      // 顾客key
        string m_food;                          // 食物key

        public string customer
        {
            get { return m_customer; }
            set { m_customer = value; }
        }

        public string food
        {
            get { return m_food; }
            set { m_food = value; }
        }

        public float wait_time
        {
            get { return m_waitTime; }
            set { m_waitTime = value; }
        }

        public int tip
        {
            get { return m_tip; }
            set { m_tip = value; }
        }

        public float consider_time
        {
            get { return m_considerTime; }
            set { m_considerTime = value; }
        }
    }
}
