using UnityEngine;

public class Cell : MonoBehaviour, ICellActivable
{
    /// <summary>Active toutes les actions associées à cette case.</summary>
    public virtual void Activate(Player CurrentPawn)
    {
        IActionable[] actions = GetComponents<IActionable>();
        foreach (IActionable action in actions)
        {
            action.Action(CurrentPawn);
        }
    }
}
