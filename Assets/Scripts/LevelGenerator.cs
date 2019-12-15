using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public TextAsset levelFile;

    public StringToObject[] objects;

    public float nodeWidth = 1, nodeHeight = 1;

    public void GenerateLevel()
    {
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(levelFile.text);

        foreach (NodeData nodeData in saveFile.nodeData)
        {
            if (!string.IsNullOrEmpty(nodeData.spriteValue))
            {
                try
                {
                    GameObject o = Instantiate(GetObjectFromName(nodeData.spriteValue), new Vector2(nodeData.x * nodeWidth, nodeData.y * nodeHeight), Quaternion.identity);
                    o.transform.SetParent(transform);
                }
                catch
                {
                    continue;
                }
            }

            if (!string.IsNullOrEmpty(nodeData.floorValue))
            {
                try
                {
                    GameObject o = Instantiate(GetObjectFromName(nodeData.floorValue), new Vector2(nodeData.x * nodeWidth, nodeData.y * nodeHeight), Quaternion.identity);
                    o.transform.SetParent(transform);
                }
                catch
                {
                    continue;
                }
            }
        }
    }

    public void ClearLevel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public GameObject GetObjectFromName(string name)
    {
        foreach (StringToObject stringToObject in objects)
        {
            if (stringToObject.name.Equals(name))
            {
                return stringToObject.prefab;
            }
        }

        return null;
    }
}

[System.Serializable]
public class StringToObject
{
    public string name;
    public GameObject prefab;

    public StringToObject(string name, GameObject prefab)
    {
        this.name = name;
        this.prefab = prefab;
    }
}

[System.Serializable]
public class NodeData
{
    public int x, y;
    public string spriteValue, floorValue;

    public NodeData(int x, int y, string spriteValue, string floorValue)
    {
        this.x = x;
        this.y = y;
        this.spriteValue = spriteValue;
        this.floorValue = floorValue;
    }
}

[System.Serializable]
public class SaveFile
{
    public string levelName;
    public int width, height;
    public NodeData[] nodeData;

    public SaveFile(string levelName, int width, int height, NodeData[] nodeData)
    {
        this.levelName = levelName;
        this.width = width;
        this.height = height;
        this.nodeData = nodeData;
    }
}
