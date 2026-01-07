using UnityEngine;

public class LumberjackQuest : DialogueComponent
{
    [Header("House Settings")]
    [SerializeField] private GameObject _house;
    [SerializeField] private Vector3 _houseOffset = new Vector3(3f, 0f, 0f);
    [SerializeField] private float _houseAppearDuration = 2f;

    private int _questVisitCount = 0;
    private bool _houseSpawned = false;
    private bool _confettiPlayed = false;

    private void Start()
    {
        if (_house != null)
        {
            _house.SetActive(false);
        }
    }

    public override void Action(Player CurrentPawn)
    {
        if (_questVisitCount == 0)
        {
            StartQuestDialogue(0);
            _questVisitCount++;
        }
        else if (_questVisitCount == 1 && QuestManager.Instance.HasWood)
        {
            StartQuestDialogue(1);
            _questVisitCount++;

            if (!_houseSpawned && _house != null)
            {
                SpawnHouse();
            }
        }
        else if (_questVisitCount >= 2)
        {
            if (_dialogueDatasArray.Length > 2)
            {
                StartQuestDialogue(2);
            }
        }
        else
        {
            Debug.Log("Vous devez d'abord récupérer le bois !");
        }
    }

    private void StartQuestDialogue(int dialogueIndex)
    {
        if (_dialogueDatasArray != null && dialogueIndex < _dialogueDatasArray.Length)
        {
            _currentDialogueData = _dialogueDatasArray[dialogueIndex];
            _currentRowIndex = 0;
            _currentRow = _currentDialogueData.rows[_currentRowIndex];
            _dialogueController.StartDialogue(this);
        }
    }

    public override void GetNextRow()
    {
        if (_currentRow.nextRowNumber == -1)
        {
            _dialogueController.EndDialogue();

            if (_questVisitCount == 2 && !_confettiPlayed && _confettiEffect != null)
            {
                PlayConfetti();
                _confettiPlayed = true;
            }
        }
        else
        {
            _currentRowIndex = _currentRow.nextRowNumber;
            _currentRow = GetDialogueRow();
            _dialogueController.UpdateText();
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
