using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Gtk;
using Ministone.GameCore.GameData;

namespace LevelEditor
{
    public partial class RequirementEditDialog : Gtk.Dialog
    {
        List<string> m_customerList;
        List<string> m_foodList;
        Requirements m_requirements;
        string m_lastSmileCount;

        public RequirementEditDialog(Gtk.Window parent, List<string> customerList, List<string> foodList, Requirements curReq)
        {
            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            m_customerList = customerList;
            m_foodList = foodList;

            checkbutton_allow_burn.Active = curReq.allowBurn;
            checkbutton_allow_lost.Active = curReq.allowLostCustomer;
            m_lastSmileCount = curReq.smileCount.ToString();
            entry_smile_count.Text = m_lastSmileCount;

            string jsonStr = JsonConvert.SerializeObject(curReq.requiredCustomers);
            if (!string.IsNullOrEmpty(jsonStr))
            {
                entry_required_customers.Text = jsonStr;
            }
            jsonStr = JsonConvert.SerializeObject(curReq.requiredFoods);
            if (!string.IsNullOrEmpty(jsonStr))
            {
                entry_required_foods.Text = jsonStr;
            }
        }

        public Requirements Requirements
        {
            get => m_requirements;
        }

        protected void OnButtonEditCustomersClicked(object sender, EventArgs e)
        {
            List<Requirements.NameAndNumber> reqCustomers = JsonConvert.DeserializeObject<List<Requirements.NameAndNumber>>(entry_required_customers.Text);

            CustomerRequirementDialog dlg = new CustomerRequirementDialog(this, m_customerList, reqCustomers);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                List<Requirements.NameAndNumber> req = dlg.RequiredCustomers;
                if (req != null)
                {
                    entry_required_customers.Text = JsonConvert.SerializeObject(req);
                }
            }
            dlg.Destroy();
        }

        protected void OnButtonEditFoodsClicked(object sender, EventArgs e)
        {
            List<Requirements.NameAndNumber> reqFoods = JsonConvert.DeserializeObject<List<Requirements.NameAndNumber>>(entry_required_foods.Text);

            FoodRequirementDialog dlg = new FoodRequirementDialog(this, m_foodList, reqFoods);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                List<Requirements.NameAndNumber> req = dlg.RequiredFoods;
                if (req != null)
                {
                    entry_required_foods.Text = JsonConvert.SerializeObject(req);
                }
            }
            dlg.Destroy();
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            m_requirements = new Requirements();
            m_requirements.allowBurn = checkbutton_allow_burn.Active;
            m_requirements.allowLostCustomer = checkbutton_allow_lost.Active;
            m_requirements.smileCount = int.Parse(entry_smile_count.Text);
            string jsonStr = entry_required_foods.Text;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                m_requirements.requiredFoods = JsonConvert.DeserializeObject<List<Requirements.NameAndNumber>>(jsonStr);
            }
            jsonStr = entry_required_customers.Text;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                m_requirements.requiredCustomers = JsonConvert.DeserializeObject<List<Requirements.NameAndNumber>>(jsonStr);
            }
        }

        protected void OnEntrySmileCountTextInserted(object o, TextInsertedArgs args)
        {
            string text = args.Text;
            if (text.Equals("1") || text.Equals("2")
                        || text.Equals("3") || text.Equals("4") 
                        || text.Equals("5") || text.Equals("6")
                        || text.Equals("7") || text.Equals("8")
                        || text.Equals("9") || text.Equals("0")
                || (text.Equals("-") && args.Position == 1 && !m_lastSmileCount.Substring(0, 1).Equals("-")))
            {
                m_lastSmileCount = entry_smile_count.Text;
            }
            else
            {
                entry_smile_count.Text = m_lastSmileCount;
            }
        }
    }
}
