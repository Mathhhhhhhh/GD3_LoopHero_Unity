using UnityEngine;

[CreateAssetMenu(fileName = "TurnManager", menuName = "Scriptable Objects/TurnManager")]
public class TurnManager : ScriptableObject
{
    [SerializeField] private int _maxTurns = 2;
    [SerializeField] private int _currentTurn = 0;

    public int CurrentTurn => _currentTurn;
    public int MaxTurns => _maxTurns;

    public void ResetTurns()
    {
        _currentTurn = 0;
    }

    public void IncrementTurn()
    {
        _currentTurn++;
        Debug.Log($"<color=cyan>Tour {_currentTurn} / {_maxTurns}</color>");
    }

    public bool HasCompletedAllTurns()
    {
        return _currentTurn >= _maxTurns;
    }

    public void SetMaxTurns(int maxTurns)
    {
        _maxTurns = maxTurns;
    }
}
