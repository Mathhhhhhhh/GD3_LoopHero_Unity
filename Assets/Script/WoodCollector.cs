using UnityEngine;

public class WoodCollector : MonoBehaviour, IActionable
{
    [Header("Wood Collection Settings")]
    private bool _hasBeenCollected = false;

    [Header("Object to Remove")]
    [SerializeField] private GameObject _objectToRemove;
    [SerializeField] private float _removeDelay = 0.5f;

    public void Action(Player CurrentPawn)
    {
        if (!QuestManager.Instance.BucheronQuestAccepted)
        {
            Debug.Log("<color=yellow>[WoodCollector] La quête du Bucheron n'a pas été acceptée</color>");
            return;
        }

        if (!_hasBeenCollected)
        {
            _hasBeenCollected = true;
            QuestManager.Instance.HasWood = true;

            DialogueComponent dialogue = GetComponent<DialogueComponent>();
            if (dialogue != null)
            {
                dialogue.Action(CurrentPawn);
            }
        }
    }

    public void OnDialogueComplete()
    {
        if (_objectToRemove != null)
        {
            StartCoroutine(RemoveObjectAfterDelay());
        }
    }

    private System.Collections.IEnumerator RemoveObjectAfterDelay()
    {
        yield return new WaitForSeconds(_removeDelay);

        if (_objectToRemove != null)
        {
            _objectToRemove.SetActive(false);
            Debug.Log($"<color=green>[WoodCollector] Objet {_objectToRemove.name} supprimé de la map</color>");
        }
    }
}
