
// This file has been generated by the GUI designer. Do not modify.
namespace LevelEditor
{
	public partial class FailTipsEditDialog
	{
		private global::Gtk.VBox vbox3;

		private global::Gtk.HBox hbox12;

		private global::Gtk.Label label7;

		private global::Gtk.Entry entry_failtips_id;

		private global::Gtk.CheckButton checkbutton_is_upgrade;

		private global::Gtk.Frame frame10;

		private global::Gtk.Alignment GtkAlignment2;

		private global::Gtk.VBox vbox5;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TreeView treeview_failtips_list;

		private global::Gtk.HButtonBox hbuttonbox2;

		private global::Gtk.Button button_add_failtips;

		private global::Gtk.Button button_remove_failtips;

		private global::Gtk.Label GtkLabel3;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget LevelEditor.FailTipsEditDialog
			this.Name = "LevelEditor.FailTipsEditDialog";
			this.Title = global::Mono.Unix.Catalog.GetString("过关提示设置");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child LevelEditor.FailTipsEditDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox12 = new global::Gtk.HBox();
			this.hbox12.Name = "hbox12";
			this.hbox12.Spacing = 6;
			// Container child hbox12.Gtk.Box+BoxChild
			this.label7 = new global::Gtk.Label();
			this.label7.Name = "label7";
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString("提示ID:");
			this.hbox12.Add(this.label7);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox12[this.label7]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox12.Gtk.Box+BoxChild
			this.entry_failtips_id = new global::Gtk.Entry();
			this.entry_failtips_id.WidthRequest = 50;
			this.entry_failtips_id.CanFocus = true;
			this.entry_failtips_id.Name = "entry_failtips_id";
			this.entry_failtips_id.IsEditable = true;
			this.entry_failtips_id.InvisibleChar = '●';
			this.hbox12.Add(this.entry_failtips_id);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox12[this.entry_failtips_id]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			// Container child hbox12.Gtk.Box+BoxChild
			this.checkbutton_is_upgrade = new global::Gtk.CheckButton();
			this.checkbutton_is_upgrade.CanFocus = true;
			this.checkbutton_is_upgrade.Name = "checkbutton_is_upgrade";
			this.checkbutton_is_upgrade.Label = global::Mono.Unix.Catalog.GetString("是否是升级提示？");
			this.checkbutton_is_upgrade.DrawIndicator = true;
			this.checkbutton_is_upgrade.UseUnderline = true;
			this.hbox12.Add(this.checkbutton_is_upgrade);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox12[this.checkbutton_is_upgrade]));
			w4.Position = 2;
			w4.Expand = false;
			w4.Fill = false;
			w4.Padding = ((uint)(50));
			this.vbox3.Add(this.hbox12);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.hbox12]));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.frame10 = new global::Gtk.Frame();
			this.frame10.Name = "frame10";
			this.frame10.ShadowType = ((global::Gtk.ShadowType)(2));
			this.frame10.BorderWidth = ((uint)(5));
			// Container child frame10.Gtk.Container+ContainerChild
			this.GtkAlignment2 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
			this.GtkAlignment2.Name = "GtkAlignment2";
			this.GtkAlignment2.LeftPadding = ((uint)(12));
			// Container child GtkAlignment2.Gtk.Container+ContainerChild
			this.vbox5 = new global::Gtk.VBox();
			this.vbox5.Name = "vbox5";
			this.vbox5.Spacing = 6;
			// Container child vbox5.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.treeview_failtips_list = new global::Gtk.TreeView();
			this.treeview_failtips_list.CanFocus = true;
			this.treeview_failtips_list.Name = "treeview_failtips_list";
			this.GtkScrolledWindow.Add(this.treeview_failtips_list);
			this.vbox5.Add(this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox5[this.GtkScrolledWindow]));
			w7.Position = 0;
			// Container child vbox5.Gtk.Box+BoxChild
			this.hbuttonbox2 = new global::Gtk.HButtonBox();
			this.hbuttonbox2.Name = "hbuttonbox2";
			this.hbuttonbox2.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(3));
			// Container child hbuttonbox2.Gtk.ButtonBox+ButtonBoxChild
			this.button_add_failtips = new global::Gtk.Button();
			this.button_add_failtips.CanFocus = true;
			this.button_add_failtips.Name = "button_add_failtips";
			this.button_add_failtips.UseUnderline = true;
			this.button_add_failtips.Label = global::Mono.Unix.Catalog.GetString("+");
			this.hbuttonbox2.Add(this.button_add_failtips);
			global::Gtk.ButtonBox.ButtonBoxChild w8 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox2[this.button_add_failtips]));
			w8.Expand = false;
			w8.Fill = false;
			// Container child hbuttonbox2.Gtk.ButtonBox+ButtonBoxChild
			this.button_remove_failtips = new global::Gtk.Button();
			this.button_remove_failtips.CanFocus = true;
			this.button_remove_failtips.Name = "button_remove_failtips";
			this.button_remove_failtips.UseUnderline = true;
			this.button_remove_failtips.Label = global::Mono.Unix.Catalog.GetString("-");
			this.hbuttonbox2.Add(this.button_remove_failtips);
			global::Gtk.ButtonBox.ButtonBoxChild w9 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox2[this.button_remove_failtips]));
			w9.Position = 1;
			w9.Expand = false;
			w9.Fill = false;
			this.vbox5.Add(this.hbuttonbox2);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox5[this.hbuttonbox2]));
			w10.Position = 1;
			w10.Expand = false;
			w10.Fill = false;
			this.GtkAlignment2.Add(this.vbox5);
			this.frame10.Add(this.GtkAlignment2);
			this.GtkLabel3 = new global::Gtk.Label();
			this.GtkLabel3.Name = "GtkLabel3";
			this.GtkLabel3.LabelProp = global::Mono.Unix.Catalog.GetString("<b>提示内容</b>");
			this.GtkLabel3.UseMarkup = true;
			this.frame10.LabelWidget = this.GtkLabel3;
			this.vbox3.Add(this.frame10);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.frame10]));
			w13.Position = 1;
			w1.Add(this.vbox3);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(w1[this.vbox3]));
			w14.Position = 0;
			// Internal child LevelEditor.FailTipsEditDialog.ActionArea
			global::Gtk.HButtonBox w15 = this.ActionArea;
			w15.Name = "dialog1_ActionArea";
			w15.Spacing = 10;
			w15.BorderWidth = ((uint)(5));
			w15.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget(this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w16 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w15[this.buttonCancel]));
			w16.Expand = false;
			w16.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget(this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w17 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w15[this.buttonOk]));
			w17.Position = 1;
			w17.Expand = false;
			w17.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 648;
			this.DefaultHeight = 425;
			this.Show();
			this.entry_failtips_id.TextInserted += new global::Gtk.TextInsertedHandler(this.OnEntryFailtipsIdTextInserted);
			this.button_add_failtips.Clicked += new global::System.EventHandler(this.OnButtonAddFailtipsClicked);
			this.button_remove_failtips.Clicked += new global::System.EventHandler(this.OnButtonRemoveFailtipsClicked);
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}