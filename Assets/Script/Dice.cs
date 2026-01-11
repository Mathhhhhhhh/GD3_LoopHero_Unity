using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Player _player;

    public void RollTheDice()
    {
        int value = 1;
        _player.TryMouving(value);
    }

}
