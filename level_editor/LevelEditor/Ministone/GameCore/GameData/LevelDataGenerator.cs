using System;
using System.Collections.Generic;


namespace Ministone.GameCore.GameData
{
    public class LevelDataGenerator
    {
        FoodDataManager _foodMgr = FoodDataManager.GetInstance();
        CustomerDataManager _custMgr = CustomerDataManager.GetInstance();
        OrderDataManager _orderMgr = OrderDataManager.GetInstance();
        Random _random = new Random();

        static int MIN_REQUIRE_FOOD_NUM = 4;

        public LevelDataGenerator()
        {
        }

        public LevelData GenerateLevelData(AutoGenConfig config)
        {
            LevelData lvlData = new LevelData();

            lvlData.type = config.type;
            lvlData.total = config.total;
            lvlData.max_order = config.maxOrder;
            lvlData.first_arrivals = config.firstArrivals;
            lvlData.full_patience_num = config.fullPatienceNum;
            lvlData.order_interval = config.orderInterval;
            lvlData.litter_interval = config.litterInterval;
            lvlData.broken_interval = config.brokenInterval;
            lvlData.waiting_decay = config.waitDecay;
            lvlData.cooking_decay = config.cookingDecay;
            lvlData.burn_decay = config.burnDecay;
            lvlData.order_decay = config.orderDecay;

            List<CustomerOrder> orders = GenerateOrders(config.foodWeightList, config.customerList);
            int score;
            Requirements requirements;
            if (GenerateRequirements(orders, config, out score, out requirements))
            {
                lvlData.score = score;
                lvlData.requirements = requirements;
            }
            lvlData.orders = orders;
            return lvlData;
        }

        protected List<CustomerOrder> GenerateOrders(Dictionary<string, float> foodWeights, List<string> customers)
        {
            List<CustomerOrder> orders = new List<CustomerOrder>();

            foreach (var item in foodWeights)
            {
                float totalWeight = item.Value;
                string food = item.Key;

                List<OrderData> validOrders = _orderMgr.GetFoodOrders(food);

                for (int i = validOrders.Count - 1; i >= 0; -- i)
                {
                    OrderData ord = validOrders[i];
                    if (!customers.Contains(ord.customer))
                    {
                        validOrders.RemoveAt(i);
                    }
                }

                int max = validOrders.Count > 4 ? 4 : validOrders.Count;
                int min = (int)Math.Ceiling(0.5f * max);
                int count = _random.Next(min, max);
                float sum = 0;
                float[] orderWeights = new float[count];
                for (int i = 0; i < count; ++ i)
                {
                    float w = (float)(0.2 + _random.NextDouble() * 0.8);
                    sum += w;
                    orderWeights[i] = w;
                }

                for (int i = 0; i < count; ++ i)
                {
                    int index = _random.Next(0, validOrders.Count - 1);
                    OrderData ord = validOrders[index];

                    CustomerOrder cord = new CustomerOrder();
                    cord.customer = ord.customer;
                    cord.foods.Add(ord.food);
                    cord.weight = totalWeight * orderWeights[i] / sum;
                    orders.Add(cord);
                    validOrders.RemoveAt(index);
                }
            }

            return orders;
        }

        public bool GenerateRequirements(List<CustomerOrder> orders, AutoGenConfig config, out int score, out Requirements requirements)
        {
            int estimateCustomerCount;
            score = EstimateTargetScore(orders, config, out estimateCustomerCount);
            requirements = CalcRequirements(orders, config, estimateCustomerCount);
            return true;
        }

        protected float CalcOrderTime(CustomerOrder order)
        {
            float time = 0;
            foreach (string food in order.foods)
            {
                OrderData ord = _orderMgr.GetOrder(order.customer, food);
                time += ord.consider_time;
                FoodData fd = _foodMgr.GetFood(food);
                time += fd.cook_time;
            }
            return time;
        }

        protected float CalcFoodCookTime(string food)
        {
            float cookTime = 0;
            FoodData fd = _foodMgr.GetFood(food);
            if (fd != null)
            {
                foreach (CookingStep step in fd.cooking_step_list)
                {
                    if (step.cookwareType == 0 || step.canIgnore)
                    {
                        continue;
                    }
                    cookTime += step.cookTime;
                }
            }
            return cookTime;
        }

        protected float CalcIncome(CustomerOrder order, out float waitTime)
        {
            float income = 0;
            waitTime = 0;
            foreach (string food in order.foods)
            {
                FoodData fd = _foodMgr.GetFood(food);
                income += fd.suggested_price;

                OrderData ord = _orderMgr.GetOrder(order.customer, food);
                income += ord.tip;
                waitTime += ord.wait_time;
            }

            return income;
        }

