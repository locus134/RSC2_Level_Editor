using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Data.SQLite;
using Ministone.GameCore.GameData.Generic;
using System;

namespace Ministone.GameCore.GameData
{
    

    public class OrderDataManager
    {
        static OrderDataManager _instance = null;
        List<OrderData> m_orderList;                    // 所有订单列表
        Dictionary<string, int> m_orderIndex;           // 订单索引表，按照customer#food来索引
        Dictionary<string, List<int>> m_customerIndex;  // 顾客索引表
        Dictionary<string, List<int>> m_foodIndex;      // 食物索引表

        string m_dbPath;
        string m_connectString;

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

        public void AddOrders(List<OrderData> orders)
        {
            var cnn = new SQLiteConnection(m_connectString);
            if (cnn.State != System.Data.ConnectionState.Connecting)
            {
                cnn.Open();

                var cmd = cnn.CreateCommand();
                cmd.CommandText = "INSERT INTO orderfood(customer_id,customer_key,Name_CN,food_id,food_key,foodname_cn,wait_time,tips,consider_time) VALUES(@customer_id,@customer_key,@Name_CN,@food_id,@food_key,@foodname_cn,@wait_time,@tips,@consider_time)";

                foreach(OrderData ord in orders)
                {
                    var cusInfo = CustomerDataManager.GetInstance().GetCustomer(ord.customer);
                    var foodInfo = FoodDataManager.GetInstance().GetFood(ord.food);

                    cmd.Parameters.Add("customer_id", System.Data.DbType.Int32).Value = cusInfo.id;
                    cmd.Parameters.Add("customer_key", System.Data.DbType.String).Value = cusInfo.key;
                    cmd.Parameters.Add("Name_CN", System.Data.DbType.String).Value = cusInfo.display_name["cn"];

                    cmd.Parameters.Add("food_id", System.Data.DbType.Int32).Value = foodInfo.id;
                    cmd.Parameters.Add("food_key", System.Data.DbType.String).Value = foodInfo.key;
                    cmd.Parameters.Add("foodname_cn", System.Data.DbType.String).Value = foodInfo.display_name["cn"];

                    cmd.Parameters.Add("wait_time", System.Data.DbType.Double).Value = ord.wait_time;
                    cmd.Parameters.Add("tips", System.Data.DbType.Int32).Value = ord.tip;
                    cmd.Parameters.Add("consider_time", System.Data.DbType.Double).Value = ord.consider_time;
                    int ret = cmd.ExecuteNonQuery();
                    MyDebug.WriteLine("Add Order result : " + ret);
                }
                cmd.Dispose();
            }
        
            cnn.Close();
            cnn.Dispose();
        }

        public void DeleteOrders(List<OrderData> orders)
        {
            var cnn = new SQLiteConnection(m_connectString);
            if (cnn.State != System.Data.ConnectionState.Connecting)
            {
                cnn.Open();

                var cmd = cnn.CreateCommand();
                foreach (OrderData ord in orders)
                {
                    cmd.CommandText = string.Format("DELETE FROM orderfood WHERE customer_key='{0}' AND food_key='{1}'", ord.customer, ord.food);
                    try{
                        int ret = cmd.ExecuteNonQuery();
                        MyDebug.WriteLine("Delete Order result : " + ret); 
                    }catch(Exception e)
                    {
                        MyDebug.WriteLine("Delete Order error"); 
                        MyDebug.WriteLine(e);
                    }

                }
                cmd.Dispose();
            }
            cnn.Close();
            cnn.Dispose();
        }



        public void UpdateOrders(List<OrderData> orders)
        {
            var cnn = new SQLiteConnection(m_connectString);
            if (cnn.State != System.Data.ConnectionState.Connecting)
            {
                cnn.Open();

                string addCmd = "INSERT INTO orderfood(customer_id,customer_key,Name_CN,food_id,food_key,foodname_cn,wait_time,tips,consider_time) VALUES(@customer_id,@customer_key,@Name_CN,@food_id,@food_key,@foodname_cn,@wait_time,@tips,@consider_time)";
                foreach (OrderData ord in orders)
                {
                    //cmd.CommandText = string.Format("SELECT * FROM orderfood WHERE customer_key={0},food_key={1}", ord.customer, ord.food);
                    var findCmd = cnn.CreateCommand();
                    findCmd.CommandText = string.Format("SELECT food_key " +
                                                        "FROM orderfood " +
                                                        "WHERE customer_key='{0}' AND food_key='{1}'", ord.customer, ord.food);
                    var reader = findCmd.ExecuteReader();
                    bool isExsit = reader.Read();
                    reader.Close();
                    findCmd.Dispose();

                    var updateOrAddCmd = cnn.CreateCommand();
                    if(isExsit)
                    {// 找到了
                        
                        updateOrAddCmd.CommandText = string.Format("UPDATE orderfood " +
                                                                   "SET customer_id=@customer_id,customer_key=@customer_key,Name_CN=@Name_CN,food_id=@food_id,food_key=@food_key,foodname_cn=@foodname_cn,wait_time=@wait_time,tips=@tips,consider_time=@consider_time " +
                                                                   "WHERE customer_key='{0}' AND food_key='{1}'", ord.customer, ord.food);
                        MyDebug.WriteLine("Update Order");
                    }
                    else
                    {// 找不到
                        updateOrAddCmd.CommandText = addCmd;
                        MyDebug.WriteLine("Create Order");
                    }

                    var cusInfo = CustomerDataManager.GetInstance().GetCustomer(ord.customer);
                    var foodInfo = FoodDataManager.GetInstance().GetFood(ord.food);

                    updateOrAddCmd.Parameters.Add("customer_id", System.Data.DbType.Int32).Value = cusInfo.id;
                    updateOrAddCmd.Parameters.Add("customer_key", System.Data.DbType.String).Value = cusInfo.key;
                    updateOrAddCmd.Parameters.Add("Name_CN", System.Data.DbType.String).Value = cusInfo.display_name["cn"];

                    updateOrAddCmd.Parameters.Add("food_id", System.Data.DbType.Int32).Value = foodInfo.id;
                    updateOrAddCmd.Parameters.Add("food_key", System.Data.DbType.String).Value = foodInfo.key;
                    updateOrAddCmd.Parameters.Add("foodname_cn", System.Data.DbType.String).Value = foodInfo.display_name["cn"];

                    updateOrAddCmd.Parameters.Add("wait_time", System.Data.DbType.Double).Value = ord.wait_time;
                    updateOrAddCmd.Parameters.Add("tips", System.Data.DbType.Int32).Value = ord.tip;
                    updateOrAddCmd.Parameters.Add("consider_time", System.Data.DbType.Double).Value = ord.consider_time;

                    try
                    {
                        int updateOrAddResult = updateOrAddCmd.ExecuteNonQuery();
                        MyDebug.WriteLine("Update Order result : " + updateOrAddResult);
                    }
                    catch (Exception e)
                    {
                        MyDebug.WriteLine(e);
                    }
                    updateOrAddCmd.Dispose();
                }
            }

            cnn.Close();
        }


        // 加载JSON格式订单配置数据
        public bool LoadFromDBFile(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                return false;
            }
            m_dbPath = dbPath;
            m_connectString = string.Format("Data Source={0};Version=3", dbPath);

            var sqlCnn = new SQLiteConnection(m_connectString);
            sqlCnn.Open();

            var cmd = sqlCnn.CreateCommand();
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
            reader.Close();
            sqlCnn.Close();

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
