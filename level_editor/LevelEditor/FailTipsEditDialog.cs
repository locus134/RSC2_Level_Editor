using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using Ministone.GameCore.GameData;


namespace LevelEditor
{
    public partial class FailTipsEditDialog : Gtk.Dialog
    {
        FailTipData m_failTips;
        ListStore m_tipsStore;
        List<string> m_allLangs;
        string m_lastID;

        public FailTipsEditDialog(Gtk.Window parent, FailTipData curFailTips)
        {
            this.Build();
            this.TransientFor = parent;
            this.SetPosition(WindowPosition.CenterAlways);

            ListStore comboModel = new ListStore(typeof(string));
            ComboBox cbbox = new ComboBox(comboModel);
            //cbbox.AppendText(DEFAULT_LANG);
            m_allLangs = Utils.GetSupportedLanauges();
            foreach (string lang in m_allLangs)
            {
                cbbox.AppendText(lang);
            }
            cbbox.Active = 0;

            CellRendererCombo cellLang = new CellRendererCombo();
            cellLang.Editable = true;
            cellLang.Edited += OnEditedLang;
            cellLang.TextColumn = 0;
            cellLang.Text = cbbox.ActiveText;
            cellLang.Model = comboModel;
            cellLang.WidthChars = 18;
            treeview_failtips_list.AppendColumn("语言", cellLang, "text", 0);

            CellRendererText cellTips = new CellRendererText();
            cellTips.Editable = true;
            cellTips.Edited += OnEditedTips;
            treeview_failtips_list.AppendColumn("提示内容", cellTips, "text", 1);

            m_tipsStore = new ListStore(typeof(string), typeof(string), typeof(string));
            if (curFailTips != null)
            {
                entry_failtips_id.Text = curFailTips.id.ToString();
                checkbutton_is_upgrade.Active = curFailTips.isUpgrade;
                foreach (var tips in curFailTips.tips)
                {
                    string lang = Utils.GetLanguageName(tips.Key);
                    m_tipsStore.AppendValues(lang, tips.Value, tips.Key);
                }
            }
            treeview_failtips_list.Model = m_tipsStore;
            treeview_failtips_list.Selection.Mode = SelectionMode.Multiple;
        }

        public FailTipData FailTips
        {
            get => m_failTips;
        }

        protected void OnEditedLang(object o, EditedArgs args)
        {
            TreeIter iter;
            if (m_tipsStore.GetIterFirst(out iter))
            {
                do
                {
                    string lang = (string)m_tipsStore.GetValue(iter, 0);
                    if (lang.Equals(args.NewText))
                    {
                        return;
                    }
                } while (m_tipsStore.IterNext(ref iter));
            }

            if (m_tipsStore.GetIterFromString(out iter, args.Path))
            {
                string lang = Utils.GetLanguageCode(args.NewText);
                m_tipsStore.SetValue(iter, 0, args.NewText);
                m_tipsStore.SetValue(iter, 2, lang);
            }
        }

        protected void OnEditedTips(object o, EditedArgs args)
        {
            TreeIter iter;
            if (m_tipsStore.GetIterFromString(out iter, args.Path))
            {
                m_tipsStore.SetValue(iter, 1, args.NewText);
            }
        }

        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            m_failTips = new FailTipData();
            m_failTips.id = int.Parse(entry_failtips_id.Text);
            m_failTips.isUpgrade = checkbutton_is_upgrade.Active;
            TreeIter iter;
            if (m_tipsStore.GetIterFirst(out iter))
            {
                Dictionary<string, string> tips = new Dictionary<string, string>();
                do
                {
                    string lang = (string)m_tipsStore.GetValue(iter, 2);
                    string content = (string)m_tipsStore.GetValue(iter, 1);
                    if (!string.IsNullOrEmpty(lang) && !string.IsNullOrEmpty(content))
                    {
                        tips.Add(lang, content);
                    }
                } while (m_tipsStore.IterNext(ref iter));
                m_failTips.tips = tips;
            }
        }

        protected void OnEntryFailtipsIdTextInserted(object o, TextInsertedArgs args)
        {
            string text = args.Text;
            if (text.Equals("1") || text.Equals("2")
                        || text.Equals("3") || text.Equals("4")
                        || text.Equals("5") || text.Equals("6")
                        || text.Equals("7") || text.Equals("8")
                        || text.Equals("9") || text.Equals("0"))
            {
                m_lastID = entry_failtips_id.Text;
            }
            else
            {
                entry_failtips_id.Text = m_lastID;
            }
        }

        protected void OnButtonAddFailtipsClicked(object sender, EventArgs e)
        {
            m_tipsStore.AppendValues("<请选择...>", "", "");
        }

        protected void OnButtonRemoveFailtipsClicked(object sender, EventArgs e)
        {
            TreePath[] paths = treeview_failtips_list.Selection.GetSelectedRows();
            if (paths != null && paths.Length > 0)
            {
                for (int i = paths.Length - 1; i >= 0 ; -- i)
                {
                    TreeIter iter;
                    if (m_tipsStore.GetIter(out iter, paths[i]))
                    {
                        m_tipsStore.Remove(ref iter);
                    }
                }
            }
        }
    }
}
