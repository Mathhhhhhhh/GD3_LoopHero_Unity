using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameDatas gameDatas;

    public void ContinueGame()
    {
        //TO DO: CHARGER LA DERNIERE SAUVEGARDE
        GetComponent<SaveManager>().LoadGame();
        //TO DO: CHARGER LE DERNIER NIVEAU JOUé
        if (gameDatas.Datas.IsPlayerInMiniGame)
        {
            SceneManager.LoadScene(gameDatas.Datas.MiniGameNumber);
        }
        else
        {
            SceneManager.LoadScene(0);
        }

    }
}
