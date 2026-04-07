using UnityEditor;
using UnityEngine;

/// <summary>Génère l'asset de dialogue de remise des carottes au vieux. Supprimer après usage.</summary>
public static class CreateCarrotDialogueAsset
{
    [MenuItem("Tools/Créer VieuxCarrottesDialogue")]
    public static void Create()
    {
        DialogueDatas asset = ScriptableObject.CreateInstance<DialogueDatas>();
        asset.rows = new DialogueRow[]
        {
            new DialogueRow
            {
                rowNumber = 0,
                characterName = "Lulu",
                longDialogue = "Vieux ! Je les ai retrouvées, vos 3 carottes ! Quelqu'un les avait cachées dans un champ au bout du chemin.",
                nextRowNumber = 1
            },
            new DialogueRow
            {
                rowNumber = 1,
                characterName = "Vieux",
                longDialogue = "Mes carottes ! Tu les as vraiment trouvées... Je n'en crois pas mes yeux. Merci, gamin, merci du fond du cœur !",
                nextRowNumber = 2
            },
            new DialogueRow
            {
                rowNumber = 2,
                characterName = "Vieux",
                longDialogue = "Tiens, prends ça. C'est tout ce que j'ai à t'offrir. Tu m'as rendu un fier service aujourd'hui.",
                nextRowNumber = -1
            }
        };

        AssetDatabase.CreateAsset(asset, "Assets/Script/Dialogues/VieuxCarrottesDialogue.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        Debug.Log("[CreateCarrotDialogueAsset] VieuxCarrottesDialogue.asset créé !");
    }
}
