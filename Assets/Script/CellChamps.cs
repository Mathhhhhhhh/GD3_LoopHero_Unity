using UnityEngine;

public class CellChamps : MonoBehaviour, IActionable
{
    [Header("Configuration")]
    [SerializeField] private DialogueComponent _dialogueToUnlock;
    [SerializeField] private int _lineToUnlock = 1;

    public void Action(Player CurrentPawn)
    {
        if (_dialogueToUnlock != null)
        {
            _dialogueToUnlock.UnlockDialogueLine(_lineToUnlock);
        }
    }
}
