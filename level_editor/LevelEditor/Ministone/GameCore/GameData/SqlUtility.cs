using System;
using System.Data.SQLite;
using Ministone.GameCore.GameData.Generic;

namespace Ministone.GameCore.GameData
{
    public static class SqlUtility
    {
        public static string GetStringSafe(this SQLiteDataReader reader, int col)
        {
            object value = reader.GetValue(col);
            if (!Convert.IsDBNull(value))
            {
                return value.ToString();
            }
            return "";
        }

        public static int GetInt32Safe(this SQLiteDataReader reader, int col)
        {
            object value = reader.GetValue(col);
            if (!Convert.IsDBNull(value))
            {
                string str = value.ToString();
                if(!string.IsNullOrEmpty(str))
                {
                    return str.ToInt32();
                }
            }
            return 0;
        }

        public static float GetFloatSafe(this SQLiteDataReader reader, int col)
        {
            object value = reader.GetValue(col);
            if (!Convert.IsDBNull(value))
            {
                string str = value.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    return Convert.ToSingle(str);
                }
            }
            return 0.0f;
        }
    }
}
