using UnityEngine;

public class BucheronQuestTrigger : MonoBehaviour
{
    [Header("House Settings")]
    [SerializeField] private GameObject _house;
    [SerializeField] private Vector3 _houseOffset = new Vector3(3f, 0f, 0f);
    [SerializeField] private float _houseAppearDuration = 2f;

    private bool _houseSpawned = false;

    private void Start()
    {
        if (_house != null)
        {
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
        Vector3 housePosition = transform.position + _houseOffset;
        _house.transform.position = housePosition;
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
