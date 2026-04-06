using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameDatas gameDatas;
    private SaveController saveController;

    private void Start()
    {
        saveController = new SaveController();
    }
    public void SaveGame()
    {
        saveController.SaveGameData(gameDatas.Datas,"savegame.txt");
    }

    public void LoadGame()
    {
        gameDatas.Datas = saveController.LoadGameData("savegame.txt");
    }
}
