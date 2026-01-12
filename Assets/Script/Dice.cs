using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private EnergyBar _energyBar;
    [SerializeField] private float _energyCostPerRoll = 10f;

    public void RollTheDice()
    {
        if (_energyBar != null)
        {
            if (!_energyBar.HasEnergy(_energyCostPerRoll))
            {
                Debug.LogWarning("[Dice] Pas assez d'énergie pour lancer le dé!");
                return;
            }

            _energyBar.RemoveEnergy(_energyCostPerRoll);
        }

        int value = Random.Range(1, 1);
        Debug.Log($"Le dé a fait {value}");
        _player.TryMouving(value);
    }
}
