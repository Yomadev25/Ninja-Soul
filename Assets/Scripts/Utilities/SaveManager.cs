using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public const string MessageSaveBegin = "Save Begin";
    public const string MessageSaveDone = "Save Done";
    public const string MessageLoadBegin = "Load Begin";
    public const string MessageLoadDone = "Load Done";

    public void Save(Player player)
    {
        MessagingCenter.Send(this, MessageSaveBegin);

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + $"/{player.id}.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        Player data = new Player(player);
        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Saved data in " + path);
        MessagingCenter.Send(this, MessageSaveDone);
    }

    public Player Load(int id)
    {
        MessagingCenter.Send(this, MessageLoadBegin);
        string path = Application.persistentDataPath + $"/{id}.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Player data = formatter.Deserialize(stream) as Player;
            stream.Close();

            MessagingCenter.Send(this, MessageLoadDone);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            MessagingCenter.Send(this, MessageLoadDone);
            return null;
        }
    }

    public List<string> GetSaveFiles()
    {
        string directoryPath = Application.persistentDataPath;
        DirectoryInfo directory = new DirectoryInfo(directoryPath);

        FileInfo[] saveFiles = directory.GetFiles("*.save");
        List<string> saveFileNames = saveFiles.Select(file => Path.GetFileNameWithoutExtension(file.Name)).ToList();

        return saveFileNames;
    }
}
