using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UiDialogueController : MonoBehaviour
{
    [SerializeField] private DialogueComponent _dialogueComponent;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private GameObject _diceButton;

    [Header("Choice System")]
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _choiceButtonsContainer;
    [SerializeField] private GameObject _choiceButtonPrefab;

    private List<GameObject> _activeChoiceButtons = new List<GameObject>();

    public void StartDialogue(DialogueComponent dialogueComponent)
    {
        Debug.Log($"<color=blue>[UiDialogueController] StartDialogue appelé</color>");
        _dialogueComponent = dialogueComponent;
        UpdateText();
        _dialoguePanel.SetActive(true);
        _diceButton.SetActive(false);
    }

    public void ChangeRow()
    {
        Debug.Log($"<color=blue>[UiDialogueController] ChangeRow (bouton Continue cliqué)</color>");
        _dialogueComponent.GetNextRow();
    }

    public void UpdateText()
    {
        Debug.Log($"<color=red>[UiDialogueController] UpdateText() DÉBUT</color>");

        DialogueRow currentRow = _dialogueComponent.GetDialogueRow();

        Debug.Log($"<color=red>[UiDialogueController] Row reçue de GetDialogueRow() - RowNumber: {currentRow.rowNumber}, Texte : {currentRow.longDialogue}</color>");

        string characterName = _dialogueComponent.GetCharactername();
        string dialogueText = _dialogueComponent.GetDialogueText();

        Debug.Log($"<color=red>[UiDialogueController] GetCharactername(): {characterName}</color>");
        Debug.Log($"<color=red>[UiDialogueController] GetDialogueText(): {dialogueText}</color>");

        _characterNameText.text = characterName;
        _dialogueText.text = dialogueText;

        Debug.Log($"<color=red>[UiDialogueController] Texte affiché dans l'UI: {_dialogueText.text}</color>");

        ClearChoiceButtons();

        if (currentRow.hasChoices && currentRow.choices != null && currentRow.choices.Length > 0)
        {
            ShowChoices(currentRow.choices);
        }
        else
        {
            ShowContinueButton();
        }
    }

    private void ShowChoices(DialogueChoice[] choices)
    {
        if (_continueButton != null)
        {
            _continueButton.SetActive(false);
        }

        if (_choiceButtonsContainer != null)
        {
            _choiceButtonsContainer.SetActive(true);
        }

        foreach (DialogueChoice choice in choices)
        {
            GameObject buttonObj = Instantiate(_choiceButtonPrefab, _choiceButtonsContainer.transform);
            _activeChoiceButtons.Add(buttonObj);

            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = choice.choiceText;
            }

            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                int nextRow = choice.nextRowNumber;
                bool doesNotProgress = choice.doesNotProgress;
                button.onClick.AddListener(() => OnChoiceSelected(nextRow, doesNotProgress));
            }
        }
    }

    private void ShowContinueButton()
    {
        if (_continueButton != null)
        {
            _continueButton.SetActive(true);
        }

        if (_choiceButtonsContainer != null)
        {
            _choiceButtonsContainer.SetActive(false);
        }
    }

    private void OnChoiceSelected(int nextRowNumber, bool doesNotProgress)
    {
        Debug.Log($"<color=blue>[UiDialogueController] OnChoiceSelected - NextRow: {nextRowNumber}, DoesNotProgress: {doesNotProgress}</color>");

        if (doesNotProgress)
        {
            _dialogueComponent.SetShouldIncrementVisit(false);
        }

        _dialogueComponent.GoToRow(nextRowNumber);
    }

    private void ClearChoiceButtons()
    {
        foreach (GameObject button in _activeChoiceButtons)
        {
            Destroy(button);
        }
        _activeChoiceButtons.Clear();
    }

    public void EndDialogue()
    {
        Debug.Log($"<color=blue>[UiDialogueController] EndDialogue appelé</color>");
        ClearChoiceButtons();
        _dialoguePanel.SetActive(false);
        _diceButton.SetActive(true);
    }
}
