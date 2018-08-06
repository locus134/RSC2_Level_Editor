
// This file has been generated by the GUI designer. Do not modify.
namespace LevelEditor
{
	public partial class UnlockItemsDialog
	{
		private global::Gtk.VBox vbox2;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TreeView treeview_item_list;

		private global::Gtk.HButtonBox hbuttonbox2;

		private global::Gtk.Button button_add_item;

		private global::Gtk.Button button_remove_item;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget LevelEditor.UnlockItemsDialog
			this.Name = "LevelEditor.UnlockItemsDialog";
			this.Title = global::Mono.Unix.Catalog.GetString("解锁物品");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child LevelEditor.UnlockItemsDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.treeview_item_list = new global::Gtk.TreeView();
			this.treeview_item_list.CanFocus = true;
			this.treeview_item_list.Name = "treeview_item_list";
			this.GtkScrolledWindow.Add(this.treeview_item_list);
			this.vbox2.Add(this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.GtkScrolledWindow]));
			w3.Position = 0;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbuttonbox2 = new global::Gtk.HButtonBox();
			this.hbuttonbox2.Name = "hbuttonbox2";
			this.hbuttonbox2.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(3));
			// Container child hbuttonbox2.Gtk.ButtonBox+ButtonBoxChild
			this.button_add_item = new global::Gtk.Button();
			this.button_add_item.CanFocus = true;
			this.button_add_item.Name = "button_add_item";
			this.button_add_item.UseUnderline = true;
			this.button_add_item.Label = global::Mono.Unix.Catalog.GetString("+");
			this.hbuttonbox2.Add(this.button_add_item);
			global::Gtk.ButtonBox.ButtonBoxChild w4 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox2[this.button_add_item]));
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbuttonbox2.Gtk.ButtonBox+ButtonBoxChild
			this.button_remove_item = new global::Gtk.Button();
			this.button_remove_item.CanFocus = true;
			this.button_remove_item.Name = "button_remove_item";
			this.button_remove_item.UseUnderline = true;
			this.button_remove_item.Label = global::Mono.Unix.Catalog.GetString("-");
			this.hbuttonbox2.Add(this.button_remove_item);
			global::Gtk.ButtonBox.ButtonBoxChild w5 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox2[this.button_remove_item]));
			w5.Position = 1;
			w5.Expand = false;
			w5.Fill = false;
			this.vbox2.Add(this.hbuttonbox2);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbuttonbox2]));
			w6.Position = 1;
			w6.Expand = false;
			w6.Fill = false;
			w1.Add(this.vbox2);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(w1[this.vbox2]));
			w7.Position = 0;
			// Internal child LevelEditor.UnlockItemsDialog.ActionArea
			global::Gtk.HButtonBox w8 = this.ActionArea;
			w8.Name = "dialog1_ActionArea";
			w8.Spacing = 10;
			w8.BorderWidth = ((uint)(5));
			w8.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget(this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w9 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w8[this.buttonCancel]));
			w9.Expand = false;
			w9.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget(this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w10 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w8[this.buttonOk]));
			w10.Position = 1;
			w10.Expand = false;
			w10.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 433;
			this.Show();
			this.button_add_item.Clicked += new global::System.EventHandler(this.OnButtonAddItemClicked);
			this.button_remove_item.Clicked += new global::System.EventHandler(this.OnButtonRemoveItemClicked);
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}
