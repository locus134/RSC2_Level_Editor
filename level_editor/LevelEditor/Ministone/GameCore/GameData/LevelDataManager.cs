using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data.SQLite;
using System;
using Ministone.GameCore.GameData.Generic;
using System.Text;
using System.IO;

namespace Ministone.GameCore.GameData
{
    public class LevelDataManager
    {
        static private LevelDataManager _instance = null;
        Dictionary<string, List<LevelData>> m_levelDataDict;
        Dictionary<string, Dictionary<int, int>> m_levelIndexes;

        Dictionary<string, LevelType> LevelTypeDict;

        HashSet<string> m_customers = new HashSet<string>();
        HashSet<string> m_foods = new HashSet<string>();

        string m_dbPath;

        private LevelDataManager()
        {
            LevelTypeDict = new Dictionary<string, LevelType>();
            LevelTypeDict.Add("FIXED_CUSTOMER", LevelType.FIXED_CUSTOMER);
            LevelTypeDict.Add("FIXED_TIME", LevelType.FIXED_TIME);
            LevelTypeDict.Add("LOST_CUSTOMER", LevelType.LOST_CUSTOMER);

            m_levelDataDict = new Dictionary<string, List<LevelData>>();
            m_levelIndexes = new Dictionary<string, Dictionary<int, int>>();
        }

