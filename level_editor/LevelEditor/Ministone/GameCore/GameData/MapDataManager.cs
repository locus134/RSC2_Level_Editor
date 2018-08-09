using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using Newtonsoft.Json;
using Ministone.GameCore.GameData.Generic;

namespace Ministone.GameCore.GameData
{
    public class MapDataManager
    {
        static MapDataManager _instance = null;
        List<MapData> m_mapDataList;
        KeyIndexMap<int> m_indexMap;

        string m_dbPath;

        private MapDataManager()
        {
            m_mapDataList = new List<MapData>();
            m_indexMap = new KeyIndexMap<int>();
        }

        static public MapDataManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MapDataManager();
            }
            return _instance;
        }

        public bool LoadFromDBFile(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                return false;
            }
            m_dbPath = dbPath;
            var sqlCnn = new SQLiteConnection("data source=" + dbPath);
            sqlCnn.Open();

            var cmd = sqlCnn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Map";
            var reader = cmd.ExecuteReader();
            if(reader == null)
            {
                MyDebug.WriteLine("Failed to load map data from Map");
                return false;
            }
            m_mapDataList.Clear();
            m_indexMap.Clear();
            while(reader.Read())
            {
                int column = 0;
                var mapData = new MapData();
                mapData.id = reader.GetInt32Safe(column++);
                mapData.key = reader.GetStringSafe(column++);
                mapData.display_name.Add("cn", reader.GetStringSafe(column++));

                string containLv = reader.GetStringSafe(column++);
                int spIdx = containLv.IndexOf(',');
                mapData.start_level = containLv.Substring(0, spIdx).ToInt32();
                mapData.end_level = containLv.Substring(spIdx + 1).ToInt32();
                mapData.level_count = mapData.end_level - mapData.start_level  + 1;

                //cloumnName = reader.GetName(column);
                object value = reader.GetValue(column++);
                if(value != null)
                {
                    string foods = value.ToString();
                    if (!string.IsNullOrEmpty(foods))
                    {
                        mapData.food_list = new List<string>(foods.Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
                    } 
                }

                value = reader.GetValue(column++);
                if (value != null)
                {
                    string customers = value.ToString();
                    if (!string.IsNullOrEmpty(customers))
                    {
                        mapData.customer_list = new List<string>(customers.Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
                    }
                }

                value = reader.GetValue(column++);
                if (value != null)
                {
                    string spCustomers = value.ToString();
                    if (!string.IsNullOrEmpty(spCustomers))
                    {
                        mapData.special_customer_list = new List<string>(spCustomers.Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries));
                    } 
                }

                m_mapDataList.Add(mapData);
                m_indexMap.AddValue(mapData.key, mapData.id, m_mapDataList.Count - 1);
            }

            reader.Close();
            sqlCnn.Close();

            return true;
        }

        public string GetJsonString()
        {
            return JsonConvert.SerializeObject(m_mapDataList, Formatting.Indented);
        }

        public MapData GetMapData(string key)
        {
            int index;
            if (m_indexMap.GetValue(key, out index))
            {
                return m_mapDataList[index];
            }
            return null;
        }

        public List<MapData> GetAllMaps()
        {
            return m_mapDataList;
        }

        public MapData GetMapData(int id)
        {
            int index;
            if (m_indexMap.GetValue(id, out index))
            {
                return m_mapDataList[index];
            }
            return null;
        }

        public void AddMapData(MapData rest)
        {
            int i;
            if (!m_indexMap.GetValue(rest.key, out i))
            {
                m_mapDataList.Add(rest);
                i = m_mapDataList.Count - 1;
                m_indexMap.AddValue(rest.key, rest.id, i);
            }
        }

        public void setMapData(MapData rest, int index)
        {
            m_mapDataList.RemoveAt(index);
            m_mapDataList.Insert(index, rest);
        }

        public void RemoveMapData(string key)
        {
            int i;
            if (m_indexMap.GetValue(key, out i))
            {
                MapData rd = m_mapDataList[i];
                m_indexMap.RemoveValue(key, rd.id);
                m_mapDataList.RemoveAt(i);
            }
        }

        public void RemoveMapData(int id)
        {
            int i;
            if (m_indexMap.GetValue(id, out i))
            {
                MapData rd = m_mapDataList[i];
                m_indexMap.RemoveValue(rd.key, id);
                m_mapDataList.RemoveAt(i);
            }
        }

        public bool IsSpecialCustomer(int restId, string customer)
        {
            MapData rd = GetMapData(restId);
            if (rd == null)
            {
                return false;
            }
            return rd.IsSpecialCustomer(customer);
        }

        public bool IsSpecialCustomer(string restKey, string customer)
        {
            MapData rd = GetMapData(restKey);
            if (rd == null)
            {
                return false;
            }
            return rd.IsSpecialCustomer(customer);
        }
    }
}
