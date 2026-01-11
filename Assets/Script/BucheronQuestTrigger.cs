using UnityEngine;

public class BucheronQuestTrigger : MonoBehaviour
{
    [Header("House Settings")]
    [SerializeField] private GameObject _house;
    [SerializeField] private float _houseAppearDuration = 2f;

    private bool _houseSpawned = false;
    private Vector3 _initialHousePosition;
    private Quaternion _initialHouseRotation;

    private void Start()
    {
        if (_house != null)
        {
            _initialHousePosition = _house.transform.position;
            _initialHouseRotation = _house.transform.rotation;
            _house.SetActive(false);
        }
    }

    public void OnQuestComplete()
    {
        if (!_houseSpawned && QuestManager.Instance.HasWood && _house != null)
        {
            SpawnHouse();
        }
    }

    private void SpawnHouse()
    {
        _houseSpawned = true;
        _house.transform.position = _initialHousePosition;
        _house.transform.rotation = _initialHouseRotation;
        _house.SetActive(true);
        StartCoroutine(AnimateHouseAppearance());
    }

    private System.Collections.IEnumerator AnimateHouseAppearance()
    {
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;
        float elapsedTime = 0f;

        _house.transform.localScale = startScale;

        while (elapsedTime < _houseAppearDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _houseAppearDuration;
            _house.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        _house.transform.localScale = targetScale;
    }
}
