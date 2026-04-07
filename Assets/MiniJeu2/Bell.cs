using UnityEngine;
using UnityEngine.InputSystem;

public class Bell : MonoBehaviour
{
    [SerializeField] private int _bellId;
    [SerializeField] private float _interactionRadius = 2f;
    [SerializeField] private Transform _player;
    [SerializeField] private AudioClip _bellSound;

    private const float SwingAngle = 30f;
    private const float SwingSpeed = 5f;

    private bool _isSwinging = false;
    private float _swingTimer = 0f;
    private Quaternion _initialRotation;

    private void Start()
    {
        _initialRotation = transform.rotation;
    }

    private void Update()
    {
        CheckInteraction();
        HandleSwingAnimation();
    }

    private void CheckInteraction()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);
        bool isInRange = distance <= _interactionRadius;

        if (isInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Activate();
        }
    }

    /// <summary>Active la cloche : déclenche l'animation, joue le son et notifie le BellGameManager.</summary>
    public void Activate()
    {
        if (_isSwinging) return;

        _isSwinging = true;
        _swingTimer = 0f;

        if (_bellSound != null)
            AudioSource.PlayClipAtPoint(_bellSound, transform.position);

        if (BellGameManager.Instance != null)
        {
            BellGameManager.Instance.RegisterBellActivation(_bellId);
        }
        else
        {
            Debug.LogWarning($"[Bell] BellGameManager introuvable — Cloche {_bellId} activée sans enregistrement.");
        }
    }

    private void HandleSwingAnimation()
    {
        if (!_isSwinging) return;

        _swingTimer += Time.deltaTime * SwingSpeed;

        float angle = Mathf.Sin(_swingTimer * Mathf.PI) * SwingAngle * Mathf.Exp(-_swingTimer * 0.5f);
        transform.rotation = _initialRotation * Quaternion.Euler(0f, 0f, angle);

        if (_swingTimer > 3f)
        {
            _isSwinging = false;
            transform.rotation = _initialRotation;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactionRadius);
    }
}
