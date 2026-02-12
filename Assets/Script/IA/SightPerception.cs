using UnityEngine;

public class SightPerception : MonoBehaviour
{
    public bool isDetected = false;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private GameObject detectionObject;
    private Vector3 targetDirection;

    private void Update()
    {
        ActiveDetection();
    }

    private void ActiveDetection()
    {
        targetDirection = detectionObject.transform.position - transform.position;
        if (Vector3.Dot(transform.forward, Vector3.Normalize(targetDirection)) > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, targetDirection, out hit, detectionRadius))
            {
                if (hit.collider.gameObject == detectionObject) //Il faudrait faire un test basé sur un component ou un tag pour l'opti
                {
                    isDetected = true;
                    return;
                }
            }
        }
        isDetected = false;
    }
}
// il faut faire une zone de perte plus grande qu'une zone de detection
// En gros:
// Zone de detection 5 mètres
// Zone de perte 6 mètres, pour éviter de rentrer sortir rentrer sortir de la zone

// Faire en sorte qu'il y ait plusieurs navpoints pour qu'il fasse une ronde


