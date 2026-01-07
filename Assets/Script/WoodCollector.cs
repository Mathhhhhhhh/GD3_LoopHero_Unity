using UnityEngine;

public class WoodCollector : MonoBehaviour, IActionable
{
    private bool _hasBeenCollected = false;

    public void Action(Player CurrentPawn)
    {
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
}