        protected int EstimateTargetScore(List<CustomerOrder> orders, AutoGenConfig config, out int estimateCustomerCount)
        {
            int score = 0;
            estimateCustomerCount = 0;

            float totalprice = 0;
            float multiple = 1;
            switch (config.difficulty)
            {
                case 1:
                    multiple = 1.3f;
                    break;
                case 2:
                    multiple = 1.5f;
                    break;
                case 3:
                    multiple = 1.8f;
                    break;
                case 4:
                    multiple = 2.5f;
                    break;
                case 5:
                    multiple = 3.5f;
                    break;
                default:
                    break;
            }

            if (config.type == LevelType.FIXED_TIME)
            {
                // 简单利用加权平均制作时间乘以加权平均价格来计算
                float totaltime = 0;
                foreach (CustomerOrder ord in orders)
                {
                    float waittime;
                    totaltime += CalcOrderTime(ord) * ord.weight;
                    totalprice += CalcIncome(ord, out waittime) * ord.weight;
                }
                float lastFirstArriveTime = 0;
                if (config.firstArrivals.Count > 0)
                {
                    lastFirstArriveTime = config.firstArrivals[config.firstArrivals.Count - 1];
                }

                estimateCustomerCount = (int)(Math.Ceiling(((float)config.total - lastFirstArriveTime) / totaltime) + config.maxOrder * multiple);
            }
            else if (config.type == LevelType.FIXED_CUSTOMER)
            {
                foreach (CustomerOrder ord in orders)
                {
                    float waittime;
                    totalprice += CalcIncome(ord, out waittime) * ord.weight;
                }
                estimateCustomerCount = config.total;
            }
            else if (config.type == LevelType.LOST_CUSTOMER)
            {
                // 估算极限时间
                float totaltime = 0;
                float total_waittime = 0;
                foreach (CustomerOrder ord in orders)
                {
                    float waittime;
                    totaltime += CalcOrderTime(ord) * ord.weight;
                    totalprice += CalcIncome(ord, out waittime) * ord.weight;
                    total_waittime += waittime * ord.weight;
                }

                totaltime *= 1.3f + (5 - config.difficulty) * 0.1f;

                int num = 0;
                float decay = 1.0f;
                while (decay * total_waittime > totaltime)
                {
                    num += (int)config.waitDecay.interval;
                    decay *= config.waitDecay.rate;
                }
                estimateCustomerCount = num;
                // 多一个流失机会则多加一轮
                estimateCustomerCount += (int)((config.total - 1) * config.maxOrder * (multiple * 0.5f));
            }
            else
            {
                System.Diagnostics.Debug.Assert(false, "无效的关卡类型！");
            }

            float score_ratio = 1.0f + (config.difficulty - 1) * 0.05f;
            score = (int)(totalprice * estimateCustomerCount * score_ratio / 10) * 10;

            float scoreAmp = 1.0f + (0.05f * config.difficulty) + ((float)_random.NextDouble() * 0.05f + 0.1f) * config.difficulty;
            int score2 = (int)(score * scoreAmp / 10) * 10;
            int score3 = (int)(score2 * scoreAmp / 10) * 10;

            return score;
        }

