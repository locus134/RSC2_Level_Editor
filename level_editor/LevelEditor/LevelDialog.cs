using System;
using System.IO;
using Gtk;
using Gdk;
using Newtonsoft.Json;
using System.Collections.Generic;
using Ministone.GameCore.GameData;
using Ministone.GameCore.GameData.Generic;

namespace LevelEditor
{
    public partial class LevelDialog : Gtk.Dialog
    {
        MapDataManager _restMgr = MapDataManager.GetInstance();
        FoodDataManager _foodMgr = FoodDataManager.GetInstance();
        CustomerDataManager _custMgr = CustomerDataManager.GetInstance();
        OrderDataManager _orderMgr = OrderDataManager.GetInstance();
        LevelDataManager _lvlMgr = LevelDataManager.GetInstance();
        //string m_levelFileDir;

        int m_curLevelId;
        int m_maxLevelId;
        MapData m_curRestData;
        List<LevelData> m_allLevelData;
        Dictionary<string, List<string>> m_foodCustomerDict;
        LevelData m_curLevelData;
        ListStore m_orderListStore;
        Pixbuf ADD_ICON;

        enum OrderSeq
        {
            customerIcon = 0,
            foodIcon = 1,
            weiget,
            minInterval,
            maxInterval,
            randomFood,
            lastestCome,
            guide,
            customerKey,
            foods,
        };

        public LevelDialog(Gtk.Window parent, string mapKey)
        {
            //m_levelFileDir = levelFileDir;

            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            m_curLevelId = Utils.AppConfig.select_level;

            // 读取餐厅数据
            m_curRestData = _restMgr.GetMapData(mapKey);
            m_allLevelData = _lvlMgr.GetAllLevels(m_curRestData.key);
            if (m_allLevelData == null)
            {
                m_allLevelData = new List<LevelData>();
            }

            m_foodCustomerDict = new Dictionary<string, List<string>>();
            foreach (string food in m_curRestData.food_list)
            {
                List<string> customerList = new List<string>();
                List<OrderData> orderList = _orderMgr.GetFoodOrders(food);
                foreach (OrderData ord in orderList)
                {
                    if (m_curRestData.customer_list.Contains(ord.customer) || m_curRestData.special_customer_list.Contains(ord.customer))
                    {
                        customerList.Add(ord.customer);
                    }
                }
                m_foodCustomerDict.Add(food, customerList);
            }

            m_maxLevelId = m_curRestData.level_count;

            // UI 初始化
            ADD_ICON = Pixbuf.LoadFromResource("add_icon.png");
            tree_level_foods.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            tree_level_foods.AppendColumn("名称", new CellRendererText(), "text", 1);
            tree_level_foods.Selection.Mode = SelectionMode.Multiple;
            tree_level_foods.Selection.Changed += OnTreeLevelFoodSelectionChanged;

            tree_level_customer.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            tree_level_customer.AppendColumn("名称", new CellRendererText(), "text", 1);
            tree_level_customer.Selection.Mode = SelectionMode.Multiple;

            tree_order_list.AppendColumn("顾客", new CellRendererPixbuf(), "pixbuf", (int)OrderSeq.customerIcon);
            tree_order_list.AppendColumn("食物", new CellRendererPixbuf(), "pixbuf", (int)OrderSeq.foodIcon);

            CellRendererText cell = new CellRendererText();
            cell.Editable = true;
            cell.Edited += OnEditedOrderWeight;
            tree_order_list.AppendColumn("比重", cell, "text", (int)OrderSeq.weiget);

            cell = new CellRendererText();
            cell.Editable = true;
            cell.Edited += OnEditedOrderIntervalStart;
            tree_order_list.AppendColumn("间隔始", cell, "text", (int)OrderSeq.minInterval);

            cell = new CellRendererText();
            cell.Editable = true;
            cell.Edited += OnEditedOrderIntervalEnd;
            tree_order_list.AppendColumn("间隔末", cell, "text", (int)OrderSeq.maxInterval);

            cell = new CellRendererText();
            cell.Editable = true;
            cell.Edited += OnEditedOrderRandomFood;
            tree_order_list.AppendColumn("随机食物", cell, "text", (int)OrderSeq.randomFood);

            cell = new CellRendererText();
            cell.Editable = true;
            cell.Edited += OnEditedLatestFirstCome;
            tree_order_list.AppendColumn("最晚", cell, "text", (int)OrderSeq.lastestCome);

            CellRendererToggle cellToggle = new CellRendererToggle();
            cellToggle.Activatable = true;
            cellToggle.Toggled += OnToggledGuideOrder;
            tree_order_list.AppendColumn("引导?", cellToggle, "active", (int)OrderSeq.guide);
            m_orderListStore = new ListStore(typeof(Pixbuf), typeof(Pixbuf),typeof(string), typeof(int),typeof(int), typeof(string), 
                                             typeof(int), typeof(bool), typeof(string), typeof(List<string>));
            tree_order_list.Model = m_orderListStore;

            label_level_range.Text = string.Format("关卡ID范围：1-{0}", m_maxLevelId);
            for (int i = 1; i <= m_curRestData.level_count; ++i)
            {
                string level = string.Format("{0}({1})", i, i + m_curRestData.start_level -1);
                combobox_level_id.AppendText(level);
            }
            combobox_level_id.Active = m_curLevelId - 1;

            Utils.ShowFoodList(m_curRestData.food_list, tree_level_foods);

            text_level_rewards.WrapMode = WrapMode.Char;
            text_level_tips.WrapMode = WrapMode.Char;
            text_level_comments.WrapMode = WrapMode.WordChar;

            ClearAutogen();
        }

        void ClearAutogen()
        {
            combobox_autogen_difficulty.Active = 0;
            text_autogen_food_list.Text = "";
            text_auto_customer_list.Text = "";
            text_autogen_specialnum.Text = "0";
            check_autogen_random_food.Active = false;
            check_autogen_random_customer.Active = false;
            check_autogen_random_smile.Active = false;
            check_autogen_any_customer.Active = false;
        }

