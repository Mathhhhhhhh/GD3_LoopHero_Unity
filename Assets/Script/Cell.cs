using UnityEngine;

public class Cell : MonoBehaviour, ICellActivable
{
    public virtual void Activate(Player CurrentPawn)
    { 
        if(GetComponent<IActionable>() != null)
        { 
            GetComponent<IActionable>().Action(CurrentPawn);
        }
    }

}