using System.Collections.Generic;
using Newtonsoft.Json;
using Ministone.GameCore.GameData.Generic;


namespace Ministone.GameCore.GameData
{

    public enum LevelType
    {
        UNKNOWED = 0,
        FIXED_TIME,
        FIXED_CUSTOMER,
        LOST_CUSTOMER
    }

    public class DecayData
    {
        public float interval;      // 衰减间隔
        public float rate;          // 每次衰减比例
        public float limit;         // 衰减极限值

        public DecayData()
        {
            
        }

        public DecayData(float intv, float r, float l = -1)
        {
            interval = intv;
            rate = r;
            limit = l;
        }

        public void set(float intv, float r, float l = -1)
        {
            interval = intv;
            rate = r;
            limit = l;
        }
    }

    public class RewardData
    {
        public string itemKey;                      // 奖励的物品ID
        public int itemCount;                       // 奖励的物品数量
    }

    public class CustomerOrder
    {
        public int latestFirstCome;                 // 最晚出现的次序
        public float weight;                        // 权重
        public string customer;                     // 顾客名称
        public List<string> foods = new List<string>();                  // 所点的食物列表
        public RangeData<int> interval = new RangeData<int>(0, 0);             // 出现的间隔
        public string randomFoodStep = "";
    }

    public class FailTipData
    {
        public int id;
        public bool isUpgrade;
        public Dictionary<string, string> tips = new Dictionary<string, string>();
    }

    public class SecretCustomer
    {
        public string customer;
        public bool isRemind;               //  是否会刚出订单时显示食物，然后再变成问号
        public List<int> showOrders = new List<int>();               // 第几次出现之前都会显示订单
    }

    public class Requirements
    {
        public class NameAndNumber
        {
            public NameAndNumber() { name = ""; number = 0; }
            public NameAndNumber(string _name, int _num)
            {
                name = _name;
                number = _num;
            }
            public string name;
            public int number;
        }
        public bool allowBurn = true;                      // 是否允许烧焦
        public bool allowLostCustomer = true;              // 是否允许流失顾客
        public int smileCount = 0;                             // 需要收集的笑脸个数
        public List<NameAndNumber> requiredCustomers = new List<NameAndNumber>();      // 需要服务的顾客个数
        public List<NameAndNumber> requiredFoods = new List<NameAndNumber>();          // 需要服务食物个数
    }

    public class AutoGenConfig
    {
        public LevelType type;
        public int total;
        public int maxOrder;
        public int difficulty;
        public List<float> firstArrivals = new List<float>();
        public List<string> customerList = new List<string>();
        public Dictionary<string, float> foodWeightList = new Dictionary<string, float>();
        public int specialNum;
        public int fullPatienceNum;
        public RangeData<float> orderInterval = new RangeData<float>(0, 0);
        public RangeData<int> litterInterval = new RangeData<int>(0, 0);
        public RangeData<int> brokenInterval = new RangeData<int>(0, 0);
        public RangeData<int> rainInterval = new RangeData<int>(0, 0);
        public DecayData waitDecay = new DecayData();
        public DecayData cookingDecay = new DecayData();
        public DecayData burnDecay = new DecayData();
        public DecayData orderDecay = new DecayData();
        public bool random_customer_requirement;
        public bool random_food_requirement;
        public bool random_smile_requirement;
        public bool any_customer_requirement;
    }

    public class LevelData
    {
        int m_levelId;                            // 关卡ID
        LevelType m_type = LevelType.UNKNOWED;    // 关卡类型
        int m_total;                              // 关卡设定的总时间/总顾客数/流失人数
        List<string> m_scoreList = new List<string>();  // 过关分数 
        int m_maxOrder;                           // 同时存在的最大订单数
        int m_fullPatienceNum;                    // 满耐心值
        int m_minInstructSteps;                   // 提示制作过程的最小步数
        List<float> m_firstArrivals = new List<float>();                  // 首次出现顾客的时间
        RangeData<float> m_orderInterval = new RangeData<float>(0, 0);    // 订单的间隔范围
        RangeData<int> m_litterInterval = new RangeData<int>(0, 0);       // 顾客扔垃圾的间隔
        RangeData<int> m_brokenInterval = new RangeData<int>(0, 0);       // 厨具损坏的间隔
        RangeData<int> m_rainInterval = new RangeData<int>(0, 0);         // 下雨的间隔
        DecayData m_waitingDecay = new DecayData();                       // 等待时间衰减
        DecayData m_cookingDecay = new DecayData();                       // 制作时间衰减
        DecayData m_burnDecay = new DecayData();                          // 烧焦时间衰减
        DecayData m_orderDecay = new DecayData();                         // 订单出现时间衰减

