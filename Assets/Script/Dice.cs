using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Player _player;

    public void RollTheDice()
    {
        int value = 1;
        Debug.Log($"Le dé a fait {value}");
        _player.TryMouving(value);
    }

}
