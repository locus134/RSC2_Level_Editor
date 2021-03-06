
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VBox vbox2;

	private global::Gtk.Frame DB_PATH;

	private global::Gtk.Alignment GtkAlignment;

	private global::Gtk.HBox hbox3;

	private global::Gtk.Entry text_dbpath;

	private global::Gtk.Button browse_dbpath;

	private global::Gtk.Button btn_reload;

	private global::Gtk.Label Label_DB_PATH;

	private global::Gtk.Frame MAP_FRAME;

	private global::Gtk.Alignment GtkAlignment1;

	private global::Gtk.HBox hbox9;

	private global::Gtk.Label label8;

	private global::Gtk.ComboBox map_index;

	private global::Gtk.HBox hbox10;

	private global::Gtk.Label label_map_name;

	private global::Gtk.Entry text_rest_id;

	private global::Gtk.Label label_key;

	private global::Gtk.Entry text_rest_key;

	private global::Gtk.Button btn_edit_name;

	private global::Gtk.Fixed fixed4;

	private global::Gtk.HBox hbox12;

	private global::Gtk.Label label4;

	private global::Gtk.Entry text_level_count;

	private global::Gtk.Label MAP_SELECTOR;

	private global::Gtk.Frame FOOD_CUSTOMER;

	private global::Gtk.Alignment GtkAlignment2;

	private global::Gtk.HBox hbox16;

	private global::Gtk.Frame frame_food;

	private global::Gtk.Alignment GtkAlignment3;

	private global::Gtk.VBox vbox9;

	private global::Gtk.ScrolledWindow GtkScrolledWindow;

	private global::Gtk.TreeView food_list;

	private global::Gtk.Label label_foodlist;

	private global::Gtk.Frame frame_customer;

	private global::Gtk.Alignment GtkAlignment4;

	private global::Gtk.VBox vbox10;

	private global::Gtk.ScrolledWindow GtkScrolledWindow1;

	private global::Gtk.TreeView customer_list;

	private global::Gtk.Label label_customerlist;

	private global::Gtk.Frame frame_special_customer;

	private global::Gtk.Alignment GtkAlignment5;

	private global::Gtk.VBox vbox11;

	private global::Gtk.ScrolledWindow GtkScrolledWindow2;

	private global::Gtk.TreeView special_customer_list;

	private global::Gtk.Label label_speciallist;

	private global::Gtk.Label GtkLabel6;

	private global::Gtk.HButtonBox hbuttonbox3;

	private global::Gtk.Button btn_save;

	private global::Gtk.Button btn_edit_order;

	private global::Gtk.Button btn_edit_level;

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString("餐厅数据加载");
		this.WindowPosition = ((global::Gtk.WindowPosition)(3));
		this.Resizable = false;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox2 = new global::Gtk.VBox();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.DB_PATH = new global::Gtk.Frame();
		this.DB_PATH.Name = "DB_PATH";
		this.DB_PATH.ShadowType = ((global::Gtk.ShadowType)(2));
		this.DB_PATH.BorderWidth = ((uint)(5));
		// Container child DB_PATH.Gtk.Container+ContainerChild
		this.GtkAlignment = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment.Name = "GtkAlignment";
		this.GtkAlignment.LeftPadding = ((uint)(12));
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		this.hbox3 = new global::Gtk.HBox();
		this.hbox3.Name = "hbox3";
		this.hbox3.Spacing = 6;
		// Container child hbox3.Gtk.Box+BoxChild
		this.text_dbpath = new global::Gtk.Entry();
		this.text_dbpath.CanFocus = true;
		this.text_dbpath.Name = "text_dbpath";
		this.text_dbpath.Text = global::Mono.Unix.Catalog.GetString("请先设置数据库目录");
		this.text_dbpath.IsEditable = true;
		this.text_dbpath.InvisibleChar = '●';
		this.hbox3.Add(this.text_dbpath);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.text_dbpath]));
		w1.Position = 0;
		w1.Padding = ((uint)(2));
		// Container child hbox3.Gtk.Box+BoxChild
		this.browse_dbpath = new global::Gtk.Button();
		this.browse_dbpath.CanFocus = true;
		this.browse_dbpath.Name = "browse_dbpath";
		this.browse_dbpath.UseUnderline = true;
		this.browse_dbpath.Label = global::Mono.Unix.Catalog.GetString("选择数据库目录");
		this.hbox3.Add(this.browse_dbpath);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.browse_dbpath]));
		w2.Position = 1;
		w2.Expand = false;
		w2.Fill = false;
		w2.Padding = ((uint)(5));
		// Container child hbox3.Gtk.Box+BoxChild
		this.btn_reload = new global::Gtk.Button();
		this.btn_reload.CanFocus = true;
		this.btn_reload.Name = "btn_reload";
		this.btn_reload.UseUnderline = true;
		this.btn_reload.Label = global::Mono.Unix.Catalog.GetString("重新加载");
		this.hbox3.Add(this.btn_reload);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.btn_reload]));
		w3.Position = 2;
		w3.Expand = false;
		w3.Fill = false;
		this.GtkAlignment.Add(this.hbox3);
		this.DB_PATH.Add(this.GtkAlignment);
		this.Label_DB_PATH = new global::Gtk.Label();
		this.Label_DB_PATH.Name = "Label_DB_PATH";
		this.Label_DB_PATH.LabelProp = global::Mono.Unix.Catalog.GetString("<b>数据库路径设置</b>");
		this.Label_DB_PATH.UseMarkup = true;
		this.DB_PATH.LabelWidget = this.Label_DB_PATH;
		this.vbox2.Add(this.DB_PATH);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.DB_PATH]));
		w6.Position = 0;
		w6.Expand = false;
		w6.Fill = false;
		w6.Padding = ((uint)(3));
		// Container child vbox2.Gtk.Box+BoxChild
		this.MAP_FRAME = new global::Gtk.Frame();
		this.MAP_FRAME.Name = "MAP_FRAME";
		this.MAP_FRAME.ShadowType = ((global::Gtk.ShadowType)(2));
		this.MAP_FRAME.BorderWidth = ((uint)(5));
		// Container child MAP_FRAME.Gtk.Container+ContainerChild
		this.GtkAlignment1 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment1.Name = "GtkAlignment1";
		this.GtkAlignment1.LeftPadding = ((uint)(12));
		// Container child GtkAlignment1.Gtk.Container+ContainerChild
		this.hbox9 = new global::Gtk.HBox();
		this.hbox9.Name = "hbox9";
		this.hbox9.Spacing = 6;
		// Container child hbox9.Gtk.Box+BoxChild
		this.label8 = new global::Gtk.Label();
		this.label8.Name = "label8";
		this.label8.LabelProp = global::Mono.Unix.Catalog.GetString("餐厅选择：");
		this.hbox9.Add(this.label8);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox9[this.label8]));
		w7.Position = 0;
		w7.Expand = false;
		w7.Fill = false;
		// Container child hbox9.Gtk.Box+BoxChild
		this.map_index = global::Gtk.ComboBox.NewText();
		this.map_index.Name = "map_index";
		this.hbox9.Add(this.map_index);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox9[this.map_index]));
		w8.Position = 1;
		w8.Expand = false;
		w8.Fill = false;
		// Container child hbox9.Gtk.Box+BoxChild
		this.hbox10 = new global::Gtk.HBox();
		this.hbox10.Name = "hbox10";
		this.hbox10.Spacing = 6;
		// Container child hbox10.Gtk.Box+BoxChild
		this.label_map_name = new global::Gtk.Label();
		this.label_map_name.Name = "label_map_name";
		this.label_map_name.LabelProp = global::Mono.Unix.Catalog.GetString("餐厅ID：");
		this.hbox10.Add(this.label_map_name);
		global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox10[this.label_map_name]));
		w9.Position = 0;
		w9.Expand = false;
		w9.Fill = false;
		// Container child hbox10.Gtk.Box+BoxChild
		this.text_rest_id = new global::Gtk.Entry();
		this.text_rest_id.WidthRequest = 50;
		this.text_rest_id.CanFocus = true;
		this.text_rest_id.Name = "text_rest_id";
		this.text_rest_id.IsEditable = true;
		this.text_rest_id.InvisibleChar = '●';
		this.hbox10.Add(this.text_rest_id);
		global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox10[this.text_rest_id]));
		w10.Position = 1;
		w10.Padding = ((uint)(2));
		// Container child hbox10.Gtk.Box+BoxChild
		this.label_key = new global::Gtk.Label();
		this.label_key.Name = "label_key";
		this.label_key.LabelProp = global::Mono.Unix.Catalog.GetString("餐厅Key：");
		this.hbox10.Add(this.label_key);
		global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox10[this.label_key]));
		w11.Position = 2;
		w11.Expand = false;
		w11.Fill = false;
		// Container child hbox10.Gtk.Box+BoxChild
		this.text_rest_key = new global::Gtk.Entry();
		this.text_rest_key.WidthRequest = 100;
		this.text_rest_key.CanFocus = true;
		this.text_rest_key.Name = "text_rest_key";
		this.text_rest_key.IsEditable = true;
		this.text_rest_key.InvisibleChar = '●';
		this.hbox10.Add(this.text_rest_key);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox10[this.text_rest_key]));
		w12.Position = 3;
		// Container child hbox10.Gtk.Box+BoxChild
		this.btn_edit_name = new global::Gtk.Button();
		this.btn_edit_name.CanFocus = true;
		this.btn_edit_name.Name = "btn_edit_name";
		this.btn_edit_name.UseUnderline = true;
		this.btn_edit_name.Label = global::Mono.Unix.Catalog.GetString("编辑名称");
		this.hbox10.Add(this.btn_edit_name);
		global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox10[this.btn_edit_name]));
		w13.Position = 4;
		w13.Expand = false;
		w13.Fill = false;
		// Container child hbox10.Gtk.Box+BoxChild
		this.fixed4 = new global::Gtk.Fixed();
		this.fixed4.Name = "fixed4";
		this.fixed4.HasWindow = false;
		this.hbox10.Add(this.fixed4);
		global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox10[this.fixed4]));
		w14.Position = 5;
		w14.Padding = ((uint)(20));
		this.hbox9.Add(this.hbox10);
		global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox9[this.hbox10]));
		w15.Position = 2;
		// Container child hbox9.Gtk.Box+BoxChild
		this.hbox12 = new global::Gtk.HBox();
		this.hbox12.Name = "hbox12";
		this.hbox12.Spacing = 6;
		// Container child hbox12.Gtk.Box+BoxChild
		this.label4 = new global::Gtk.Label();
		this.label4.Name = "label4";
		this.label4.LabelProp = global::Mono.Unix.Catalog.GetString("关卡数量：");
		this.hbox12.Add(this.label4);
		global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.hbox12[this.label4]));
		w16.Position = 0;
		w16.Expand = false;
		w16.Fill = false;
		// Container child hbox12.Gtk.Box+BoxChild
		this.text_level_count = new global::Gtk.Entry();
		this.text_level_count.WidthRequest = 100;
		this.text_level_count.CanFocus = true;
		this.text_level_count.Name = "text_level_count";
		this.text_level_count.IsEditable = true;
		this.text_level_count.InvisibleChar = '●';
		this.hbox12.Add(this.text_level_count);
		global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.hbox12[this.text_level_count]));
		w17.Position = 1;
		w17.Expand = false;
		w17.Fill = false;
		this.hbox9.Add(this.hbox12);
		global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox9[this.hbox12]));
		w18.Position = 3;
		this.GtkAlignment1.Add(this.hbox9);
		this.MAP_FRAME.Add(this.GtkAlignment1);
		this.MAP_SELECTOR = new global::Gtk.Label();
		this.MAP_SELECTOR.Name = "MAP_SELECTOR";
		this.MAP_SELECTOR.LabelProp = global::Mono.Unix.Catalog.GetString("<b>地图</b>");
		this.MAP_SELECTOR.UseMarkup = true;
		this.MAP_FRAME.LabelWidget = this.MAP_SELECTOR;
		this.vbox2.Add(this.MAP_FRAME);
		global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.MAP_FRAME]));
		w21.Position = 1;
		w21.Expand = false;
		w21.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.FOOD_CUSTOMER = new global::Gtk.Frame();
		this.FOOD_CUSTOMER.Name = "FOOD_CUSTOMER";
		this.FOOD_CUSTOMER.ShadowType = ((global::Gtk.ShadowType)(2));
		this.FOOD_CUSTOMER.BorderWidth = ((uint)(5));
		// Container child FOOD_CUSTOMER.Gtk.Container+ContainerChild
		this.GtkAlignment2 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment2.Name = "GtkAlignment2";
		this.GtkAlignment2.LeftPadding = ((uint)(12));
		// Container child GtkAlignment2.Gtk.Container+ContainerChild
		this.hbox16 = new global::Gtk.HBox();
		this.hbox16.Name = "hbox16";
		this.hbox16.Spacing = 6;
		// Container child hbox16.Gtk.Box+BoxChild
		this.frame_food = new global::Gtk.Frame();
		this.frame_food.Name = "frame_food";
		this.frame_food.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child frame_food.Gtk.Container+ContainerChild
		this.GtkAlignment3 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment3.Name = "GtkAlignment3";
		this.GtkAlignment3.LeftPadding = ((uint)(12));
		// Container child GtkAlignment3.Gtk.Container+ContainerChild
		this.vbox9 = new global::Gtk.VBox();
		this.vbox9.Name = "vbox9";
		this.vbox9.Spacing = 6;
		// Container child vbox9.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.food_list = new global::Gtk.TreeView();
		this.food_list.WidthRequest = 250;
		this.food_list.HeightRequest = 500;
		this.food_list.CanFocus = true;
		this.food_list.Name = "food_list";
		this.GtkScrolledWindow.Add(this.food_list);
		this.vbox9.Add(this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.vbox9[this.GtkScrolledWindow]));
		w23.Position = 0;
		this.GtkAlignment3.Add(this.vbox9);
		this.frame_food.Add(this.GtkAlignment3);
		this.label_foodlist = new global::Gtk.Label();
		this.label_foodlist.Name = "label_foodlist";
		this.label_foodlist.LabelProp = global::Mono.Unix.Catalog.GetString("<b>食物列表：</b>");
		this.label_foodlist.UseMarkup = true;
		this.frame_food.LabelWidget = this.label_foodlist;
		this.hbox16.Add(this.frame_food);
		global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.hbox16[this.frame_food]));
		w26.Position = 0;
		w26.Expand = false;
		w26.Fill = false;
		w26.Padding = ((uint)(10));
		// Container child hbox16.Gtk.Box+BoxChild
		this.frame_customer = new global::Gtk.Frame();
		this.frame_customer.Name = "frame_customer";
		this.frame_customer.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child frame_customer.Gtk.Container+ContainerChild
		this.GtkAlignment4 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment4.Name = "GtkAlignment4";
		this.GtkAlignment4.LeftPadding = ((uint)(12));
		// Container child GtkAlignment4.Gtk.Container+ContainerChild
		this.vbox10 = new global::Gtk.VBox();
		this.vbox10.Name = "vbox10";
		this.vbox10.Spacing = 6;
		// Container child vbox10.Gtk.Box+BoxChild
		this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
		this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
		this.customer_list = new global::Gtk.TreeView();
		this.customer_list.WidthRequest = 250;
		this.customer_list.CanFocus = true;
		this.customer_list.Name = "customer_list";
		this.GtkScrolledWindow1.Add(this.customer_list);
		this.vbox10.Add(this.GtkScrolledWindow1);
		global::Gtk.Box.BoxChild w28 = ((global::Gtk.Box.BoxChild)(this.vbox10[this.GtkScrolledWindow1]));
		w28.Position = 0;
		this.GtkAlignment4.Add(this.vbox10);
		this.frame_customer.Add(this.GtkAlignment4);
		this.label_customerlist = new global::Gtk.Label();
		this.label_customerlist.Name = "label_customerlist";
		this.label_customerlist.LabelProp = global::Mono.Unix.Catalog.GetString("<b>顾客列表：</b>");
		this.label_customerlist.UseMarkup = true;
		this.frame_customer.LabelWidget = this.label_customerlist;
		this.hbox16.Add(this.frame_customer);
		global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.hbox16[this.frame_customer]));
		w31.Position = 1;
		w31.Expand = false;
		w31.Fill = false;
		w31.Padding = ((uint)(10));
		// Container child hbox16.Gtk.Box+BoxChild
		this.frame_special_customer = new global::Gtk.Frame();
		this.frame_special_customer.Name = "frame_special_customer";
		this.frame_special_customer.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child frame_special_customer.Gtk.Container+ContainerChild
		this.GtkAlignment5 = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment5.Name = "GtkAlignment5";
		this.GtkAlignment5.LeftPadding = ((uint)(12));
		// Container child GtkAlignment5.Gtk.Container+ContainerChild
		this.vbox11 = new global::Gtk.VBox();
		this.vbox11.Name = "vbox11";
		this.vbox11.Spacing = 6;
		// Container child vbox11.Gtk.Box+BoxChild
		this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
		this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
		this.special_customer_list = new global::Gtk.TreeView();
		this.special_customer_list.WidthRequest = 250;
		this.special_customer_list.CanFocus = true;
		this.special_customer_list.Name = "special_customer_list";
		this.GtkScrolledWindow2.Add(this.special_customer_list);
		this.vbox11.Add(this.GtkScrolledWindow2);
		global::Gtk.Box.BoxChild w33 = ((global::Gtk.Box.BoxChild)(this.vbox11[this.GtkScrolledWindow2]));
		w33.Position = 0;
		this.GtkAlignment5.Add(this.vbox11);
		this.frame_special_customer.Add(this.GtkAlignment5);
		this.label_speciallist = new global::Gtk.Label();
		this.label_speciallist.Name = "label_speciallist";
		this.label_speciallist.LabelProp = global::Mono.Unix.Catalog.GetString("<b>特殊顾客列表：</b>");
		this.label_speciallist.UseMarkup = true;
		this.frame_special_customer.LabelWidget = this.label_speciallist;
		this.hbox16.Add(this.frame_special_customer);
		global::Gtk.Box.BoxChild w36 = ((global::Gtk.Box.BoxChild)(this.hbox16[this.frame_special_customer]));
		w36.Position = 2;
		w36.Expand = false;
		w36.Fill = false;
		w36.Padding = ((uint)(10));
		this.GtkAlignment2.Add(this.hbox16);
		this.FOOD_CUSTOMER.Add(this.GtkAlignment2);
		this.GtkLabel6 = new global::Gtk.Label();
		this.GtkLabel6.Name = "GtkLabel6";
		this.GtkLabel6.LabelProp = global::Mono.Unix.Catalog.GetString("<b>食物与顾客</b>");
		this.GtkLabel6.UseMarkup = true;
		this.FOOD_CUSTOMER.LabelWidget = this.GtkLabel6;
		this.vbox2.Add(this.FOOD_CUSTOMER);
		global::Gtk.Box.BoxChild w39 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.FOOD_CUSTOMER]));
		w39.Position = 2;
		w39.Padding = ((uint)(5));
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbuttonbox3 = new global::Gtk.HButtonBox();
		this.hbuttonbox3.Name = "hbuttonbox3";
		this.hbuttonbox3.BorderWidth = ((uint)(5));
		this.hbuttonbox3.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(2));
		// Container child hbuttonbox3.Gtk.ButtonBox+ButtonBoxChild
		this.btn_save = new global::Gtk.Button();
		this.btn_save.CanFocus = true;
		this.btn_save.Name = "btn_save";
		this.btn_save.UseUnderline = true;
		this.btn_save.Label = global::Mono.Unix.Catalog.GetString("保存");
		this.hbuttonbox3.Add(this.btn_save);
		global::Gtk.ButtonBox.ButtonBoxChild w40 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox3[this.btn_save]));
		w40.Expand = false;
		w40.Fill = false;
		// Container child hbuttonbox3.Gtk.ButtonBox+ButtonBoxChild
		this.btn_edit_order = new global::Gtk.Button();
		this.btn_edit_order.CanFocus = true;
		this.btn_edit_order.Name = "btn_edit_order";
		this.btn_edit_order.UseUnderline = true;
		this.btn_edit_order.Label = global::Mono.Unix.Catalog.GetString("编辑餐厅订单");
		this.hbuttonbox3.Add(this.btn_edit_order);
		global::Gtk.ButtonBox.ButtonBoxChild w41 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox3[this.btn_edit_order]));
		w41.Position = 1;
		w41.Expand = false;
		w41.Fill = false;
		// Container child hbuttonbox3.Gtk.ButtonBox+ButtonBoxChild
		this.btn_edit_level = new global::Gtk.Button();
		this.btn_edit_level.CanFocus = true;
		this.btn_edit_level.Name = "btn_edit_level";
		this.btn_edit_level.UseUnderline = true;
		this.btn_edit_level.Label = global::Mono.Unix.Catalog.GetString("编辑关卡");
		this.hbuttonbox3.Add(this.btn_edit_level);
		global::Gtk.ButtonBox.ButtonBoxChild w42 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox3[this.btn_edit_level]));
		w42.Position = 2;
		w42.Expand = false;
		w42.Fill = false;
		this.vbox2.Add(this.hbuttonbox3);
		global::Gtk.Box.BoxChild w43 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbuttonbox3]));
		w43.Position = 3;
		w43.Expand = false;
		w43.Fill = false;
		w43.Padding = ((uint)(5));
		this.Add(this.vbox2);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.DefaultWidth = 945;
		this.DefaultHeight = 822;
		this.Show();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
		this.browse_dbpath.Clicked += new global::System.EventHandler(this.OnBrowseDbpathClicked);
		this.btn_reload.Clicked += new global::System.EventHandler(this.OnBtnReloadClicked);
		this.map_index.Changed += new global::System.EventHandler(this.OnMapIndexChanged);
		this.btn_save.Clicked += new global::System.EventHandler(this.OnBtnSaveClicked);
		this.btn_edit_order.Clicked += new global::System.EventHandler(this.OnBtnEditOrderClicked);
		this.btn_edit_level.Clicked += new global::System.EventHandler(this.OnBtnEditLevelClicked);
	}
}
