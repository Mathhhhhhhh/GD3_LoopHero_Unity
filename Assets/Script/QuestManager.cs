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

    public bool HasWood
    {
        get { return _hasWood; }
        set { _hasWood = value; }
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
