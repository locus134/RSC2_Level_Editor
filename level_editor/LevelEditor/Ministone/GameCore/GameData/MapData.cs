using System.Collections.Generic;
using Ministone.GameCore.GameData.Generic;
using Newtonsoft.Json;

namespace Ministone.GameCore.GameData
{
    public class MapData
    {
        int m_id;                                                     // 餐厅ID
        int m_levelCount;                                             // 关卡数量
        string m_key;                                                 // 餐厅key
        int m_startLv;                                                // 开始关卡
        int m_endLv;                                                  // 结束关卡
        Dictionary<string, string> m_displayName;                     // 餐厅名称（多国语言）
        List<string> m_foodList = new List<string>();                 // 包含的食物列表
        List<string> m_customerList = new List<string>();             // 包含的普通顾客列表
        List<string> m_specialCustomerList = new List<string>();      // 特殊顾客列表

        public MapData()
        {
            m_displayName = new Dictionary<string, string>();
            m_foodList = new List<string>();
            m_customerList = new List<string>();
            m_specialCustomerList = new List<string>();
        }

        public int id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public string key
        {
            get { return m_key; }
            set { m_key = value; }
        }

        public Dictionary<string, string> display_name
        {
            get { return m_displayName; }
            set { m_displayName = value; }
        }

        public string GetDisplayName(string lang = "en")
        {
            string name;
            m_displayName.TryGetValue(lang, out name);
            return name;
        }

        public int start_level
        {
            get { return m_startLv; }
            set { m_startLv = value; }
        } 

        public int end_level
        {
            get { return m_endLv; }
            set { m_endLv = value; }
        }

        public int level_count
        {
            get { return m_levelCount; }
            set { m_levelCount = value; }
        }

        public List<string> food_list
        {
            get { return m_foodList; }
            set { m_foodList = value; }
        }

        public List<string> customer_list
        {
            get { return m_customerList; }
            set { m_customerList = value; }
        }

        public List<string> special_customer_list
        {
            get { return m_specialCustomerList; }
            set { m_specialCustomerList = value; }
        }

        public bool IsSpecialCustomer(string customer)
        {
            return m_specialCustomerList.Contains(customer);
        }

    }
}
