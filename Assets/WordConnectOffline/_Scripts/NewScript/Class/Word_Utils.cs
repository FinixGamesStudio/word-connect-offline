using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public static class PluginExtension
{
    public static void Shuffle<T>(this IList<T> list, int? seed = null)
    {
        System.Random rng = seed != null ? new System.Random((int)seed) : new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static void LookAt2D(this Transform transform, Vector3 target, float angle = 0)
    {
        Vector3 dir = target - transform.position;
        angle += Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }

    public static void LookAt2D(this Transform transform, Transform target, float angle = 0)
    {
        LookAt2D(transform, target.position, angle);
    }
}

namespace WordConnectByFinix
{
    public class Word_Utils
    {
        public static T GetRandom<T>(params T[] arr)
        {
            return arr[UnityEngine.Random.Range(0, arr.Length)];
        }


        public static string BuildStringFromCollection(ICollection values, char split = '|')
        {
            string results = "";
            int i = 0;
            foreach (var value in values)
            {
                results += value;
                if (i != values.Count - 1)
                {
                    results += split;
                }
                i++;
            }
            return results;
        }

        public static List<T> BuildListFromString<T>(string values, char split = '|')
        {
            List<T> list = new List<T>();
            if (string.IsNullOrEmpty(values))
                return list;

            string[] arr = values.Split(split);
            foreach (string value in arr)
            {
                if (string.IsNullOrEmpty(value)) continue;
                T val = (T)Convert.ChangeType(value, typeof(T));
                list.Add(val);
            }
            return list;
        }

        public static void LoadScene(int sceneIndex, bool useScreenFader = false)
        {
            if (useScreenFader)
            {
                ConfigController.instance.screenFader.GotoScene(sceneIndex);
            }
            else
            {
                SceneManager.LoadScene(sceneIndex);
            }
        }

       
        public static Vector3 GetMiddlePoint(Vector3 begin, Vector3 end, float delta = 0)
        {
            Vector3 center = Vector3.Lerp(begin, end, 0.5f);
            Vector3 beginEnd = end - begin;
            Vector3 perpendicular = new Vector3(-beginEnd.y, beginEnd.x, 0).normalized;
            Vector3 middle = center + perpendicular * delta;
            return middle;
        }
    }
}