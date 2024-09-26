using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordConnectByFinix
{
    public static class Word_AllPrefs
    {
        public static int unlockedLevel
        {
            get
            {
                if (Word_GameState.unlockedLevel == -1)
                {
                    int value = Word_AllPlayerPrefs.GetInt("unlocked_level");
                    Word_GameState.unlockedLevel = value;
                }
                return Word_GameState.unlockedLevel;
            }
            set { Word_AllPlayerPrefs.SetInt("unlocked_level", value); Word_GameState.unlockedLevel = value; }
        }

        public static List<int> GetPanWordIndexes(int level)
        {
            string data = PlayerPrefs.GetString("pan_word_indexes_v2_" + level);
            return Word_Utils.BuildListFromString<int>(data);
        }

        public static void SetPanWordIndexes(int level, int[] indexes)
        {
            string data = Word_Utils.BuildStringFromCollection(indexes);
            PlayerPrefs.SetString("pan_word_indexes_v2_" + level, data);
        }

        public static bool IsLastLevel()
        {
            return Word_GameState.currentLevel == unlockedLevel;
        }

        public static void SetExtraWords(int level, string[] extraWords)
        {
            Word_AllPlayerPrefsSet.SetStringArray("extra_words_" + level, extraWords);
        }

        public static string[] GetExtraWords(int level)
        {
            return Word_AllPlayerPrefsSet.GetStringArray("extra_words_" + level);
        }

        public static int extraProgress
        {
            get { return Word_AllPlayerPrefs.GetInt("extra_progress", 0); }
            set { Word_AllPlayerPrefs.SetInt("extra_progress", value); }
        }

        public static int extraTarget
        {
            get { return Word_AllPlayerPrefs.GetInt("extra_target", 5); }
            set { Word_AllPlayerPrefs.SetInt("extra_target", value); }
        }

        public static int totalExtraAdded
        {
            get { return Word_AllPlayerPrefs.GetInt("total_extra_added", 0); }
            set { Word_AllPlayerPrefs.SetInt("total_extra_added", value); }
        }

        public static string[] levelProgress
        {
            get { return Word_AllPlayerPrefsSet.GetStringArray("level_progress"); }
            set { Word_AllPlayerPrefsSet.SetStringArray("level_progress", value); }
        }

        public static void AddToNumHint(int level)
        {
            int numHint = GetNumHint(level);
            PlayerPrefs.SetInt("numhint_used_" + level, numHint + 1);
        }

        public static int GetNumHint(int level)
        {
            return PlayerPrefs.GetInt("numhint_used_" + level);
        }
    }
}