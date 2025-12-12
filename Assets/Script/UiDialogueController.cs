using TMPro;
using UnityEngine;

public class UiDialogueController : MonoBehaviour
{
    [SerializeField] private DialogueComponent _dialogueComponent;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] private TMP_Text _dialogueText;
    public void StartDialogue(DialogueComponent dialogueComponent)
    {
        _dialogueComponent = dialogueComponent;
        UpdateText();
        _dialoguePanel.SetActive(true);
    }

    public void ChangeRow()
    {
        _dialogueComponent.GetNextRow();
    }

    public void UpdateText()
    {
        _characterNameText.text = _dialogueComponent.GetCharactername();
        _dialogueText.text = _dialogueComponent.GetDialogueText();
    }

    public void EndDialogue()
    {
        _dialoguePanel.SetActive(false);
    }
}