        void ClearData()
        {
            m_curLevelData = new LevelData();

            combobox_config_type.Active = 0;
            text_config_total.Text = "0";
            text_config_score.Text = "3000;3500;4000";
            text_config_max_order.Text = "0";
            text_config_full_patience.Text = "0";
            text_config_order_interval_start.Text = "0";
            text_config_order_interval_end.Text = "0";
            text_config_first_arrivals.Text = "(逗号隔开，如2,5,8,11)";

            text_waitdecay_interval.Text = "0";
            text_waitdecay_rate.Text = "0";
            text_waitdecay_limit.Text = "0";
            text_rainInterval_start.Text = "0";
            text_rainInterval_end.Text = "0";
            text_orderdecay_interval.Text = "0";
            text_orderdecay_rate.Text = "0";
            text_orderdecay_limit.Text = "0";
            text_litterinterval_start.Text = "0";
            text_litterinterval_end.Text = "0";
            text_cookingdecay_interval.Text = "0";
            text_cookingdecay_rate.Text = "0";
            text_cookingdecay_limit.Text = "0";
            text_brokeninterval_start.Text = "0";
            text_brokeninterval_end.Text = "0";

            text_secret_customers.Text = "";
            text_level_requirements.Text = "";
            text_min_instruct_steps.Text = "0";
            text_unlock_item.Text = "";
            text_level_rewards.Buffer.Text = "";
            text_level_tips.Buffer.Text = "";
            text_level_comments.Buffer.Text = "";

            m_orderListStore.Clear();

            ClearAutogen();
        }

        void ShowData()
        {
            combobox_config_type.Active = (int)(m_curLevelData.type - 1);
            text_config_total.Text = m_curLevelData.total.ToString();
            text_config_score.Text = string.Format("{0};{1};{2}", m_curLevelData.scoreList[0], m_curLevelData.scoreList[1], m_curLevelData.scoreList[2]);
            text_config_max_order.Text = m_curLevelData.max_order.ToString();
            text_config_full_patience.Text = m_curLevelData.full_patience_num.ToString();
            text_config_order_interval_start.Text = m_curLevelData.order_interval.min.ToString();
            text_config_order_interval_end.Text = m_curLevelData.order_interval.max.ToString();
            if (m_curLevelData.first_arrivals.Count > 0)
            {
                text_config_first_arrivals.Text = string.Join(",", m_curLevelData.first_arrivals);
            }
            else
            {
                text_config_first_arrivals.Text = "(逗号隔开，如2,5,8,11)";
            }

            text_waitdecay_interval.Text = m_curLevelData.waiting_decay.interval.ToString();
            text_waitdecay_rate.Text = m_curLevelData.waiting_decay.rate.ToString();
            text_waitdecay_limit.Text = m_curLevelData.waiting_decay.limit.ToString();
            text_rainInterval_start.Text = m_curLevelData.rain_interval.min.ToString();
            text_rainInterval_end.Text = m_curLevelData.rain_interval.max.ToString();
            text_orderdecay_interval.Text = m_curLevelData.order_decay.interval.ToString();
            text_orderdecay_rate.Text = m_curLevelData.order_decay.rate.ToString();
            text_orderdecay_limit.Text = m_curLevelData.order_decay.limit.ToString();
            text_litterinterval_start.Text = m_curLevelData.litter_interval.min.ToString();
            text_litterinterval_end.Text = m_curLevelData.litter_interval.max.ToString();
            text_cookingdecay_interval.Text = m_curLevelData.cooking_decay.interval.ToString();
            text_cookingdecay_rate.Text = m_curLevelData.cooking_decay.rate.ToString();
            text_cookingdecay_limit.Text = m_curLevelData.cooking_decay.limit.ToString();
            text_brokeninterval_start.Text = m_curLevelData.broken_interval.min.ToString();
            text_brokeninterval_end.Text = m_curLevelData.broken_interval.max.ToString();

            if (m_curLevelData.secret_customers.Count > 0)
            {
                //text_secret_customers.Text = JsonConvert.SerializeObject(m_curLevelData.secret_customers);
                string showStr = "";
                foreach(SecretCustomer secret in m_curLevelData.secret_customers)
                {
                    showStr += secret.customer;
                    if(secret.showOrder > 0)
                    {
                        showStr += "<" + secret.showOrder.ToString() + ">";
                    }

                    showStr += ";";
                }
                if (showStr.Length > 0) showStr = showStr.Substring(0, showStr.Length - 1);
                text_secret_customers.Text = showStr;
            }
            else
            {
                text_secret_customers.Text = "";
            }

            string requireStr = "";
            if(m_curLevelData.requirements.requiredCustomers.Count > 0)
            {
                foreach(var req in m_curLevelData.requirements.requiredCustomers)
                {
                    requireStr += string.Format("{0}*{1};", req.name, req.number); 
                }
            }else if(m_curLevelData.requirements.requiredFoods.Count > 0){
                foreach (var req in m_curLevelData.requirements.requiredFoods)
                {
                    requireStr += string.Format("{0}*{1};", req.name, req.number);
                }
            }else if(!m_curLevelData.requirements.allowBurn){
                requireStr += "no_burn;";
            }else if (!m_curLevelData.requirements.allowLostCustomer)
            {
                requireStr += "no_lost;";
            }else if (m_curLevelData.requirements.smileCount > 0)
            {
                requireStr += "smile*" + m_curLevelData.requirements.smileCount + ";";
            }
            if(requireStr.Length > 0)
            {
                requireStr = requireStr.Substring(0, requireStr.Length - 1);
            }
            text_level_requirements.Text = requireStr;

            text_min_instruct_steps.Text = m_curLevelData.min_instruct_steps.ToString();

            if (m_curLevelData.unlock_items.Count > 0)
            {
                //text_unlock_item.Text = JsonConvert.SerializeObject(m_curLevelData.unlock_items);
                string unlocks = "";
                foreach(int id in m_curLevelData.unlock_items)
                {
                    unlocks += id.ToString() + ";";
                }
                if(unlocks.Length > 0)
                {
                    unlocks = unlocks.Substring(0, unlocks.Length - 1);
                }
                text_unlock_item.Text = unlocks;
            }
            else
            {
                text_unlock_item.Text = "";
            }

            if (m_curLevelData.rewards.Count > 0)
            {
                //text_level_rewards.Buffer.Text = JsonConvert.SerializeObject(m_curLevelData.rewards);

                string rewards = "";
                foreach (RewardData reward in m_curLevelData.rewards)
                {
                    rewards += reward.itemKey + "*" + reward.itemCount + ";";
                }
                if (rewards.Length > 0) rewards = rewards.Substring(0, rewards.Length - 1);
            }
            else
            {
                text_level_rewards.Buffer.Text = "";
            }
            text_level_tips.Buffer.Text = JsonConvert.SerializeObject(m_curLevelData.fail_tips);
            text_level_comments.Buffer.Text = m_curLevelData.comments;

            m_orderListStore.Clear();
            AppendOrderList(m_curLevelData.guide_orders, true);
            AppendOrderList(m_curLevelData.orders, false);
            AppendOrderList(m_curLevelData.specialOrders, false);
            AppendOrderList(m_curLevelData.anyfoodOrders, false);
        }

