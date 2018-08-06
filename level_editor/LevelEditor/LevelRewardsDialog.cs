using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using Ministone.GameCore.GameData;

namespace LevelEditor
{
    public partial class LevelRewardsDialog : Gtk.Dialog
    {
        ListStore m_rewardStore;
        List<RewardData> m_rewards;

        public LevelRewardsDialog(Gtk.Window parent, List<RewardData> rewards)
        {
            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            CellRendererText cellID = new CellRendererText();
            cellID.Editable = true;
            cellID.Edited += onEditedItemID;
            cellID.WidthChars = 10;
            treeview_reward_list.AppendColumn("ID", cellID, "text", 0);

            CellRendererText cellNum = new CellRendererText();
            cellNum.Editable = true;
            cellNum.Edited += onEditedItemNumber;
            cellNum.WidthChars = 10;
            treeview_reward_list.AppendColumn("数量", cellNum, "text", 1);
            treeview_reward_list.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 2);
            treeview_reward_list.AppendColumn("名称", new CellRendererText(), "text", 3);

            m_rewardStore = new ListStore(typeof(int), typeof(int), typeof(Pixbuf), typeof(string));
            if (rewards != null)
            {
                foreach (RewardData rd in rewards)
                {
                    SetRewardItem(rd.itemID, rd.itemCount, null);
                }
            }
            treeview_reward_list.Model = m_rewardStore;
            treeview_reward_list.Selection.Mode = SelectionMode.Multiple;
        }

        public List<RewardData> Rewards
        {
            get => m_rewards;
        }

        protected void SetRewardItem(int id, int count, TreePath path)
        {
            TreeIter iter;
            if (path != null)
            {
                if (!m_rewardStore.GetIter(out iter, path))
                {
                    return;
                }
                m_rewardStore.SetValue(iter, 0, id);
                if (count >= 0)
                {
                    m_rewardStore.SetValue(iter, 1, count);
                }
            }
            else
            {
                Pixbuf pb = Pixbuf.LoadFromResource("LevelEditor.question.png");
                iter = m_rewardStore.AppendValues(id, count, pb, "");
            }

            //TODO: 修改物品的图片和名称
        }

        protected void onEditedItemID(object o, EditedArgs args)
        {
            int id;
            if (Utils.ParseInteger(args.NewText, out id, this))
            {
                TreeIter iter;
                if (m_rewardStore.GetIterFromString(out iter, args.Path))
                {
                    SetRewardItem(id, -1, m_rewardStore.GetPath(iter));
                }
            }
        }

        protected void onEditedItemNumber(object o, EditedArgs args)
        {
            int num;
            if (Utils.ParseInteger(args.NewText, out num, this))
            {
                TreeIter iter;
                if (m_rewardStore.GetIterFromString(out iter, args.Path))
                {
                    m_rewardStore.SetValue(iter, 1, num);
                }
            }
        }

        protected void OnButtonAddRewardClicked(object sender, EventArgs e)
        {
            SetRewardItem(0, 0, null);
        }

        protected void OnButtonRemoveRewardClicked(object sender, EventArgs e)
        {
            TreePath[] paths = treeview_reward_list.Selection.GetSelectedRows();
            if (paths != null && paths.Length > 0)
            {
                for (int i = paths.Length - 1; i >= 0; -- i)
                {
                    TreeIter iter;
                    if (m_rewardStore.GetIter(out iter, paths[i]))
                    {
                        m_rewardStore.Remove(ref iter);
                    }
                }
            }
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            m_rewards = new List<RewardData>();
            TreeIter iter;
            if (m_rewardStore.GetIterFirst(out iter))
            {
                do
                {
                    RewardData rd = new RewardData();
                    rd.itemID = (int)m_rewardStore.GetValue(iter, 0);
                    if (rd.itemID > 0)
                    {
                        rd.itemCount = (int)m_rewardStore.GetValue(iter, 1);
                        if (rd.itemCount > 0)
                        {
                            m_rewards.Add(rd);
                        }
                    }
                } while (m_rewardStore.IterNext(ref iter));
            }
        }
    }
}
