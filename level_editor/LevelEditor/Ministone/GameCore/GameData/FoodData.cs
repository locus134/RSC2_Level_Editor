using System.Collections.Generic;
using Newtonsoft.Json;
using Ministone.GameCore.GameData.Generic;

namespace Ministone.GameCore.GameData
{
    public class CookingStep
    {
        public bool canIgnore;
        public int id;
        public int price;
        public int cookwareType;
        public float cookTime;
        public string food;
        public List<string> ingredients;
    }

    public class FoodData
    {
        int m_foodId;                                 // 食物ID
        int m_suggestedPrice;                         // 建议价格,这是公式计算出来的价格
        int m_price;                                  // 食物价格
        int m_cookwareType;                           // 最终烹饪所使用的厨具类型
        float m_cookTime;                             // 食物制作时间
        int m_shapeIndex;                             // 形状
        float m_burnTime;                             // 食物烧焦时间
        string m_foodKey;                             // 食物名称键值
        string m_texture;                             // 食物的显示纹理
        Dictionary<string, string> m_displayName;     // 食物的显示名称
        List<string> m_finalIngredients;              // 最终烹饪所需要的食材/食物
        HashSet<string> m_allIngredientSet;           // 需要的所有食材集合
        List<CookingStep> m_cookingStepList;          // 制作步骤

        public FoodData()
        {
            m_displayName = new Dictionary<string, string>();
            m_finalIngredients = new List<string>();
            m_allIngredientSet = new HashSet<string>();
            m_cookingStepList = new List<CookingStep>();
            m_cookwareType = 0;
            m_shapeIndex = -1;
        }

        public int id
        {
            get { return m_foodId; }
            set { m_foodId = value; }
        }

        public string key
        {
            get { return m_foodKey; }
            set { m_foodKey = value; }
        }

        public int suggested_price
        {
            get { return m_suggestedPrice; }
            set { m_suggestedPrice = value; }
        }

        public int price
        {
            get { return m_price; }
            set { m_price = value; }
        }

        public int cookware_type
        {
            get { return m_cookwareType; }
            set { m_cookwareType = value; }
        }

        public int shapeIndex
        {
            get { return m_shapeIndex; }
            set { m_shapeIndex = value; }
        }

        public float cook_time
        {
            get { return m_cookTime; }
            set { m_cookTime = value; }
        }

        public float burn_time
        {
            get { return m_burnTime; }
            set { m_burnTime = value; }
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

        public List<string> ingredients
        {
            get { return m_finalIngredients; }
            set { m_finalIngredients = value; }
        }

        public List<CookingStep> cooking_step_list
        {
            get { return m_cookingStepList; }
            set { m_cookingStepList = value; }
        }

        public string texture
        {
            get { return m_texture; }
            set { m_texture = value; }
        }

        public HashSet<string> ingredient_set
        {
            get { return m_allIngredientSet; }
            set { m_allIngredientSet = value; }
        }
    }


}
