using System;
using System.IO;
using System.Collections.Generic;
using Ministone.GameCore.GameData;
using Gtk;
using Gdk;

namespace LevelEditor
{
    public partial class OrderEditor : Gtk.Dialog
    {
        string DIR_SEPRATOR;

        FoodDataManager _foodMgr;
        CustomerDataManager _custMgr;
        OrderDataManager _orderMgr;
        List<OrderData> m_allOrders;

        List<string> m_foodList;
        List<string> m_customerList;
        string m_orderFilePath;
        string m_curFood;

        public OrderEditor(Gtk.Window parent, List<string> foodList, List<string> customerList, string orderFilePath)
        {
            DIR_SEPRATOR = System.IO.Path.DirectorySeparatorChar.ToString();
            _foodMgr = FoodDataManager.GetInstance();
            _custMgr = CustomerDataManager.GetInstance();
            _orderMgr = OrderDataManager.GetInstance();
            m_allOrders = _orderMgr.GetAllOrders();

            m_foodList = foodList;
            m_customerList = customerList;
            m_orderFilePath = orderFilePath;

            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            treeview_foodlist.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_foodlist.AppendColumn("名称", new CellRendererText(), "text", 1);

            treeview_orderlist.AppendColumn("顾客", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_orderlist.AppendColumn("食物", new CellRendererPixbuf(), "pixbuf", 1);

            CellRendererText cellWaitTime = new CellRendererText();
            cellWaitTime.Editable = true;
            cellWaitTime.Edited += OnEditedWaitTime;
            treeview_orderlist.AppendColumn("等待时间", cellWaitTime, "text", 2);

            CellRendererText cellTip = new CellRendererText();
            cellTip.Editable = true;
            cellTip.Edited += OnEditedTip;
            cellTip.Width = 50;
            treeview_orderlist.AppendColumn("小费", cellTip, "text", 3);

            CellRendererText cellConsiderTime = new CellRendererText();
            cellConsiderTime.Editable = true;
            cellConsiderTime.Edited += OnEditedConsiderTime;
            treeview_orderlist.AppendColumn("考虑时间", cellConsiderTime, "text", 4);
            //treeview_orderlist.AppendColumn("索引", new CellRendererText(), "text", 5);

            ReloadData();
        }

        void ReloadData()
        {
            Utils.ShowFoodList(m_foodList, treeview_foodlist);
            Utils.SelectTreeRow(treeview_foodlist, 0);
            m_curFood = m_foodList[0];

            ShowFoodOrders(m_curFood);
        }

        void ShowFoodOrders(string food)
        {
            float iconSize = 50.0f;
            ListStore orderListStore = new ListStore(typeof(Pixbuf), typeof(Pixbuf),
                                                     typeof(string), typeof(string), typeof(string), typeof(int));
            for (int i = 0; i < m_allOrders.Count; ++i)
            {
                OrderData ord = m_allOrders[i];
                // 只显示本卡中出现的顾客
                if (ord.food.Equals(food) && m_customerList.Contains(ord.customer))
                {
                    FoodData fd = _foodMgr.GetFood(ord.food);
                    CustomerData cust = _custMgr.GetCustomer(ord.customer);

                    Pixbuf pixBuf = new Pixbuf(Utils.GetFoodImagePath(ord.food));
                    float scalex = iconSize / pixBuf.Width;
                    float scaley = iconSize / pixBuf.Height;
                    if (scalex > scaley)
                    {
                        scalex = scaley;
                    }

                    Pixbuf foodPixBuf = pixBuf.ScaleSimple((int)(pixBuf.Width * scalex),
                                                              (int)(pixBuf.Height * scalex), InterpType.Bilinear);

                    pixBuf = new Pixbuf(Utils.GetCustomerImagePath(ord.customer));
                    scalex = iconSize / pixBuf.Width;
                    scaley = iconSize / pixBuf.Height;
                    if (scalex > scaley)
                    {
                        scalex = scaley;
                    }

                    Pixbuf customerPixBuf = pixBuf.ScaleSimple((int)(pixBuf.Width * scalex),
                                                               (int)(pixBuf.Height * scalex), InterpType.Bilinear);
                    orderListStore.AppendValues(customerPixBuf, foodPixBuf, ord.wait_time.ToString(),
                                                ord.tip.ToString(), ord.consider_time.ToString(), i);
                }
            }
            treeview_orderlist.Model = orderListStore;
            Utils.SelectTreeRow(treeview_orderlist, 0);
        }

        void OnEditedWaitTime(object o, EditedArgs args)
        {
            TreeIter iter;
            treeview_orderlist.Model.GetIter(out iter, new TreePath(args.Path));
            int index = (int)treeview_orderlist.Model.GetValue(iter, 5);

            float waitTime;
            if (!Utils.ParseFloat(args.NewText, out waitTime, this))
            {
                return;
            }
            m_allOrders[index].wait_time = waitTime;
            treeview_orderlist.Model.SetValue(iter, 2, args.NewText);
        }

        void OnEditedTip(object o, EditedArgs args)
        {
            TreeIter iter;
            treeview_orderlist.Model.GetIter(out iter, new TreePath(args.Path));
            int index = (int)treeview_orderlist.Model.GetValue(iter, 5);

            int tip;
            if (!Utils.ParseInteger(args.NewText, out tip, this))
            {
                return;
            }
            m_allOrders[index].tip = tip;
            treeview_orderlist.Model.SetValue(iter, 3, args.NewText);
        }

        void OnEditedConsiderTime(object o, EditedArgs args)
        {
            TreeIter iter;
            treeview_orderlist.Model.GetIter(out iter, new TreePath(args.Path));
            int index = (int)treeview_orderlist.Model.GetValue(iter, 5);

            float considerTime;
            if (!Utils.ParseFloat(args.NewText, out considerTime, this))
            {
                return;
            }
            m_allOrders[index].consider_time = considerTime;
            treeview_orderlist.Model.SetValue(iter, 4, args.NewText);
        }

        protected void OnTreeviewFoodlistCursorChanged(object sender, EventArgs e)
        {
            TreeIter iter;
            TreeModel model;
            if (treeview_foodlist.Selection.GetSelected(out model, out iter))
            {
                int index = model.GetPath(iter).Indices[0];
                string food = m_foodList[index];
                ShowFoodOrders(food);
                m_curFood = food;
            }
        }

        protected void OnButtonSaveClicked(object sender, EventArgs e)
        {
            _orderMgr.SetAllOrders(m_allOrders);
            string jsonStr = _orderMgr.GetJsonString();
            File.WriteAllText(m_orderFilePath, jsonStr);
        }

        protected void OnBtnRemoveOrderClicked(object sender, EventArgs e)
        {
            TreeIter iter;
            TreeModel model;
            if (treeview_orderlist.Selection.GetSelected(out model, out iter))
            {
                int index = (int)model.GetValue(iter, 5);
                m_allOrders.RemoveAt(index);
                ShowFoodOrders(m_curFood);
            }
        }

        protected void OnBtnAddOrderClicked(object sender, EventArgs e)
        {
            List<string> exsitCustomers = new List<string>();
            for (int i = 0; i < m_allOrders.Count; ++ i)
            {
                OrderData ord = m_allOrders[i];
                if (ord.food.Equals(m_curFood))
                {
                    exsitCustomers.Add(ord.customer);
                }
            }
            List<string> newCustomers = new List<string>();
            for (int i = 0; i < m_customerList.Count; ++ i)
            {
                string customer = m_customerList[i];
                if (!exsitCustomers.Contains(customer))
                {
                    newCustomers.Add(customer);
                }
            }

            NewOrderDialog dlg = new NewOrderDialog(this, m_curFood, newCustomers);
            dlg.Run();
            OrderData newOrd = dlg.GetNewOrder();
            if (newOrd != null)
            {
                m_allOrders.Add(newOrd);
                ShowFoodOrders(m_curFood);
            }
            dlg.Destroy();
        }
    }
}
