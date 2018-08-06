
// This file has been generated by the GUI designer. Do not modify.
namespace LevelEditor
{
	public partial class FoodWeightSelectDialog
	{
		private global::Gtk.HBox hbox1;

		private global::Gtk.Frame frame1;

		private global::Gtk.Alignment GtkAlignment2;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TreeView treeview_food_list;

		private global::Gtk.Label GtkLabel2;

		private global::Gtk.VButtonBox vbuttonbox1;

		private global::Gtk.Button button_add_food;

		private global::Gtk.Button button_normalize_weight;

		private global::Gtk.Button button_remove_food;

		private global::Gtk.Frame frame3;

		private global::Gtk.Alignment GtkAlignment3;

		private global::Gtk.ScrolledWindow GtkScrolledWindow1;

		private global::Gtk.TreeView treeview_select_foods;

		private global::Gtk.Label GtkLabel6;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget LevelEditor.FoodWeightSelectDialog
			this.Name = "LevelEditor.FoodWeightSelectDialog";
			this.Title = global::Mono.Unix.Catalog.GetString("食物比重");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child LevelEditor.FoodWeightSelectDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.frame1 = new global::Gtk.Frame();
			this.frame1.Name = "frame1";
			this.frame1.ShadowType = ((global::Gtk.ShadowType)(2));
			this.frame1.BorderWidth = ((uint)(5));
			// Container child frame1.Gtk.Container+ContainerChild
			this.GtkAlignment2 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
			this.GtkAlignment2.Name = "GtkAlignment2";
			this.GtkAlignment2.LeftPadding = ((uint)(12));
			// Container child GtkAlignment2.Gtk.Container+ContainerChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.treeview_food_list = new global::Gtk.TreeView();
			this.treeview_food_list.WidthRequest = 150;
			this.treeview_food_list.CanFocus = true;
			this.treeview_food_list.Name = "treeview_food_list";
			this.GtkScrolledWindow.Add(this.treeview_food_list);
			this.GtkAlignment2.Add(this.GtkScrolledWindow);
			this.frame1.Add(this.GtkAlignment2);
			this.GtkLabel2 = new global::Gtk.Label();
			this.GtkLabel2.Name = "GtkLabel2";
			this.GtkLabel2.LabelProp = global::Mono.Unix.Catalog.GetString("<b>食物列表</b>");
			this.GtkLabel2.UseMarkup = true;
			this.frame1.LabelWidget = this.GtkLabel2;
			this.hbox1.Add(this.frame1);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.frame1]));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbuttonbox1 = new global::Gtk.VButtonBox();
			this.vbuttonbox1.Name = "vbuttonbox1";
			this.vbuttonbox1.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(1));
			// Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.button_add_food = new global::Gtk.Button();
			this.button_add_food.CanFocus = true;
			this.button_add_food.Name = "button_add_food";
			this.button_add_food.UseUnderline = true;
			this.button_add_food.Label = global::Mono.Unix.Catalog.GetString(">>>>");
			this.vbuttonbox1.Add(this.button_add_food);
			global::Gtk.ButtonBox.ButtonBoxChild w6 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.button_add_food]));
			w6.Expand = false;
			w6.Fill = false;
			// Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.button_normalize_weight = new global::Gtk.Button();
			this.button_normalize_weight.CanFocus = true;
			this.button_normalize_weight.Name = "button_normalize_weight";
			this.button_normalize_weight.UseUnderline = true;
			this.button_normalize_weight.Label = global::Mono.Unix.Catalog.GetString("归一化比重");
			this.vbuttonbox1.Add(this.button_normalize_weight);
			global::Gtk.ButtonBox.ButtonBoxChild w7 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.button_normalize_weight]));
			w7.Position = 1;
			w7.Expand = false;
			w7.Fill = false;
			// Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.button_remove_food = new global::Gtk.Button();
			this.button_remove_food.CanFocus = true;
			this.button_remove_food.Name = "button_remove_food";
			this.button_remove_food.UseUnderline = true;
			this.button_remove_food.Label = global::Mono.Unix.Catalog.GetString("<<<<");
			this.vbuttonbox1.Add(this.button_remove_food);
			global::Gtk.ButtonBox.ButtonBoxChild w8 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1[this.button_remove_food]));
			w8.Position = 2;
			w8.Expand = false;
			w8.Fill = false;
			this.hbox1.Add(this.vbuttonbox1);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.vbuttonbox1]));
			w9.Position = 1;
			w9.Expand = false;
			w9.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.frame3 = new global::Gtk.Frame();
			this.frame3.Name = "frame3";
			this.frame3.ShadowType = ((global::Gtk.ShadowType)(2));
			this.frame3.BorderWidth = ((uint)(5));
			// Container child frame3.Gtk.Container+ContainerChild
			this.GtkAlignment3 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
			this.GtkAlignment3.Name = "GtkAlignment3";
			this.GtkAlignment3.LeftPadding = ((uint)(12));
			// Container child GtkAlignment3.Gtk.Container+ContainerChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.treeview_select_foods = new global::Gtk.TreeView();
			this.treeview_select_foods.WidthRequest = 250;
			this.treeview_select_foods.CanFocus = true;
			this.treeview_select_foods.Name = "treeview_select_foods";
			this.GtkScrolledWindow1.Add(this.treeview_select_foods);
			this.GtkAlignment3.Add(this.GtkScrolledWindow1);
			this.frame3.Add(this.GtkAlignment3);
			this.GtkLabel6 = new global::Gtk.Label();
			this.GtkLabel6.Name = "GtkLabel6";
			this.GtkLabel6.LabelProp = global::Mono.Unix.Catalog.GetString("<b>已选食物</b>");
			this.GtkLabel6.UseMarkup = true;
			this.frame3.LabelWidget = this.GtkLabel6;
			this.hbox1.Add(this.frame3);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.frame3]));
			w13.Position = 2;
			w13.Expand = false;
			w13.Fill = false;
			w1.Add(this.hbox1);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(w1[this.hbox1]));
			w14.Position = 0;
			// Internal child LevelEditor.FoodWeightSelectDialog.ActionArea
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
			this.DefaultWidth = 576;
			this.DefaultHeight = 615;
			this.Show();
			this.button_add_food.Clicked += new global::System.EventHandler(this.OnButtonAddFoodClicked);
			this.button_normalize_weight.Clicked += new global::System.EventHandler(this.OnButtonNormalizeWeightClicked);
			this.button_remove_food.Clicked += new global::System.EventHandler(this.OnButtonRemoveFoodClicked);
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}
