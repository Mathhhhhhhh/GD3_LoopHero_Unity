using UnityEngine;

public class CellLuciole : MonoBehaviour, IActionable
{
    [Header("Configuration")]
    [SerializeField] private GameObject[] _cellsToReveal;
    [SerializeField] private Board _board;
    [SerializeField] private AudioClip _revealSound;
    [SerializeField] private Material _cellMaterialAfterActivation;

    private bool _hasBeenActivated = false;
    private bool _powerEnabled = false;

    public void Action(Player CurrentPawn)
    {
        if (!_powerEnabled)
        {
            Debug.Log("[CellLuciole] Pouvoir pas encore activé");
            return;
        }

        if (_hasBeenActivated)
        {
            Debug.Log("[CellLuciole] Déjà activée, rien ne se passe");
            return;
        }

        Debug.Log("[CellLuciole] Révélation de nouvelles cases !");
        _hasBeenActivated = true;

        foreach (GameObject cell in _cellsToReveal)
        {
            if (cell != null)
            {
                Debug.Log($"[CellLuciole] Révélation de {cell.name}");
                cell.SetActive(true);
            }
        }

        if (_board != null)
        {
            Debug.Log("[CellLuciole] RefreshCells du Board");
            _board.RefreshCells();
        }

        if (_revealSound != null)
        {
            AudioSource.PlayClipAtPoint(_revealSound, transform.position);
        }

        if (_cellMaterialAfterActivation != null)
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = _cellMaterialAfterActivation;
                Debug.Log("[CellLuciole] Changement de matériau après activation");
            }
        }
    }

    public void EnablePower()
    {
        _powerEnabled = true;
        Debug.Log("[CellLuciole] Pouvoir des lucioles activé !");
    }
}
