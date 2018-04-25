using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveDataHelper {

    static string saveFilePath = "Assets/Levels/";

    /**
     * Loads the save data from the disk
     */
    public static List<string> getNamesOfSaves()
    {

        List<string> saveList = new List<string>();
        DirectoryInfo info = new DirectoryInfo(saveFilePath);
        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo)
        {
            if(file.Name.EndsWith(".level"))
                saveList.Add(file.Name);
        }

        return saveList;
    }

    /**
     * Loads the save data from the disk
     */
    public static MapData loadMapData(string saveName)
    {

        MapData mapData = new MapData();
        string savePath = saveFilePath  + saveName;
        Debug.Log("Loading file from disk " + savePath);
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            List<TileSave> tiles = (List<TileSave>)bf.Deserialize(file);
            Debug.Log(tiles);
            mapData.tiles = tiles;
            mapData.mapName = saveName.Substring(0,saveName.Length-6);
            file.Close();
        }
        return mapData;
    }


    /**
     * Saves the save data to the disk
     */
    public static void updateSaveFile(MapData data, string savePath)
    {

        Debug.Log("Saving file to disk " + savePath);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        file.SetLength(0);
        bf.Serialize(file, data.tiles);
        file.Close();
    }

    /**
     * Saves the save data to the disk
     */
    public static string createSaveFile(MapData data)
    {
        string saveName = createMapSaveName(data);
        string savePath = saveFilePath + saveName;
        //string jsonString = JsonUtility.ToJson(data);
        Debug.Log("Saving file to disk " + savePath);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        bf.Serialize(file, data);
        file.Close();
        return saveName;
    }

    /**
     * Saves the save data to the disk
     */
    public static void createSaveFile(MapData data, string savePath)
    {
        Debug.Log("Saving file to disk " + savePath);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        //string jsonString = JsonUtility.ToJson(data.tiles);
        bf.Serialize(file, data.tiles);
        file.Close();
    }

    public static void saveFile(MapData data, string saveName)
    {
        string savePath = saveFilePath + saveName + ".level";
        if(File.Exists(savePath))
        {
            updateSaveFile(data, savePath);
        } else
        {
            createSaveFile(data, savePath);
        }
    }

    public static string createMapSaveName(MapData data)
    {
        return data.mapName;
    }
}