        protected Requirements CalcRequirements(List<CustomerOrder> orders, AutoGenConfig config, int customerCount)
        {
            Requirements requirements = new Requirements();
            int diffculty = config.difficulty;
            if (diffculty > 3)
            {
                diffculty = 3;
            }

            int additionFoodNum = diffculty;
            if (config.difficulty >= 3 && config.waitDecay.interval > 0)
            {
                float ratio = 1.0f + (config.difficulty - 1) * 0.15f;
                additionFoodNum = (int)Math.Ceiling(additionFoodNum * ratio);
            }

            foreach (CustomerOrder ord in orders)
            {
                ord.latestFirstCome = 0;
            }

            int additionCustNum = config.difficulty == 1 ? 1 : (int)Math.Ceiling((config.difficulty - 1) * 1.5f);

            // 生成指定食物
            if (config.random_food_requirement)
            {
                List<KeyValuePair<string, float>> sortedFoodWeights = new List<KeyValuePair<string, float>>();
                foreach (var item in config.foodWeightList)
                {
                    sortedFoodWeights.Add(item);
                }
                sortedFoodWeights.Sort((x, y) =>
                {
                    return y.Value.CompareTo(x.Value);
                });

                // 随机取概率排前两位的食物
                int index = 0;
                int max = sortedFoodWeights.Count;

                if (max > 2)
                {
                    max = 2;
                }
                index = _random.Next(0, max - 1);
                string requiredFood = sortedFoodWeights[index].Key;

                // 动态计算食物个数
                int specialCount = 0;
                foreach (CustomerOrder ord in orders)
                {
                    if (ord.foods.Contains(requiredFood) && ord.interval.min > 0)
                    {
                        specialCount += customerCount / ((ord.interval.max + ord.interval.min) / 2);
                    }
                }
                float weight;
                config.foodWeightList.TryGetValue(requiredFood, out weight);
                int requireNum = (int)Math.Ceiling(weight * (customerCount - specialCount)) + specialCount + additionFoodNum;

                if (requireNum < MIN_REQUIRE_FOOD_NUM)
                {
                    requireNum = MIN_REQUIRE_FOOD_NUM;
                }

                if (config.type == LevelType.FIXED_TIME)
                {
                    float cookTime = CalcFoodCookTime(requiredFood);
                    if (cookTime > 1e-6)
                    {
                        max = (int)Math.Ceiling(config.total / cookTime);
                        if (requireNum > max)
                        {
                            requireNum = max;
                        }
                    }
                }

                Requirements.NameAndNumber req = new Requirements.NameAndNumber();
                req.name = requiredFood;
                req.number = requireNum;
                requirements.requiredFoods.Add(req);

                int maxWeightIndex = -1;
                for (int i = 0; i < orders.Count; ++ i)
                {
                    if (orders[i].foods.Contains(requiredFood))
                    {
                        if (maxWeightIndex < 0)
                        {
                            maxWeightIndex = i;
                        }
                        else if (orders[i].weight > orders[maxWeightIndex].weight)
                        {
                            maxWeightIndex = i;
                        }
                    }
                }
                if (maxWeightIndex >= 0)
                {
                    orders[maxWeightIndex].latestFirstCome = (int)Math.Ceiling((float)customerCount / requireNum);
                }
            }

            // 计算服务任意顾客总数
            if (config.any_customer_requirement)
            {
                Requirements.NameAndNumber req = new Requirements.NameAndNumber();
                req.name = "anyone";
                req.number = customerCount + additionCustNum;
                requirements.requiredCustomers.Add(req);
            }

            // 生成指定顾客
            if (config.random_customer_requirement)
            {
                float weight = 0;
                // 随机选取概率排前二位的顾客
                Dictionary<string, float> customerWeights = new Dictionary<string, float>();
                foreach (CustomerOrder ord in orders)
                {
                    weight = 0;
                    customerWeights.TryGetValue(ord.customer, out weight);
                    customerWeights[ord.customer] = weight + ord.weight;
                }
                List<KeyValuePair<string, float>> sortedCustomerWeights = new List<KeyValuePair<string, float>>();
                foreach(var item in customerWeights)
                {
                    sortedCustomerWeights.Add(item);
                }
                sortedCustomerWeights.Sort((x, y) =>
                {
                    return y.Value.CompareTo(x.Value);
                });
                int index = _random.Next(0, 1);
                string requiredCustomer = sortedCustomerWeights[index].Key;

                // 动态计算个数
                int specialCount = 0;
                foreach (CustomerOrder ord in orders)
                {
                    if (ord.customer.Equals(requiredCustomer) && ord.interval.min > 0)
                    {
                        specialCount += customerCount / ((ord.interval.max + ord.interval.min) / 2);
                    }
                }

                weight = 0;
                float total_weight = 0;
                foreach (CustomerOrder ord in orders)
                {
                    if (ord.customer.Equals(requiredCustomer))
                    {
                        weight += ord.weight;
                    }
                    total_weight += ord.weight;
                }

                int min = 0;
                if (diffculty >= 3 && config.type == LevelType.LOST_CUSTOMER)
                {
                    min = (int)Math.Ceiling((float)customerCount / (float)config.maxOrder);
                }
                int requireNum = (int)Math.Ceiling(weight / total_weight * (customerCount - specialCount)) + specialCount + additionCustNum;
                if (requireNum < min)
                {
                    requireNum = min;
                }

                Requirements.NameAndNumber req = new Requirements.NameAndNumber();
                req.name = requiredCustomer;
                req.number = requireNum;
                requirements.requiredCustomers.Add(req);

                int maxWeightIndex = -1;
                for (int i = 0; i < orders.Count; ++i)
                {
                    if (orders[i].customer.Equals(requiredCustomer))
                    {
                        if (maxWeightIndex < 0)
                        {
                            maxWeightIndex = i;
                        }
                        else if (orders[i].weight > orders[maxWeightIndex].weight)
                        {
                            maxWeightIndex = i;
                        }
                    }
                }
                if (maxWeightIndex >= 0)
                {
                    orders[maxWeightIndex].latestFirstCome = (int)Math.Ceiling((float)customerCount / requireNum);
                }
            }

            // 生成指定收集笑脸个数
            if (config.random_smile_requirement && config.fullPatienceNum > 0)
            {
                float patiencePerCustomer = 4.5f;
                if (config.waitDecay.interval > 0)
                {
                    patiencePerCustomer -= 0.2f;
                }

                requirements.smileCount = (int)Math.Ceiling((float)customerCount * patiencePerCustomer / config.fullPatienceNum);
            }

            return requirements;
        }
    }
}
