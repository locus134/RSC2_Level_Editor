using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Ministone.GameCore.GameData.Generic;
using System.Data.SQLite;

namespace Ministone.GameCore.GameData
{
    public class FoodDataManager
    {
        static FoodDataManager _instance = null;

        string m_dbPath;

        List<FoodData> m_foodList;
        KeyIndexMap<int> m_indexMap;
        Dictionary<string, int> m_foodPriceDict;

        IngredientDataManager _ingMgr = IngredientDataManager.GetInstance();
        CookwareDataManager _cwMgr = CookwareDataManager.GetInstance(); 

        private FoodDataManager()
        {
            m_foodList = new List<FoodData>();
            m_indexMap = new KeyIndexMap<int>();
            m_foodPriceDict = new Dictionary<string, int>();
        }

        static public FoodDataManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FoodDataManager();
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

            var sqlCnn = new SQLiteConnection();
            string cnnStr = string.Format("Data Source={0};Version=3", dbPath);
            sqlCnn.ConnectionString = cnnStr;
            sqlCnn.Open();

            string sql_select = "SELECT FoodID,KeyName,name_cn,TexturePath,FoodPrice,MakeTime,BurnTime,CookingwareType,Materiallist FROM food";
            SQLiteCommand cmd = sqlCnn.CreateCommand();
            cmd.CommandText = sql_select;
            var reader = cmd.ExecuteReader();
            if(reader == null)
            {
                MyDebug.WriteLine("Error reading Food Data from DBFile");
                return false;
            }
            m_indexMap.Clear();
            m_foodList.Clear();
            while(reader.Read())
            {
                int column = 0;
                var foodData = new FoodData();
                foodData.id = reader.GetInt32Safe(column++);
                foodData.key = reader.GetStringSafe(column++);
                foodData.display_name.Add("cn", reader.GetStringSafe(column++));
                foodData.texture = reader.GetStringSafe(column++);
                foodData.price = reader.GetInt32Safe(column++);
                foodData.cook_time = reader.GetFloatSafe(column++);
                foodData.burn_time = reader.GetFloatSafe(column++);
                string cwTypeStr = reader.GetStringSafe(column++);
                if(cwTypeStr.Contains(":"))
                {
                    int pos = cwTypeStr.IndexOf(':');
                    foodData.cookware_type = cwTypeStr.Substring(0, pos).ToInt32();
                    foodData.shapeIndex = cwTypeStr.Substring(pos + 1, cwTypeStr.Length - pos - 1).ToInt32();
                }else{
                    if(cwTypeStr.Length > 0)
                    {
                        foodData.cookware_type = cwTypeStr.ToInt32(); 
                    }
                }
                string ingredients = reader.GetStringSafe(column++);
                if(ingredients.Length > 0)
                {
                    foodData.ingredients = new List<string>(ingredients.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)); 
                }

                m_foodList.Add(foodData);
                m_indexMap.AddValue(foodData.key, foodData.id, m_foodList.Count - 1);
            }

            reader.Close();
            cmd.Dispose();
            sqlCnn.Close();
            sqlCnn.Dispose();

            for (int i = 0; i < m_foodList.Count; ++i)
            {
                FoodData fd = m_foodList[i];
                getCookingStep(fd.key, ref fd);
                fd.suggested_price = calcFoodPrice(fd.key);
            }

