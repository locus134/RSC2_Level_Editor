using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Ministone.GameCore.GameData.Generic;
using System.Data.SQLite;

namespace Ministone.GameCore.GameData
{
    public class IngredientDataManager
    {
        static IngredientDataManager _instance = null;
        List<IngredientData> m_ingredientList;
        KeyIndexMap<int> m_indexMap;
        SQLiteConnection m_cnn;

        private IngredientDataManager()
        {
            m_ingredientList = new List<IngredientData>();
            m_indexMap = new KeyIndexMap<int>();
            m_cnn = null;
        }

        public static IngredientDataManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new IngredientDataManager();
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
            cmd.CommandText = "Select * FROM material";

            var reader = cmd.ExecuteReader();
            if (reader == null)
            {
                MyDebug.WriteLine("Error loading Material data from DBFile!");
                return false;
            }
            m_ingredientList.Clear();
            m_indexMap.Clear();
            while (reader.Read())
            {
                int column = 0;
                var matData = new IngredientData();
                matData.id = reader.GetInt32Safe(column++);
                matData.key = reader.GetStringSafe(column++);
                matData.food_id = reader.GetInt32Safe(column++);
                matData.price = reader.GetInt32Safe(column++);
                matData.texture = reader.GetStringSafe(column++);

                string colName = reader.GetName(column);
                if(colName.Contains("name_"))
                {
                    string location = colName.Substring(5);
                    matData.display_name.Add(location, reader.GetStringSafe(column));
                }
                else if(colName.Contains("position"))
                {
                    //int mapId = int.Parse(colName.Substring(8));
                    matData.display_position.Add(colName.Substring(8), reader.GetStringSafe(column));
                }
                column++;

                m_ingredientList.Add(matData);
                m_indexMap.AddValue(matData.key, matData.id, m_ingredientList.Count - 1);
            }

            //var list = JsonConvert.DeserializeObject<List<IngredientData>>(jsonStr);
            //if (list == null)
            //{
            //    Debug.WriteLine("Error loading ingredients from JSON!");
            //    return false;
            //}

            //m_ingredientList = list;
            //m_indexMap.Clear();
            //for (int i = 0; i < m_ingredientList.Count; ++ i)
            //{
            //    IngredientData ing = m_ingredientList[i];
            //    m_indexMap.AddValue(ing.key, ing.id, i);
            //}

            return true;
        }

        public string GetJsonString()
        {
            return JsonConvert.SerializeObject(m_ingredientList, Formatting.Indented);
        }

        public IngredientData GetIngredient(string key)
        {
            int i;
            if (m_indexMap.GetValue(key, out i))
            {
                return m_ingredientList[i];
            }
            return null;
        }

        public IngredientData GetIngredient(int id)
        {
            int i;
            if (m_indexMap.GetValue(id, out i))
            {
                return m_ingredientList[i];
            }
            return null;
        }

        public void AddIngredient(IngredientData ing)
        {
            int i;
            if (!m_indexMap.GetValue(ing.key, out i))
            {
                m_ingredientList.Add(ing);
                i = m_ingredientList.Count - 1;
                m_indexMap.AddValue(ing.key, ing.id, i);
            }
        }

        public void RemoveIngredient(string key)
        {
            int i;
            if (m_indexMap.GetValue(key, out i))
            {
                IngredientData ing = m_ingredientList[i];
                m_indexMap.RemoveValue(key, ing.id);
                m_ingredientList.RemoveAt(i);
            }
        }

        public void RemoveIngredient(int id)
        {
            int i;
            if (m_indexMap.GetValue(id, out i))
            {
                IngredientData ing = m_ingredientList[i];
                m_indexMap.RemoveValue(ing.key, id);
                m_ingredientList.RemoveAt(i);
            }
        }

        public bool IsIngredientKey(string key)
        {
            return m_indexMap.IsExist(key);
        }

        public bool IsIngredientId(int id)
        {
            return m_indexMap.IsExist(id);
        }
    }
}
