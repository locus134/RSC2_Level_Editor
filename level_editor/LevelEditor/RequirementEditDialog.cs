using System;
using System.Collections.Generic;
using Gtk;
using Ministone.GameCore.GameData;
using Ministone.GameCore.GameData.Generic;

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

            string requireStr = "";
            foreach (var req in curReq.requiredCustomers)
            {
                requireStr += string.Format("{0}*{1};", req.name, req.number);
            }
            entry_required_customers.Text = requireStr;

            requireStr = "";
            foreach (var req in curReq.requiredFoods)
            {
                requireStr += string.Format("{0}*{1};", req.name, req.number);
            }
            entry_required_foods.Text = requireStr;
        }

        public Requirements Requirements
        {
            get => m_requirements;
        }

        protected void OnButtonEditCustomersClicked(object sender, EventArgs e)
        {
            List<Requirements.NameAndNumber> reqCustomers = new List<Requirements.NameAndNumber>();
            string txtRequire = entry_required_customers.Text;
            if (!string.IsNullOrEmpty(txtRequire))
            {
                if (txtRequire.Contains("*"))
                {
                    string[] requireFoods = txtRequire.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string require in requireFoods)
                    {
                        int pos = require.IndexOf('*');
                        string food = require.Substring(0, pos);
                        int number = require.Substring(pos + 1, require.Length - pos - 1).ToInt32();
                        var nn = new Requirements.NameAndNumber(food, number);
                        reqCustomers.Add(nn);
                    }
                }
            }

            CustomerRequirementDialog dlg = new CustomerRequirementDialog(this, m_customerList, reqCustomers);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                txtRequire = "";
                List<Requirements.NameAndNumber> req = dlg.RequiredCustomers;
                if (req != null)
                {
                    foreach (var requrie in req)
                    {
                        txtRequire += string.Format("{0}*{1};", requrie.name, requrie.number);
                    }
                }
                entry_required_customers.Text = txtRequire;
            }
            dlg.Destroy();
        }

        protected void OnButtonEditFoodsClicked(object sender, EventArgs e)
        {
            List<Requirements.NameAndNumber> reqFoods = new List<Requirements.NameAndNumber>();
            string txtRequire = entry_required_foods.Text;
            if (!string.IsNullOrEmpty(txtRequire))
            {
                if (txtRequire.Contains("*"))
                {
                    string[] requireFoods = txtRequire.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string require in requireFoods)
                    {
                        int pos = require.IndexOf('*');
                        string food = require.Substring(0, pos);
                        int number = require.Substring(pos + 1, require.Length - pos - 1).ToInt32();
                        var nn = new Requirements.NameAndNumber(food, number);
                        reqFoods.Add(nn);
                    }
                }
            }
            FoodRequirementDialog dlg = new FoodRequirementDialog(this, m_foodList, reqFoods);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                txtRequire = "";
                List<Requirements.NameAndNumber> req = dlg.RequiredFoods;
                if (req != null)
                {
                    foreach (var requrie in req)
                    {
                        txtRequire += string.Format("{0}*{1};", requrie.name, requrie.number);
                    }
                }
                entry_required_foods.Text = txtRequire;
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
                if(jsonStr.Contains("*"))
                {
                    string[] requireFoods = jsonStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string reqFood in requireFoods)
                    {
                        int pos = reqFood.IndexOf('*');
                        string food = reqFood.Substring(0, pos);
                        int number = reqFood.Substring(pos + 1, reqFood.Length - pos - 1).ToInt32();
                        var nn = new Requirements.NameAndNumber(food, number);
                        m_requirements.requiredFoods.Add(nn);
                    }
                }
            }
            jsonStr = entry_required_customers.Text;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                if (jsonStr.Contains("*"))
                {
                    string[] requireCustomers = jsonStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string reqCus in requireCustomers)
                    {
                        int pos = reqCus.IndexOf('*');
                        string food = reqCus.Substring(0, pos);
                        int number = reqCus.Substring(pos + 1, reqCus.Length - pos - 1).ToInt32();
                        var nn = new Requirements.NameAndNumber(food, number);
                        m_requirements.requiredCustomers.Add(nn);
                    }
                }
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
