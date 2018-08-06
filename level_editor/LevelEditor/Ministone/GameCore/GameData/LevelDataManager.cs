using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ministone.GameCore.GameData
{
    public class LevelDataManager
    {
        static private LevelDataManager _instance = null;
        Dictionary<string, List<LevelData>> m_levelDataDict;
        Dictionary<string, Dictionary<int, int>> m_levelIndexes;


        private LevelDataManager()
        {
            m_levelDataDict = new Dictionary<string, List<LevelData>>();
            m_levelIndexes = new Dictionary<string, Dictionary<int, int>>();
        }

        static public LevelDataManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LevelDataManager();
            }
            return _instance;
        }

        public void Clear()
        {
            m_levelDataDict.Clear();
            m_levelIndexes.Clear();
        }

        public bool LoadFromDBFile(string restaurant, string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr) || string.IsNullOrEmpty(restaurant))
            {
                return false;
            }

            List<LevelData> levels = JsonConvert.DeserializeObject<List<LevelData>>(jsonStr);
            if (levels == null)
            {
                Debug.WriteLine("Failed to load level data for {0} from JSON!", restaurant);
                return false;
            }

            SetRestauranttLevelData(restaurant, levels);
            return true;
        }

        public string GetJsonString(string restaurant)
        {
            string ret = null;
            List<LevelData> levels;
            if (m_levelDataDict.TryGetValue(restaurant, out levels))
            {
                ret = JsonConvert.SerializeObject(levels, Formatting.Indented);
            }

            return ret;
        }

        public void SetRestauranttLevelData(string restaurant, List<LevelData> levels)
        {
            if (m_levelDataDict.ContainsKey(restaurant))
            {
                m_levelDataDict.Remove(restaurant);
            }
            if (m_levelIndexes.ContainsKey(restaurant))
            {
                m_levelIndexes.Remove(restaurant);
            }

            m_levelDataDict.Add(restaurant, levels);
            Dictionary<int, int> indexDict = new Dictionary<int, int>();
            for (int i = 0; i < levels.Count; ++ i)
            {
                LevelData lvd = levels[i];
                indexDict.Add(lvd.id, i);
            }
            m_levelIndexes.Add(restaurant, indexDict);
        }

        public LevelData GetLevelData(string restaurant, int levelId)
        {
            LevelData lvd = null;

            Dictionary<int, int> indexDict;
            List<LevelData> levels;
            int index;
            if (m_levelDataDict.TryGetValue(restaurant, out levels) 
                && m_levelIndexes.TryGetValue(restaurant, out indexDict) 
                && indexDict.TryGetValue(levelId, out index))
            {
                lvd = levels[index];
            }
            return lvd;
        }

        public List<LevelData> GetAllLevels(string restaurant)
        {
            List<LevelData> ret = null;
            m_levelDataDict.TryGetValue(restaurant, out ret);
            return ret;
        }
    }
}
