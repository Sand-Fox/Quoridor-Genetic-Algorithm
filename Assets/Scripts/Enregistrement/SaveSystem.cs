using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveSystem
{
    private static string path => Application.persistentDataPath + "/Parties";

    public static void Save<T>(T obj, string key)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        Directory.CreateDirectory(path);
        FileStream stream = new FileStream(path + "/" + key, FileMode.Create);
        formatter.Serialize(stream, obj);
        stream.Close();
    }

    public static T Load<T>(string key)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        T data = default;
        if(File.Exists(path + "/" + key))
        {
            FileStream stream = new FileStream(path + "/" + key, FileMode.Open);
            data = (T)formatter.Deserialize(stream);
            stream.Close();
        }
        else Debug.LogError("Save not found in " + path + "/" + key);
        return data;
    }

    public static FileInfo[] GetFiles()
    {
        var info = new DirectoryInfo(path);
        return info.GetFiles();
    }
}
