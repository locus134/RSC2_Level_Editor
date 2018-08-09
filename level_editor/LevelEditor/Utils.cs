using System;
using System.Collections.Generic;
using System.IO;
using Gdk;
using Gtk;
using Ministone.GameCore.GameData;
using Newtonsoft.Json;

namespace LevelEditor
{
    public class Utils
    {
        static public AppConfig AppConfig = new AppConfig();
        static private string DIR_SEPRATOR = System.IO.Path.DirectorySeparatorChar.ToString();
        static private string CONFIG_PATH = Directory.GetCurrentDirectory() + DIR_SEPRATOR + "config.json";
        static private Dictionary<string, string> SUPPORTED_LANGUAGES;

        private Utils()
        {
        }

        public static void DeserializeAppConfig()
        {
            if (File.Exists(CONFIG_PATH))
            {
                string jsonStr = File.ReadAllText(CONFIG_PATH);
                Utils.AppConfig = JsonConvert.DeserializeObject<AppConfig>(jsonStr);
            }
        }

        public static void SerializeAppConfig()
        {
            string jsonStr = JsonConvert.SerializeObject(Utils.AppConfig);
            File.WriteAllText(CONFIG_PATH, jsonStr);
        }

        private static void InitLangs()
        {
            if (SUPPORTED_LANGUAGES == null)
            {
                SUPPORTED_LANGUAGES = new Dictionary<string, string>();
                SUPPORTED_LANGUAGES.Add("英文", "en");
                SUPPORTED_LANGUAGES.Add("简体中文", "cn");
                SUPPORTED_LANGUAGES.Add("繁体中文", "zh-tw");
                SUPPORTED_LANGUAGES.Add("德文", "de");
                SUPPORTED_LANGUAGES.Add("西班牙文", "es");
                SUPPORTED_LANGUAGES.Add("法文", "fr");
                SUPPORTED_LANGUAGES.Add("意大利文", "it");
                SUPPORTED_LANGUAGES.Add("葡萄牙文", "pt");
                SUPPORTED_LANGUAGES.Add("俄文", "ru");
                SUPPORTED_LANGUAGES.Add("日文", "ja");
                SUPPORTED_LANGUAGES.Add("韩文", "ko");
                SUPPORTED_LANGUAGES.Add("泰文", "th");
                SUPPORTED_LANGUAGES.Add("印尼文", "id");
            }
        }

        public static List<string> GetSupportedLanauges()
        {
            InitLangs();
            return new List<string>(SUPPORTED_LANGUAGES.Keys);
        }

        public static string GetLanguageCode(string lang)
        {
            InitLangs();
            string code;
            SUPPORTED_LANGUAGES.TryGetValue(lang, out code);
            return code;
        }

        public static string GetLanguageName(string code)
        {
            InitLangs();
            foreach(var item in SUPPORTED_LANGUAGES)
            {
                if (code.Equals(item.Value))
                {
                    return item.Key;
                }
            }
            return null;
        }

        public static bool ParseInteger(string text, out int ret, Gtk.Window parent)
        {
            if (Int32.TryParse(text, out ret))
            {
                return true;
            }
            MessageDialog dlg = new MessageDialog(parent, DialogFlags.Modal, MessageType.Error, ButtonsType.Close,
                                                  "输入的\"{0}\"不是一个有效的整数", text);
            dlg.Run();
            dlg.Destroy();
            return false;
        }

        public static bool ParseFloat(string text, out float ret, Gtk.Window parent)
        {
            if (float.TryParse(text, out ret))
            {
                return true;
            }
            MessageDialog dlg = new MessageDialog(parent, DialogFlags.Modal, MessageType.Error, ButtonsType.Close,
                                                  "输入的\"{0}\"不是一个有效的数字", text);
            dlg.Run();
            dlg.Destroy();
            return false;
        }

        public static void ShowFoodList(List<string> foodList, TreeView treeview, string foodTexPath = null)
        {
            if (foodTexPath == null)
            {
                foodTexPath = AppConfig.sd_path;
            }
            FoodDataManager _foodMgr = FoodDataManager.GetInstance();
            ListStore foodListStore = new ListStore(typeof(Pixbuf), typeof(string), typeof(string));
            for (int i = 0; i < foodList.Count; ++i)
            {
                FoodData fd = _foodMgr.GetFood(foodList[i]);
                if (fd != null)
                {
                    Pixbuf pixBuf = new Pixbuf(foodTexPath + fd.texture);
                    float scale = 60.0f / pixBuf.Height;

                    Pixbuf scaledBuf = pixBuf.ScaleSimple((int)(pixBuf.Width * scale),
                                                              (int)(pixBuf.Height * scale), InterpType.Bilinear);
                    foodListStore.AppendValues(scaledBuf, fd.GetDisplayName("cn"), fd.key);
                }
            }
            treeview.Model = foodListStore;
        }

        public static void ShowCustomerList(List<string> customerList, TreeView treeView, string customerTexPath = null)
        {
            if (customerTexPath == null)
            {
                customerTexPath = AppConfig.sd_path;
            }
            string seperator = System.IO.Path.DirectorySeparatorChar.ToString();
            CustomerDataManager _custMgr = CustomerDataManager.GetInstance();
            ListStore custListStore = new ListStore(typeof(Pixbuf), typeof(string), typeof(string));
            for (int i = 0; i < customerList.Count; ++i)
            {
                CustomerData cust = _custMgr.GetCustomer(customerList[i]);
                if (cust != null)
                {
                    Pixbuf pixBuf = new Pixbuf(customerTexPath + cust.icon_texture);
                    float scale = 80.0f / pixBuf.Height;

                    Pixbuf scaledBuf = pixBuf.ScaleSimple((int)(pixBuf.Width * scale), (int)(pixBuf.Height * scale), InterpType.Bilinear);
                    custListStore.AppendValues(scaledBuf, cust.GetDisplayName("cn"), cust.key);
                }
            }
            treeView.Model = custListStore;
        }

        public static string GetFoodImagePath(string food)
        {
            FoodData fd = FoodDataManager.GetInstance().GetFood(food);
            return AppConfig.sd_path + fd.texture;
        }

        public static string GetFoodImagePath(FoodData fd)
        {
            return AppConfig.sd_path + fd.texture;
        }

        public static string GetCustomerImagePath(string customer)
        {
            CustomerData cust = CustomerDataManager.GetInstance().GetCustomer(customer);
            return AppConfig.sd_path + cust.icon_texture;
        }

        public static string GetCustomerImagePath(CustomerData cust)
        {
            return AppConfig.sd_path + cust.icon_texture;
        }

        public static void SelectTreeRow(TreeView treeView, int row)
        {
            TreeIter iter;
            if (treeView.Model.IterNthChild(out iter, row))
            {
                treeView.Selection.SelectIter(iter);
            }
        }

        public static Pixbuf GetGeneralIcon()
        {
            Pixbuf pb = Pixbuf.LoadFromResource("LevelEditor.question.png");
            return pb;
        }
    }
}