        static public LevelDataManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LevelDataManager();
            }
            return _instance;
        }

        public void Clear()
        {
            m_levelDataDict.Clear();
            m_levelIndexes.Clear();
        }

        public bool initWithDBFile(string dbPath)
        {
            if(string.IsNullOrEmpty(dbPath))
            {
                return false;
            }

            if(File.Exists(dbPath))
            {
                m_dbPath = dbPath;
            }

            return true;
        }


        public bool LoadLevelDataForMap(string mapKey)
        {
            m_customers.Clear();
            m_foods.Clear();

            var mapData = MapDataManager.GetInstance().GetMapData(mapKey);

            var sqlCnn = new SQLiteConnection();
            sqlCnn.ConnectionString = string.Format("Data Source={0};Version = 3", m_dbPath);
            sqlCnn.Open();

            var cmd = sqlCnn.CreateCommand();

            List<LevelData> levels = new List<LevelData>();
            for (int i = mapData.start_level; i <= mapData.end_level; i++)
            {
                
                cmd.CommandText = string.Format("SELECT " +
                                                "level,type,total,star_score,orders,special_orders,new_foods,guide_orders," +
                                                "anyfood_orders,max_order,order_interval,first_arrival,waiting_time_decay,secret_customers," +
                                                "mucky_interval,broken_interval,rain_interval,requirement,organic_materials,required_kitchenware,unlock,rewards " +
                                                "FROM Level " +
                                                "WHERE level={0}", i);
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (reader == null)
                {
                    Generic.MyDebug.WriteLine(string.Format("Level={0} is not found", i));
                    return false;
                }

                while (reader.Read())
                {
                    LevelData lvData = new LevelData();
                    int column = 0;

                    Func<string> getNextString = () =>
                    {
                        return reader.GetStringSafe(column++);
                    };

                    Func<int> getNextInt32 = () =>
                    {
                        return reader.GetInt32Safe(column++);
                    };

                    Func<float> getNextFloat = () =>
                    {
                        return reader.GetFloatSafe(column++);
                    };

                    lvData.id = getNextInt32();
                    lvData.type = LevelTypeDict[getNextString()];
                    lvData.total = getNextInt32();
                    string strScoreList = getNextString();
                    if (strScoreList.Length > 0)
                    {
                        var scoreList = strScoreList.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string score in scoreList)
                        {
                            lvData.scoreList = new List<string>(scoreList);
                        }
                    }
                    lvData.orders = ParseOrders(getNextString());
                    lvData.specialOrders = ParseSpecialOrders(getNextString());
                    string newfood = getNextString();
                    string guideOrders = getNextString();
                    lvData.anyfoodOrders = ParseAnyfoodOrders(getNextString());
                    lvData.max_order = getNextInt32();
                    string[] orderIntervals = getNextString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); ;
                    lvData.order_interval.set(orderIntervals[0].ToFloat(), orderIntervals[1].ToFloat());
                    string[] first_arrivals = getNextString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); ;
                    foreach (string arrTime in first_arrivals)
                    {
                        lvData.first_arrivals.Add(arrTime.ToFloat());
                    }
                    string[] decayInfo = getNextString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); ;
                    if (decayInfo.Length >= 2)
                    {
                        lvData.waiting_decay.set(decayInfo[0].ToFloat(), decayInfo[1].ToFloat());
                    }

                    string secretStr = getNextString();
                    lvData.parseSecretCustomer(secretStr);

                    var litter_interval = getNextString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); ;
                    if (litter_interval.Length > 0)
                    {
                        lvData.litter_interval.set(litter_interval[0].ToInt32(), litter_interval[1].ToInt32());
                    }

                    var broken_interval = getNextString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); ;
                    if (broken_interval.Length > 0)
                    {
                        lvData.broken_interval.set(broken_interval[0].ToInt32(), broken_interval[1].ToInt32());
                    }

                    var rain_interval = getNextString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); ;
                    if (rain_interval.Length > 0)
                    {
                        lvData.rain_interval.set(rain_interval[0].ToInt32(), rain_interval[1].ToInt32());
                    }

                    string requirementStr = getNextString();
                    lvData.parseRequirement(requirementStr);

                    string[] organics = getNextString().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (organics.Length > 0)
                    {
                        lvData.organicMaterials = new List<string>(organics);
                    }

                    int requireKitchenware = getNextInt32();

                    string[] unlockItems = getNextString().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (unlockItems.Length > 0)
                    {
                        foreach (string itemId in unlockItems)
                        {
                            lvData.unlock_items.Add(itemId.ToInt32());
                        }
                    }

                    lvData.parseRewards(getNextString());

                    levels.Add(lvData);
                }
                reader.Close();
            }
            sqlCnn.Close();
            sqlCnn.Dispose();

            SetMapLevelDatas(mapKey, levels);

            return true;
        }

        private List<CustomerOrder> ParseOrders(string orderStr)
        {
            List<CustomerOrder> orderList = new List<CustomerOrder>();
            var orderElements = orderStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries); ;
            foreach (string element in orderElements)
            {
                //(Fried sausage,cowgirl,1.0);
                if (element.Length > 0)
                {
                    string sourceStr = element.Substring(1, element.Length - 2);

                    string[] infos = sourceStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (infos.Length >= 3 && infos.Length <= 4)
                    {
                        var order = new CustomerOrder();
                        if (infos[0].Contains("&"))
                        {
                            int pos = infos[0].IndexOf('&');
                            order.foods.Add(infos[0].Substring(0, pos));
                            order.foods.Add(infos[0].Substring(pos + 1, infos[0].Length - pos - 1));
                        }
                        else
                        {
                            order.foods.Add(infos[0]);
                        }
                        order.customer = infos[1];
                        //MyDebug.WriteLine(sourceStr + " ==== " + infos[2]);
                        order.weight = infos[2].ToFloat();

                        if (infos.Length == 4)
                        {
                            order.latestFirstCome = infos[3].ToInt32();
                        }
                        orderList.Add(order);

                        MyDebug.Assert(CustomerDataManager.GetInstance().IsCustomerKey(order.customer), "Error customer key : " + order.customer + ", Order :" + element);
                        m_customers.Add(order.customer);
                        if(order.foods.Count == 1)
                        {
                            MyDebug.Assert(FoodDataManager.GetInstance().IsFoodKey(order.foods[0]), "Error food key : " + order.foods[0] + ", Order :" + element);
                            m_foods.Add(order.foods[0]);
                        }else{
                            string combineFood = string.Format("{0}&{1}", order.foods[0], order.foods[1]);
                            MyDebug.Assert(FoodDataManager.GetInstance().IsFoodKey(combineFood), "Error food key : " + combineFood + ", Order :" + element);
                            m_foods.Add(combineFood);
                        }
                    }
                    else
                    {
                        MyDebug.Assert(false, element);
                    }
                }
            }
            return orderList;
        }

        private List<CustomerOrder> ParseSpecialOrders(string orderStr)
        {
            List<CustomerOrder> orderList = new List<CustomerOrder>();
            var orderElements = orderStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries); ;
            foreach (string element in orderElements)
            {
                //([7,8],Fried bread,ents);
                if (element.Length > 0)
                {
                    // 去掉括号
                    string sourceStr = element.Substring(2, element.Length - 3);

                    var order = new CustomerOrder();

                    int index = sourceStr.IndexOf(']');
                    string intervalStr = sourceStr.Substring(0, index);
                    string[] interval = intervalStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    MyDebug.Assert(interval.Length == 2, "");
                    order.interval.min = interval[0].ToInt32();
                    order.interval.max = interval[1].ToInt32();

                    int lastIdx = sourceStr.LastIndexOf(',');
                    string foodName = sourceStr.Substring(index + 2, lastIdx - index - 2);
                    if (foodName.Contains("&"))
                    {
                        int pos = foodName.IndexOf('&');
                        order.foods.Add(foodName.Substring(0, pos));
                        order.foods.Add(foodName.Substring(pos + 1, foodName.Length - pos - 1));
                    }
                    else
                    {
                        order.foods.Add(foodName);
                    }

                    string customerName = sourceStr.Substring(lastIdx + 1, sourceStr.Length - lastIdx - 1);
                    order.customer = customerName;

                    orderList.Add(order);

                    MyDebug.Assert(CustomerDataManager.GetInstance().IsCustomerKey(order.customer), "Error customer key : " + order.customer + ", Order :" + element);
                    m_customers.Add(order.customer);
                    if (order.foods.Count == 1)
                    {
                        MyDebug.Assert(FoodDataManager.GetInstance().IsFoodKey(order.foods[0]), "Error food key : " + order.foods[0] + ", Order :" + element);
                        m_foods.Add(order.foods[0]);
                    }
                    else
                    {
                        string combineFood = string.Format("{0}&{1}", order.foods[0], order.foods[1]);
                        MyDebug.Assert(FoodDataManager.GetInstance().IsFoodKey(combineFood), "Error food key : " + combineFood + ", Order :" + element);
                        m_foods.Add(combineFood);
                    }
                }
            }
            return orderList;
        }

        private List<CustomerOrder> ParseAnyfoodOrders(string orderStr)
        {
            List<CustomerOrder> orderList = new List<CustomerOrder>();

            var orderElements = orderStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries); ;
            foreach (string element in orderElements)
            {
                //([9,9],more3,mesmerizer)
                if (element.Length > 0)
                {
                    //Console.WriteLine(element);
                    // 去掉括号
                    string sourceStr = element.Substring(2, element.Length - 3);

                    var order = new CustomerOrder();

                    int index = sourceStr.IndexOf(']');
                    string intervalStr = sourceStr.Substring(0, index);
                    string[] interval = intervalStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    MyDebug.Assert(interval.Length == 2, "");
                    order.interval.min = interval[0].ToInt32();
                    order.interval.max = interval[1].ToInt32();

                    int lastIdx = sourceStr.LastIndexOf(',');
                    order.randomFoodRule = sourceStr.Substring(index + 2, lastIdx - index - 2);

                    string customerName = sourceStr.Substring(lastIdx + 1, sourceStr.Length - lastIdx - 1);
                    order.customer = customerName;

                    orderList.Add(order);

                    MyDebug.Assert(CustomerDataManager.GetInstance().IsCustomerKey(order.customer), "Error customer key : " + order.customer + ", Order :" + element);
                    m_customers.Add(order.customer);
                }
            }
            return orderList;
        }

        public string exportOrders(List<CustomerOrder> orders)
        {
            StringBuilder orderStr = new StringBuilder();
            foreach(CustomerOrder ord in orders)
            {
                string foodKey = ord.foods.Count > 1 ? string.Format("{0}&{1}",ord.foods[0], ord.foods[1]) : ord.foods[0];
                orderStr.Append("(").Append(foodKey).Append(",").Append(ord.customer).Append(",").Append(ord.weight);
                if(ord.latestFirstCome > 0)
                {
                    orderStr.Append(",").Append(ord.latestFirstCome);
                }
                orderStr.Append(");");
            }
            return orders.ToString();
        }

        public string exportSpecialOrders(List<CustomerOrder> orders)
        {
            StringBuilder orderStr = new StringBuilder();
            foreach (CustomerOrder ord in orders)
            {
                string foodKey = ord.foods.Count > 1 ? string.Format("{0}&{1}", ord.foods[0], ord.foods[1]) : ord.foods[0];
                orderStr.Append("([").Append(ord.interval.min).Append(",").Append(ord.interval.max).Append("],").Append(foodKey).Append(",").Append(ord.customer).Append(");");
            }
            return orders.ToString();
        }

        public string exportRandomfoodOrders(List<CustomerOrder> orders)
        {
            StringBuilder orderStr = new StringBuilder();
            foreach (CustomerOrder ord in orders)
            {
                orderStr.Append("([").Append(ord.interval.min).Append(",").Append(ord.interval.max).Append("],").Append(ord.randomFoodRule).Append(",").Append(ord.customer).Append(");");
            }
            return orders.ToString();
        }

        public string GetJsonString(string restaurant)
        {
            string ret = null;
            List<LevelData> levels;
            if (m_levelDataDict.TryGetValue(restaurant, out levels))
            {
                ret = JsonConvert.SerializeObject(levels, Formatting.Indented);
            }

            return ret;
        }

        public void SetMapLevelDatas(string restaurant, List<LevelData> levels)
        {
            if (m_levelDataDict.ContainsKey(restaurant))
            {
                m_levelDataDict.Remove(restaurant);
            }
            if (m_levelIndexes.ContainsKey(restaurant))
            {
                m_levelIndexes.Remove(restaurant);
            }

            m_levelDataDict.Add(restaurant, levels);
            Dictionary<int, int> indexDict = new Dictionary<int, int>();
            for (int i = 0; i < levels.Count; ++i)
            {
                LevelData lvd = levels[i];
                indexDict.Add(lvd.id, i);
            }
            m_levelIndexes.Add(restaurant, indexDict);
        }

        public void saveLevelData(LevelData lvData)
        {
            SQLiteConnection cnn = new SQLiteConnection("data source = " + m_dbPath);
            cnn.Open();

            var cmd = cnn.CreateCommand();
            cmd.CommandText = string.Format("SELECT * FROM Level WHERE level={0}", lvData.id);
            var reader = cmd.ExecuteReader();

            string[] keys = "level,type,total,star_score,orders,special_orders,anyfood_orders,max_order,order_interval,first_arrival,waiting_time_decay,secret_customers,mucky_interval,broken_interval,rain_interval,requirement,organic_materials,unlock".Split(',');

            string updateStr = "";
            if(reader.Read())
            {// 已存在，修改数值
                StringBuilder cmdStr = new StringBuilder();
                cmdStr.Append("UPDATE Level SET ");

                var itor = keys.GetEnumerator();
                while(true)
                {
                    bool result = itor.MoveNext();
                    if(!result)
                    {
                        cmdStr.Remove(cmdStr.Length - 1, 1);
                        break;
                    }
                    string key = (string)itor.Current;
                    cmdStr.Append(key).Append("=@").Append(key).Append(",");
                }

                cmdStr.Append(" WHERE level=").Append(lvData.id);

                updateStr = cmdStr.ToString();
                MyDebug.WriteLine("Update cmdStr = " + updateStr);
            }
            else{
                StringBuilder cmdStr = new StringBuilder();
                cmdStr.Append("INSERT INTO Level(");

                var itor = keys.GetEnumerator();
                while (true)
                {
                    bool result = itor.MoveNext();
                    if (!result)
                    {
                        cmdStr.Remove(cmdStr.Length - 1, 1);
                        break;
                    }
                    string key = (string)itor.Current;
                    cmdStr.Append(key).Append(",");
                }

                cmdStr.Append(") VALUES(");

                itor = keys.GetEnumerator();
                while (true)
                {
                    bool result = itor.MoveNext();
                    if (!result)
                    {
                        cmdStr.Remove(cmdStr.Length - 1, 1);
                        break;
                    }
                    string key = (string)itor.Current;
                    cmdStr.Append("@").Append(key).Append(",");
                }

                cmdStr.Append(")");

                updateStr = cmdStr.ToString();
                MyDebug.WriteLine("Add cmdStr = " + updateStr);
                //updateStr = "INSERT INTO Level(customer_id, customer_key, Name_CN, food_id, food_key, foodname_cn, wait_time, tips, consider_time) VALUES(@customer_id, @customer_key, @Name_CN, @food_id, @food_key, @foodname_cn, @wait_time, @tips, @consider_time)"
            }
            reader.Close();
            cmd.Dispose();

            cmd = cnn.CreateCommand();
            cmd.CommandText = updateStr;
            //"level,type,total,star_score,orders,special_orders,anyfood_orders,max_order,order_interval,first_arrival,waiting_time_decay," +
            //"secret_customers,mucky_interval,broken_interval,rain_interval,requirement,organic_materials,required_kitchenware,unlock,rewards"
            cmd.Parameters.Add("level", System.Data.DbType.Int32).Value = lvData.id;
            cmd.Parameters.Add("type", System.Data.DbType.String).Value = (lvData.type == LevelType.FIXED_CUSTOMER ? "FIXED_CUSTOMER" : (lvData.type == LevelType.FIXED_TIME ? "FIXED_TIME" : "LOST_CUSTOMER"));
            cmd.Parameters.Add("total", System.Data.DbType.Int32).Value = lvData.total;
            cmd.Parameters.Add("star_score", System.Data.DbType.String).Value = ConvertList.List2String<string>(lvData.scoreList, ';');
            cmd.Parameters.Add("orders", System.Data.DbType.String).Value = exportOrders(lvData.orders);
            cmd.Parameters.Add("special_orders", System.Data.DbType.String).Value = exportOrders(lvData.specialOrders);
            cmd.Parameters.Add("anyfood_orders", System.Data.DbType.String).Value = exportOrders(lvData.anyfoodOrders);
            cmd.Parameters.Add("max_order", System.Data.DbType.Int32).Value = lvData.max_order;
            cmd.Parameters.Add("order_interval", System.Data.DbType.String).Value = string.Format("{0},{1}", lvData.order_interval.min, lvData.order_interval.max);
            cmd.Parameters.Add("first_arrival", System.Data.DbType.String).Value = ConvertList.List2String<float>(lvData.first_arrivals, ',');
            cmd.Parameters.Add("waiting_time_decay", System.Data.DbType.String).Value = string.Format("{0},{1}", lvData.waiting_decay.interval, lvData.waiting_decay.rate);
            string secretStr = "";
            var it = lvData.secret_customers.GetEnumerator();
            bool hasNext = it.MoveNext();
            while (hasNext)
            {
                SecretCustomer secCus = it.Current;
                secretStr += secCus.customer;
                if (secCus.showOrder > 0)
                {
                    secretStr += string.Format("<{0}>", secCus.showOrder);
                }
                hasNext = it.MoveNext();
                if (hasNext)
                {
                    secretStr += ";";
                }
            }
            cmd.Parameters.Add("secret_customers", System.Data.DbType.String).Value = secretStr;
            cmd.Parameters.Add("mucky_interval", System.Data.DbType.String).Value = string.Format("{0},{1}", lvData.litter_interval.min, lvData.litter_interval.max);
            cmd.Parameters.Add("broken_interval", System.Data.DbType.String).Value = string.Format("{0},{1}", lvData.broken_interval.min, lvData.broken_interval.max);
            cmd.Parameters.Add("rain_interval", System.Data.DbType.String).Value = string.Format("{0},{1}", lvData.rain_interval.min, lvData.rain_interval.max);
            string requireStr = "";
            if (lvData.requirements.requiredCustomers.Count > 0)
            {
                foreach (var req in lvData.requirements.requiredCustomers)
                {
                    requireStr += string.Format("{0}*{1};", req.name, req.number);
                }
            }
            else if (lvData.requirements.requiredFoods.Count > 0)
            {
                foreach (var req in lvData.requirements.requiredFoods)
                {
                    requireStr += string.Format("{0}*{1};", req.name, req.number);
                }
            }
            else if (!lvData.requirements.allowBurn)
            {
                requireStr += "no_burn;";
            }
            else if (!lvData.requirements.allowLostCustomer)
            {
                requireStr += "no_lost;";
            }
            else if (lvData.requirements.smileCount > 0)
            {
                requireStr += "smile*" + lvData.requirements.smileCount + ";";
            }
            if (requireStr.Length > 0)
            {
                requireStr = requireStr.Substring(0, requireStr.Length - 1);
            }
            cmd.Parameters.Add("secret_customers", System.Data.DbType.String).Value = requireStr;
            cmd.Parameters.Add("organic_materials", System.Data.DbType.String).Value = ConvertList.List2String<string>(lvData.organicMaterials, ';');
            cmd.Parameters.Add("unlock", System.Data.DbType.Int32).Value = ConvertList.List2String<int>(lvData.unlock_items, ';');

            try{
                int updateRlt = cmd.ExecuteNonQuery();
                MyDebug.WriteLine("Update levelData result : ", updateRlt);
            }catch(Exception e)
            {
                MyDebug.WriteLine(e);
            }
        }


        public LevelData GetLevelData(string restaurant, int levelId)
        {
            LevelData lvd = null;

            Dictionary<int, int> indexDict;
            List<LevelData> levels;
            int index;
            if (m_levelDataDict.TryGetValue(restaurant, out levels)
                && m_levelIndexes.TryGetValue(restaurant, out indexDict)
                && indexDict.TryGetValue(levelId, out index))
            {
                lvd = levels[index];
            }
            return lvd;
        }

        public List<LevelData> GetAllLevels(string restaurant)
        {
            List<LevelData> ret = null;
            m_levelDataDict.TryGetValue(restaurant, out ret);
            return ret;
        }
    }
}
