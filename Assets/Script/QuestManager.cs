using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager _instance;
    public static QuestManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<QuestManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("QuestManager");
                    _instance = go.AddComponent<QuestManager>();
                }
            }
            return _instance;
        }
    }

    private bool _hasWood = false;
    private bool _bucheronQuestAccepted = false;

    public bool HasWood
    {
        get { return _hasWood; }
        set { _hasWood = value; }
    }

    public bool BucheronQuestAccepted
    {
        get { return _bucheronQuestAccepted; }
        set { _bucheronQuestAccepted = value; }
    }

    public void AcceptBucheronQuest()
    {
        _bucheronQuestAccepted = true;
        Debug.Log("<color=green>[QuestManager] Quête du Bucheron acceptée!</color>");
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
}
