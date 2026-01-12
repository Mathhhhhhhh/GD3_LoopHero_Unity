using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;

    private void Awake()
    {
        RefreshCells();
    }

    public void RefreshCells()
    {
        _cells = GetComponentsInChildren<Cell>(false);
        Debug.Log($"Board rafraîchi : {_cells.Length} cellules actives");
    }

    public Cell GetCellByNumber(int number)
    {
        if (number >= 0 && number < _cells.Length)
        {
            return _cells[number];
        }
        return null;
    }

    public int GetNextCellToMove(int cellNumber)
    {
        return cellNumber % _cells.Length;
    }
}
