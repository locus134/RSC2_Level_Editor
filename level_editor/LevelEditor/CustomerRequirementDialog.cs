using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using Ministone.GameCore.GameData;

namespace LevelEditor
{
    public partial class CustomerRequirementDialog : Gtk.Dialog
    {
        CustomerDataManager _custMgr = CustomerDataManager.GetInstance();

        List<string> m_customerList;
        List<Requirements.NameAndNumber> m_requiredCustomers;
        ListStore m_reqCustStore;

        public CustomerRequirementDialog(Gtk.Window parent, List<string> customerList, List<Requirements.NameAndNumber> requiredCustomers)
        {
            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            m_customerList = customerList;

            treeview_customer_list.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_customer_list.AppendColumn("名称", new CellRendererText(), "text", 1);
            treeview_customer_list.Selection.Mode = SelectionMode.Multiple;
            Utils.ShowCustomerList(m_customerList, treeview_customer_list);

            treeview_require_customers.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_require_customers.AppendColumn("名称", new CellRendererText(), "text", 1);
            CellRendererText cell = new CellRendererText();
            cell.Editable = true;
            cell.Edited += OnEditedCustomerNumber;
            treeview_require_customers.AppendColumn("数量", cell, "text", 2);
            treeview_require_customers.Selection.Mode = SelectionMode.Multiple;
            m_reqCustStore = new ListStore(typeof(Pixbuf), typeof(string), typeof(int), typeof(string));
            if (requiredCustomers != null)
            {
                foreach (Requirements.NameAndNumber req in requiredCustomers)
                {
                    AppendNewCustomerRequirement(req);
                }
            }
            treeview_require_customers.Model = m_reqCustStore;
        }

        public List<Requirements.NameAndNumber> RequiredCustomers
        {
            get => m_requiredCustomers;
        }

        protected void OnEditedCustomerNumber(object o, EditedArgs args)
        {
            TreeIter iter;
            if (m_reqCustStore.GetIterFromString(out iter, args.Path))
            {
                int num;
                if (Utils.ParseInteger(args.NewText, out num, this))
                {
                    if (num > 0)
                    {
                        m_reqCustStore.SetValue(iter, 2, num);
                    }
                }
            }
        }

        protected void AppendNewCustomerRequirement(Requirements.NameAndNumber req)
        {
            CustomerData cust = _custMgr.GetCustomer(req.name);
            Pixbuf pb = new Pixbuf(Utils.GetCustomerImagePath(cust));
            float scale = 80.0f / pb.Height;
            Pixbuf custIcon = pb.ScaleSimple((int)(scale * pb.Width), (int)(scale * pb.Height), InterpType.Hyper);
            m_reqCustStore.AppendValues(custIcon, cust.GetDisplayName("zh-cn"), req.number, req.name);
        }

        protected void OnButtonAddCustomerClicked(object sender, EventArgs e)
        {
            TreeModel model;
            TreePath[] paths = treeview_customer_list.Selection.GetSelectedRows(out model);
            if (paths == null)
            {
                return;
            }
            foreach (TreePath path in paths)
            {
                string customer = m_customerList[path.Indices[0]];
                bool isNew = true;
                TreeIter iter;
                if (m_reqCustStore.GetIterFirst(out iter))
                {
                    do
                    {
                        string custStr = (string)m_reqCustStore.GetValue(iter, 3);
                        if (customer.Equals(custStr))
                        {
                            isNew = false;
                            break;
                        }
                    } while (m_reqCustStore.IterNext(ref iter));
                }

                if (isNew)
                {
                    Requirements.NameAndNumber req = new Requirements.NameAndNumber();
                    req.name = customer;
                    req.number = 0;
                    AppendNewCustomerRequirement(req);
                }
            }
        }

        protected void OnButtonRemoveCustomerClicked(object sender, EventArgs e)
        {
            TreePath[] paths = treeview_require_customers.Selection.GetSelectedRows();
            if (paths == null)
            {
                return;
            }
            for (int i = paths.Length - 1; i >= 0; -- i)
            {
                TreeIter iter;
                if (m_reqCustStore.GetIter(out iter, paths[i]))
                {
                    m_reqCustStore.Remove(ref iter);
                }
            }
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            m_requiredCustomers = new List<Requirements.NameAndNumber>();
            TreeIter iter;
            if (m_reqCustStore.GetIterFirst(out iter))
            {
                do
                {
                    int num = (int)m_reqCustStore.GetValue(iter, 2);
                    if (num > 0)
                    {
                        Requirements.NameAndNumber req = new Requirements.NameAndNumber();
                        req.name = (string)m_reqCustStore.GetValue(iter, 3);
                        req.number = num;
                        m_requiredCustomers.Add(req);
                    }
                } while (m_reqCustStore.IterNext(ref iter));
            }
        }
    }
}
