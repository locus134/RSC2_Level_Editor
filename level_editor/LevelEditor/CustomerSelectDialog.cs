using System;
using System.Collections.Generic;
using Gtk;

namespace LevelEditor
{
    public partial class CustomerSelectDialog : Gtk.Dialog
    {
        List<string> m_selectedCustomers;
        List<string> m_customerList;

        public CustomerSelectDialog(Gtk.Window parent, List<string> customerList, List<string> selectedCustomers)
        {
            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);
            m_customerList = customerList;

            treeview_customer_list.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_customer_list.AppendColumn("名称", new CellRendererText(), "text", 1);
            treeview_customer_list.Selection.Mode = SelectionMode.Multiple;
            Utils.ShowCustomerList(m_customerList, treeview_customer_list);

            if (selectedCustomers != null && selectedCustomers.Count > 0)
            {
                TreeIter iter;
                TreeModel model = treeview_customer_list.Model;
                if (model.GetIterFirst(out iter))
                {
                    do
                    {
                        if (selectedCustomers.Contains((string)model.GetValue(iter, 2)))
                        {
                            treeview_customer_list.Selection.SelectIter(iter);
                        }
                    } while (model.IterNext(ref iter));
                }
            }
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            m_selectedCustomers = new List<string>();
            TreePath[] paths = treeview_customer_list.Selection.GetSelectedRows();
            if (paths != null && paths.Length > 0)
            {
                for (int i = 0; i < paths.Length; ++i)
                {
                    m_selectedCustomers.Add(m_customerList[paths[i].Indices[0]]);
                }
            }
        }

        public List<string> SelectedCustomers
        {
            get => m_selectedCustomers;
        }

        protected void OnButtonClearClicked(object sender, EventArgs e)
        {
            treeview_customer_list.Selection.Mode = SelectionMode.None;
            treeview_customer_list.Selection.Mode = SelectionMode.Multiple;
        }
    }
}
