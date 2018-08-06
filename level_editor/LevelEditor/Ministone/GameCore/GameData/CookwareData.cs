using System.Collections.Generic;
using Newtonsoft.Json;
using Ministone.GameCore.GameData.Generic;

namespace Ministone.GameCore.GameData
{
    public class CookwareData
    {
        int m_id;                                           // 厨具ID
        int m_level;                                        // 等级
        float m_workSpeed = 1.0f;                           // 烹饪速率
        float m_burnSpeed = 1.0f;                           // 烧焦速率
        int m_defaultFoodId;                                // 默认食物ID
        int m_makeCount = 1;                                // 产出的食物数量
        float m_baseWorkTime;                               // 基础工作时间
        string m_key;                                       // 键值
        Price m_price;                                      // 购买价格
        List<string> m_functions;                           // 功能类型列表
        List<string> m_specialEffects;                      // 特效列表
        List<int> m_scopeMap;                               // 厨具可以使用的地图
        Dictionary<string, string> m_displayName;           // 显示名称
        Dictionary<string, string> m_displayPosition;       // 显示位置
        string m_iconTexture;                               // 静态图片材质
        string m_animation;                                 // 动画资源

        public CookwareData()
        {
            m_functions = new List<string>();
            m_displayName = new Dictionary<string, string>();
            m_displayPosition = new Dictionary<string, string>();
            m_specialEffects = new List<string>();
            m_scopeMap = new List<int>();
            m_price = new Price();
        }

        public int id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public string key
        {
            get { return m_key; }
            set { m_key = value; }
        }

        public int level
        {
            get { return m_level; }
            set { m_level = value; }
        }

        public float work_speed
        {
            get { return m_workSpeed; }
            set { m_workSpeed = value; }
        }

        public float burn_speed
        {
            get { return m_burnSpeed; }
            set { m_burnSpeed = value; }
        }

        public int default_food_id
        {
            get { return m_defaultFoodId; }
            set { m_defaultFoodId = value; }
        }

        public int make_count
        {
            get { return m_makeCount; }
            set { m_makeCount = value; }
        }

        public float base_work_time
        {
            get { return m_baseWorkTime; }
            set { m_baseWorkTime = value; }
        }

        public Price price
        {
            get { return m_price; }
            set { m_price = value; }
        }

        public List<string> functions
        {
            get { return m_functions; }
            set { m_functions = value; }
        }

        public List<string> special_effects
        {
            get { return m_specialEffects; }
            set { m_specialEffects = value; }
        }

        public Dictionary<string, string> display_name
        {
            get { return m_displayName; }
            set { m_displayName = value; }
        }

        public Dictionary<string, string> display_position
        {
            get { return m_displayPosition; }
            set { m_displayPosition = value; }
        }

        public List<int> scopeMap
        {
            get { return m_scopeMap; }
            set { m_scopeMap = value; }
        }

        public string GetdDisplayName(string lang = "en")
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

        public string icon_texture
        {
            get { return m_iconTexture; }
            set { m_iconTexture = value; }
        }

        public string animation
        {
            get { return m_animation; }
            set { m_animation = value; }
        }
    }
}