        protected bool ReadData()
        {
            string errMsg;

            LevelData levelData = new LevelData();
            levelData.id = m_curLevelId;
            levelData.type = LevelType.UNKNOWED + combobox_config_type.Active + 1;
            int intNum;
            if (!int.TryParse(text_config_total.Text, out intNum) || intNum <= 0)
            {
                errMsg = "请输入正确的类型参数";
                goto err_return;
            }
            levelData.total = intNum;

            //if (!int.TryParse(text_config_score.Text, out intNum) || intNum <= 0)
            //{
            //    errMsg = "请输入正确的分数值";
            //    goto err_return;
            //}
            //levelData.score = intNum;

            //if (!List<int>.TryParse(text_config_score.Text, out intNum) || intNum <= 0)
            string scoreText = text_config_score.Text;
            bool hasSptChar = scoreText.Contains(";");
            string[] scoreList = scoreText.Split(';');
            if(!hasSptChar || scoreList.Length != 3)
            {
                errMsg = "请输入正确的分数值";
                goto err_return;
            }
            levelData.scoreList = new List<string>(scoreList);


            if (!int.TryParse(text_config_max_order.Text, out intNum) || intNum <= 0)
            {
                errMsg = "请输入正确的最大订单数";
                goto err_return;
            }
            levelData.max_order = intNum;

            //if (!int.TryParse(text_config_full_patience.Text, out intNum) || intNum <= 0)
            //{
            //    errMsg = "请输入正确的满耐心值";
            //    goto err_return;
            //}
            //levelData.full_patience_num = intNum;

            float fNum;
            if (!float.TryParse(text_config_order_interval_start.Text, out fNum) || fNum < 1e-6)
            {
                errMsg = "请输入正确的最小订单间隔时间";
                goto err_return;
            }
            levelData.order_interval.min = fNum;

            if (!float.TryParse(text_config_order_interval_end.Text, out fNum) || fNum < 1e-6)
            {
                errMsg = "请输入正确的最大订单间隔时间";
                goto err_return;
            }
            levelData.order_interval.max = fNum;

            string[] strArr = text_config_first_arrivals.Text.Split(',');
            if (strArr.Length != levelData.max_order)
            {
                errMsg = "输入的首次来客时间个数与最大订单数不符";
                goto err_return;
            }

            for (int i = 0; i < strArr.Length; ++ i)
            {
                if (!float.TryParse(strArr[i], out fNum))
                {
                    errMsg = string.Format("第{0}个首次来客时间输入有误！", i + 1);
                    goto err_return;
                }
                levelData.first_arrivals.Add(fNum);
            }

            float decay_intv, decay_rate, decay_limit;
            if (!float.TryParse(text_waitdecay_interval.Text, out decay_intv)
                || !float.TryParse(text_waitdecay_rate.Text, out decay_rate)
                || !float.TryParse(text_waitdecay_limit.Text, out decay_limit))
            {
                errMsg = "请输入正确的等待时间衰减配置";
                goto err_return;
            }
            levelData.waiting_decay = new DecayData(decay_intv, decay_rate, decay_limit);

            if (!float.TryParse(text_orderdecay_interval.Text, out decay_intv)
                || !float.TryParse(text_orderdecay_rate.Text, out decay_rate)
                || !float.TryParse(text_orderdecay_limit.Text, out decay_limit))
            {
                errMsg = "请输入正确的来客时间衰减配置";
                goto err_return;
            }
            levelData.order_decay = new DecayData(decay_intv, decay_rate, decay_limit);

            if (!float.TryParse(text_cookingdecay_interval.Text, out decay_intv)
                || !float.TryParse(text_cookingdecay_rate.Text, out decay_rate)
                || !float.TryParse(text_cookingdecay_limit.Text, out decay_limit))
            {
                errMsg = "请输入正确的制作时间衰减配置";
                goto err_return;
            }
            levelData.cooking_decay = new DecayData(decay_intv, decay_rate, decay_limit);

            int min, max;
            if (!int.TryParse(text_rainInterval_start.Text, out min)
                || !int.TryParse(text_rainInterval_end.Text, out max))
            {
                errMsg = "请输入正确的下雨时间间隔";
                goto err_return;
            }
            levelData.rain_interval = new RangeData<int>(min, max);

            if (!int.TryParse(text_litterinterval_start.Text, out min)
                || !int.TryParse(text_litterinterval_end.Text, out max))
            {
                errMsg = "请输入正确的扔垃圾间隔";
                goto err_return;
            }
            levelData.litter_interval = new RangeData<int>(min, max);

            if (!int.TryParse(text_brokeninterval_start.Text, out min)
                || !int.TryParse(text_brokeninterval_end.Text, out max))
            {
                errMsg = "请输入正确的厨具损坏间隔";
                goto err_return;
            }
            levelData.broken_interval = new RangeData<int>(min, max);

            // 读取神秘顾客
            string secretStr = text_secret_customers.Text;
            levelData.parseSecretCustomer(secretStr);

            // 读取额外条件
            string requirements = text_level_requirements.Text;
            levelData.parseRequirement(requirements);

            if (!int.TryParse(text_min_instruct_steps.Text, out intNum))
            {
                errMsg = "请输入正确的最小提示步数";
                goto err_return;
            }
            levelData.min_instruct_steps = intNum;

            string[] unlockItems = text_unlock_item.Text.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (unlockItems.Length > 0)
            {
                foreach (string itemId in unlockItems)
                {
                    levelData.unlock_items.Add(itemId.ToInt32());
                }
            }

            string rewards = text_level_rewards.Buffer.Text;
            levelData.parseRewards(rewards);

            //FailTipData failTips = JsonConvert.DeserializeObject<FailTipData>(text_level_tips.Buffer.Text);
            //if (failTips != null)
            //{
            //    levelData.fail_tips = failTips;
            //}
            //if (!string.IsNullOrEmpty(text_level_comments.Buffer.Text))
            //{
            //    levelData.comments = text_level_comments.Buffer.Text;
            //}

            TreeIter iter;
            if (m_orderListStore.GetIterFirst(out iter))
            {
                do
                {
                    CustomerOrder ord = new CustomerOrder();
                    ord.weight = float.Parse((string)m_orderListStore.GetValue(iter, (int)OrderSeq.weiget));
                    ord.interval.min = (int)m_orderListStore.GetValue(iter, (int)OrderSeq.minInterval);
                    ord.interval.max = (int)m_orderListStore.GetValue(iter, (int)OrderSeq.maxInterval);
                    ord.randomFoodRule = (string)m_orderListStore.GetValue(iter, (int)OrderSeq.randomFood);
                    ord.latestFirstCome = (int)m_orderListStore.GetValue(iter, (int)OrderSeq.lastestCome);
                    bool isGuide = (bool)m_orderListStore.GetValue(iter, (int)OrderSeq.guide);
                    ord.customer = (string)m_orderListStore.GetValue(iter, (int)OrderSeq.customerKey);
                    ord.foods = (List<string>)m_orderListStore.GetValue(iter, (int)OrderSeq.foods);

                    if (isGuide)
                    {
                        levelData.guide_orders.Add(ord);
                    }
                    else if(ord.weight > 0)
                    {
                        levelData.orders.Add(ord);
                    }
                    else if(ord.interval.min > 0 && ord.interval.max > 0)
                    {
                        if(ord.randomFoodRule != "none")
                        {
                            levelData.anyfoodOrders.Add(ord);
                        }else {
                            levelData.specialOrders.Add(ord);
                        }
                    }
                } while (m_orderListStore.IterNext(ref iter));
            }

            m_curLevelData = levelData;
            return true;

        err_return:
            if (errMsg == null)
            {
                errMsg = "输入的数值有误，请检查后再次输入！";
            }
            MessageDialog dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close,
                                                  errMsg);
            dlg.Run();
            dlg.Destroy();
            return false;
        }

