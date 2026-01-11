using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private PlayerDatas _playerData;

    private void Start()
    {
        _playerData.ResetToStart();
        MoveToCell();
    }

    private void MoveToCell()
    {
        Transform NewPos = _board.GetCellByNumber(_playerData._cellNumber).transform;
        transform.position = NewPos.position;
        transform.rotation = NewPos.rotation;
    }

    public void TryMouving(int value)
    {
        if (GameManager.Instance.MovementTimer.IsGameOver())
        {
            Debug.Log("Impossible de bouger : Game Over !");
            return;
        }

        _playerData._cellNumber = _board.GetNextCellToMove(_playerData._cellNumber + value);
        MoveToCell();

        bool canContinue = GameManager.Instance.MovementTimer.DecrementMove();

        if (!canContinue)
        {
            GameManager.Instance.TriggerGameOver();
        }
        else
        {
            ActivateCell();
        }
    }

    public void ActivateCell()
    {
        Cell cell = _board.GetCellByNumber(_playerData._cellNumber);
        cell.Activate(this);
    }
}
