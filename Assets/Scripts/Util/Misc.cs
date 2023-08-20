using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;


static class Util
{
    static string prefabPath = "Prefabs/";
    static string tableFolderPath = Application.dataPath + "/CsvTable/";

    static public void PrintBox(Vector2 topLeft, Vector2 bottomRight)
    {
        PrintBox(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    static public void PrintBox(float x1, float y1, float x2, float y2)
    {
        Debug.DrawLine(new Vector2(x1, y1), new Vector2(x2, y1), Color.green);
        Debug.DrawLine(new Vector2(x1, y1), new Vector2(x1, y2), Color.green);
        Debug.DrawLine(new Vector2(x2, y2), new Vector2(x2, y1), Color.green);
        Debug.DrawLine(new Vector2(x2, y2), new Vector2(x1, y2), Color.green);
    }


    static public bool ContainsAll<T>(List<T> l1, List<T> l2)
    {
        if (l1.Count == 0 || l2 == null || l2.Count == 0 || l1.Count < l2.Count)
            return false;

        bool[] check = new bool[l1.Count];
        for (int i2 = 0; i2 < l2.Count; i2++)
        {
            bool found = false;
            for (int i1 = 0; i1 < l1.Count; i1++)
            {
                if (l2[i2].Equals(l1[i1]) && !check[i1])
                {
                    check[i1] = true;
                    found = true;
                    break;
                }
            }
            if (!found)
                return false;
        }
        return true;
    }

    static public GameObject LoadPrefab(string path)
    {
        string finalPath = prefabPath + path;
        return Resources.Load<GameObject>(finalPath);
    }

    static public Dictionary<string, Dictionary<string, string>> ReadCsv(string filePath)
    {
        string path = tableFolderPath + filePath;
        using StreamReader reader = new(path, Encoding.UTF8);
        Dictionary<string, Dictionary<string, string>> result = new();
        string[] fields = reader.ReadLine().Split(",");
        string line = null;
        while ((line = reader.ReadLine()) != null)
        {
            string[] lineDataTmp = line.Split(",");
            Dictionary<string, string> lineData = new();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] != string.Empty)
                    lineData.Add(fields[i], lineDataTmp[i]);
            }
            result[lineData["id"]] = lineData;//todo 改回Add
        }
        return result;
    }



    static public string PrintProperties(object obj)
    {
        StringBuilder sb = new();
        Type type = obj.GetType();
        foreach (PropertyInfo p in type.GetProperties())
        {
            sb.Append(p.Name);
            sb.Append(": ");
            sb.Append(p.GetValue(obj));
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