        protected void AppendOrderList(List<CustomerOrder> orderList, bool isGuide)
        {
            int iconSize = 60;
            foreach (CustomerOrder ord in orderList)
            {
                Pixbuf pb = new Pixbuf(Utils.GetCustomerImagePath(ord.customer));
                float scale = (float)iconSize / pb.Height;
                Pixbuf custIcon = pb.ScaleSimple((int)(scale * pb.Width), iconSize, InterpType.Hyper);

                if(ord.foods.Count > 0)
                {
                    pb = new Pixbuf(Utils.GetFoodImagePath(ord.foods[0]));  
                }else{
                    pb = Utils.GetGeneralIcon();
                }
                scale = (float)iconSize / pb.Height;
                Pixbuf foodIcon;
                int count = ord.foods.Count;
                if (count < 2)
                {
                    foodIcon = pb.ScaleSimple((int)(scale * pb.Width), iconSize, InterpType.Hyper);
                }
                else
                {
                    int totalWidth = (int)(scale * pb.Width);
                    if (totalWidth < ADD_ICON.Width)
                    {
                        totalWidth = ADD_ICON.Width;
                    }
                    int totalHeight = iconSize * count + ADD_ICON.Height * (count - 1);
                    List<Pixbuf> foodPixBufs = new List<Pixbuf>();
                    foodPixBufs.Add(pb.ScaleSimple((int)(scale * pb.Width), iconSize, InterpType.Hyper));
                    for (int i = 1; i < count; ++i)
                    {
                        pb = new Pixbuf(Utils.GetFoodImagePath(ord.foods[i]));
                        scale = (float)iconSize / pb.Height;
                        int w = (int)(scale * pb.Width);
                        foodPixBufs.Add(pb.ScaleSimple(w, iconSize, InterpType.Hyper));
                        if (totalWidth < w)
                        {
                            totalWidth = w;
                        }
                    }

                    int dest_y = 0;
                    foodIcon = new Pixbuf(pb.Colorspace, true, pb.BitsPerSample, totalWidth, totalHeight);
                    foodIcon.Fill(0);
                    Pixbuf foodpb = foodPixBufs[0];
                    foodpb.Composite(foodIcon, (totalWidth - foodpb.Width) / 2, dest_y, foodpb.Width, foodpb.Height,
                                     (totalWidth - foodpb.Width) / 2, dest_y,
                                     1.0, 1.0, InterpType.Hyper, 255);
                    dest_y += foodpb.Height;
                    for (int i = 1; i < count; ++i)
                    {
                        ADD_ICON.Composite(foodIcon, (totalWidth - ADD_ICON.Width) / 2, dest_y, ADD_ICON.Width, ADD_ICON.Height,
                                           (totalWidth - ADD_ICON.Width) / 2, dest_y,
                                           1.0, 1.0, InterpType.Hyper, 255);
                        dest_y += ADD_ICON.Height;
                        foodpb = foodPixBufs[i];
                        foodpb.Composite(foodIcon, (totalWidth - foodpb.Width) / 2, dest_y, foodpb.Width, foodpb.Height,
                                         (totalWidth - foodpb.Width) / 2, dest_y,
                                         1.0, 1.0, InterpType.Hyper, 255);
                        dest_y += foodpb.Height;
                    }
                }

                m_orderListStore.AppendValues(custIcon, foodIcon, ord.weight.ToString(), ord.interval.min, ord.interval.max, ord.randomFoodRule, ord.latestFirstCome, isGuide, ord.customer, ord.foods);
            }
        }

        void LoadLevel(int levelId)
        {
            LevelData lvd = null;
            for (int i = 0; i < m_allLevelData.Count; ++i)
            {
                LevelData lv = m_allLevelData[i];
                if (lv.id == levelId + m_curRestData.start_level - 1)
                {
                    lvd = lv;
                    break;
                }
            }

            if (lvd == null)
            {
                ClearData();
            }
            else
            {
                m_curLevelData = lvd;
                ShowData();
            }
        }

        protected void OnEditedOrderWeight(object o, EditedArgs args)
        {
            TreeIter iter;
            if (m_orderListStore.GetIterFromString(out iter, args.Path))
            {
                float weight;
                if (Utils.ParseFloat(args.NewText, out weight, this))
                {
                    if(weight > 0)
                    {
                        m_orderListStore.SetValue(iter, (int)OrderSeq.weiget, weight.ToString());

                        m_orderListStore.SetValue(iter, (int)OrderSeq.minInterval, 0);
                        m_orderListStore.SetValue(iter, (int)OrderSeq.maxInterval, 0);
                        m_orderListStore.SetValue(iter, (int)OrderSeq.randomFood, "none"); 
                    }else{
                        m_orderListStore.SetValue(iter, (int)OrderSeq.weiget, weight.ToString());
                    }
                }
            }
        }

        protected void OnEditedOrderIntervalStart(object o, EditedArgs args)
        {
            TreeIter iter;
            if (m_orderListStore.GetIterFromString(out iter, args.Path))
            {
                int num;
                if (Utils.ParseInteger(args.NewText, out num, this) && num >= 0)
                {
                    m_orderListStore.SetValue(iter, (int)OrderSeq.minInterval, num);

                    if (num > 0)
                    {
                        m_orderListStore.SetValue(iter, 2, "0");
                        int end = (int)m_orderListStore.GetValue(iter, (int)OrderSeq.maxInterval);
                        if (end < num)
                        {
                            m_orderListStore.SetValue(iter, (int)OrderSeq.maxInterval, num + 1);
                        }
                    }
                }
            }
        }

