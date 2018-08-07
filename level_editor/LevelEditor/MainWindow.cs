using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Gtk;
using LevelEditor;
using Ministone.GameCore.GameData;
using System.Data.SQLite;

public partial class MainWindow : Gtk.Window
{
    MapDataManager _restMgr = MapDataManager.GetInstance();
    CookwareDataManager _cwMgr = CookwareDataManager.GetInstance();
    IngredientDataManager _ingMgr = IngredientDataManager.GetInstance();
    FoodDataManager _foodMgr = FoodDataManager.GetInstance();
    CustomerDataManager _custMgr = CustomerDataManager.GetInstance();
    OrderDataManager _ordMgr = OrderDataManager.GetInstance();
    LevelDataManager _lvlMgr = LevelDataManager.GetInstance();

    const string RESTAURANT_FILE = "GameConfig.s3db";
    const string COOKWARE_FILE = "CookingWareInfo.s3db";
    const string INGREDIENT_FILE = "MaterialInfo.s3db";
    const string FOOD_FILE = "FoodInfo.s3db";
    const string CUSTOMER_FILE = "customer.s3db";

    //const string ORDER_FILE = "order.json";
    const string LEVEL_DIR = "levels";
    string DIR_SEPRATOR;

    string m_restFilePath;
    string m_cookwareFilePath;
    string m_ingFilePath;
    string m_foodFilePath;
    string m_custFilePath;
    string m_orderFilePath;
    string m_levelFilePath;

    string m_configFilePath;
    MapData m_curRestData;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        DIR_SEPRATOR = System.IO.Path.DirectorySeparatorChar.ToString();

        this.Build();
        this.SetPosition(WindowPosition.CenterAlways);

        food_list.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
        food_list.AppendColumn("名称", new CellRendererText(), "text", 1);

        customer_list.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
        customer_list.AppendColumn("名称", new CellRendererText(), "text", 1);

        special_customer_list.AppendColumn("图片", new CellRendererPixbuf(), "pixbuf", 0);
        special_customer_list.AppendColumn("名称", new CellRendererText(), "text", 1);

