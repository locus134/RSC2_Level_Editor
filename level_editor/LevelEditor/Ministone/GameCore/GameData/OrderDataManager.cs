using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace Ministone.GameCore.GameData
{
    

    public class OrderDataManager
    {
        static OrderDataManager _instance = null;
        List<OrderData> m_orderList;                    // 所有订单列表
        Dictionary<string, int> m_orderIndex;           // 订单索引表，按照customer#food来索引
        Dictionary<string, List<int>> m_customerIndex;  // 顾客索引表
        Dictionary<string, List<int>> m_foodIndex;      // 食物索引表

        SQLiteConnection m_cnn;

        private OrderDataManager()
        {
            m_orderList = new List<OrderData>();
            m_customerIndex = new Dictionary<string, List<int>>();
            m_foodIndex = new Dictionary<string, List<int>>();
            m_orderIndex = new Dictionary<string, int>();
        }

        public static OrderDataManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new OrderDataManager();
            }
            return _instance;
        }

        private void addNewOrder(OrderData od, int index)
        {
            string key = od.customer + "#" + od.food;
            if(m_orderIndex.ContainsKey(key))
            {
                System.Diagnostics.Debug.WriteLine("重复的订单:" +  key);       
            }
            else
            {
                m_orderIndex.Add(key, index);
            }

            List<int> orderList = null;
            if (m_customerIndex.TryGetValue(od.customer, out orderList))
            {
                orderList.Add(index);
            }
            else
            {
                orderList = new List<int>();
                orderList.Add(index);
                m_customerIndex.Add(od.customer, orderList);
            }

            if (m_foodIndex.TryGetValue(od.food, out orderList))
            {
                orderList.Add(index);
            }
            else
            {
                orderList = new List<int>();
                orderList.Add(index);
                m_foodIndex.Add(od.food, orderList);
            }
        }

        public void SetAllOrders(List<OrderData> allOrders)
        {
            m_orderList = allOrders;
            m_orderIndex.Clear();
            m_customerIndex.Clear();
            m_foodIndex.Clear();

            for (int i = 0; i < m_orderList.Count; ++i)
            {
                addNewOrder(m_orderList[i], i);
            }
        }

        // 加载JSON格式订单配置数据
        public bool LoadFromDBFile(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                return false;
            }

            m_cnn = new SQLiteConnection();
            m_cnn.ConnectionString = string.Format("Data Source={0};Version=3", dbPath);
            m_cnn.Open();

            var cmd = m_cnn.CreateCommand();
            cmd.CommandText = "Select customer_key,food_key,wait_time,tips,consider_time FROM orderfood";
            var reader = cmd.ExecuteReader();
            if (reader == null)
            {
                System.Diagnostics.Debug.WriteLine("Error loading order data from DBFile!");
                return false;
            }
            m_orderList.Clear();
            m_orderIndex.Clear();
            m_customerIndex.Clear();
            m_foodIndex.Clear();

            List<OrderData> orderList = new List<OrderData>();
            while (reader.Read())
            {
                int column = 0;
                var ordData = new OrderData();
                ordData.customer = reader.GetStringSafe(column++);
                ordData.food = reader.GetStringSafe(column++);
                ordData.wait_time = reader.GetFloatSafe(column++);
                ordData.tip = reader.GetInt32Safe(column++);
                ordData.consider_time = reader.GetFloatSafe(column++);

                orderList.Add(ordData);
            }
            //List<OrderData> orderList = JsonConvert.DeserializeObject<List<OrderData>>(jsonStr);
            SetAllOrders(orderList);

            return true;
        }

        // 获取JSON字符串
        public string GetJsonString()
        {
            return JsonConvert.SerializeObject(m_orderList, Formatting.Indented);
        }

        // 根据顾客和食物获取订单
        public OrderData GetOrder(string customer, string food)
        {
            string index = customer + "#" + food;
            int i;
            if (m_orderIndex.TryGetValue(index, out i))
            {
                return m_orderList[i];
            }
            return null;
        }

        public List<OrderData> GetAllOrders()
        {
            List<OrderData> orders = new List<OrderData>(m_orderList);
            return orders;
        }

        // 获取顾客所有订单
        public List<OrderData> GetCustomerOrders(string customer)
        {
            List<int> orderIndexes;
            List<OrderData> ret = new List<OrderData>();
            if (m_customerIndex.TryGetValue(customer, out orderIndexes))
            {
                for (int i = 0; i < orderIndexes.Count; ++ i)
                {
                    ret.Add(m_orderList[orderIndexes[i]]);
                }
            }
            return ret;
        }

        // 获取食物的所有订单
        public List<OrderData> GetFoodOrders(string food)
        {
            List<int> orderIndexes;
            List<OrderData> ret = new List<OrderData>();
            if (m_foodIndex.TryGetValue(food, out orderIndexes))
            {
                for (int i = 0; i < orderIndexes.Count; ++i)
                {
                    ret.Add(m_orderList[orderIndexes[i]]);
                }
            }
            return ret;
        }

        // 增加一个订单配置
        public void AddOrder(OrderData ord)
        {
            string key = ord.customer + "#" + ord.food;
            if (!m_orderIndex.ContainsKey(key))
            {
                m_orderList.Add(ord);
                addNewOrder(ord, m_orderList.Count - 1);
            }
        }

        // 删除一个订单
        public void RemoveOrder(string customer, string food)
        {
            string key = customer + "#" + food;
            int index;
            if (m_orderIndex.TryGetValue(key, out index))
            {
                RemoveOrder(index);
            }
        }

        public void RemoveOrder(OrderData ord)
        {
            RemoveOrder(ord.customer, ord.food);
        }

        public void RemoveOrder(int index)
        {
            if (index >= 0 && index < m_orderList.Count)
            {
                OrderData ord = m_orderList[index];
                string customer = ord.customer;
                string food = ord.food;
                string key = customer + "#" + food;

                m_orderList.RemoveAt(index);
                m_orderIndex.Remove(key);

                List<int> orderList;
                if (m_customerIndex.TryGetValue(customer, out orderList))
                {
                    for (int i = 0; i < orderList.Count; ++i)
                    {
                        if (orderList[i] > index)
                        {
                            orderList[i] -= 1;
                        }
                        else if (orderList[i] == index)
                        {
                            orderList.Remove(i);
                        }
                    }
                }

                if (m_foodIndex.TryGetValue(food, out orderList))
                {
                    for (int i = 0; i < orderList.Count; ++i)
                    {
                        if (orderList[i] > index)
                        {
                            orderList[i] -= 1;
                        }
                        else if (orderList[i] == index)
                        {
                            orderList.Remove(i);
                        }
                    }
                }
            }
        }


    }
}
