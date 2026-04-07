using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;

    private void Awake()
    {
        // Au retour d'un mini-jeu, réactiver les cases nécessaires avant le RefreshCells
        if (GameInstance.Instance != null && GameInstance.Instance.HasCellsToActivate())
        {
            string[] namesToActivate = GameInstance.Instance.GetAndClearCellsToActivate();
            Cell[] allCells = GetComponentsInChildren<Cell>(true); // true = inclure les inactifs

            foreach (Cell cell in allCells)
            {
                foreach (string cellName in namesToActivate)
                {
                    if (cell.gameObject.name == cellName)
                    {
                        cell.gameObject.SetActive(true);
                        Debug.Log($"[Board] Case réactivée au retour : {cellName}");
                    }
                }
            }
        }

        RefreshCells();
    }

    /// <summary>Rafraîchit la liste des cases actives dans le board.</summary>
    public void RefreshCells()
    {
        _cells = GetComponentsInChildren<Cell>(false);
        Debug.Log($"[Board] Rafraîchi : {_cells.Length} cellules actives");
    }

    public Cell GetCellByNumber(int number)
    {
        if (number >= 0 && number < _cells.Length)
            return _cells[number];

        return null;
    }

    public int GetNextCellToMove(int cellNumber)
    {
        return cellNumber % _cells.Length;
    }

    /// <summary>Retourne l'index dans le board de la case portant ce nom, ou -1 si introuvable.</summary>
    public int GetCellIndexByName(string cellName)
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            if (_cells[i].gameObject.name == cellName)
                return i;
        }

        Debug.LogWarning($"[Board] Case introuvable : {cellName}");
        return -1;
    }
}
