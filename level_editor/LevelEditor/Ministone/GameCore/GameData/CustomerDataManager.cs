using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Ministone.GameCore.GameData.Generic;
using System.Data.SQLite;

namespace Ministone.GameCore.GameData
{
    public class CustomerDataManager
    {
        static CustomerDataManager _instance = null;
        List<CustomerData> m_customerList;
        KeyIndexMap<int> m_indexMap;

        SQLiteConnection m_cnn;

        private CustomerDataManager()
        {
            m_customerList = new List<CustomerData>();
            m_indexMap = new KeyIndexMap<int>();
            m_cnn = null;
        }

        public static CustomerDataManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CustomerDataManager();
            }
            return _instance;
        }

        public bool LoadFromDBFile(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                return false;
            }

            m_cnn = new SQLiteConnection();
            m_cnn.ConnectionString = string.Format("Data Source={0};Version = 3", dbPath);
            m_cnn.Open();

            var cmd = m_cnn.CreateCommand();
            cmd.CommandText = "Select customer_id,customer_key,displayname_cn,isLeftGarbage,type,sourceDir,icon_file FROM customerInfo";

            var reader = cmd.ExecuteReader();
            if(reader == null)
            {
                MyDebug.WriteLine("Error loading customer data from DBFile!");
                return false;
            }
            m_customerList.Clear();
            m_indexMap.Clear();
            while(reader.Read())
            {
                int column = 0;
                var cusData = new CustomerData();
                cusData.id = reader.GetInt32Safe(column++);
                cusData.key = reader.GetStringSafe(column++);
                cusData.display_name.Add("cn", reader.GetStringSafe(column++));
                cusData.can_litter = reader.GetInt32Safe(column++) > 0;
                cusData.type = reader.GetStringSafe(column++);
                string sourceDir = reader.GetStringSafe(column++);
                string iconName = reader.GetStringSafe(column++);
                cusData.icon_texture = sourceDir + iconName;

                m_customerList.Add(cusData);
                m_indexMap.AddValue(cusData.key, cusData.id, m_customerList.Count - 1);
            }


            //List<CustomerData> list = JsonConvert.DeserializeObject<List<CustomerData>>(jsonStr);
            //if (list == null)
            //{
            //    Debug.WriteLine("Error loading customer data from JSON!");
            //    return false;
            //}

            //m_customerList = list;
            //m_indexMap.Clear();
            //for (int i = 0; i < m_customerList.Count; ++ i)
            //{
            //    CustomerData cd = m_customerList[i];
            //    m_indexMap.AddValue(cd.key, cd.id, i);
            //}

            return true;
        }

        public string GetJsonString()
        {
            return JsonConvert.SerializeObject(m_customerList, Formatting.Indented);
        }

        public CustomerData GetCustomer(string key)
        {
            int i;
            if (m_indexMap.GetValue(key, out i))
            {
                return m_customerList[i];
            }
            return null;
        }

        public CustomerData GetCustomer(int id)
        {
            int i;
            if (m_indexMap.GetValue(id, out i))
            {
                return m_customerList[i];
            }
            return null;
        }

        public void AddCustomer(CustomerData cd)
        {
            int i;
            if (!m_indexMap.GetValue(cd.key, out i))
            {
                m_customerList.Add(cd);
                i = m_customerList.Count - 1;
                m_indexMap.AddValue(cd.key, cd.id, i);
            }
        }

        public void RemoveCustomer(string key)
        {
            int i;
            if (m_indexMap.GetValue(key, out i))
            {
                CustomerData cd = m_customerList[i];
                m_indexMap.RemoveValue(cd.key, cd.id);
                m_customerList.RemoveAt(i);
            }
        }

        public void RemoveCustomer(int id)
        {
            int i;
            if (m_indexMap.GetValue(id, out i))
            {
                CustomerData cd = m_customerList[i];
                m_indexMap.RemoveValue(cd.key, cd.id);
                m_customerList.RemoveAt(i);
            }
        }

        public bool IsCustomerKey(string key)
        {
            return m_indexMap.IsExist(key);
        }

        public bool IsCustomerId(int id)
        {
            return m_indexMap.IsExist(id);
        }
    }
}
