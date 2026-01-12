using UnityEngine;

public class CellCameraSwitch : MonoBehaviour, IActionable
{
    [Header("Configuration caméra")]
    [SerializeField] private Camera _cameraToActivate;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private bool _switchOnlyOnce = true;

    private bool _hasSwitched = false;

    public void Action(Player CurrentPawn)
    {
        if (_switchOnlyOnce && _hasSwitched)
        {
            Debug.Log("[CellCameraSwitch] Déjà changé, on ignore");
            return;
        }

        if (_cameraToActivate != null && _mainCamera != null)
        {
            _mainCamera.gameObject.SetActive(false);
            _cameraToActivate.gameObject.SetActive(true);
            _hasSwitched = true;

            Debug.Log($"[CellCameraSwitch] Changement de caméra vers {_cameraToActivate.name}");
        }
        else
        {
            Debug.LogWarning("[CellCameraSwitch] Caméras non assignées!");
        }
    }
}