        m_configFilePath = string.Format("{0}{1}config.json", Directory.GetCurrentDirectory(), DIR_SEPRATOR);
        Deserialize();
        if (!string.IsNullOrEmpty(Utils.AppConfig.db_path))
        {
            text_dbpath.Text = Utils.AppConfig.db_path;
            ReloadData();
        }
    }

    protected void Deserialize()
    {
        if (File.Exists(m_configFilePath))
        {
            string jsonStr = File.ReadAllText(m_configFilePath);
            Utils.AppConfig = JsonConvert.DeserializeObject<AppConfig>(jsonStr);
        }
    }

    protected void Serialize()
    {
        string jsonStr = JsonConvert.SerializeObject(Utils.AppConfig);
        File.WriteAllText(m_configFilePath, jsonStr);
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Serialize();
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnBtnEditLevelClicked(object sender, EventArgs e)
    {
        LevelDialog dlg = new LevelDialog(this, m_curRestData.key);
        dlg.Run();
        dlg.Destroy();
    }

    protected void OnBrowseDbpathClicked(object sender, EventArgs e)
    {
        FileChooserDialog dlg = new FileChooserDialog("选择文件夹", this,
                                                      FileChooserAction.SelectFolder, 
                                                      "选择", ResponseType.Ok, "取消", ResponseType.Close);
        if (dlg.Run() == (int)ResponseType.Ok)
        {
            string dbPath = dlg.Filename;
            if (!dbPath.EndsWith(DIR_SEPRATOR, StringComparison.Ordinal))
            {
                dbPath += DIR_SEPRATOR;
            }

            text_dbpath.Text = dbPath;
            Utils.AppConfig.db_path = dbPath;

            DirectoryInfo parentDir = Directory.GetParent(dlg.Filename);    // Resource
            string rootDir = parentDir.FullName;
            Utils.AppConfig.sd_path = string.Format("{0}{1}sd{2}", rootDir, DIR_SEPRATOR, DIR_SEPRATOR);

            ReloadData();
        }
        dlg.Destroy();

        Serialize();
    }

    void ResetDisplay()
    {
        map_index.Clear();
        text_rest_key.Text = "";
        text_rest_id.Text = "";
        text_level_count.Text = "";
    }

    protected void ReloadLevelData()
    {
        _lvlMgr.Clear();
        List<MapData> restaurants = _restMgr.GetAllMaps();
        foreach (MapData rest in restaurants)
        {
            //string levelFilePath = m_levelFileDir + rest.key + ".json";
            //if (File.Exists(levelFilePath))
            //{
            //    string jsonStr = File.ReadAllText(levelFilePath);
            //    _lvlMgr.LoadFromDBFile(rest.key, jsonStr);
            //}
            _lvlMgr.LoadLevelDataForMap(rest.key);
        }
    }

    protected bool ReloadData()
    {
        ResetDisplay();

        m_restFilePath = Utils.AppConfig.db_path + RESTAURANT_FILE;
        m_cookwareFilePath = Utils.AppConfig.db_path + COOKWARE_FILE;
        m_ingFilePath = Utils.AppConfig.db_path + INGREDIENT_FILE;
        m_foodFilePath = Utils.AppConfig.db_path + FOOD_FILE;
        m_custFilePath = Utils.AppConfig.db_path + CUSTOMER_FILE;
        m_orderFilePath = m_custFilePath;
        m_levelFilePath = m_restFilePath;

        try
        {
            if (File.Exists(m_levelFilePath))
            {
                if (!_lvlMgr.initWithDBFile(m_levelFilePath))
                {
                    return false;
                }
            }



            if (File.Exists(m_cookwareFilePath))
            {
                if (!_cwMgr.LoadFromDBFile(m_cookwareFilePath))
                {
                    return false;
                }
            }

            if (File.Exists(m_ingFilePath))
            {
                if (!_ingMgr.LoadFromDBFile(m_ingFilePath))
                {
                    return false;
                }
            }

            if (File.Exists(m_foodFilePath))
            {
                if (!_foodMgr.LoadFromDBFile(m_foodFilePath))
                {
                    return false;
                }
            }

            if (File.Exists(m_custFilePath))
            {
                if (!_custMgr.LoadFromDBFile(m_custFilePath))
                {
                    return false;
                }
            }

            if (File.Exists(m_orderFilePath))
            {
                if (!_ordMgr.LoadFromDBFile(m_orderFilePath))
                {
                    return false;
                }

            }

            if (File.Exists(m_restFilePath))
            {
                if (_restMgr.LoadFromDBFile(m_restFilePath))
                {
                    ReloadLevelData();
                }
                else
                {
                    return false;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        ReloadMapData();

        return true;
    }

    void ReloadMapData()
    {
        List<MapData> allMapDatas = _restMgr.GetAllMaps();
        map_index.Clear();
        if (allMapDatas.Count > 0)
        {
            CellRendererText cell = new CellRendererText();
            map_index.PackStart(cell, false);
            map_index.AddAttribute(cell, "text", 0);
            ListStore store = new ListStore(typeof(string));
            map_index.Model = store;

            for (int i = 0; i < allMapDatas.Count; ++i)
            {
                MapData restData = allMapDatas[i];
                store.AppendValues(restData.GetDisplayName("cn"));
            }

            int curIndex = 0;
            if (m_curRestData != null)
            {
                Utils.AppConfig.select_restaurant = m_curRestData.GetDisplayName("cn");
            }

            if (Utils.AppConfig.select_restaurant != null)
            {
                TreeIter iter;
                int i = 0;
                map_index.Model.GetIterFirst(out iter);
                do
                {
                    GLib.Value thisRow = new GLib.Value();
                    map_index.Model.GetValue(iter, 0, ref thisRow);
                    if ((thisRow.Val as string).Equals(Utils.AppConfig.select_restaurant))
                    {
                        curIndex = i;
                        break;
                    }
                    ++i;
                } while (map_index.Model.IterNext(ref iter));
            }
            map_index.Active = curIndex;
            m_curRestData = allMapDatas[curIndex];
            RefreshCurrent();
        }
    }

    void ReloadFoodList()
    {
        Utils.ShowFoodList(m_curRestData.food_list, food_list);
    }

    void ReloadCustomerList()
    {
        Utils.ShowCustomerList(m_curRestData.customer_list, customer_list);
        Utils.ShowCustomerList(m_curRestData.special_customer_list, special_customer_list);
    }

    void RefreshCurrent()
    {
        text_rest_key.Text = m_curRestData.key;
        text_rest_id.Text = m_curRestData.id.ToString();
        text_level_count.Text = m_curRestData.level_count.ToString();
        ReloadFoodList();
        ReloadCustomerList();
    }

    protected void OnMapIndexChanged(object sender, EventArgs e)
    {
        m_curRestData = _restMgr.GetAllMaps()[map_index.Active];
        Utils.AppConfig.select_restaurant = m_curRestData.GetDisplayName("cn");
        Serialize();
        RefreshCurrent();
    }

    protected void ShowMessage(string msg)
    {
        MessageDialog msgDlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, msg);
        msgDlg.Run();
        msgDlg.Destroy();
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        if (m_curRestData == null)
        {
            return;
        }

        int id;
        if (!int.TryParse(text_rest_id.Text, out id))
        {
            text_rest_id.Text = "";
            ShowMessage("请输入有效的餐厅ID");
            return;
        }
        int count;
        if (!int.TryParse(text_level_count.Text, out count))
        {
            text_level_count.Text = "";
            ShowMessage("请输入有效的关卡数量");
            return;
        }

        m_curRestData.id = id;
        m_curRestData.key = text_rest_key.Text;
        m_curRestData.level_count = count;

        _restMgr.setMapData(m_curRestData, map_index.Active);

        string jsonStr = _restMgr.GetJsonString();
        File.WriteAllText(m_restFilePath, jsonStr);

        ReloadMapData();
    }

    protected void OnBtnReloadClicked(object sender, EventArgs e)
    {
        ReloadData();
    }

    protected void OnBtnEditOrderClicked(object sender, EventArgs e)
    {
        List<string> customerList = new List<string>(m_curRestData.customer_list);
        customerList.AddRange(m_curRestData.special_customer_list);
        OrderEditor dlg = new OrderEditor(this, m_curRestData.food_list, customerList, m_orderFilePath);
        dlg.Run();
        dlg.Destroy();
    }
}
