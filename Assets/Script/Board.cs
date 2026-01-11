using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;

    public void RefreshCells()
    {
        _cells = GetComponentsInChildren<Cell>(true);
        Debug.Log($"Board rafraîchi : {_cells.Length} cellules");
    }

    public Cell GetCellByNumber(int number)
    {
        return _cells[number];
    }

    public int GetNextCellToMove(int cellNumber)
    {
        return cellNumber % _cells.Length;
    }
}
