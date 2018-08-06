using System;
using System.Collections.Generic;
using Gtk;
using Gdk;

namespace LevelEditor
{
    public partial class UnlockItemsDialog : Gtk.Dialog
    {
        ListStore m_itemStore;
        List<int> m_unlockItems;

        public UnlockItemsDialog(Gtk.Window parent, List<int> unlockItems)
        {
            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            CellRendererText cell = new CellRendererText();
            cell.Editable = true;
            cell.Edited += onEditedItemID;
            cell.WidthChars = 10;
            treeview_item_list.AppendColumn("物品ID", cell, "text", 0);

            treeview_item_list.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 1);
            treeview_item_list.AppendColumn("名称", new CellRendererText(), "text", 2);
            treeview_item_list.Selection.Mode = SelectionMode.Multiple;

            m_itemStore = new ListStore(typeof(int), typeof(Pixbuf), typeof(string));
            if (unlockItems != null)
            {
                for (int i = 0; i < unlockItems.Count; ++ i)
                {
                    SetItemID(unlockItems[i], null);
                }
            }
            treeview_item_list.Model = m_itemStore;
        }

        public List<int> UnlockItems
        {
            get => m_unlockItems;
        }

        protected void SetItemID(int id, TreePath path)
        {
            TreeIter iter;
            if (path != null)
            {
                if (!m_itemStore.GetIter(out iter, path))
                {
                    return;
                }
                m_itemStore.SetValue(iter, 0, id);
            }
            else
            {
                Pixbuf pb = Pixbuf.LoadFromResource("LevelEditor.question.png");
                iter = m_itemStore.AppendValues(id, pb, "");
            }

            //TODO: 修改物品的图片和名称
        }

        protected void onEditedItemID(object o, EditedArgs args)
        {
            int num;
            if (Utils.ParseInteger(args.NewText, out num, this))
            {
                TreeIter iter;
                if (m_itemStore.GetIterFromString(out iter, args.Path))
                {
                    SetItemID(num, m_itemStore.GetPath(iter));
                }
            }
        }

        protected void OnButtonAddItemClicked(object sender, EventArgs e)
        {
            SetItemID(0, null);
        }

        protected void OnButtonRemoveItemClicked(object sender, EventArgs e)
        {
            TreePath[] paths = treeview_item_list.Selection.GetSelectedRows();
            if (paths != null && paths.Length > 0)
            {
                for (int i = paths.Length - 1; i >= 0; -- i)
                {
                    TreeIter iter;
                    if (m_itemStore.GetIter(out iter, paths[i]))
                    {
                        m_itemStore.Remove(ref iter);
                    }
                }
            }
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            m_unlockItems = new List<int>();
            TreeIter iter;
            if (m_itemStore.GetIterFirst(out iter))
            {
                do
                {
                    int id = (int)m_itemStore.GetValue(iter, 0);
                    if (id > 0 && !m_unlockItems.Contains(id))
                    {
                        m_unlockItems.Add(id);
                    }

                } while (m_itemStore.IterNext(ref iter));
            }
        }
    }
}
