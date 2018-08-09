using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Ministone.GameCore.GameData.Generic;
using System.Data.SQLite;

namespace Ministone.GameCore.GameData
{
    public class CookwareDataManager
    {
        static CookwareDataManager _instance = null;
        List<CookwareData> m_cookwareList;
        KeyIndexMap<int> m_indexMap;

        string m_dbpath;

        private CookwareDataManager()
        {
            m_cookwareList = new List<CookwareData>();
            m_indexMap = new KeyIndexMap<int>();
        }

        public static CookwareDataManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CookwareDataManager();
            }
            return _instance;
        }

        public bool LoadFromDBFile(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                return false;
            }
            m_dbpath = dbPath;

            var cwCnn = new SQLiteConnection();
            cwCnn.ConnectionString = string.Format("Data Source={0};Version=3", dbPath);
            cwCnn.Open();

            string sql_select = "SELECT CookingWareID,KeyName,lv,type,specialEffects,name_cn,Coin,Cash,BaseWorkTime,WorkSpeed,BurnSpeed,Makecount,ScopeMap,DefaultFoodID,ArmaturePath,Thumbnails FROM cookingware";
            SQLiteCommand cmd = cwCnn.CreateCommand();
            cmd.CommandText = sql_select;
            var reader = cmd.ExecuteReader();
            if(reader == null)
            {
                MyDebug.WriteLine("Error reading CW Data from DBFile");
                return false;
            }

            m_cookwareList.Clear();
            m_indexMap.Clear();
            while(reader.Read())
            {
                var cwData = new CookwareData();
                int column = 0;
                cwData.id = reader.GetInt32Safe(column++);
                cwData.key = reader.GetStringSafe(column++);
                cwData.level = reader.GetInt32Safe(column++);
                cwData.functions.Add(reader.GetStringSafe(column++));
                cwData.special_effects.Add(reader.GetStringSafe(column++));
                cwData.display_name.Add("cn", reader.GetStringSafe(column++));
                cwData.price.coin = reader.GetInt32Safe(column++);
                cwData.price.gem = reader.GetInt32Safe(column++);
                cwData.base_work_time = reader.GetFloatSafe(column++);
                cwData.work_speed = reader.GetInt32Safe(column++) * 0.01f;
                cwData.burn_speed = reader.GetInt32Safe(column++) * 0.01f;
                cwData.make_count = reader.GetInt32Safe(column++);

                string scopeMapStr = reader.GetStringSafe(column++);
                if(!string.IsNullOrEmpty(scopeMapStr))
                {
                    if (scopeMapStr.Contains(","))
                    {
                        var mapList = scopeMapStr.Split(',');
                        foreach (string map in mapList)
                        {
                            int mapId = int.Parse(map);
                            cwData.scopeMap.Add(mapId);
                        }

                    }
                    else if (scopeMapStr.Contains("-"))
                    {
                        int spIdx = scopeMapStr.IndexOf('-');
                        int start = int.Parse(scopeMapStr.Substring(0, spIdx));
                        int end = int.Parse(scopeMapStr.Substring(spIdx + 1, scopeMapStr.Length - spIdx - 1));

                        for (int i = start; i <= end; i++)
                        {
                            cwData.scopeMap.Add(i);
                        }
                    }
                }

                cwData.default_food_id = reader.GetInt32Safe(column++);
                cwData.animation = reader.GetStringSafe(column++);
                cwData.icon_texture = reader.GetStringSafe(column++);

                m_cookwareList.Add(cwData);
                m_indexMap.AddValue(cwData.key, cwData.id, m_cookwareList.Count - 1);
            }
            reader.Close();
            cmd.Dispose();
            cwCnn.Close();
            cwCnn.Dispose();
            
            return true;
        }

        public string GetJsonString()
        {
            return JsonConvert.SerializeObject(m_cookwareList, Formatting.Indented);
        }

        public CookwareData GetCookware(string key)
        {
            int i;
            if (m_indexMap.GetValue(key, out i))
            {
                return m_cookwareList[i];
            }
            return null;
        }

        public CookwareData GetCookware(int id)
        {
            int i;
            if (m_indexMap.GetValue(id, out i))
            {
                return m_cookwareList[i];
            }
            return null;
        }

        public void AddCookware(CookwareData cw)
        {
            int i;
            if (!m_indexMap.GetValue(cw.key, out i))
            {
                m_cookwareList.Add(cw);
                i = m_cookwareList.Count - 1;
                m_indexMap.AddValue(cw.key, cw.id, i);
            }
        }

        public void RemoveCookware(string key)
        {
            int i;
            if (m_indexMap.GetValue(key, out i))
            {
                CookwareData cd = m_cookwareList[i];
                m_indexMap.RemoveValue(key, cd.id);
                m_cookwareList.RemoveAt(i);
            }
        }

        public void RemoveCookware(int id)
        {
            int i;
            if (m_indexMap.GetValue(id, out i))
            {
                CookwareData cd = m_cookwareList[i];
                m_indexMap.RemoveValue(cd.key, id);
                m_cookwareList.RemoveAt(i);
            }
        }
    }
}
