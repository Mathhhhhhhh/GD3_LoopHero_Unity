using TMPro;
using UnityEngine;

public class UiDialogueController : MonoBehaviour
{
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] private TMP_Text _dialogueText;

    [Header("Bouton de dé")]
    [SerializeField] private GameObject _diceButton;

    private IDialogue _currentDialogue;

    public void StartDialogue(IDialogue dialogue)
    {
        _currentDialogue = dialogue;
        UpdateText();
        _dialoguePanel.SetActive(true);
        HideDiceButton();
    }

    public void ShowWaitingMessage(string message)
    {
        _characterNameText.text = "";
        _dialogueText.text = message;
        _dialoguePanel.SetActive(true);
        HideDiceButton();
    }

    public void ChangeRow()
    {
        _currentDialogue?.GetNextRow();
    }

    public void UpdateText()
    {
        _characterNameText.text = _currentDialogue.GetCharactername();
        _dialogueText.text = _currentDialogue.GetDialogueText();
    }

    public void EndDialogue()
    {
        _dialoguePanel.SetActive(false);
        ShowDiceButton();
    }

    private void HideDiceButton()
    {
        if (_diceButton != null)
            _diceButton.SetActive(false);
    }

    private void ShowDiceButton()
    {
        if (_diceButton != null)
            _diceButton.SetActive(true);
    }
}