        protected void OnEditedOrderIntervalEnd(object o, EditedArgs args)
        {
            TreeIter iter;
            if (m_orderListStore.GetIterFromString(out iter, args.Path))
            {
                int num;
                if (Utils.ParseInteger(args.NewText, out num, this) && num >= 0)
                {
                    m_orderListStore.SetValue(iter, (int)OrderSeq.maxInterval, num);

                    if (num > 0)
                    {
                        m_orderListStore.SetValue(iter, (int)OrderSeq.weiget, "0");
                        int start = (int)m_orderListStore.GetValue(iter, (int)OrderSeq.minInterval);
                        if (start == 0)
                        {
                            m_orderListStore.SetValue(iter, (int)OrderSeq.minInterval, 1);
                        }
                        else if (start > num)
                        {
                            m_orderListStore.SetValue(iter, (int)OrderSeq.minInterval, num);
                        }
                    }
                }
            }
        }

        protected void OnEditedOrderRandomFood(object o, EditedArgs args)
        {
            TreeIter iter;
            if (m_orderListStore.GetIterFromString(out iter, args.Path))
            {
                string rule = args.NewText;

                if (rule == "only2" || rule == "only3" || rule == "more2" || rule == "more3" )
                {
                    
                    m_orderListStore.SetValue(iter, (int)OrderSeq.weiget, "0");
                    m_orderListStore.SetValue(iter, (int)OrderSeq.randomFood, rule);
                }
                else if(rule == "none" || rule == "")
                {
                    m_orderListStore.SetValue(iter, (int)OrderSeq.randomFood, "none");
                }
                else{
                    MessageDialog dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close,
                                                          "不支持的随机食物规则，只能:\nnone,only2,more2,only3,more3\n的一种");
                    dlg.Run();
                    dlg.Destroy();
                }
            }
        }

        protected void OnEditedLatestFirstCome(object o, EditedArgs args)
        {
            TreeIter iter;
            if (m_orderListStore.GetIterFromString(out iter, args.Path))
            {
                int num;
                if (Utils.ParseInteger(args.NewText, out num, this) && num >= 0)
                {
                    m_orderListStore.SetValue(iter, (int)OrderSeq.lastestCome, num);
                }
            }
        }

        protected void OnToggledGuideOrder(object o, ToggledArgs args)
        {
            TreeIter iter;
            if (m_orderListStore.GetIterFromString(out iter, args.Path))
            {
                bool val = (bool)m_orderListStore.GetValue(iter, (int)OrderSeq.guide);
                m_orderListStore.SetValue(iter, (int)OrderSeq.guide, !val);
            }
        }

        protected void OnButtonCancelClicked(object sender, EventArgs e)
        {
            this.Destroy();
        }

        protected void OnComboboxLevelIdChanged(object sender, EventArgs e)
        {
            m_curLevelId = combobox_level_id.Active + 1;
            text_level_id.Text = m_curLevelId.ToString();
            LoadLevel(m_curLevelId);
            Utils.AppConfig.select_level = m_curLevelId;
            Utils.SerializeAppConfig();
        }

        protected void OnBtnReloadLevelClicked(object sender, EventArgs e)
        {
            int levelId;
            if (Utils.ParseInteger(text_level_id.Text, out levelId, this))
            {
                if (levelId > 0 && levelId <= m_maxLevelId)
                {
                    int curSel = combobox_level_id.Active;
                    if (curSel == levelId - 1)
                    {

                        m_curLevelId = levelId;
                        LoadLevel(m_curLevelId);
                    }
                    else
                    {
                        combobox_level_id.Active = levelId - 1;
                    }
                }
                else
                {
                    text_level_id.Text = m_curLevelId.ToString();
                }
            }
        }

        protected void OnTreeLevelFoodSelectionChanged(object sender, EventArgs e)
        {
            TreeSelection treeSelection = sender as TreeSelection;
            TreeModel model;
            TreePath[] iterPaths = treeSelection.GetSelectedRows(out model);
            if (model != null && iterPaths != null && iterPaths.Length > 0)
            {
                List<string> customerList;
                string food = m_curRestData.food_list[iterPaths[0].Indices[0]];
                if (m_foodCustomerDict.TryGetValue(food, out customerList) && iterPaths.Length > 1)
                {
                    for (int i = customerList.Count - 1; i >= 0; --i)
                    {
                        string customer = customerList[i];
                        bool valid = true;
                        for (int j = 1; j < iterPaths.Length; ++j)
                        {
                            food = m_curRestData.food_list[iterPaths[j].Indices[0]];
                            List<string> cl;
                            if (!m_foodCustomerDict.TryGetValue(food, out cl) || !cl.Contains(customer))
                            {
                                valid = false;
                                break;
                            }
                        }
                        if (!valid)
                        {
                            customerList.RemoveAt(i);
                        }
                    }
                }
                Utils.ShowCustomerList(customerList, tree_level_customer);
            }
        }

        protected void OnBtnClearDataClicked(object sender, EventArgs e)
        {
            ClearData();
        }

        protected void OnBtnAddOrderClicked(object sender, EventArgs e)
        {
            TreePath[] paths = tree_level_foods.Selection.GetSelectedRows();
            if (paths != null && paths.Length > 0)
            {
                List<string> foodList = new List<string>();
                TreeModel model = tree_level_foods.Model;
                foreach (TreePath path in paths)
                {
                    TreeIter iter;
                    if (model.GetIter(out iter, path))
                    {
                        foodList.Add((string)model.GetValue(iter, 2));
                    }
                }

                model = tree_level_customer.Model;
                paths = tree_level_customer.Selection.GetSelectedRows();
                if (paths != null && paths.Length > 0)
                {
                    TreeIter iter;
                    List<CustomerOrder> orderList = new List<CustomerOrder>();
                    foreach (TreePath path in paths)
                    {
                        if (model.GetIter(out iter, path))
                        {
                            CustomerOrder order = new CustomerOrder();
                            order.weight = 0.1f;
                            order.customer = (string)model.GetValue(iter, 2);
                            order.foods = foodList;
                            orderList.Add(order);
                        }
                    }
                    AppendOrderList(orderList, false);
                }
            }
        }

        protected void OnBtnRemoveOrderClicked(object sender, EventArgs e)
        {
            TreeIter iter;
            if (tree_order_list.Selection.GetSelected(out iter))
            {
                TreePath path = m_orderListStore.GetPath(iter);
                if (m_orderListStore.Remove(ref iter))
                {
                    if (m_orderListStore.GetIter(out iter, path))
                    {
                        tree_order_list.Selection.SelectIter(iter);
                    }
                }
            }
        }

        protected void SwapCustomerOrder(ref TreeIter iter1, ref TreeIter iter2)
        {
            Pixbuf custIcon1 = (Pixbuf)m_orderListStore.GetValue(iter1, (int)OrderSeq.customerIcon);
            Pixbuf foodIcon1 = (Pixbuf)m_orderListStore.GetValue(iter1, (int)OrderSeq.foodIcon);
            float w1 = float.Parse((string)m_orderListStore.GetValue(iter1, (int)OrderSeq.weiget));
            int interval_min1 = (int)m_orderListStore.GetValue(iter1, (int)OrderSeq.minInterval);
            int interval_max1 = (int)m_orderListStore.GetValue(iter1, (int)OrderSeq.maxInterval);
            string randomFood1 = (string)m_orderListStore.GetValue(iter1, (int)OrderSeq.randomFood); 
            int lc1 = (int)m_orderListStore.GetValue(iter1, (int)OrderSeq.lastestCome);
            bool guide1 = (bool)m_orderListStore.GetValue(iter1, (int)OrderSeq.guide);
            string customer1 = (string)m_orderListStore.GetValue(iter1, (int)OrderSeq.customerKey);
            List<string> foods1 = (List<string>)m_orderListStore.GetValue(iter1, (int)OrderSeq.foods);

            Pixbuf custIcon2 = (Pixbuf)m_orderListStore.GetValue(iter2, (int)OrderSeq.customerIcon);
            Pixbuf foodIcon2 = (Pixbuf)m_orderListStore.GetValue(iter2, (int)OrderSeq.foodIcon);
            float w2 = float.Parse((string)m_orderListStore.GetValue(iter2, (int)OrderSeq.weiget));
            int interval_min2 = (int)m_orderListStore.GetValue(iter2, (int)OrderSeq.minInterval);
            int interval_max2 = (int)m_orderListStore.GetValue(iter2, (int)OrderSeq.maxInterval);
            string randomFood2 = (string)m_orderListStore.GetValue(iter2, (int)OrderSeq.randomFood); 
            int lc2 = (int)m_orderListStore.GetValue(iter2, (int)OrderSeq.lastestCome);
            bool guide2 = (bool)m_orderListStore.GetValue(iter2, (int)OrderSeq.guide);
            string customer2 = (string)m_orderListStore.GetValue(iter2, (int)OrderSeq.customerKey);
            List<string> foods2 = (List<string>)m_orderListStore.GetValue(iter2, (int)OrderSeq.foods);

            m_orderListStore.SetValues(iter1, custIcon2, foodIcon2, w2.ToString(), interval_min2, interval_max2, randomFood2, lc2, guide2, customer2, foods2);
            m_orderListStore.SetValues(iter2, custIcon1, foodIcon1, w1.ToString(), interval_min1, interval_max1, randomFood1, lc1, guide1, customer1, foods1);
        }

        protected void OnButtonOrderUpClicked(object sender, EventArgs e)
        {
            TreeIter iter;
            TreeIter it;
            if (tree_order_list.Selection.GetSelected(out iter) && m_orderListStore.GetIterFirst(out it) && !iter.Equals(it))
            {
                TreeIter upIter = it;
                while (!it.Equals(iter))
                {
                    upIter = it;
                    m_orderListStore.IterNext(ref it);
                }
                SwapCustomerOrder(ref iter, ref upIter);
                tree_order_list.Selection.SelectIter(upIter);
            }
        }

        protected void OnButtonOrderDownClicked(object sender, EventArgs e)
        {
            TreeIter iter;
            if (tree_order_list.Selection.GetSelected(out iter))
            {
                TreeIter lowerIter = iter;
                if (m_orderListStore.IterNext(ref lowerIter))
                {
                    SwapCustomerOrder(ref iter, ref lowerIter);
                    tree_order_list.Selection.SelectIter(lowerIter);
                }
            }
        }

        protected void OnButtonNormalizeWeightClicked(object sender, EventArgs e)
        {
            float totalWeight = 0;
            TreeIter iter;
            if (m_orderListStore.GetIterFirst(out iter))
            {
                do
                {
                    float weight;
                    if (float.TryParse((string)m_orderListStore.GetValue(iter, 2), out weight) && weight > 1e-6)
                    {
                        totalWeight += weight;
                    }
                } while (m_orderListStore.IterNext(ref iter));

                m_orderListStore.GetIterFirst(out iter);
                do
                {
                    float weight;
                    if (float.TryParse((string)m_orderListStore.GetValue(iter, 2), out weight) && weight > 1e-6)
                    {
                        weight = weight / totalWeight;
                        m_orderListStore.SetValue(iter, 2, string.Format("{0:F2}", weight));
                    }
                } while (m_orderListStore.IterNext(ref iter));
            }
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            if (ReadData())
            {
                bool isNew = true;
                for (int i = 0; i < m_allLevelData.Count; ++i)
                {
                    if (m_allLevelData[i].id == m_curLevelId)
                    {
                        m_allLevelData[i] = m_curLevelData;
                        isNew = false;
                        break;
                    }
                }
                if (isNew)
                {
                    m_allLevelData.Add(m_curLevelData);
                }
                _lvlMgr.SetMapLevelDatas(m_curRestData.key, m_allLevelData);
                try{
                    _lvlMgr.saveLevelData(m_curLevelData);
                }catch(Exception err)
                {
                    Console.WriteLine("Save LevelData Error:" + err.ToString());
                }

                //string jsonStr = _lvlMgr.GetJsonString(m_curRestData.key);
                //string path = m_levelFileDir + m_curRestData.key + ".json";
                //File.WriteAllText(path, jsonStr);

                MessageDialog dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Close,
                                                      "保存成功！");
                dlg.Run();
                dlg.Destroy();
            }
        }

        protected void OnBtnConfigSecretCustomersClicked(object sender, EventArgs e)
        {
            List<string> customerList = new List<string>();
            TreeIter iter;
            if (m_orderListStore.GetIterFirst(out iter))
            {
                do
                {
                    string customer = (string)m_orderListStore.GetValue(iter, (int)OrderSeq.customerKey);
                    customerList.Add(customer);
                } while (m_orderListStore.IterNext(ref iter));
            }

            List<SecretCustomer> selectedCustomers = new List<SecretCustomer>();
            string secText = text_secret_customers.Text;
            if (!string.IsNullOrEmpty(secText))
            {
                string[] secList = secText.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach(string element in secList)
                {
                    SecretCustomer secretCustomer = new SecretCustomer();
                    if (secText.Contains("<"))
                    {
                        int index = element.IndexOf('<');
                        secretCustomer.customer = element.Substring(0, index);
                        string showOrder = element.Substring(index + 1, element.Length - index - 2);
                        secretCustomer.showOrder = showOrder.ToInt32();
                    }
                    else
                    {
                        secretCustomer.customer = element;
                    }
                    selectedCustomers.Add(secretCustomer);
                }
            }
            SecretCustomerEditDialog dlg = new SecretCustomerEditDialog(this, customerList, selectedCustomers);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                selectedCustomers = dlg.SecretCustomers;
                m_curLevelData.secret_customers = selectedCustomers;
                if (selectedCustomers != null && selectedCustomers.Count > 0)
                {
                    string text = "";
                    var it = selectedCustomers.GetEnumerator();
                    bool hasNext = it.MoveNext();
                    while(hasNext)
                    {
                        SecretCustomer secCus = it.Current;
                        text += secCus.customer;
                        if (secCus.showOrder > 0){
                            text += string.Format("<{0}>", secCus.showOrder);
                        }
                        hasNext = it.MoveNext();
                        if (hasNext)
                        {
                            text += ";";
                        }
                    }
                    text_secret_customers.Text = text;
                }
                else
                {
                    text_secret_customers.Text = "";
                }
            }
            dlg.Destroy();
        }

        protected void OnBtnConfigLevelRequirementsClicked(object sender, EventArgs args)
        {
            List<string> customerList = new List<string>(m_curRestData.customer_list);
            customerList.AddRange(m_curRestData.special_customer_list);
            RequirementEditDialog dlg = new RequirementEditDialog(this, customerList, m_curRestData.food_list, m_curLevelData.requirements);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                Requirements requirements = dlg.Requirements;
                if (requirements != null)
                {
                    m_curLevelData.requirements = requirements;
                    text_level_requirements.Text = JsonConvert.SerializeObject(requirements);
                }
            }
            dlg.Destroy();
        }

        protected void OnBtnConfigLevelTipsClicked(object sender, EventArgs args)
        {
            FailTipData failTips = null;
            string jsonStr = text_level_tips.Buffer.Text;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                try
                {
                    failTips = JsonConvert.DeserializeObject<FailTipData>(jsonStr);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            FailTipsEditDialog dlg = new FailTipsEditDialog(this, failTips);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                failTips = dlg.FailTips;
                if (failTips != null)
                {
                    m_curLevelData.fail_tips = failTips;
                    text_level_tips.Buffer.Text = JsonConvert.SerializeObject(failTips);
                }
            }
            dlg.Destroy();
        }

        protected void OnButtonEditUnlockItemsClicked(object sender, EventArgs e)
        {
            List<int> unlockItems = null;
            string unlockStrs = text_unlock_item.Text;
            if (!string.IsNullOrEmpty(unlockStrs))
            {
                var unlockStrList = unlockStrs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                ConvertList.List_String2Int(unlockStrList, out unlockItems);
            }
            UnlockItemsDialog dlg = new UnlockItemsDialog(this, unlockItems);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                unlockItems = dlg.UnlockItems;
                if (unlockItems != null)
                {
                    text_unlock_item.Text = ConvertList.List2String<int>(unlockItems, ',');
                }
            }
            dlg.Destroy();
        }

        protected void OnButtonEditRewardsClicked(object sender, EventArgs e)
        {
            List<RewardData> rewardDatas = null;
            string jsonStr = text_level_rewards.Buffer.Text;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                rewardDatas = JsonConvert.DeserializeObject<List<RewardData>>(jsonStr);
            }
            LevelRewardsDialog dlg = new LevelRewardsDialog(this, rewardDatas);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                rewardDatas = dlg.Rewards;
                if (rewardDatas != null)
                {
                    text_level_rewards.Buffer.Text = JsonConvert.SerializeObject(rewardDatas);
                }
            }
            dlg.Destroy();
        }

        protected void OnButtonEditAutogenFoodListClicked(object sender, EventArgs e)
        {
            Dictionary<string, float> foodWeights = null;
            string jsonStr = text_autogen_food_list.Text;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                foodWeights = JsonConvert.DeserializeObject<Dictionary<string, float>>(jsonStr);
            }
            FoodWeightSelectDialog dlg = new FoodWeightSelectDialog(this, m_curRestData.food_list, foodWeights);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                foodWeights = dlg.FoodWeights;
                if (foodWeights != null && foodWeights.Count > 0)
                {
                    text_autogen_food_list.Text = JsonConvert.SerializeObject(foodWeights);
                }
                else
                {
                    text_autogen_food_list.Text = "";
                }
            }
            dlg.Destroy();
        }

        protected void OnButtonEditAutogenCustomerListClicked(object sender, EventArgs e)
        {
            List<string> customerList = new List<string>(m_curRestData.customer_list);
            customerList.AddRange(m_curRestData.special_customer_list);

            List<string> selCustomers = null;
            string jsonStr = text_auto_customer_list.Text;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                selCustomers = JsonConvert.DeserializeObject<List<string>>(jsonStr);
            }

            CustomerSelectDialog dlg = new CustomerSelectDialog(this, customerList, selCustomers);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                selCustomers = dlg.SelectedCustomers;

                if (selCustomers != null && selCustomers.Count > 0)
                {
                    text_auto_customer_list.Text = JsonConvert.SerializeObject(selCustomers);
                }
                else
                {
                    text_auto_customer_list.Text = "";
                }
            }
            dlg.Destroy();
        }

        protected AutoGenConfig GetAutogenConfig(out string errMsg)
        {
            errMsg = null;
            AutoGenConfig autoGenConfig = new AutoGenConfig();
            do
            {
                autoGenConfig.type = (LevelType)(combobox_config_type.Active + 1);
                autoGenConfig.difficulty = combobox_autogen_difficulty.Active + 1;
                if (!int.TryParse(text_config_total.Text, out autoGenConfig.total) || autoGenConfig.total <= 0)
                {
                    errMsg = "输入的关卡参数值有误！";
                    break;
                }

                if (!int.TryParse(text_config_max_order.Text, out autoGenConfig.maxOrder) || autoGenConfig.maxOrder <= 0)
                {
                    errMsg = "输入的最大订单数有误！";
                    break;
                }

                if (!int.TryParse(text_autogen_specialnum.Text, out autoGenConfig.specialNum) || autoGenConfig.specialNum < 0)
                {
                    errMsg = "输入的特殊顾客数量有误！";
                    break;
                }

                if (!int.TryParse(text_config_full_patience.Text, out autoGenConfig.fullPatienceNum) || autoGenConfig.fullPatienceNum < 0)
                {
                    errMsg = "满耐心值有误！";
                    break;
                }

                string[] strArr = text_config_first_arrivals.Text.Split(',');
                if (strArr.Length != autoGenConfig.maxOrder)
                {
                    errMsg = "输入的首次来客时间个数与最大订单数不符";
                    break;
                }
                for (int i = 0; i < strArr.Length; ++i)
                {
                    float time;
                    if (!float.TryParse(strArr[i], out time))
                    {
                        errMsg = "输入的首次来客时间有误！";
                        break;
                    }
                    autoGenConfig.firstArrivals.Add(time);
                }
                if (errMsg != null)
                {
                    break;
                }

                float fNum;
                if (!float.TryParse(text_config_order_interval_start.Text, out fNum) || fNum < 1e-6)
                {
                    errMsg = "请输入正确的最小订单间隔时间";
                    break;
                }
                autoGenConfig.orderInterval.min = fNum;

                if (!float.TryParse(text_config_order_interval_end.Text, out fNum) || fNum < 1e-6)
                {
                    errMsg = "请输入正确的最大订单间隔时间";
                    break;
                }
                autoGenConfig.orderInterval.max = fNum;


                float decay_intv, decay_rate, decay_limit;
                if (!float.TryParse(text_waitdecay_interval.Text, out decay_intv)
                    || !float.TryParse(text_waitdecay_rate.Text, out decay_rate)
                    || !float.TryParse(text_waitdecay_limit.Text, out decay_limit))
                {
                    errMsg = "请输入正确的等待时间衰减配置";
                    break;
                }
                autoGenConfig.waitDecay.interval = decay_intv;
                autoGenConfig.waitDecay.rate = decay_rate;
                autoGenConfig.waitDecay.limit = decay_limit;

                //if (!float.TryParse(text_burndecay_interval.Text, out decay_intv)
                //    || !float.TryParse(text_burndecay_rate.Text, out decay_rate)
                //    || !float.TryParse(text_burndecay_limit.Text, out decay_limit))
                //{
                //    errMsg = "请输入正确的烧焦时间衰减配置";
                //    break;
                //}
                //autoGenConfig.burnDecay.interval = decay_intv;
                //autoGenConfig.burnDecay.rate = decay_rate;
                //autoGenConfig.burnDecay.limit = decay_limit;

                if (!float.TryParse(text_orderdecay_interval.Text, out decay_intv)
                    || !float.TryParse(text_orderdecay_rate.Text, out decay_rate)
                    || !float.TryParse(text_orderdecay_limit.Text, out decay_limit))
                {
                    errMsg = "请输入正确的来客时间衰减配置";
                    break;
                }
                autoGenConfig.orderDecay.interval = decay_intv;
                autoGenConfig.orderDecay.rate = decay_rate;
                autoGenConfig.orderDecay.limit = decay_limit;

                if (!float.TryParse(text_cookingdecay_interval.Text, out decay_intv)
                    || !float.TryParse(text_cookingdecay_rate.Text, out decay_rate)
                    || !float.TryParse(text_cookingdecay_limit.Text, out decay_limit))
                {
                    errMsg = "请输入正确的制作时间衰减配置";
                    break;
                }
                autoGenConfig.cookingDecay.interval = decay_intv;
                autoGenConfig.cookingDecay.rate = decay_rate;
                autoGenConfig.cookingDecay.limit = decay_limit;

                int min, max;
                if (!int.TryParse(text_litterinterval_start.Text, out min)
                    || !int.TryParse(text_litterinterval_end.Text, out max))
                {
                    errMsg = "请输入正确的扔垃圾间隔";
                    break;
                }
                autoGenConfig.litterInterval.min = min;
                autoGenConfig.litterInterval.max = max;

                if (!int.TryParse(text_brokeninterval_start.Text, out min)
                    || !int.TryParse(text_brokeninterval_end.Text, out max))
                {
                    errMsg = "请输入正确的厨具损坏间隔";
                    break;
                }
                autoGenConfig.brokenInterval.min = min;
                autoGenConfig.brokenInterval.max = max;

                string jsonStr = text_autogen_food_list.Text;
                if (!string.IsNullOrEmpty(jsonStr))
                {
                    autoGenConfig.foodWeightList = JsonConvert.DeserializeObject<Dictionary<string, float>>(jsonStr);
                }
                if (autoGenConfig.foodWeightList.Count == 0)
                {
                    errMsg = "请指定食物集";
                    break;
                }

                jsonStr = text_auto_customer_list.Text;
                if (!string.IsNullOrEmpty(jsonStr))
                {
                    autoGenConfig.customerList = JsonConvert.DeserializeObject<List<string>>(jsonStr);
                }
                if (autoGenConfig.customerList.Count == 0)
                {
                    errMsg = "请指定顾客集";
                    break;
                }

                autoGenConfig.random_customer_requirement = check_autogen_random_customer.Active;
                autoGenConfig.random_food_requirement = check_autogen_random_food.Active;
                autoGenConfig.random_smile_requirement = check_autogen_random_smile.Active;
                autoGenConfig.any_customer_requirement = check_autogen_any_customer.Active;


            } while (false);

            if (errMsg != null)
            {
                return null;
            }
            return autoGenConfig;
        }

        protected void OnBtnAutogenGenerateClicked(object sender, EventArgs e)
        {
            string errMsg;

            AutoGenConfig autoGenConfig = GetAutogenConfig(out errMsg);
            if (errMsg == null)
            {
                LevelDataGenerator generator = new LevelDataGenerator();
                LevelData lvlData = generator.GenerateLevelData(autoGenConfig);
                if (lvlData != null)
                {
                    m_curLevelData = lvlData;
                    ShowData();
                }
            }
            else
            {
                MessageDialog dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close,
                                                  errMsg);
                dlg.Run();
                dlg.Destroy();
            }
        }

        protected void OnButtonAutogenRequirementsClicked(object sender, EventArgs e)
        {
            string errMsg;
            AutoGenConfig autoGenConfig = GetAutogenConfig(out errMsg);
            if (errMsg == null)
            {
                LevelDataGenerator generator = new LevelDataGenerator();
                List<string> scoreList;
                Requirements requirements;
                if (generator.GenerateRequirements(m_curLevelData.orders, autoGenConfig, out scoreList, out requirements))
                {
                    m_curLevelData.requirements = requirements;
                    m_curLevelData.scoreList = scoreList;
                    ShowData();
                }
            }
            else
            {
                MessageDialog dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close,
                                                  errMsg);
                dlg.Run();
                dlg.Destroy();
            }
        }
    }
}
