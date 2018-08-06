using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using Ministone.GameCore.GameData;

namespace LevelEditor
{
    public partial class SecretCustomerEditDialog : Gtk.Dialog
    {
        ListStore m_customerStore;
        ListStore m_secretStore;
        List<SecretCustomer> m_secretCustomers;
        CustomerDataManager _custMgr = CustomerDataManager.GetInstance();

        public SecretCustomerEditDialog(Gtk.Window parent, List<string> customerList, List<SecretCustomer> secretCustomers)
        {
            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            treeview_customer_list.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_customer_list.AppendColumn("名称", new CellRendererText(), "text", 1);
            treeview_customer_list.Selection.Mode = SelectionMode.Multiple;
            m_customerStore = new ListStore(typeof(Pixbuf), typeof(string), typeof(string));
            if (customerList != null)
            {
                foreach(string customer in customerList)
                {
                    bool selected = false;
                    if (secretCustomers != null)
                    {
                        foreach (SecretCustomer sc in secretCustomers)
                        {
                            if (sc.customer.Equals(customer))
                            {
                                selected = true;
                                break;
                            }
                        }
                    }
                    if (!selected)
                    {
                        AppendCustomer(customer);
                    }
                }
            }
            treeview_customer_list.Model = m_customerStore;

            treeview_secret_customers.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_secret_customers.AppendColumn("名称", new CellRendererText(), "text", 1);
            CellRendererText cell = new CellRendererText();
            cell.Editable = true;
            cell.Edited += OnEditedShowTimes;
            cell.Alignment = Pango.Alignment.Center;
            treeview_secret_customers.AppendColumn("第几次出现后隐藏订单", cell, "text", 2);
            treeview_secret_customers.Selection.Mode = SelectionMode.Multiple;
            m_secretStore = new ListStore(typeof(Pixbuf), typeof(string), typeof(int), typeof(string));
            if (secretCustomers != null)
            {
                foreach (SecretCustomer sc in secretCustomers)
                {
                    AppendSecretCustomer(sc.customer, sc.showOrder);
                }
            }
            treeview_secret_customers.Model = m_secretStore;
        }

        protected void AppendCustomer(string customer)
        {
            float iconSize = 80.0f;
            CustomerData custData = _custMgr.GetCustomer(customer);
            Pixbuf pb = new Pixbuf(Utils.GetCustomerImagePath(customer));
            float scale = iconSize / pb.Height;
            Pixbuf custIcon = pb.ScaleSimple((int)(scale * pb.Width), (int)iconSize, InterpType.Hyper);
            m_customerStore.AppendValues(custIcon, custData.GetDisplayName("zh-cn"), customer);
        }

        protected void AppendSecretCustomer(string customer, int showOrder)
        {
            float iconSize = 60.0f;
            CustomerData custData = _custMgr.GetCustomer(customer);
            Pixbuf pb = new Pixbuf(Utils.GetCustomerImagePath(customer));
            float scale = iconSize / pb.Height;
            Pixbuf custIcon = pb.ScaleSimple((int)(scale * pb.Width), (int)iconSize, InterpType.Hyper);
            m_secretStore.AppendValues(custIcon, custData.GetDisplayName("zh-cn"), showOrder, customer);
        }

        public List<SecretCustomer> SecretCustomers
        {
            get => m_secretCustomers;
        }

        protected void OnEditedShowTimes(object o, EditedArgs args)
        {
            int num;
            if (!Utils.ParseInteger(args.NewText, out num, this) || num < 0)
            {
                return;
            }

            TreeIter iter;
            if (m_secretStore.GetIterFromString(out iter, args.Path))
            {
                m_secretStore.SetValue(iter, 2, num);
            }
        }

        protected void OnButtonAddCustomerClicked(object sender, EventArgs e)
        {
            TreePath[] paths = treeview_customer_list.Selection.GetSelectedRows();
            if (paths != null)
            {
                for (int i = 0; i < paths.Length; ++ i)
                {
                    TreeIter iter;
                    if (m_customerStore.GetIter(out iter, paths[i]))
                    {
                        string customer = (string)m_customerStore.GetValue(iter, 2);
                        AppendSecretCustomer(customer, 0);
                    }
                }
                for (int i = paths.Length - 1; i >= 0; -- i)
                {
                    TreeIter iter;
                    if (m_customerStore.GetIter(out iter, paths[i]))
                    {
                        m_customerStore.Remove(ref iter);
                    }
                }
            }
        }

        protected void OnButtonRemoveCustomerClicked(object sender, EventArgs e)
        {
            TreePath[] paths = treeview_secret_customers.Selection.GetSelectedRows();
            if (paths != null)
            {
                for (int i = 0; i < paths.Length; ++ i)
                {
                    TreeIter iter;
                    if (m_secretStore.GetIter(out iter, paths[i]))
                    {
                        string customer = (string)m_secretStore.GetValue(iter, 3);
                        AppendCustomer(customer);
                    }
                }

                for (int i = paths.Length - 1; i >= 0; -- i)
                {
                    TreeIter iter;
                    if (m_secretStore.GetIter(out iter, paths[i]))
                    {
                        m_secretStore.Remove(ref iter);
                    }
                }
            }
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            m_secretCustomers = new List<SecretCustomer>();
            TreeIter iter;
            if (m_secretStore.GetIterFirst(out iter))
            {
                do
                {
                    SecretCustomer sc = new SecretCustomer();
                    sc.customer = (string)m_secretStore.GetValue(iter, 3);
                    sc.showOrder = (int)m_secretStore.GetValue(iter, 2);
                    m_secretCustomers.Add(sc);
                } while (m_secretStore.IterNext(ref iter));
            }
        }
    }
}
