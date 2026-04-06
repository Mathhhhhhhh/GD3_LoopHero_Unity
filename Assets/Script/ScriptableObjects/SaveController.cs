using UnityEngine;
using System.IO;

public struct GameDatasStruc
{
    public int PlayerCellNumber;

    public int IsPlayerInMiniGame;

    public int MiniGameNumber;
}

public class SaveController
{   
    public void SaveGameData(GameDatasStruc gameDatas, string filename)
    {
        string data = JsonUtility.ToJson(gameDatas);

        string path = Application.persistentDataPath + "/" + filename;

        File.WriteAllText(path, data);
    }

    public GameDatasStruc LoadGameData(string filename)
    {
        GameDatasStruc gameDatas = new GameDatasStruc();

        string path = Application.persistentDataPath + "/" + filename;

        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            gameDatas = JsonUtility.FromJson<GameDatasStruc>(data);
        }

        else
        {
            SaveGameData(gameDatas, filename);
        }

            return gameDatas;
    }
}
