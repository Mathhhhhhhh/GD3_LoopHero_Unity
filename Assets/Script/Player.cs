using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private PlayerDatas _playerData;

    private void Start()
    {
        _board.RefreshCells();

        if (GameInstance.Instance != null && GameInstance.Instance.HasSpawnOverride())
        {
            string cellName = GameInstance.Instance.GetAndClearSpawnCell();
            int index = _board.GetCellIndexByName(cellName);
            if (index >= 0) _playerData._cellNumber = index;
        }

        MoveToCell();
    }

    private void MoveToCell()
    {
        Transform NewPos = _board.GetCellByNumber(_playerData._cellNumber).transform; //TODO : Get cell number
        transform.position = NewPos.position;
        transform.rotation = NewPos.rotation;
    }

    public void TryMouving(int value)
    {
        _playerData._cellNumber = _board.GetNextCellToMove(_playerData._cellNumber+value);
        MoveToCell();
        ActivateCell();
    }
    public void ActivateCell()
    {
        Cell cell = _board.GetCellByNumber(_playerData._cellNumber);
        cell.Activate(this);
    }
}
