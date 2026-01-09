using UnityEngine;

[CreateAssetMenu(fileName = "MovementTimer", menuName = "Scriptable Objects/MovementTimer")]
public class MovementTimer : ScriptableObject
{
    [SerializeField] private int _maxMoves = 20;
    [SerializeField] private int _remainingMoves;

    public int RemainingMoves => _remainingMoves;
    public int MaxMoves => _maxMoves;

    public void SetMaxMoves(int maxMoves)
    {
        _maxMoves = maxMoves;
    }

    public void ResetTimer()
    {
        _remainingMoves = _maxMoves;
    }

    public bool DecrementMove()
    {
        if (_remainingMoves > 0)
        {
            _remainingMoves--;
            return _remainingMoves > 0;
        }
        return false;
    }

    public void AddMoves(int movesToAdd)
    {
        _remainingMoves += movesToAdd;
        Debug.Log($"+{movesToAdd} mouvements ! Total : {_remainingMoves}");
    }

    public bool IsGameOver()
    {
        return _remainingMoves <= 0;
    }
}
