using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using Ministone.GameCore.GameData;

namespace LevelEditor
{
    public partial class FoodRequirementDialog : Gtk.Dialog
    {
        FoodDataManager _foodMgr = FoodDataManager.GetInstance();

        List<string> m_foodList;
        List<Requirements.NameAndNumber> m_requiredFoods;
        ListStore m_reqFoodStore;

        public FoodRequirementDialog(Gtk.Window parent, List<string> foodList, List<Requirements.NameAndNumber> requiredFoods)
        {
            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            m_foodList = foodList;
            treeview_food_list.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_food_list.AppendColumn("名称", new CellRendererText(), "text", 1);
            treeview_food_list.Selection.Mode = SelectionMode.Multiple;
            Utils.ShowFoodList(foodList, treeview_food_list);

            treeview_require_foods.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_require_foods.AppendColumn("名称", new CellRendererText(), "text", 1);
            CellRendererText cell = new CellRendererText();
            cell.Editable = true;
            cell.Edited += OnEditedFoodNumber;
            treeview_require_foods.AppendColumn("数量", cell, "text", 2);
            treeview_require_foods.Selection.Mode = SelectionMode.Multiple;
            m_reqFoodStore = new ListStore(typeof(Pixbuf), typeof(string), typeof(int), typeof(string));
            if (requiredFoods != null)
            {
                foreach (Requirements.NameAndNumber req in requiredFoods)
                {
                    AppendNewFoodRequirement(req);
                }
            }
            treeview_require_foods.Model = m_reqFoodStore;
        }

        public List<Requirements.NameAndNumber> RequiredFoods
        {
            get => m_requiredFoods;
        }

        protected void OnEditedFoodNumber(object o, EditedArgs args)
        {
            TreeIter iter;
            if (m_reqFoodStore.GetIterFromString(out iter, args.Path))
            {
                int num;
                if (Utils.ParseInteger(args.NewText, out num, this))
                {
                    if (num > 0)
                    {
                        m_reqFoodStore.SetValue(iter, 2, num);
                    }
                }
            }
        }

        protected void AppendNewFoodRequirement(Requirements.NameAndNumber req)
        {
            FoodData fd = _foodMgr.GetFood(req.name);
            Pixbuf pb = new Pixbuf(Utils.GetFoodImagePath(fd));
            float scale = 60.0f / pb.Height;
            Pixbuf foodIcon = pb.ScaleSimple((int)(scale * pb.Width), (int)(scale * pb.Height), InterpType.Hyper);
            m_reqFoodStore.AppendValues(foodIcon, fd.GetDisplayName("cn"), req.number, req.name);
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            m_requiredFoods = new List<Requirements.NameAndNumber>();
            TreeIter iter;
            if (m_reqFoodStore.GetIterFirst(out iter))
            {
                do
                {
                    int num = (int)m_reqFoodStore.GetValue(iter, 2);
                    if (num > 0)
                    {
                        Requirements.NameAndNumber req = new Requirements.NameAndNumber();
                        req.name = (string)m_reqFoodStore.GetValue(iter, 3);
                        req.number = num;
                        m_requiredFoods.Add(req);
                    }
                } while (m_reqFoodStore.IterNext(ref iter));
            }
        }

        protected void OnButtonAddFoodClicked(object sender, EventArgs e)
        {
            TreeModel model;
            TreePath[] paths = treeview_food_list.Selection.GetSelectedRows(out model);
            if (paths == null)
            {
                return;
            }
            foreach (TreePath path in paths)
            {
                string food = m_foodList[path.Indices[0]];
                bool isNew = true;
                TreeIter iter;
                if (m_reqFoodStore.GetIterFirst(out iter))
                {
                    do
                    {
                        string foodStr = (string)m_reqFoodStore.GetValue(iter, 3);
                        if (food.Equals(foodStr))
                        {
                            isNew = false;
                            break;
                        }
                    } while (m_reqFoodStore.IterNext(ref iter));
                }

                if (isNew)
                {
                    Requirements.NameAndNumber req = new Requirements.NameAndNumber();
                    req.name = food;
                    req.number = 0;
                    AppendNewFoodRequirement(req);
                }
            }
        }

        protected void OnButtonRemoveFoodClicked(object sender, EventArgs e)
        {
            TreePath[] paths = treeview_require_foods.Selection.GetSelectedRows();
            if (paths == null)
            {
                return;
            }
            for (int i = paths.Length - 1; i >= 0; --i)
            {
                TreeIter iter;
                if (m_reqFoodStore.GetIter(out iter, paths[i]))
                {
                    m_reqFoodStore.Remove(ref iter);
                }
            }
        }
    }
}