            return true;
        }

        public string GetJsonString()
        {
            return JsonConvert.SerializeObject(m_foodList, Formatting.Indented);
        }

        private void getCookingStep(string food, ref FoodData destFood)
        {
            List<string> ingreidents = new List<string>();
            FoodData fd = GetFood(food);
            MyDebug.Assert(fd != null, "Food not found: " + food);
            foreach (string key in fd.ingredients)
            {
                FoodData keyFood = GetFood(key);
                if (keyFood != null && !IngredientDataManager.GetInstance().IsIngredientKey(key))
                {
                    getCookingStep(key, ref destFood);
                }
                else
                {
                    System.Diagnostics.Debug.Assert(_ingMgr.GetIngredient(key) != null, "Ingredient not found for: " + key);
                    destFood.ingredient_set.Add(key);
                }
                ingreidents.Add(key);
            }

            CookingStep cs = new CookingStep();
            cs.canIgnore = false;
            cs.id = fd.id;
            cs.food = fd.key;
            cs.price = fd.price;
            cs.cookwareType = fd.cookware_type;
            cs.cookTime = fd.cook_time;
            cs.ingredients = ingreidents;

            destFood.cooking_step_list.Add(cs);
        }

        private int calcFoodPrice(string key)
        {
            int price = 0;
            if (!m_foodPriceDict.TryGetValue(key, out price))
            {
                FoodData fd = GetFood(key);
                if (fd != null)
                {
                    float priceDecay = 1.0f;
                    var ingList = fd.ingredients;
                    if (ingList.Count == 0)
                    {
                        price += fd.price;
                    }
                    else
                    {
                        for (int i = 0; i < ingList.Count; ++i)
                        {
                            string ingKey = ingList[i];
                            IngredientData ingData = _ingMgr.GetIngredient(ingKey);
                            if (ingData == null)
                            {
                                FoodData foodData = GetFood(ingKey);
                                if (foodData != null)
                                {
                                    price += calcFoodPrice(foodData.key);
                                }
                            }
                            else
                            {
                                price += (int)(ingData.price * priceDecay);
                                priceDecay *= 0.6f;
                            }
                        }
                    }

                    CookwareData cw = _cwMgr.GetCookware(fd.cookware_type + 1);
                    if (cw != null)
                    {
                        // 需要厨具制作则根据制作时长再作调整
                        priceDecay = 1.2f;
                        float cookTime = fd.cook_time;
                        float baseTime = cw.base_work_time;
                        if (cookTime > baseTime * 1.8f)
                        {
                            priceDecay += 0.3f;
                        }
                        else if (cookTime > baseTime)
                        {
                            priceDecay += (cookTime - baseTime) * 0.375f / baseTime;
                        }
                        price = (int)(price * priceDecay);
                    }
                }

                m_foodPriceDict.Add(key, price);
            }

            return price;
        }

        public FoodData GetFood(string food)
        {
            int index;
            if (m_indexMap.GetValue(food, out index))
            {
                return m_foodList[index];
            }
            return null;
        }

        public FoodData GetFood(int id)
        {
            int index;
            if (m_indexMap.GetValue(id, out index))
            {
                return m_foodList[index];
            }
            return null;
        }

        public void AddFood(FoodData fd)
        {
            int i;
            if (!m_indexMap.GetValue(fd.key, out i))
            {
                m_foodList.Add(fd);
                i = m_foodList.Count - 1;
                m_indexMap.AddValue(fd.key, fd.id, i);
            }
        }

        public void RemoveFood(string food)
        {
            int index;
            if (m_indexMap.GetValue(food, out index))
            {
                FoodData fd = m_foodList[index];
                m_indexMap.RemoveValue(food, fd.id);
                m_foodList.RemoveAt(index);
            }
        }

        public void RemoveFood(int id)
        {
            int index;
            if (m_indexMap.GetValue(id, out index))
            {
                FoodData fd = m_foodList[index];
                m_indexMap.RemoveValue(fd.key, id);
                m_foodList.RemoveAt(index);
            }
        }

        public List<CookingStep> GetCookingSteps(string food)
        {
            List<CookingStep> steps = null;
            FoodData fd = GetFood(food);
            if (fd != null)
            {
                steps = fd.cooking_step_list;
            }
            return steps;
        }

        public bool IsFoodKey(string key)
        {
            return m_indexMap.IsExist(key);
        }

        public bool IsFoodId(int id)
        {
            return m_indexMap.IsExist(id);
        }
    }
}
