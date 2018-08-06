using System;
using System.Collections.Generic;
using Gtk;

namespace LevelEditor
{
    public partial class FoodSelectDialog : Gtk.Dialog
    {
        List<string> m_selectedFoodList;

        public FoodSelectDialog(Gtk.Window parent, List<string> allFoodList, List<string> selectedFoodList)
        {
            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            treeview_food_list.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_food_list.AppendColumn("名称", new CellRendererText(), "text", 1);
            treeview_food_list.Selection.Mode = SelectionMode.Multiple;
            Utils.ShowFoodList(allFoodList, treeview_food_list);

            if (selectedFoodList != null && selectedFoodList.Count > 0)
            {
                TreeIter iter;
                TreeModel model = treeview_food_list.Model;
                if (model.GetIterFirst(out iter))
                {
                    do
                    {
                        if (selectedFoodList.Contains((string)model.GetValue(iter, 2)))
                        {
                            treeview_food_list.Selection.SelectIter(iter);
                        }
                    } while (model.IterNext(ref iter));
                }
            }
        }

        public List<string> SelectedFoods
        {
            get => m_selectedFoodList;
        }

        protected void OnButtonClearSelectionClicked(object sender, EventArgs e)
        {
            treeview_food_list.Selection.Mode = SelectionMode.None;
            treeview_food_list.Selection.Mode = SelectionMode.Multiple;
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            TreeIter iter;
            TreeModel model = treeview_food_list.Model;
            m_selectedFoodList = new List<string>();
            TreePath[] paths = treeview_food_list.Selection.GetSelectedRows();
            if (paths != null && paths.Length > 0)
            {
                for (int i = 0; i < paths.Length; ++i)
                {
                    if (model.GetIter(out iter, paths[i]))
                    {

                        m_selectedFoodList.Add((string)model.GetValue(iter, 2));
                    }
                }
            }
        }
    }
}
