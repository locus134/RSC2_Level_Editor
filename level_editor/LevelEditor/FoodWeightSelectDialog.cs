using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using Ministone.GameCore.GameData;

namespace LevelEditor
{
    public partial class FoodWeightSelectDialog : Gtk.Dialog
    {
        FoodDataManager _foodMgr = FoodDataManager.GetInstance();
        Dictionary<string, float> m_foodWeights;
        ListStore m_foodListStore;
        ListStore m_foodWeightListStore;

        public FoodWeightSelectDialog(Gtk.Window parent, List<string> foodList, Dictionary<string, float> selectedFoods)
        {
            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            treeview_food_list.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_food_list.AppendColumn("名称", new CellRendererText(), "text", 1);
            treeview_food_list.Selection.Mode = SelectionMode.Multiple;
            m_foodListStore = new ListStore(typeof(Pixbuf), typeof(string), typeof(string));
            for (int i = 0; i < foodList.Count; ++ i)
            {
                string food = foodList[i];
                if (selectedFoods == null || !selectedFoods.ContainsKey(food))
                {
                    AppendFoodList(food);
                }
            }
            treeview_food_list.Model = m_foodListStore;

            treeview_select_foods.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
            treeview_select_foods.AppendColumn("名称", new CellRendererText(), "text", 1);
            CellRendererText cell = new CellRendererText();
            cell.Editable = true;
            cell.Edited += OnEditedWeight;
            cell.WidthChars = 9;
            treeview_select_foods.AppendColumn("比重", cell, "text", 2);
            treeview_select_foods.Selection.Mode = SelectionMode.Multiple;
            m_foodWeightListStore = new ListStore(typeof(Pixbuf), typeof(string), typeof(string), typeof(string));
            if (selectedFoods != null)
            {
                foreach (var fw in selectedFoods)
                {
                    AppendSelectedFood(fw.Key, fw.Value);
                }
            }
            treeview_select_foods.Model = m_foodWeightListStore;
        }

        public Dictionary<string, float> FoodWeights
        {
            get => m_foodWeights;
        }

        protected void AppendFoodList(string food)
        {
            float iconSize = 60.0f;
            FoodData foodData = _foodMgr.GetFood(food);
            Pixbuf pb = new Pixbuf(Utils.GetFoodImagePath(foodData));
            float scale = iconSize / pb.Height;
            Pixbuf foodIcon = pb.ScaleSimple((int)(scale * pb.Width), (int)iconSize, InterpType.Hyper);
            m_foodListStore.AppendValues(foodIcon, foodData.GetDisplayName("cn"), food);
        }

        protected void AppendSelectedFood(string food, float weight)
        {
            float iconSize = 60.0f;
            FoodData foodData = _foodMgr.GetFood(food);
            Pixbuf pb = new Pixbuf(Utils.GetFoodImagePath(foodData));
            float scale = iconSize / pb.Height;
            Pixbuf foodIcon = pb.ScaleSimple((int)(scale * pb.Width), (int)iconSize, InterpType.Hyper);
            m_foodWeightListStore.AppendValues(foodIcon, foodData.GetDisplayName("cn"), weight.ToString(), food);
        }

        protected void OnEditedWeight(object o, EditedArgs args)
        {
            float weight;
            if (!Utils.ParseFloat(args.NewText, out weight, this))
            {
                return;
            }
            TreeIter iter;
            if (m_foodWeightListStore.GetIterFromString(out iter, args.Path))
            {
                m_foodWeightListStore.SetValue(iter, 2, args.NewText);
            }
        }

        protected void OnButtonAddFoodClicked(object sender, EventArgs e)
        {
            TreePath[] paths = treeview_food_list.Selection.GetSelectedRows();
            if (paths != null)
            {
                for (int i = 0; i < paths.Length; ++ i)
                {
                    TreeIter iter;
                    if (m_foodListStore.GetIter(out iter, paths[i]))
                    {
                        string food = (string)m_foodListStore.GetValue(iter, 2);
                        AppendSelectedFood(food, 1);
                    }
                }

                for (int i = paths.Length - 1; i >= 0; -- i)
                {
                    TreeIter iter;
                    if (m_foodListStore.GetIter(out iter, paths[i]))
                    {
                        m_foodListStore.Remove(ref iter);
                    }
                }
            }
        }

        protected void OnButtonRemoveFoodClicked(object sender, EventArgs e)
        {
            TreePath[] paths = treeview_select_foods.Selection.GetSelectedRows();
            if (paths != null)
            {
                for (int i = 0; i < paths.Length; ++ i)
                {
                    TreeIter iter;
                    if (m_foodWeightListStore.GetIter(out iter, paths[i]))
                    {
                        string food = (string)m_foodWeightListStore.GetValue(iter, 3);
                        AppendFoodList(food);
                    }
                }

                for (int i = paths.Length - 1; i >= 0; -- i)
                {
                    TreeIter iter;
                    if (m_foodWeightListStore.GetIter(out iter, paths[i]))
                    {
                        m_foodWeightListStore.Remove(ref iter);
                    }
                }
            }
        }

        protected void OnButtonNormalizeWeightClicked(object sender, EventArgs e)
        {
            TreeIter iter;
            if (m_foodWeightListStore.GetIterFirst(out iter))
            {
                float total = 0;
                do
                {
                    string strW = (string)m_foodWeightListStore.GetValue(iter, 2);
                    float weight;
                    if (float.TryParse(strW, out weight))
                    {
                        total += weight;
                    }
                } while (m_foodWeightListStore.IterNext(ref iter));

                m_foodWeightListStore.GetIterFirst(out iter);
                do
                {
                    string strW = (string)m_foodWeightListStore.GetValue(iter, 2);
                    float weight;
                    if (float.TryParse(strW, out weight))
                    {
                        weight = weight / total;
                        strW = string.Format("{0:F2}", weight);
                        m_foodWeightListStore.SetValue(iter, 2, strW);
                    }
                } while (m_foodWeightListStore.IterNext(ref iter));
            }
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            m_foodWeights = new Dictionary<string, float>();
            TreeIter iter;
            if (m_foodWeightListStore.GetIterFirst(out iter))
            {
                float total = 0;
                do
                {
                    string strW = (string)m_foodWeightListStore.GetValue(iter, 2);
                    float weight;
                    if (float.TryParse(strW, out weight))
                    {
                        total += weight;
                    }
                } while (m_foodWeightListStore.IterNext(ref iter));

                m_foodWeightListStore.GetIterFirst(out iter);
                do
                {
                    string strW = (string)m_foodWeightListStore.GetValue(iter, 2);
                    float weight;
                    if (float.TryParse(strW, out weight))
                    {
                        weight = weight / total;
                        strW = string.Format("{0:F2}", weight);
                        weight = float.Parse(strW);
                        string food = (string)m_foodWeightListStore.GetValue(iter, 3);
                        m_foodWeights.Add(food, weight);
                    }
                } while (m_foodWeightListStore.IterNext(ref iter));
            }
        }
    }
}
