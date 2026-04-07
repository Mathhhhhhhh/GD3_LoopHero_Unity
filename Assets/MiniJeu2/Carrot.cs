using UnityEngine;

public class Carrot : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _collectRadius = 1.5f;

    private void Update()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);
        if (distance <= _collectRadius)
        {
            Collect();
        }
    }

    /// <summary>Collecte la carotte : incrémente le compteur et désactive l'objet.</summary>
    private void Collect()
    {
        if (CarrotCounter.Instance != null)
        {
            CarrotCounter.Instance.AddCarrot();
        }

        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _collectRadius);
    }
}
