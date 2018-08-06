using System.Collections.Generic;
using Ministone.GameCore.GameData.Generic;

namespace Ministone.GameCore.GameData
{
    public class IngredientData
    {
        int m_id;                                       // 食材ID
        int m_foodId;                                   // 对应的食物ID(如果存在的话)
        int m_price;                                    // 价格
        string m_key;                                   // 食材键值
        string m_texture;                               // 显示纹理名称
        Dictionary<string, string> m_displayPosition;   // 显示位置
        Dictionary<string, string> m_displayName;       // 食材名称（多国语言）

        public IngredientData()
        {
            m_displayPosition = new Dictionary<string, string>();
            m_displayName = new Dictionary<string, string>();
        }

        public int id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public int food_id
        {
            get { return m_foodId; }
            set { m_foodId = value; }
        }

        public int price
        {
            get { return m_price; }
            set { m_price = value; }
        }

        public string key
        {
            get { return m_key; }
            set { m_key = value; }
        }

        public string texture
        {
            get { return m_texture; }
            set { m_texture = value; }
        }

        public Dictionary<string, string> display_position
        {
            get { return m_displayPosition; }
            set { m_displayPosition = value; }
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

        public string GetDisplayPosition(string restaurant_key)
        {
            string pos;
            m_displayPosition.TryGetValue(restaurant_key, out pos);
            return pos;
        }
    }
}
