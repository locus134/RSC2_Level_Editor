using System.Collections.Generic;
using Ministone.GameCore.GameData.Generic;

namespace Ministone.GameCore.GameData
{
    public class CustomerData
    {
        int m_id;                                       // 顾客id
        bool m_canLitter;                               // 是否会留下垃圾
        string m_key;                                   // 顾客名字键值
        string m_type;                                  // 类型
        string m_iconTexture;                           // 图标材质
        string m_modelName;                             // 模型名称
        Dictionary<string, string> m_displayName;       // 顾客显示名字（多国语言）

        public CustomerData()
        {
            m_displayName = new Dictionary<string, string>();
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

        public string type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        public bool can_litter
        {
            get { return m_canLitter; }
            set { m_canLitter = value; }
        }

        public string icon_texture
        {
            get { return m_iconTexture; }
            set { m_iconTexture = value; }
        }

        public string model_name
        {
            get { return m_modelName; }
            set { m_modelName = value; }
        }
    }
}