        List<CustomerOrder> m_orders = new List<CustomerOrder>();         // 订单列表
        List<CustomerOrder> m_specialOrders = new List<CustomerOrder>();  // 间隔出现订单列表
        List<CustomerOrder> m_anyfoodOrders = new List<CustomerOrder>();  // 随机食物订单列表
        List<CustomerOrder> m_guideOrders = new List<CustomerOrder>();    // 需要进行指引的订单

        List<SecretCustomer> m_secretCustomers = new List<SecretCustomer>();       // 神秘顾客列表
        List<int> m_unlockItems = new List<int>();                        // 解锁的物品列表
        List<RewardData> m_rewards = new List<RewardData>();              // 奖励的物品列表
        FailTipData m_failTips = new FailTipData();                       // 失败的提示内容列表，多语言

        Requirements m_requirements = new Requirements();                 // 过关条件
        List<string> m_organicMaterials = new List<string>();             // 有机食材
        string m_comments = "";                                           // 关卡设计备注

        public LevelData()
        {
            
        }

        public static LevelData ParseFromJson(string json)
        {
            return JsonConvert.DeserializeObject<LevelData>(json);
        }

        public string GetJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public int id
        {
            get { return m_levelId; }
            set { m_levelId = value; }
        }

        public LevelType type
        {
            get { return m_type; }
            set { m_type = value; }
        }


        public int total
        {
            get { return m_total; }
            set { m_total = value; }
        }

        public List<string> scoreList
        {
            get { return m_scoreList; }
            set { m_scoreList = value; }
        }

        public int max_order
        {
            get { return m_maxOrder; }
            set { m_maxOrder = value; }
        }

        public int full_patience_num
        {
            get { return m_fullPatienceNum; }
            set { m_fullPatienceNum = value; }
        }

        public int min_instruct_steps
        {
            get { return m_minInstructSteps; }
            set { m_minInstructSteps = value; }
        }

        public List<float> first_arrivals
        {
            get { return m_firstArrivals; }
            set { m_firstArrivals = value; }
        }

        public RangeData<float> order_interval
        {
            get { return m_orderInterval; }
            set { m_orderInterval = value; }
        }

        public RangeData<int> litter_interval
        {
            get { return m_litterInterval; }
            set { m_litterInterval = value; }
        }

        public RangeData<int> broken_interval
        {
            get { return m_brokenInterval; }
            set { m_brokenInterval = value; }
        }

        public RangeData<int> rain_interval
        {
            get { return m_rainInterval; }
            set { m_rainInterval = value; }
        }

        public DecayData waiting_decay
        {
            get { return m_waitingDecay; }
            set { m_waitingDecay = value; }
        }

        public DecayData cooking_decay
        {
            get { return m_cookingDecay; }
            set { m_cookingDecay = value; }
        }

        public DecayData burn_decay
        {
            get { return m_burnDecay; }
            set { m_burnDecay = value; }
        }

        public DecayData order_decay
        {
            get { return m_orderDecay; }
            set { m_orderDecay = value; }
        }

        public List<CustomerOrder> orders
        {
            get { return m_orders; }
            set { m_orders = value; }
        }

        public List<CustomerOrder> specialOrders
        {
            get { return m_specialOrders; }
            set { m_specialOrders = value; }
        }

        public List<CustomerOrder> anyfoodOrders
        {
            get { return m_anyfoodOrders; }
            set { m_anyfoodOrders = value; }
        }

        public List<CustomerOrder> guide_orders
        {
            get { return m_guideOrders; }
            set { m_guideOrders = value; }
        }

        public List<SecretCustomer> secret_customers
        {
            get { return m_secretCustomers; }
            set { m_secretCustomers = value; }
        }

        public List<int> unlock_items
        {
            get { return m_unlockItems; }
            set { m_unlockItems = value; }
        }

        public List<RewardData> rewards
        {
            get { return m_rewards; }
            set { m_rewards = value; }
        }

        public FailTipData fail_tips
        {
            get { return m_failTips; }
            set { m_failTips = value; }
        }

        public string GetFailTips(string lang = "en")
        {
            if (string.IsNullOrEmpty(lang))
            {
                lang = "en";
            }

            if (m_failTips != null)
            {
                string tips;
                m_failTips.tips.TryGetValue(lang, out tips);
                return tips;
            }
            return "";
        }

        public Requirements requirements
        {
            get { return m_requirements; }
            set { m_requirements = value; }
        }

        public List<string> organicMaterials{
            get { return m_organicMaterials; }
            set { m_organicMaterials = value; }
        }

        public string comments
        {
            get { return m_comments; }
            set { m_comments = value; }
        }
    }
}