using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using Ministone.GameCore.GameData;

namespace LevelEditor
{
    public partial class NewOrderDialog : Gtk.Dialog
    {
        FoodDataManager _foodMgr = FoodDataManager.GetInstance();
        CustomerDataManager _custMgr = CustomerDataManager.GetInstance();
        OrderDataManager _orderMgr = OrderDataManager.GetInstance();

        List<CustomerData> m_customerList;
        string m_curFood;
        string m_curCustomer;
        OrderData m_newOrder = null;

        public NewOrderDialog(Gtk.Window parent, string food, List<string> customerList)
        {
            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            entry_tip.Text = "1";
            entry_waittime.Text = "0";
            entry_consider_time.Text = "1";

            m_curFood = food;
            string imgPath = Utils.GetFoodImagePath(food);
            image_food.Pixbuf = new Pixbuf(imgPath).ScaleSimple(100, 100, InterpType.Bilinear);
            label_food_name.Text = _foodMgr.GetFood(food).GetDisplayName("zh-cn");

            m_customerList = new List<CustomerData>();
            for (int i = 0; i < customerList.Count; ++ i)
            {
                m_customerList.Add(_custMgr.GetCustomer(customerList[i]));
            }

            if (m_customerList.Count > 0)
            {
                for (int i = 0; i < m_customerList.Count; ++i)
                {
                    CustomerData cust = m_customerList[i];
                    string name = cust.GetDisplayName("zh-cn");
                    combobox_customer.AppendText(name);
                }

                combobox_customer.Active = 0;
                SelectCustomer(0);
            }
        }

        private void SelectCustomer(int index)
        {
            float destSize = 100;
            m_curCustomer = m_customerList[index].key;
            Pixbuf pixbuf = new Pixbuf(Utils.GetCustomerImagePath(m_curCustomer));
            float scalex = destSize / pixbuf.Width;
            float scaley = destSize / pixbuf.Height;
            if (scalex > scaley)
            {
                scalex = scaley;
            }
            image_customer.Pixbuf = pixbuf.ScaleSimple((int)(scalex * pixbuf.Width), (int)(scalex * pixbuf.Height), InterpType.Bilinear);
        }

        protected void OnComboboxCustomerChanged(object sender, EventArgs e)
        {
            SelectCustomer(combobox_customer.Active);
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            int tip;
            float waitTime, considerTime;

            if (Utils.ParseInteger(entry_tip.Text, out tip, this) && Utils.ParseFloat(entry_waittime.Text, out waitTime, this)
                && Utils.ParseFloat(entry_consider_time.Text, out considerTime, this) && m_curCustomer != null)
            {
                m_newOrder = new OrderData();
                m_newOrder.customer = m_curCustomer;
                m_newOrder.food = m_curFood;
                m_newOrder.tip = tip;
                m_newOrder.wait_time = waitTime;
                m_newOrder.consider_time = considerTime;
            }
            else
            {
                m_newOrder = null;
            }
        }

        public OrderData GetNewOrder()
        {
            return m_newOrder;
        }
    }
}
